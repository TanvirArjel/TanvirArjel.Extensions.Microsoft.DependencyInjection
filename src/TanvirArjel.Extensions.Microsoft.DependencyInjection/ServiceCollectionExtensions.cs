// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel and Thomas Guenther. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Contains all the <see cref="IServiceCollection"/> extension methods for dynamic service registration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static List<Assembly> _loadedAssemblies = new List<Assembly>();

        /// <summary>
        /// This will add all the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes
        /// to the dependency injection container.
        /// Therefor only the list with given assemblies will be used: no directory scanning in this case.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="loadedAssemblies">A list of assembly to be scanned for type attributes.</param>
        public static void AddServicesWithAttributeOfTypeForAssemblies<T>(this IServiceCollection serviceCollection, params Assembly[] loadedAssemblies)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            _loadedAssemblies = loadedAssemblies.OfType<Assembly>().ToList();
            AddServicesWithAttributeOfType<T>(serviceCollection);
        }

        /// <summary>
        /// This will add all the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// Therefor only the list with given assemblies will be used: no directory scanning in this case.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="loadedAssemblies">A list of assembly to be scanned for the given type param.</param>
        public static void AddServicesOfTypeForAssemblies<T>(this IServiceCollection serviceCollection, params Assembly[] loadedAssemblies)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            _loadedAssemblies = loadedAssemblies.OfType<Assembly>().ToList();
            AddServicesOfType<T>(serviceCollection);
        }

        /// <summary>
        /// This will add all the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="scanAssembliesStartsWith">Assembly name starts with any of the provided strings will only be scanned.</param>
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            ServiceLifetime lifetime = ServiceLifetime.Scoped;

            switch (typeof(T).Name)
            {
                case nameof(ITransientService):
                    lifetime = ServiceLifetime.Transient;
                    break;
                case nameof(IScopedService):
                    lifetime = ServiceLifetime.Scoped;
                    break;
                case nameof(ISingletonService):
                    lifetime = ServiceLifetime.Singleton;
                    break;
                default:
                    throw new ArgumentException($"The type {typeof(T).Name} is not a valid type in this context.");
            }

            if (!_loadedAssemblies.Any())
            {
                Console.WriteLine("Scanning of assemblies necessary....");
                LoadAssemblies(scanAssembliesStartsWith);
            }
            else
            {
                Console.WriteLine("List of assemblies already filled....");
            }

            List<Type> implementations = _loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass).ToList();

            foreach (Type implementation in implementations)
            {
                Type[] servicesToBeRegistered = implementation.GetInterfaces()
                    .Where(i => i != typeof(ITransientService) && i != typeof(IScopedService) && i != typeof(ISingletonService)).ToArray();

                if (servicesToBeRegistered.Any())
                {
                    foreach (Type serviceType in servicesToBeRegistered)
                    {
                        bool isGenericTypeDefinition = implementation.IsGenericType && implementation.IsGenericTypeDefinition;
                        Type service = isGenericTypeDefinition
                            && serviceType.IsGenericType
                            && serviceType.IsGenericTypeDefinition == false
                            && serviceType.ContainsGenericParameters
                                ? serviceType.GetGenericTypeDefinition()
                                : serviceType;

                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == service && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(service, implementation, lifetime));
                        }
                    }
                }
                else
                {
                    if (implementation.IsClass)
                    {
                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == implementation && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(implementation, implementation, lifetime));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This will add all the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes
        /// to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="scanAssembliesStartsWith">Assembly name starts with any of the provided strings will only be scanned.</param>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            ServiceLifetime lifetime = ServiceLifetime.Scoped;

            switch (typeof(T).Name)
            {
                case nameof(TransientServiceAttribute):
                    lifetime = ServiceLifetime.Transient;
                    break;
                case nameof(ScopedServiceAttribute):
                    lifetime = ServiceLifetime.Scoped;
                    break;
                case nameof(SingletonServiceAttribute):
                    lifetime = ServiceLifetime.Singleton;
                    break;
                default:
                    throw new ArgumentException($"The type {typeof(T).Name} is not a valid type in this context.");
            }

            if (!_loadedAssemblies.Any())
            {
                Console.WriteLine("Scanning of assemblies necessary....");
                LoadAssemblies(scanAssembliesStartsWith);
            }
            else
            {
                Console.WriteLine("List of assemblies already filled....");
            }

            List<Type> servicesToBeRegistered = _loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsDefined(typeof(T), false)).ToList();

            foreach (Type serviceType in servicesToBeRegistered)
            {
                List<Type> implementations = new List<Type>();

                if (serviceType.IsGenericType && serviceType.IsGenericTypeDefinition)
                {
                    implementations = _loadedAssemblies.SelectMany(a => a.GetTypes())
                    .Where(type => type.IsGenericType && type.IsClass && type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == serviceType.GetGenericTypeDefinition()))
                    .ToList();
                }
                else
                {
                    implementations = _loadedAssemblies.SelectMany(a => a.GetTypes())
                    .Where(type => serviceType.IsAssignableFrom(type) && type.IsClass).ToList();
                }

                if (implementations.Any())
                {
                    foreach (Type implementation in implementations)
                    {
                        bool isGenericTypeDefinition = implementation.IsGenericType && implementation.IsGenericTypeDefinition;
                        Type service = isGenericTypeDefinition
                            && serviceType.IsGenericType
                            && serviceType.IsGenericTypeDefinition == false
                            && serviceType.ContainsGenericParameters
                                  ? serviceType.GetGenericTypeDefinition()
                                  : serviceType;

                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == service && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(service, implementation, lifetime));
                        }
                    }
                }
                else
                {
                    if (serviceType.IsClass)
                    {
                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == serviceType && s.ImplementationType == serviceType);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(serviceType, serviceType, lifetime));
                        }
                    }
                }
            }
        }

        private static void LoadAssemblies(params string[] scanAssembliesStartsWith)
        {
            List<Assembly> loadedAssemblies = new List<Assembly>();

            List<string> assembliesToBeLoaded = new List<string>();

            string appDllsDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (scanAssembliesStartsWith != null && scanAssembliesStartsWith.Any())
            {
                if (scanAssembliesStartsWith.Length == 1)
                {
                    string searchPattern = $"{scanAssembliesStartsWith.First()}*.dll";
                    string[] assemblyPaths = Directory.GetFiles(appDllsDirectory, searchPattern, SearchOption.AllDirectories);

                    assembliesToBeLoaded.AddRange(assemblyPaths);
                }

                if (scanAssembliesStartsWith.Length > 1)
                {
                    foreach (string starsWith in scanAssembliesStartsWith)
                    {
                        string searchPattern = $"{starsWith}*.dll";
                        string[] assemblyPaths = Directory.GetFiles(appDllsDirectory, searchPattern, SearchOption.AllDirectories);
                        assembliesToBeLoaded.AddRange(assemblyPaths);
                    }
                }
            }
            else
            {
                string[] assemblyPaths = Directory.GetFiles(appDllsDirectory, "*.dll", SearchOption.AllDirectories);
                assembliesToBeLoaded.AddRange(assemblyPaths);
            }

            foreach (string path in assembliesToBeLoaded)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(path);
                    loadedAssemblies.Add(assembly);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            _loadedAssemblies = loadedAssemblies;
        }
    }
}
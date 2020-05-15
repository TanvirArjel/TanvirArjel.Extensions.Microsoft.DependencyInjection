// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.ServiceRegistration.Dynamic.Extensions
{
    /// <summary>
    /// Contains all the <see cref="IServiceCollection"/> extension methods for dynamic service registration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static List<Assembly> _loadedAssemblies = new List<Assembly>();

        /// <summary>
        /// This extension method is used to register the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="dllsStartsWithToBeIgnored">Assembly name starts with any of the provided strings will be ignored during type scanning.</param>
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection, params string[] dllsStartsWithToBeIgnored)
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
                LoadAssemblies(dllsStartsWithToBeIgnored);
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
                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == serviceType && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(serviceType, implementation, lifetime));
                        }
                    }
                }
                else
                {
                    bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == implementation && s.ImplementationType == implementation);

                    if (!isAlreadyRegistered)
                    {
                        serviceCollection.Add(new ServiceDescriptor(implementation, implementation, lifetime));
                    }
                }
            }
        }

        /// <summary>
        /// This extension method is used to register the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="dllsStartsWithToBeIgnored">Assembly name starts with any of the provided strings will be ignored during type scanning.</param>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection, params string[] dllsStartsWithToBeIgnored)
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
                LoadAssemblies(dllsStartsWithToBeIgnored);
            }

            List<Type> servicesToBeRegistered = _loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsDefined(typeof(T), false)).ToList();

            foreach (Type serviceType in servicesToBeRegistered)
            {
                List<Type> implementations = _loadedAssemblies.SelectMany(a => a.GetTypes())
                    .Where(type => serviceType.IsAssignableFrom(type) && type.IsClass).ToList();

                if (implementations.Any())
                {
                    foreach (Type implementation in implementations)
                    {
                        bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == serviceType && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered)
                        {
                            serviceCollection.Add(new ServiceDescriptor(serviceType, implementation, lifetime));
                        }
                    }
                }
                else
                {
                    bool isAlreadyRegistered = serviceCollection.Any(s => s.ServiceType == serviceType && s.ImplementationType == serviceType);

                    if (!isAlreadyRegistered)
                    {
                        serviceCollection.Add(new ServiceDescriptor(serviceType, serviceType, lifetime));
                    }
                }
            }
        }

        private static void LoadAssemblies(params string[] dllsStartsWithToBeIgnored)
        {
            List<Assembly> loadedAssemblies = new List<Assembly>();

            string[] assembliesToBeLoaded = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (string path in assembliesToBeLoaded)
            {
                try
                {
                    string dllName = Path.GetFileName(path);
                    bool isToBeIgnored = dllsStartsWithToBeIgnored.Any(dll => dllName.StartsWith(dll, StringComparison.InvariantCultureIgnoreCase));
                    if (!isToBeIgnored)
                    {
                        Assembly assembly = Assembly.LoadFrom(path);
                        loadedAssemblies.Add(assembly);
                    }
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

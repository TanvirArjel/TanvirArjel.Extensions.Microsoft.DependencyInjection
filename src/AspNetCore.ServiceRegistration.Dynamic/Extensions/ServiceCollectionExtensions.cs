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
    public static class ServiceCollectionExtensions
    {
        private static List<Assembly> _loadedAssemblies = new List<Assembly>();

        /// <summary>
        /// This method is used to register the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        [Obsolete("This extension method has been marked as obsolete and will be removed in future versions. Pleae use 'services.AddServicesOfType<T>()' instead.")]
        public static void RegisterAllTypes<T>(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            RegisterAllServices<T>(serviceCollection);
        }

        /// <summary>
        /// This extension method is used to register the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            RegisterAllServices<T>(serviceCollection);
        }

        /// <summary>
        /// This extension method is used to register the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection)
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
                LoadAssemblies();
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

        private static void RegisterAllServices<T>(IServiceCollection serviceCollection)
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
                LoadAssemblies();
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

        private static void LoadAssemblies()
        {
            List<Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList();
            string[] loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            List<string> toLoadAssemblies = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            foreach (string path in toLoadAssemblies)
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

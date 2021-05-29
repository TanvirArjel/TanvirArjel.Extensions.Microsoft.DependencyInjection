// <copyright file="InterfaceBasedServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Contains <see cref="IServiceCollection"/> extension methods to register all the services
    /// implementing <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>.
    /// </summary>
    [Obsolete("This has been marked as obsolete and will be removed in next version.")]
    public static class InterfaceBasedServiceCollectionExtensions
    {
        /// <summary>
        /// This will add all the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="scanAssembliesStartsWith">Assembly name starts with any of the provided strings will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        [Obsolete("This has been marked as obsolete and will be removed in next version. Please use `AddServicesOfAllTypes()` method instead.")]
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = AssemblyHelper.GetLoadedAssemblies(scanAssembliesStartsWith);
            AddServicesOfType<T>(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="assemblyToBeScanned">The <see cref="Assembly"/> will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyToBeScanned"/> is <see langword="null"/>.</exception>
        [Obsolete("This has been marked as obsolete and will be removed in next version. Please use `AddServicesOfAllTypes()` method instead.")]
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection, Assembly assemblyToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (assemblyToBeScanned == null)
            {
                throw new ArgumentNullException(nameof(assemblyToBeScanned));
            }

            List<Assembly> assemblies = new List<Assembly> { assemblyToBeScanned };
            AddServicesOfType<T>(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types implementing any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/> interfaces.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="assembliesToBeScanned">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> which will be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembliesToBeScanned"/> is <see langword="null"/>.</exception>
        [Obsolete("This has been marked as obsolete and will be removed in next version. Please use `AddServicesOfAllTypes()` method instead.")]
        public static void AddServicesOfType<T>(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (assembliesToBeScanned == null)
            {
                throw new ArgumentNullException(nameof(assembliesToBeScanned));
            }

            if (!assembliesToBeScanned.Any())
            {
                throw new ArgumentException($"The {assembliesToBeScanned} is empty.", nameof(assembliesToBeScanned));
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

            List<Type> implementations = assembliesToBeScanned
                .SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(T).IsAssignableFrom(type) && type != typeof(T)).ToList();

            List<Type> implementationClasses = implementations.Where(type => type.IsClass).ToList();
            List<Type> implementationInterfaces = implementations.Where(type => type.IsInterface).ToList();

            foreach (Type implementation in implementationClasses)
            {
                Type[] servicesToBeRegistered = implementation.GetInterfaces()
                    .Where(i => implementationInterfaces.Select(ii => ii.Name).Contains(i.Name)).ToArray();

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
    }
}

// <copyright file="AttributeBasedServiceCollectionExtensions.cs" company="TanvirArjel">
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
    /// Contains <see cref="IServiceCollection"/> extension methods to register all the services containing
    /// <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.
    /// </summary>
    public static class AttributeBasedServiceCollectionExtensions
    {
        /// <summary>
        /// Add all the services containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/>
        /// and <see cref="SingletonServiceAttribute"/> attributes to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="scanAssembliesStartsWith">Assembly name starts with any of the provided strings will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = AssemblyHelper.GetLoadedAssemblies(scanAssembliesStartsWith);
            AddServicesWithAttributeOfType<T>(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes
        /// to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="assemblyToBeScanned">The <see cref="Assembly"/> will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyToBeScanned"/> is <see langword="null"/>.</exception>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection, Assembly assemblyToBeScanned)
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
            AddServicesWithAttributeOfType<T>(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes
        /// to the dependency injection container.
        /// </summary>
        /// <typeparam name="T">Any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes.</typeparam>
        /// <param name="serviceCollection">Type to be extended.</param>
        /// <param name="assembliesToBeScanned">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> which will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembliesToBeScanned"/> is <see langword="null"/>.</exception>
        public static void AddServicesWithAttributeOfType<T>(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToBeScanned)
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

            // Get all the types which contains T attribute but not contain [IgnoreServiceRegistration] attribute
            List<Type> servicesToBeRegistered = assembliesToBeScanned
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDefined(typeof(T), false) && !type.IsDefined(typeof(IgnoreServiceRegistrationAttribute), false)).ToList();

            foreach (Type serviceType in servicesToBeRegistered)
            {
                List<Type> implementations = new List<Type>();

                if (serviceType.IsGenericType && serviceType.IsGenericTypeDefinition)
                {
                    // Get all implementations of serviceType that does not contains [IgnoreServiceRegistration] attribute
                    implementations = assembliesToBeScanned.SelectMany(a => a.GetTypes())
                    .Where(type => !type.IsDefined(typeof(IgnoreServiceRegistrationAttribute), false) && type.IsGenericType && type.IsClass && type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == serviceType.GetGenericTypeDefinition()))
                    .ToList();
                }
                else
                {
                    // Get all implementations of serviceType that does not contains [IgnoreServiceRegistration] attribute
                    implementations = assembliesToBeScanned.SelectMany(a => a.GetTypes())
                    .Where(type => !type.IsDefined(typeof(IgnoreServiceRegistrationAttribute), false) && serviceType.IsAssignableFrom(type) && type.IsClass)
                    .ToList();
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
    }
}

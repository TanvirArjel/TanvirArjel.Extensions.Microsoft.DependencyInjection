﻿// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Contains all the <see cref="IServiceCollection"/> extension methods for dynamic service registration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// <para>
        /// This will add all the types implementing <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </para>
        /// <para>
        /// This will add all the types containing any of the <see cref="ScopedServiceAttribute"/>, <see cref="TransientServiceAttribute"/> and <see cref="SingletonServiceAttribute"/> attributes
        /// to the dependency injection container.
        /// </para>
        /// </summary>
        /// <param name="serviceCollection">The type that has been extended.</param>
        /// <param name="scanAssembliesStartsWith">Assembly name starts with any of the provided strings will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        public static void AddServicesOfAllTypes(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = AssemblyHelper.GetLoadedAssemblies(scanAssembliesStartsWith);
            AddServicesOfAllTypes(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types implementing <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <param name="serviceCollection">The type that has been extended.</param>
        /// <param name="assemblyToBeScanned">The <see cref="Assembly"/> will only be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyToBeScanned"/> is <see langword="null"/>.</exception>
        public static void AddServicesOfAllTypes(this IServiceCollection serviceCollection, Assembly assemblyToBeScanned)
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
            AddServicesOfAllTypes(serviceCollection, assemblies);
        }

        /// <summary>
        /// This will add all the types implementing <see cref="IScopedService"/>, <see cref="ITransientService"/> and <see cref="ISingletonService"/>
        /// interfaces to the dependency injection container.
        /// </summary>
        /// <param name="serviceCollection">The type that has been extended.</param>
        /// <param name="assembliesToBeScanned">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> which will be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembliesToBeScanned"/> is <see langword="null"/>.</exception>
        public static void AddServicesOfAllTypes(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (assembliesToBeScanned == null)
            {
                throw new ArgumentNullException(nameof(assembliesToBeScanned));
            }

            serviceCollection.AddServicesOfType<ITransientService>(assembliesToBeScanned);
            serviceCollection.AddServicesOfType<IScopedService>(assembliesToBeScanned);
            serviceCollection.AddServicesOfType<ISingletonService>(assembliesToBeScanned);

            serviceCollection.AddServicesWithAttributeOfType<TransientServiceAttribute>(assembliesToBeScanned);
            serviceCollection.AddServicesWithAttributeOfType<ScopedServiceAttribute>(assembliesToBeScanned);
            serviceCollection.AddServicesWithAttributeOfType<SingletonServiceAttribute>(assembliesToBeScanned);
        }
    }
}

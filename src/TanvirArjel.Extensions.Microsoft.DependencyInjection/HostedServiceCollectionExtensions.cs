// <copyright file="HostedServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Contains <see cref="IServiceCollection"/> extension methods to register all the <see cref="IHostedService"/>
    /// containing <see cref="HostedServiceAttribute"/> attribute at once.
    /// </summary>
    public static class HostedServiceCollectionExtensions
    {
        /// <summary>
        /// Add all the <see cref="IHostedService"/> containing <see cref="HostedServiceAttribute"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The type to be extended.</param>
        /// <param name="scanAssembliesStartsWith">Assemblies to be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        public static void AddHostedServices(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = AssemblyHelper.GetLoadedAssemblies(scanAssembliesStartsWith);
            AddHostedServices(serviceCollection, assemblies);
        }

        /// <summary>
        /// Add all the <see cref="IHostedService"/> containing <see cref="HostedServiceAttribute"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The type to be extended.</param>
        /// <param name="assemblyToBeScanned"><see cref="Assembly"/> to be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        public static void AddHostedServices(this IServiceCollection serviceCollection, Assembly assemblyToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = new List<Assembly> { assemblyToBeScanned };
            AddHostedServices(serviceCollection, assemblies);
        }

        /// <summary>
        /// Add all the <see cref="IHostedService"/> containing <see cref="HostedServiceAttribute"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The type to be extended.</param>
        /// <param name="assembliesToBeScanned">Assemblies to be scanned.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceCollection"/> is <see langword="null"/>.</exception>
        public static void AddHostedServices(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Type> hostedServices = assembliesToBeScanned
                .SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsDefined(typeof(HostedServiceAttribute), false)).ToList();

            foreach (Type hostedService in hostedServices)
            {
                serviceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(IHostedService), hostedService, ServiceLifetime.Singleton));
            }
        }
    }
}

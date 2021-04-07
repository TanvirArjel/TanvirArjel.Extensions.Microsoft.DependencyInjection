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
    /// Contains all the <see cref="IServiceCollection"/> extension methods for dynamic service registration.
    /// </summary>
    public static class HostedServiceCollectionExtensions
    {
        public static void AddHostedServices(this IServiceCollection serviceCollection, params string[] scanAssembliesStartsWith)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = AssemblyHelper.GetLoadedAssemblies(scanAssembliesStartsWith);
            AddHostedServices(serviceCollection, assemblies);
        }

        public static void AddHostedServices(this IServiceCollection serviceCollection, Assembly assemblyToBeScanned)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            List<Assembly> assemblies = new List<Assembly> { assemblyToBeScanned };
            AddHostedServices(serviceCollection, assemblies);
        }

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

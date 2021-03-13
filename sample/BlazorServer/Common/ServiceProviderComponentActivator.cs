using System;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Common
{
    public class ServiceProviderComponentActivator : IComponentActivator
    {
        public ServiceProviderComponentActivator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public IComponent CreateInstance(Type componentType)
        {
            if (componentType == null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }

            object instance = ServiceProvider.GetService(componentType);

            if (instance == null)
            {
                instance = Activator.CreateInstance(componentType);
            }

            if (instance is not IComponent component)
            {
                throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
            }

            return component;
        }
    }
}

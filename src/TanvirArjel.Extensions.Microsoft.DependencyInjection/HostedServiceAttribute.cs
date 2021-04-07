// <copyright file="HostedServiceAttribute.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// The <see cref="IHostedService"/> containing this <see cref="Attribute"/> will automatically be registered in <see cref="IServiceCollection"/>
    /// with <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HostedServiceAttribute : Attribute
    {
    }
}

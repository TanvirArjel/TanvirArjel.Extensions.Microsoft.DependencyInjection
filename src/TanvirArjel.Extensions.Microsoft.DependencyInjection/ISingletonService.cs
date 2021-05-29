// <copyright file="ISingletonService.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// The services implemented this interface will automatically be registered in <see cref="IServiceCollection"/>
    /// with <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    [Obsolete("This has been marked as obsolete and will be removed in the next version. Please use [SingletonService] attribute instead.")]
    public interface ISingletonService
    {
        // This is a marker interface
    }
}

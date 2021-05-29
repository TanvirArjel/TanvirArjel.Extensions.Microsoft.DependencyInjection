// <copyright file="IScopedService.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// The services implemented this interface will automatically be registered in <see cref="IServiceCollection"/>
    /// with <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    [Obsolete("This has been marked as obsolete and will be removed in the next version. Please use [ScopedService] attribute instead.")]
    public interface IScopedService
    {
        // This is a marker interface
    }
}

// <copyright file="IScopedService.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

namespace AspNetCore.ServiceRegistration.Dynamic.Interfaces
{
    /// <summary>
    /// The services implemented this interface will automatically be registered with scoped life time in
    /// ASP.NET Core Dependency Injection Container.
    /// </summary>
    public interface IScopedService
    {
        // This is a marker interface
    }
}

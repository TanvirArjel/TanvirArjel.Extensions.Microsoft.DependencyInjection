// <copyright file="ITransientService.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

namespace AspNetCore.DependencyInjection.ServiceRegistration.Interfaces
{
    /// <summary>
    /// The services implemented this interface will automatically be registered with transient life time in
    /// ASP.NET Core Dependency Injection Container.
    /// </summary>
    public interface ITransientService
    {
        // This is a marker interface
    }
}

// <copyright file="ScopedServiceAttribute.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;

namespace AspNetCore.ServiceRegistration.Dynamic
{
    /// <summary>
    /// The services containing this <c>Attribute</c> will automatically be registered with scoped life time in
    /// ASP.NET Core Dependency Injection Container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ScopedServiceAttribute : Attribute
    {
    }
}

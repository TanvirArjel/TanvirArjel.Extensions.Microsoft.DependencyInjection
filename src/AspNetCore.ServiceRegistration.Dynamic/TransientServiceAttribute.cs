// <copyright file="TransientServiceAttribute.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;

namespace AspNetCore.ServiceRegistration.Dynamic
{
    /// <summary>
    /// The services containing this <c>Attribute</c> will automatically be registered with transient life time in
    /// ASP.NET Core Dependency Injection Container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TransientServiceAttribute : Attribute
    {
    }
}

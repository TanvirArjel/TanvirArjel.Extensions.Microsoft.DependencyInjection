// <copyright file="IgnoreServiceRegistrationAttribute.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// The services containing this <c>Attribute</c> will be ignored during service registration in <see cref="IServiceCollection"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreServiceRegistrationAttribute : Attribute
    {
    }
}

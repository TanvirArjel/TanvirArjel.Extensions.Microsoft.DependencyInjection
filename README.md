 # 👑 .NET/.NET Core Dynamic Service Registration 👑

This is a NET 5.0 and .NET Core dynamic service registration library which enables you to register all your services into .NET 5.0 and .NET Core Dependency Injection container at once without exposing the service implementation.
 
 ## ⭐ Give a star ⭐
   
   **If you find this library useful to you, please don't forget to encouraging me to do such more stuffs by giving a star (⭐) to this repository. Thank you.**

## ❤️ What's new in version 1.0.0 ❤️

1. This is the initial relase of this library which was previously named as `AspNetCore.ServiceRegistration.Dynamic` [Branch Link](https://github.com/TanvirArjel/TanvirArjel.Extensions.Microsoft.DependencyInjection/tree/AspNetCore.ServiceRegistration.Dynamic)

## ✈️ How do I get started? ✈️

First install the lastest version of `
TanvirArjel.Extensions.Microsoft.DependencyInjection` [nuget package](https://www.nuget.org/packages/TanvirArjel.Extensions.Microsoft.DependencyInjection) into your project as follows:
 
    Install-Package TanvirArjel.Extensions.Microsoft.DependencyInjection
    
Now in your `ConfigureServices` method of the `Startup` class:

```C#
public static void ConfigureServices(IServiceCollection services)
{
    services.AddServicesOfType<IScopedService>();
    services.AddServicesWithAttributeOfType<ScopedServiceAttribute>();
}
```
    
 Moreover, if you want only specific assemblies to be scanned during type scanning:

```C#
public static void ConfigureServices(IServiceCollection services)
{
    // Assemblies start with "TanvirArjel.Web", "TanvirArjel.Application" will only be scanned.
    string[] assembliesToBeScanned = new string[] { "TanvirArjel.Web", "TanvirArjel.Application" };
    services.AddServicesOfType<IScopedService>(assembliesToBeScanned);
    services.AddServicesWithAttributeOfType<ScopedServiceAttribute>(assembliesToBeScanned);
}
```
    
## 🛠️ Usage: Marker Interface: 🛠️

Now let your services to inherit any of the `ITransientService`, `IScoperService` and `ISingletonService` marker interfaces as follows:

```C#
// Inherit `IScopedService` interface if you want to register `IEmployeeService` as scoped service.
public interface IEmployeeService : IScopedService
{
    Task CreateEmployeeAsync(Employee employee);
}

internal class EmployeeService : IEmployeeService 
{
    public async Task CreateEmployeeAsync(Employee employee)
    {
        // Implementation here
    };
}
```
        
## 🛠️ Usage: Attribute: 🛠️

Now mark your services with any of the `ScopedServiceAttribute`, `TransientServiceAttribute` and `SingletonServiceAttribute` attributes as follows:

```C#
// Mark with ScopedServiceAttribute if you want to register `IEmployeeService` as scoped service.
[ScopedService]
public interface IEmployeeService
{
    Task CreateEmployeeAsync(Employee employee);
}

internal class EmployeeService : IEmployeeService 
{
    public async Task CreateEmployeeAsync(Employee employee)
    {
       // Implementation here
    };
}
```
  
## 🐞 Bug Report 🐞
   
   Dont forget to submit an issue if you face. we will try to resolve as soon as possible.

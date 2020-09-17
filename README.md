# ASP.NET Core Dynammic Service Registration

This is a ASP.NET Core dynamic service registration library which enables you to register all your services into ASP.NET Core Dependency Injection container at once without exposing the service implementation.

## How do I get started?

First install the lastest version of `AspNetCore.ServiceRegistration.Dynamic` [nuget package](https://www.nuget.org/packages/AspNetCore.ServiceRegistration.Dynamic) into your project as follows:
 
    Install-Package AspNetCore.ServiceRegistration.Dynamic
    
### Using Marker Interface:

Now let your services to inherit any of the `ITransientService`, `IScoperService` and `ISingletonService` marker interfaces as follows:
 
    // Inherit `IScopedService` interface if you want to register `IEmployeeService` as scoped service.
    public class IEmployeeService : IScopedService
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
        
 ### Using Attribute:

Now mark your services with any of the `ScopedServiceAttribute`, `TransientServiceAttribute` and `SingletonServiceAttribute` attributes as follows:
 
    // Mark with ScopedServiceAttribute if you want to register `IEmployeeService` as scoped service.
    [ScopedService]
    public class IEmployeeService
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
        
  Now in your `ConfigureServices` method of the `Startup` class:
  
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesOfType<IScopedService>();
        services.AddServicesWithAttributeOfType<ScopedServiceAttribute>();
    }
    
  Moreover, if you want any assembly to be ignored during type scanning:
  
    public static void ConfigureServices(IServiceCollection services)
    {
        // Assemblies start with "Microsoft.AspNetCore", "Microsoft.Extensions" will be ignored during type scanning.
        string[] assembliesToBeIngored = new string[] { "Microsoft.AspNetCore", "Microsoft.Extensions" };
        services.AddServicesOfType<IScopedService>(assembliesToBeIngored);
        services.AddServicesWithAttributeOfType<ScopedServiceAttribute>(assembliesToBeIngored);
    }
  
       
  ### That's it!!
  
  # Note
   
   Dont forget to submit an issue if you face. we will try to resolve as soon as possible.
   
  # Request
   
   **If you find this library useful to you, please don't forget to encouraging me to do such more stuffs by giving a star to this repository. Thank you.**

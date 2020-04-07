# ASP.NET Core Dynammic Service Registration

This is a ASP.NET Core dynamic service registration library which enables you to register all your services into ASP.NET Core Dependency Injection container at once without exposing the service implementation.

## How do I get started?

Configuring **AspNetCore.DependencyInjection.ServiceRegistration** into your ASP.NET Core project is simple as below:

 1. First install the lastest version of `AspNetCore.DependencyInjection.ServiceRegistration` [nuget package](https://www.nuget.org/packages/AspNetCore.CustomValidation) into your project as follows:
 
    `Install-Package AspNetCore.DependencyInjection.ServiceRegistration`
    
 Now let your services to inherit any of the `ITransientService`, `IScoperService` and `ISingletonService` marker interfaces as follows:
 
        public class IEmployeeService : IScopedService // Inherit `IScopedService` interface if you want to register `IEmployeeService` as scoped service.
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
            services.RegisterAllTypes<IScopedService>(); // This will register all the Scoped services of your application.
            services.RegisterAllTypes<ITransientService>(); // This will register all the Transient services of your application.
            services.RegisterAllTypes<ISingletonService>(); // This will register all the Singleton services of your application.
            services.AddControllersWithViews();
       }
       
  ### That's it!!
  
  # Note
   
   Dont forget to submit an issue if you face. we will try to resolve as soon as possible.
   
  # Request
   
   If you find this library useful to you, please don't forget to encouraging us to do such more stuffs by giving a star to this repository. Thank you.

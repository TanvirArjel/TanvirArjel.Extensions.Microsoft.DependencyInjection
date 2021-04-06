using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BlazorWasm.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.Replace(ServiceDescriptor.Transient<IComponentActivator, ServiceProviderComponentActivator>());

            builder.Services.AddServicesOfAllTypes(new[] { Assembly.GetExecutingAssembly() });

            await builder.Build().RunAsync();
        }
    }
}

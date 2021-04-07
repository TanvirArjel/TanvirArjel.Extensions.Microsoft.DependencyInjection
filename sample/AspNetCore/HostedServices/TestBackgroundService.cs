using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.HostedServices
{
    [HostedService]
    public class TestBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Hosted service called.");
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}

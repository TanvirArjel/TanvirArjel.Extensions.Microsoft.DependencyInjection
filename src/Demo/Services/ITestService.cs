using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.DependencyInjection.ServiceRegistration.Interfaces;

namespace Demo.Services
{
    public interface ITestService : IScopedService
    {
        string GetMyName();
    }
}

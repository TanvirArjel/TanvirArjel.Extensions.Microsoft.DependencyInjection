using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using AspNetCore.ServiceRegistration.Dynamic.Interfaces;

namespace Demo.Services
{
    [ScopedService]
    public interface ITestService : IScopedService
    {
        string GetMyName();
    }
}

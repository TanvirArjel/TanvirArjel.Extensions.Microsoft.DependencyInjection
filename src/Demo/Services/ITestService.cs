using AspNetCore.ServiceRegistration.Dynamic;

namespace Demo.Services
{
    [ScopedService]
    public interface ITestService : IScopedService
    {
        string GetMyName();
    }
}

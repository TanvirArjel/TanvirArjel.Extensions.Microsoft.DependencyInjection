using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Services
{
    [ScopedService]
    public interface ITestService
    {
        string GetMyName();
    }
}

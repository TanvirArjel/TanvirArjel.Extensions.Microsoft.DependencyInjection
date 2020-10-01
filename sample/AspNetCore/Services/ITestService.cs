using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Services
{
    ////[ScopedService]
    public interface ITestService : IScopedService
    {
        string GetMyName();
    }
}

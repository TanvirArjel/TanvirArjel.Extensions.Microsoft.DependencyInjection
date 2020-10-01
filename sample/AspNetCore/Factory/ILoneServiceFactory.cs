using AspNetCore.Services;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Factory
{
    [ScopedService]
    public interface ILoneServiceFactory
    {
        ILoneService GetLoneSerive(string implementationName);
    }
}

using AspNetCore.ServiceRegistration.Dynamic;
using Demo.Services;

namespace Demo.Factory
{
    [ScopedService]
    public interface ILoneServiceFactory
    {
        ILoneService GetLoneSerive(string implementationName);
    }
}

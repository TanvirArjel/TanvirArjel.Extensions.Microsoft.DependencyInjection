using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Demo.Services;

namespace Demo.Factory
{
    [ScopedService]
    public interface ILoneServiceFactory
    {
        ILoneService GetLoneSerive(string implementationName);
    }
}

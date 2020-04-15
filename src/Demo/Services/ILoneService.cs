using AspNetCore.ServiceRegistration.Dynamic.Interfaces;

namespace Demo.Services
{
    public interface ILoneService : IScopedService
    {
        string GetString();
    }
}

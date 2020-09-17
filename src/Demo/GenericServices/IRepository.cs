using AspNetCore.ServiceRegistration.Dynamic;

namespace Demo.GenericServices
{
    public interface IRepository<T> : IScopedService
        where T : class
    {
        string Welcome(string name);
    }
}

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.GenericServices
{
    [ScopedService]
    public interface IRepository<T> : IScopedService
        where T : class
    {
        string Welcome(string name);
    }
}

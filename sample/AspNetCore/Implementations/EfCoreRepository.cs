using AspNetCore.GenericServices;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Implementations
{
    ////[IgnoreServiceRegistration]
    public class EfCoreRepository<T> : IRepository<T>
        where T : class
    {
        public string Welcome(string name)
        {
            return $"Welcome! {name}";
        }
    }
}

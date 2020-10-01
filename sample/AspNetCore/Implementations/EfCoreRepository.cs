using AspNetCore.GenericServices;

namespace AspNetCore.Implementations
{
    public class EfCoreRepository<T> : IRepository<T>
        where T : class
    {
        public string Welcome(string name)
        {
            return $"Welcome! {name}";
        }
    }
}

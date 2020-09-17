using Demo.GenericServices;

namespace Demo.Implementations
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

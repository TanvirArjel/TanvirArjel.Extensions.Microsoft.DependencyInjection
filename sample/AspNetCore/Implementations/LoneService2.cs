using AspNetCore.Services;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Implementations
{
    [IgnoreServiceRegistration]
    public class LoneService2 : ILoneService
    {
        public string GetString()
        {
            return "Message from lone service 2";
        }
    }
}

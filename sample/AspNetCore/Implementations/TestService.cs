using AspNetCore.Services;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore.Implementations
{
    public class TestService : ITestService
    {
        public string GetMyName()
        {
            return "Tanvir Ahmad Arjel";
        }
    }
}

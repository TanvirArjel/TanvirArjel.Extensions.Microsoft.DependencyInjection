using AspNetCore.Services;

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

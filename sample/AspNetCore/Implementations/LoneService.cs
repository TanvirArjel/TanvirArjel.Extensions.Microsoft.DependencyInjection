using AspNetCore.Services;

namespace AspNetCore.Implementations
{
    public class LoneService : ILoneService
    {
        public string GetString()
        {
            return "Hello! I am tanvir";
        }
    }
}

using Demo.Services;

namespace Demo.Implementations
{
    public class LoneService2 : ILoneService
    {
        public string GetString()
        {
            return "Message from lone service 2";
        }
    }
}

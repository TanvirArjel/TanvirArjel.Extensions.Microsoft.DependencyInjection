using System;
using Demo.Implementations;
using Demo.Services;

namespace Demo.Factory
{
    public class LoneServiceFactory : ILoneServiceFactory
    {
        public ILoneService GetLoneSerive(string implementationName)
        {
            if (implementationName == "1")
            {
                return new LoneService();
            }
            else if (implementationName == "2")
            {
                return new LoneService2();
            }
            else
            {
                throw new ArgumentException("No such type implementation");
            }
        }
    }
}

using System;
using AspNetCore.Implementations;
using AspNetCore.Services;

namespace AspNetCore.Factory
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

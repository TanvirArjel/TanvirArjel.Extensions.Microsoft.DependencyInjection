using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class TestService : ITestService
    {
        public string GetMyName()
        {
            return "Tanvir Ahmad Arjel";
        }
    }
}

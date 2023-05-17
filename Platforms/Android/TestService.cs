using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewfinder.Services;

namespace Viewfinder.Platforms

{
    public class TestService : ITestService
    {
        public string GetTestString()
        {
            return "This is a test string from the TestService.";
        }
    }
}

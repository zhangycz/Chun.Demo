using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chun.Demo.TestHelper;

namespace Chun.Demo.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var testThread = new TestThread();
            testThread.TestMain();
            Console.Read();
        }
    }
}

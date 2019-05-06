using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.TestHelper;
using Chun.Work.Common.Helper;

namespace Chun.Demo.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //var testThread = new TestThread();
            //testThread.TestMain();

           var testList =new List<string>(){
                "1",  "11",
                "2",  "12",
                "3",  "13",
                "21", "22", "23",
                "31", "32","33"
           };
           var testList1 =new List<string>(){
                "a",  "b",
                "c"
           };
           Parallel.ForEach(testList, (t) => {
               Thread.Sleep(1000);
               LogHelper.Fatal($@"{t}");
           });
           Parallel.ForEach(testList1, (t) => {
                 Thread.Sleep(1000);
                LogHelper.Fatal($@"{t}");
           });

           LogHelper.Debug("main end");
            Console.Read();


        }
    }
}

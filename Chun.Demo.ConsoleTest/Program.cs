using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common.Tool;
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
            //var address = @"http://img1.mm131.me/pic/4864/51.jpg";
            //var bAddress = @"C:\Users\a2863\Desktop\51.jpg";
            //Existed(address);
           
            Console.Read();


        }

        /// <summary>
        ///     检查文件是否存在
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Existed(string address)
        {
            var existed = false;

           
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "HEAD";
                //request.Referer = @"http://www.mm131.com/xinggan";
                long size;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    size = response.ContentLength;
                    LogHelper.Debug($"文件 {address} 长度 {size} ");
                }
              
            }
            catch (WebException e)
            {
                LogHelper.Debug($"校验文件大小时异常");
                LogHelper.Error(e);
                return false;
            }

            return existed;
        }
    }
}

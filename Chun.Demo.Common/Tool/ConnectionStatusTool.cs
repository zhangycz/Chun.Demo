using System;
using System.Net.NetworkInformation;

namespace Chun.Demo.Common
{
     public class ConnectionStatusTool
    {
        /// <summary>
        /// 检测网络连接状态
        /// </summary>
        /// <param name="urls"></param>
        public static String CheckServeStatus(string urls)
        {
            String responseStatus = "200";

            if (!LocalConnectionStatus())
            {
                responseStatus = "400";
                Console.WriteLine("网络异常~无连接");
            }
            else if (!MyPing(urls))
            {
               responseStatus = "404";
               Console.WriteLine("网络异常~连接多次无响应");
            }
            else
            {
                responseStatus = "200";
                Console.WriteLine("网络正常");
            }

            return responseStatus;
        }

        #region 网络检测

        private const int InternetConnectionModem = 1;
        private const int InternetConnectionLan = 2;

        [System.Runtime.InteropServices.DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        /// <summary>
        /// 判断本地的连接状态
        /// </summary>
        /// <returns></returns>
        private static bool LocalConnectionStatus()
        {
            System.Int32 dwFlag = new Int32();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                Console.WriteLine("LocalConnectionStatus--未连网!");
                return false;
            }
            else
            {
                if ((dwFlag & InternetConnectionModem) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                    return true;
                }
                else if ((dwFlag & InternetConnectionLan) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                    return true;
                }
            }
            return false;
        }

         /// <summary>
         /// Ping命令检测网络是否畅通
         /// </summary>
         /// <param name="urls">URL数据</param>
         /// <returns></returns>
         public static bool MyPing(string urls)
        {
            bool isconn = true;
            Ping ping = new Ping();
            
            try
            {
                var pr = ping.Send(urls);
                    if (pr != null && pr.Status != IPStatus.Success)
                    {
                        isconn = false;
                    }
                if (pr != null) Console.WriteLine("Ping " + urls + "    " + pr.Status.ToString());
            }
            catch(PingException ex)
            {
                Console.WriteLine(ex.Message+Environment.NewLine+ex.Data);
                isconn = false;
            }
            return isconn;
        }

        #endregion
    }


}

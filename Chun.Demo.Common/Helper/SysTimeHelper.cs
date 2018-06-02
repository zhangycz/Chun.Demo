/**************Code Info**************************
* 文 件 名： SysTimeHelper
* 创 建 人： Zhengp
* 创建日期：2012/8/12 14:53:00
* 修 改 人：
* 修改日期：
* 备注描述：更新系统时间
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chun.Demo.Common
{
    /// </summary>   
    public class SysTimeHelper
    {
        SysTimeHelper()
        {

        }
        #region   ComputerTime
        [StructLayout(LayoutKind.Sequential)]
        private struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMiliseconds;
        }
        private class Win32
        {
            [DllImport("Kernel32.dll ")]
            public static extern bool SetSystemTime(ref   SystemTime SysTime);
            [DllImport("Kernel32.dll ")]
            public static extern void GetSystemTime(ref   SystemTime SysTime);
        }
        #endregion

        #region   时间同步
        ///   <summary> 
        ///   设置与服务器同步时间   
        ///   </summary> 
        public static void SynchronousTime(DateTime serverTime)
        {
            try
            {
                #region   更改计算机时间

                var sysTime = new SystemTime();

                var ServerTime = serverTime;

                sysTime.wYear = Convert.ToUInt16(ServerTime.Year);

                sysTime.wMonth = Convert.ToUInt16(ServerTime.Month);

                //处置北京时间   

                int nBeijingHour = ServerTime.Hour - 8;

                if (nBeijingHour <= 0)
                {
                    nBeijingHour += 24;

                    sysTime.wDay = Convert.ToUInt16(ServerTime.Day - 1);

                    sysTime.wDayOfWeek = Convert.ToUInt16(ServerTime.DayOfWeek - 1);
                }
                else
                {
                    sysTime.wDay = Convert.ToUInt16(ServerTime.Day);

                    sysTime.wDayOfWeek = Convert.ToUInt16(ServerTime.DayOfWeek);
                }

                sysTime.wHour = Convert.ToUInt16(nBeijingHour);

                sysTime.wMinute = Convert.ToUInt16(ServerTime.Minute);

                sysTime.wSecond = Convert.ToUInt16(ServerTime.Second);

                sysTime.wMiliseconds = Convert.ToUInt16(ServerTime.Millisecond);

                Win32.SetSystemTime(ref   sysTime);

                #endregion
            }
            catch (Exception ex)
            {
                DebugHelper.Out(ex.ToString());
            }
        }
        #endregion
    }
}

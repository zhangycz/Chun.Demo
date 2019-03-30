using Chun.Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainFrom
{
    static class Program
    {
        /// <summary>
        /// 用于检测启动程序
        /// </summary>
        private static Mutex __instanceMutex = null;

        /// <summary>
        /// 处理异常
        /// </summary>
        private static DbgHelper.UnhandledExceptionFilter exceptionCallback = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
              
                LogHelper.Debug($"Client Startup! Version = {Application.ProductVersion}");

                //互斥量，启动一个
                bool flag;

                __instanceMutex = new Mutex(true, "Pactera.CTIClient", out flag);

                if (!flag)
                {
                    LogHelper.Debug("Client is already running.");
                    return;
                }
                else
                {
                    exceptionCallback = new DbgHelper.UnhandledExceptionFilter(UnhandledExceptionFilter);

                    GC.KeepAlive(exceptionCallback);

                    DbgHelper.SetUnhandledExceptionFilter(exceptionCallback);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Application.Run(new MainFrom.MainForm());
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error("Client startup failed. {0}", ex);

                MessageBox.Show("启动客户端失败. " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                LogHelper.Flush();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.Error("Application_ThreadException: {0}", e.Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error("CurrentDomain_UnhandledException: {0}", e.ExceptionObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionPointer"></param>
        static int UnhandledExceptionFilter(IntPtr exceptionPointer)
        {
            uint code, address;

            DbgHelper.GetExceptionInfo(exceptionPointer, out code, out address);

            LogHelper.Fatal("Client crashed!!! ExceptionCode = {0:X}, ExceptionAddress = {1:X}", code, address);
            LogHelper.Flush();

            //MiniDump.Write(LogHelper.LogPath + "crash.dmp", MiniDump.Option.WithFullMemory, MiniDump.ExceptionInfo.None, exceptionPointer);

            MessageBox.Show("客户端遇到问题需要关闭，请联系技术支持。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

            return 1;
        }
    }
}

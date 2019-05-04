using System;
using System.Threading;
using System.Windows.Forms;
using Chun.Work.Common.Helper;
using MainForm.Properties;

namespace MainForm
{
    internal static class Program
    {
        /// <summary>
        ///     用于检测启动程序
        /// </summary>
        private static Mutex _instanceMutex;

        /// <summary>
        ///     处理异常
        /// </summary>
        private static DbgHelper.UnhandledExceptionFilter _exceptionCallback;

        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main() {
            try {
                LogHelper.Debug($"Client Startup! Version = {Application.ProductVersion}");

                //互斥量，启动一个
                bool flag;

                _instanceMutex = new Mutex(true, "Pactera.CTIClient", out flag);

                if (!flag) {
                    LogHelper.Debug("Client is already running.");
                }
                else {
                    _exceptionCallback = UnhandledExceptionFilter;

                    GC.KeepAlive(_exceptionCallback);

                    DbgHelper.SetUnhandledExceptionFilter(_exceptionCallback);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Application.Run(new MainForm());
                }
            }
            catch (Exception ex) {
                LogHelper.Error("Client startup failed. {0}", ex);

                MessageBox.Show(Resources.ClientStartupFail + ex.Message, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally {
                LogHelper.Flush();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            LogHelper.Error("Application_ThreadException: {0}", e.Exception);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            LogHelper.Error("CurrentDomain_UnhandledException: {0}", e.ExceptionObject);
        }

        /// <summary>
        /// </summary>
        /// <param name="exceptionPointer"></param>
        private static int UnhandledExceptionFilter(IntPtr exceptionPointer) {
            uint code, address;

            DbgHelper.GetExceptionInfo(exceptionPointer, out code, out address);

            LogHelper.Fatal("Client crashed!!! ExceptionCode = {0:X}, ExceptionAddress = {1:X}", code, address);
            LogHelper.Flush();

            //MiniDump.Write(LogHelper.LogPath + "crash.dmp", MiniDump.Option.WithFullMemory, MiniDump.ExceptionInfo.None, exceptionPointer);

            MessageBox.Show(Resources.ClientClosed, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

            return 1;
        }
    }
}
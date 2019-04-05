using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Chun.Demo.Common.Helper
{


    /// <summary>
    /// 日志类
    /// </summary>
    public class LogHelper
    {
        private static readonly Logger logger;
        public static readonly string LogPath = Path.Combine(Path.GetDirectoryName(typeof(LogHelper).Assembly.Location), "logs\\");

        /// <summary>
        /// 日志配置
        /// </summary>
        static LogHelper()
        {

           // InitLogManager();
            logger = LogManager.GetCurrentClassLogger();
        }

        public static void InitLogManager()
        {
            FileTarget fileTarget = new FileTarget();
            fileTarget.FileName = LogPath + "cticlient.log";
            fileTarget.Layout = "${longdate} Thread ${ThreadId} ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";
            fileTarget.CreateDirs = true;
            fileTarget.ArchiveEvery = FileArchivePeriod.Day;
            fileTarget.ArchiveFileName = LogPath + "cticlient-{#}.log";
            fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
            fileTarget.MaxArchiveFiles = 7;
           

            ConsoleTarget console = new ConsoleTarget();
            console.Name = "console";
            console.Layout = "${longdate} Thread ${ThreadId} ${pad:padding=5:inner=${level:uppercase=true}} ${message}";
            //console.Footer = ""


            //RichTextBoxTarget richTextTarget = new RichTextBoxTarget();
            //richTextTarget.Name = "control";
            //richTextTarget.ControlName = "txtLoggerRich";
            //richTextTarget.Layout = "${longdate} Thread ${ThreadId} ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";
            //richTextTarget.AutoScroll = true;
            //richTextTarget.FormName = "MainForm";
            //richTextTarget.UseDefaultRowColoringRules = true;
       

            // 异步输出
            NLog.Targets.Wrappers.AsyncTargetWrapper asyncTarget = new NLog.Targets.Wrappers.AsyncTargetWrapper(fileTarget);

            LoggingConfiguration config = new LoggingConfiguration();
            config.AddTarget("file", asyncTarget);
            config.AddTarget("console", console);
           // config.AddTarget("control", richTextTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, asyncTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, console));
          //  config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, richTextTarget));


            LogManager.Configuration = config;
        }
      



        /// <summary>
        /// 
        /// </summary>
        public static void Flush()
        {
            LogManager.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            LogEvent(LogLevel.Info, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info(string message, params object[] args)
        {
            LogEvent(LogLevel.Info, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            LogEvent(LogLevel.Error, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(string message, params object[] args)
        {
            LogEvent(LogLevel.Error, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            LogEvent(LogLevel.Error, "{0}", ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex,string message)
        {
            if (logger != null){
                logger.Error(ex, message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            LogEvent(LogLevel.Debug, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Debug(string message, params object[] args)
        {
            LogEvent(LogLevel.Debug, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            LogEvent(LogLevel.Warn, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Warn(string message, params object[] args)
        {
            LogEvent(LogLevel.Warn, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            LogEvent(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Fatal(string message, params object[] args)
        {
            LogEvent(LogLevel.Fatal, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            LogEvent(LogLevel.Trace, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Trace(string message, params object[] args)
        {
            LogEvent(LogLevel.Trace, message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetLastScopeName()
        {
            StackTrace st = new StackTrace();
            MethodBase method = st.GetFrame(2).GetMethod();

            return method.DeclaringType.Name + "." + method.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TraceEnter()
        {
            Trace("Enter {0}", GetLastScopeName());
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TraceExit()
        {
            Trace("Leave {0}", GetLastScopeName());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void TraceError(Exception ex)
        {
            Error("Leave {0} with exception. {1}", GetLastScopeName(), ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void LogEvent(LogLevel level, string message, params object[] args)
        {
            if (logger != null)
            {
                if(level!= LogLevel.Trace)
                {
                    MyMessageBox.Add($"线程 {Thread.CurrentThread.ManagedThreadId}  {message}");
                }
                logger.Log(level, message, args);
            }
        }


    }
}
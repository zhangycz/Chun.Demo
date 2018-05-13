// create By 08628 20180411

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Digiwin.Chun.Common.Model;
using Digiwin.Chun.Common.Views;

namespace Digiwin.Chun.Common.Controller {
    /// <summary>
    /// 日志
    /// </summary>
    public static class LogTools {
        #region 日志

     
        /// <summary>
        ///     日志
        /// </summary>
        public static void WriteLogByTreeView(MyTreeView treeView) {
            var toolpars = MyTools.Toolpars;
            var pathDic = MyTools.GetTreeViewFilePath(treeView.Nodes);
            var operationLog = (toolpars.CustomerName == null || toolpars.CustomerName.Equals(string.Empty)
                ? DateTime.Now.ToString("yyyyMMddhhmmss")
                : toolpars.CustomerName);
            var logPath=GetLogDir(operationLog);

            var logStr = new StringBuilder();
           const string empStr = @"      ";
            foreach (var kv in pathDic)
            {
                foreach (var fileinfo in kv.Value)
                logStr.AppendLine($"{(logStr.Length>0?empStr:string.Empty)}# {kv.Key} {empStr}{fileinfo.FileName}");
            }
            LogMsg(logPath, logStr.ToString());
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        public static void LogError(string msg)
        {
            var logPath = GetLogDir($@"error_{DateTime.Now:yyyyMMdd}");
            LogMsg(logPath, msg);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logPath"></param>
        public static void LogMsg(string logPath, string msg ) {
         
            var logStr = new StringBuilder();
            var headContentStr = GetHeadStr(@"_", 50);
            var empStr = "      ";
            if (!File.Exists(logPath)) {
                logStr.AppendLine(GetHardInfo());
            }
            logStr.AppendLine($"{headContentStr}OperationMsg{headContentStr}");
            logStr.AppendLine($"{empStr}#OperationTime#{empStr}{DateTime.Now:yyyy-MM-dd hh:mm:fff}")
                .AppendLine($"{empStr}#OperationUser#{empStr}{Environment.MachineName}")
                .AppendLine($"{empStr}#WinUser#{empStr}{Environment.UserName}")
                .AppendLine($"{empStr}{msg}").AppendLine($"{headContentStr}OperationMsg{headContentStr}");

            WriteToFile(logPath, logStr.ToString());
        }

        /// <summary>
        /// 硬件信息
        /// </summary>
        /// <returns></returns>
        public static string GetHardInfo() {
            var hardwareInfo = MyTools.HardwareInfo;
            var empStr = "      ";
            var logStr = new StringBuilder();
            var headContentStr = GetHeadStr(@"_", 50);
            logStr.AppendLine($"{headContentStr}PCInfo{headContentStr}");
            if (hardwareInfo != null) {
                logStr.AppendLine($"{empStr}#CpuCount#{empStr}{hardwareInfo.CpuInfos?.Count}");

                for (var i = 0; i < hardwareInfo.CpuInfos?.Count; i++)
                {
                    var cpuInfos = hardwareInfo.CpuInfos[i];

                    logStr.AppendLine($"{empStr}#Manufacturer{i}#{empStr}{cpuInfos.Manufacturer}")
                        .AppendLine($"{empStr}#CpuName{i}#{empStr}{cpuInfos.Name}")
                        .AppendLine($"{empStr}#Speed#{empStr}{cpuInfos.MaxClockSpeed}");
                }
                for (var i = 0; i < hardwareInfo.OsInfo?.Count; i++)
                {
                    var osInfo = hardwareInfo.OsInfo[i];
                    logStr.AppendLine($"{empStr}#OSName{i}#{empStr}{osInfo.Name}")
                        .AppendLine($"{empStr}#OSVersion{i}#{empStr}{osInfo.Version}")
                        .AppendLine($"{empStr}#WinverInfo#{empStr}{Environment.OSVersion.VersionString}")
                        .AppendLine($"{empStr}#CurrentDomain#{empStr}{Environment.UserDomainName}");
                }
                for (var i = 0; i < hardwareInfo.NetworkInfos?.Count; i++)
                {
                    var netInfo = hardwareInfo.NetworkInfos[i];
                    logStr.AppendLine($"{empStr}#Description#{empStr}{netInfo.Description}");
                    logStr.AppendLine($"{empStr}#IpAddress{i}#{empStr}{netInfo.IpAddress}");
                    logStr.AppendLine($"{empStr}#MacAddress{i}#{empStr}{netInfo.MacAddress}");
                }

                for (var i = 0; i < hardwareInfo.MemoryInfo?.Count; i++)
                {
                    var memoryInfos = hardwareInfo.MemoryInfo[i];
                    logStr.AppendLine($"{empStr}#Manufacturer{i}#{empStr}{memoryInfos.Manufacturer}");
                    logStr.AppendLine($"{empStr}#Size{i}#{empStr}{memoryInfos.Size}G");
                    logStr.AppendLine($"{empStr}#Speed{i}#{empStr}{memoryInfos.Speed}MHz");
                }
                for (var i = 0; i < hardwareInfo.MainBoardInfos?.Count; i++)
                {
                    var mainBoardInfos = hardwareInfo.MainBoardInfos[i];
                    logStr.AppendLine($"{empStr}#Manufacturer{i}#{empStr}{mainBoardInfos.Manufacturer}");
                    logStr.AppendLine($"{empStr}#Product{i}#{empStr}{mainBoardInfos.Product}");
                    logStr.AppendLine($"{empStr}#SerialNumber{i}#{empStr}{mainBoardInfos.SerialNumber}");
                    logStr.AppendLine($"{empStr}#Version{i}#{empStr}{mainBoardInfos.Version}");
                }
                for (var i = 0; i < hardwareInfo.DiskDriveInfos?.Count; i++)
                {
                    var diskDriveInfo = hardwareInfo.DiskDriveInfos[i];
                    logStr.AppendLine($"{empStr}#Model{i}#{empStr}{diskDriveInfo.Model}");
                    logStr.AppendLine($"{empStr}#SerialNumber{i}#{empStr}{diskDriveInfo.SerialNumber}");
                    logStr.AppendLine($"{empStr}#Size{i}#{empStr}{diskDriveInfo.Size}GB");
                    logStr.AppendLine($"{empStr}#UsedSpace{i}#{empStr}{diskDriveInfo.UsedSpace}GB");
                    logStr.AppendLine($"{empStr}#FreeSpace{i}#{empStr}{diskDriveInfo.FreeSpace}GB");
                }
            }
            logStr.AppendLine($"{headContentStr}PCInfo{headContentStr}");
            return logStr.ToString();
        }

        /// <summary>
        /// 记录到文件,已存在则追加
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logMsg"></param>
        public static void WriteToFile(string path, string logMsg)
        {
            using (var sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine(logMsg);
                sw.Flush();
            }
        }
     
        /// <summary>
        /// 获取log日志目录
        /// </summary>
        /// <returns></returns>
        public static string GetLogDir(string operationLog)
        {
            var toolpars = MyTools.Toolpars;
            var varAppPath = PathTools.PathCombine(toolpars.MvsToolpath, "log");
            if (!Directory.Exists(varAppPath))
                Directory.CreateDirectory(varAppPath);
            var logPath = $@"{varAppPath}\\{operationLog}.log";
            return logPath;
        }

        /// <summary>
        /// 生成一段指定长度的字符
        /// </summary>
        /// <param name="targetStr"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetHeadStr(string targetStr, int len)
        {
            var headStr = string.Empty;
            for (var i = 0; i <= len; i++)
                headStr += targetStr;
            return headStr;
        }
      
        /// <summary>
        /// 记录到Server
        /// </summary>
        /// <param name="fileInfos"></param>
        public static void WriteToServer(IEnumerable<FileInfos> fileInfos) {
            var toolpars = MyTools.Toolpars;
            SqlTools.InsertToolInfo(toolpars.FormEntity.TxtNewTypeKey, fileInfos);
        }

        #endregion
    }
}
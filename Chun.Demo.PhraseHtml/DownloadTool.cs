/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: DownloadTool
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/7 7:14:16
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.Model.Entity;
using Chun.Work.Common.Helper;

namespace Chun.Demo.PhraseHtml
{
    public class DownloadTool
    {
        private readonly Queue<filepath> _urlStatusQueue = new Queue<filepath>();
        public List<filepath> FilePathList = new List<filepath>();
        /// <summary>
        /// 总下载量
        /// </summary>
        public int TargetCount=> FilePathList.Count;
        /// <summary>
        /// 全部下载完成
        /// </summary>
        public Action OnCompleted;

        public string Referer { get; set; }
        public DownloadTool()
        {
            _completedAction = new CompletedAction();
            _completedAction.Action += StopUpdateListener;
        }

        private readonly CompletedAction _completedAction;

        /// <summary>
        ///     更新url访问状态线程
        /// </summary>
        private Thread _updateThread;

        /// <summary>
        /// 开启更新线程
        /// </summary>
        public void StartUpdateListener()
        {
            LogHelper.Debug("UpdateThread Start");
            ThreadHelper.StartThread(UpdateUrlStatus, ref _updateThread);
        }
        /// <summary>
        /// 关闭更新线程
        /// </summary>
        public void StopUpdateListener()
        {
            LogHelper.Debug("UpdateThread Stop");
            OnCompleted?.Invoke();
            ThreadHelper.StopInsertListener(ref _updateThread);
        }

        public void Start()
        {
            try
            {
                if (FilePathList==null|| FilePathList.Count == 0 )
                {
                    LogHelper.Error("download list is 0,return");
                    return;
                }

                _completedAction.TargetCount = TargetCount;
                //启动更新线程
                StartUpdateListener();
                LogHelper.Debug($@"{TargetCount} url  will be download");

                var num = Math.Ceiling(TargetCount / 100.0);

                for (var i = 0; i < num; i++)
                {
                    var processList = FilePathList.Skip(i * 100).Take(100).ToList();

                    LogHelper.Debug($"Process patch {i} -- num {processList.Count}");
                    Parallel.ForEach(processList,new ParallelOptions(){MaxDegreeOfParallelism = 20}, CreateDirAndDownload);
                }
                
            }
            catch
            {
                // ignored
            }
        }

        private void CreateDirAndDownload(filepath entity)
        {
            //LogHelper.TraceEnter();
            var path = entity.file_Path;
            var fileId = entity.id;
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(path))
                return;
            var dirName = entity.file_innerTxt;


            try
            {
                var fileNames = path.Split('/');
                if (fileNames.Length == 0)
                    return;
                var fileName = fileNames[fileNames.Length - 1];

                var dealString = string.IsNullOrEmpty(dirName)
                    ? fileNames[fileNames.Length - 3] + @"\"
                      + fileNames[fileNames.Length - 2]
                    : dirName;
                //处理目录中非法的字符
                dealString =
                    dealString.ToCharArray()
                        .Where(ch => !@"\/*|:?*<> ".ToCharArray().Contains(ch))
                        .Aggregate(string.Empty, (f, ch) => f + ch);
                var createDir =
                    (saveFilePath.EndsWith(@"\") ? saveFilePath : saveFilePath + @"\")
                    + dealString + @"\";

                if (!Directory.Exists(createDir))
                    try
                    {
                        Directory.CreateDirectory(createDir);
                    }
                    catch
                    {
                        LogHelper.Error($"Path {createDir} Error！FileName {fileName} ");
                    }

                fileName = createDir + fileName;
                DownLoad(path, fileName, status => {
                    entity = null;//释放对象
                    _completedAction.EventHandler();
                    lock (_urlStatusQueue)
                    {
                        var item = new filepath { id=fileId, file_Path = path, file_status_id = status };
                        _urlStatusQueue.Enqueue(item);
                        Monitor.Pulse(_urlStatusQueue);
                    }
                });
            }
            catch (Exception)
            {
                // ignored
            }
            //LogHelper.TraceExit();
        }

        private void UpdateUrlStatus()
        {
            try
            {
                while (true)
                {
                    filepath filepath = null;
                    lock (_urlStatusQueue)
                    {
                        if (_urlStatusQueue.Count > 0)
                            filepath = _urlStatusQueue.Dequeue();
                        else
                            Monitor.Wait(_urlStatusQueue);
                    }
                    if (filepath == null)
                        continue;
                    try
                    {
                        var path = filepath.file_Path;
                        var status = filepath.file_status_id;
                        var id = filepath.id;
                        LogHelper.Trace($@"Update {path}, status {status}");
                        if (status != null)
                             Tool.UpdateFilePath(id, (int)status);
                           // Tool.UpdateFilePath(path, 12, (int)status);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                LogHelper.Debug("UpdateThread thread abort.");
            }
            catch (Exception ex)
            {
                LogHelper.Debug("UpdateThread error. {0}", ex);
            }
            finally
            {
                LogHelper.Debug("UpdateThread  stopped.");
            }
        }

        /// <summary>
        ///     将文件下载到本地
        /// </summary>
        /// <param name="address">网络地址</param>
        /// <param name="fileName">本地地址</param>
        /// <param name="updateAction">访问状态处理</param>
        public  void DownLoad(string address, string fileName, Action<int> updateAction)
        {
            //100s无响应取消

            var newFileName = fileName;
            //if (Existed(address, fileName,out var timeOut))
            //{
            //    LogHelper.Debug($"file {fileName} existed！");
            //    updateAction(1);
            //    return;
            //}

            //if (timeOut) {
            //    LogHelper.Error($"download {address} Failed! Timeout！");
            //    updateAction(2);
            //    return;
            //}

            try
            {
                using (var wc = new MyWebClient { Timeout = 1000 })
                {
                    if (!string.IsNullOrEmpty(Referer)) {
                        wc.Headers.Add("Referer", Referer);
                    }
                    wc.DownloadFileAsync(new Uri(address), newFileName);
                    //wc.DownloadFileAsyncWithTimeout(new Uri(address), newFileName, "");
                    wc.DownloadFileCompleted += delegate {
                        LogHelper.Debug($"file {newFileName} download completed,url ： {address}");
                        updateAction(1);
                    };
                }
            }
            catch (WebException e)
            {
                updateAction(2);
                LogHelper.Error($"download {address} failed ! ErrorMsg {e.Message} data {e.Data} ");
                LogHelper.Error(e);
            }
            catch (Exception e)
            {
                updateAction(2);
                LogHelper.Error($"download {address}failed! ErrorMsg {e.Message} data {e.Data} ");
                LogHelper.Error(e);
            }
        }

        /// <summary>
        ///     检查文件是否存在
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public  bool Existed(string address, string fileName,out bool timeOut)
        {
            var existed = false;
            timeOut = false;
            if (!File.Exists(fileName))
                return false;

            var fileSize = new FileInfo(fileName).Length;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "HEAD";
                if (!string.IsNullOrEmpty(Referer)) {
                    request.Referer = Referer;
                }
                long size;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    size = response.ContentLength;
                    
                    LogHelper.Debug($"local file existed，file {fileName} web size {size} ，local size {fileSize}");
                    request.Abort();
                }
                if (size <= -1 || fileSize.Equals(size))
                    //>= 102400
                    existed = true;
            }
            catch (WebException e)
            {
                LogHelper.Debug($"validate file size error");

                LogHelper.Error(e);
                timeOut = true;
                return false;
            }

            return existed;
        }
    }
}

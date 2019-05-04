using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;
using Chun.Work.Common.Helper;

// ReSharper disable once CheckNamespace
namespace Chun.Demo.PhraseHtml
{
    public class DownLoadPic : IGetService
    {
        private readonly Queue<filepath> _urlStatusQueue = new Queue<filepath>();

        /// <summary>
        ///     更新url访问状态线程
        /// </summary>
        private Thread _updataThread;

        public event Action OnCompleted;

        public void GetService(PhraseHtmlType phraseHtmlType) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(saveFilePath) || !Directory.Exists(saveFilePath)) {
                LogHelper.Error($"Save Path Error");
                return;
            }

            var iognoreFailed = formPars.IgnoreFailed;
            var type = 3;
            if (iognoreFailed)
                type = 0;
            var startTime = formPars.StartDateTime;
            var endTime = formPars.EndDateTime;


            #region

            //获取未下载的地址
            var filePathList = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType), type)
                .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime)
                .Select(p => p).ToList();
            LogHelper.Debug($"{filePathList.Count} file  will be download");


            var count = filePathList.Count;
            if (count <= 0)
                return;
            StartUpdataListener();
            var num = Math.Ceiling(count / 100.0);

            for (var i = 0; i < num; i++) {
                var processList = filePathList.Skip(i * 100).Take(100).ToList();

                LogHelper.Debug($"Process patch {i} -- num {processList.Count}");
                Parallel.ForEach(processList, CreateDirAndDownload);
            }
            StopUpdataListener();
            OnCompleted?.Invoke();

            #endregion

            LogHelper.TraceExit();
        }

        public void StartUpdataListener() {
            LogHelper.Debug("UpdataThread Start");
            ThreadHelper.StartThread(UpdataUrlStatus, ref _updataThread);
        }

        public void StopUpdataListener() {
            LogHelper.Debug("UpdataThread Stop");

            ThreadHelper.StopInsertListener(ref _updataThread);
        }

        private void CreateDirAndDownload(filepath entity) {
            //LogHelper.TraceEnter();
            var path = entity.file_Path;
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(path))
                return;
            var dirName = entity.file_innerTxt;


            try {
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
                    try {
                        Directory.CreateDirectory(createDir);
                    }
                    catch {
                        LogHelper.Error($"Path {createDir} Error！FileName {fileName} ");
                    }

                fileName = createDir + fileName;
                Tool.DownLoad(path, fileName, status => {
                    lock (_urlStatusQueue) {
                        var item = new filepath {file_Path = path, file_status_id = status};
                        _urlStatusQueue.Enqueue(item);
                        Monitor.Pulse(_urlStatusQueue);
                    }
                });
            }
            catch (Exception) {
                // ignored
            }
            //LogHelper.TraceExit();
        }

        private void UpdataUrlStatus() {
            try {
                while (true) {
                    // OnCheckTaskCompleted?.Invoke(Filepaths, null);
                    filepath filepath = null;
                    lock (_urlStatusQueue) {
                        if (_urlStatusQueue.Count > 0)
                            filepath = _urlStatusQueue.Dequeue();
                        else
                            Monitor.Wait(_urlStatusQueue);
                    }
                    if (filepath == null)
                        continue;
                    try {
                        var path = filepath.file_Path;
                        var status = filepath.file_status_id;
                        LogHelper.Trace($@"updata {path}, status {status}");
                        if (status != null)
                            Tool.UpdatefilePath(path, 12, (int) status);
                    }
                    catch (Exception e) {
                        LogHelper.Error(e);
                    }
                }
            }
            catch (ThreadAbortException) {
                LogHelper.Debug("UpdataThread thread abort.");
            }
            catch (Exception ex) {
                LogHelper.Debug("UpdataThread error. {0}", ex);
            }
            finally {
                LogHelper.Debug("UpdataThread  stopped.");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Events;
using Chun.Demo.Common.Helper;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.PhraseHtml.Helper
{
  
    /// <summary>
    ///     解析html
    /// </summary>
    public class PhraseHtml
    {
        /// <summary>
        ///     数据库插入线程
        /// </summary>
        private Thread _insertListenerThread;

        /// <summary>
        ///     过滤已加入得目标
        /// </summary>
        public List<string> FilterPath { private get; set; }

        public List<string> TargetPath { private get; set; }

        /// <summary>
        ///     属性名，如"src"、"img"
        /// </summary>
        public string AttrName { private get; set; }

        /// <summary>
        ///     1.目录
        ///     2.文件地址
        /// </summary>
        public PhraseHtmlType PhraseHtmlType {  get; set; }
        private Queue<filepath> Filepaths { get; } = new Queue<filepath>();


        public string MatchNode { private get; set; }

        public event EventHandler<OnStartEventArgs> OnStart; //爬虫启动事件

        public event EventHandler<OnCompletedEventArgs> OnCompleted; //爬虫完成事件

        public event EventHandler<OnErrorEventArgs> OnError; //爬虫出错事件

        public event EventHandler OnCheckTaskCompleted; //检查任务完成情况
         

        public void Start() {
            try {
                if (TargetPath == null || TargetPath.Count == 0) {
                    LogHelper.Error("url list is null,return");
                    return;
                }
                //启动插入线程
                StartInsertListener();
                Parallel.ForEach(TargetPath, PhrasehtmlAsync);
            }
            catch {
                // ignored
            }
        }

        private void StartInsertListener() {
            try {
                if (_insertListenerThread != null && _insertListenerThread.IsAlive)
                    LogHelper.Debug(
                        $"the InsertListenerThread {_insertListenerThread.ManagedThreadId} isAlive,ignore start");
                var thread = new Thread( InsertfilePath) {IsBackground = true};
                thread.Start();

                var obj = Interlocked.Exchange(ref _insertListenerThread, thread);

                if (obj != null && obj.IsAlive)
                    obj.Abort();
            }
            catch {
                // ignored
            }
        }

        /// <summary>
        ///     数据库停止线程
        /// </summary>
        public void StopInsertListener() {
            try {
                var thread = Interlocked.Exchange(ref _insertListenerThread, null);

                if (thread == null || !thread.IsAlive)
                    return;
                var flag = thread.Join(1000);

                if (flag)
                    return;
                if (Filepaths.Count > 0) {
                    LogHelper.Debug("Filepaths count > 0,Continue");
                    return;
                }
                
                thread.Abort();
            }
            catch {
                // ignored
            }
        }

        /// <summary>
        ///     attrName
        ///     fileType
        /// </summary>
        /// <param name="orignUrl"></param>
        private async void PhrasehtmlAsync(string orignUrl) {
            Uri uri = null;
            //获取目录地址
            try {
                var url = orignUrl.ToUpper().StartsWith("HTTP")
                    ? orignUrl
                    : PhraseHtmlType.Equals(PhraseHtmlType.Img)
                        ? Tool.ConcatHttpPath(MyTools.FormPars.BasePath, "pw", orignUrl)
                        : Tool.ConcatHttpPath(MyTools.FormPars.BasePath, orignUrl);
                uri = new Uri(url);

                OnStart?.Invoke(this, new OnStartEventArgs(uri));

                var watch = new Stopwatch();

                watch.Start();

                var htmlDocument = await HtmlHelper.Start(uri);

                var hnCollection = HtmlHelper.GetNodeCollect(htmlDocument, MatchNode); //MyTools.FormPars.Match

                var titleCollection = HtmlHelper.GetNodeCollect(htmlDocument, "//head/title");

                var pathList = new List<filepath>();
                foreach (var hn in hnCollection) {
                    var path = hn.Attributes[AttrName].Value;

                    var innerTxt = string.IsNullOrEmpty(hn.InnerHtml)
                        ? (!string.IsNullOrEmpty(hn.InnerText)
                            ? hn.InnerText
                            : (titleCollection != null
                                ? (titleCollection.Count > 0 ? titleCollection[0].InnerHtml : "")
                                : ""))
                        : hn.InnerHtml;

                    if (string.IsNullOrEmpty(path))
                        continue;

                    if (!pathList.Any(p => p.file_Path.Equals(path)))
                        pathList.Add(new filepath {
                            file_Path = path,
                            file_innerTxt = innerTxt,
                            file_Type_id = Convert.ToInt32(PhraseHtmlType),
                            file_status_id = 0,
                            file_CreateTime = DateTime.Now,
                            file_parent_path = uri.AbsoluteUri
                        });
                }
                lock (FilterPath) {
                    pathList.ForEach(filePath => {
                        var path = filePath.file_Path;
                        if (!FilterPath.Contains(path)) {
                            FilterPath.Add(path);
                            if (path.ToUpper().StartsWith("READ")) {
                                var loc = path.LastIndexOf("&", StringComparison.Ordinal);
                                if (loc != -1)
                                    path = path.Substring(0, loc);
                            }
                            lock (Filepaths) {
                                filePath.file_Path = path;
                                Filepaths.Enqueue(filePath);
                                Monitor.Pulse(Filepaths);
                            }
                        }
                    });
                }
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID
                var milliseconds = watch.ElapsedMilliseconds; //获取请求执行时间
              
                OnCompleted?.Invoke(this,
                    new OnCompletedEventArgs(uri, threadId, milliseconds, htmlDocument.ToString(),orignUrl));
            }
            catch (Exception ex) {
                OnError?.Invoke(this, new OnErrorEventArgs(uri, ex,orignUrl));
            }
        }

        private void InsertfilePath()
        {
            LogHelper.Debug("Insertlistener thread start.");
            try
            {
                while (true)
                {
                    OnCheckTaskCompleted?.Invoke(Filepaths,null);
                    filepath filepath = null;
                    lock (Filepaths)
                    {
                        if (Filepaths.Count > 0)
                            filepath = Filepaths.Dequeue();
                        else
                            Monitor.Wait(Filepaths);
                    }
                    if (filepath == null)
                        continue;
                    try
                    {
                        var type = filepath.file_Type_id.ToString().EndsWith("1") ? "dir" : "file";
                        LogHelper.Debug($@"Insert {type}, filePath {filepath.file_Path}");
                        Tool.InsertfilePathByLinq(filepath);
                        var picType = MyTools.FormPars.PicType;
                        if (!string.IsNullOrEmpty(picType) && filepath.file_Type_id.Equals(12))
                        {
                            var categoryInfo = new category_info
                            {
                                category_id = picType,
                                category_path = filepath.file_Path
                            };
                            Tool.InsertCategoryInfo(categoryInfo);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                LogHelper.Debug("InsertListenerThread thread abort.");
            }
            catch (Exception ex)
            {
                LogHelper.Debug("InsertListenerThread error. {0}", ex);
            }
            finally
            {
                LogHelper.Debug("InsertListenerThread  stopped.");
            }

            //  LogHelper.TraceEnter();
            //var xinnerText = innerTxt.MyReplace("xp1024,核工厂,1024,.com,-,_,露出激情,图文欣賞,美图欣賞,|,powered by phpwind.net, ");

            //  InfoDAL.InsertfilePath(path, innerTxt, fileType, file_status_id, URL);
        }
    }
}
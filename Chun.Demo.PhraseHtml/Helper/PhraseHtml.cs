using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Helper;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;
using Chun.Work.Common.EventArgs;
using Chun.Work.Common.Helper;

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
        private Queue<filepath> FilepathQueue { get; } = new Queue<filepath>();


        public string MatchNode { private get; set; }

        /// <summary>
        /// 启动事件
        /// </summary>
        public event EventHandler<OnStartEventArgs> OnStart;
        /// <summary>
        /// 解析完成事件
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnPhraseUrlCompleted;

        /// <summary>
        /// 插入数据库才算完成
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnInsertCompleted; 

        /// <summary>
        /// 解析出错
        /// </summary>
        public event EventHandler<OnErrorEventArgs> OnError; 

        /// <summary>
        /// 检查插入
        /// </summary>
        public event EventHandler OnCheckTaskCompleted; 

        /// <summary>
        /// 完成
        /// </summary>
        public event EventHandler OnCompleted; 
         

        public void Start() {
            try {
                if (TargetPath == null || TargetPath.Count == 0)
                {
                    LogHelper.Error("url list is null,return");
                    return;
                }
                //启动插入线程
                StartInsertListener();
                Parallel.ForEach(TargetPath, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, PhraseHtmlAsync);
                TargetPath = null;
                OnCompleted?.Invoke(null, null);
            }
            catch {
                // ignored
            }
        }

        private void StartInsertListener() {
            ThreadHelper.StartThread(InsertFilePath,ref _insertListenerThread);
        }

        /// <summary>
        ///     数据库停止线程
        /// </summary>
        public void StopInsertListener() {
            lock (FilepathQueue) {
                if (FilepathQueue.Count > 0)
                {
                    LogHelper.Debug($@"Phrase Thread report Complete,but Insert Thread is Alive,return");

                    return;
                }
            }
            ThreadHelper.StopInsertListener(ref _insertListenerThread);
        }

        /// <summary>
        ///     attrName
        ///     fileType
        /// </summary>
        /// <param name="orignUrl"></param>
        private async void PhraseHtmlAsync(string orignUrl) {
            Uri uri = null;
            //获取目录地址
            try {
                var url = orignUrl.ToUpper().StartsWith("HTTP")
                    ? orignUrl
                    : PhraseHtmlType.Equals(PhraseHtmlType.Img)
                        ? UrlHelper.ConcatHttpPath(MyTools.FormPars.BasePath, "pw", orignUrl)
                        : UrlHelper.ConcatHttpPath(MyTools.FormPars.BasePath, orignUrl);
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
                    var replaceInnerText = innerTxt.MyReplace("xp1024,核工厂,1024,.com,-,_,露出激情,图文欣賞,美图欣賞,唯美写真,网友自拍,|,powered by phpwind.net, ");
                    if (!pathList.Any(p => p.file_Path.Equals(path))) {
                        var filepath = new filepath {
                            file_Path = path,
                            file_innerTxt = replaceInnerText,
                            file_Type_id = Convert.ToInt32(PhraseHtmlType),
                            file_status_id = 0,
                            file_CreateTime = DateTime.Now,
                            file_parent_path = orignUrl //uri.PathAndQuery
                        };
                        if (PhraseHtmlType.Equals(PhraseHtmlType.Dir)) {
                            filepath.category_id = MyTools.FormPars.PicType;
                        }

                        pathList.Add(item: filepath);
                    }
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
                            lock (FilepathQueue) {
                                filePath.file_Path = path;
                                FilepathQueue.Enqueue(filePath);
                                Monitor.Pulse(FilepathQueue);
                            }
                        }
                    });
                }
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID
                var milliseconds = watch.ElapsedMilliseconds; //获取请求执行时间
              
                OnPhraseUrlCompleted?.Invoke(this,
                    new OnCompletedEventArgs(uri, threadId, milliseconds, htmlDocument.ToString(),orignUrl));
            }
            catch (Exception ex) {
                OnError?.Invoke(this, new OnErrorEventArgs(uri, ex,orignUrl));
            }
        }

        private void InsertFilePath()
        {
            LogHelper.Debug("InsertListener thread start.");
            try
            {
                while (true)
                {
                    OnCheckTaskCompleted?.Invoke(null, null);

                    filepath filepath = null;
                    lock (FilepathQueue)
                    {

                        if (FilepathQueue.Count > 0)
                            filepath = FilepathQueue.Dequeue();
                        else
                            Monitor.Wait(FilepathQueue);
                    }
                    if (filepath == null)
                        continue;
                    try
                    {
                        var type = filepath.file_Type_id.ToString().EndsWith("1") ? "dir" : "file";
                        LogHelper.Debug($@"Insert {type}, filePath {filepath.file_Path}");
                        Tool.InsertfilePathByLinq(filepath);
                        Tool.UpdatefilePath(filepath.file_parent_path, Convert.ToInt32(PhraseHtmlType) - 1, 1);
                        OnInsertCompleted?.Invoke(null,null);
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
            //

            //  InfoDAL.InsertFilePath(path, innerTxt, fileType, file_status_id, URL);
        }
    }
}
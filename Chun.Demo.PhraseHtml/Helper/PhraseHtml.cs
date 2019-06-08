using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Helper;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Implement;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.EventArgs;
using Chun.Work.Common.Helper;
using HtmlAgilityPack;

namespace Chun.Demo.PhraseHtml.Helper
{

    /// <summary>
    ///     解析html
    /// </summary>
    public class PhraseHtml : IDisposable
    {
        #region Field
        /// <summary>
        ///     数据库插入线程
        /// </summary>
        private Thread _insertListenerThread;

        /// <summary>
        ///     过滤已加入得目标
        /// </summary>
       // public List<filepath> FilterPath { private get; set; }

        public List<filepath> TargetPath { private get; set; }

        /// <summary>
        ///     1.目录
        ///     2.文件地址
        /// </summary>
        public PhraseHtmlType PhraseHtmlType { get; set; }
        private Queue<filepath> FilepathQueue { get; } = new Queue<filepath>();


        public SiteInfo SiteInfo { private get; set; }
        #endregion

        #region Event
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
        #endregion


        public void Start()
        {
            try
            {
                if (TargetPath == null || TargetPath.Count == 0)
                {
                    LogHelper.Error("url list is null,return");
                    return;
                }
                //启动插入线程
                StartInsertListener();
                LogHelper.Debug($@"{TargetPath.Count} url  will be phrase");
                Parallel.ForEach(TargetPath, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, PhraseHtmlAsync);
            }
            catch
            {
                // ignored
            }
        }

        private void StartInsertListener()
        {
            ThreadHelper.StartThread(InsertFilePath, ref _insertListenerThread);
        }

        /// <summary>
        ///     数据库停止线程
        /// </summary>
        public void StopInsertListener()
        {
            lock (FilepathQueue)
            {
                if (FilepathQueue.Count > 0)
                {
                    LogHelper.Debug($@"PhraseThread report Complete,but InsertThread report is not Completed,Waiting");
                    return;
                }
            }
            OnCompleted?.Invoke(null, null);
            ThreadHelper.StopInsertListener(ref _insertListenerThread);
        }

        /// <summary>
        ///     attrName
        ///     fileType
        /// </summary>
        /// <param name="pathModel"></param>
        private async void PhraseHtmlAsync(filepath pathModel)
        {
            Uri uri = null;
            var origUrl = pathModel.file_Path;
            var fileId = pathModel.id;
            //获取目录地址
            try
            {
                var url = origUrl.ToUpper().StartsWith("HTTP")
                    ? origUrl
                    : PhraseHtmlType.Equals(PhraseHtmlType.Img)
                        ? UrlHelper.ConcatHttpPath(SiteInfo.BaseUrl, "pw", origUrl)
                        : UrlHelper.ConcatHttpPath(SiteInfo.BaseUrl, origUrl);
                uri = new Uri(url);

                OnStart?.Invoke(this, new OnStartEventArgs(uri));

                var watch = new Stopwatch();

                watch.Start();

                var htmlDocument = await HtmlHelper.Start(uri, SiteInfo.Encoding);

                var hnCollection = HtmlHelper.GetNodeCollect(htmlDocument, SiteInfo.TargetMatch);

                var titleCollection = HtmlHelper.GetNodeCollect(htmlDocument, SiteInfo.ExtendMatch);

                var pathList = new List<filepath>();

                if (PhraseHtmlType.Equals(PhraseHtmlType.Img) && SiteInfo is Mm131PageInfo && hnCollection.Count == 1)
                {
                    Mm131GeneratePath(url, hnCollection, titleCollection, pathList);
                }
                else
                {
                    if (hnCollection == null)
                    {
                        LogHelper.Fatal($"pageSource {htmlDocument.DocumentNode.InnerHtml}");
                    }
                    foreach (var hn in hnCollection)
                    {
                        var path = hn.Attributes[SiteInfo.AttrName].Value;

                        if (string.IsNullOrEmpty(path))
                            continue;

                        var nodeInnerText = SiteInfo.GetTitle(hn);

                        var innerTxt = !string.IsNullOrEmpty(nodeInnerText)
                            ? nodeInnerText
                            : (titleCollection != null
                                ? (titleCollection.Count > 0 ? titleCollection[0].InnerText : "")
                                : "");

                        if (!pathList.Any(p => p.file_Path.Equals(path)))
                        {
                            var replaceInnerText = innerTxt.MyReplace(
                                "xp1024,核工厂,1024,.com,-,_,露出激情,图文欣賞,美图欣賞,唯美写真,网友自拍,www.mm131,美女图片,|,powered by phpwind.net, ");

                            var filepath = new filepath
                            {
                                file_Path = path,
                                file_innerTxt = replaceInnerText,
                                file_Type_id = Convert.ToInt32(PhraseHtmlType),
                                file_status_id = 0,
                                file_CreateTime = DateTime.Now,
                                file_parent_path = origUrl //uri.PathAndQuery
                            };
                            if (PhraseHtmlType.Equals(PhraseHtmlType.Dir))
                            {
                                filepath.category_id = SiteInfo.Type; // MyTools.FormPars.PicType;
                            }

                            pathList.Add(item: filepath);
                        }

                    }
                }

                pathList.ForEach(filePath =>
                {
                    var path = filePath.file_Path;
                    if (path.ToUpper().StartsWith("READ"))
                    {
                        var loc = path.LastIndexOf("&", StringComparison.Ordinal);
                        if (loc != -1)
                            path = path.Substring(0, loc);
                    }
             
                    lock (FilepathQueue)
                    {
                        filePath.file_Path = path;
                        FilepathQueue.Enqueue(filePath);
                        //var fileSerializeObject = SerializeHelper.SerializeObject(filePath);
                        //LogHelper.Debug($@"add new url {fileSerializeObject}");
                        Monitor.Pulse(FilepathQueue);
                    }
                  
                });
                pathList.Clear();
                watch.Stop();
                var threadId = Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID
                var milliseconds = watch.ElapsedMilliseconds; //获取请求执行时间


                OnPhraseUrlCompleted?.Invoke(fileId,
                    new OnCompletedEventArgs(uri, threadId, milliseconds, htmlDocument.ToString(), origUrl));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(fileId, new OnErrorEventArgs(uri, ex, origUrl));
            }
        }

        private void Mm131GeneratePath(string url, HtmlNodeCollection hnCollection, HtmlNodeCollection titleCollection,
             List<filepath> pathList)
        {
            var pageNum =
                Convert.ToInt32(Regex.Replace(url, $@"{SiteInfo.BaseUrl}/{SiteInfo.ExtendUrl}/", "").Replace(@".html", ""));
            var hn = hnCollection[0];
            //共34页
            var countText = hn.InnerText;
            LogHelper.Debug(countText);
            var result = Convert.ToInt32(Regex.Replace(countText, @"[^0-9]+", ""));
            var pathListStr = new List<string>();
            for (var i = 1; i <= result; i++)
            {
                pathListStr.Add($@"http://img1.mm131.me/pic/{pageNum}/{i}.jpg");
            }

            var title = titleCollection != null
                ? (titleCollection.Count > 0 ? titleCollection[0].InnerText : "")
                : "";

            var replaceInnerText = title.MyReplace("xp1024,核工厂,1024,.com,-,_,露出激情,图文欣賞,美图欣賞,唯美写真,网友自拍,www.mm131,美女图片,|,powered by phpwind.net, ");
            foreach (var s in pathListStr)
            {
                var filepath = new filepath
                {
                    file_Path = s,
                    file_innerTxt = title,
                    file_Type_id = Convert.ToInt32(PhraseHtmlType),
                    file_status_id = 0,
                    file_CreateTime = DateTime.Now,
                    file_parent_path = url //uri.PathAndQuery
                };
                if (PhraseHtmlType.Equals(PhraseHtmlType.Dir))
                {
                    filepath.category_id = SiteInfo.Type; // MyTools.FormPars.PicType;
                }

                var fileSerializeObject = SerializeHelper.SerializeObject(filepath);
                LogHelper.Debug($@"add new url {fileSerializeObject}");

                pathList.Add(item: filepath);
            }
        }


        private void InsertFilePath()
        {
            LogHelper.Debug("InsertListener thread start.");
            try
            {
                while (true)
                {


                    filepath filepath = null;
                    lock (FilepathQueue)
                    {

                        if (FilepathQueue.Count > 0)
                            filepath = FilepathQueue.Dequeue();
                        else
                        {
                            OnCheckTaskCompleted?.Invoke(null, null);
                            Monitor.Wait(FilepathQueue);
                        }
                    }
                    if (filepath == null)
                        continue;
                    try
                    {
                        var type = filepath.file_Type_id.ToString().EndsWith("1") ? "dir" : "file";
                        LogHelper.Debug($@"Insert {type}, filePath {filepath.file_Path}");
                      
                        Tool.InsertFilePathByLinq(filepath);

                        filepath = null;

                        OnInsertCompleted?.Invoke(null, null);
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
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
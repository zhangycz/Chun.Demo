﻿using System;

namespace Chun.Demo.Common.Events
{
    /// <summary>
    /// 爬虫完成事件
    /// </summary>
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; private set; }// 爬虫URL地址
        public int ThreadId { get; private set; }// 任务线程ID
        public string PageSource { get; private set; }// 页面源代码
        public long Milliseconds { get; private set; }// 爬虫请求执行事件
        public string OrignUrl { get; set; }

        public OnCompletedEventArgs(Uri uri, int threadId, long milliseconds, string pageSource,string orignUrl="")
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
            OrignUrl = orignUrl;
        }
    }
}
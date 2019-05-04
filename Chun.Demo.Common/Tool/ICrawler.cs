using Chun.Work.Common.EventArgs;
using System;
using System.Threading.Tasks;

namespace Chun.Demo.Common
{
    public interface ICrawler
    {
        event EventHandler<OnStartEventArgs> OnStart;//爬虫启动事件

        event EventHandler<OnCompletedEventArgs> OnCompleted;//爬虫完成事件

        event EventHandler<OnErrorEventArgs> OnError;//爬虫出错事件

        Task<string> Start(Uri uri, string proxy); //异步爬虫
    }
}

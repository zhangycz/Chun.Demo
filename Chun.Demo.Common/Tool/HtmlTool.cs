using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chun.Demo.Model;
using HtmlAgilityPack;

namespace Chun.Demo.Common
{
    public class HtmlTool
    {

        /// <summary>
        /// 载入当前页面的文档
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public  HtmlDocument LoadHtml(string URL)
        {
            HtmlDocument htmlDocument = new HtmlWeb().Load(URL);
            return htmlDocument;
        }

        /// <summary>
        /// 返回匹配的节点数据
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <param name="matchNode"></param>
        /// <returns></returns>
        public  HtmlNodeCollection GetNodeCollect(HtmlDocument htmlDocument, string matchNode)
        {
            return htmlDocument.DocumentNode.SelectNodes(matchNode);
        }

        public HtmlNodeCollection getHtmlNodeCollection(string URL,string match, HtmlDocument htmlDocument)
        {
          
            HtmlNodeCollection targetCollection = null;
            try
            {
                targetCollection = GetNodeCollect(htmlDocument, match);
            }
            catch (Exception EX)
            {
                MyMessageBox.Add(string.Format("线程 {0} 获取文件 {1} 时发生了错误，错误信息 {2} ，错误详情 {3} ", System.Threading.Thread.CurrentThread.ManagedThreadId, URL, EX.Message, EX.Data));
                Console.WriteLine("线程 {0} 获取文件 {1} 时发生了错误，错误信息 {2} ，错误详情 {3} ", System.Threading.Thread.CurrentThread.ManagedThreadId, URL, EX.Message, EX.Data);

               // return Successed;
            }

            return targetCollection;

        }
    }
}

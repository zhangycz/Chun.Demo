using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

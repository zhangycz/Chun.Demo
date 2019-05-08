/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: Mm131PageInfo
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/6 20:12:44
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.Helper;
using HtmlAgilityPack;

namespace Chun.Demo.PhraseHtml.Implement
{
    public class Mm131PageInfo:SiteInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Type { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public override int PageSum { get; set; }

        /// <summary>
        /// 起始页数
        /// </summary>
        public override int StartPageNum { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        public override string BaseUrl { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public override string ExtendUrl { get; set; }

        /// <summary>
        /// 访问地址集合
        /// </summary>
        public override List<filepath> TargetPathList { get; set; }

        /// <summary>
        /// 标题正则
        /// </summary>
        public override string ExtendMatch { get; set; }
        /// <summary>
        /// 内容正则
        /// </summary>
        public override string TargetMatch { get; set; }
        /// <summary>
        /// 获取标签属性值
        /// </summary>
        public override string AttrName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public override Encoding Encoding { get; set; }

        /// <summary>
        /// Referer
        /// </summary>
        public override string Referer { get; set; }

        public override List<filepath> GetTargetList() {
            var currentPathList = new List<filepath>();
            for (var i = StartPageNum; i <= PageSum; i++)
            {
                var netPath = UrlHelper.ConcatHttpPath(BaseUrl,
                    ExtendUrl);
                if(0 == i ) continue;
                
                var url = 1==i ? netPath : $@"{netPath}/list_{Type}_{i}.html";

                currentPathList.Add(new filepath(){id = -1,file_Path = url});
            }

            TargetPathList = currentPathList;
            return currentPathList;
        }

        public override string GetTitle(HtmlNode node) {
            var innerText="";
            try {
                var selectSingleNode = node.SelectSingleNode(node.XPath+@"//img");
                 innerText = selectSingleNode.Attributes["alt"].Value;
            }
            catch (Exception e) {
               LogHelper.Error(e);
            }
            
            return innerText;
        }

        
    }
}

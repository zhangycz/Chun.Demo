/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: _1024PageInfo
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/6 20:33:41
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.Helper;
using HtmlAgilityPack;

namespace Chun.Demo.PhraseHtml.Implement
{
    public class Xp1024PageInfo :SiteInfo
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
        public override List<filepath> GetTargetList()
        {
            var currentPathList = new List<filepath>();


            for (var i = StartPageNum; i <= PageSum; i++)
            {
                string url;
                var netPath = UrlHelper.ConcatHttpPath(BaseUrl,
                    ExtendUrl);
                if (i == 1)
                {
                    url = netPath.Substring(0, netPath.Contains("-page-")
                        ? netPath.LastIndexOf("-page-", StringComparison.Ordinal)
                        : netPath.LastIndexOf("page=", StringComparison.Ordinal));
                }
                else
                {
                    if (netPath.Contains("-page-"))
                        url = $@"{netPath}{i}.html";
                    else
                        url = netPath + i;
                }

                currentPathList.Add(new filepath() { id = -1, file_Path = url });
            }

            return currentPathList;
        }

        public override string GetTitle(HtmlNode node) {
            return node.InnerText;
        }
    }
}

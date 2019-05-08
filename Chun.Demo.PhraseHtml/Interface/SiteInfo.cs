/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: IGetPageInfo
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/6 20:08:05
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chun.Demo.Model.Entity;
using HtmlAgilityPack;

namespace Chun.Demo.PhraseHtml.Interface
{
    public abstract class SiteInfo
    {
        /// <summary>
        /// 获取种类
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public virtual int PageSum { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public virtual string ExtendUrl { get; set; }

        /// <summary>
        /// Referer
        /// </summary>
        public virtual string Referer { get; set; }

        /// <summary>
        /// 起始页数
        /// </summary>
        public virtual int StartPageNum{ get; set; }


        /// <summary>
        /// 访问地址集合
        /// </summary>
        public virtual List<filepath> TargetPathList { get; set; }

        /// <summary>
        /// 标题正则
        /// </summary>
        public virtual string ExtendMatch { get; set; }
        /// <summary>
        /// 内容正则
        /// </summary>
        public virtual string TargetMatch { get; set; } 
        
        /// <summary>
        /// 获取标签属性值
        /// </summary>
        public virtual string AttrName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual Encoding Encoding { get; set; }
        /// <summary>
        /// 生成访问地址
        /// </summary>
        /// <returns></returns>
        public virtual List<filepath> GetTargetList() {
            throw new NotImplementedException();
        }

        public virtual string GetTitle(HtmlNode node) {
            throw new NotImplementedException();
        }

      
    }
}

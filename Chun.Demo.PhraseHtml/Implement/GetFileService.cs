using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.Helper;
using static System.String;

namespace Chun.Demo.PhraseHtml.Implement
{
    public static class PhraseHtmlConfig
    {
        private static string _maxDirPath;

        private static string _minDirPath;

        private static FormPars FormPars => MyTools.FormPars;

        /// <summary>
        ///     获取目录最大量
        /// </summary>
        public static string MaxDirNum {
            get {
                if (IsNullOrEmpty(_maxDirPath))
                    _maxDirPath = ConfigerHelper.GetAppConfig("MaxDirNum");
                return _maxDirPath;
            }
        }

        /// <summary>
        ///     获取目录最小量
        /// </summary>
        public static string MinDirNum {
            get {
                if (IsNullOrEmpty(_minDirPath))
                    _minDirPath = ConfigerHelper.GetAppConfig("MinDirNum");
                return _minDirPath;
            }
        }

        public static bool ValidateHtml() {
            var pass = true;
            if (!UrlHelper.ValidateHtml(FormPars.BasePath)) {
                LogHelper.Error("网站基址不是正确的格式");
                pass = false;
            }

            if (IsNullOrEmpty(FormPars.Match) || IsNullOrEmpty(FormPars.AttrName)) {
                LogHelper.Error("匹配字符不可为空");
                pass = false;
            }

            return pass;
        }
    }

    public class GetFileService : IGetService
    {
        public event Action OnCompleted;
        public SiteInfo SiteInfo { get; set; }

        /// <summary>
        ///     提供文件类型与获取属性获取文件地址
        ///     file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="phraseHtmlType">当前要获取的文件类型</param>
        public void GetService(PhraseHtmlType phraseHtmlType) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
           

            //获取数据库中未操作和失败的
            List<filepath> currentPathList;
            //SiteInfo pageInfo = new Mm131PageInfo()
            //{
            //    BaseUrl = @"http://www.mm131.com",//MyTools.FormPars.BasePath,
            //    ExtendUrl = @"xinggan",//MyTools.FormPars.ExtendPath
            //    Type = "6",
            //    //目录
            //    //TargetMatch = @"//div[@class='main']/dl/dd[not(@class='page' or @class='public-title')]/a",
            //    //内容
            //    TargetMatch = @"//div[@class='content']/div[@class='content-page']/span[1]",
            //    //ExtendMatch = @"//head/title",
            //    ExtendMatch = @"//div[@class='content']/h5",
            //    AttrName = "href",
            //    Encoding = Encoding.GetEncoding("gb2312")
            //};

            if (phraseHtmlType.Equals(PhraseHtmlType.Dir)) {
                LogHelper.Debug("Get DirPath");
                if (PhraseHtmlConfig.MaxDirNum == null)
                    throw new ArgumentNullException(nameof(PhraseHtmlConfig.MaxDirNum));

                var maxDirPath = Convert.ToInt32(PhraseHtmlConfig.MaxDirNum);
                var minDirPath = Convert.ToInt32(PhraseHtmlConfig.MinDirNum);

                SiteInfo.StartPageNum = minDirPath;
                SiteInfo.PageSum = maxDirPath;

                currentPathList = SiteInfo.GetTargetList();


            }
            else {
                LogHelper.Debug("获取文件地址");
                var ignoreFailed = formPars.IgnoreFailed;
                //是否忽略操作失败的，不勾选则不忽略
                var type = 3;
                if (ignoreFailed)
                    type = 0;
                var startTime = formPars.StartDateTime;
                var endTime = formPars.EndDateTime;
                //获取文件地址
                currentPathList = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType) - 1, type)
                    .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime && (p.category_id== SiteInfo.Type|| IsNullOrEmpty(SiteInfo.Type))).Select(p => new {
                        id= p.id,
                        file_Path = p.file_Path
                    }).ToList().Select(p=>new filepath() {
                        id = p.id,
                        file_Path = p.file_Path
                    }).ToList();
            }

            //获取数据库中已经有的文件地址，即过滤这些地址
            var filterPath = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType), 4)
                .Select(p => new {
                    file_Path = p.file_Path,
                     file_innerTxt = p.file_innerTxt
                }).ToList().Select(p=>new filepath() {
                    file_Path = p.file_Path,
                    file_innerTxt = p.file_innerTxt
                }).ToList();

            var phraseHtmlTool = new PhraseHtmlTool();

            phraseHtmlTool.OnCompleted += () => {
                OnCompleted?.Invoke();
            };

            phraseHtmlTool.StartPhraseHtml(phraseHtmlType, SiteInfo, filterPath, currentPathList);


            LogHelper.TraceExit();
        }


    }
}
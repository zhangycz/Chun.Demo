﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
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
                if (IsNullOrEmpty(_maxDirPath))
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

    public class GetPath : IGetService
    {
        public event Action OnCompleted;

        /// <summary>
        ///     提供文件类型与获取属性获取文件地址
        ///     file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="phraseHtmlType">当前要获取的文件类型</param>
        public void GetService(PhraseHtmlType phraseHtmlType) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
           

            //获取数据库中未操作和失败的
            var currentPathList = new List<string>();

            if (phraseHtmlType.Equals(PhraseHtmlType.Dir)) {
                LogHelper.Debug("Get DirPath");
                if (PhraseHtmlConfig.MaxDirNum == null)
                    throw new ArgumentNullException(nameof(PhraseHtmlConfig.MaxDirNum));

                var maxDirPath = Convert.ToInt32(PhraseHtmlConfig.MaxDirNum);
                var mixDirPath = Convert.ToInt32(PhraseHtmlConfig.MinDirNum);

                for (var i = mixDirPath; i <= maxDirPath; i++) {
                    string url;
                    var netPath = UrlHelper.ConcatHttpPath(MyTools.FormPars.BasePath,
                        MyTools.FormPars.ExtendPath);
                    if (i == 1) {
                        url = netPath.Substring(0, netPath.Contains("-page-")
                            ? netPath.LastIndexOf("-page-", StringComparison.Ordinal)
                            : netPath.LastIndexOf("page=", StringComparison.Ordinal));
                    }
                    else {
                        if (netPath.Contains("-page-"))
                            url = netPath + i + ".html";
                        else
                            url = netPath + i;
                    }

                    currentPathList.Add(url);
                }
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
                    .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime).Select(p => p.file_Path)
                    .ToList();
            }

            //获取数据库中已经有的文件地址，即过滤这些地址
            var filterPath = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType), 4).Select(p => p.file_Path).ToList();

            var phraseHtmlTool = new PhraseHtmlTool();
            phraseHtmlTool.OnCompleted += () => { OnCompleted?.Invoke(); };

            phraseHtmlTool.StartPhraseHtml(phraseHtmlType, formPars, filterPath, currentPathList);


            LogHelper.TraceExit();
        }
    }
}
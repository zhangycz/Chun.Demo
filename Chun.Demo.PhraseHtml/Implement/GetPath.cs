﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using static System.String;

namespace Chun.Demo.PhraseHtml {
    public class GetPath : IGetService {
        private object Locker { get; set; } = new object();

        /// <summary>
        ///     提供文件类型与获取属性获取文件地址
        ///     file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="fileTypeId">当前要获取的文件类型</param>
        public void GetService(int fileTypeId) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
            if (!Tool.ValidateHtml(formPars.BasePath)) {
                LogHelper.Error("网站基址不是正确的格式");
                return;
            }
            if (IsNullOrEmpty(formPars.Match) || IsNullOrEmpty(formPars.AttrName)) {
                LogHelper.Error("匹配字符不可为空");
                return;
            }
            var gt = new GetHtml();
        
            //获取数据库中未操作和失败的
            var currentPathList = new List<string>();
            var appConfig = ConfigerHelper.GetAppConfig("MaxDirPath");
            var minDirPathConfig = ConfigerHelper.GetAppConfig("MinDirPath");
            if (fileTypeId.ToString().EndsWith("1")) {
                LogHelper.Debug("获取目录地址");
                //获取目录地址
                //获取多少页目录
                if (appConfig == null)
                    throw new ArgumentNullException(nameof(appConfig));

                var maxDirPath = Convert.ToInt32(appConfig);
                var mixDirPath = Convert.ToInt32(minDirPathConfig);

                //for (var i = 1; i <= maxDirPath; i++) {
                for (var i = mixDirPath; i <= maxDirPath; i++) {
                    var url = Empty;
                    var netpath = Tool.ConcatHttpPath(MyTools.FormPars.BasePath,
                        MyTools.FormPars.ExtendPath);
                    if (i == 1 ) {
                        if (netpath.Contains("-page-")) {
                            url = netpath.Substring(0, netpath.LastIndexOf("-page-", StringComparison.Ordinal));
                        }
                        else {
                            url = netpath.Substring(0, netpath.LastIndexOf("page=", StringComparison.Ordinal));

                        }
                    }
                    else {
                        if (netpath.Contains("-page-")) {
                            url = netpath + i + ".html";
                        }
                        else {
                            url = netpath + i ;
                        }
                    }
                    currentPathList.Add(url);
                }
            }
            else {
                LogHelper.Debug("获取文件地址");
                var iognoreFailed = formPars.IgnoreFailed;
                //是否忽略操作失败的，不勾选则不忽略
                var type = 3;
                if (iognoreFailed)
                    type = 0;
                var startTime = formPars.StartDateTime;
                var endTime = formPars.EndDateTime;
                //获取文件地址
                currentPathList = Tool.ReadPathByLinq(fileTypeId - 1, type)
                    .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime<= endTime).Select(p => p.file_Path).ToList();
            }


            //获取数据库中已经有的文件地址，即过滤这些地址
            var targetPathList = Tool.ReadPathByLinq(fileTypeId, 4).Select(p => p.file_Path).ToList();

            gt.DirPath = targetPathList;

            Parallel.ForEach(currentPathList, item => {
                var url = item.ToUpper().StartsWith("HTTP")
                    ? item
                    : Tool.ConcatHttpPath(MyTools.FormPars.BasePath, item);

                if (gt.Phrasehtml(MyTools.FormPars.AttrName, url, fileTypeId)) {
                    Tool.UpdatefilePath(item, fileTypeId - 1, 1);
                    LogHelper.Debug($"{url} 的获取完成");
                }
                else {
                    Tool.UpdatefilePath(item, fileTypeId - 1, 2);
                    LogHelper.Debug($"{url} 的获取失败");
                }
            });
            LogHelper.TraceExit();
        }
    }
}
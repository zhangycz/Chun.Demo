using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Helper;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using static System.String;

namespace Chun.Demo.PhraseHtml.Implement
{
     
    public class GetPath : IGetService
    {
        public event Action OnCompleted;



        //Action IGetService.OnCompleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        ///     提供文件类型与获取属性获取文件地址
        ///     file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="phraseHtmlType">当前要获取的文件类型</param>
        public void GetService(PhraseHtmlType phraseHtmlType) {
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

            //获取数据库中未操作和失败的
            var currentPathList = new List<string>();
            var appConfig = ConfigerHelper.GetAppConfig("MaxDirPath");
            var minDirPathConfig = ConfigerHelper.GetAppConfig("MinDirPath");
            if (phraseHtmlType.Equals(PhraseHtmlType.Dir)) {
                LogHelper.Debug("获取目录地址");
                //获取目录地址
                //获取多少页目录
                if (appConfig == null)
                    throw new ArgumentNullException(nameof(appConfig));

                var maxDirPath = Convert.ToInt32(appConfig);
                var mixDirPath = Convert.ToInt32(minDirPathConfig);

                //for (var i = 1; i <= maxDirPath; i++) {
                for (var i = mixDirPath; i <= maxDirPath; i++) {
                    string url;
                    var netpath = Tool.ConcatHttpPath(MyTools.FormPars.BasePath,
                        MyTools.FormPars.ExtendPath);
                    if (i == 1) {
                        url = netpath.Substring(0, netpath.Contains("-page-")
                            ? netpath.LastIndexOf("-page-", StringComparison.Ordinal)
                            : netpath.LastIndexOf("page=", StringComparison.Ordinal));
                    }
                    else {
                        if (netpath.Contains("-page-"))
                            url = netpath + i + ".html";
                        else
                            url = netpath + i;
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
                currentPathList = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType) -1, type)
                    .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime).Select(p => p.file_Path)
                    .ToList();
            }


            //获取数据库中已经有的文件地址，即过滤这些地址
            var filterPath = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType), 4).Select(p => p.file_Path).ToList();

            var phraseHtmlTool = new PhraseHtmlTool();
            phraseHtmlTool.OnCompleted += () => {
                OnCompleted?.Invoke();
            };
            phraseHtmlTool.StartPhraseHtml(phraseHtmlType, formPars, filterPath, currentPathList);


            LogHelper.TraceExit();
            
        }
      
      
    }
}
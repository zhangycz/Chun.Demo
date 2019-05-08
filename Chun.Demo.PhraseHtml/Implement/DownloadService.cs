using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Implement;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.Helper;

// ReSharper disable once CheckNamespace
namespace Chun.Demo.PhraseHtml
{
    public class DownloadService : IGetService
    {
        /// <summary>
        /// 下载完成
        /// </summary>
        public event Action OnCompleted;

        public SiteInfo SiteInfo { get; set; } = new Xp1024PageInfo();

        public void GetService(PhraseHtmlType phraseHtmlType) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(saveFilePath) || !Directory.Exists(saveFilePath)) {
                LogHelper.Error($"Save Path Error");
                return;
            }

            var ignoreFailed = formPars.IgnoreFailed;
            var fileStatus = 3;
            if (ignoreFailed)
                fileStatus = 0;
            var startTime = formPars.StartDateTime;
            var endTime = formPars.EndDateTime;


            #region
            // startTime = new DateTime(2019,5,7,18,0,0);
            //获取未下载的地址
            var filePathList = Tool.ReadPathByLinq(Convert.ToInt32(phraseHtmlType), fileStatus)
                .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime )
                .Select(p => new {
                    id = p.id,
                    file_Path = p.file_Path,
                    file_innerTxt = p.file_innerTxt
                }).ToList().Select(p=>new filepath() {
                    id = p.id,
                    file_Path = p.file_Path,
                    file_innerTxt = p.file_innerTxt
                }).ToList();
            LogHelper.Debug($"{filePathList.Count} file  will be download");

            if (filePathList.Count <= 0)
                return;

            var downloadTool = new DownloadTool() {
                FilePathList = filePathList,
                Referer = SiteInfo.Referer
            };
            downloadTool.OnCompleted += () => {
                OnCompleted?.Invoke();
                try {
                    Tool.UpdateLocalPath(@"EXEC dbo.UpdateLocalPath");
                }
                catch(Exception e) {
                    LogHelper.Error(e);
                }
            };
            downloadTool.Start();
           

            #endregion

            LogHelper.TraceExit();
        }


    }
}
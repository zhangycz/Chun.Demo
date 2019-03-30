using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

// ReSharper disable once CheckNamespace
namespace Chun.Demo.PhraseHtml {
    public class DownLoadPic : IGetService {
        public void GetService(int fileTypeId) {
            LogHelper.TraceEnter();
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(saveFilePath) || !Directory.Exists(saveFilePath)) {
                LogHelper.Error($"保存地址有误");
                return;
            }

            var iognoreFailed = formPars.IgnoreFailed;
            var type = 3;
            if (iognoreFailed)
                type = 0;
            var startTime = formPars.StartDateTime;
            var endTime = formPars.EndDateTime;

       
            #region
            //获取未下载的地址
            var filePathList = Tool.ReadPathByLinq(fileTypeId, type)
                .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime)
                .Select(p => p).ToList();

            //最好不要使用全局变量
            Parallel.ForEach(filePathList, CreateDirAndDownload);
            #endregion
            LogHelper.TraceExit();
        }

        private void CreateDirAndDownload(filepath entity) {
            //LogHelper.TraceEnter();
            var path = entity.file_Path;
            var formPars = MyTools.FormPars;
            var saveFilePath = formPars.SavePath;
            if (string.IsNullOrEmpty(path))
                return;
            var dirName = entity.file_innerTxt;


            try {
                var fileNames = path.Split('/');
                if (fileNames.Length == 0)
                    return;
                var fileName = fileNames[fileNames.Length - 1];

                var dealString = string.IsNullOrEmpty(dirName)
                    ? fileNames[fileNames.Length - 3] + @"\"
                      + fileNames[fileNames.Length - 2]
                    : dirName;
                //处理目录中非法的字符
                dealString =
                    dealString.ToCharArray()
                        .Where(ch => !@"\/*|:?*<> ".ToCharArray().Contains(ch))
                        .Aggregate(string.Empty, (f, ch) => f + ch);
                var createDir =
                    (saveFilePath.EndsWith(@"\") ? saveFilePath : saveFilePath + @"\")
                    + dealString + @"\";

                if (!Directory.Exists(createDir))
                    try {
                        Directory.CreateDirectory(createDir);
                    }
                    catch {
                        LogHelper.Error($"路径 {createDir} 存在错误！文件名 {fileName} ");
                    }
                fileName = createDir + fileName;
                Tool.DownLoad(path, fileName);
            }
            catch (Exception) {
                // ignored
            }
            //LogHelper.TraceExit();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.PhraseHtml {
    public class DownLoadPic : IGetService {
        public void GetService(int fileTypeId) {
            if (string.IsNullOrEmpty(SaveFilePath) || !Directory.Exists(SaveFilePath)) {
                MyMessageBox.Add("保存地址有误");
                return;
            }

            #region

            var formPars = MyTools.FormPars;
            var iognoreFailed = formPars.IgnoreFailed;
            var type = 3;
            if (iognoreFailed)
                type = 0;
            var startTime = formPars.StartDateTime;
            var endTime = formPars.EndDateTime;
            //获取未下载的地址
            _filePathList = Tool.ReadPathByLinq(fileTypeId, type)
                .Where(p => p.file_CreateTime >= startTime && p.file_CreateTime <= endTime)
                .Select(p => p).ToList();

            //最好不要使用全局变量
            Parallel.ForEach(_filePathList, CreateDirAndDownLoad);

            #endregion
        }

        private void CreateDirAndDownLoad(filepath entity) {
            var path = entity.file_Path;

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
                    (SaveFilePath.EndsWith(@"\") ? SaveFilePath : SaveFilePath + @"\")
                    + dealString + @"\";

                if (!Directory.Exists(createDir))
                    try {
                        Directory.CreateDirectory(createDir);
                    }
                    catch {
                        MyMessageBox.Add($"路径 {createDir} 存在错误！文件名 {fileName} ");
                    }
                fileName = createDir + fileName;
                Tool.DownLoad(path, fileName);
            }
            catch (Exception) {
                // Console.WriteLine(e.Data + "\n" + e.Message);
            }
        }


        #region 属性

        /// <summary>
        ///     保存地址
        /// </summary>
        public string SaveFilePath { get; set; }

        //= @"J:\Picture\";

        /// <summary>
        ///     基址
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        ///     匹配器
        /// </summary>
        public string FileXpath { get; set; }

        /// <summary>
        ///     网络地址
        /// </summary>
        public string NetPath { get; set; }

        /// <summary>
        ///     获取属性
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///     文件地址
        /// </summary>
        public List<filepath> _filePathList { get; set; }

        /// <summary>
        ///     目录地址
        /// </summary>
        public List<filepath> _dirPathList { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using static System.String;

namespace Chun.Demo.PhraseHtml {
    public class GetPath : IGetService {
        public object locker { get; set; } = new object();

        #region 属性

        #region MyRegion

        //网络地址
        //public String NetPath { get; set; }
        //匹配选项
        //public String FileXpath { get; set; }
        //文件基址
        //public String BasePath { get; set; }
        //  public string PropertyName { get; set; } 

        #endregion


        public string SaveFilePath { get; set; }

        #endregion

        /// <summary>
        ///     提供文件类型与获取属性获取文件地址
        ///     file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="fileTypeId">当前要获取的文件类型</param>
        public void GetService(int fileTypeId) {
            var formPars = MyTools.FormPars;
            if (!Tool.ValidateHtml(formPars.BasePath)) {
                MyMessageBox.Add("网站基址不是正确的格式");
                return;
            }
            if (IsNullOrEmpty(formPars.Match) || IsNullOrEmpty(formPars.AttrName)) {
                MyMessageBox.Add("匹配字符不可为空");
                return;
            }

            #region MyRegion

            //if(!ConnectionStatusTool.CheckServeStatus( _netPath).Equals("200"))
            //{
            //    Console.WriteLine("网络故障！地址不可访问！");
            //    return;
            //}
            //  string netPath = "http://w1.vt97.biz/pw/thread.php?fid=16&page=";

            //获取目录地址
            //string dirXpath = "//tr[@class='tr3 t_one']/td/h3/a";
            // List<string> dirpathList = Tool.ReadPathByMySQL(1,2); 

            #endregion

            var gt = new GetHtml();
            //gt.Match = FileXpath;
            // gt.Html.Match = FileXpath;

            //List<string> currentPathList = new List<string>( );
            //获取数据库中未操作和失败的
            var currentPathList = new List<string>();
            var MaxDirPath = ConfigerHelper.GetAppConfig("MaxDirPath");
            if (fileTypeId.ToString().EndsWith("1")) {
                //获取目录地址
                //获取多少页目录
                if (MaxDirPath == null)
                    throw new ArgumentNullException(nameof(MaxDirPath));

                var maxDirPath = Convert.ToInt32(MaxDirPath);

                for (var i = 1; i <= maxDirPath; i++) {
                    var netpath = Tool.ConcatHttpPath(MyTools.FormPars.BasePath,
                        MyTools.FormPars.ExtendPath);
                    var url = netpath + i;
                    currentPathList.Add(url);
                }
            }
            else {
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

            gt.dirPath = targetPathList;

            Parallel.ForEach(currentPathList, item => {
                var url = item.ToUpper().StartsWith("HTTP")
                    ? item
                    : Tool.ConcatHttpPath(MyTools.FormPars.BasePath, item);

                gt = new GetHtml {
                    // Match = FileXpath,
                    dirPath = targetPathList
                };
                //gt.Html.Match = FileXpath;
                if (gt.run(MyTools.FormPars.AttrName, url, fileTypeId)) {
                    lock (locker) {
                        Tool.UpdatefilePath(item, fileTypeId - 1, 1);
                    }
                    MyMessageBox.Add($"线程 {Thread.CurrentThread.ManagedThreadId} 已经完成了文件 {url} 的获取！");
                }
                else {
                    lock (locker) {
                        Tool.UpdatefilePath(item, fileTypeId - 1, 2);
                    }
                    MyMessageBox.Add($"线程 {Thread.CurrentThread.ManagedThreadId} 对 {url} 的获取失败了！");
                }
            });
        }
    }
}
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.PhraseHtml
{
    public class GetPath : IGetService
    {
        #region 属性
        //网络地址
        public String NetPath
        {
            get; set;
        }
        //匹配选项
        public String FileXpath
        {
            get; set;
        }
        //文件基址
        public String BasePath
        {
            get; set;
        }

        public string SaveFilePath
        {
            get;
            set;

        }

        public string PropertyName
        {
            get;
            set;
        }
        #endregion

        //public void GetService ( )
        //{
        //    if (String.IsNullOrEmpty(_netPath) || (String.IsNullOrEmpty(_fileXpath)))
        //    {
        //        Console.WriteLine("地址或匹配字符为空");
        //        return;
        //    }
        //    GetHtml gt = new GetHtml( );

        //    gt.Match = _fileXpath;

        //    //读取数据库中已经存在的目录地址,过滤已经添加的
        //    List<string> dirpathList = Tool.ReadPathByLinq(1, 3).Select(p => p.file_Path).ToList( );

        //    gt.dirPath = dirpathList;

        //    Parallel.For(0, 153, item =>
        //    {
        //        string url = _netPath + item;
        //        gt.URL = url;
        //        if (gt.run("href", 1))
        //        {
        //            // Tool.UpdatefilePath(url, 1);
        //            Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
        //        }
        //        else
        //        {
        //            Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
        //        }
        //    });
        //    // gt.writeTxt(filepath);
        //}

        /// <summary>
        /// 提供文件类型与获取属性获取文件地址
        /// file_type_id 获取的文件类型 0 顶层结构 1 目录结构 2 文件结构
        /// </summary>
        /// <param name="fileTypeId">当前要获取的文件类型</param>
        public void GetService(int fileTypeId)
        {
            if (String.IsNullOrEmpty(FileXpath) || String.IsNullOrEmpty(PropertyName))
            {
                MyMessageBox.Add("匹配字符不可为空");
                Console.WriteLine("匹配字符不可为空");
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

            GetHtml gt = new GetHtml();
            gt.Match = FileXpath;
            //List<string> currentPathList = new List<string>( );
            //获取数据库中未操作和失败的
            List<string> currentPathList = new List<string>();
            if (fileTypeId.ToString().EndsWith("1"))
            {
                string MaxDirPath = ConfigerHelper.GetAppConfig("MaxDirPath");
                int maxDirPath = Convert.ToInt32(MaxDirPath);
                currentPathList = Tool.ReadPathByLinq(fileTypeId - 1, 4).Where(p => p.file_Path.StartsWith(NetPath)).Select(p => p.file_Path).Take(maxDirPath).ToList();
                if (currentPathList.Count == 0)
                {
                    Tool.CreateRootDir(NetPath);
                    GetService(fileTypeId);
                }

            }
            else
            {
                currentPathList = Tool.ReadPathByLinq(fileTypeId - 1, 3).Select(p => p.file_Path).ToList();
            }

            //--------------------------------------------------------
            //List<filepath> currentPathListEntity = new List<filepath>();
            //List<filepath> filterPathListEntity = Tool.ReadPathByLinq(fileTypeId - 1, 3);
            //--------------------------------------------------------


            #region old 弃用
            //不输入网址则取数据库中的网址
            //if (string.IsNullOrEmpty(NetPath) || fileTypeId.ToString().EndsWith("2"))
            //{
            //    //读取目录地址,即当前访问的地址
            //    currentPathList = filterPathList;
            //}
            //else
            //{
            //    #region 创建根目录
            //    //创建根目录

            //    if (currentPathList.Count == 0)
            //        if (String.IsNullOrEmpty(NetPath))
            //        {
            //            MyMessageBox.Add("没有查询到可访问的目录！可能未填写地址？");
            //            Console.WriteLine("没有查询到可访问的目录！可能未填写地址？");
            //            return;
            //        }
            //        else
            //        {

            //            for (int i = 1; i < 20; i++)
            //            {
            //                string url = NetPath + i.ToString();
            //                if (!currentPathList.Contains(url) && !filterPathList.Contains(url))
            //                {
            //                    currentPathList.Add(url);
            //                    filepath filepath = new filepath()
            //                    {
            //                        file_Path = url,
            //                        file_innerTxt = "",
            //                        file_Type_id = fileTypeId - 1,
            //                        file_status_id = 0,
            //                        file_CreateTime = DateTime.Now,
            //                        file_parent_path = "0"
            //                    };
            //                    Tool.InsertfilePathByLinq(filepath);
            //                }
            //            }

            //        }
            //    if (currentPathList.Count == 0)
            //    {
            //        if (filterPathList.Count == 0)
            //        {
            //            MyMessageBox.Add("没有查询到可访问的目录！");
            //            Console.WriteLine("没有查询到可访问的目录！");
            //            return;
            //        }

            //        currentPathList = filterPathList;

            //    }
            //    #endregion

            //} 
            #endregion



            //获取数据库中已经有的文件地址，即过滤这些地址
            List<string> targetPathList = Tool.ReadPathByLinq(fileTypeId, 4).Select(p => p.file_Path).ToList();

            gt.dirPath = targetPathList;

            //--------------------------------------------------------
            //List<filepath> targetPathListEntity = Tool.ReadPathByLinq(fileTypeId, 4);
            //gt.dirPathEntity = targetPathListEntity;
            //--------------------------------------------------------

        
            #region 测试用
            //foreach (string item in currentPathList)
            //{
            //    string url = string.Empty;
            //    if (item.ToUpper( ).StartsWith("HTTP"))
            //    {
            //        url = item;
            //    }
            //    else
            //        url = _basePath + item;
            //    if (gt.run(_PropertyName, url, file_type_id))
            //    {
            //        Tool.UpdatefilePath(url, file_type_id - 1, 1);
            //        Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
            //    }
            //    else
            //    {
            //        Tool.UpdatefilePath(url, file_type_id - 1, 2);
            //        Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
            //    }
            //} 
            //Parallel.ForEach(currentPathListEntity, item =>
            //{
            //    string url = string.Empty;
            //    if (item.file_Path.ToUpper().StartsWith("HTTP"))
            //    {
            //        url = item.file_Path;
            //    }
            //    else
            //        url = BasePath + item;
            //    if (gt.run(PropertyName, url, fileTypeId))
            //    {
            //        Tool.UpdatefilePath(item, fileTypeId - 1, 1);
            //        Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
            //    }
            //    else
            //    {
            //        Tool.UpdatefilePath(item, fileTypeId - 1, 2);
            //        Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
            //    }
            //});
            #endregion
            
            Parallel.ForEach(currentPathList, item =>
            {
                string url = string.Empty;
                if (item.ToUpper().StartsWith("HTTP"))
                {
                    url = item;
                }
                else
                    url = BasePath + item;
                //  Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
                gt = new GetHtml()
                {
                    Match = FileXpath,
                    dirPath = targetPathList
                };
                if (gt.run(PropertyName, url, fileTypeId))
                {
                    Tool.UpdatefilePath(item, fileTypeId - 1, 1);
                    MyMessageBox.Add($"线程 {Thread.CurrentThread.ManagedThreadId} 已经完成了文件 {url} 的获取！");
                    Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
                }
                else
                {
                    Tool.UpdatefilePath(item, fileTypeId - 1, 2);
                    // ReSharper disable once UseStringInterpolation
                    MyMessageBox.Add(string.Format("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url));
                    Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
                }
            });

        }

    }
}

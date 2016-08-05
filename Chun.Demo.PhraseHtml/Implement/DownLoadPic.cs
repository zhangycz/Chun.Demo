using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.IO;
using Chun.Demo.Common;
using Chun.Demo.Model.Entity;
using System.Threading;

namespace Chun.Demo.PhraseHtml
{
    public class DownLoadPic : IGetService
    {
        #region 属性
        /// <summary>
        /// 保存地址
        /// </summary>
        public string SaveFilePath
        {
            get; set;
        }
        //= @"J:\Picture\";

        /// <summary>
        /// 基址
        /// </summary>
        public string BasePath
        {
            get;
            set;
        }

        /// <summary>
        /// 匹配器
        /// </summary>
        public string FileXpath
        {
            get;
            set;
        }

        /// <summary>
        /// 网络地址
        /// </summary>
        public string NetPath
        {
            get;
            set;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        public string PropertyName
        {
            get;

            set;
        }

        /// <summary>
        /// 文件地址
        /// </summary>
        public List<filepath> _filePathList
        {
            get;

            set;

        }

        /// <summary>
        /// 目录地址
        /// </summary>
        public List<filepath> _dirPathList
        {
            get;

            set;
        } 
        #endregion
        
        public void GetService ( int fileTypeId )
        {

            if (String.IsNullOrEmpty(SaveFilePath) || !Directory.Exists(SaveFilePath))
            {
                Console.WriteLine("保存地址有误");
                return;
            }
            
            #region 废弃
            //string picPath = @"E:\Test\picpath.txt";
            //string downPath = @"E:\Test\picDone.txt";
            //读取未下载的图片地址
            // List<string> picPathList = Tool.ReadTxt(picPath);

            //读取已经下载完成的地址
            //List<string> DoneList = Tool.ReadTxt(downPath);
            //移除已经下载完成的
            //foreach (string doneItem in DoneList)
            //{
            //    if (picPathList.Contains(doneItem))
            //        picPathList.Remove(doneItem);
            //}
            #endregion

            #region 旧的
            //  List<string> picPathList = Tool.ReadPathByMySQL(filetype, 0);
            //  ////获取所属目录
            ////  _dirPathList = Tool.ReadPathByLinq(filetype - 1, 4);

            //  Parallel.ForEach(picPathList, path =>
            //  {
            //      dosomething(path);
            //  });
            #endregion

            #region 新的 待启用

            //获取未下载的地址
            _filePathList = Tool.ReadPathByLinq(fileTypeId, 0);

            //最好不要使用全局变量

            Parallel.ForEach(_filePathList, entity => CreateDirAndDownLoad(entity));

            //foreach (filepath entity in _filePathList)
            //{
                //new Thread(new ThreadStart(()=>
                //{
                //    CreateDirAndDownLoad(entity);
                //})).Start();
                //Task.Factory.StartNew(()=>
                //{
                //    CreateDirAndDownLoad(entity);
                //});
            //}

            #endregion

        }

        void CreateDirAndDownLoad ( filepath entity )
        {
            
            string path = entity.file_Path;
            
            if (string.IsNullOrEmpty(path))
                return;
            string dirName = entity.file_innerTxt;

            //List<string> dirPathNameList = _dirPathList.Where(item => item.file_Path == entity.file_parent_path).Select(item => item.file_innerTxt).Distinct( ).ToList();
            //string dealString = (string.IsNullOrEmpty(dirName) ? (dirPathNameList.Count != 0 ? dirPathNameList[0].ToString( ) : (fileNames[fileNames.Length - 3] + @"\"
            //      + fileNames[fileNames.Length - 2])) : dirName);
            //dealString = dealString.ToCharArray( ).Where(ch => !(@"\/*|:?*<> ".ToCharArray( ).Contains(ch))).Aggregate(string.Empty, ( f, ch ) => f + ch);

            try
            {
                string[] fileNames = path.Split('/');
                if (fileNames.Length == 0)
                    return;
                string fileName = fileNames[fileNames.Length - 1];

                string dealString = (string.IsNullOrEmpty(dirName) ? (fileNames[fileNames.Length - 3] + @"\"
                    + fileNames[fileNames.Length - 2]) : dirName);
                dealString = dealString.ToCharArray( ).Where(ch => !(@"\/*|:?*<> ".ToCharArray( ).Contains(ch))).Aggregate(string.Empty, ( f, ch ) => f + ch);
                string createDir =
                    (SaveFilePath.EndsWith(@"\") ? SaveFilePath : SaveFilePath + @"\")
                   + dealString+ @"\";

                if (!Directory.Exists(createDir))
                {
                    try
                    {
                      Directory.CreateDirectory(createDir);
                    }
                    catch
                    {
                        Console.WriteLine("路径 {0} 存在错误！文件名 {1} ", createDir,fileName);
                    }
                    
                }
                fileName = createDir + fileName;
                Tool.DownLoad(path, fileName);
                //  Console.WriteLine("线程 {0} 执行完了! 下载地址 {1} 本机地址 {2} ", Thread.CurrentThread.ManagedThreadId, path, fileName);

            }
            catch (Exception)
            {
               // Console.WriteLine(e.Data + "\n" + e.Message);
            }
        }

        void Dosomething ( String path )
        {
         
            if (string.IsNullOrEmpty(path))
                return;
            Console.WriteLine("线程 {0} 执行完了! 下载地址 {1} ", Thread.CurrentThread.ManagedThreadId, path);
            return;
            try
            {
                string[] dirstact = path.Split('/');
                if (dirstact.Length == 0)
                    return;
                string fileName = dirstact[dirstact.Length - 1];

                string createDir = SaveFilePath
                    + dirstact[dirstact.Length - 3] + @"\"
                    + dirstact[dirstact.Length - 2] + @"\";

                if (!Directory.Exists(createDir))
                {
                   Directory.CreateDirectory(createDir);
                }
              //  Console.WriteLine("线程 {0} 执行完了! 下载地址 {1} 本机地址 {2} ", Thread.CurrentThread.ManagedThreadId, path, fileName);

                fileName = createDir + fileName;
                Tool.DownLoad(path, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data + "\n" + e.Message);
                return;
            }
        }
    }
}

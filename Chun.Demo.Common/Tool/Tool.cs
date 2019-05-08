using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Chun.Demo.DAL;
using Chun.Demo.Model;
using Chun.Demo.Model.Entity;
using Chun.Work.Common.Helper;
using static System.String;

namespace Chun.Demo.Common.Tool
{
    public static class Tool
    {
        /// <summary>
        ///     将list写入文件
        /// </summary>
        /// <param name="dirPath"> list 文件</param>
        /// <param name="filepath"> 文本文件</param>
        public static void WriteTxt(List<string> dirPath, string filepath) {
            foreach (var path in dirPath) {
                var bytes = Encoding.Default.GetBytes(path + Environment.NewLine);

                using (var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
        }

        /// <summary>
        ///     从文本读入list
        /// </summary>
        /// <param name="filepath">文本文件</param>
        /// <returns></returns>
        public static List<string> ReadTxt(string filepath) {
            var dirPath = new List<string>();
            using (
                var sr = new StreamReader(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            ) {
                string strLine;
                while (!IsNullOrEmpty(strLine = sr.ReadLine()))
                    dirPath.Add(strLine);
                sr.Close();
            }
            return dirPath;
        }

        /// <summary>
        ///     从数据库读入list
        ///     type 1 目录
        ///     2 文件
        ///     file_status 0 未操作 和操作失败的
        ///     1  已经操作
        ///     2  操作失败
        ///     其他 其他
        /// </summary>
        /// <param name="type">读取类型</param>
        /// <param name="fileStatus">读取类型</param>
        /// <returns></returns>
        public static List<string> ReadPathByMySql(int type, int fileStatus) {
            return InfoDal.ReadPathByMySql(type, fileStatus);
        }

        /// <summary>
        ///     从数据库读入list
        ///     type 1 目录
        ///     2 文件
        ///     file_status 0 未操作 和操作失败的
        ///     1  已经操作
        ///     2  操作失败
        ///     3  未操作和操作失败的
        ///     其他 全部
        /// </summary>
        /// <param name="type">读取类型</param>
        /// <param name="fileStatus">读取类型</param>
        /// <returns></returns>
        public static IQueryable<filepath> ReadPathByLinq(int type, int fileStatus) {
            //return InfoDal.ReadPathByLinq(type, fileStatus);
            return InfoDal.ReadToQueryable(type, fileStatus);
        }

        public static IEnumerable<QueryTitleModel> QueryTitle(string procedureStr, object[] sqlparams) {
            return InfoDal.QueryTitle(procedureStr, sqlparams);
        }
        public static void UpdateLocalPath(string procedureStr) {
             InfoDal.UpdateLocalPath(procedureStr);
        }

        /// <summary>
        ///     errorPath
        /// </summary>
        /// <param name="errorPath"></param>
        public static void InsertErrorFileByLinq(errorpath errorPath) {
            InfoDal.InsertErrorFileByLinq(errorPath);
        }

        /// <summary>
        ///     将实体数据插入到filepath
        /// </summary>
        /// <param name="filepath"></param>
        public static void InsertFilePathByLinq(filepath filepath) {
            InfoDal.InsertFilePathByLinq(filepath);
        }

        /// <summary>
        ///     将实体数据插入到分类信息
        /// </summary>
        /// <param name="filepath"></param>
        public static void InsertCategoryInfo(category_info filepath) {
            InfoDal.InsertCategoryByLinq(filepath);
        }

      

        /// <summary>
        ///     更新文件状态
        ///     file_status 0 未操作
        ///     1 已操作 2 操作出错
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="title"></param>
        /// <param name="fileType">类型</param>
        /// <param name="fileStatus">状态</param>
        public static void UpdateFilePath(string path,string title, int fileType, int fileStatus) {
            //InfoDal.UpdateFilePath(path, fileType, fileStatus);

            InfoDal.UpdateFilePathByLinq(path, title,fileType, fileStatus);
        }  
        /// <summary>
        ///     更新文件状态
        ///     file_status 0 未操作
        ///     1 已操作 2 操作出错
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileStatus">状态</param>
        public static void UpdateFilePath(int id,  int fileStatus) {
            //InfoDal.UpdateFilePath(path, fileType, fileStatus);

            InfoDal.UpdateFilePathByLinq(id, fileStatus);
        }

       

        /// 更改种子文件的内部名称对文件重命名
        /// <param name="fileName">文件名</param>
        /// <param name="newDirPath">存放目录</param>
        /// <param name="fileEx">文件扩展名</param>
        /// <returns></returns>
        public static bool ChangFileName(string fileName, string newDirPath, string fileEx) {
            var success = false;
            if (!Directory.Exists(newDirPath))
                Directory.CreateDirectory(newDirPath);
            if (IsNullOrEmpty(fileName) || !Path.GetExtension(fileName).ToUpper().Equals(fileEx))
                return false;
            try {
                var tor = new TorrentHelper(fileName);
                if (!IsNullOrEmpty(tor.NameUtf8) || !IsNullOrEmpty(tor.Name)) {
                    var newFilePath = newDirPath + @"\" +
                                      (IsNullOrEmpty(tor.NameUtf8) ? tor.Name : tor.NameUtf8) + ".TORRENT";
                    if (File.Exists(newFilePath))
                        newFilePath = newDirPath + @"\" + Path.GetFileNameWithoutExtension(newFilePath) + "(1)" +
                                      ".TORRENT";
                    var fi = new FileInfo(fileName);
                    fi.MoveTo(newFilePath);
                    success = true;
                }
            }
            catch (Exception e1) {
                Console.WriteLine(e1.Message);
            }
            return success;
        }

        public static void DelEmptyDirAndFile(string basePath) {
            if (!Directory.Exists(basePath))
                return;
            var baseDir = new DirectoryInfo(basePath);

            var baseFileInfo = baseDir.GetFiles();
            foreach (var nextFile in baseFileInfo)
                if (nextFile.Length == 0)
                    nextFile.Delete();

            foreach (var nextFolder in baseDir.GetDirectories()) {
                if (nextFolder.GetDirectories().Length == 0 && nextFolder.GetFiles().Length == 0)
                    nextFolder.Delete();
                DelEmptyDirAndFile(nextFolder.FullName);
            }
        }

        
    }
}
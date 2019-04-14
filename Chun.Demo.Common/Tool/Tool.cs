using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Chun.Demo.Common.Helper;
using Chun.Demo.DAL;
using Chun.Demo.Model;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.Common.Tool
{
    public static class Tool
    {
        private static readonly object locker = new object();

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
                while (!String.IsNullOrEmpty(strLine = sr.ReadLine()))
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
        public static IEnumerable<filepath> ReadPathByLinq(int type, int fileStatus) {
            return InfoDal.ReadPathByLinq(type, fileStatus);
        }

        public static IEnumerable<QueryTitleModel> QueryTitle(string procedureStr, object[] sqlparms) {
            return InfoDal.QueryTitle(procedureStr, sqlparms);
        }

        /// <summary>
        ///     将实体数据插入到errorpath
        /// </summary>
        /// <param name="errorpath"></param>
        public static void InserErrorFileByLinq(errorpath errorpath) {
            InfoDal.InserErrorFileByLinq(errorpath);
        }

        /// <summary>
        ///     将实体数据插入到filepath
        /// </summary>
        /// <param name="filepath"></param>
        public static void InsertfilePathByLinq(filepath filepath) {
            InfoDal.InsertfilePathByLinq(filepath);
        }

        /// <summary>
        ///     将实体数据插入到分类信息
        /// </summary>
        /// <param name="filepath"></param>
        public static void InsertCategoryInfo(category_info filepath) {
            InfoDal.InsertCategoryByLinq(filepath);
        }

        /// <summary>
        ///     将文件下载到本地
        /// </summary>
        /// <param name="address">网络地址</param>
        /// <param name="fileName">本地地址</param>
        public static void DownLoad(string address, string fileName) {
            var wc = new MyWebClient {Timeout = 100};
            var newfileName = fileName;
            if (Existed(address, fileName)) {
                LogHelper.Debug($"文件{fileName} 已经存在！");
                lock (locker) {
                    UpdatefilePath(address, 12, 1);
                }
                return;
            }

            try {
                wc.DownloadFile(new Uri(address), newfileName);
                lock (locker) {
                    UpdatefilePath(address, 12, 1);
                }
                LogHelper.Debug($"文件 {newfileName} 下载完成,地址 ： {address}");
            }
            catch (WebException e) {
                lock (locker) {
                    UpdatefilePath(address, 12, 2);
                }
                LogHelper.Error($"文件 {address}下载失败! 错误信息 {e.Message} 错误详情 {e.Data} ");
            }
            catch (Exception e) {
                lock (locker) {
                    UpdatefilePath(address, 12, 2);
                }
                LogHelper.Error($"文件 {address}下载失败! 错误信息 {e.Message} 错误详情 {e.Data} ");
            }
        }

        /// <summary>
        ///     更新文件状态
        ///     file_status 0 未操作
        ///     1 已操作 2 操作出错
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filetype">类型</param>
        /// <param name="fileStatus">状态</param>
        public static void UpdatefilePath(string path, int filetype, int fileStatus) {
            //InfoDal.UpdatefilePath(path, filetype, fileStatus);

            InfoDal.UpdatefilePathByLinq(path, filetype, fileStatus);
        }

        /// <summary>
        ///     检查文件是否存在
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Existed(string address, string fileName) {
            var existed = false;

            if (!File.Exists(fileName))
                return false;

            var fileSize = new FileInfo(fileName).Length;
            //try
            //{
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            //request.Method = "HEAD";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //size = response.ContentLength;
            //response.Close();

            if (fileSize >= 102400)
                existed = true;
            //}
            //catch(WebException e)
            //{
            //    Console.WriteLine("线程 {0} 校验文件引发了异常！ \r\n 错误信息： {1} 错误详情： {2}", Thread.CurrentThread.ManagedThreadId,e.Message,e.Data);
            //    return false;
            //}

            return existed;
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
            if (String.IsNullOrEmpty(fileName) || !Path.GetExtension(fileName).ToUpper().Equals(fileEx))
                return false;
            try {
                var tor = new TorrentHelper(fileName);
                if (!String.IsNullOrEmpty(tor.NameUTF8) || !String.IsNullOrEmpty(tor.Name)) {
                    var newFilePath = newDirPath + @"\" +
                                      (String.IsNullOrEmpty(tor.NameUTF8) ? tor.Name : tor.NameUTF8) + ".TORRENT";
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

        public static void CreateRootDir(string netPath) {
            var MaxCreateDirPath = ConfigerHelper.GetAppConfig("MaxCreateDirPath");
            if (MaxCreateDirPath == null)
                throw new ArgumentNullException(nameof(MaxCreateDirPath));
            try {
                var maxCreateDirPath = Convert.ToInt32(MaxCreateDirPath);
                for (var i = 1; i < maxCreateDirPath; i++) {
                    var url = netPath + i;

                    var filepath = new filepath {
                        file_Path = url,
                        file_innerTxt = "",
                        file_Type_id = 10,
                        file_status_id = 0,
                        file_CreateTime = DateTime.Now,
                        file_parent_path = "0"
                    };
                    InsertfilePathByLinq(filepath);
                }
            }
            catch (Exception) {
                MyMessageBox.Add("创建根目录时发生错误！");
            }
        }

        /// <summary>
        ///     校验网址
        /// </summary>
        /// <param name="netPath"></param>
        /// <returns></returns>
        public static bool ValidateHtml(string netPath) {
            //支持http或https打头的字符串；
            //不含http的，但是以www打头的字符串；
            //不含http，但是支持xxx.com\xxx.cn\xxx.com.cn\xxx.net\xxx.net.cn 的字符串；
            var httpMatch =
                @"^((http|https)://)?(www.)?[A-Za-z0-9]+\.(com|net|cn|com\.cn|com\.net|net\.cn)?";
            //   @"(http | ftp | https):\/\/[\w\-_] + (\.[\w\-_]+)+([\w\-\.,@?^=% &amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            return Regex.IsMatch(netPath, httpMatch);
        }

        /// <summary>
        ///     拼接网址
        /// </summary>
        public static string ConcatHttpPath(string bathpath, params string[] paths) {
            if (!Regex.IsMatch(bathpath, @"^((http|https)://)"))
                bathpath = String.Concat(@"http://", bathpath);
            var extendPath = String.Empty;
            foreach (var path in paths) {
                if (extendPath.EndsWith(@"/")) {
                    var substring = extendPath.Substring(0, extendPath.Length - 2);
                }
                extendPath = path.StartsWith(@"/") ? String.Concat(extendPath, path) : String.Concat(extendPath, @"/", path);
            }

            return bathpath + extendPath;
        }
    }
}
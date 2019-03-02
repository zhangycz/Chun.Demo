using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using Chun.Demo.Common;
using Chun.Demo.Model.Entity;
using HtmlAgilityPack;

namespace Chun.Demo.PhraseHtml {
    public class GetHtml {
        public List<string> DirPath { get; set; }

        //--------------------------------------------------------
        public List<filepath> DirPathEntity { get; set; }

        //--------------------------------------------------------
        public HtmlTool HtmlTool { get; set; } = new HtmlTool();


        /// <summary>
        ///     attrName
        ///     属性名，如"src"、"img"
        ///     fileType
        ///     1.目录
        ///     2.文件地址
        /// </summary>
        /// <param name="attrName">获取指定内容</param>
        /// <param name="url"></param>
        /// <param name="fileType"></param>
        public bool Run(string attrName, string url, int fileType) {
            var successed = false;

            HtmlNodeCollection hnCollection;
            HtmlNodeCollection titleCollection;
            //获取目录地址
            try
            {
                var htmlDocument = HtmlTool.LoadHtml(url);
               // Console.WriteLine($"获取内容：{htmlDocument.DocumentNode.InnerHtml}");
                hnCollection = HtmlTool.GetNodeCollect(htmlDocument, MyTools.FormPars.Match);
                titleCollection = HtmlTool.GetNodeCollect(htmlDocument, "//head/title");
            }
            catch (Exception ex)
            {
                MyMessageBox.Add(
                    $"线程 {Thread.CurrentThread.ManagedThreadId} 获取文件 {url} 时发生了错误，错误信息 {ex.Message} ，错误详情 {ex.Data} ");

                return false;
            }


            if (hnCollection == null)
            {
                MyMessageBox.Add($"线程 {Thread.CurrentThread.ManagedThreadId} 获取文件 {url} 时发生了错误 ,解析时发生错误！");

                return false;
            }
            // Console.WriteLine("线程 {0} 获取文件 {1} 正在操作，锁定中……", System.Threading.Thread.CurrentThread.ManagedThreadId, URL);
            foreach (var hn in hnCollection)
            {
                var path = hn.Attributes[attrName].Value;
                var innerTxt = string.IsNullOrEmpty(hn.InnerHtml)
                    ? (!string.IsNullOrEmpty(hn.InnerText)
                        ? hn.InnerText
                        : (titleCollection != null
                            ? (titleCollection.Count > 0 ? titleCollection[0].InnerHtml : "")
                            : ""))
                    : hn.InnerHtml;
                if (string.IsNullOrEmpty(path))
                    continue;
                if (!DirPath.Contains(path))
                {
                    DirPath.Add(path);
                    try
                    {
                        if (path.ToUpper().StartsWith("READ"))
                        {
                            var loc = path.LastIndexOf("&", StringComparison.Ordinal);
                            path = path.Substring(0, loc);
                        }

                        InsertfilePath(path, innerTxt, fileType, 0, url);
                    }
                    catch
                    {
                        return successed;
                    }
                    successed = true;
                }
                else
                {
                    successed = true;
                }
            }
            return successed;
        }

        public bool Run(string attrName, filepath fileEntity) {
            var success = false;

            HtmlNodeCollection hnCollection;
            HtmlNodeCollection titleCollection;
            //获取目录地址
            try {
                var htmlDocument = HtmlTool.LoadHtml(fileEntity.file_Path);
                hnCollection = HtmlTool.GetNodeCollect(htmlDocument, MyTools.FormPars.Match);
                titleCollection = HtmlTool.GetNodeCollect(htmlDocument, "//head/title");
            }
            catch (Exception ex) {
                Console.WriteLine("线程 {0} 获取文件 {1} 时发生了错误，错误信息 {2} ，错误详情 {3} ", Thread.CurrentThread.ManagedThreadId,
                    fileEntity.file_Path, ex.Message, ex.Data);
                //InsertSql(fileType,URL);
                return success;
            }


            if (hnCollection == null) {
                //errorList.Add(URL);
                Console.WriteLine("线程 {0} 获取文件 {1} 时发生了错误 ,未能加载网页！", Thread.CurrentThread.ManagedThreadId,
                    fileEntity.file_Path);
                //InsertSql(fileType, URL);
                return success;
            }
            // Console.WriteLine("线程 {0} 获取文件 {1} 正在操作，锁定中……", System.Threading.Thread.CurrentThread.ManagedThreadId, URL);
            foreach (var hn in hnCollection) {
                var path = hn.Attributes[attrName].Value;
                var innerTxt = string.IsNullOrEmpty(hn.InnerHtml)
                    ? !string.IsNullOrEmpty(hn.InnerText)
                        ? hn.InnerText
                        : (titleCollection != null
                            ? ""
                            : (titleCollection.Count > 0 ? titleCollection[0].InnerHtml : ""))
                    : hn.InnerHtml;
                if (string.IsNullOrEmpty(path))
                    continue;
                if (!DirPathEntity.Any(p => p.file_Path.Equals(path)))
                    // if (!dirPath.Contains(path))
                {
                    DirPath.Add(path);
                    try {
                        //  InsertfilePath(path, innerTxt, fileType, 0, URL);
                    }
                    catch {
                        return success;
                    }
                    success = true;
                }
                //else if(!dirPathEntity.Any(p=>p.file_innerTxt))
                //  else
                success = true;
                //}
            }


            return success;
        }

        #region

        public void InsertSql(int fileType, string url) {
            //  InfoDAL.InsertSql(fileType, URL);
            var errorpath = new errorpath {
                error_CreateTime = DateTime.Now,
                error_path = url,
                error_type = fileType
            };
            Tool.InserErrorFileByLinq(errorpath);
        }

        public void InsertfilePath(string path, string innerTxt, int fileType, int fileStatusId, string url) {
            var xinnerText = innerTxt.MyReplace("xp1024,核工厂,1024,.com,-,_,露出激情,图文欣賞,美图欣賞,|,powered by phpwind.net, ");
            var filepath = new filepath {
                file_Path = path,
                file_innerTxt = innerTxt,
                file_Type_id = fileType,
                file_status_id = fileStatusId,
                file_CreateTime = DateTime.Now,
                file_parent_path = url
            };
            Tool.InsertfilePathByLinq(filepath);
            var picType = MyTools.FormPars.PicType;
            if (!string.IsNullOrEmpty(picType) && fileType == 12) {
                var categoryInfo = new category_info
                {
                    category_id = picType,
                    category_path = path
                };
                Tool.InsertCategoryInfo(categoryInfo);
            }
           

            //  InfoDAL.InsertfilePath(path, innerTxt, fileType, file_status_id, URL);
        }

       


        #endregion
    }
}
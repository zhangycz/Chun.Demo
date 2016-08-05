using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Chun.Demo.ICommon;
using System.Data.SqlClient;
using Chun.Demo.Common;
using Chun.Demo.DAL;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.PhraseHtml
{
    public class GetHtml
    {
        // string url ="";
        static object locker = new object( );
        public string Match
        {
            get;
            set;
        }
        
       
        public List<string> dirPath
        {
            get;
            set;
        }
        HtmlDocument loadHtml( string URL )
        {
            HtmlDocument htmlDocument = new HtmlWeb().Load(URL);
            return htmlDocument;

        }

        HtmlNodeCollection getNodeCollect( HtmlDocument htmlDocument, string matchNode)
        {
            return htmlDocument.DocumentNode.SelectNodes(matchNode);
        }

        /// <summary>
        /// attrName
        /// 属性名，如"src"、"img"
        /// fileType
        /// 1.目录
        /// 2.文件地址
        /// </summary>
        /// <param name="attrName">获取指定内容</param>
        /// <param name="fileType"></param>
        public bool run(string attrName,string URL, int fileType)
        {
            bool Successed = false;
            HtmlNodeCollection hnCollection;
            HtmlNodeCollection titleCollection;
            //获取目录地址
            try
            {
                HtmlDocument htmlDocument = loadHtml(URL);
                hnCollection = getNodeCollect(htmlDocument, Match);
                titleCollection = getNodeCollect(htmlDocument,"//head/title");
            }
            catch(Exception EX)
            {
                Console.WriteLine("线程 {0} 获取文件 {1} 时发生了错误，错误信息 {2} ，错误详情 {3} ", System.Threading.Thread.CurrentThread.ManagedThreadId, URL,EX.Message,EX.Data);
                //InsertSql(fileType,URL);
                return Successed;
            }


            if (hnCollection == null)
            {
                //errorList.Add(URL);
                Console.WriteLine("线程 {0} 获取文件 {1} 时发生了错误 ,未能加载网页！", System.Threading.Thread.CurrentThread.ManagedThreadId, URL);
                //InsertSql(fileType, URL);
                return Successed;
            }
           // Console.WriteLine("线程 {0} 获取文件 {1} 正在操作，锁定中……", System.Threading.Thread.CurrentThread.ManagedThreadId, URL);
            foreach (HtmlNode hn in hnCollection)
            {
                string path = hn.Attributes[attrName].Value;
                string innerTxt = string.IsNullOrEmpty(hn.InnerHtml) ? !string.IsNullOrEmpty(hn.InnerText)? hn.InnerText : (titleCollection != null ?"": (titleCollection.Count > 0 ? titleCollection[0].InnerHtml : "")):hn.InnerHtml;
                if (!string.IsNullOrEmpty(path))
                {
                    //lock (locker)
                    //{
                        if (!dirPath.Contains(path))
                        {
                           dirPath.Add(path);
                            try
                            {
                                InsertfilePath(path, innerTxt, fileType, 0, URL);
                            }
                            catch
                            {
                                return Successed;
                            }
                            Successed = true;
                        }
                        else
                            Successed = true;
                    //}
                }
            }
          //  Console.WriteLine("线程 {0} 获取文件 {1} 操作完成了，锁定解除了！", System.Threading.Thread.CurrentThread.ManagedThreadId, URL);
            return Successed;
        }

        #region 

        public void InsertSql(int fileType, string URL )
        {
          //  InfoDAL.InsertSql(fileType, URL);
            errorpath errorpath = new errorpath( )
            {
                error_CreateTime = DateTime.Now,
                error_path = URL,
                error_type = fileType
            };
            Tool.InserErrorFileByLinq(errorpath);

        }
        public void InsertfilePath(string path, string innerTxt, int fileType,int file_status_id ,string URL)
        {
            filepath filepath = new filepath( )
            {
                file_Path = path,
                file_innerTxt = innerTxt,
                file_Type_id = fileType,
                file_status_id = file_status_id,
                file_CreateTime = DateTime.Now,
                file_parent_path = URL
            };
            Tool.InsertfilePathByLinq(filepath);
          //  InfoDAL.InsertfilePath(path, innerTxt, fileType, file_status_id, URL);
        }

        //List<string> errorList = new List<string>( );
        ////List<errorpath> errorList = new List<errorpath>( );
        //public void writeTxt(string filepath)
        //{
        //    Tool.writeTxt(dirPath, filepath);
        //    if (errorList != null)
        //        Tool.writeTxt(errorList, @"E:\\Test\errorList.txt");
        //}
        #endregion
    }
}

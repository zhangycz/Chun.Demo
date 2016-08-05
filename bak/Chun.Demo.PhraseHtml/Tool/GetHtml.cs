using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Text;
using Chun.Demo.ICommon;
using System.Data.SqlClient;

namespace Chun.Demo.PhraseHtml
{
    public class GetHtml
    {
        // string url ="";
        public string URL
        {
            get;
            set;
        }
        public string Match
        {
            get;
            set;
        }
        HtmlWeb hw = new HtmlWeb();
        HtmlNodeCollection hnCollection;
        public List<String> dirPath
        {
            get;
            set;
        }
        List<string> errorList = new List<string>();
        HtmlDocument loadHtml()
        {
            return hw.Load(URL);

        }

        HtmlNodeCollection getNodeCollect(string matchNode)
        {
            return loadHtml().DocumentNode.SelectNodes(matchNode);
        }

        /// <summary>
        /// 1.目录
        /// 2.文件地址
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="downType"></param>
        public bool run(string attrName, int fileType)
        {
            bool Successed = false;
            
            //获取目录地址
            hnCollection = getNodeCollect(Match);
          
            if (hnCollection == null)
            {
                //errorList.Add(URL);
                InsertSql(fileType);
                return false;

            }

            foreach (HtmlNode hn in hnCollection)
            {
                string path = hn.Attributes[attrName].Value.ToString();
                string innerTxt = hn.InnerText;
                if (!string.IsNullOrEmpty(path))
                {
                    if (!dirPath.Contains(path))
                    {
                        dirPath.Add(path);
                        try
                        {
                            InsertfilePath(path, innerTxt, fileType,0);
                        }
                        catch
                        {
                            return Successed;
                        }
                        Successed = true;
                    }
                }
            }
            return Successed;
        }

        #region 

        public void InsertSql(int fileType)
        {
            string sql = "insert into errorpath (error_path,error_type,error_CreateTime) values ('" + URL + "','" + fileType + "','" + DateTime.Now + "')";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.run(sql, mysql.getInsert);
        }
        public void InsertfilePath(string path, string innerTxt, int fileType,int file_status)
        {
            string sql = "insert into FilePath (filePath,innerTxt,fileType,file_status,file_CreateTime) values ('" + path + "'" + ",'" + innerTxt + "','" + fileType + "','"+ file_status + "','" + DateTime.Now + "')";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.run(sql, mysql.getInsert);
        }
        public void writeTxt(string filepath)
        {
            Tool.writeTxt(dirPath, filepath);
            if (errorList != null)
                Tool.writeTxt(errorList, @"E:\\Test\errorList.txt");
        }
        #endregion
    }
}

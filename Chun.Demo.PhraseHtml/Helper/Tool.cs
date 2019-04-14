using Chun.Demo.ICommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chun.Demo.PhraseHtml
{
   public static class Tool
    {
        
        /// <summary>
        /// 将list写入文件
        /// </summary>
        /// <param name="dirPath"> list 文件</param>
        /// <param name="filepath"> 文本文件</param>
        public static void writeTxt(List<string> dirPath,string filepath)
        {

            foreach (string path in dirPath)
            {
                byte[] bytes = Encoding.Default.GetBytes(path + Environment.NewLine);

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }

            }
        }

        /// <summary>
        /// 从文本读入list
        /// </summary>
        /// <param name="filepath">文本文件</param>
        /// <param name="filterList">过滤条目</param>
        /// <returns></returns>
        public static List<string> ReadTxt(string filepath)
        {
            List<string> dirPath = new List<string>();
            using (StreamReader sr = new StreamReader(new FileStream(filepath,FileMode.OpenOrCreate,FileAccess.ReadWrite)))
            {
                string strLine ;
                while (!String.IsNullOrEmpty(strLine=sr.ReadLine()))
                {
                       dirPath.Add(strLine);

                }
                sr.Close();
            }
            return dirPath;
        }

        /// <summary>
        /// 从数据库读入list
        /// type 1 目录
        ///      2 文件
        /// file_status 0 未操作 和操作失败的
        ///        1  已经操作
        ///        2  操作失败
        ///        其他 其他
        /// </summary>
        /// <param name="type">读取类型</param>
        /// <param name="file_status">读取类型</param>
        /// <returns></returns>
        public static List<string> ReadPathByMySQL(int type,int file_status)
        {
            string sql = "select distinct filepath from filepath where filetype= " + type.ToString() + " and file_status in ( " + (file_status==0?"0,2":(file_status==1?"1":"0,1,2"))+")";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.run(sql, mysql.getListBysql);
            return mysql.pathList;
        }


        public static List<string> doneList = new List<string>();

        /// <summary>
        /// 将文件下载到本地
        /// </summary>
        /// <param name="address">网络地址</param>
        /// <param name="fileName">本地地址</param>
        public static void DownLoad(string address,string fileName)
        {
            MyWebClient wc = new MyWebClient();
            string newfileName = fileName;
            try
            {
                String ConnectingStatus = ConnectionStatusTool.CheckServeStatus(new string[] { address });
                if (ConnectingStatus.Equals("404") || ConnectingStatus.Equals("400"))
                {
                    throw new Exception("网络错误");
                }

                if (Existed(address, fileName))
                {
                    Console.WriteLine("文件{0} 已经存在！",fileName);
                    UpdatefilePath(address, 2, 1);
                    return;
                }

               Console.WriteLine("线程：{0} 开始下载 {1} ,地址 ： {2}", Thread.CurrentThread.ManagedThreadId, newfileName, address);
               wc.DownloadFile(new Uri(address), newfileName);
               doneList.Add(address);
               UpdatefilePath(address, 2, 1);
               Console.WriteLine("线程：{0} 退出，文件 {1} 下载完成,地址 ： {2}", Thread.CurrentThread.ManagedThreadId, newfileName, address);
            }
            catch (Exception e)
            {
                UpdatefilePath(address,2,2);
                Console.WriteLine(" 线程 {0} 下载失败了，文件 {1} ",Thread.CurrentThread.ManagedThreadId, address, e.Data+"\n"+e.Message);
            }
        }

        /// <summary>
        /// 更新文件状态
        /// file_status 0 未操作
        ///  1 已操作 2 操作出错
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filetype">类型</param>
        /// <param name="file_status">状态</param>
        public static void UpdatefilePath(string path,int filetype,int file_status)
        {

            string sql = "update filepath set file_status ="+ file_status + " ,file_updatetime = '"+DateTime.Now+"' where  filepath=  '" + path + "' and filetype = "+ filetype ;
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.run(sql, mysql.getUpdate);
        }
        
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Existed(string address,string fileName)
        {

            long size = 0L;
            bool existed = false;

            if (!File.Exists(fileName))
                return existed;

            long fileSize = new FileInfo(fileName).Length;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "HEAD";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                size = response.ContentLength;
                response.Close();

                if (fileSize == size)
                    existed = true;
            }
            catch(WebException e)
            {
                Console.WriteLine("线程 {0} 校验文件引发了异常！ \r\n 错误信息： {1} 错误详情： {2}", Thread.CurrentThread.ManagedThreadId,e.Message,e.Data);
                return false;
            }

            return existed;
            
        }
    
    }
}

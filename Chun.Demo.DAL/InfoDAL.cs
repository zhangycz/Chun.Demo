using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Chun.Demo.DAL.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.DAL
{
    public class InfoDal
    {
        /// <summary>
        ///     从数据库读入list
        ///     type 1 目录
        ///     2 文件
        ///     file_status 0 未操作
        ///     1  已经操作
        ///     2  操作失败
        ///     3  未操作和操作失败的
        ///     其他 全部
        /// </summary>
        /// <param name="fileTypeId"></param>
        /// <param name="fileStatusId"></param>
        /// <returns></returns>
        public static IEnumerable<filepath> ReadPathByLinq(int fileTypeId, int fileStatusId)
        {
            var newFileStatusId = fileStatusId == 0
                ? new int?[] {0}
                : (fileStatusId == 1
                    ? new int?[] {1}
                    : (fileStatusId == 2
                        ? new int?[] {2}
                        : (fileStatusId == 3
                            ? new int?[] {0, 2}
                            : new int?[] {0, 1, 2})));
            Expression<Func<filepath, bool>> func =
                c => c.file_Type_id == fileTypeId && newFileStatusId.Contains(c.file_status_id);
            var ls = new BaseDataQuery<filepath>().QueryByLinq(func);
            return ls;
        }

        public static void InsertfilePathByLinq(filepath filepath)
        {
            new BaseDataQuery<filepath>().Add(filepath);
        }

        public static void InserErrorFileByLinq(errorpath errorpath)
        {
            new BaseDataQuery<errorpath>().Add(errorpath);
        }

        public static void UpdatefilePathByLinq(string file_Path, int file_Type_id, int file_status_id)
        {
            // new BaseDataQuery<filepath>( ).Update(filepath);
            var sql =
                "update filepath set file_status_id = {0} ,file_updatetime = {1} where  file_path=  {2} and file_type_id = {3}";
            var Sql = "updatefilePath @file_status_id @file_updatetime, @file_path ,@file_type_id";

            new BaseDataQuery<filepath>().ExecuteSql(sql,
                new object[] {file_status_id, DateTime.Now, file_Path, file_Type_id});
        }

        #region 普通sql法

        /// <summary>
        ///     插入错误信息
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="url"></param>
        public static void InsertSql(int fileType, string url)
        {
            var sql = "insert into errorpath (error_path,error_type,error_CreateTime) values ('" + url + "','" +
                      fileType + "','" + DateTime.Now + "')";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetInsert);
        }

        /// <summary>
        ///     插入文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="innerTxt"></param>
        /// <param name="fileType"></param>
        /// <param name="fileStatus"></param>
        /// <param name="fileParentPath"></param>
        public static void InsertfilePath(string path, string innerTxt, int fileType, int fileStatus,
            string fileParentPath)
        {
            var sql =
                "insert into FilePath (file_Path,file_innerTxt,file_Type_id,file_status_id,file_CreateTime,file_parent_path) values ('" +
                path + "'" + ",'" + innerTxt + "','" + fileType + "','" + fileStatus + "','" + DateTime.Now + "','" +
                fileParentPath + "')";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetInsert);
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
        public static List<string> ReadPathByMySql(int type, int fileStatus)
        {
            var sql = "select distinct file_path from filepath where file_type_id= " + type +
                      " and file_status_id in ( " + (fileStatus == 0 ? "0,2" : (fileStatus == 1 ? "1" : "0,1,2")) + ")";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetListBysql);
            return mysql.PathList;
        }

        /// <summary>
        ///     更新文件状态
        ///     file_status 0 未操作
        ///     1 已操作 2 操作出错
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filetype">类型</param>
        /// <param name="file_status">状态</param>
        public static void UpdatefilePath(string path, int filetype, int file_status)
        {
            var sql = "update filepath set file_status_id =" + file_status + " ,file_updatetime = '" + DateTime.Now +
                      "' where  file_path=  '" + path + "' and file_type_id = " + filetype;
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetUpdate);
        }

        #endregion
    }
}
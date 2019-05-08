using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Chun.Demo.DAL.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.DAL
{
    public static class InfoDal
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
        public static IQueryable<filepath> ReadToQueryable(int fileTypeId, int fileStatusId)
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
            //var predicate = PredicateBuilder.True<filepath>();
            //predicate = predicate.And(p => p.file_Type_id == fileTypeId);
            //predicate = predicate.And(p => newFileStatusId.Contains(p.file_status_id));
           
            Expression<Func<filepath, bool>> funcExpression =
                p => p.file_Type_id == fileTypeId && newFileStatusId.Contains(p.file_status_id);
           return Execute(funcExpression);
        }

        //public static Expression<Func<T, TU>> GeneratExpression<T,TU>(List<string> paraList,object[] values) {
        //    var parameter = Expression.Parameter(typeof(T), "f");
        //    var fileTypeIdExpression = Expression.PropertyOrField(parameter, "file_Type_id");
        //    var fileStatusExpression = Expression.PropertyOrField(parameter, "file_status_id");
        //    var constant1 = Expression.Constant("郑浩");

        //    var constant2 = Expression.Constant("河北");

        //}


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
        /// <returns></returns>
        public static IQueryable<filepath> Execute(Expression<Func<filepath, bool>> funcExpression)
        {
            return new BaseDataQuery<filepath>().Query(funcExpression);
        }

        public static IEnumerable<QueryTitleModel> QueryTitle(string procedureStr, object[] sqlparms)
        {

            var ls = new BaseDataQuery<filepath>().QueryByStoredProcedure<QueryTitleModel>(procedureStr, sqlparms);
            return ls;
        }
        public static void UpdateLocalPath(string procedureStr)
        {

            new BaseDataQuery<filepath>().QueryByStoredProcedure(procedureStr);
          
        }
        

        public static void InsertFilePathByLinq(filepath filepath)
        {
            new BaseDataQuery<filepath>().Add(filepath);
        }
        public static void InsertCategoryByLinq(category_info categoryInfo)
        {
            var sql = $@"insert into category_info ( category_id , category_path,create_time ) values ('{categoryInfo.category_id}','
                      {categoryInfo.category_path}','{DateTime.Now}')";
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetInsert);
        }

        public static void InsertErrorFileByLinq(errorpath errorPath)
        {
            new BaseDataQuery<errorpath>().Add(errorPath);
        }

        public static void UpdateFilePathByLinq(string filePath,string title, int fileTypeId, int fileStatusId)
        {
            // new BaseDataQuery<filepath>( ).Update(filepath);
            var sql =
                "update filepath set file_status_id = {0} ,file_updatetime = {1} where  file_path=  {2} and file_type_id = {3} and file_innerTxt = {4}";
            new BaseDataQuery<filepath>().ExecuteSql(sql,
                new object[] {fileStatusId, DateTime.Now, filePath, fileTypeId, title });
        }

        public static void UpdateFilePathByLinq(int id, int fileStatusId)
        {
            // new BaseDataQuery<filepath>( ).Update(filepath);
            var sql =
                "update filepath set file_status_id = {0} ,file_updatetime = {1} where  id=  {2}";
            new BaseDataQuery<filepath>().ExecuteSql(sql,
                new object[] {fileStatusId, DateTime.Now, id});
        }

        #region 普通sql法

        /// <summary>
        ///     插入错误信息
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="url"></param>
        public static void InsertSql(int fileType, string url)
        {
            var sql = "insert into errorPath (error_path,error_type,error_CreateTime) values ('" + url + "','" +
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
        public static void InsertFilePath(string path, string innerTxt, int fileType, int fileStatus,
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
        /// <param name="fileStatus">状态</param>
        public static void UpdateFilePath(string path, int filetype, int fileStatus)
        {
            var sql = $@"update filepath set file_status_id ={fileStatus},file_updatetime = { DateTime.Now }
                       where  file_path=  {path}   and file_type_id = {filetype}" ;
            ISql<SqlCommand, SqlConnection> mysql = new MsSql();
            mysql.Run(sql, mysql.GetUpdate);
        }

        #endregion
    }
}
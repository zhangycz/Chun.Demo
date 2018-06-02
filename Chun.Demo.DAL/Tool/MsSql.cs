using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Chun.Demo.ICommon;

namespace Chun.Demo.DAL.Tool {
    public class MsSql : ISql<SqlCommand, SqlConnection> {
        private const string StringKey = "SqlString";
        private SqlConnection _sqpConnection;

        public SqlConnection SqlConn {
            get {
                if (_sqpConnection != null)
                    return _sqpConnection;
                var connectionString = ConfigurationManager.ConnectionStrings[StringKey].ConnectionString;
                _sqpConnection = new SqlConnection(connectionString);
                return _sqpConnection;
            }
            set => _sqpConnection = value;
        }

        public List<string> PathList { get; set; }

        public void Run(string sql, excuteSql<SqlCommand> exec) {
            try
            {
                using (var conn = SqlConn) {
                    conn.Open();
                    var sqlCommand = GetSqlCommand(sql);
                    //解决可能超时的问题
                    sqlCommand.CommandTimeout = 18000;
                    exec(sqlCommand);
                }

            }
            catch (SqlException)
            {
               
            }
            finally
            {
                SqlConn.Close();
            }

        }

        /// <summary>
        ///     建立执行命令语句对象
        /// </summary>
        /// <param name="sql">
        /// </param>
        /// <returns></returns>
        public SqlCommand GetSqlCommand(string sql) {
            return new SqlCommand(sql, SqlConn);
        }

        /// <summary>
        ///     查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void GetListBysql(SqlCommand sqlCommand) {
            PathList = new List<string>();
            var reader = sqlCommand.ExecuteReader();
            try {
                while (reader.Read())
                    if (reader.HasRows)
                        PathList.Add(reader["file_Path"].ToString());
            }
            catch (Exception) {
                Console.WriteLine("查询失败了！");
            }
            finally {
                reader.Close();
            }
        }


        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void GetInsert(SqlCommand sqlCommand) {
            try {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex) {
                var message = ex.Message;
                Console.WriteLine("插入数据失败了！" + message);
            }
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void GetUpdate(SqlCommand sqlCommand) {
            try {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex) {
                var message = ex.Message;
                Console.WriteLine("修改数据失败了！" + message);
            }
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void GetDel(SqlCommand sqlCommand) {
            try {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex) {
                var message = ex.Message;
                Console.WriteLine("删除数据失败了！" + message);
            }
        }
    }
}
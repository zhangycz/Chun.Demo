using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Chun.Demo.DAL
{
    public  class MsSql : ISql<SqlCommand, SqlConnection>
    {

        public  SqlConnection sqlConn
        {
                   get;set;
        }
        public List<string> pathList
        {
            get;
            set;
        }
    
        public  MsSql()
        {
            const string stringKey = "SqlString";

            string connectionString = ConfigurationManager.ConnectionStrings[stringKey].ConnectionString;
            
            sqlConn = new SqlConnection(connectionString);

        }

        public void run(string sql, excuteSql<SqlCommand> exec)
        {
            SqlCommand sqlCommand = getSqlCommand(sql);
            //解决可能超时的问题
            sqlCommand.CommandTimeout = 18000;
            sqlConn.Open();
            try
            {
                exec(sqlCommand);
            }catch(SqlException)
            {

            }
            finally
            {
                sqlConn.Close();

            }
        }

        /// <summary>
        /// 建立执行命令语句对象
        /// </summary>
        /// <param name="sql">
        /// </param>
        /// <returns></returns>
        public SqlCommand getSqlCommand(string sql)
        {
            return new SqlCommand(sql, sqlConn);
        }

        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void getListBysql(SqlCommand sqlCommand)
        {
            pathList = new List<string>();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        //Console.WriteLine(reader["filePath"].ToString());
                        pathList.Add(reader["file_Path"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("查询失败了！");
            }
            finally
            {
                reader.Close();
            }

        }


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void getInsert(SqlCommand sqlCommand)
        {
            try

            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                Console.WriteLine("插入数据失败了！" + message);
            }

        }

        /// <summary>
        /// 修改数据 
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void getUpdate(SqlCommand sqlCommand)
        {
            try

            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine("修改数据失败了！" + message);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void getDel(SqlCommand sqlCommand)
        {
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("删除数据失败了！" + message);
            }
        }
    }
}

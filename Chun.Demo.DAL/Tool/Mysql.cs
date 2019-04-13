using Chun.Demo.ICommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Chun.Demo.DAL
{
    
     public class Mysql : ISql<MySqlCommand, MySqlConnection>
    {
        
        public List<string> PathList
        {
            get;
            set;
        }

        public MySqlConnection SqlConn
        {
            get;
            set;
        }
        
        /// <summary>
        /// 建立mysql数据库链接
        /// </summary>
        /// <returns></returns>
        public Mysql(MySqlConnection sqlConn)
        {
            this.SqlConn = sqlConn;
        }

         /// <summary>
         /// 建立执行命令语句对象
         /// </summary>
         /// <param name="sql"></param>
         /// <returns></returns>
         public  MySqlCommand GetSqlCommand(String sql)
        {
            var mySqlCommand = new MySqlCommand(sql, SqlConn);
            return mySqlCommand;
        }

        public  void Run(string sql, Action<MySqlCommand> exec)
        {
            var mySqlCommand = GetSqlCommand(sql);
            SqlConn.Open();
            try
            {
                exec(mySqlCommand);
            }
            catch (MySqlException)
            {
            	
            }
            finally
            {
                SqlConn.Close();

            }
        }

        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void GetListBysql(MySqlCommand sqlCommand)
        {
            PathList = new List<string>();
            var reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        //Console.WriteLine(reader["filePath"].ToString());
                        PathList.Add(reader["filePath"].ToString());
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
        public  void GetInsert(MySqlCommand sqlCommand)
        {
            try

            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("插入数据失败了！" + message);
            }

        }
        /// <summary>
        /// 修改数据 
        /// </summary>
        /// <param name="sqlCommand"></param>
        public  void GetUpdate(MySqlCommand sqlCommand)
        {
            try

            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("修改数据失败了！" + message);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        public  void GetDel(MySqlCommand sqlCommand)
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

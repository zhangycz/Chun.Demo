using Chun.Demo.ICommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Chun.Demo.DAL
{
    
     public class Mysql : ISql<MySqlCommand, MySqlConnection>
    {
        
        public List<string> pathList
        {
            get;
            set;
        }

        public MySqlConnection sqlConn
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
            this.sqlConn = sqlConn;
        }

         /// <summary>
         /// 建立执行命令语句对象
         /// </summary>
         /// <param name="sql"></param>
         /// <returns></returns>
         public  MySqlCommand getSqlCommand(String sql)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(sql, sqlConn);
            return mySqlCommand;
        }

        public  void run(string sql,excuteSql<MySqlCommand> exec)
        {
            MySqlCommand mySqlCommand = getSqlCommand(sql);
            sqlConn.Open();
            try
            {
                exec(mySqlCommand);
            }
            catch (MySqlException)
            {
            	
            }
            finally
            {
                sqlConn.Close();

            }
        }

        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        public void getListBysql(MySqlCommand sqlCommand)
        {
            pathList = new List<string>();
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        //Console.WriteLine(reader["filePath"].ToString());
                        pathList.Add(reader["filePath"].ToString());
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
        public  void getInsert(MySqlCommand sqlCommand)
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
        public  void getUpdate(MySqlCommand sqlCommand)
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
        public  void getDel(MySqlCommand sqlCommand)
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

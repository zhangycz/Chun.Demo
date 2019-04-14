using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.PhraseHtml
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
            const string StringKey = "SqlString";

            string ConnectionString = ConfigurationManager.ConnectionStrings[StringKey].ConnectionString;
            
            sqlConn = new SqlConnection(ConnectionString);


        }
        /// <summary>
        /// 建立执行命令语句对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlCommand getSqlCommand(string sql)
        {
            return new SqlCommand(sql, sqlConn);
        }

        public void run(string sql, excuteSql<SqlCommand> exec)
        {
            SqlCommand SqlCommand = getSqlCommand(sql);
            sqlConn.Open();
            try
            {
                exec(SqlCommand);
            }catch(SqlException ex)
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
        /// <param name="SqlCommand"></param>
        public void getListBysql(SqlCommand SqlCommand)
        {
            pathList = new List<string>();
            SqlDataReader reader = SqlCommand.ExecuteReader();
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
        /// <param name="SqlCommand"></param>
        public void getInsert(SqlCommand SqlCommand)
        {
            try

            {
                SqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("插入数据失败了！" + message);
            }

        }
        /// <summary>
        /// 修改数据 
        /// </summary>
        /// <param name="SqlCommand"></param>
        public void getUpdate(SqlCommand SqlCommand)
        {
            try

            {
                SqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("修改数据失败了！" + message);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="SqlCommand"></param>
        public void getDel(SqlCommand SqlCommand)
        {
            try
            {
                SqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                String message = ex.Message; Console.WriteLine("删除数据失败了！" + message);
            }
        }

        
    }
}

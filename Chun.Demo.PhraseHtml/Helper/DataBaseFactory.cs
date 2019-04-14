using Chun.Demo.ICommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.PhraseHtml
{
    public static class DataBaseFactory 
    {
        const string StringKey = "SqlString";
       
        public static  SqlConnection sqlConn
        {
            get;
            set;
        }

        public static SqlConnection CreateDataBase()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings[StringKey].ConnectionString;
            if (sqlConn != null)
            {
                return sqlConn;
            }
           sqlConn = new SqlConnection(ConnectionString);
           return sqlConn;
            
        } 
    }
}

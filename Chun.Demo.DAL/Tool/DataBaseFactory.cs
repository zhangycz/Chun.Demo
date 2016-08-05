using System.Configuration;
using System.Data.SqlClient;

namespace Chun.Demo.DAL
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

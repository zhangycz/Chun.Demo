using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.ICommon
{
    public delegate void excuteSql<T>(T mycommand);
    public interface ISql<T,U>
    {
        U SqlConn
        {
            get;
            set;
        }
        List<string> PathList
        {
            get;
            set;
        }
        void Run(string sql,excuteSql<T> exec);

        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        void GetListBysql(T sqlCommand);

        T GetSqlCommand(string sql);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        void GetInsert(T sqlCommand);

        /// <summary>
        /// 修改数据 
        /// </summary>
        /// <param name="sqlCommand"></param>
         void GetUpdate(T sqlCommand);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sqlCommand"></param>
         void GetDel(T sqlCommand);
      


    }
}
 
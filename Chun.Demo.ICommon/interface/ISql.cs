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
        U sqlConn
        {
            get;
            set;
        }
        List<string> pathList
        {
            get;
            set;
        }
        void run(String sql,excuteSql<T> exec);

        /// <summary>
        /// 查询并获得结果集并遍历
        /// </summary>
        /// <param name="sqlCommand"></param>
        void getListBysql(T sqlCommand);

        T getSqlCommand(string sql);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sqlCommand"></param>
        void getInsert(T sqlCommand);

        /// <summary>
        /// 修改数据 
        /// </summary>
        /// <param name="sqlCommand"></param>
         void getUpdate(T sqlCommand);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sqlCommand"></param>
         void getDel(T sqlCommand);
      


    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.ICommon
{
    public interface IBaseDataQuery<T> where T:class
    {
        T Add ( T model );
        T Update ( T model );
        void Delete ( T model );
        //按主键删除,keyValues是主键值  
        void Delete ( params object[] keyValues );
        //keyValues是主键值  
        T Find ( params object[] keyValues );
        List<T> FindAll ( );
    }
}

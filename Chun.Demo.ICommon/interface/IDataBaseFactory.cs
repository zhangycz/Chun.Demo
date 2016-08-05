using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.ICommon
{
    public interface IDataBaseFactory<T>
    {
        T sqlConn
        {
            get;
            set;

        }
        T CreateDataBase();

    }
}

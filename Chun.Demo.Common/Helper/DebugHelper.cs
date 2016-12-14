/**************Code Info**************************
* 文 件 名： DebugHelper
* 创 建 人： Zhengp
* 创建日期：2012/8/12 14:53:00
* 修 改 人：
* 修改日期：
* 备注描述：DebugHelper
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Chun.Demo.Common
{
    public class DebugHelper
    {
        DebugHelper()
        {
        }
        [Conditional("DEBUG")]
        public static void Out(string debugInfo)
        {
            Trace.WriteLine(debugInfo);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.Common
{
    public static  class ExtendTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="repalceStr"></param>
        /// <returns></returns>
        public static string MyReplace(this string str, string repalceStr)
        {
            var splitStrs = repalceStr.Split(',');

            splitStrs.ToList().ForEach(splitStr => {
                str = str.Replace(splitStr, "");
            });
            return str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Chun.Demo.Common
{
    /// <summary>
    /// 文本文件读取
    /// </summary>
    public class TxtFileRead
    {
        public StreamReader ReadFile(string fileName)
        {
            return new StreamReader(fileName, System.Text.Encoding.Default);
        }
    }
}

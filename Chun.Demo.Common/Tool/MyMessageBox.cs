using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.Common
{
    public delegate void MessageBoxHandler();

    public static class MyMessageBox
    {
        private static string MessageBuilder { get; set; }
    
        public static event MessageBoxHandler MessageBoxEvent;
        public static void Add(string appendStr)
        {
            MessageBuilder=appendStr;
            MessageBoxEvent?.Invoke();
        }

         public static String GetMessageBuilder()=> MessageBuilder;

    }
}

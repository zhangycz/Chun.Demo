using System;
namespace Chun.Demo.Common
{

    public static class MyMessageBox
    {
        private static string MessageBuilder { get; set; }
        
        public static Action<string> MessageBoxEvent;

        public static void Add(string appendStr)
        {
            MessageBuilder=appendStr;
            MessageBoxEvent?.Invoke(MessageBuilder);
        }

    }
}

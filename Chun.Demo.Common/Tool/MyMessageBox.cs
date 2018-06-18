using System;
namespace Chun.Demo.Common
{

    public static class MyMessageBox
    {
        private static string MessageBuilder { get; set; }

        public static Action MessageBoxEvent;

        public static void Add(string appendStr)
        {
            MessageBuilder=appendStr;
            MessageBoxEvent?.Invoke();
        }

         public static string GetMessageBuilder()=> MessageBuilder;

    }
}

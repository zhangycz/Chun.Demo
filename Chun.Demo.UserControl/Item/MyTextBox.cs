using System;
using System.Windows.Forms;

namespace Chun.Demo.myUserControl
{
    public static class MyTextBox
    {
        private static string format = string.Empty;

        public static void Formart(this TextBox textBox, string formart)
        {
            textBox.KeyPress += textBox_KeyPressEvent;
            textBox.Leave += textBox_Changed;
            format = formart;
        }

        private static void textBox_Changed(object sender, EventArgs args)
        {
            var textBox = sender as TextBox;
            textBox.Text = Convert.ToDecimal(textBox.Text).ToString(format);
        }

        private static void textBox_KeyPressEvent(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == (char) Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chun.Demo.UserControl
{
    public static class MyTextBox
    {
        public static void Formart(this TextBox textBox,string formart)
        {
            textBox.KeyPress +=textBox_KeyPressEvent;
            textBox.Leave += textBox_Changed;
            format = formart;
        }

        private static string format = string.Empty;
        private static void textBox_Changed(object sender,EventArgs args)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = Convert.ToDecimal(textBox.Text).ToString(format);
           
        }

        private static void textBox_KeyPressEvent(object sender, KeyPressEventArgs args)
        {
            if (args.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chun.Demo.myUserControl
{
    public partial class OpenFileFolder : System.Windows.Forms.UserControl
    {
        #region 属性
        
        public TextBox TextBox
        {
            get { return textBox1; }
            set { value = textBox1; }
        }

        #endregion
        public OpenFileFolder()
        {
            InitializeComponent();
        }

        private void OpenFileFolder_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    TextBox.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.Common;

namespace Chun.Demo.VIEW
{
    public partial class AddPictureForm : Form
    {
        public AddPictureForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var openDig = new MyFolderBrowserDialog();
            if (openDig.ShowDialog(this) != DialogResult.OK)
                return;
            var path = openDig.DirectoryPath;
            if (Directory.Exists(path)) {
                
            }
        }
    }
}

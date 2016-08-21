using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chun.Demo.VIEW
{
    public partial class MyBroswer : Form
    {
        public MyBroswer()
        {
            InitializeComponent();
        }

        private void UrlTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string urlHead = @"Http://";
                string urlstr = UrlTB.Text.Trim();
                if (string.IsNullOrEmpty(urlstr))
                    return;
                if (!urlstr.StartsWith(urlHead))
                    urlstr = urlHead + urlstr;
                Uri url = new Uri(urlstr);
                webBrowser1.Url = url;
              //  webBrowser1.Navigate(url);
            }
        }
    }
}

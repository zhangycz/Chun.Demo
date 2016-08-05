using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.AnalyzeTorrent;

namespace Chun.Demo.VIEW
{
    public partial class AlterTorrentByInnerName : Form
    {
        object _locker;

        private List<Torrent> tlList { get; set; } = new List<Torrent>();
        

        public AlterTorrentByInnerName()
        {
            InitializeComponent();
        }
        public AlterTorrentByInnerName(string[] filePaths)
        {
            InitializeComponent();
            try
            {
                foreach (var item in filePaths)
                {
                    var tor = new Torrent(item);
                    tlList.Add(tor);
                }

                torrentBindingSource.DataSource = tlList;
                //Parallel.ForEach(filePaths, item =>
                //{

                //    if (Tool.ChangFileName(item, @"C:\Users\a2863\Desktop\种子", ".TORRENT"))
                //    {
                //        lock (Locker)
                //            if (currentCount < maxCount - loseCount)
                //                currentCount++;
                //    }
                //    else
                //    {
                //        lock (Locker)
                //            loseCount++;
                //    }
                //});
                //this.Invoke(new MethodInvoker(( ) => MessageBox.Show("successful")));
            }
            catch (Exception ex)
            {
               // Invoke(new MethodInvoker(() => MessageBox.Show(Resources.MainForm_打开文件ToolStripMenuItem_Click_)));
            }
        }

        public object Locker
        {
            get
            {
                return _locker;
            }

            set
            {
                _locker = value;
            }
        }
    }
}

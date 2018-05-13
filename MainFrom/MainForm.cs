using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using Chun.Demo.PhraseHtml;
using MainFrom.Properties;
using Chun.Demo.Model;

namespace MainFrom
{
    public delegate void GetAddressAndMath();

    public partial class MainForm : Form
    {
        private int _fileTypeId;

        private Task _myTask;
        private int _currentCount;

        private IGetService _igetsrv;

        
        public object locker = new object();
        private int loseCount;

        private int maxCount;

        public MainForm()
        {
            InitializeComponent();
        }

       
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            //new AlterTorrentByInnerName(this.openFileDialog.FileNames).Show();
            //return; 

            timer1.Start();
            _currentCount = 0;
            loseCount = 0;
            maxCount = openFileDialog.FileNames.Length;

            ThreadPool.QueueUserWorkItem(w =>
            {
                try
                {
                    Parallel.ForEach(openFileDialog.FileNames, item =>
                    {
                        if (Tool.ChangFileName(item, @"C:\Users\a2863\Desktop\种子", ".TORRENT"))
                        {
                            lock (locker)
                                if (_currentCount < maxCount - loseCount)
                                    _currentCount++;
                        }
                        else
                        {
                            lock (locker)
                                loseCount++;
                        }
                    });
                }
                catch (Exception)
                {
                    Invoke(new MethodInvoker(() => MessageBox.Show(Resources.MainForm_打开文件ToolStripMenuItem_Click_)));
                }
            }, null);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            _fileTypeId = 1;
            _fileTypeId = 11;
            //获取目录
            GetPath();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _fileTypeId = 12;
            GetPath();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _fileTypeId = Convert.ToInt32(ConfigerHelper.GetAppConfig("FilePathId"));
            Download();
        }

        private void Download()
        {
            if (!backgroundWorker1.IsBusy)
            {
                _igetsrv = new DownLoadPic
                {
                    SaveFilePath = SaveTextBox.Text.Trim(),
                    BasePath = BasePathTextBox.Text.Trim(),
                    FileXpath = fileXpath.Text.Trim(),
                    NetPath = AddressTextBox.Text.Trim(),
                    PropertyName = PropertyName.Text.Trim()
                };
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("正在进行其他操作");
            }
        }

        private void GetPath()
        {
            #region Task实现

            _igetsrv = new GetPath
            {
                SaveFilePath = SaveTextBox.Text.Trim(),
                //BasePath = BasePathTextBox.Text.Trim(),
               // FileXpath = fileXpath.Text.Trim(),
                //NetPath = AddressTextBox.Text.Trim(),
               // PropertyName = PropertyName.Text.Trim()
            };
            if (_myTask != null && !_myTask.IsCompleted)
            {
                Invoke(new MethodInvoker(() => MessageBox.Show("正在执行操作请耐心等待")));
                return;
            }
            _myTask = Task.Factory.StartNew(() =>
            {
                Invoke(new MethodInvoker(() => MessageBox.Show("正在执行操作请耐心等待……")));
                _igetsrv.GetService(_fileTypeId);
                Invoke(new MethodInvoker(() => MessageBox.Show("完成了操作")));
            });
            #endregion

            #region Backgroudworker 实现

            //if (!backgroundWorker1.IsBusy)
            //{

            #region 线程池实现异步

            //线程池实现异步
            //ThreadPool.QueueUserWorkItem(item =>
            //{
            //Igetsrv = new GetPath( );
            //Igetsrv._saveFilePath = this.SaveTextBox.Text.Trim( );
            //Igetsrv._basePath = this.BasePathTextBox.Text.Trim( );
            //Igetsrv._fileXpath = this.fileXpath.Text.Trim( );
            //Igetsrv._netPath = this.AddressTextBox.Text.Trim( );
            //Igetsrv._PropertyName = this.PropertyName.Text.Trim( );
            //Igetsrv.GetService(_file_type_id);
            //Invoke(new MethodInvoker(( ) => MessageBox.Show("完成了操作")));
            //backgroundWorker1.RunWorkerAsync( );
            //}
            //);
            //Invoke(new MethodInvoker(( ) => MessageBox.Show("正在执行操作请耐心等待"))); 

            #endregion

            //}
            //    else
            //    {
            //        MessageBox.Show("正在进行其他操作");
            //    } 

            #endregion
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                _igetsrv.GetService(_fileTypeId);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e) {
            MessageBox.Show(!e.Cancelled ? "正常完成了操作" : "用户取消了操作");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(item =>
                {
                    Tool.DelEmptyDirAndFile(DelEmptyFile.Text.Trim());
                    Invoke(new MethodInvoker(() => MessageBox.Show("完成！你可能需要执行多次以删除空文件夹！")));
                }
            );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private static void SayHello(params object[] args)
        {
            var inputType = string.Empty;
            var returnExs = new List<string>();
            var queryId = 0;
            var operOpenerWindow = new OpenerWindow();
            foreach (var arg in args)
            {
                inputType = arg.GetType().Name.ToUpper();
                switch (inputType)
                {
                    case "STRING":
                    {
                        returnExs[0] = arg.ToString();
                        break;
                    }
                    case "INT32":
                        queryId = Convert.ToInt32(arg);
                        break;
                    case "STRING[]":
                        foreach (var singlearg in (object[]) arg)
                        {
                            returnExs.Add(singlearg.ToString());
                        }
                        break;
                    case "INT32[]":
                        Console.WriteLine(Resources.MainForm_SayHello_输入的是int__);
                        break;
                    case "OpenerWindow":
                        break;
                }
            }
            operOpenerWindow.QueryId = queryId;

            foreach (var returnEx in returnExs)
            {
                operOpenerWindow.ReturnExs.Add(returnEx);
            }

            Console.WriteLine("完成！");
        }

        private void SetMessageBox()
        {
            BeginInvoke(new MethodInvoker(() => {
                var msg = MyMessageBox.GetMessageBuilder() + Environment.NewLine;
                Console.WriteLine(msg);
                try {
                    LogTools.LogInfo(msg);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                
                textBox1.AppendText(msg);
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var x = this.htmlModelBindingSource.DataSource;
            //TextBoxHelper tb = new TextBoxHelper();
            //tb.TextBoxInputOnlyFloatNum(testBOX);
            //testBOX.Formart("F4");

            //new MyBroswer().Show(); 

            #region 测试可变参数

            //OpenerWindow oper1 = new OpenerWindow();
            //SayHello(2, new[] { "1", "2" }, oper1, new List<OpenerWindow>()); 

            #endregion

            #region 测试ping类

            //String path = "www.baidu.com";
            //Ping p1 = new Ping( ); //只是演示，没有做错误处理
            //PingReply reply;
            //StringBuilder sbuilder = new StringBuilder( );
            //try
            //{
            //    reply = p1.Send(path);
            //    sbuilder.Append(string.Format("Address: {0} ", reply.Address.ToString( )));
            //    sbuilder.Append(string.Format("RoundTrip time: {0} ", reply.RoundtripTime));
            //    sbuilder.Append(string.Format("Time to live: {0} ", reply.Options.Ttl));
            //    sbuilder.Append(string.Format("Don't fragment: {0} ", reply.Options.DontFragment));
            //    sbuilder.Append(string.Format("Buffer size: {0} ", reply.Buffer.Length));
            // Console.WriteLine(sbuilder.ToString( ));
            //}
            //catch (PingException pe)
            //{
            //    Console.WriteLine(pe.Message, pe.Data);
            //} 
            //String path = "http://m2.urrpic.info/img/upload/image/20160423/4230002730.jpg";
            //String stat = ConnectionStatusTool.CheckServeStatus(path);
            //Console.WriteLine("网络状态：{0} ",stat);

            #endregion

            #region 测试字符串拆分

            //string createDir = "宝贝别哭，我会温柔的对待你的[14P] | 自拍偷拍 | 圖文欣賞 - xp1024.com - 1024核工厂";
            //createDir = createDir.ToCharArray( ).Where(ch => !(@"\/*|:?*<> ".ToCharArray( ).Contains(ch))).Aggregate(string.Empty, ( f, ch ) => f + ch);
            //Console.WriteLine(createDir); 

            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MyMessageBox.MessageBoxEvent += SetMessageBox;

            //获取目录地址
            var basicUrl = ConfigerHelper.GetAppConfig("BasicUrl");
            var savePath = ConfigerHelper.GetAppConfig("SavePath");
            var hm = new FormPars(basicUrl, "thread.php?fid=16&page=", "//div[@class='tpc_content']/img",
                "src", savePath, "");
            MyTools.FormPars = hm;
            this.htmlModelBindingSource.DataSource = MyTools.FormPars;

            this.BasePathTextBox.DataBindings.Add(new Binding("Text", this.htmlModelBindingSource, "BasePath", true, DataSourceUpdateMode.OnPropertyChanged));
            this.BasePathTextBox.DataBindings.Add(new Binding("Tag", this.htmlModelBindingSource, "BasePath", true, DataSourceUpdateMode.OnPropertyChanged));

            this.AddressTextBox.DataBindings.Add(new Binding("Text", this.htmlModelBindingSource, "ExtendPath", true, DataSourceUpdateMode.OnPropertyChanged));
            this.AddressTextBox.DataBindings.Add(new Binding("Tag", this.htmlModelBindingSource, "ExtendPath", true, DataSourceUpdateMode.OnPropertyChanged));

            this.fileXpath.DataBindings.Add(new Binding("Text", this.htmlModelBindingSource, "Match", true, DataSourceUpdateMode.OnPropertyChanged));
            this.fileXpath.DataBindings.Add(new Binding("Tag", this.htmlModelBindingSource, "Match", true, DataSourceUpdateMode.OnPropertyChanged));

            this.PropertyName.DataBindings.Add(new Binding("Text", this.htmlModelBindingSource, "AttrName", true, DataSourceUpdateMode.OnPropertyChanged));
            this.PropertyName.DataBindings.Add(new Binding("Tag", this.htmlModelBindingSource, "AttrName", true, DataSourceUpdateMode.OnPropertyChanged));

            this.SaveTextBox.DataBindings.Add(new Binding("Text", this.htmlModelBindingSource, "SavePath", true, DataSourceUpdateMode.OnPropertyChanged));
            this.startDateTime.DataBindings.Add(new Binding("Value", this.htmlModelBindingSource, "StartDateTime", true, DataSourceUpdateMode.OnPropertyChanged));
            this.EndDateTime.DataBindings.Add(new Binding("Value", this.htmlModelBindingSource, "EndDateTime", true, DataSourceUpdateMode.OnPropertyChanged));
            this.IgnoreFailed.DataBindings.Add(new Binding("Checked", this.htmlModelBindingSource, "IgnoreFailed", true, DataSourceUpdateMode.OnPropertyChanged));
            MyTools.FormPars.StartDateTime =DateTime.Now;
            MyTools.FormPars.EndDateTime =DateTime.MaxValue;
        }

        private delegate void test(string fileName);

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e) {
            var SavePath = MyTools.FormPars.SavePath;
            PathTools.OpenDir(SavePath);
        }
    }
}
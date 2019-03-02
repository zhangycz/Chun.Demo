using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.Common;
using Chun.Demo.ICommon;
using Chun.Demo.PhraseHtml;
using MainFrom.Properties;
using Chun.Demo.Model;
using Chun.Demo.VIEW;

namespace MainFrom
{
    public delegate void GetAddressAndMath();

    public partial class MainForm : Form
    {
        private int _fileTypeId;

        private Task _myTask;
        private int _currentCount;

        private IGetService _igetsrv;


        private object locker = new object();
        private int loseCount;

        private int maxCount;

        public IGetService Getsrv { get => _igetsrv; set => _igetsrv = value; }

        public MainForm()
        {
            InitializeComponent();
        }

       
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

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


        private void button1_Click(object sender, EventArgs e) {
            fileXpath.SelectedIndex = 1;
            PropertyName.SelectedIndex = 1;
            _fileTypeId = 1;
            _fileTypeId = 11;
            //获取目录
            GetPath();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fileXpath.SelectedIndex = 0;
            PropertyName.SelectedIndex = 0;
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
                Getsrv = new DownLoadPic();
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(Resources.DoOtherWork);
            }
        }

        private void GetPath()
        {
            #region Task实现

            Getsrv = new GetPath();
            if (_myTask != null && !_myTask.IsCompleted)
            {
                Invoke(new MethodInvoker(() => MessageBox.Show(Resources.IsRunning)));
                return;
            }
            _myTask = Task.Factory.StartNew(() =>
            {
                Invoke(new MethodInvoker(() => MessageBox.Show(Resources.IsRunning)));
                Getsrv.GetService(_fileTypeId);
                Invoke(new MethodInvoker(() => MessageBox.Show(Resources.Completed)));
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
            //Igetsrv._saveFilePath = SaveTextBox.Text.Trim( );
            //Igetsrv._basePath = BasePathTextBox.Text.Trim( );
            //Igetsrv._fileXpath = fileXpath.Text.Trim( );
            //Igetsrv._netPath = AddressTextBox.Text.Trim( );
            //Igetsrv._PropertyName = PropertyName.Text.Trim( );
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

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                Getsrv.GetService(_fileTypeId);
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
                    Invoke(new MethodInvoker(() => MessageBox.Show(Resources.DeletedDirDone)));
                }
            );
        }

        internal void Button4_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private static void SayHello(params object[] args)
        {
            var returnExs = new List<string>();
            var queryId = 0;
            var operOpenerWindow = new OpenerWindow();
            foreach (var arg in args) {
                var inputType = arg.GetType().Name.ToUpper();
                switch (inputType) {
                    case "STRING":
                        returnExs[0] = arg.ToString();
                        break;
                    case "INT32":
                        queryId = Convert.ToInt32(arg);
                        break;
                    case "STRING[]":
                        returnExs.AddRange(from singlearg in (object[]) arg select singlearg.ToString());
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

            Console.WriteLine(Resources.Completed);
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

        private void button6_Click(object sender, EventArgs e) {

            var x = new AddPictureForm();
            x.Show();
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
            //var hm = new FormPars(basicUrl, "thread.php?mod=viewthread&fid=16&page=", "//div[@class='tpc_content']/img",
            var hm = new FormPars(basicUrl, "pw/thread.php?fid=16&page=", "//div[@class='tpc_content']/img",
                "src", savePath, "","");
            MyTools.FormPars = hm;
            htmlModelBindingSource.DataSource = MyTools.FormPars;
            
            BasePathTextBox.DataBindings.Add(new Binding("Text",  htmlModelBindingSource, "BasePath", true, DataSourceUpdateMode.OnPropertyChanged));
            BasePathTextBox.DataBindings.Add(new Binding("Tag",  htmlModelBindingSource, "BasePath", true, DataSourceUpdateMode.OnPropertyChanged));
            
            AddressTextBox.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "ExtendPath", true, DataSourceUpdateMode.OnPropertyChanged));
            AddressTextBox.DataBindings.Add(new Binding("Tag",htmlModelBindingSource, "ExtendPath", true, DataSourceUpdateMode.OnPropertyChanged));
            
            fileXpath.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "Match", true, DataSourceUpdateMode.OnPropertyChanged));
            fileXpath.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "Match", true, DataSourceUpdateMode.OnPropertyChanged));
            
            PropertyName.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "AttrName", true, DataSourceUpdateMode.OnPropertyChanged));
            PropertyName.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "AttrName", true, DataSourceUpdateMode.OnPropertyChanged));
            
            SaveTextBox.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "SavePath", true, DataSourceUpdateMode.OnPropertyChanged));
            startDateTime.DataBindings.Add(new Binding("Value", htmlModelBindingSource, "StartDateTime", true, DataSourceUpdateMode.OnPropertyChanged));
            EndDateTime.DataBindings.Add(new Binding("Value", htmlModelBindingSource, "EndDateTime", true, DataSourceUpdateMode.OnPropertyChanged));
            IgnoreFailed.DataBindings.Add(new Binding("Checked", htmlModelBindingSource, "IgnoreFailed", true, DataSourceUpdateMode.OnPropertyChanged));

            typeText.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "PicType", true, DataSourceUpdateMode.OnPropertyChanged));

            MyTools.FormPars.StartDateTime =DateTime.Now;
            MyTools.FormPars.EndDateTime =DateTime.MaxValue;
        }

        private delegate void Test(string fileName);

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Button7_Click(object sender, EventArgs e) {
            var savePath = MyTools.FormPars.SavePath;
            PathTools.OpenDir(savePath);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.Common;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using Chun.Demo.PhraseHtml;
using Chun.Demo.PhraseHtml.Implement;
using Chun.Demo.VIEW;
using Chun.Work.Common.Helper;
using MainForm.Properties;

namespace MainForm
{
    public partial class MainForm : Form
    {
        private int _currentCount;

        /// <summary>
        ///     下载进程
        /// </summary>
        private Thread _downloadThread;

        /// <summary>
        ///     获取目录、文件线程
        /// </summary>
        private Thread _getThread;

        private int _loseCount;

        private int _maxCount;


        private readonly object locker = new object();

        public MainForm() {
            InitializeComponent();
        }

        private PhraseHtmlType PhraseHtmlType { get; set; }

        private LogForm LogForm { get; set; }

        private IGetService Getsrv { get; set; }


        private void ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            timer1.Start();
            _currentCount = 0;
            _loseCount = 0;
            _maxCount = openFileDialog.FileNames.Length;

            ThreadPool.QueueUserWorkItem(w => {
                try {
                    Parallel.ForEach(openFileDialog.FileNames, item => {
                        if (Tool.ChangFileName(item, @"C:\Users\a2863\Desktop\种子", ".TORRENT")) {
                            lock (locker) {
                                if (_currentCount < _maxCount - _loseCount)
                                    _currentCount++;
                            }
                        }
                        else {
                            lock (locker) {
                                _loseCount++;
                            }
                        }
                    });
                }
                catch (Exception) {
                    Invoke(new MethodInvoker(() => MessageBox.Show(Resources.MainForm_打开文件ToolStripMenuItem_Click_)));
                }
            }, null);
        }


        private void button1_Click(object sender, EventArgs e) {
            fileXpath.SelectedIndex = 1;
            PropertyName.SelectedIndex = 1;
            PhraseHtmlType = PhraseHtmlType.Dir;
            //获取目录
            GetPath();
        }

        private void button2_Click(object sender, EventArgs e) {
            fileXpath.SelectedIndex = 0;
            PropertyName.SelectedIndex = 0;
            PhraseHtmlType = PhraseHtmlType.Img;
            GetPath();
        }

        private void button3_Click(object sender, EventArgs e) {
            PhraseHtmlType = PhraseHtmlType.Img;
            Download();
        }

        private void Download() {
            if (_downloadThread != null && _downloadThread.IsAlive) {
                MessageBox.Show(Resources.DoOtherWork);
            }
            else {
                Getsrv = new DownloadService() {
                    SiteInfo = new Xp1024PageInfo() {
                        BaseUrl = MyTools.FormPars.BasePath,
                        ExtendUrl = MyTools.FormPars.ExtendPath,
                        Type = MyTools.FormPars.PicType,
                        TargetMatch = MyTools.FormPars.Match,
                        ExtendMatch = @"//head/title",
                        AttrName = MyTools.FormPars.AttrName,
                        Encoding = Encoding.UTF8
                    }
                };
                Getsrv.OnCompleted += () => {
                    Invoke(new MethodInvoker(() => MessageBox.Show(Resources.Completed)));
                    StopGetThread(_downloadThread);
                };
                StartDownloadThread();
            }
        }


        private void StartGetThread() {
            ThreadHelper.StartThread(() => {
                Invoke(new MethodInvoker(() => MessageBox.Show(string.Format(Resources.IsRunning, PhraseHtmlType))));
                Getsrv.GetService(PhraseHtmlType);
            }, ref _getThread);
        }

        private void StartDownloadThread() {
            ThreadHelper.StartThread(() => {
                Invoke(new MethodInvoker(() => MessageBox.Show(string.Format(Resources.IsRunning, PhraseHtmlType))));
                Getsrv.GetService(PhraseHtmlType);
            }, ref _downloadThread);
        }

        private void StopGetThread(Thread thread) {
            ThreadHelper.StopInsertListener(ref thread);
        }

        private void GetPath() {
            #region Task实现
            if (!PhraseHtmlConfig.ValidateHtml())
                return;

            if (_getThread != null && _getThread.IsAlive) {
                MessageBox.Show(Resources.DoOtherWork);
                return;
            }

            Getsrv = new GetFileService() {
                SiteInfo = new Xp1024PageInfo() {
                    BaseUrl = MyTools.FormPars.BasePath,
                    ExtendUrl = MyTools.FormPars.ExtendPath,
                    Type = MyTools.FormPars.PicType,
                    TargetMatch = MyTools.FormPars.Match,
                    ExtendMatch = @"//head/title",
                    AttrName = MyTools.FormPars.AttrName,
                    Encoding = Encoding.UTF8
                }
            };

            Getsrv.OnCompleted += () => {
                Invoke(new MethodInvoker(() => MessageBox.Show(Resources.Completed)));
                StopGetThread(_getThread);
            };

            StartGetThread();

            #endregion
        }


        private void button5_Click(object sender, EventArgs e) {
            ThreadPool.QueueUserWorkItem(item => {
                    Tool.DelEmptyDirAndFile(DelEmptyFile.Text.Trim());
                    Invoke(new MethodInvoker(() => MessageBox.Show(Resources.DeletedDirDone)));
                }
            );
        }

        private static void SayHello(params object[] args) {
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
                        returnExs.AddRange(from singleArg in (object[]) arg select singleArg.ToString());
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
                operOpenerWindow.ReturnExs.Add(returnEx);

            Console.WriteLine(Resources.Completed);
        }


        private void button6_Click(object sender, EventArgs e) {
            ParseQuery($@"{MyTools.FormPars.BasePath}/{MyTools.FormPars.ExtendPath}");
            return;
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

        private void MainForm_Load(object sender, EventArgs e) {

            //获取目录地址
            var basicUrl = ConfigerHelper.GetAppConfig("BasicUrl");
            var savePath = ConfigerHelper.GetAppConfig("SavePath");
            //var hm = new FormPars(basicUrl, "thread.php?mod=viewthread&fid=16&page=", "//div[@class='tpc_content']/img",
            var hm = new FormPars(basicUrl, "pw/thread.php?fid=16&page=", "//div[@class='tpc_content']/img",
                "src", savePath, "", "");
            MyTools.FormPars = hm;
            var htmlModelBindingSource = new BindingSource(components) {DataSource = MyTools.FormPars};

            BasePathTextBox.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "BasePath", true,
                DataSourceUpdateMode.OnPropertyChanged));
            BasePathTextBox.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "BasePath", true,
                DataSourceUpdateMode.OnPropertyChanged));

            AddressTextBox.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "ExtendPath", true,
                DataSourceUpdateMode.OnPropertyChanged));
            AddressTextBox.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "ExtendPath", true,
                DataSourceUpdateMode.OnPropertyChanged));

            fileXpath.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "Match", true,
                DataSourceUpdateMode.OnPropertyChanged));
            fileXpath.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "Match", true,
                DataSourceUpdateMode.OnPropertyChanged));

            PropertyName.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "AttrName", true,
                DataSourceUpdateMode.OnPropertyChanged));
            PropertyName.DataBindings.Add(new Binding("Tag", htmlModelBindingSource, "AttrName", true,
                DataSourceUpdateMode.OnPropertyChanged));

            SaveTextBox.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "SavePath", true,
                DataSourceUpdateMode.OnPropertyChanged));
            startDateTime.DataBindings.Add(new Binding("Value", htmlModelBindingSource, "StartDateTime", true,
                DataSourceUpdateMode.OnPropertyChanged));
            EndDateTime.DataBindings.Add(new Binding("Value", htmlModelBindingSource, "EndDateTime", true,
                DataSourceUpdateMode.OnPropertyChanged));
            IgnoreFailed.DataBindings.Add(new Binding("Checked", htmlModelBindingSource, "IgnoreFailed", true,
                DataSourceUpdateMode.OnPropertyChanged));

            typeText.DataBindings.Add(new Binding("Text", htmlModelBindingSource, "PicType", true,
                DataSourceUpdateMode.OnPropertyChanged));

            MyTools.FormPars.StartDateTime = DateTime.Now;
            MyTools.FormPars.EndDateTime = DateTime.MaxValue;
            typeText.Text = "16";
          //  LogHelper.SupportRichLog();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) {
        }

        private void Button7_Click(object sender, EventArgs e) {
            var savePath = MyTools.FormPars.SavePath;
            PathTools.OpenDir(savePath);
        }


        private void OpenLogToolStripMenuItem_Click(object sender, EventArgs e) {
            LogHelper.TraceEnter();
            try {
                var logfile = PathTools.PathCombine(MyTools.FormPars.AppPath, "Logs", "info.log");
                Process.Start("notepad++.exe", logfile);
            }
            catch (Exception ex) {
                LogHelper.Error(ex, "Open log error");
            }
            finally {
                LogHelper.TraceExit();
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e) {
            SplitContainer.Panel2Collapsed = true;
            if (LogForm == null || LogForm.IsDisposed) {
                LogForm = new LogForm {StartPosition = FormStartPosition.CenterScreen};
                LogForm.Closed += (o, args) => {
                    LogHelper.ChangeTargetControl(this, "txtLogger");
                    SplitContainer.Panel2Collapsed = false;
                };
                LogHelper.ChangeTargetControl(LogForm, "LogBox");
                LogForm.Show();
            }
            else {
                LogForm.WindowState = FormWindowState.Normal;
                LogForm.Activate();
            }
        }

        private void CancleBtn_Click(object sender, EventArgs e) {
            StopGetThread(_downloadThread);
        }

        private object ParseQuery(string url) {
            NameValueCollection queryObj = null;
            queryObj = UrlPhraseHelper.Phrase(url);
            foreach (string s in queryObj)
                LogHelper.Debug($@"{s}--{queryObj[s]}");

            var fid = UrlPhraseHelper.GetQueryParas( url);
            return queryObj;
        }

        private void BasePathTextBox_Validated(object sender, EventArgs e) {
        }

        private void AddressTextBox_Validated(object sender, EventArgs e)
        {
            try {
                var paras = UrlPhraseHelper.GetQueryParas(MyTools.FormPars.ExtendPath);
                if (paras != null) {
                    var fid = paras["fid"];
                    typeText .Text= fid;
                }
            }
            catch {

            }
        }
    }
}
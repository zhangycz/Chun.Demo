using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Demo.ICommon;
using Chun.Demo.PhraseHtml;
using Chun.Demo.Common;
using System.Threading;
using System.Net;
using MainFrom.Properties;

namespace MainFrom
{
    public delegate void getAddressAndMath ( );
    public partial class MainForm : Form
    {
        int currentCount = 0;

        int maxCount = 0;
        int loseCount = 0;

        int _file_type_id = 0;


        public object locker = new object( );

        private IGetService Igetsrv;
        public MainForm ( )
        {
            InitializeComponent( );

            //Thread th = new Thread(new ThreadStart(add));
            //th.Start();
            //Parallel.Invoke(add);

        }
        private void timer1_Tick ( object sender, EventArgs e )
        {
            if (this.progressBar1.Value == this.progressBar1.Maximum)
            {
                this.progressBar1.Value = 0;
                timer1.Stop( );
                this.Invoke(new MethodInvoker(( ) => MessageBox.Show("successful")));
            }

            this.progressBar1.Maximum = maxCount - loseCount;
            this.progressBar1.Value = currentCount;
        }
        private void add ( )
        {
            double sum = 0f;
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    sum += i * j;
                }
                currentCount++;
            }
        }

        delegate void test ( string fileName );
        private void 打开文件ToolStripMenuItem_Click ( object sender, EventArgs e )
        {

            if (this.openFileDialog.ShowDialog( ) == DialogResult.OK)
            {
                timer1.Start( );
                currentCount = 0;
                loseCount = 0;
                maxCount = this.openFileDialog.FileNames.Length;
                ThreadPool.QueueUserWorkItem(w =>
                {
                    try
                    {
                        Parallel.ForEach(this.openFileDialog.FileNames, item =>
                        {
                            if (Tool.ChangFileName(item, @"C:\Users\a2863\Desktop\种子", ".TORRENT"))
                            {
                                lock (locker)
                                    if (currentCount < maxCount - loseCount)
                                        currentCount++;
                            }
                            else
                            {
                                lock (locker)
                                    loseCount++;
                            }
                        });
                        //this.Invoke(new MethodInvoker(( ) => MessageBox.Show("successful")));
                    }
                    catch (Exception)
                    {
                        Invoke(new MethodInvoker(( ) => MessageBox.Show(Resources.MainForm_打开文件ToolStripMenuItem_Click_)));
                    }

                }, null);
            }

        }


        private void button1_Click ( object sender, EventArgs e )
        {
            _file_type_id = 1;
            //获取目录
            _file_type_id = 11;
            GetPath( );
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            _file_type_id = 2;
            GetPath( );
        }

        private void button3_Click ( object sender, EventArgs e )
        {

            _file_type_id = 2;
            Download( );
        }
        private void Download ( )
        {
            if (!backgroundWorker1.IsBusy)
            {
                Igetsrv = new DownLoadPic
                {
                    SaveFilePath = this.SaveTextBox.Text.Trim(),
                    BasePath = this.BasePathTextBox.Text.Trim(),
                    FileXpath = this.fileXpath.Text.Trim(),
                    NetPath = this.AddressTextBox.Text.Trim(),
                    PropertyName = this.PropertyName.Text.Trim()
                };
                backgroundWorker1.RunWorkerAsync( );
            }
            else
            {
                MessageBox.Show("正在进行其他操作");
            }
        }

        private Task _myTask;
        private void GetPath ( )
        {
            #region Task实现
            Igetsrv = new GetPath( );
            Igetsrv.SaveFilePath = this.SaveTextBox.Text.Trim( );
            Igetsrv.BasePath = this.BasePathTextBox.Text.Trim( );
            Igetsrv.FileXpath = this.fileXpath.Text.Trim( );
            Igetsrv.NetPath = this.AddressTextBox.Text.Trim( );
            Igetsrv.PropertyName = this.PropertyName.Text.Trim( );
            if (_myTask != null && !_myTask.IsCompleted)
            {
                Invoke(new MethodInvoker(( ) => MessageBox.Show("正在执行操作请耐心等待")));
                return;
            }
            _myTask = Task.Factory.StartNew(( ) =>
            {
                Invoke(new MethodInvoker(( ) => MessageBox.Show("正在执行操作请耐心等待……")));
                Igetsrv.GetService(_file_type_id);
                Invoke(new MethodInvoker(( ) => MessageBox.Show("完成了操作")));
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

        private void backgroundWorker1_DoWork ( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                Igetsrv.GetService(_file_type_id);
            }

        }

        private void button4_Click ( object sender, EventArgs e )
        {

            backgroundWorker1.CancelAsync( );
        }

        private void backgroundWorker1_RunWorkerCompleted ( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e )
        {
            if (!e.Cancelled)
                MessageBox.Show("正常完成了操作");
            else
                MessageBox.Show("用户取消了操作");
        }

        private void button5_Click ( object sender, EventArgs e )
        {
            ThreadPool.QueueUserWorkItem(item =>
           {
               Tool.DelEmptyDirAndFile(this.DelEmptyFile.Text.Trim( ));
               Invoke(new MethodInvoker(( ) => MessageBox.Show("完成！你可能需要执行多次以删除空文件夹！")));
           }
           );

        }

        static void SayHello(params object[] args)
        {
            string inputType = string.Empty;
            List<string> returnExs = new List<string>();
            int queryId = 0;
            OpenerWindow operOpenerWindow = new OpenerWindow();
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

        private void button6_Click ( object sender, EventArgs e )
        {
            OpenerWindow oper1 = new OpenerWindow();
            SayHello(2,new[]{"1","2"}, oper1,new List<OpenerWindow>());

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
    }
}

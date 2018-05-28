
namespace MainFrom
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            button1 = new System.Windows.Forms.Button();
            AddressTextBox = new System.Windows.Forms.TextBox();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            button4 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            IgnoreFailed = new System.Windows.Forms.CheckBox();
            EndDateTime = new System.Windows.Forms.DateTimePicker();
            label7 = new System.Windows.Forms.Label();
            startDateTime = new System.Windows.Forms.DateTimePicker();
            button6 = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            button5 = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            SaveTextBox = new System.Windows.Forms.TextBox();
            DelEmptyFile = new System.Windows.Forms.TextBox();
            fileXpath = new System.Windows.Forms.ComboBox();
            PropertyName = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            BasePathTextBox = new System.Windows.Forms.TextBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            htmlModelBindingSource = new System.Windows.Forms.BindingSource(components);
            button7 = new System.Windows.Forms.Button();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(htmlModelBindingSource)).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog2";
            openFileDialog.Filter = "\"Torrent文件|*.torrent|所有文件|*.*\"";
            openFileDialog.Multiselect = true;
            openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            文件ToolStripMenuItem,
            设置ToolStripMenuItem});
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1284, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            打开文件ToolStripMenuItem});
            文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开文件ToolStripMenuItem
            // 
            打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            打开文件ToolStripMenuItem.ShortcutKeyDisplayString = "";
            打开文件ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            打开文件ToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
            打开文件ToolStripMenuItem.Text = "打开文件";
            打开文件ToolStripMenuItem.Click += new System.EventHandler(打开文件ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            目录ToolStripMenuItem});
            设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            设置ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            设置ToolStripMenuItem.Text = "设置";
            // 
            // 目录ToolStripMenuItem
            // 
            目录ToolStripMenuItem.Name = "目录ToolStripMenuItem";
            目录ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            目录ToolStripMenuItem.Text = "目录";
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button1.Location = new System.Drawing.Point(587, 25);
            button1.Margin = new System.Windows.Forms.Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(100, 29);
            button1.TabIndex = 2;
            button1.Text = "获取目录";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // AddressTextBox
            // 
            AddressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            AddressTextBox.Location = new System.Drawing.Point(179, 59);
            AddressTextBox.Margin = new System.Windows.Forms.Padding(4);
            AddressTextBox.Name = "AddressTextBox";
            AddressTextBox.Size = new System.Drawing.Size(398, 25);
            AddressTextBox.TabIndex = 4;
            AddressTextBox.Text = "http://x3.1024lualu.pw/pw/thread.php?fid=16&page=";
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button2.Location = new System.Drawing.Point(695, 25);
            button2.Margin = new System.Windows.Forms.Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(133, 29);
            button2.TabIndex = 6;
            button2.Text = "获取文件地址";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // button3
            // 
            button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button3.Location = new System.Drawing.Point(587, 61);
            button3.Margin = new System.Windows.Forms.Padding(4);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(100, 29);
            button3.TabIndex = 7;
            button3.Text = "下载文件";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(button3_Click);
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            // 
            // button4
            // 
            button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button4.Location = new System.Drawing.Point(587, 101);
            button4.Margin = new System.Windows.Forms.Padding(4);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(100, 29);
            button4.TabIndex = 8;
            button4.Text = "取消操作";
            button4.UseVisualStyleBackColor = true;
            button4.Click += new System.EventHandler(Button4_Click);
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(68, 66);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 15);
            label1.TabIndex = 9;
            label1.Text = "请输入地址：";
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(60, 98);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 15);
            label2.TabIndex = 11;
            label2.Text = "请输入XPATH：";
            // 
            // textBox1
            // 
            textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox1.Location = new System.Drawing.Point(4, 22);
            textBox1.Margin = new System.Windows.Forms.Padding(4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBox1.Size = new System.Drawing.Size(1276, 131);
            textBox1.TabIndex = 12;
            textBox1.Text = "基址：访问相对路径，地址：访问起始地址，,XPATH:查找的项目，获取项目：获取哪一项";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button7);
            groupBox1.Controls.Add(IgnoreFailed);
            groupBox1.Controls.Add(EndDateTime);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(startDateTime);
            groupBox1.Controls.Add(button6);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(SaveTextBox);
            groupBox1.Controls.Add(DelEmptyFile);
            groupBox1.Controls.Add(fileXpath);
            groupBox1.Controls.Add(PropertyName);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(BasePathTextBox);
            groupBox1.Controls.Add(AddressTextBox);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(0, 28);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(1284, 345);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "参数信息";
            // 
            // IgnoreFailed
            // 
            IgnoreFailed.AutoSize = true;
            IgnoreFailed.Location = new System.Drawing.Point(585, 189);
            IgnoreFailed.Name = "IgnoreFailed";
            IgnoreFailed.Size = new System.Drawing.Size(164, 19);
            IgnoreFailed.TabIndex = 27;
            IgnoreFailed.Text = "忽略操作失败的链接";
            IgnoreFailed.UseVisualStyleBackColor = true;
            // 
            // EndDateTime
            // 
            EndDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            EndDateTime.Location = new System.Drawing.Point(385, 189);
            EndDateTime.Name = "EndDateTime";
            EndDateTime.Size = new System.Drawing.Size(192, 25);
            EndDateTime.TabIndex = 26;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(84, 189);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(67, 15);
            label7.TabIndex = 25;
            label7.Text = "日期介于";
            // 
            // startDateTime
            // 
            startDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            startDateTime.Location = new System.Drawing.Point(179, 189);
            startDateTime.Name = "startDateTime";
            startDateTime.Size = new System.Drawing.Size(184, 25);
            startDateTime.TabIndex = 24;
            // 
            // button6
            // 
            button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button6.Location = new System.Drawing.Point(695, 61);
            button6.Margin = new System.Windows.Forms.Padding(4);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(133, 29);
            button6.TabIndex = 21;
            button6.Text = "测试用";
            button6.UseVisualStyleBackColor = true;
            button6.Click += new System.EventHandler(button6_Click);
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] {
            "获取目录",
            "获取文件地址",
            "下载文件"});
            comboBox1.Location = new System.Drawing.Point(179, 290);
            comboBox1.Margin = new System.Windows.Forms.Padding(4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(160, 23);
            comboBox1.TabIndex = 23;
            comboBox1.SelectedIndexChanged += new System.EventHandler(ComboBox1_SelectedIndexChanged);
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(585, 257);
            button5.Margin = new System.Windows.Forms.Padding(4);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(153, 25);
            button5.TabIndex = 15;
            button5.Text = "清空空文件和目录";
            button5.UseVisualStyleBackColor = true;
            button5.Click += new System.EventHandler(button5_Click);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(84, 261);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(82, 15);
            label6.TabIndex = 20;
            label6.Text = "清空目录：";
            // 
            // SaveTextBox
            // 
            SaveTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            SaveTextBox.Location = new System.Drawing.Point(179, 157);
            SaveTextBox.Margin = new System.Windows.Forms.Padding(4);
            SaveTextBox.Name = "SaveTextBox";
            SaveTextBox.Size = new System.Drawing.Size(398, 25);
            SaveTextBox.TabIndex = 22;
            SaveTextBox.Text = "J:\\Picture\\";
            // 
            // DelEmptyFile
            // 
            DelEmptyFile.Location = new System.Drawing.Point(179, 257);
            DelEmptyFile.Margin = new System.Windows.Forms.Padding(4);
            DelEmptyFile.Name = "DelEmptyFile";
            DelEmptyFile.Size = new System.Drawing.Size(398, 25);
            DelEmptyFile.TabIndex = 20;
            DelEmptyFile.Text = "J:\\Picture\\";
            // 
            // fileXpath
            // 
            fileXpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            fileXpath.FormattingEnabled = true;
            fileXpath.Items.AddRange(new object[] {
            "//div[@class=\'tpc_content\']/img",
            "//tr[@class=\'tr3 t_one\']/td/h3/a"});
            fileXpath.Location = new System.Drawing.Point(179, 94);
            fileXpath.Margin = new System.Windows.Forms.Padding(4);
            fileXpath.Name = "fileXpath";
            fileXpath.Size = new System.Drawing.Size(398, 23);
            fileXpath.TabIndex = 19;
            fileXpath.Text = "//div[@class=\'tpc_content\']/img";
            // 
            // PropertyName
            // 
            PropertyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            PropertyName.FormattingEnabled = true;
            PropertyName.Items.AddRange(new object[] {
            "src",
            "href",
            "img"});
            PropertyName.Location = new System.Drawing.Point(179, 126);
            PropertyName.Margin = new System.Windows.Forms.Padding(4);
            PropertyName.Name = "PropertyName";
            PropertyName.Size = new System.Drawing.Size(398, 23);
            PropertyName.TabIndex = 18;
            PropertyName.Text = "href";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(40, 129);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(127, 15);
            label5.TabIndex = 17;
            label5.Text = "请输入获取项目：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(53, 160);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(112, 15);
            label4.TabIndex = 15;
            label4.Text = "本地保存地址：";
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(68, 29);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(97, 15);
            label3.TabIndex = 13;
            label3.Text = "请输入基址：";
            // 
            // BasePathTextBox
            // 
            BasePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            BasePathTextBox.Location = new System.Drawing.Point(179, 25);
            BasePathTextBox.Margin = new System.Windows.Forms.Padding(4);
            BasePathTextBox.Name = "BasePathTextBox";
            BasePathTextBox.Size = new System.Drawing.Size(398, 25);
            BasePathTextBox.TabIndex = 12;
            BasePathTextBox.Text = "http://x3.1024lualu.pw";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox1);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            groupBox2.Location = new System.Drawing.Point(0, 381);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(1284, 157);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "输出信息";
            // 
            // button7
            // 
            button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button7.Font = new System.Drawing.Font("宋体", 9.5F);
            button7.ForeColor = System.Drawing.Color.DarkCyan;
            button7.Location = new System.Drawing.Point(587, 157);
            button7.Margin = new System.Windows.Forms.Padding(4);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(31, 29);
            button7.TabIndex = 28;
            button7.Text = "○";
            button7.UseVisualStyleBackColor = true;
            button7.Click += new System.EventHandler(Button7_Click);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(1284, 538);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(1278, 585);
            Name = "MainForm";
            ShowIcon = false;
            Load += new System.EventHandler(MainForm_Load);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(htmlModelBindingSource)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 目录ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BasePathTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox PropertyName;
        private System.Windows.Forms.ComboBox fileXpath;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox DelEmptyFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox SaveTextBox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.BindingSource htmlModelBindingSource;
        private System.Windows.Forms.DateTimePicker EndDateTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker startDateTime;
        private System.Windows.Forms.CheckBox IgnoreFailed;
        private System.Windows.Forms.Button button7;
    }
}
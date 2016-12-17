namespace Chun.Demo.VIEW
{
    partial class AlterTorrentByInnerName
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.torrentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openErrorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameUTF8DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.announceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.announceListDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codePageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentUTF8DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.encodingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileListDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pieceLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherUTF8DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherUrlUTF8DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.torrentBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1407, 504);
            this.splitContainer1.SplitterDistance = 377;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer2.Size = new System.Drawing.Size(1407, 377);
            this.splitContainer2.SplitterDistance = 307;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.openErrorDataGridViewTextBoxColumn,
            this.nameUTF8DataGridViewTextBoxColumn,
            this.totalLengthDataGridViewTextBoxColumn,
            this.openFileDataGridViewCheckBoxColumn,
            this.announceDataGridViewTextBoxColumn,
            this.announceListDataGridViewTextBoxColumn,
            this.createTimeDataGridViewTextBoxColumn,
            this.codePageDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn,
            this.commentUTF8DataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.encodingDataGridViewTextBoxColumn,
            this.fileListDataGridViewTextBoxColumn,
            this.pieceLengthDataGridViewTextBoxColumn,
            this.publisherDataGridViewTextBoxColumn,
            this.publisherUTF8DataGridViewTextBoxColumn,
            this.publisherUrlDataGridViewTextBoxColumn,
            this.publisherUrlUTF8DataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.torrentBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1096, 377);
            this.dataGridView1.TabIndex = 0;
            // 
            // torrentBindingSource
            // 
            this.torrentBindingSource.DataSource = typeof(Chun.Demo.AnalyzeTorrent.Torrent);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "名称";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // openErrorDataGridViewTextBoxColumn
            // 
            this.openErrorDataGridViewTextBoxColumn.DataPropertyName = "OpenError";
            this.openErrorDataGridViewTextBoxColumn.HeaderText = "开窗错误";
            this.openErrorDataGridViewTextBoxColumn.Name = "openErrorDataGridViewTextBoxColumn";
            // 
            // nameUTF8DataGridViewTextBoxColumn
            // 
            this.nameUTF8DataGridViewTextBoxColumn.DataPropertyName = "NameUTF8";
            this.nameUTF8DataGridViewTextBoxColumn.HeaderText = "名称UTF8";
            this.nameUTF8DataGridViewTextBoxColumn.Name = "nameUTF8DataGridViewTextBoxColumn";
            // 
            // totalLengthDataGridViewTextBoxColumn
            // 
            this.totalLengthDataGridViewTextBoxColumn.DataPropertyName = "TotalLength";
            this.totalLengthDataGridViewTextBoxColumn.HeaderText = "总大小";
            this.totalLengthDataGridViewTextBoxColumn.Name = "totalLengthDataGridViewTextBoxColumn";
            this.totalLengthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // openFileDataGridViewCheckBoxColumn
            // 
            this.openFileDataGridViewCheckBoxColumn.DataPropertyName = "OpenFile";
            this.openFileDataGridViewCheckBoxColumn.HeaderText = "打开文件";
            this.openFileDataGridViewCheckBoxColumn.Name = "openFileDataGridViewCheckBoxColumn";
            // 
            // announceDataGridViewTextBoxColumn
            // 
            this.announceDataGridViewTextBoxColumn.DataPropertyName = "Announce";
            this.announceDataGridViewTextBoxColumn.HeaderText = "Announce";
            this.announceDataGridViewTextBoxColumn.Name = "announceDataGridViewTextBoxColumn";
            // 
            // announceListDataGridViewTextBoxColumn
            // 
            this.announceListDataGridViewTextBoxColumn.DataPropertyName = "AnnounceList";
            this.announceListDataGridViewTextBoxColumn.HeaderText = "AnnounceList";
            this.announceListDataGridViewTextBoxColumn.Name = "announceListDataGridViewTextBoxColumn";
            // 
            // createTimeDataGridViewTextBoxColumn
            // 
            this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.HeaderText = "创建时间";
            this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
            // 
            // codePageDataGridViewTextBoxColumn
            // 
            this.codePageDataGridViewTextBoxColumn.DataPropertyName = "CodePage";
            this.codePageDataGridViewTextBoxColumn.HeaderText = "CodePage";
            this.codePageDataGridViewTextBoxColumn.Name = "codePageDataGridViewTextBoxColumn";
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            // 
            // commentUTF8DataGridViewTextBoxColumn
            // 
            this.commentUTF8DataGridViewTextBoxColumn.DataPropertyName = "CommentUTF8";
            this.commentUTF8DataGridViewTextBoxColumn.HeaderText = "CommentUTF8";
            this.commentUTF8DataGridViewTextBoxColumn.Name = "commentUTF8DataGridViewTextBoxColumn";
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "作者";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            // 
            // encodingDataGridViewTextBoxColumn
            // 
            this.encodingDataGridViewTextBoxColumn.DataPropertyName = "Encoding";
            this.encodingDataGridViewTextBoxColumn.HeaderText = "编码";
            this.encodingDataGridViewTextBoxColumn.Name = "encodingDataGridViewTextBoxColumn";
            // 
            // fileListDataGridViewTextBoxColumn
            // 
            this.fileListDataGridViewTextBoxColumn.DataPropertyName = "FileList";
            this.fileListDataGridViewTextBoxColumn.HeaderText = "文件列表";
            this.fileListDataGridViewTextBoxColumn.Name = "fileListDataGridViewTextBoxColumn";
            // 
            // pieceLengthDataGridViewTextBoxColumn
            // 
            this.pieceLengthDataGridViewTextBoxColumn.DataPropertyName = "PieceLength";
            this.pieceLengthDataGridViewTextBoxColumn.HeaderText = "文件长度";
            this.pieceLengthDataGridViewTextBoxColumn.Name = "pieceLengthDataGridViewTextBoxColumn";
            // 
            // publisherDataGridViewTextBoxColumn
            // 
            this.publisherDataGridViewTextBoxColumn.DataPropertyName = "Publisher";
            this.publisherDataGridViewTextBoxColumn.HeaderText = "发行";
            this.publisherDataGridViewTextBoxColumn.Name = "publisherDataGridViewTextBoxColumn";
            // 
            // publisherUTF8DataGridViewTextBoxColumn
            // 
            this.publisherUTF8DataGridViewTextBoxColumn.DataPropertyName = "PublisherUTF8";
            this.publisherUTF8DataGridViewTextBoxColumn.HeaderText = "发行UTF8";
            this.publisherUTF8DataGridViewTextBoxColumn.Name = "publisherUTF8DataGridViewTextBoxColumn";
            // 
            // publisherUrlDataGridViewTextBoxColumn
            // 
            this.publisherUrlDataGridViewTextBoxColumn.DataPropertyName = "PublisherUrl";
            this.publisherUrlDataGridViewTextBoxColumn.HeaderText = "发行Url";
            this.publisherUrlDataGridViewTextBoxColumn.Name = "publisherUrlDataGridViewTextBoxColumn";
            // 
            // publisherUrlUTF8DataGridViewTextBoxColumn
            // 
            this.publisherUrlUTF8DataGridViewTextBoxColumn.DataPropertyName = "PublisherUrlUTF8";
            this.publisherUrlUTF8DataGridViewTextBoxColumn.HeaderText = "发行UrlUTF8";
            this.publisherUrlUTF8DataGridViewTextBoxColumn.Name = "publisherUrlUTF8DataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(307, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // AlterTorrentByInnerName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1407, 504);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AlterTorrentByInnerName";
            this.Text = "修改种子文件信息";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.torrentBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource torrentBindingSource;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn openErrorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameUTF8DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn openFileDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn announceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn announceListDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codePageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentUTF8DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn encodingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileListDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pieceLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherUTF8DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherUrlUTF8DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
    }
}
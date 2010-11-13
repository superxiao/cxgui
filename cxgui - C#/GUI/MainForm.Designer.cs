namespace CXGUI.GUI
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.未处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressPage = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLable = new System.Windows.Forms.ToolStripStatusLabel();
            this.muxTimeLeft = new System.Windows.Forms.Label();
            this.muxTimeUsed = new System.Windows.Forms.Label();
            this.audioTimeLeft = new System.Windows.Forms.Label();
            this.audioTimeUsed = new System.Windows.Forms.Label();
            this.videoTimeLeft = new System.Windows.Forms.Label();
            this.videoTimeUsed = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.muxProgressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.audioProgressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.videoProgressBar = new System.Windows.Forms.ProgressBar();
            this.inputPage = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.profileBox = new System.Windows.Forms.ComboBox();
            this.jobItemListView = new System.Windows.Forms.ListView();
            this.stateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sourceFileColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.destinationFileColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.settingButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.delButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.listViewMenu.SuspendLayout();
            this.progressPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.inputPage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(521, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选项ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.选项ToolStripMenuItem.Text = "选项";
            this.选项ToolStripMenuItem.Click += new System.EventHandler(this.选项ToolStripMenuItemClick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
            // 
            // listViewMenu
            // 
            this.listViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.未处理ToolStripMenuItem,
            this.添加ToolStripMenuItem1,
            this.删除ToolStripMenuItem,
            this.清空ToolStripMenuItem,
            this.打开目录ToolStripMenuItem});
            this.listViewMenu.Name = "contextMenuStrip1";
            this.listViewMenu.Size = new System.Drawing.Size(153, 158);
            this.listViewMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1Opening);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.SettingButtonClick);
            // 
            // 未处理ToolStripMenuItem
            // 
            this.未处理ToolStripMenuItem.Name = "未处理ToolStripMenuItem";
            this.未处理ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.未处理ToolStripMenuItem.Text = "未处理";
            this.未处理ToolStripMenuItem.Click += new System.EventHandler(this.未处理ToolStripMenuItemClick);
            // 
            // 添加ToolStripMenuItem1
            // 
            this.添加ToolStripMenuItem1.Name = "添加ToolStripMenuItem1";
            this.添加ToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem1.Text = "添加";
            this.添加ToolStripMenuItem1.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.DelButtonClick);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.ClearButtonClick);
            // 
            // 打开目录ToolStripMenuItem
            // 
            this.打开目录ToolStripMenuItem.Name = "打开目录ToolStripMenuItem";
            this.打开目录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打开目录ToolStripMenuItem.Text = "打开目录";
            this.打开目录ToolStripMenuItem.Click += new System.EventHandler(this.打开目录ToolStripMenuItemClick);
            // 
            // progressPage
            // 
            this.progressPage.Controls.Add(this.statusStrip);
            this.progressPage.Controls.Add(this.muxTimeLeft);
            this.progressPage.Controls.Add(this.muxTimeUsed);
            this.progressPage.Controls.Add(this.audioTimeLeft);
            this.progressPage.Controls.Add(this.audioTimeUsed);
            this.progressPage.Controls.Add(this.videoTimeLeft);
            this.progressPage.Controls.Add(this.videoTimeUsed);
            this.progressPage.Controls.Add(this.stopButton);
            this.progressPage.Controls.Add(this.label8);
            this.progressPage.Controls.Add(this.label9);
            this.progressPage.Controls.Add(this.label6);
            this.progressPage.Controls.Add(this.label7);
            this.progressPage.Controls.Add(this.label5);
            this.progressPage.Controls.Add(this.label4);
            this.progressPage.Controls.Add(this.label3);
            this.progressPage.Controls.Add(this.muxProgressBar);
            this.progressPage.Controls.Add(this.label2);
            this.progressPage.Controls.Add(this.audioProgressBar);
            this.progressPage.Controls.Add(this.label1);
            this.progressPage.Controls.Add(this.videoProgressBar);
            this.progressPage.Location = new System.Drawing.Point(4, 22);
            this.progressPage.Name = "progressPage";
            this.progressPage.Padding = new System.Windows.Forms.Padding(3);
            this.progressPage.Size = new System.Drawing.Size(513, 434);
            this.progressPage.TabIndex = 1;
            this.progressPage.Text = "进度";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLable});
            this.statusStrip.Location = new System.Drawing.Point(3, 409);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(507, 22);
            this.statusStrip.TabIndex = 19;
            // 
            // statusLable
            // 
            this.statusLable.Name = "statusLable";
            this.statusLable.Size = new System.Drawing.Size(0, 17);
            // 
            // muxTimeLeft
            // 
            this.muxTimeLeft.Location = new System.Drawing.Point(183, 247);
            this.muxTimeLeft.Name = "muxTimeLeft";
            this.muxTimeLeft.Size = new System.Drawing.Size(100, 23);
            this.muxTimeLeft.TabIndex = 18;
            // 
            // muxTimeUsed
            // 
            this.muxTimeUsed.Location = new System.Drawing.Point(183, 224);
            this.muxTimeUsed.Name = "muxTimeUsed";
            this.muxTimeUsed.Size = new System.Drawing.Size(100, 23);
            this.muxTimeUsed.TabIndex = 17;
            // 
            // audioTimeLeft
            // 
            this.audioTimeLeft.Location = new System.Drawing.Point(183, 163);
            this.audioTimeLeft.Name = "audioTimeLeft";
            this.audioTimeLeft.Size = new System.Drawing.Size(100, 23);
            this.audioTimeLeft.TabIndex = 16;
            // 
            // audioTimeUsed
            // 
            this.audioTimeUsed.Location = new System.Drawing.Point(183, 140);
            this.audioTimeUsed.Name = "audioTimeUsed";
            this.audioTimeUsed.Size = new System.Drawing.Size(100, 23);
            this.audioTimeUsed.TabIndex = 15;
            // 
            // videoTimeLeft
            // 
            this.videoTimeLeft.Location = new System.Drawing.Point(183, 79);
            this.videoTimeLeft.Name = "videoTimeLeft";
            this.videoTimeLeft.Size = new System.Drawing.Size(100, 23);
            this.videoTimeLeft.TabIndex = 14;
            // 
            // videoTimeUsed
            // 
            this.videoTimeUsed.Location = new System.Drawing.Point(183, 56);
            this.videoTimeUsed.Name = "videoTimeUsed";
            this.videoTimeUsed.Size = new System.Drawing.Size(100, 23);
            this.videoTimeUsed.TabIndex = 13;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(430, 363);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 30);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "中止";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(111, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 23);
            this.label8.TabIndex = 11;
            this.label8.Text = "剩余时间：";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(111, 224);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 23);
            this.label9.TabIndex = 10;
            this.label9.Text = "已用时间：";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(111, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "剩余时间：";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(111, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "已用时间：";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(111, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 23);
            this.label5.TabIndex = 7;
            this.label5.Text = "剩余时间：";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(111, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "已用时间：";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "合成进度：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // muxProgressBar
            // 
            this.muxProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.muxProgressBar.Location = new System.Drawing.Point(111, 189);
            this.muxProgressBar.Name = "muxProgressBar";
            this.muxProgressBar.Size = new System.Drawing.Size(394, 23);
            this.muxProgressBar.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "音频编码进度：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // audioProgressBar
            // 
            this.audioProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProgressBar.Location = new System.Drawing.Point(111, 105);
            this.audioProgressBar.Name = "audioProgressBar";
            this.audioProgressBar.Size = new System.Drawing.Size(394, 23);
            this.audioProgressBar.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "视频编码进度：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoProgressBar
            // 
            this.videoProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProgressBar.Location = new System.Drawing.Point(111, 17);
            this.videoProgressBar.Name = "videoProgressBar";
            this.videoProgressBar.Size = new System.Drawing.Size(394, 23);
            this.videoProgressBar.TabIndex = 0;
            // 
            // inputPage
            // 
            this.inputPage.Controls.Add(this.label10);
            this.inputPage.Controls.Add(this.profileBox);
            this.inputPage.Controls.Add(this.jobItemListView);
            this.inputPage.Controls.Add(this.settingButton);
            this.inputPage.Controls.Add(this.startButton);
            this.inputPage.Controls.Add(this.clearButton);
            this.inputPage.Controls.Add(this.addButton);
            this.inputPage.Controls.Add(this.delButton);
            this.inputPage.Location = new System.Drawing.Point(4, 22);
            this.inputPage.Name = "inputPage";
            this.inputPage.Padding = new System.Windows.Forms.Padding(3);
            this.inputPage.Size = new System.Drawing.Size(513, 434);
            this.inputPage.TabIndex = 0;
            this.inputPage.Text = "输入";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.Location = new System.Drawing.Point(8, 389);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "预设";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // profileBox
            // 
            this.profileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileBox.FormattingEnabled = true;
            this.profileBox.Location = new System.Drawing.Point(89, 392);
            this.profileBox.Name = "profileBox";
            this.profileBox.Size = new System.Drawing.Size(237, 20);
            this.profileBox.TabIndex = 8;
            this.profileBox.SelectedIndexChanged += new System.EventHandler(this.ProfileBoxSelectedIndexChanged);
            // 
            // jobItemListView
            // 
            this.jobItemListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.jobItemListView.AllowColumnReorder = true;
            this.jobItemListView.AllowDrop = true;
            this.jobItemListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jobItemListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.stateColumn,
            this.sourceFileColumn,
            this.destinationFileColumn});
            this.jobItemListView.FullRowSelect = true;
            this.jobItemListView.Location = new System.Drawing.Point(8, 26);
            this.jobItemListView.Name = "jobItemListView";
            this.jobItemListView.Size = new System.Drawing.Size(499, 324);
            this.jobItemListView.TabIndex = 3;
            this.jobItemListView.UseCompatibleStateImageBehavior = false;
            this.jobItemListView.View = System.Windows.Forms.View.Details;
            this.jobItemListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ListView1ItemDrag);
            this.jobItemListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.JobItemListViewItemSelectionChanged);
            this.jobItemListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.jobItemListView_DragDrop);
            this.jobItemListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListView1DragEnter);
            this.jobItemListView.DoubleClick += new System.EventHandler(this.jobItemListView_DoubleClick);
            // 
            // stateColumn
            // 
            this.stateColumn.Text = "状态";
            // 
            // sourceFileColumn
            // 
            this.sourceFileColumn.Text = "源文件";
            this.sourceFileColumn.Width = 361;
            // 
            // destinationFileColumn
            // 
            this.destinationFileColumn.Text = "输出文件";
            this.destinationFileColumn.Width = 321;
            // 
            // settingButton
            // 
            this.settingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingButton.Enabled = false;
            this.settingButton.Location = new System.Drawing.Point(89, 356);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(75, 30);
            this.settingButton.TabIndex = 7;
            this.settingButton.Text = "设置";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.SettingButtonClick);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(370, 363);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(137, 49);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "开始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearButton.Location = new System.Drawing.Point(251, 356);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 30);
            this.clearButton.TabIndex = 6;
            this.clearButton.Text = "清空";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButtonClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(8, 356);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "添加";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // delButton
            // 
            this.delButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delButton.Location = new System.Drawing.Point(170, 356);
            this.delButton.Name = "delButton";
            this.delButton.Size = new System.Drawing.Size(75, 30);
            this.delButton.TabIndex = 4;
            this.delButton.Text = "删除";
            this.delButton.UseVisualStyleBackColor = true;
            this.delButton.Click += new System.EventHandler(this.DelButtonClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.inputPage);
            this.tabControl1.Controls.Add(this.progressPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(521, 460);
            this.tabControl1.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 489);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(510, 38);
            this.Name = "MainForm";
            this.Text = "CXGUI";
            this.Activated += new System.EventHandler(this.MainFormActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.listViewMenu.ResumeLayout(false);
            this.progressPage.ResumeLayout(false);
            this.progressPage.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.inputPage.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip listViewMenu;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 未处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开目录ToolStripMenuItem;
        private System.Windows.Forms.TabPage progressPage;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLable;
        private System.Windows.Forms.Label muxTimeLeft;
        private System.Windows.Forms.Label muxTimeUsed;
        private System.Windows.Forms.Label audioTimeLeft;
        private System.Windows.Forms.Label audioTimeUsed;
        private System.Windows.Forms.Label videoTimeLeft;
        private System.Windows.Forms.Label videoTimeUsed;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar muxProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar audioProgressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar videoProgressBar;
        private System.Windows.Forms.TabPage inputPage;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox profileBox;
        private System.Windows.Forms.ListView jobItemListView;
        private System.Windows.Forms.ColumnHeader stateColumn;
        private System.Windows.Forms.ColumnHeader sourceFileColumn;
        private System.Windows.Forms.ColumnHeader destinationFileColumn;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button delButton;
        private System.Windows.Forms.TabControl tabControl1;
	}
}
namespace CXGUI.GUI

import System.Windows.Forms

partial class MainForm(System.Windows.Forms.Form):
	private components as System.ComponentModel.IContainer = null
	
	protected override def Dispose(disposing as bool) as void:
		if disposing:
			if components is not null:
				components.Dispose()
		super(disposing)
	
	// This method is required for Windows Forms designer support.
	// Do not change the method contents inside the source code editor. The Forms designer might
	// not be able to load this method if it was changed manually.
	private def InitializeComponent():
		self.components = System.ComponentModel.Container()
		self.addButton = System.Windows.Forms.Button()
		self.delButton = System.Windows.Forms.Button()
		self.startButton = System.Windows.Forms.Button()
		self.clearButton = System.Windows.Forms.Button()
		self.tabControl1 = System.Windows.Forms.TabControl()
		self.inputPage = System.Windows.Forms.TabPage()
		self.label10 = System.Windows.Forms.Label()
		self.profileBox = System.Windows.Forms.ComboBox()
		self.jobItemListView = System.Windows.Forms.ListView()
		self.stateColumn = System.Windows.Forms.ColumnHeader()
		self.sourceFileColumn = System.Windows.Forms.ColumnHeader()
		self.destinationFileColumn = System.Windows.Forms.ColumnHeader()
		self.listViewMenu = System.Windows.Forms.ContextMenuStrip(self.components)
		self.设置ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.未处理ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.添加ToolStripMenuItem1 = System.Windows.Forms.ToolStripMenuItem()
		self.删除ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.清空ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.打开目录ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.settingButton = System.Windows.Forms.Button()
		self.progressPage = System.Windows.Forms.TabPage()
		self.statusStrip = System.Windows.Forms.StatusStrip()
		self.statusLable = System.Windows.Forms.ToolStripStatusLabel()
		self.muxTimeLeft = System.Windows.Forms.Label()
		self.muxTimeUsed = System.Windows.Forms.Label()
		self.audioTimeLeft = System.Windows.Forms.Label()
		self.audioTimeUsed = System.Windows.Forms.Label()
		self.videoTimeLeft = System.Windows.Forms.Label()
		self.videoTimeUsed = System.Windows.Forms.Label()
		self.stopButton = System.Windows.Forms.Button()
		self.label8 = System.Windows.Forms.Label()
		self.label9 = System.Windows.Forms.Label()
		self.label6 = System.Windows.Forms.Label()
		self.label7 = System.Windows.Forms.Label()
		self.label5 = System.Windows.Forms.Label()
		self.label4 = System.Windows.Forms.Label()
		self.label3 = System.Windows.Forms.Label()
		self.muxProgressBar = System.Windows.Forms.ProgressBar()
		self.label2 = System.Windows.Forms.Label()
		self.audioProgressBar = System.Windows.Forms.ProgressBar()
		self.label1 = System.Windows.Forms.Label()
		self.videoProgressBar = System.Windows.Forms.ProgressBar()
		self.openFileDialog1 = System.Windows.Forms.OpenFileDialog()
		self.menuStrip1 = System.Windows.Forms.MenuStrip()
		self.文件ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.添加ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.工具ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.选项ToolStripMenuItem = System.Windows.Forms.ToolStripMenuItem()
		self.backgroundWorker1 = System.ComponentModel.BackgroundWorker()
		self.tabControl1.SuspendLayout()
		self.inputPage.SuspendLayout()
		self.listViewMenu.SuspendLayout()
		self.progressPage.SuspendLayout()
		self.statusStrip.SuspendLayout()
		self.menuStrip1.SuspendLayout()
		self.SuspendLayout()
		# 
		# addButton
		# 
		self.addButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.addButton.Location = System.Drawing.Point(8, 356)
		self.addButton.Name = "addButton"
		self.addButton.Size = System.Drawing.Size(75, 30)
		self.addButton.TabIndex = 3
		self.addButton.Text = "添加"
		self.addButton.UseVisualStyleBackColor = true
		self.addButton.Click += self.AddButtonClick as System.EventHandler
		# 
		# delButton
		# 
		self.delButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.delButton.Location = System.Drawing.Point(170, 356)
		self.delButton.Name = "delButton"
		self.delButton.Size = System.Drawing.Size(75, 30)
		self.delButton.TabIndex = 4
		self.delButton.Text = "删除"
		self.delButton.UseVisualStyleBackColor = true
		self.delButton.Click += self.DelButtonClick as System.EventHandler
		# 
		# startButton
		# 
		self.startButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.startButton.Location = System.Drawing.Point(370, 363)
		self.startButton.Name = "startButton"
		self.startButton.Size = System.Drawing.Size(137, 49)
		self.startButton.TabIndex = 5
		self.startButton.Text = "开始"
		self.startButton.UseVisualStyleBackColor = true
		self.startButton.Click += self.StartButtonClick as System.EventHandler
		# 
		# clearButton
		# 
		self.clearButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.clearButton.Location = System.Drawing.Point(251, 356)
		self.clearButton.Name = "clearButton"
		self.clearButton.Size = System.Drawing.Size(75, 30)
		self.clearButton.TabIndex = 6
		self.clearButton.Text = "清空"
		self.clearButton.UseVisualStyleBackColor = true
		self.clearButton.Click += self.ClearButtonClick as System.EventHandler
		# 
		# tabControl1
		# 
		self.tabControl1.Anchor = cast(System.Windows.Forms.AnchorStyles,(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.tabControl1.Controls.Add(self.inputPage)
		self.tabControl1.Controls.Add(self.progressPage)
		self.tabControl1.Location = System.Drawing.Point(0, 28)
		self.tabControl1.Name = "tabControl1"
		self.tabControl1.SelectedIndex = 0
		self.tabControl1.Size = System.Drawing.Size(521, 460)
		self.tabControl1.TabIndex = 7
		# 
		# inputPage
		# 
		self.inputPage.Controls.Add(self.label10)
		self.inputPage.Controls.Add(self.profileBox)
		self.inputPage.Controls.Add(self.jobItemListView)
		self.inputPage.Controls.Add(self.settingButton)
		self.inputPage.Controls.Add(self.startButton)
		self.inputPage.Controls.Add(self.clearButton)
		self.inputPage.Controls.Add(self.addButton)
		self.inputPage.Controls.Add(self.delButton)
		self.inputPage.Location = System.Drawing.Point(4, 22)
		self.inputPage.Name = "inputPage"
		self.inputPage.Padding = System.Windows.Forms.Padding(3)
		self.inputPage.Size = System.Drawing.Size(513, 434)
		self.inputPage.TabIndex = 0
		self.inputPage.Text = "输入"
		# 
		# label10
		# 
		self.label10.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.label10.Location = System.Drawing.Point(8, 389)
		self.label10.Name = "label10"
		self.label10.Size = System.Drawing.Size(59, 23)
		self.label10.TabIndex = 9
		self.label10.Text = "预设"
		self.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# profileBox
		# 
		self.profileBox.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.profileBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.profileBox.FormattingEnabled = true
		self.profileBox.Location = System.Drawing.Point(89, 392)
		self.profileBox.Name = "profileBox"
		self.profileBox.Size = System.Drawing.Size(237, 20)
		self.profileBox.TabIndex = 8
		self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged as System.EventHandler
		# 
		# listView1
		# 
		self.jobItemListView.Activation = System.Windows.Forms.ItemActivation.OneClick
		self.jobItemListView.AllowColumnReorder = true
		self.jobItemListView.AllowDrop = true
		self.jobItemListView.Anchor = cast(System.Windows.Forms.AnchorStyles,(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.jobItemListView.Columns.AddRange((of System.Windows.Forms.ColumnHeader: self.stateColumn, self.sourceFileColumn, self.destinationFileColumn))
		self.jobItemListView.ContextMenuStrip = self.listViewMenu
		self.jobItemListView.FullRowSelect = true
		self.jobItemListView.Location = System.Drawing.Point(8, 26)
		self.jobItemListView.Name = "listView1"
		self.jobItemListView.Size = System.Drawing.Size(499, 324)
		self.jobItemListView.TabIndex = 3
		self.jobItemListView.UseCompatibleStateImageBehavior = false
		self.jobItemListView.View = System.Windows.Forms.View.Details
		self.jobItemListView.DoubleClick += self.ListView1DoubleClick as System.EventHandler
		self.jobItemListView.DragDrop += self.ListView1DragDrop as System.Windows.Forms.DragEventHandler
		self.jobItemListView.ItemSelectionChanged += self.JobItemListViewItemSelectionChanged as System.Windows.Forms.ListViewItemSelectionChangedEventHandler
		self.jobItemListView.DragEnter += self.ListView1DragEnter as System.Windows.Forms.DragEventHandler
		self.jobItemListView.ItemDrag += self.ListView1ItemDrag as System.Windows.Forms.ItemDragEventHandler
		# 
		# stateColumn
		# 
		self.stateColumn.Text = "状态"
		# 
		# sourceFileColumn
		# 
		self.sourceFileColumn.Text = "源文件"
		self.sourceFileColumn.Width = 361
		# 
		# destinationFileColumn
		# 
		self.destinationFileColumn.Text = "输出文件"
		self.destinationFileColumn.Width = 321
		# 
		# listViewMenu
		# 
		self.listViewMenu.Items.AddRange((of System.Windows.Forms.ToolStripItem: self.设置ToolStripMenuItem, self.未处理ToolStripMenuItem, self.添加ToolStripMenuItem1, self.删除ToolStripMenuItem, self.清空ToolStripMenuItem, self.打开目录ToolStripMenuItem))
		self.listViewMenu.Name = "contextMenuStrip1"
		self.listViewMenu.Size = System.Drawing.Size(125, 136)
		self.listViewMenu.Opening += self.ContextMenuStrip1Opening as System.ComponentModel.CancelEventHandler
		# 
		# 设置ToolStripMenuItem
		# 
		self.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem"
		self.设置ToolStripMenuItem.Size = System.Drawing.Size(124, 22)
		self.设置ToolStripMenuItem.Text = "设置"
		self.设置ToolStripMenuItem.Click += self.设置ToolStripMenuItemClick as System.EventHandler
		# 
		# 未处理ToolStripMenuItem
		# 
		self.未处理ToolStripMenuItem.Name = "未处理ToolStripMenuItem"
		self.未处理ToolStripMenuItem.Size = System.Drawing.Size(124, 22)
		self.未处理ToolStripMenuItem.Text = "未处理"
		self.未处理ToolStripMenuItem.Click += self.未处理ToolStripMenuItemClick as System.EventHandler
		# 
		# 添加ToolStripMenuItem1
		# 
		self.添加ToolStripMenuItem1.Name = "添加ToolStripMenuItem1"
		self.添加ToolStripMenuItem1.Size = System.Drawing.Size(124, 22)
		self.添加ToolStripMenuItem1.Text = "添加"
		self.添加ToolStripMenuItem1.Click += self.添加ToolStripMenuItem1Click as System.EventHandler
		# 
		# 删除ToolStripMenuItem
		# 
		self.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem"
		self.删除ToolStripMenuItem.Size = System.Drawing.Size(124, 22)
		self.删除ToolStripMenuItem.Text = "删除"
		self.删除ToolStripMenuItem.Click += self.删除ToolStripMenuItemClick as System.EventHandler
		# 
		# 清空ToolStripMenuItem
		# 
		self.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem"
		self.清空ToolStripMenuItem.Size = System.Drawing.Size(124, 22)
		self.清空ToolStripMenuItem.Text = "清空"
		self.清空ToolStripMenuItem.Click += self.清空ToolStripMenuItemClick as System.EventHandler
		# 
		# 打开目录ToolStripMenuItem
		# 
		self.打开目录ToolStripMenuItem.Name = "打开目录ToolStripMenuItem"
		self.打开目录ToolStripMenuItem.Size = System.Drawing.Size(124, 22)
		self.打开目录ToolStripMenuItem.Text = "打开目录"
		self.打开目录ToolStripMenuItem.Click += self.打开目录ToolStripMenuItemClick as System.EventHandler
		# 
		# settingButton
		# 
		self.settingButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.settingButton.Enabled = false
		self.settingButton.Location = System.Drawing.Point(89, 356)
		self.settingButton.Name = "settingButton"
		self.settingButton.Size = System.Drawing.Size(75, 30)
		self.settingButton.TabIndex = 7
		self.settingButton.Text = "设置"
		self.settingButton.UseVisualStyleBackColor = true
		self.settingButton.Click += self.SettingButtonClick as System.EventHandler
		# 
		# progressPage
		# 
		self.progressPage.Controls.Add(self.statusStrip)
		self.progressPage.Controls.Add(self.muxTimeLeft)
		self.progressPage.Controls.Add(self.muxTimeUsed)
		self.progressPage.Controls.Add(self.audioTimeLeft)
		self.progressPage.Controls.Add(self.audioTimeUsed)
		self.progressPage.Controls.Add(self.videoTimeLeft)
		self.progressPage.Controls.Add(self.videoTimeUsed)
		self.progressPage.Controls.Add(self.stopButton)
		self.progressPage.Controls.Add(self.label8)
		self.progressPage.Controls.Add(self.label9)
		self.progressPage.Controls.Add(self.label6)
		self.progressPage.Controls.Add(self.label7)
		self.progressPage.Controls.Add(self.label5)
		self.progressPage.Controls.Add(self.label4)
		self.progressPage.Controls.Add(self.label3)
		self.progressPage.Controls.Add(self.muxProgressBar)
		self.progressPage.Controls.Add(self.label2)
		self.progressPage.Controls.Add(self.audioProgressBar)
		self.progressPage.Controls.Add(self.label1)
		self.progressPage.Controls.Add(self.videoProgressBar)
		self.progressPage.Location = System.Drawing.Point(4, 22)
		self.progressPage.Name = "progressPage"
		self.progressPage.Padding = System.Windows.Forms.Padding(3)
		self.progressPage.Size = System.Drawing.Size(513, 434)
		self.progressPage.TabIndex = 1
		self.progressPage.Text = "进度"
		# 
		# statusStrip
		# 
		self.statusStrip.Items.AddRange((of System.Windows.Forms.ToolStripItem: self.statusLable))
		self.statusStrip.Location = System.Drawing.Point(3, 409)
		self.statusStrip.Name = "statusStrip"
		self.statusStrip.Size = System.Drawing.Size(507, 22)
		self.statusStrip.TabIndex = 19
		# 
		# statusLable
		# 
		self.statusLable.Name = "statusLable"
		self.statusLable.Size = System.Drawing.Size(0, 17)
		# 
		# muxTimeLeft
		# 
		self.muxTimeLeft.Location = System.Drawing.Point(183, 247)
		self.muxTimeLeft.Name = "muxTimeLeft"
		self.muxTimeLeft.Size = System.Drawing.Size(100, 23)
		self.muxTimeLeft.TabIndex = 18
		# 
		# muxTimeUsed
		# 
		self.muxTimeUsed.Location = System.Drawing.Point(183, 224)
		self.muxTimeUsed.Name = "muxTimeUsed"
		self.muxTimeUsed.Size = System.Drawing.Size(100, 23)
		self.muxTimeUsed.TabIndex = 17
		# 
		# audioTimeLeft
		# 
		self.audioTimeLeft.Location = System.Drawing.Point(183, 163)
		self.audioTimeLeft.Name = "audioTimeLeft"
		self.audioTimeLeft.Size = System.Drawing.Size(100, 23)
		self.audioTimeLeft.TabIndex = 16
		# 
		# audioTimeUsed
		# 
		self.audioTimeUsed.Location = System.Drawing.Point(183, 140)
		self.audioTimeUsed.Name = "audioTimeUsed"
		self.audioTimeUsed.Size = System.Drawing.Size(100, 23)
		self.audioTimeUsed.TabIndex = 15
		# 
		# videoTimeLeft
		# 
		self.videoTimeLeft.Location = System.Drawing.Point(183, 79)
		self.videoTimeLeft.Name = "videoTimeLeft"
		self.videoTimeLeft.Size = System.Drawing.Size(100, 23)
		self.videoTimeLeft.TabIndex = 14
		# 
		# videoTimeUsed
		# 
		self.videoTimeUsed.Location = System.Drawing.Point(183, 56)
		self.videoTimeUsed.Name = "videoTimeUsed"
		self.videoTimeUsed.Size = System.Drawing.Size(100, 23)
		self.videoTimeUsed.TabIndex = 13
		# 
		# stopButton
		# 
		self.stopButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.stopButton.Location = System.Drawing.Point(430, 363)
		self.stopButton.Name = "stopButton"
		self.stopButton.Size = System.Drawing.Size(75, 30)
		self.stopButton.TabIndex = 12
		self.stopButton.Text = "中止"
		self.stopButton.UseVisualStyleBackColor = true
		self.stopButton.Click += self.StopButtonClick as System.EventHandler
		# 
		# label8
		# 
		self.label8.Location = System.Drawing.Point(111, 247)
		self.label8.Name = "label8"
		self.label8.Size = System.Drawing.Size(66, 23)
		self.label8.TabIndex = 11
		self.label8.Text = "剩余时间："
		# 
		# label9
		# 
		self.label9.Location = System.Drawing.Point(111, 224)
		self.label9.Name = "label9"
		self.label9.Size = System.Drawing.Size(66, 23)
		self.label9.TabIndex = 10
		self.label9.Text = "已用时间："
		# 
		# label6
		# 
		self.label6.Location = System.Drawing.Point(111, 163)
		self.label6.Name = "label6"
		self.label6.Size = System.Drawing.Size(66, 23)
		self.label6.TabIndex = 9
		self.label6.Text = "剩余时间："
		# 
		# label7
		# 
		self.label7.Location = System.Drawing.Point(111, 140)
		self.label7.Name = "label7"
		self.label7.Size = System.Drawing.Size(66, 23)
		self.label7.TabIndex = 8
		self.label7.Text = "已用时间："
		# 
		# label5
		# 
		self.label5.Location = System.Drawing.Point(111, 79)
		self.label5.Name = "label5"
		self.label5.Size = System.Drawing.Size(66, 23)
		self.label5.TabIndex = 7
		self.label5.Text = "剩余时间："
		# 
		# label4
		# 
		self.label4.Location = System.Drawing.Point(111, 56)
		self.label4.Name = "label4"
		self.label4.Size = System.Drawing.Size(66, 23)
		self.label4.TabIndex = 6
		self.label4.Text = "已用时间："
		# 
		# label3
		# 
		self.label3.Location = System.Drawing.Point(3, 189)
		self.label3.Name = "label3"
		self.label3.Size = System.Drawing.Size(97, 23)
		self.label3.TabIndex = 5
		self.label3.Text = "合成进度："
		self.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# muxProgressBar
		# 
		self.muxProgressBar.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.muxProgressBar.Location = System.Drawing.Point(111, 189)
		self.muxProgressBar.Name = "muxProgressBar"
		self.muxProgressBar.Size = System.Drawing.Size(394, 23)
		self.muxProgressBar.TabIndex = 4
		# 
		# label2
		# 
		self.label2.Location = System.Drawing.Point(8, 105)
		self.label2.Name = "label2"
		self.label2.Size = System.Drawing.Size(97, 23)
		self.label2.TabIndex = 3
		self.label2.Text = "音频编码进度："
		self.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# audioProgressBar
		# 
		self.audioProgressBar.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.audioProgressBar.Location = System.Drawing.Point(111, 105)
		self.audioProgressBar.Name = "audioProgressBar"
		self.audioProgressBar.Size = System.Drawing.Size(394, 23)
		self.audioProgressBar.TabIndex = 2
		# 
		# label1
		# 
		self.label1.Location = System.Drawing.Point(8, 17)
		self.label1.Name = "label1"
		self.label1.Size = System.Drawing.Size(97, 23)
		self.label1.TabIndex = 1
		self.label1.Text = "视频编码进度："
		self.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# videoProgressBar
		# 
		self.videoProgressBar.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.videoProgressBar.Location = System.Drawing.Point(111, 17)
		self.videoProgressBar.Name = "videoProgressBar"
		self.videoProgressBar.Size = System.Drawing.Size(394, 23)
		self.videoProgressBar.TabIndex = 0
		# 
		# openFileDialog1
		# 
		self.openFileDialog1.Multiselect = true
		self.openFileDialog1.FileOk += self.OpenFileDialog1FileOk as System.ComponentModel.CancelEventHandler
		# 
		# menuStrip1
		# 
		self.menuStrip1.Items.AddRange((of System.Windows.Forms.ToolStripItem: self.文件ToolStripMenuItem, self.工具ToolStripMenuItem))
		self.menuStrip1.Location = System.Drawing.Point(0, 0)
		self.menuStrip1.Name = "menuStrip1"
		self.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
		self.menuStrip1.Size = System.Drawing.Size(521, 25)
		self.menuStrip1.TabIndex = 8
		self.menuStrip1.Text = "menuStrip1"
		# 
		# 文件ToolStripMenuItem
		# 
		self.文件ToolStripMenuItem.DropDownItems.AddRange((of System.Windows.Forms.ToolStripItem: self.添加ToolStripMenuItem))
		self.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem"
		self.文件ToolStripMenuItem.Size = System.Drawing.Size(44, 21)
		self.文件ToolStripMenuItem.Text = "文件"
		# 
		# 添加ToolStripMenuItem
		# 
		self.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem"
		self.添加ToolStripMenuItem.Size = System.Drawing.Size(100, 22)
		self.添加ToolStripMenuItem.Text = "添加"
		self.添加ToolStripMenuItem.Click += self.添加ToolStripMenuItemClick as System.EventHandler
		# 
		# 工具ToolStripMenuItem
		# 
		self.工具ToolStripMenuItem.DropDownItems.AddRange((of System.Windows.Forms.ToolStripItem: self.选项ToolStripMenuItem))
		self.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem"
		self.工具ToolStripMenuItem.Size = System.Drawing.Size(44, 21)
		self.工具ToolStripMenuItem.Text = "工具"
		# 
		# 选项ToolStripMenuItem
		# 
		self.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem"
		self.选项ToolStripMenuItem.Size = System.Drawing.Size(100, 22)
		self.选项ToolStripMenuItem.Text = "选项"
		self.选项ToolStripMenuItem.Click += self.选项ToolStripMenuItemClick as System.EventHandler
		# 
		# backgroundWorker1
		# 
		self.backgroundWorker1.WorkerReportsProgress = true
		self.backgroundWorker1.WorkerSupportsCancellation = true
		self.backgroundWorker1.DoWork += self.BackgroundWorker1DoWork as System.ComponentModel.DoWorkEventHandler
		self.backgroundWorker1.RunWorkerCompleted += self.NextJobOrExist as System.ComponentModel.RunWorkerCompletedEventHandler
		self.backgroundWorker1.ProgressChanged += self.BackgroundWorker1ProgressChanged as System.ComponentModel.ProgressChangedEventHandler
		# 
		# MainForm
		# 
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.ClientSize = System.Drawing.Size(521, 489)
		self.Controls.Add(self.tabControl1)
		self.Controls.Add(self.menuStrip1)
		self.MainMenuStrip = self.menuStrip1
		self.MinimumSize = System.Drawing.Size(510, 38)
		self.Name = "MainForm"
		self.Text = "CXGUI"
		self.Load += self.MainFormLoad as System.EventHandler
		self.Activated += self.MainFormActivated as System.EventHandler
		self.FormClosing += self.MainFormFormClosing as System.Windows.Forms.FormClosingEventHandler
		self.tabControl1.ResumeLayout(false)
		self.inputPage.ResumeLayout(false)
		self.listViewMenu.ResumeLayout(false)
		self.progressPage.ResumeLayout(false)
		self.progressPage.PerformLayout()
		self.statusStrip.ResumeLayout(false)
		self.statusStrip.PerformLayout()
		self.menuStrip1.ResumeLayout(false)
		self.menuStrip1.PerformLayout()
		self.ResumeLayout(false)
		self.PerformLayout()
	private profileBox as System.Windows.Forms.ComboBox
	private label10 as System.Windows.Forms.Label
	private 打开目录ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private statusLable as System.Windows.Forms.ToolStripStatusLabel
	private statusStrip as System.Windows.Forms.StatusStrip
	private 添加ToolStripMenuItem1 as System.Windows.Forms.ToolStripMenuItem
	private 清空ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 删除ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 设置ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 未处理ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private listViewMenu as System.Windows.Forms.ContextMenuStrip
	private backgroundWorker1 as System.ComponentModel.BackgroundWorker
	private label3 as System.Windows.Forms.Label
	private settingButton as System.Windows.Forms.Button
	private 选项ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 工具ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 添加ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private 文件ToolStripMenuItem as System.Windows.Forms.ToolStripMenuItem
	private menuStrip1 as System.Windows.Forms.MenuStrip
	private sourceFileColumn as System.Windows.Forms.ColumnHeader
	private stateColumn as System.Windows.Forms.ColumnHeader
	private destinationFileColumn as System.Windows.Forms.ColumnHeader
	private openFileDialog1 as System.Windows.Forms.OpenFileDialog
	private stopButton as System.Windows.Forms.Button
	private videoTimeUsed as System.Windows.Forms.Label
	private videoTimeLeft as System.Windows.Forms.Label
	private audioTimeUsed as System.Windows.Forms.Label
	private audioTimeLeft as System.Windows.Forms.Label
	private muxTimeUsed as System.Windows.Forms.Label
	private muxTimeLeft as System.Windows.Forms.Label
	private addButton as System.Windows.Forms.Button
	private delButton as System.Windows.Forms.Button
	private startButton as System.Windows.Forms.Button
	private clearButton as System.Windows.Forms.Button
	private inputPage as System.Windows.Forms.TabPage
	private progressPage as System.Windows.Forms.TabPage
	private muxProgressBar as System.Windows.Forms.ProgressBar
	private audioProgressBar as System.Windows.Forms.ProgressBar
	private videoProgressBar as System.Windows.Forms.ProgressBar
	private label4 as System.Windows.Forms.Label
	private label5 as System.Windows.Forms.Label
	private label7 as System.Windows.Forms.Label
	private label6 as System.Windows.Forms.Label
	private label9 as System.Windows.Forms.Label
	private label8 as System.Windows.Forms.Label
	private label1 as System.Windows.Forms.Label
	private label2 as System.Windows.Forms.Label
	private tabControl1 as System.Windows.Forms.TabControl
	private jobItemListView as System.Windows.Forms.ListView




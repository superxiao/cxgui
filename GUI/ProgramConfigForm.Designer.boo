namespace CXGUI.GUI

partial class ProgramConfigForm(System.Windows.Forms.Form):
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
		self.groupBox1 = System.Windows.Forms.GroupBox()
		self.chbInputDir = System.Windows.Forms.CheckBox()
		self.chbSilentRestart = System.Windows.Forms.CheckBox()
		self.label1 = System.Windows.Forms.Label()
		self.destDirComboBox = System.Windows.Forms.ComboBox()
		self.outputButton = System.Windows.Forms.Button()
		self.OKButton = System.Windows.Forms.Button()
		self.cacelButton = System.Windows.Forms.Button()
		self.folderBrowserDialog1 = System.Windows.Forms.FolderBrowserDialog()
		self.groupBox2 = System.Windows.Forms.GroupBox()
		self.cbAudioAutoSF = System.Windows.Forms.CheckBox()
		self.editGroupBox = System.Windows.Forms.GroupBox()
		self.label2 = System.Windows.Forms.Label()
		self.previewPlayerButton = System.Windows.Forms.Button()
		self.previewPlayerComboBox = System.Windows.Forms.ComboBox()
		self.openFileDialog1 = System.Windows.Forms.OpenFileDialog()
		self.groupBox1.SuspendLayout()
		self.groupBox2.SuspendLayout()
		self.editGroupBox.SuspendLayout()
		self.SuspendLayout()
		# 
		# groupBox1
		# 
		self.groupBox1.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox1.Controls.Add(self.chbInputDir)
		self.groupBox1.Controls.Add(self.chbSilentRestart)
		self.groupBox1.Controls.Add(self.label1)
		self.groupBox1.Controls.Add(self.destDirComboBox)
		self.groupBox1.Controls.Add(self.outputButton)
		self.groupBox1.Location = System.Drawing.Point(12, 12)
		self.groupBox1.Name = "groupBox1"
		self.groupBox1.Size = System.Drawing.Size(410, 119)
		self.groupBox1.TabIndex = 0
		self.groupBox1.TabStop = false
		self.groupBox1.Text = "工作列表"
		# 
		# chbInputDir
		# 
		self.chbInputDir.Location = System.Drawing.Point(6, 79)
		self.chbInputDir.Name = "chbInputDir"
		self.chbInputDir.Size = System.Drawing.Size(165, 24)
		self.chbInputDir.TabIndex = 5
		self.chbInputDir.Text = "显示输入文件所在目录"
		self.chbInputDir.UseVisualStyleBackColor = true
		# 
		# chbSilentRestart
		# 
		self.chbSilentRestart.Location = System.Drawing.Point(6, 49)
		self.chbSilentRestart.Name = "chbSilentRestart"
		self.chbSilentRestart.Size = System.Drawing.Size(165, 24)
		self.chbSilentRestart.TabIndex = 4
		self.chbSilentRestart.Text = "中止项重新开始时不提示"
		self.chbSilentRestart.UseVisualStyleBackColor = true
		# 
		# label1
		# 
		self.label1.Location = System.Drawing.Point(6, 23)
		self.label1.Name = "label1"
		self.label1.Size = System.Drawing.Size(75, 23)
		self.label1.TabIndex = 3
		self.label1.Text = "输出文件夹"
		# 
		# destDirComboBox
		# 
		self.destDirComboBox.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.destDirComboBox.FormattingEnabled = true
		self.destDirComboBox.Location = System.Drawing.Point(87, 20)
		self.destDirComboBox.Name = "destDirComboBox"
		self.destDirComboBox.Size = System.Drawing.Size(236, 20)
		self.destDirComboBox.TabIndex = 2
		# 
		# outputButton
		# 
		self.outputButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
		self.outputButton.Location = System.Drawing.Point(329, 18)
		self.outputButton.Name = "outputButton"
		self.outputButton.Size = System.Drawing.Size(75, 23)
		self.outputButton.TabIndex = 1
		self.outputButton.Text = "浏览"
		self.outputButton.UseVisualStyleBackColor = true
		self.outputButton.Click += self.BrowseButtonClick as System.EventHandler
		# 
		# OKButton
		# 
		self.OKButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.OKButton.Location = System.Drawing.Point(266, 313)
		self.OKButton.Name = "OKButton"
		self.OKButton.Size = System.Drawing.Size(75, 23)
		self.OKButton.TabIndex = 1
		self.OKButton.Text = "确定"
		self.OKButton.UseVisualStyleBackColor = true
		self.OKButton.Click += self.OKButtonClick as System.EventHandler
		# 
		# cacelButton
		# 
		self.cacelButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.cacelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
		self.cacelButton.Location = System.Drawing.Point(347, 313)
		self.cacelButton.Name = "cacelButton"
		self.cacelButton.Size = System.Drawing.Size(75, 23)
		self.cacelButton.TabIndex = 2
		self.cacelButton.Text = "取消"
		self.cacelButton.UseVisualStyleBackColor = true
		self.cacelButton.Click += self.CacelButtonClick as System.EventHandler
		# 
		# groupBox2
		# 
		self.groupBox2.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox2.Controls.Add(self.cbAudioAutoSF)
		self.groupBox2.Location = System.Drawing.Point(12, 220)
		self.groupBox2.Name = "groupBox2"
		self.groupBox2.Size = System.Drawing.Size(410, 87)
		self.groupBox2.TabIndex = 3
		self.groupBox2.TabStop = false
		self.groupBox2.Text = "音频"
		# 
		# cbAudioAutoSF
		# 
		self.cbAudioAutoSF.Checked = true
		self.cbAudioAutoSF.CheckState = System.Windows.Forms.CheckState.Checked
		self.cbAudioAutoSF.Location = System.Drawing.Point(6, 20)
		self.cbAudioAutoSF.Name = "cbAudioAutoSF"
		self.cbAudioAutoSF.Size = System.Drawing.Size(217, 24)
		self.cbAudioAutoSF.TabIndex = 0
		self.cbAudioAutoSF.Text = "当音频读取有错误时自动更改源滤镜"
		self.cbAudioAutoSF.UseVisualStyleBackColor = true
		# 
		# editGroupBox
		# 
		self.editGroupBox.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.editGroupBox.Controls.Add(self.label2)
		self.editGroupBox.Controls.Add(self.previewPlayerButton)
		self.editGroupBox.Controls.Add(self.previewPlayerComboBox)
		self.editGroupBox.Location = System.Drawing.Point(12, 137)
		self.editGroupBox.Name = "editGroupBox"
		self.editGroupBox.Size = System.Drawing.Size(410, 77)
		self.editGroupBox.TabIndex = 4
		self.editGroupBox.TabStop = false
		self.editGroupBox.Text = "编辑与编码"
		# 
		# label2
		# 
		self.label2.Location = System.Drawing.Point(6, 22)
		self.label2.Name = "label2"
		self.label2.Size = System.Drawing.Size(75, 23)
		self.label2.TabIndex = 2
		self.label2.Text = "预览播放器"
		# 
		# previewPlayerButton
		# 
		self.previewPlayerButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
		self.previewPlayerButton.Location = System.Drawing.Point(329, 17)
		self.previewPlayerButton.Name = "previewPlayerButton"
		self.previewPlayerButton.Size = System.Drawing.Size(75, 23)
		self.previewPlayerButton.TabIndex = 1
		self.previewPlayerButton.Text = "浏览"
		self.previewPlayerButton.UseVisualStyleBackColor = true
		self.previewPlayerButton.Click += self.PreviewPlayerButtonClick as System.EventHandler
		# 
		# previewPlayerComboBox
		# 
		self.previewPlayerComboBox.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.previewPlayerComboBox.FormattingEnabled = true
		self.previewPlayerComboBox.Location = System.Drawing.Point(87, 20)
		self.previewPlayerComboBox.Name = "previewPlayerComboBox"
		self.previewPlayerComboBox.Size = System.Drawing.Size(236, 20)
		self.previewPlayerComboBox.TabIndex = 0
		# 
		# openFileDialog1
		# 
		self.openFileDialog1.Filter = "可执行文件|*.exe"
		# 
		# ProgramConfigForm
		# 
		self.AcceptButton = self.OKButton
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.CancelButton = self.cacelButton
		self.ClientSize = System.Drawing.Size(434, 348)
		self.Controls.Add(self.editGroupBox)
		self.Controls.Add(self.groupBox2)
		self.Controls.Add(self.cacelButton)
		self.Controls.Add(self.OKButton)
		self.Controls.Add(self.groupBox1)
		self.MinimumSize = System.Drawing.Size(270, 270)
		self.Name = "ProgramConfigForm"
		self.Text = "选项"
		self.Load += self.ProgramConfigFormLoad as System.EventHandler
		self.groupBox1.ResumeLayout(false)
		self.groupBox2.ResumeLayout(false)
		self.editGroupBox.ResumeLayout(false)
		self.ResumeLayout(false)
	private openFileDialog1 as System.Windows.Forms.OpenFileDialog
	private previewPlayerComboBox as System.Windows.Forms.ComboBox
	private previewPlayerButton as System.Windows.Forms.Button
	private label2 as System.Windows.Forms.Label
	private editGroupBox as System.Windows.Forms.GroupBox
	private outputButton as System.Windows.Forms.Button
	public chbSilentRestart as System.Windows.Forms.CheckBox
	public chbInputDir as System.Windows.Forms.CheckBox
	public cbAudioAutoSF as System.Windows.Forms.CheckBox
	private groupBox2 as System.Windows.Forms.GroupBox
	private folderBrowserDialog1 as System.Windows.Forms.FolderBrowserDialog
	public destDirComboBox as System.Windows.Forms.ComboBox
	private cacelButton as System.Windows.Forms.Button
	private OKButton as System.Windows.Forms.Button
	private label1 as System.Windows.Forms.Label
	private groupBox1 as System.Windows.Forms.GroupBox


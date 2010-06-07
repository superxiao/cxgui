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
		self.label1 = System.Windows.Forms.Label()
		self.destDirComboBox = System.Windows.Forms.ComboBox()
		self.browseButton = System.Windows.Forms.Button()
		self.OKButton = System.Windows.Forms.Button()
		self.cacelButton = System.Windows.Forms.Button()
		self.folderBrowserDialog1 = System.Windows.Forms.FolderBrowserDialog()
		self.groupBox2 = System.Windows.Forms.GroupBox()
		self.cbAudioAutoSF = System.Windows.Forms.CheckBox()
		self.groupBox1.SuspendLayout()
		self.groupBox2.SuspendLayout()
		self.SuspendLayout()
		# 
		# groupBox1
		# 
		self.groupBox1.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox1.Controls.Add(self.label1)
		self.groupBox1.Controls.Add(self.destDirComboBox)
		self.groupBox1.Controls.Add(self.browseButton)
		self.groupBox1.Location = System.Drawing.Point(12, 12)
		self.groupBox1.Name = "groupBox1"
		self.groupBox1.Size = System.Drawing.Size(289, 85)
		self.groupBox1.TabIndex = 0
		self.groupBox1.TabStop = false
		self.groupBox1.Text = "输出"
		# 
		# label1
		# 
		self.label1.Location = System.Drawing.Point(6, 26)
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
		self.destDirComboBox.Location = System.Drawing.Point(87, 23)
		self.destDirComboBox.Name = "destDirComboBox"
		self.destDirComboBox.Size = System.Drawing.Size(82, 20)
		self.destDirComboBox.TabIndex = 2
		# 
		# browseButton
		# 
		self.browseButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
		self.browseButton.Location = System.Drawing.Point(188, 21)
		self.browseButton.Name = "browseButton"
		self.browseButton.Size = System.Drawing.Size(75, 23)
		self.browseButton.TabIndex = 1
		self.browseButton.Text = "浏览"
		self.browseButton.UseVisualStyleBackColor = true
		self.browseButton.Click += self.BrowseButtonClick as System.EventHandler
		# 
		# OKButton
		# 
		self.OKButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.OKButton.Location = System.Drawing.Point(145, 313)
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
		self.cacelButton.Location = System.Drawing.Point(226, 313)
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
		self.groupBox2.Location = System.Drawing.Point(12, 103)
		self.groupBox2.Name = "groupBox2"
		self.groupBox2.Size = System.Drawing.Size(289, 87)
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
		# ProgramConfigForm
		# 
		self.AcceptButton = self.OKButton
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.CancelButton = self.cacelButton
		self.ClientSize = System.Drawing.Size(313, 348)
		self.Controls.Add(self.groupBox2)
		self.Controls.Add(self.cacelButton)
		self.Controls.Add(self.OKButton)
		self.Controls.Add(self.groupBox1)
		self.MinimumSize = System.Drawing.Size(270, 270)
		self.Name = "ProgramConfigForm"
		self.Text = "ProgramConfigForm"
		self.Load += self.ProgramConfigFormLoad as System.EventHandler
		self.groupBox1.ResumeLayout(false)
		self.groupBox2.ResumeLayout(false)
		self.ResumeLayout(false)
	public cbAudioAutoSF as System.Windows.Forms.CheckBox
	private groupBox2 as System.Windows.Forms.GroupBox
	private folderBrowserDialog1 as System.Windows.Forms.FolderBrowserDialog
	public destDirComboBox as System.Windows.Forms.ComboBox
	private cacelButton as System.Windows.Forms.Button
	private OKButton as System.Windows.Forms.Button
	private browseButton as System.Windows.Forms.Button
	private label1 as System.Windows.Forms.Label
	private groupBox1 as System.Windows.Forms.GroupBox


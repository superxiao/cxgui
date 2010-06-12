namespace CXGUI.GUI

partial class MediaSettingForm(System.Windows.Forms.Form):
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
		self.tabControl1 = System.Windows.Forms.TabControl()
		self.tabPage1 = System.Windows.Forms.TabPage()
		self.cbAudioMode = System.Windows.Forms.ComboBox()
		self.cbVideoMode = System.Windows.Forms.ComboBox()
		self.label17 = System.Windows.Forms.Label()
		self.label16 = System.Windows.Forms.Label()
		self.destFileBox = System.Windows.Forms.ComboBox()
		self.label6 = System.Windows.Forms.Label()
		self.btOutBrowse = System.Windows.Forms.Button()
		self.gbAudioAvs = System.Windows.Forms.GroupBox()
		self.tbSepAudio = System.Windows.Forms.TextBox()
		self.btSepAudio = System.Windows.Forms.Button()
		self.chbSepAudio = System.Windows.Forms.CheckBox()
		self.audioSourceComboBox = System.Windows.Forms.ComboBox()
		self.downMixBox = System.Windows.Forms.CheckBox()
		self.normalizeBox = System.Windows.Forms.CheckBox()
		self.gbVideoSource = System.Windows.Forms.GroupBox()
		self.convertFPSCheckBox = System.Windows.Forms.CheckBox()
		self.sourceFrameRateCheckBox = System.Windows.Forms.CheckBox()
		self.label4 = System.Windows.Forms.Label()
		self.frameRateBox = System.Windows.Forms.ComboBox()
		self.videoSourceBox = System.Windows.Forms.ComboBox()
		self.gbResolution = System.Windows.Forms.GroupBox()
		self.sourceResolutionCheckBox = System.Windows.Forms.CheckBox()
		self.modBox = System.Windows.Forms.ComboBox()
		self.label3 = System.Windows.Forms.Label()
		self.resizerBox = System.Windows.Forms.ComboBox()
		self.label5 = System.Windows.Forms.Label()
		self.lockARCheckBox = System.Windows.Forms.CheckBox()
		self.label2 = System.Windows.Forms.Label()
		self.aspectRatioBox = System.Windows.Forms.ComboBox()
		self.label1 = System.Windows.Forms.Label()
		self.heightBox = System.Windows.Forms.TextBox()
		self.widthBox = System.Windows.Forms.TextBox()
		self.tabPage2 = System.Windows.Forms.TabPage()
		self.groupBox6 = System.Windows.Forms.GroupBox()
		self.muxerComboBox = System.Windows.Forms.ComboBox()
		self.groupBox5 = System.Windows.Forms.GroupBox()
		self.neroAacRateFactorBox = System.Windows.Forms.DomainUpDown()
		self.label14 = System.Windows.Forms.Label()
		self.neroAacRateControlBox = System.Windows.Forms.ComboBox()
		self.label13 = System.Windows.Forms.Label()
		self.groupBox4 = System.Windows.Forms.GroupBox()
		self.slow_firstpass = System.Windows.Forms.CheckBox()
		self.tune = System.Windows.Forms.ComboBox()
		self.label12 = System.Windows.Forms.Label()
		self.level = System.Windows.Forms.ComboBox()
		self.label11 = System.Windows.Forms.Label()
		self.profile = System.Windows.Forms.ComboBox()
		self.label10 = System.Windows.Forms.Label()
		self.label9 = System.Windows.Forms.Label()
		self.rateFactorBox = System.Windows.Forms.DomainUpDown()
		self.preset = System.Windows.Forms.ComboBox()
		self.label8 = System.Windows.Forms.Label()
		self.label7 = System.Windows.Forms.Label()
		self.rateControlBox = System.Windows.Forms.ComboBox()
		self.setDefaultButton = System.Windows.Forms.Button()
		self.cancelButton = System.Windows.Forms.Button()
		self.okButton = System.Windows.Forms.Button()
		self.saveFileDialog1 = System.Windows.Forms.SaveFileDialog()
		self.openFileDialog1 = System.Windows.Forms.OpenFileDialog()
		self.tabControl1.SuspendLayout()
		self.tabPage1.SuspendLayout()
		self.gbAudioAvs.SuspendLayout()
		self.gbVideoSource.SuspendLayout()
		self.gbResolution.SuspendLayout()
		self.tabPage2.SuspendLayout()
		self.groupBox6.SuspendLayout()
		self.groupBox5.SuspendLayout()
		self.groupBox4.SuspendLayout()
		self.SuspendLayout()
		# 
		# tabControl1
		# 
		self.tabControl1.Anchor = cast(System.Windows.Forms.AnchorStyles,(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.tabControl1.Controls.Add(self.tabPage1)
		self.tabControl1.Controls.Add(self.tabPage2)
		self.tabControl1.Location = System.Drawing.Point(12, 12)
		self.tabControl1.Name = "tabControl1"
		self.tabControl1.SelectedIndex = 0
		self.tabControl1.Size = System.Drawing.Size(440, 489)
		self.tabControl1.TabIndex = 0
		# 
		# tabPage1
		# 
		self.tabPage1.Controls.Add(self.cbAudioMode)
		self.tabPage1.Controls.Add(self.cbVideoMode)
		self.tabPage1.Controls.Add(self.label17)
		self.tabPage1.Controls.Add(self.label16)
		self.tabPage1.Controls.Add(self.destFileBox)
		self.tabPage1.Controls.Add(self.label6)
		self.tabPage1.Controls.Add(self.btOutBrowse)
		self.tabPage1.Controls.Add(self.gbAudioAvs)
		self.tabPage1.Controls.Add(self.gbVideoSource)
		self.tabPage1.Controls.Add(self.gbResolution)
		self.tabPage1.Location = System.Drawing.Point(4, 22)
		self.tabPage1.Name = "tabPage1"
		self.tabPage1.Padding = System.Windows.Forms.Padding(3)
		self.tabPage1.Size = System.Drawing.Size(432, 463)
		self.tabPage1.TabIndex = 0
		self.tabPage1.Text = "编辑"
		self.tabPage1.UseVisualStyleBackColor = true
		# 
		# cbAudioMode
		# 
		self.cbAudioMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.cbAudioMode.FormattingEnabled = true
		self.cbAudioMode.Items.AddRange((of object: "编码", "复制", "无"))
		self.cbAudioMode.Location = System.Drawing.Point(281, 24)
		self.cbAudioMode.Name = "cbAudioMode"
		self.cbAudioMode.Size = System.Drawing.Size(121, 20)
		self.cbAudioMode.TabIndex = 15
		self.cbAudioMode.SelectedIndexChanged += self.CbAudioModeSelectedIndexChanged as System.EventHandler
		# 
		# cbVideoMode
		# 
		self.cbVideoMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.cbVideoMode.FormattingEnabled = true
		self.cbVideoMode.Items.AddRange((of object: "编码", "复制", "无"))
		self.cbVideoMode.Location = System.Drawing.Point(74, 24)
		self.cbVideoMode.Name = "cbVideoMode"
		self.cbVideoMode.Size = System.Drawing.Size(121, 20)
		self.cbVideoMode.TabIndex = 14
		self.cbVideoMode.SelectedIndexChanged += self.CbVideoModeSelectedIndexChanged as System.EventHandler
		# 
		# label17
		# 
		self.label17.Location = System.Drawing.Point(214, 24)
		self.label17.Name = "label17"
		self.label17.Size = System.Drawing.Size(61, 23)
		self.label17.TabIndex = 13
		self.label17.Text = "音频模式"
		self.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# label16
		# 
		self.label16.Location = System.Drawing.Point(13, 24)
		self.label16.Name = "label16"
		self.label16.Size = System.Drawing.Size(55, 23)
		self.label16.TabIndex = 12
		self.label16.Text = "视频模式"
		self.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# destFileBox
		# 
		self.destFileBox.FormattingEnabled = true
		self.destFileBox.Location = System.Drawing.Point(74, 427)
		self.destFileBox.Name = "destFileBox"
		self.destFileBox.Size = System.Drawing.Size(266, 20)
		self.destFileBox.TabIndex = 11
		# 
		# label6
		# 
		self.label6.Location = System.Drawing.Point(6, 425)
		self.label6.Name = "label6"
		self.label6.Size = System.Drawing.Size(74, 23)
		self.label6.TabIndex = 8
		self.label6.Text = "输出文件："
		self.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# btOutBrowse
		# 
		self.btOutBrowse.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
		self.btOutBrowse.Location = System.Drawing.Point(346, 425)
		self.btOutBrowse.Name = "btOutBrowse"
		self.btOutBrowse.Size = System.Drawing.Size(75, 23)
		self.btOutBrowse.TabIndex = 7
		self.btOutBrowse.Text = "浏览"
		self.btOutBrowse.UseVisualStyleBackColor = true
		self.btOutBrowse.Click += self.BtOutBrowseClick as System.EventHandler
		# 
		# gbAudioAvs
		# 
		self.gbAudioAvs.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.gbAudioAvs.Controls.Add(self.tbSepAudio)
		self.gbAudioAvs.Controls.Add(self.btSepAudio)
		self.gbAudioAvs.Controls.Add(self.chbSepAudio)
		self.gbAudioAvs.Controls.Add(self.audioSourceComboBox)
		self.gbAudioAvs.Controls.Add(self.downMixBox)
		self.gbAudioAvs.Controls.Add(self.normalizeBox)
		self.gbAudioAvs.Location = System.Drawing.Point(6, 310)
		self.gbAudioAvs.Name = "gbAudioAvs"
		self.gbAudioAvs.Size = System.Drawing.Size(415, 109)
		self.gbAudioAvs.TabIndex = 5
		self.gbAudioAvs.TabStop = false
		self.gbAudioAvs.Text = "音频"
		# 
		# tbSepAudio
		# 
		self.tbSepAudio.Enabled = false
		self.tbSepAudio.Location = System.Drawing.Point(85, 67)
		self.tbSepAudio.Name = "tbSepAudio"
		self.tbSepAudio.ReadOnly = true
		self.tbSepAudio.Size = System.Drawing.Size(249, 21)
		self.tbSepAudio.TabIndex = 13
		# 
		# btSepAudio
		# 
		self.btSepAudio.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
		self.btSepAudio.Enabled = false
		self.btSepAudio.Location = System.Drawing.Point(340, 67)
		self.btSepAudio.Name = "btSepAudio"
		self.btSepAudio.Size = System.Drawing.Size(63, 23)
		self.btSepAudio.TabIndex = 12
		self.btSepAudio.Text = "浏览"
		self.btSepAudio.UseVisualStyleBackColor = true
		self.btSepAudio.Click += self.BtSepAudioClick as System.EventHandler
		# 
		# chbSepAudio
		# 
		self.chbSepAudio.Location = System.Drawing.Point(6, 65)
		self.chbSepAudio.Name = "chbSepAudio"
		self.chbSepAudio.Size = System.Drawing.Size(91, 24)
		self.chbSepAudio.TabIndex = 3
		self.chbSepAudio.Text = "独立音轨："
		self.chbSepAudio.UseVisualStyleBackColor = true
		self.chbSepAudio.CheckedChanged += self.ChbSepAudioCheckedChanged as System.EventHandler
		# 
		# audioSourceComboBox
		# 
		self.audioSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.audioSourceComboBox.FormattingEnabled = true
		self.audioSourceComboBox.Items.AddRange((of object: "FFAudioSource", "DirectShowSource"))
		self.audioSourceComboBox.Location = System.Drawing.Point(7, 20)
		self.audioSourceComboBox.Name = "audioSourceComboBox"
		self.audioSourceComboBox.Size = System.Drawing.Size(121, 20)
		self.audioSourceComboBox.TabIndex = 2
		# 
		# downMixBox
		# 
		self.downMixBox.Location = System.Drawing.Point(143, 20)
		self.downMixBox.Name = "downMixBox"
		self.downMixBox.Size = System.Drawing.Size(104, 24)
		self.downMixBox.TabIndex = 1
		self.downMixBox.Text = "立体声混音"
		self.downMixBox.UseVisualStyleBackColor = true
		# 
		# normalizeBox
		# 
		self.normalizeBox.Location = System.Drawing.Point(257, 20)
		self.normalizeBox.Name = "normalizeBox"
		self.normalizeBox.Size = System.Drawing.Size(104, 24)
		self.normalizeBox.TabIndex = 0
		self.normalizeBox.Text = "规格化"
		self.normalizeBox.UseVisualStyleBackColor = true
		# 
		# gbVideoSource
		# 
		self.gbVideoSource.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.gbVideoSource.Controls.Add(self.convertFPSCheckBox)
		self.gbVideoSource.Controls.Add(self.sourceFrameRateCheckBox)
		self.gbVideoSource.Controls.Add(self.label4)
		self.gbVideoSource.Controls.Add(self.frameRateBox)
		self.gbVideoSource.Controls.Add(self.videoSourceBox)
		self.gbVideoSource.Location = System.Drawing.Point(6, 225)
		self.gbVideoSource.Name = "gbVideoSource"
		self.gbVideoSource.Size = System.Drawing.Size(415, 79)
		self.gbVideoSource.TabIndex = 1
		self.gbVideoSource.TabStop = false
		self.gbVideoSource.Text = "源滤镜"
		# 
		# convertFPSCheckBox
		# 
		self.convertFPSCheckBox.Location = System.Drawing.Point(133, 17)
		self.convertFPSCheckBox.Name = "convertFPSCheckBox"
		self.convertFPSCheckBox.Size = System.Drawing.Size(147, 24)
		self.convertFPSCheckBox.TabIndex = 4
		self.convertFPSCheckBox.Text = "允许增减帧以维持同步"
		self.convertFPSCheckBox.UseVisualStyleBackColor = true
		# 
		# sourceFrameRateCheckBox
		# 
		self.sourceFrameRateCheckBox.Location = System.Drawing.Point(166, 44)
		self.sourceFrameRateCheckBox.Name = "sourceFrameRateCheckBox"
		self.sourceFrameRateCheckBox.Size = System.Drawing.Size(103, 24)
		self.sourceFrameRateCheckBox.TabIndex = 3
		self.sourceFrameRateCheckBox.Text = "与源视频相同"
		self.sourceFrameRateCheckBox.CheckedChanged += self.SourceFrameRateCheckBoxCheckedChanged as System.EventHandler
		# 
		# label4
		# 
		self.label4.Location = System.Drawing.Point(14, 44)
		self.label4.Name = "label4"
		self.label4.Size = System.Drawing.Size(49, 23)
		self.label4.TabIndex = 2
		self.label4.Text = "帧率："
		self.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# frameRateBox
		# 
		self.frameRateBox.FormattingEnabled = true
		self.frameRateBox.Items.AddRange((of object: "23.976", "25", "29.970", "30"))
		self.frameRateBox.Location = System.Drawing.Point(85, 48)
		self.frameRateBox.Name = "frameRateBox"
		self.frameRateBox.Size = System.Drawing.Size(67, 20)
		self.frameRateBox.TabIndex = 1
		self.frameRateBox.KeyPress += self.AllowFloat as System.Windows.Forms.KeyPressEventHandler
		# 
		# videoSourceBox
		# 
		self.videoSourceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.videoSourceBox.FormattingEnabled = true
		self.videoSourceBox.Items.AddRange((of object: "DirectShowSource", "FFVideoSource", "DSS2"))
		self.videoSourceBox.Location = System.Drawing.Point(6, 18)
		self.videoSourceBox.Name = "videoSourceBox"
		self.videoSourceBox.Size = System.Drawing.Size(121, 20)
		self.videoSourceBox.TabIndex = 0
		self.videoSourceBox.SelectedIndexChanged += self.VideoSourceBoxSelectedIndexChanged as System.EventHandler
		# 
		# gbResolution
		# 
		self.gbResolution.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.gbResolution.Controls.Add(self.sourceResolutionCheckBox)
		self.gbResolution.Controls.Add(self.modBox)
		self.gbResolution.Controls.Add(self.label3)
		self.gbResolution.Controls.Add(self.resizerBox)
		self.gbResolution.Controls.Add(self.label5)
		self.gbResolution.Controls.Add(self.lockARCheckBox)
		self.gbResolution.Controls.Add(self.label2)
		self.gbResolution.Controls.Add(self.aspectRatioBox)
		self.gbResolution.Controls.Add(self.label1)
		self.gbResolution.Controls.Add(self.heightBox)
		self.gbResolution.Controls.Add(self.widthBox)
		self.gbResolution.Location = System.Drawing.Point(6, 75)
		self.gbResolution.Name = "gbResolution"
		self.gbResolution.Size = System.Drawing.Size(415, 144)
		self.gbResolution.TabIndex = 0
		self.gbResolution.TabStop = false
		self.gbResolution.Text = "分辨率"
		# 
		# sourceResolutionCheckBox
		# 
		self.sourceResolutionCheckBox.Location = System.Drawing.Point(165, 18)
		self.sourceResolutionCheckBox.Name = "sourceResolutionCheckBox"
		self.sourceResolutionCheckBox.Size = System.Drawing.Size(104, 24)
		self.sourceResolutionCheckBox.TabIndex = 9
		self.sourceResolutionCheckBox.Text = "与源视频相同"
		self.sourceResolutionCheckBox.UseVisualStyleBackColor = true
		self.sourceResolutionCheckBox.CheckedChanged += self.SourceResolutionCheckBoxCheckedChanged as System.EventHandler
		# 
		# modBox
		# 
		self.modBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.modBox.FormattingEnabled = true
		self.modBox.Items.AddRange((of object: "2", "4", "8", "16", "32"))
		self.modBox.Location = System.Drawing.Point(340, 21)
		self.modBox.Name = "modBox"
		self.modBox.Size = System.Drawing.Size(60, 20)
		self.modBox.TabIndex = 8
		self.modBox.SelectedIndexChanged += self.ModBoxSelectedIndexChanged as System.EventHandler
		# 
		# label3
		# 
		self.label3.Location = System.Drawing.Point(275, 20)
		self.label3.Name = "label3"
		self.label3.Size = System.Drawing.Size(47, 23)
		self.label3.TabIndex = 7
		self.label3.Text = "mod"
		self.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		# 
		# resizerBox
		# 
		self.resizerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.resizerBox.FormattingEnabled = true
		self.resizerBox.Items.AddRange((of object: "LanczosResize", "Lanczos4Resize", "BicubicResize", "BilinearResize"))
		self.resizerBox.Location = System.Drawing.Point(85, 85)
		self.resizerBox.Name = "resizerBox"
		self.resizerBox.Size = System.Drawing.Size(121, 20)
		self.resizerBox.TabIndex = 2
		# 
		# label5
		# 
		self.label5.Location = System.Drawing.Point(6, 82)
		self.label5.Name = "label5"
		self.label5.Size = System.Drawing.Size(68, 23)
		self.label5.TabIndex = 3
		self.label5.Text = "缩放滤镜："
		self.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# lockARCheckBox
		# 
		self.lockARCheckBox.Location = System.Drawing.Point(180, 55)
		self.lockARCheckBox.Name = "lockARCheckBox"
		self.lockARCheckBox.Size = System.Drawing.Size(89, 24)
		self.lockARCheckBox.TabIndex = 5
		self.lockARCheckBox.Text = "锁定宽高比"
		self.lockARCheckBox.UseVisualStyleBackColor = true
		self.lockARCheckBox.CheckedChanged += self.LockARCheckBoxCheckedChanged as System.EventHandler
		# 
		# label2
		# 
		self.label2.Location = System.Drawing.Point(7, 55)
		self.label2.Name = "label2"
		self.label2.Size = System.Drawing.Size(56, 21)
		self.label2.TabIndex = 4
		self.label2.Text = "宽高比："
		self.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# aspectRatioBox
		# 
		self.aspectRatioBox.FormattingEnabled = true
		self.aspectRatioBox.Location = System.Drawing.Point(85, 55)
		self.aspectRatioBox.MaxLength = 8
		self.aspectRatioBox.Name = "aspectRatioBox"
		self.aspectRatioBox.Size = System.Drawing.Size(80, 20)
		self.aspectRatioBox.TabIndex = 3
		self.aspectRatioBox.Validating += self.ResolutionValidating as System.ComponentModel.CancelEventHandler
		self.aspectRatioBox.KeyPress += self.AllowFloat as System.Windows.Forms.KeyPressEventHandler
		self.aspectRatioBox.KeyUp += self.AspectRatioBoxKeyUp as System.Windows.Forms.KeyEventHandler
		# 
		# label1
		# 
		self.label1.Location = System.Drawing.Point(56, 21)
		self.label1.Name = "label1"
		self.label1.Size = System.Drawing.Size(19, 21)
		self.label1.TabIndex = 2
		self.label1.Text = "X"
		self.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		# 
		# heightBox
		# 
		self.heightBox.Location = System.Drawing.Point(85, 22)
		self.heightBox.MaxLength = 4
		self.heightBox.Name = "heightBox"
		self.heightBox.Size = System.Drawing.Size(43, 21)
		self.heightBox.TabIndex = 1
		self.heightBox.KeyUp += self.HeightBoxKeyUp as System.Windows.Forms.KeyEventHandler
		self.heightBox.KeyPress += self.AllowInteger as System.Windows.Forms.KeyPressEventHandler
		self.heightBox.Validating += self.ResolutionValidating as System.ComponentModel.CancelEventHandler
		# 
		# widthBox
		# 
		self.widthBox.Location = System.Drawing.Point(6, 21)
		self.widthBox.MaxLength = 4
		self.widthBox.Name = "widthBox"
		self.widthBox.Size = System.Drawing.Size(43, 21)
		self.widthBox.TabIndex = 0
		self.widthBox.KeyUp += self.WidthBoxKeyUp as System.Windows.Forms.KeyEventHandler
		self.widthBox.KeyPress += self.AllowInteger as System.Windows.Forms.KeyPressEventHandler
		self.widthBox.Validating += self.ResolutionValidating as System.ComponentModel.CancelEventHandler
		# 
		# tabPage2
		# 
		self.tabPage2.Controls.Add(self.groupBox6)
		self.tabPage2.Controls.Add(self.groupBox5)
		self.tabPage2.Controls.Add(self.groupBox4)
		self.tabPage2.Location = System.Drawing.Point(4, 22)
		self.tabPage2.Name = "tabPage2"
		self.tabPage2.Padding = System.Windows.Forms.Padding(3)
		self.tabPage2.Size = System.Drawing.Size(432, 463)
		self.tabPage2.TabIndex = 1
		self.tabPage2.Text = "编码器"
		self.tabPage2.UseVisualStyleBackColor = true
		# 
		# groupBox6
		# 
		self.groupBox6.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox6.Controls.Add(self.muxerComboBox)
		self.groupBox6.Location = System.Drawing.Point(6, 257)
		self.groupBox6.Name = "groupBox6"
		self.groupBox6.Size = System.Drawing.Size(420, 200)
		self.groupBox6.TabIndex = 2
		self.groupBox6.TabStop = false
		self.groupBox6.Text = "容器"
		# 
		# muxerComboBox
		# 
		self.muxerComboBox.FormattingEnabled = true
		self.muxerComboBox.Items.AddRange((of object: "MP4"))
		self.muxerComboBox.Location = System.Drawing.Point(6, 20)
		self.muxerComboBox.Name = "muxerComboBox"
		self.muxerComboBox.Size = System.Drawing.Size(121, 20)
		self.muxerComboBox.TabIndex = 0
		# 
		# groupBox5
		# 
		self.groupBox5.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox5.Controls.Add(self.neroAacRateFactorBox)
		self.groupBox5.Controls.Add(self.label14)
		self.groupBox5.Controls.Add(self.neroAacRateControlBox)
		self.groupBox5.Controls.Add(self.label13)
		self.groupBox5.Location = System.Drawing.Point(6, 168)
		self.groupBox5.Name = "groupBox5"
		self.groupBox5.Size = System.Drawing.Size(420, 83)
		self.groupBox5.TabIndex = 1
		self.groupBox5.TabStop = false
		self.groupBox5.Text = "NeroAac"
		# 
		# neroAacRateFactorBox
		# 
		self.neroAacRateFactorBox.Location = System.Drawing.Point(71, 43)
		self.neroAacRateFactorBox.Name = "neroAacRateFactorBox"
		self.neroAacRateFactorBox.Size = System.Drawing.Size(75, 21)
		self.neroAacRateFactorBox.TabIndex = 4
		self.neroAacRateFactorBox.Validating += self.NeroAacRateFactorBoxValidating as System.ComponentModel.CancelEventHandler
		self.neroAacRateFactorBox.KeyPress += self.AllowFloat as System.Windows.Forms.KeyPressEventHandler
		# 
		# label14
		# 
		self.label14.Location = System.Drawing.Point(9, 43)
		self.label14.Name = "label14"
		self.label14.Size = System.Drawing.Size(56, 21)
		self.label14.TabIndex = 2
		self.label14.Text = "质量"
		# 
		# neroAacRateControlBox
		# 
		self.neroAacRateControlBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.neroAacRateControlBox.FormattingEnabled = true
		self.neroAacRateControlBox.Items.AddRange((of object: "质量", "可变码率", "恒定码率"))
		self.neroAacRateControlBox.Location = System.Drawing.Point(71, 17)
		self.neroAacRateControlBox.Name = "neroAacRateControlBox"
		self.neroAacRateControlBox.Size = System.Drawing.Size(127, 20)
		self.neroAacRateControlBox.TabIndex = 1
		self.neroAacRateControlBox.SelectedIndexChanged += self.NeroAacRateControlBoxSelectedIndexChanged as System.EventHandler
		# 
		# label13
		# 
		self.label13.Location = System.Drawing.Point(6, 17)
		self.label13.Name = "label13"
		self.label13.Size = System.Drawing.Size(59, 20)
		self.label13.TabIndex = 0
		self.label13.Text = "码率控制"
		# 
		# groupBox4
		# 
		self.groupBox4.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox4.Controls.Add(self.slow_firstpass)
		self.groupBox4.Controls.Add(self.tune)
		self.groupBox4.Controls.Add(self.label12)
		self.groupBox4.Controls.Add(self.level)
		self.groupBox4.Controls.Add(self.label11)
		self.groupBox4.Controls.Add(self.profile)
		self.groupBox4.Controls.Add(self.label10)
		self.groupBox4.Controls.Add(self.label9)
		self.groupBox4.Controls.Add(self.rateFactorBox)
		self.groupBox4.Controls.Add(self.preset)
		self.groupBox4.Controls.Add(self.label8)
		self.groupBox4.Controls.Add(self.label7)
		self.groupBox4.Controls.Add(self.rateControlBox)
		self.groupBox4.Location = System.Drawing.Point(6, 6)
		self.groupBox4.Name = "groupBox4"
		self.groupBox4.Size = System.Drawing.Size(420, 156)
		self.groupBox4.TabIndex = 0
		self.groupBox4.TabStop = false
		self.groupBox4.Text = "x264"
		# 
		# slow_firstpass
		# 
		self.slow_firstpass.Location = System.Drawing.Point(152, 41)
		self.slow_firstpass.Name = "slow_firstpass"
		self.slow_firstpass.Size = System.Drawing.Size(111, 20)
		self.slow_firstpass.TabIndex = 12
		self.slow_firstpass.Text = "slow-firstpass"
		self.slow_firstpass.UseVisualStyleBackColor = true
		self.slow_firstpass.CheckedChanged += self.BoolChanged as System.EventHandler
		# 
		# tune
		# 
		self.tune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.tune.FormattingEnabled = true
		self.tune.Items.AddRange((of object: "无", "film", "animation", "grain", "stillimage", "psnr", "ssim", "fastdecode", "zerolatency", "touhou"))
		self.tune.Location = System.Drawing.Point(71, 120)
		self.tune.Name = "tune"
		self.tune.Size = System.Drawing.Size(75, 20)
		self.tune.TabIndex = 11
		self.tune.SelectedIndexChanged += self.StringChanged as System.EventHandler
		# 
		# label12
		# 
		self.label12.Location = System.Drawing.Point(9, 120)
		self.label12.Name = "label12"
		self.label12.Size = System.Drawing.Size(59, 20)
		self.label12.TabIndex = 10
		self.label12.Text = "Tune"
		# 
		# level
		# 
		self.level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.level.FormattingEnabled = true
		self.level.Items.AddRange((of object: "自动", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1"))
		self.level.Location = System.Drawing.Point(215, 94)
		self.level.Name = "level"
		self.level.Size = System.Drawing.Size(75, 20)
		self.level.TabIndex = 9
		self.level.SelectedIndexChanged += self.StringChanged as System.EventHandler
		# 
		# label11
		# 
		self.label11.Location = System.Drawing.Point(167, 94)
		self.label11.Name = "label11"
		self.label11.Size = System.Drawing.Size(42, 20)
		self.label11.TabIndex = 8
		self.label11.Text = "Level"
		# 
		# profile
		# 
		self.profile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.profile.FormattingEnabled = true
		self.profile.Items.AddRange((of object: "自动", "baseline", "main", "high"))
		self.profile.Location = System.Drawing.Point(71, 94)
		self.profile.Name = "profile"
		self.profile.Size = System.Drawing.Size(75, 20)
		self.profile.TabIndex = 7
		self.profile.SelectedIndexChanged += self.StringChanged as System.EventHandler
		# 
		# label10
		# 
		self.label10.Location = System.Drawing.Point(6, 94)
		self.label10.Name = "label10"
		self.label10.Size = System.Drawing.Size(59, 20)
		self.label10.TabIndex = 6
		self.label10.Text = "Profile"
		# 
		# label9
		# 
		self.label9.Location = System.Drawing.Point(9, 40)
		self.label9.Name = "label9"
		self.label9.Size = System.Drawing.Size(56, 21)
		self.label9.TabIndex = 5
		self.label9.Text = "量化器"
		# 
		# rateFactorBox
		# 
		self.rateFactorBox.Location = System.Drawing.Point(71, 40)
		self.rateFactorBox.Name = "rateFactorBox"
		self.rateFactorBox.Size = System.Drawing.Size(75, 21)
		self.rateFactorBox.Sorted = true
		self.rateFactorBox.TabIndex = 4
		self.rateFactorBox.Validating += self.RateFactorBoxValidating as System.ComponentModel.CancelEventHandler
		self.rateFactorBox.KeyPress += self.AllowFloat as System.Windows.Forms.KeyPressEventHandler
		# 
		# preset
		# 
		self.preset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.preset.FormattingEnabled = true
		self.preset.Items.AddRange((of object: "ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"))
		self.preset.Location = System.Drawing.Point(71, 67)
		self.preset.Name = "preset"
		self.preset.Size = System.Drawing.Size(127, 20)
		self.preset.TabIndex = 3
		self.preset.SelectedIndexChanged += self.StringChanged as System.EventHandler
		# 
		# label8
		# 
		self.label8.Location = System.Drawing.Point(6, 68)
		self.label8.Name = "label8"
		self.label8.Size = System.Drawing.Size(59, 20)
		self.label8.TabIndex = 2
		self.label8.Text = "Preset"
		# 
		# label7
		# 
		self.label7.Location = System.Drawing.Point(6, 14)
		self.label7.Name = "label7"
		self.label7.Size = System.Drawing.Size(59, 20)
		self.label7.TabIndex = 1
		self.label7.Text = "码率控制"
		# 
		# rateControlBox
		# 
		self.rateControlBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		self.rateControlBox.FormattingEnabled = true
		self.rateControlBox.Items.AddRange((of object: "恒定质量(crf)", "恒定量化器(qp)", "码率 1pass"))
		self.rateControlBox.Location = System.Drawing.Point(71, 14)
		self.rateControlBox.Name = "rateControlBox"
		self.rateControlBox.Size = System.Drawing.Size(127, 20)
		self.rateControlBox.TabIndex = 0
		self.rateControlBox.SelectedIndexChanged += self.RateControlBoxSelectedIndexChanged as System.EventHandler
		# 
		# setDefaultButton
		# 
		self.setDefaultButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))
		self.setDefaultButton.Location = System.Drawing.Point(12, 507)
		self.setDefaultButton.Name = "setDefaultButton"
		self.setDefaultButton.Size = System.Drawing.Size(75, 23)
		self.setDefaultButton.TabIndex = 4
		self.setDefaultButton.Text = "设为默认"
		self.setDefaultButton.UseVisualStyleBackColor = true
		self.setDefaultButton.Click += self.SetDefaultButtonClick as System.EventHandler
		# 
		# cancelButton
		# 
		self.cancelButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
		self.cancelButton.Location = System.Drawing.Point(379, 507)
		self.cancelButton.Name = "cancelButton"
		self.cancelButton.Size = System.Drawing.Size(73, 23)
		self.cancelButton.TabIndex = 3
		self.cancelButton.Text = "取消"
		self.cancelButton.UseVisualStyleBackColor = true
		self.cancelButton.Click += self.CancelButtonClick as System.EventHandler
		# 
		# okButton
		# 
		self.okButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.okButton.Location = System.Drawing.Point(298, 507)
		self.okButton.Name = "okButton"
		self.okButton.Size = System.Drawing.Size(75, 23)
		self.okButton.TabIndex = 2
		self.okButton.Text = "确定"
		self.okButton.UseVisualStyleBackColor = true
		self.okButton.Click += self.OkButtonClick as System.EventHandler
		# 
		# openFileDialog1
		# 
		self.openFileDialog1.FileName = "openFileDialog1"
		# 
		# MediaSettingForm
		# 
		self.AcceptButton = self.okButton
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.CancelButton = self.cancelButton
		self.ClientSize = System.Drawing.Size(464, 542)
		self.Controls.Add(self.tabControl1)
		self.Controls.Add(self.setDefaultButton)
		self.Controls.Add(self.okButton)
		self.Controls.Add(self.cancelButton)
		self.MinimumSize = System.Drawing.Size(480, 580)
		self.Name = "MediaSettingForm"
		self.Text = "设置"
		self.Load += self.MediaSettingFormLoad as System.EventHandler
		self.FormClosed += self.MediaSettingFormFormClosed as System.Windows.Forms.FormClosedEventHandler
		self.tabControl1.ResumeLayout(false)
		self.tabPage1.ResumeLayout(false)
		self.gbAudioAvs.ResumeLayout(false)
		self.gbAudioAvs.PerformLayout()
		self.gbVideoSource.ResumeLayout(false)
		self.gbResolution.ResumeLayout(false)
		self.gbResolution.PerformLayout()
		self.tabPage2.ResumeLayout(false)
		self.groupBox6.ResumeLayout(false)
		self.groupBox5.ResumeLayout(false)
		self.groupBox4.ResumeLayout(false)
		self.ResumeLayout(false)
	private label16 as System.Windows.Forms.Label
	private label17 as System.Windows.Forms.Label
	private cbVideoMode as System.Windows.Forms.ComboBox
	private cbAudioMode as System.Windows.Forms.ComboBox
	private tbSepAudio as System.Windows.Forms.TextBox
	private openFileDialog1 as System.Windows.Forms.OpenFileDialog
	private btSepAudio as System.Windows.Forms.Button
	private chbSepAudio as System.Windows.Forms.CheckBox
	private btOutBrowse as System.Windows.Forms.Button
	private gbVideoSource as System.Windows.Forms.GroupBox
	private gbAudioAvs as System.Windows.Forms.GroupBox
	private gbResolution as System.Windows.Forms.GroupBox
	private audioSourceComboBox as System.Windows.Forms.ComboBox
	private muxerComboBox as System.Windows.Forms.ComboBox
	private groupBox6 as System.Windows.Forms.GroupBox
	private neroAacRateFactorBox as System.Windows.Forms.DomainUpDown
	private neroAacRateControlBox as System.Windows.Forms.ComboBox
	private label13 as System.Windows.Forms.Label
	private label14 as System.Windows.Forms.Label
	private tune as System.Windows.Forms.ComboBox
	private level as System.Windows.Forms.ComboBox
	private profile as System.Windows.Forms.ComboBox
	private preset as System.Windows.Forms.ComboBox
	private rateFactorBox as System.Windows.Forms.DomainUpDown
	private slow_firstpass as System.Windows.Forms.CheckBox
	private rateControlBox as System.Windows.Forms.ComboBox
	private label7 as System.Windows.Forms.Label
	private label8 as System.Windows.Forms.Label
	private label9 as System.Windows.Forms.Label
	private label10 as System.Windows.Forms.Label
	private label11 as System.Windows.Forms.Label
	private label12 as System.Windows.Forms.Label
	private groupBox4 as System.Windows.Forms.GroupBox
	private groupBox5 as System.Windows.Forms.GroupBox
	private label6 as System.Windows.Forms.Label
	private destFileBox as System.Windows.Forms.ComboBox
	private saveFileDialog1 as System.Windows.Forms.SaveFileDialog
	private normalizeBox as System.Windows.Forms.CheckBox
	private downMixBox as System.Windows.Forms.CheckBox
	private sourceResolutionCheckBox as System.Windows.Forms.CheckBox
	private setDefaultButton as System.Windows.Forms.Button
	private cancelButton as System.Windows.Forms.Button
	private okButton as System.Windows.Forms.Button
	private convertFPSCheckBox as System.Windows.Forms.CheckBox
	private sourceFrameRateCheckBox as System.Windows.Forms.CheckBox
	private frameRateBox as System.Windows.Forms.ComboBox
	private videoSourceBox as System.Windows.Forms.ComboBox
	private resizerBox as System.Windows.Forms.ComboBox
	private label4 as System.Windows.Forms.Label
	private label5 as System.Windows.Forms.Label
	private modBox as System.Windows.Forms.ComboBox
	private label3 as System.Windows.Forms.Label
	private lockARCheckBox as System.Windows.Forms.CheckBox
	private aspectRatioBox as System.Windows.Forms.ComboBox
	private heightBox as System.Windows.Forms.TextBox
	private widthBox as System.Windows.Forms.TextBox
	private label1 as System.Windows.Forms.Label
	private label2 as System.Windows.Forms.Label
	private tabPage2 as System.Windows.Forms.TabPage
	private tabPage1 as System.Windows.Forms.TabPage
	private tabControl1 as System.Windows.Forms.TabControl


namespace CXGUI.GUI
{
	partial class JobSettingForm
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
            this.previewButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.videoEditTabPage = new System.Windows.Forms.TabPage();
            this.cbVideoMode = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.destFileBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btOutBrowse = new System.Windows.Forms.Button();
            this.gbVideoSource = new System.Windows.Forms.GroupBox();
            this.convertFPSCheckBox = new System.Windows.Forms.CheckBox();
            this.sourceFrameRateCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.frameRateBox = new System.Windows.Forms.ComboBox();
            this.videoSourceBox = new System.Windows.Forms.ComboBox();
            this.gbResolution = new System.Windows.Forms.GroupBox();
            this.lockToSourceARCheckBox = new System.Windows.Forms.CheckBox();
            this.sourceResolutionCheckBox = new System.Windows.Forms.CheckBox();
            this.modBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.resizerBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.allowAutoChangeARCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.aspectRatioBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.audioEditTabPage = new System.Windows.Forms.TabPage();
            this.normalizeBox = new System.Windows.Forms.CheckBox();
            this.downMixBox = new System.Windows.Forms.CheckBox();
            this.btSepAudio = new System.Windows.Forms.Button();
            this.tbSepAudio = new System.Windows.Forms.TextBox();
            this.cbAudioMode = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chbSepAudio = new System.Windows.Forms.CheckBox();
            this.audioSourceComboBox = new System.Windows.Forms.ComboBox();
            this.avsInputTabPage = new System.Windows.Forms.TabPage();
            this.encTabPage = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.muxerComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.neroAacRateFactorBox = new System.Windows.Forms.DomainUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.neroAacRateControlBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.useCustomCmdBox = new System.Windows.Forms.CheckBox();
            this.editCmdButton = new System.Windows.Forms.Button();
            this.tune = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.level = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.profile = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rateFactorBox = new System.Windows.Forms.DomainUpDown();
            this.preset = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rateControlBox = new System.Windows.Forms.ComboBox();
            this.subtitleTabPage = new System.Windows.Forms.TabPage();
            this.customSubCheckBox = new System.Windows.Forms.CheckBox();
            this.customSubGroupBox = new System.Windows.Forms.GroupBox();
            this.fontBottomBox = new System.Windows.Forms.DomainUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.fontButton = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.fontSizeBox = new System.Windows.Forms.DomainUpDown();
            this.subtitleTextBox = new System.Windows.Forms.TextBox();
            this.subtitleButton = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.profileBox = new System.Windows.Forms.ComboBox();
            this.saveProfileButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tabControl1.SuspendLayout();
            this.videoEditTabPage.SuspendLayout();
            this.gbVideoSource.SuspendLayout();
            this.gbResolution.SuspendLayout();
            this.audioEditTabPage.SuspendLayout();
            this.encTabPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.subtitleTabPage.SuspendLayout();
            this.customSubGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewButton
            // 
            this.previewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previewButton.Location = new System.Drawing.Point(12, 508);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(75, 23);
            this.previewButton.TabIndex = 19;
            this.previewButton.Text = "预览";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.PreviewButtonClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.videoEditTabPage);
            this.tabControl1.Controls.Add(this.audioEditTabPage);
            this.tabControl1.Controls.Add(this.avsInputTabPage);
            this.tabControl1.Controls.Add(this.encTabPage);
            this.tabControl1.Controls.Add(this.subtitleTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 490);
            this.tabControl1.TabIndex = 13;
            // 
            // videoEditTabPage
            // 
            this.videoEditTabPage.Controls.Add(this.cbVideoMode);
            this.videoEditTabPage.Controls.Add(this.label16);
            this.videoEditTabPage.Controls.Add(this.destFileBox);
            this.videoEditTabPage.Controls.Add(this.label6);
            this.videoEditTabPage.Controls.Add(this.btOutBrowse);
            this.videoEditTabPage.Controls.Add(this.gbVideoSource);
            this.videoEditTabPage.Controls.Add(this.gbResolution);
            this.videoEditTabPage.Location = new System.Drawing.Point(4, 22);
            this.videoEditTabPage.Name = "videoEditTabPage";
            this.videoEditTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.videoEditTabPage.Size = new System.Drawing.Size(432, 464);
            this.videoEditTabPage.TabIndex = 0;
            this.videoEditTabPage.Text = "视频";
            this.videoEditTabPage.UseVisualStyleBackColor = true;
            // 
            // cbVideoMode
            // 
            this.cbVideoMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoMode.FormattingEnabled = true;
            this.cbVideoMode.Items.AddRange(new object[] {
            "编码",
            "复制",
            "无"});
            this.cbVideoMode.Location = new System.Drawing.Point(74, 24);
            this.cbVideoMode.Name = "cbVideoMode";
            this.cbVideoMode.Size = new System.Drawing.Size(121, 20);
            this.cbVideoMode.TabIndex = 14;
            this.cbVideoMode.SelectedIndexChanged += new System.EventHandler(this.CbVideoModeSelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(13, 24);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 23);
            this.label16.TabIndex = 12;
            this.label16.Text = "视频模式";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // destFileBox
            // 
            this.destFileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.destFileBox.FormattingEnabled = true;
            this.destFileBox.Location = new System.Drawing.Point(74, 427);
            this.destFileBox.Name = "destFileBox";
            this.destFileBox.Size = new System.Drawing.Size(266, 20);
            this.destFileBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 425);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "输出文件：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btOutBrowse
            // 
            this.btOutBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOutBrowse.Location = new System.Drawing.Point(346, 425);
            this.btOutBrowse.Name = "btOutBrowse";
            this.btOutBrowse.Size = new System.Drawing.Size(75, 23);
            this.btOutBrowse.TabIndex = 7;
            this.btOutBrowse.Text = "浏览";
            this.btOutBrowse.UseVisualStyleBackColor = true;
            this.btOutBrowse.Click += new System.EventHandler(this.BtOutBrowseClick);
            // 
            // gbVideoSource
            // 
            this.gbVideoSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVideoSource.Controls.Add(this.convertFPSCheckBox);
            this.gbVideoSource.Controls.Add(this.sourceFrameRateCheckBox);
            this.gbVideoSource.Controls.Add(this.label4);
            this.gbVideoSource.Controls.Add(this.frameRateBox);
            this.gbVideoSource.Controls.Add(this.videoSourceBox);
            this.gbVideoSource.Location = new System.Drawing.Point(6, 225);
            this.gbVideoSource.Name = "gbVideoSource";
            this.gbVideoSource.Size = new System.Drawing.Size(415, 79);
            this.gbVideoSource.TabIndex = 1;
            this.gbVideoSource.TabStop = false;
            this.gbVideoSource.Text = "源滤镜";
            // 
            // convertFPSCheckBox
            // 
            this.convertFPSCheckBox.Location = new System.Drawing.Point(133, 17);
            this.convertFPSCheckBox.Name = "convertFPSCheckBox";
            this.convertFPSCheckBox.Size = new System.Drawing.Size(147, 24);
            this.convertFPSCheckBox.TabIndex = 4;
            this.convertFPSCheckBox.Text = "允许增减帧以维持同步";
            this.convertFPSCheckBox.UseVisualStyleBackColor = true;
            // 
            // sourceFrameRateCheckBox
            // 
            this.sourceFrameRateCheckBox.Location = new System.Drawing.Point(166, 44);
            this.sourceFrameRateCheckBox.Name = "sourceFrameRateCheckBox";
            this.sourceFrameRateCheckBox.Size = new System.Drawing.Size(103, 24);
            this.sourceFrameRateCheckBox.TabIndex = 3;
            this.sourceFrameRateCheckBox.Text = "与源视频相同";
            this.sourceFrameRateCheckBox.CheckedChanged += new System.EventHandler(this.SourceFrameRateCheckBoxCheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "帧率：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frameRateBox
            // 
            this.frameRateBox.FormattingEnabled = true;
            this.frameRateBox.Items.AddRange(new object[] {
            "23.976",
            "25",
            "29.970",
            "30"});
            this.frameRateBox.Location = new System.Drawing.Point(85, 48);
            this.frameRateBox.Name = "frameRateBox";
            this.frameRateBox.Size = new System.Drawing.Size(67, 20);
            this.frameRateBox.TabIndex = 1;
            this.frameRateBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowFloat);
            this.frameRateBox.Validating += new System.ComponentModel.CancelEventHandler(this.FrameRateBoxValidating);
            // 
            // videoSourceBox
            // 
            this.videoSourceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoSourceBox.FormattingEnabled = true;
            this.videoSourceBox.Items.AddRange(new object[] {
            "DirectShowSource",
            "FFVideoSource",
            "DSS2"});
            this.videoSourceBox.Location = new System.Drawing.Point(6, 18);
            this.videoSourceBox.Name = "videoSourceBox";
            this.videoSourceBox.Size = new System.Drawing.Size(121, 20);
            this.videoSourceBox.TabIndex = 0;
            // 
            // gbResolution
            // 
            this.gbResolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResolution.Controls.Add(this.lockToSourceARCheckBox);
            this.gbResolution.Controls.Add(this.sourceResolutionCheckBox);
            this.gbResolution.Controls.Add(this.modBox);
            this.gbResolution.Controls.Add(this.label3);
            this.gbResolution.Controls.Add(this.resizerBox);
            this.gbResolution.Controls.Add(this.label5);
            this.gbResolution.Controls.Add(this.allowAutoChangeARCheckBox);
            this.gbResolution.Controls.Add(this.label2);
            this.gbResolution.Controls.Add(this.aspectRatioBox);
            this.gbResolution.Controls.Add(this.label1);
            this.gbResolution.Controls.Add(this.heightBox);
            this.gbResolution.Controls.Add(this.widthBox);
            this.gbResolution.Location = new System.Drawing.Point(6, 75);
            this.gbResolution.Name = "gbResolution";
            this.gbResolution.Size = new System.Drawing.Size(415, 144);
            this.gbResolution.TabIndex = 0;
            this.gbResolution.TabStop = false;
            this.gbResolution.Text = "分辨率";
            // 
            // lockToSourceARCheckBox
            // 
            this.lockToSourceARCheckBox.Location = new System.Drawing.Point(275, 55);
            this.lockToSourceARCheckBox.Name = "lockToSourceARCheckBox";
            this.lockToSourceARCheckBox.Size = new System.Drawing.Size(120, 24);
            this.lockToSourceARCheckBox.TabIndex = 10;
            this.lockToSourceARCheckBox.Text = "锁定为源宽高比";
            this.lockToSourceARCheckBox.UseVisualStyleBackColor = true;
            this.lockToSourceARCheckBox.CheckedChanged += new System.EventHandler(this.UseSourceARCheckedChanged);
            // 
            // sourceResolutionCheckBox
            // 
            this.sourceResolutionCheckBox.Location = new System.Drawing.Point(165, 18);
            this.sourceResolutionCheckBox.Name = "sourceResolutionCheckBox";
            this.sourceResolutionCheckBox.Size = new System.Drawing.Size(104, 24);
            this.sourceResolutionCheckBox.TabIndex = 9;
            this.sourceResolutionCheckBox.Text = "与源视频相同";
            this.sourceResolutionCheckBox.UseVisualStyleBackColor = true;
            this.sourceResolutionCheckBox.CheckedChanged += new System.EventHandler(this.SourceResolutionCheckBoxCheckedChanged);
            // 
            // modBox
            // 
            this.modBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modBox.FormattingEnabled = true;
            this.modBox.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32"});
            this.modBox.Location = new System.Drawing.Point(340, 21);
            this.modBox.Name = "modBox";
            this.modBox.Size = new System.Drawing.Size(60, 20);
            this.modBox.TabIndex = 8;
            this.modBox.SelectedIndexChanged += new System.EventHandler(this.ModBoxSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(275, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "mod";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // resizerBox
            // 
            this.resizerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resizerBox.FormattingEnabled = true;
            this.resizerBox.Items.AddRange(new object[] {
            "LanczosResize",
            "Lanczos4Resize",
            "BicubicResize",
            "BilinearResize"});
            this.resizerBox.Location = new System.Drawing.Point(85, 85);
            this.resizerBox.Name = "resizerBox";
            this.resizerBox.Size = new System.Drawing.Size(121, 20);
            this.resizerBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "缩放滤镜：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // allowAutoChangeARCheckBox
            // 
            this.allowAutoChangeARCheckBox.Location = new System.Drawing.Point(180, 55);
            this.allowAutoChangeARCheckBox.Name = "allowAutoChangeARCheckBox";
            this.allowAutoChangeARCheckBox.Size = new System.Drawing.Size(100, 24);
            this.allowAutoChangeARCheckBox.TabIndex = 5;
            this.allowAutoChangeARCheckBox.Text = "允许自动更改";
            this.allowAutoChangeARCheckBox.UseVisualStyleBackColor = true;
            this.allowAutoChangeARCheckBox.CheckedChanged += new System.EventHandler(this.AllowAutoChangeARCheckBoxCheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "宽高比：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aspectRatioBox
            // 
            this.aspectRatioBox.FormattingEnabled = true;
            this.aspectRatioBox.Location = new System.Drawing.Point(85, 55);
            this.aspectRatioBox.MaxLength = 8;
            this.aspectRatioBox.Name = "aspectRatioBox";
            this.aspectRatioBox.Size = new System.Drawing.Size(80, 20);
            this.aspectRatioBox.TabIndex = 3;
            this.aspectRatioBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowFloat);
            this.aspectRatioBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AspectRatioBoxKeyUp);
            this.aspectRatioBox.Validating += new System.ComponentModel.CancelEventHandler(this.ResolutionValidating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // heightBox
            // 
            this.heightBox.Location = new System.Drawing.Point(85, 22);
            this.heightBox.MaxLength = 4;
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(43, 21);
            this.heightBox.TabIndex = 1;
            this.heightBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowInteger);
            this.heightBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HeightBoxKeyUp);
            this.heightBox.Validating += new System.ComponentModel.CancelEventHandler(this.ResolutionValidating);
            // 
            // widthBox
            // 
            this.widthBox.Location = new System.Drawing.Point(6, 21);
            this.widthBox.MaxLength = 4;
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(43, 21);
            this.widthBox.TabIndex = 0;
            this.widthBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowInteger);
            this.widthBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WidthBoxKeyUp);
            this.widthBox.Validating += new System.ComponentModel.CancelEventHandler(this.ResolutionValidating);
            // 
            // audioEditTabPage
            // 
            this.audioEditTabPage.Controls.Add(this.normalizeBox);
            this.audioEditTabPage.Controls.Add(this.downMixBox);
            this.audioEditTabPage.Controls.Add(this.btSepAudio);
            this.audioEditTabPage.Controls.Add(this.tbSepAudio);
            this.audioEditTabPage.Controls.Add(this.cbAudioMode);
            this.audioEditTabPage.Controls.Add(this.label17);
            this.audioEditTabPage.Controls.Add(this.chbSepAudio);
            this.audioEditTabPage.Controls.Add(this.audioSourceComboBox);
            this.audioEditTabPage.Location = new System.Drawing.Point(4, 22);
            this.audioEditTabPage.Name = "audioEditTabPage";
            this.audioEditTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.audioEditTabPage.Size = new System.Drawing.Size(432, 464);
            this.audioEditTabPage.TabIndex = 4;
            this.audioEditTabPage.Text = "音频";
            this.audioEditTabPage.UseVisualStyleBackColor = true;
            // 
            // normalizeBox
            // 
            this.normalizeBox.Location = new System.Drawing.Point(288, 61);
            this.normalizeBox.Name = "normalizeBox";
            this.normalizeBox.Size = new System.Drawing.Size(104, 24);
            this.normalizeBox.TabIndex = 0;
            this.normalizeBox.Text = "规格化";
            this.normalizeBox.UseVisualStyleBackColor = true;
            // 
            // downMixBox
            // 
            this.downMixBox.Location = new System.Drawing.Point(178, 61);
            this.downMixBox.Name = "downMixBox";
            this.downMixBox.Size = new System.Drawing.Size(104, 24);
            this.downMixBox.TabIndex = 1;
            this.downMixBox.Text = "立体声混音";
            this.downMixBox.UseVisualStyleBackColor = true;
            // 
            // btSepAudio
            // 
            this.btSepAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSepAudio.Enabled = false;
            this.btSepAudio.Location = new System.Drawing.Point(351, 104);
            this.btSepAudio.Name = "btSepAudio";
            this.btSepAudio.Size = new System.Drawing.Size(63, 23);
            this.btSepAudio.TabIndex = 12;
            this.btSepAudio.Text = "浏览";
            this.btSepAudio.UseVisualStyleBackColor = true;
            this.btSepAudio.Click += new System.EventHandler(this.BtSepAudioClick);
            // 
            // tbSepAudio
            // 
            this.tbSepAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSepAudio.Enabled = false;
            this.tbSepAudio.Location = new System.Drawing.Point(96, 106);
            this.tbSepAudio.Name = "tbSepAudio";
            this.tbSepAudio.ReadOnly = true;
            this.tbSepAudio.Size = new System.Drawing.Size(249, 21);
            this.tbSepAudio.TabIndex = 13;
            // 
            // cbAudioMode
            // 
            this.cbAudioMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioMode.FormattingEnabled = true;
            this.cbAudioMode.Items.AddRange(new object[] {
            "编码",
            "复制",
            "无"});
            this.cbAudioMode.Location = new System.Drawing.Point(74, 24);
            this.cbAudioMode.Name = "cbAudioMode";
            this.cbAudioMode.Size = new System.Drawing.Size(121, 20);
            this.cbAudioMode.TabIndex = 17;
            this.cbAudioMode.SelectedIndexChanged += new System.EventHandler(this.CbAudioModeSelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(13, 24);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(61, 23);
            this.label17.TabIndex = 16;
            this.label17.Text = "音频模式";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chbSepAudio
            // 
            this.chbSepAudio.Location = new System.Drawing.Point(13, 104);
            this.chbSepAudio.Name = "chbSepAudio";
            this.chbSepAudio.Size = new System.Drawing.Size(88, 24);
            this.chbSepAudio.TabIndex = 3;
            this.chbSepAudio.Text = "独立音轨：";
            this.chbSepAudio.UseVisualStyleBackColor = true;
            this.chbSepAudio.CheckedChanged += new System.EventHandler(this.ChbSepAudioCheckedChanged);
            // 
            // audioSourceComboBox
            // 
            this.audioSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioSourceComboBox.FormattingEnabled = true;
            this.audioSourceComboBox.Items.AddRange(new object[] {
            "FFAudioSource",
            "DirectShowSource"});
            this.audioSourceComboBox.Location = new System.Drawing.Point(51, 63);
            this.audioSourceComboBox.Name = "audioSourceComboBox";
            this.audioSourceComboBox.Size = new System.Drawing.Size(121, 20);
            this.audioSourceComboBox.TabIndex = 2;
            // 
            // avsInputTabPage
            // 
            this.avsInputTabPage.Location = new System.Drawing.Point(4, 22);
            this.avsInputTabPage.Name = "avsInputTabPage";
            this.avsInputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.avsInputTabPage.Size = new System.Drawing.Size(432, 464);
            this.avsInputTabPage.TabIndex = 3;
            this.avsInputTabPage.Text = "avs输入";
            this.avsInputTabPage.UseVisualStyleBackColor = true;
            // 
            // encTabPage
            // 
            this.encTabPage.Controls.Add(this.groupBox6);
            this.encTabPage.Controls.Add(this.groupBox5);
            this.encTabPage.Controls.Add(this.groupBox4);
            this.encTabPage.Location = new System.Drawing.Point(4, 22);
            this.encTabPage.Name = "encTabPage";
            this.encTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.encTabPage.Size = new System.Drawing.Size(432, 464);
            this.encTabPage.TabIndex = 1;
            this.encTabPage.Text = "编码器";
            this.encTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.muxerComboBox);
            this.groupBox6.Location = new System.Drawing.Point(6, 257);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(420, 200);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "容器";
            // 
            // muxerComboBox
            // 
            this.muxerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.muxerComboBox.FormattingEnabled = true;
            this.muxerComboBox.Items.AddRange(new object[] {
            "MP4",
            "MKV"});
            this.muxerComboBox.Location = new System.Drawing.Point(6, 20);
            this.muxerComboBox.Name = "muxerComboBox";
            this.muxerComboBox.Size = new System.Drawing.Size(121, 20);
            this.muxerComboBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.neroAacRateFactorBox);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.neroAacRateControlBox);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Location = new System.Drawing.Point(6, 168);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(420, 83);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "NeroAac";
            // 
            // neroAacRateFactorBox
            // 
            this.neroAacRateFactorBox.Location = new System.Drawing.Point(71, 43);
            this.neroAacRateFactorBox.Name = "neroAacRateFactorBox";
            this.neroAacRateFactorBox.Size = new System.Drawing.Size(75, 21);
            this.neroAacRateFactorBox.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(9, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 21);
            this.label14.TabIndex = 2;
            this.label14.Text = "质量";
            // 
            // neroAacRateControlBox
            // 
            this.neroAacRateControlBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neroAacRateControlBox.FormattingEnabled = true;
            this.neroAacRateControlBox.Items.AddRange(new object[] {
            "质量",
            "可变码率",
            "恒定码率"});
            this.neroAacRateControlBox.Location = new System.Drawing.Point(71, 17);
            this.neroAacRateControlBox.Name = "neroAacRateControlBox";
            this.neroAacRateControlBox.Size = new System.Drawing.Size(127, 20);
            this.neroAacRateControlBox.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 20);
            this.label13.TabIndex = 0;
            this.label13.Text = "码率控制";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.useCustomCmdBox);
            this.groupBox4.Controls.Add(this.editCmdButton);
            this.groupBox4.Controls.Add(this.tune);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.level);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.profile);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.rateFactorBox);
            this.groupBox4.Controls.Add(this.preset);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.rateControlBox);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(420, 156);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "x264";
            // 
            // useCustomCmdBox
            // 
            this.useCustomCmdBox.Location = new System.Drawing.Point(326, 12);
            this.useCustomCmdBox.Name = "useCustomCmdBox";
            this.useCustomCmdBox.Size = new System.Drawing.Size(104, 24);
            this.useCustomCmdBox.TabIndex = 14;
            this.useCustomCmdBox.Text = "自定义命令行";
            this.useCustomCmdBox.UseVisualStyleBackColor = true;
            this.useCustomCmdBox.CheckedChanged += new System.EventHandler(this.UseCustomCmdBoxCheckedChanged);
            // 
            // editCmdButton
            // 
            this.editCmdButton.Location = new System.Drawing.Point(326, 40);
            this.editCmdButton.Name = "editCmdButton";
            this.editCmdButton.Size = new System.Drawing.Size(75, 23);
            this.editCmdButton.TabIndex = 13;
            this.editCmdButton.Text = "编辑命令行";
            this.editCmdButton.UseVisualStyleBackColor = true;
            this.editCmdButton.Click += new System.EventHandler(this.EditCmdButtonClick);
            // 
            // tune
            // 
            this.tune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tune.FormattingEnabled = true;
            this.tune.Items.AddRange(new object[] {
            "无",
            "film",
            "animation",
            "grain",
            "stillimage",
            "psnr",
            "ssim",
            "fastdecode",
            "zerolatency",
            "touhou"});
            this.tune.Location = new System.Drawing.Point(71, 120);
            this.tune.Name = "tune";
            this.tune.Size = new System.Drawing.Size(75, 20);
            this.tune.TabIndex = 11;
            this.tune.SelectedIndexChanged += new System.EventHandler(this.StringOptionChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(9, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Tune";
            // 
            // level
            // 
            this.level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.level.FormattingEnabled = true;
            this.level.Items.AddRange(new object[] {
            "自动",
            "1",
            "1.1",
            "1.2",
            "1.3",
            "2",
            "2.1",
            "2.2",
            "3",
            "3.1",
            "3.2",
            "4",
            "4.1",
            "4.2",
            "5",
            "5.1"});
            this.level.Location = new System.Drawing.Point(200, 91);
            this.level.Name = "level";
            this.level.Size = new System.Drawing.Size(75, 20);
            this.level.TabIndex = 9;
            this.level.SelectedIndexChanged += new System.EventHandler(this.StringOptionChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(152, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 20);
            this.label11.TabIndex = 8;
            this.label11.Text = "Level";
            // 
            // profile
            // 
            this.profile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profile.FormattingEnabled = true;
            this.profile.Items.AddRange(new object[] {
            "自动",
            "baseline",
            "main",
            "high"});
            this.profile.Location = new System.Drawing.Point(71, 94);
            this.profile.Name = "profile";
            this.profile.Size = new System.Drawing.Size(75, 20);
            this.profile.TabIndex = 7;
            this.profile.SelectedIndexChanged += new System.EventHandler(this.StringOptionChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Profile";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(9, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 21);
            this.label9.TabIndex = 5;
            this.label9.Text = "量化器";
            // 
            // rateFactorBox
            // 
            this.rateFactorBox.Location = new System.Drawing.Point(71, 40);
            this.rateFactorBox.Name = "rateFactorBox";
            this.rateFactorBox.Size = new System.Drawing.Size(75, 21);
            this.rateFactorBox.Sorted = true;
            this.rateFactorBox.TabIndex = 4;
            this.rateFactorBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowFloat);
            this.rateFactorBox.Validating += new System.ComponentModel.CancelEventHandler(this.RateFactorBoxValidating);
            // 
            // preset
            // 
            this.preset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.preset.FormattingEnabled = true;
            this.preset.Items.AddRange(new object[] {
            "ultrafast",
            "superfast",
            "veryfast",
            "faster",
            "fast",
            "medium",
            "slow",
            "slower",
            "veryslow",
            "placebo"});
            this.preset.Location = new System.Drawing.Point(71, 67);
            this.preset.Name = "preset";
            this.preset.Size = new System.Drawing.Size(127, 20);
            this.preset.TabIndex = 3;
            this.preset.SelectedIndexChanged += new System.EventHandler(this.StringOptionChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 20);
            this.label8.TabIndex = 2;
            this.label8.Text = "Preset";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "码率控制";
            // 
            // rateControlBox
            // 
            this.rateControlBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rateControlBox.FormattingEnabled = true;
            this.rateControlBox.Items.AddRange(new object[] {
            "恒定质量(crf)",
            "恒定量化器(qp)",
            "码率 1pass",
            "码率 自动2pass",
            "码率 自动3pass"});
            this.rateControlBox.Location = new System.Drawing.Point(71, 14);
            this.rateControlBox.Name = "rateControlBox";
            this.rateControlBox.Size = new System.Drawing.Size(127, 20);
            this.rateControlBox.TabIndex = 0;
            this.rateControlBox.SelectedIndexChanged += new System.EventHandler(this.RateControlBoxSelectedIndexChanged);
            // 
            // subtitleTabPage
            // 
            this.subtitleTabPage.Controls.Add(this.customSubCheckBox);
            this.subtitleTabPage.Controls.Add(this.customSubGroupBox);
            this.subtitleTabPage.Controls.Add(this.subtitleTextBox);
            this.subtitleTabPage.Controls.Add(this.subtitleButton);
            this.subtitleTabPage.Controls.Add(this.label18);
            this.subtitleTabPage.Location = new System.Drawing.Point(4, 22);
            this.subtitleTabPage.Name = "subtitleTabPage";
            this.subtitleTabPage.Size = new System.Drawing.Size(432, 464);
            this.subtitleTabPage.TabIndex = 2;
            this.subtitleTabPage.Text = "字幕";
            this.subtitleTabPage.UseVisualStyleBackColor = true;
            // 
            // customSubCheckBox
            // 
            this.customSubCheckBox.Location = new System.Drawing.Point(9, 81);
            this.customSubCheckBox.Name = "customSubCheckBox";
            this.customSubCheckBox.Size = new System.Drawing.Size(139, 28);
            this.customSubCheckBox.TabIndex = 10;
            this.customSubCheckBox.Text = "自定义字幕风格";
            this.customSubCheckBox.UseVisualStyleBackColor = true;
            this.customSubCheckBox.CheckedChanged += new System.EventHandler(this.CustomSubCheckBoxCheckedChanged);
            // 
            // customSubGroupBox
            // 
            this.customSubGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customSubGroupBox.Controls.Add(this.fontBottomBox);
            this.customSubGroupBox.Controls.Add(this.label19);
            this.customSubGroupBox.Controls.Add(this.label21);
            this.customSubGroupBox.Controls.Add(this.fontButton);
            this.customSubGroupBox.Controls.Add(this.label20);
            this.customSubGroupBox.Controls.Add(this.fontSizeBox);
            this.customSubGroupBox.Location = new System.Drawing.Point(3, 115);
            this.customSubGroupBox.Name = "customSubGroupBox";
            this.customSubGroupBox.Size = new System.Drawing.Size(426, 117);
            this.customSubGroupBox.TabIndex = 11;
            this.customSubGroupBox.TabStop = false;
            this.customSubGroupBox.Text = "字幕风格";
            // 
            // fontBottomBox
            // 
            this.fontBottomBox.Location = new System.Drawing.Point(66, 89);
            this.fontBottomBox.Name = "fontBottomBox";
            this.fontBottomBox.Size = new System.Drawing.Size(79, 21);
            this.fontBottomBox.TabIndex = 9;
            this.fontBottomBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowInteger);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(6, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 23);
            this.label19.TabIndex = 4;
            this.label19.Text = "字体";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(6, 89);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(66, 23);
            this.label21.TabIndex = 8;
            this.label21.Text = "底部边距";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fontButton
            // 
            this.fontButton.Location = new System.Drawing.Point(66, 51);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(75, 23);
            this.fontButton.TabIndex = 5;
            this.fontButton.Text = "宋体";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new System.EventHandler(this.FontButtonClick);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(172, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(38, 23);
            this.label20.TabIndex = 6;
            this.label20.Text = "字号";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fontSizeBox
            // 
            this.fontSizeBox.Location = new System.Drawing.Point(216, 51);
            this.fontSizeBox.Name = "fontSizeBox";
            this.fontSizeBox.Size = new System.Drawing.Size(79, 21);
            this.fontSizeBox.TabIndex = 7;
            this.fontSizeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AllowInteger);
            // 
            // subtitleTextBox
            // 
            this.subtitleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleTextBox.Location = new System.Drawing.Point(72, 42);
            this.subtitleTextBox.Name = "subtitleTextBox";
            this.subtitleTextBox.ReadOnly = true;
            this.subtitleTextBox.Size = new System.Drawing.Size(276, 21);
            this.subtitleTextBox.TabIndex = 3;
            // 
            // subtitleButton
            // 
            this.subtitleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleButton.Location = new System.Drawing.Point(354, 40);
            this.subtitleButton.Name = "subtitleButton";
            this.subtitleButton.Size = new System.Drawing.Size(75, 23);
            this.subtitleButton.TabIndex = 2;
            this.subtitleButton.Text = "浏览";
            this.subtitleButton.UseVisualStyleBackColor = true;
            this.subtitleButton.Click += new System.EventHandler(this.SubtitleButtonClick);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(3, 38);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(100, 23);
            this.label18.TabIndex = 0;
            this.label18.Text = "字幕路径";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.Location = new System.Drawing.Point(12, 541);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 23);
            this.label15.TabIndex = 18;
            this.label15.Text = "预设";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // profileBox
            // 
            this.profileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileBox.FormattingEnabled = true;
            this.profileBox.Location = new System.Drawing.Point(67, 543);
            this.profileBox.Name = "profileBox";
            this.profileBox.Size = new System.Drawing.Size(121, 20);
            this.profileBox.TabIndex = 17;
            this.profileBox.RightToLeftChanged += new System.EventHandler(this.ProfileBoxSelectedIndexChanged);
            // 
            // saveProfileButton
            // 
            this.saveProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveProfileButton.Location = new System.Drawing.Point(194, 541);
            this.saveProfileButton.Name = "saveProfileButton";
            this.saveProfileButton.Size = new System.Drawing.Size(75, 23);
            this.saveProfileButton.TabIndex = 16;
            this.saveProfileButton.Text = "保存";
            this.saveProfileButton.UseVisualStyleBackColor = true;
            this.saveProfileButton.Click += new System.EventHandler(this.SaveProfileButtonClick);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(298, 541);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(379, 541);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(73, 23);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Filter = "支持的字幕格式|*.srt;*.ass;*.ssa|srt|*.srt|ass|*.ass|ssa|*.ssa";
            // 
            // JobSettingForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 576);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.profileBox);
            this.Controls.Add(this.saveProfileButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.MinimumSize = new System.Drawing.Size(480, 580);
            this.Name = "JobSettingForm";
            this.Text = "设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MediaSettingFormFormClosed);
            this.Load += new System.EventHandler(this.JobSettingFormLoad);
            this.tabControl1.ResumeLayout(false);
            this.videoEditTabPage.ResumeLayout(false);
            this.gbVideoSource.ResumeLayout(false);
            this.gbResolution.ResumeLayout(false);
            this.gbResolution.PerformLayout();
            this.audioEditTabPage.ResumeLayout(false);
            this.audioEditTabPage.PerformLayout();
            this.encTabPage.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.subtitleTabPage.ResumeLayout(false);
            this.subtitleTabPage.PerformLayout();
            this.customSubGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage videoEditTabPage;
        private System.Windows.Forms.ComboBox cbVideoMode;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox destFileBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btOutBrowse;
        private System.Windows.Forms.GroupBox gbVideoSource;
        private System.Windows.Forms.CheckBox convertFPSCheckBox;
        private System.Windows.Forms.CheckBox sourceFrameRateCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox frameRateBox;
        private System.Windows.Forms.ComboBox videoSourceBox;
        private System.Windows.Forms.GroupBox gbResolution;
        private System.Windows.Forms.CheckBox lockToSourceARCheckBox;
        private System.Windows.Forms.CheckBox sourceResolutionCheckBox;
        private System.Windows.Forms.ComboBox modBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox resizerBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox allowAutoChangeARCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox aspectRatioBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.TabPage audioEditTabPage;
        private System.Windows.Forms.CheckBox normalizeBox;
        private System.Windows.Forms.CheckBox downMixBox;
        private System.Windows.Forms.Button btSepAudio;
        private System.Windows.Forms.TextBox tbSepAudio;
        private System.Windows.Forms.ComboBox cbAudioMode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chbSepAudio;
        private System.Windows.Forms.ComboBox audioSourceComboBox;
        private System.Windows.Forms.TabPage avsInputTabPage;
        private System.Windows.Forms.TabPage encTabPage;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox muxerComboBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DomainUpDown neroAacRateFactorBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox neroAacRateControlBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox useCustomCmdBox;
        private System.Windows.Forms.Button editCmdButton;
        private System.Windows.Forms.ComboBox tune;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox level;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox profile;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DomainUpDown rateFactorBox;
        private System.Windows.Forms.ComboBox preset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox rateControlBox;
        private System.Windows.Forms.TabPage subtitleTabPage;
        private System.Windows.Forms.CheckBox customSubCheckBox;
        private System.Windows.Forms.GroupBox customSubGroupBox;
        private System.Windows.Forms.DomainUpDown fontBottomBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DomainUpDown fontSizeBox;
        private System.Windows.Forms.TextBox subtitleTextBox;
        private System.Windows.Forms.Button subtitleButton;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox profileBox;
        private System.Windows.Forms.Button saveProfileButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.FontDialog fontDialog1;

    }
}
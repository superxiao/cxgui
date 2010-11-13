namespace CXGUI.GUI
{
	partial class ProgramConfigForm
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
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.previewPlayerButton = new System.Windows.Forms.Button();
            this.previewPlayerComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbAudioAutoSF = new System.Windows.Forms.CheckBox();
            this.cacelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbInputDir = new System.Windows.Forms.CheckBox();
            this.chbSilentRestart = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.destDirComboBox = new System.Windows.Forms.ComboBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.editGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editGroupBox
            // 
            this.editGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editGroupBox.Controls.Add(this.label2);
            this.editGroupBox.Controls.Add(this.previewPlayerButton);
            this.editGroupBox.Controls.Add(this.previewPlayerComboBox);
            this.editGroupBox.Location = new System.Drawing.Point(12, 137);
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.Size = new System.Drawing.Size(410, 77);
            this.editGroupBox.TabIndex = 9;
            this.editGroupBox.TabStop = false;
            this.editGroupBox.Text = "编辑与编码";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "预览播放器";
            // 
            // previewPlayerButton
            // 
            this.previewPlayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPlayerButton.Location = new System.Drawing.Point(329, 17);
            this.previewPlayerButton.Name = "previewPlayerButton";
            this.previewPlayerButton.Size = new System.Drawing.Size(75, 23);
            this.previewPlayerButton.TabIndex = 1;
            this.previewPlayerButton.Text = "浏览";
            this.previewPlayerButton.UseVisualStyleBackColor = true;
            this.previewPlayerButton.Click += new System.EventHandler(this.PreviewPlayerButtonClick);
            // 
            // previewPlayerComboBox
            // 
            this.previewPlayerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewPlayerComboBox.FormattingEnabled = true;
            this.previewPlayerComboBox.Location = new System.Drawing.Point(87, 20);
            this.previewPlayerComboBox.Name = "previewPlayerComboBox";
            this.previewPlayerComboBox.Size = new System.Drawing.Size(236, 20);
            this.previewPlayerComboBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbAudioAutoSF);
            this.groupBox2.Location = new System.Drawing.Point(12, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 87);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "音频";
            // 
            // cbAudioAutoSF
            // 
            this.cbAudioAutoSF.Checked = true;
            this.cbAudioAutoSF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAudioAutoSF.Location = new System.Drawing.Point(6, 20);
            this.cbAudioAutoSF.Name = "cbAudioAutoSF";
            this.cbAudioAutoSF.Size = new System.Drawing.Size(217, 24);
            this.cbAudioAutoSF.TabIndex = 0;
            this.cbAudioAutoSF.Text = "当音频读取有错误时自动更改源滤镜";
            this.cbAudioAutoSF.UseVisualStyleBackColor = true;
            // 
            // cacelButton
            // 
            this.cacelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cacelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cacelButton.Location = new System.Drawing.Point(347, 313);
            this.cacelButton.Name = "cacelButton";
            this.cacelButton.Size = new System.Drawing.Size(75, 23);
            this.cacelButton.TabIndex = 7;
            this.cacelButton.Text = "取消";
            this.cacelButton.UseVisualStyleBackColor = true;
            this.cacelButton.Click += new System.EventHandler(this.CacelButtonClick);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(266, 313);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chbInputDir);
            this.groupBox1.Controls.Add(this.chbSilentRestart);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.destDirComboBox);
            this.groupBox1.Controls.Add(this.outputButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 119);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工作列表";
            // 
            // chbInputDir
            // 
            this.chbInputDir.Location = new System.Drawing.Point(6, 79);
            this.chbInputDir.Name = "chbInputDir";
            this.chbInputDir.Size = new System.Drawing.Size(165, 24);
            this.chbInputDir.TabIndex = 5;
            this.chbInputDir.Text = "显示输入文件所在目录";
            this.chbInputDir.UseVisualStyleBackColor = true;
            // 
            // chbSilentRestart
            // 
            this.chbSilentRestart.Location = new System.Drawing.Point(6, 49);
            this.chbSilentRestart.Name = "chbSilentRestart";
            this.chbSilentRestart.Size = new System.Drawing.Size(165, 24);
            this.chbSilentRestart.TabIndex = 4;
            this.chbSilentRestart.Text = "中止项重新开始时不提示";
            this.chbSilentRestart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "输出文件夹";
            // 
            // destDirComboBox
            // 
            this.destDirComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.destDirComboBox.FormattingEnabled = true;
            this.destDirComboBox.Location = new System.Drawing.Point(87, 20);
            this.destDirComboBox.Name = "destDirComboBox";
            this.destDirComboBox.Size = new System.Drawing.Size(236, 20);
            this.destDirComboBox.TabIndex = 2;
            // 
            // outputButton
            // 
            this.outputButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.outputButton.Location = new System.Drawing.Point(329, 18);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(75, 23);
            this.outputButton.TabIndex = 1;
            this.outputButton.Text = "浏览";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "可执行文件|*.exe";
            // 
            // ProgramConfigForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cacelButton;
            this.ClientSize = new System.Drawing.Size(434, 348);
            this.Controls.Add(this.editGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cacelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(270, 270);
            this.Name = "ProgramConfigForm";
            this.Text = "选项";
            this.Load += new System.EventHandler(this.ProgramConfigFormLoad);
            this.editGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox editGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button previewPlayerButton;
        private System.Windows.Forms.ComboBox previewPlayerComboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox cbAudioAutoSF;
        private System.Windows.Forms.Button cacelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox chbInputDir;
        public System.Windows.Forms.CheckBox chbSilentRestart;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox destDirComboBox;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}
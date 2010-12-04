namespace CXGUI.GUI
{
    using Clinky;
    using CXGUI;
    using CXGUI.AudioEncoding;
    using CXGUI.Avisynth;
    using CXGUI.Config;
    using CXGUI.Job;
    using CXGUI.VideoEncoding;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    [Serializable]
    public partial class JobSettingForm : Form
    {
        protected AudioEncConfigBase _audioEncConfig;
        protected AudioInfo _audioInfo;
        protected CommandLineBox _cmdLineBox;
        protected JobItem _jobItem;
        protected ControlResetter _resetter;
        protected ResolutionCalculator _resolutionCal;
        protected bool _usingSepAudio;
        protected VideoEncConfigBase _videoEncConfig;
        protected VideoInfo _videoInfo;

        public JobSettingForm()
        {
            this.InitializeComponent();
        }

        private void AllowAutoChangeARCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this._resolutionCal.LockAspectRatio = !this.allowAutoChangeARCheckBox.Checked;
        }

        private void AllowFloat(object sender, KeyPressEventArgs e)
        {
            int keyChar = e.KeyChar;
            if ((keyChar < 0x30) || (keyChar > 0x39))
            {
                if (keyChar != 8 && keyChar != 13 && keyChar != 0x2e)
                {
                    e.Handled = true;
                }
            }
            if ((sender as Control).Text.Contains(".") && (keyChar == 0x2e))
            {
                e.Handled = true;
            }
        }

        private void AllowInteger(object sender, KeyPressEventArgs e)
        {
            int keyChar = e.KeyChar;
            if (((keyChar < 0x30) || (keyChar > 0x39)) && ((keyChar != 8) && (keyChar != 13)))
            {
                e.Handled = true;
            }
        }

        private void AspectRatioBoxKeyUp(object sender, KeyEventArgs e)
        {
            double result = new double();
            double.TryParse(this.aspectRatioBox.Text, out result);
            if (result > 0)
            {
                double aspectRatio = this._resolutionCal.AspectRatio;
                this._resolutionCal.AspectRatio = result;
                if ((this._resolutionCal.Width <= 0x780) && (this._resolutionCal.Height <= 0x438))
                {
                    this.RefreshResolution(this.aspectRatioBox);
                }
                else
                {
                    this._resolutionCal.AspectRatio = aspectRatio;
                }
            }
        }

        private void AvsInputInitializeConfig(JobItem jobItem)
        {
            if (jobItem.JobConfig.Container == OutputContainer.MKV)
            {
                this.muxerComboBox.Text = "MKV";
            }
            else
            {
                this.muxerComboBox.Text = "MP4";
            }
        }

        private void AvsInputSaveConfig(JobItemConfig jobConfig)
        {
            if (this.muxerComboBox.Text == "MKV")
            {
                jobConfig.Container = OutputContainer.MKV;
            }
            else if (this.muxerComboBox.Text == "MP4")
            {
                jobConfig.Container = OutputContainer.MP4;
            }
        }

        private void BoolChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Enabled)
            {
                string name = box.Name.Replace('_', '-');
                (this._videoEncConfig as x264Config).SetBooleanOption(name, box.Checked);
                this.RefreshX264UI();
            }
        }

        private void BtOutBrowseClick(object sender, EventArgs e)
        {
            if (this.destFileBox.Text == string.Empty)
            {
                this.destFileBox.Text = this._jobItem.DestFile;
            }
            try
            {
                this.saveFileDialog1.InitialDirectory = Path.GetDirectoryName(this.destFileBox.Text);
                this.saveFileDialog1.FileName = Path.GetFileName(this.destFileBox.Text);
                this.saveFileDialog1.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("无效路径或含非法字符。", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void BtSepAudioClick(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
            if (File.Exists(this.openFileDialog1.FileName))
            {
                AudioInfo info = new AudioInfo(this.openFileDialog1.FileName);
                if (info.StreamsCount == 0)
                {
                    MessageBox.Show(new StringBuilder().Append(this.openFileDialog1.FileName).Append("\n检测不到所选文件中的音频流。").ToString(), "检测失败", MessageBoxButtons.OK);
                }
                else
                {
                    this._usingSepAudio = true;
                    this.tbSepAudio.Text = this.openFileDialog1.FileName;
                    this.cbAudioMode.SelectedIndex = 0;
                }
                this.SettleAudioControls();
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this._resetter.ResetControls();
            this._resetter.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CbAudioModeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Created && (this.cbAudioMode.SelectedIndex == 2))
            {
                if (this.cbVideoMode.SelectedIndex == -1 || this.cbVideoMode.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.cbAudioMode.SelectedIndex = 0;
                }
                this.SettleAudioControls();
            }
        }

        private void CbVideoModeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbVideoMode.SelectedIndex == 0)
            {
                this.gbResolution.Enabled = true;
                this.gbVideoSource.Enabled = true;
            }
            else if (this.cbVideoMode.SelectedIndex == 1)
            {
                this.gbResolution.Enabled = false;
                this.gbVideoSource.Enabled = false;
            }
            else if (this.cbVideoMode.SelectedIndex == 2)
            {
                if (this.cbAudioMode.SelectedIndex == -1 || this.cbAudioMode.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.cbVideoMode.SelectedIndex = 0;
                }
                else
                {
                    this.gbResolution.Enabled = false;
                    this.gbVideoSource.Enabled = false;
                }
            }
        }

        private void ChbSepAudioCheckedChanged(object sender, EventArgs e)
        {
            if (this.chbSepAudio.Checked)
            {
                this.tbSepAudio.Enabled = true;
                this.btSepAudio.Enabled = true;
                this._usingSepAudio = true;
                if (this.tbSepAudio.Text != string.Empty)
                {
                    if (this.cbAudioMode.SelectedIndex == -1 || this.cbAudioMode.SelectedIndex == 2)
                    {
                        this.cbAudioMode.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                if (this.cbVideoMode.SelectedIndex == -1 || this.cbVideoMode.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.chbSepAudio.Checked = true;
                }
                else
                {
                    this.tbSepAudio.Enabled = false;
                    this.btSepAudio.Enabled = false;
                    this._usingSepAudio = false;
                }
            }
            this.SettleAudioControls();
        }

        public void Clear()
        {
            this._videoEncConfig = null;
            this._audioEncConfig = null;
            this._videoInfo = null;
            this._audioInfo = null;
            this._resolutionCal = null;
            this.widthBox.Text = string.Empty;
            this.heightBox.Text = string.Empty;
            this.aspectRatioBox.Text = string.Empty;
            this.frameRateBox.Text = string.Empty;
            this.subtitleTextBox.Text = string.Empty;
        }

        private void CustomSubCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (this.customSubCheckBox.Checked)
            {
                this.customSubGroupBox.Enabled = true;
            }
            else
            {
                this.customSubGroupBox.Enabled = false;
            }
        }

        private void EditCmdButtonClick(object sender, EventArgs e)
        {
            this._cmdLineBox.ShowDialog();
            this._videoEncConfig.CustomCmdLine = this._cmdLineBox.CmdLine;
        }

        private void FontButtonClick(object sender, EventArgs e)
        {
            this.fontDialog1.Font = new Font(this.fontButton.Text, float.Parse(this.fontSizeBox.Text));
            this.fontDialog1.ShowDialog();
            this.fontButton.Text = this.fontDialog1.Font.Name;
            this.fontSizeBox.Text = this.fontDialog1.Font.SizeInPoints.ToString();
        }

        private void FrameRateBoxValidating(object sender, CancelEventArgs e)
        {
            if (this.frameRateBox.Text == "0")
            {
                this.frameRateBox.Text = "1";
            }
        }

        public string[] GetProfiles()
        {
            return ArrayBuilder.Build<string>(this.profileBox.Items);
        }

        private void HeightBoxKeyUp(object sender, KeyEventArgs e)
        {
            int result = new int();
            int.TryParse(this.heightBox.Text, out result);
            if ((result > 0) && (result <= 0x438))
            {
                int height = this._resolutionCal.Height;
                this._resolutionCal.Height = result;
                if (this._resolutionCal.Width > 0x780)
                {
                    this._resolutionCal.Height = height;
                }
                else
                {
                    this.RefreshResolution(this.heightBox);
                }
            }
        }

        private void InitializeAvsConfig(AvisynthConfig avsConfig)
        {
            this._resolutionCal = new ResolutionCalculator();
            this.InitializeResolutionCfg(avsConfig);
            this.InitializeFrameRateCfg(avsConfig);
            if (this._videoInfo.HasVideo)
            {
                this.InitializeResolution(avsConfig, this._videoInfo);
                this.InitializeFrameRate(avsConfig, this._videoInfo);
            }
            this.audioSourceComboBox.Text = avsConfig.AudioSourceFilter.ToString();
            this.downMixBox.Checked = avsConfig.DownMix;
            this.normalizeBox.Checked = avsConfig.Normalize;
        }

        private void InitializeEncConfig()
        {
            this.RefreshX264UI();
            this.RefreshNeroAac();
        }

        private void InitializeFrameRate(AvisynthConfig avsConfig, VideoInfo videoInfo)
        {
            if (this._jobItem.AvsConfig.UsingSourceFrameRate)
            {
                this.frameRateBox.Text = videoInfo.FrameRate.ToString();
                this.sourceFrameRateCheckBox.Checked = true;
            }
            else
            {
                this.frameRateBox.Text = avsConfig.FrameRate.ToString();
                this.sourceFrameRateCheckBox.Checked = false;
            }
        }

        private void InitializeFrameRateCfg(AvisynthConfig avsConfig)
        {
            this.videoSourceBox.Text = avsConfig.VideoSourceFilter.ToString();
            if (this.videoSourceBox.SelectedIndex == -1)
            {
                this.videoSourceBox.SelectedIndex = 0;
            }
            this.convertFPSCheckBox.Checked = avsConfig.ConvertFPS;
        }

        private void InitializeJobConfig(JobItemConfig jobConfig)
        {
            this.cbVideoMode.SelectedIndex = 1;
            this.cbAudioMode.SelectedIndex = 1;
            if (this._videoInfo.HasVideo)
            {
                this.cbVideoMode.Enabled = true;
                this.cbVideoMode.SelectedIndex = (int)jobConfig.VideoMode;
            }
            else
            {
                this.cbVideoMode.Enabled = false;
                this.cbVideoMode.SelectedIndex = -1;
            }
            if (this.cbVideoMode.SelectedIndex != -1)
            {
                this.chbSepAudio.Enabled = true;
                this.chbSepAudio.Checked = this._jobItem.UsingExternalAudio;
            }
            else
            {
                this.chbSepAudio.Enabled = false;
                this.chbSepAudio.Checked = false;
            }
            if ((this.chbSepAudio.Enabled && this.chbSepAudio.Checked) && (this.tbSepAudio.Text != string.Empty))
            {
                this._usingSepAudio = true;
            }
            else
            {
                this._usingSepAudio = false;
            }
            if (this._usingSepAudio || (this._audioInfo.StreamsCount != 0))
            {
                this.cbAudioMode.Enabled = true;
                this.cbAudioMode.SelectedIndex = (int)jobConfig.AudioMode;
            }
            else
            {
                this.cbAudioMode.Enabled = false;
                this.cbAudioMode.SelectedIndex = -1;
            }
            this.SettleAudioControls();
            if (this.cbVideoMode.SelectedIndex != 0)
            {
                this.gbResolution.Enabled = false;
                this.gbVideoSource.Enabled = false;
            }
            else
            {
                this.gbResolution.Enabled = true;
                this.gbVideoSource.Enabled = true;
            }
            if (jobConfig.Container == OutputContainer.MKV)
            {
                this.muxerComboBox.Text = "MKV";
            }
            else
            {
                this.muxerComboBox.Text = "MP4";
            }
        }

        private void InitializeResolution(AvisynthConfig avsConfig, VideoInfo videoInfo)
        {
            if (avsConfig.Mod == 0 || avsConfig.Mod%2 == 1)
            {
                avsConfig.Mod = 2;
            }
            this._resolutionCal.Mod = avsConfig.Mod;
            this._resolutionCal.LockAspectRatio = avsConfig.LockAspectRatio;
            this._resolutionCal.LockToSourceAR = avsConfig.LockToSourceAR;
            if (!avsConfig.LockToSourceAR && (avsConfig.AspectRatio > 0))
            {
                this._resolutionCal.AspectRatio = avsConfig.AspectRatio;
            }
            else
            {
                this._resolutionCal.AspectRatio = videoInfo.DisplayAspectRatio;
            }
            if (avsConfig.Height > 0)
            {
                this._resolutionCal.Height = avsConfig.Height;
            }
            else
            {
                this._resolutionCal.Height = videoInfo.Height;
            }
            if (avsConfig.Width > 0)
            {
                this._resolutionCal.Width = avsConfig.Width;
            }
            else
            {
                this._resolutionCal.Width = videoInfo.Width;
            }
            this.RefreshResolution(null);
        }

        private void InitializeResolutionCfg(AvisynthConfig avsConfig)
        {
            this.sourceResolutionCheckBox.CheckedChanged -= new EventHandler(this.SourceResolutionCheckBoxCheckedChanged);
            if (avsConfig.UsingSourceResolution)
            {
                this.sourceResolutionCheckBox.Checked = true;
                IEnumerator enumerator = this.gbResolution.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control current = (Control)enumerator.Current;
                    current.Enabled = false;
                }
                this.sourceResolutionCheckBox.Enabled = true;
            }
            else
            {
                this.sourceResolutionCheckBox.Checked = false;
                IEnumerator enumerator2 = this.gbResolution.Controls.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    Control control2 = (Control)enumerator2.Current;
                    control2.Enabled = true;
                }
            }
            this.sourceResolutionCheckBox.CheckedChanged += new EventHandler(this.SourceResolutionCheckBoxCheckedChanged);
            this.allowAutoChangeARCheckBox.Checked = !avsConfig.LockAspectRatio;
            this.lockToSourceARCheckBox.CheckedChanged -= new EventHandler(this.UseSourceARCheckedChanged);
            this.lockToSourceARCheckBox.Checked = avsConfig.LockToSourceAR;
            this.lockToSourceARCheckBox.CheckedChanged += new EventHandler(this.UseSourceARCheckedChanged);
            if (avsConfig.LockToSourceAR || this.sourceResolutionCheckBox.Checked)
            {
                this.aspectRatioBox.Enabled = false;
            }
            else
            {
                this.aspectRatioBox.Enabled = true;
            }
            this.resizerBox.Text = avsConfig.Resizer.ToString();
            if (this.resizerBox.SelectedIndex == -1)
            {
                this.resizerBox.SelectedIndex = 0;
            }
        }

        private void InitializeSubtitleConfig(SubtitleConfig subtitleConfig)
        {
            this.fontButton.Text = subtitleConfig.Fontname;
            this.fontSizeBox.Text = subtitleConfig.Fontsize.ToString();
            this.fontBottomBox.Text = subtitleConfig.MarginV.ToString();
            this.customSubCheckBox.Checked = subtitleConfig.UsingStyle;
            this.CustomSubCheckBoxCheckedChanged(null, null);
        }

        private void JobSettingFormLoad(object sender, EventArgs e)
        {
            if (this._resetter == null)
            {
                this._resetter = new ControlResetter();
            }
            this._resetter.SaveControls(this);
        }

        private void MediaSettingFormFormClosed(object sender, FormClosedEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && this._resetter.Changed())
            {
                if (MessageBox.Show("保存更改吗？", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.OkButtonClick(null, null);
                }
                else
                {
                    this.CancelButtonClick(null, null);
                }
            }
        }

        private void ModBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._videoInfo.HasVideo)
            {
                this._resolutionCal.Mod = int.Parse(this.modBox.Text);
                this.RefreshResolution(this.modBox);
            }
        }

        private void MuxerComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string extension = this.muxerComboBox.Text.ToLower();
            this.destFileBox.Text = MyIO.GetUniqueName(Path.ChangeExtension(this._jobItem.DestFile, extension));
        }

        private void NeroAacRateControlBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            NeroAacConfig config = this._audioEncConfig as NeroAacConfig;
            switch (this.neroAacRateControlBox.SelectedIndex)
            {
                case 0:
                    this.label14.Text = "质量";
                    if (config.Quality == 0)
                    {
                        this.neroAacRateFactorBox.Text = "0.5";
                        config.Quality = 0.5;
                    }
                    break;

                case 1:
                    this.label14.Text = "码率";
                    if (config.BitRate == 0)
                    {
                        this.neroAacRateFactorBox.Text = "96";
                        config.BitRate = 0x60;
                    }
                    break;

                case 2:
                    this.label14.Text = "码率";
                    if (config.ConstantBitRate == 0)
                    {
                        this.neroAacRateFactorBox.Text = "96";
                        config.ConstantBitRate = 0x60;
                    }
                    break;
            }
        }

        private void NeroAacRateFactorBoxValidating(object sender, CancelEventArgs e)
        {

        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            this._jobItem.VideoEncConfig = this._videoEncConfig;
            this._jobItem.AudioEncConfig = this._audioEncConfig;
            this._jobItem.State = JobState.NotProccessed;
            this._jobItem.ProfileName = this.profileBox.Text;
            if (this._videoInfo.Format == "avs")
            {
                this.AvsInputSaveConfig(this._jobItem.JobConfig);
                goto Label_0321;
            }
            try
            {
                string directoryName = Path.GetDirectoryName(this.destFileBox.Text);
                switch (directoryName)
                {
                    case "":
                    case null:
                        this.destFileBox.Text = this._jobItem.DestFile;
                        goto Label_021F;
                }
                if (MyIO.IsSameFile(this._jobItem.SourceFile, this.saveFileDialog1.FileName))
                {
                    MessageBox.Show("与源媒体文件同名。请更改文件名。", "文件重名", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (MyIO.Exists(this.saveFileDialog1.FileName))
                {
                    if (MessageBox.Show(new StringBuilder().Append(Path.GetFileName(this.saveFileDialog1.FileName)).Append(" 已存在。\n要替换它吗？").ToString(), "确认另存为", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (Directory.Exists(directoryName))
                {
                    this._jobItem.DestFile = this.destFileBox.Text;
                }
                else if (!Directory.Exists(directoryName))
                {
                    if (MessageBox.Show("目标文件夹不存在。是否新建？", "文件夹不存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(this.destFileBox.Text));
                        this._jobItem.DestFile = this.destFileBox.Text;
                    }
                    else
                    {
                        this.destFileBox.Text = this._jobItem.DestFile;
                    }
                }
                else
                {
                    this._jobItem.DestFile = this.destFileBox.Text;
                }
            }
            catch (Exception)
            {
                this.destFileBox.Text = this._jobItem.DestFile;
            }
        Label_021F:
            if (this.chbSepAudio.Checked)
            {
                if (this.tbSepAudio.Text != string.Empty)
                {
                    this._jobItem.UsingExternalAudio = true;
                    this._jobItem.ExternalAudio = this.tbSepAudio.Text;
                }
                else
                {
                    switch (MessageBox.Show("未指定外挂音轨。确定退出吗？", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        case DialogResult.No:
                            return;

                        case DialogResult.Yes:
                            this.chbSepAudio.Checked = false;
                            this._jobItem.UsingExternalAudio = false;
                            this._jobItem.ExternalAudio = string.Empty;
                            goto Label_02BE;
                    }
                }
            }
            else
            {
                this._jobItem.UsingExternalAudio = false;
                this._jobItem.ExternalAudio = string.Empty; 
            }
        Label_02BE:
            if (this.subtitleTextBox.Text != string.Empty)
            {
                this._jobItem.SubtitleFile = this.subtitleTextBox.Text;
            }
            this.SaveToAvsConfig(this._jobItem.AvsConfig);
            this.SaveToJobConfig(this._jobItem.JobConfig);
            this.SaveToSubtitleConfig(this._jobItem.SubtitleConfig);
        Label_0321:
            this._resetter.Clear();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void PreviewButtonClick(object sender, EventArgs e)
        {
            if (!MyIO.Exists(ProgramConfig.Get().PlayerPath))
            {
                MessageBox.Show("请在程序设置中指定有效的播放器路径。", "找不到播放器", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                string sourceFile;
                AvisynthConfig avsConfig = new AvisynthConfig();
                JobItemConfig jobConfig = new JobItemConfig();
                SubtitleConfig subtitleConfig = new SubtitleConfig();
                string subtitle = string.Empty;
                string text = string.Empty;
                string contents = string.Empty;
                bool writeVideoScript = false;
                bool writeAudioScript = false;
                if (this._videoInfo.Format == "avs")
                {
                    this.AvsInputSaveConfig(jobConfig);
                }
                else
                {
                    this.SaveToAvsConfig(avsConfig);
                    this.SaveToJobConfig(jobConfig);
                    this.SaveToSubtitleConfig(subtitleConfig);
                    if (MyIO.Exists(this.subtitleTextBox.Text))
                    {
                        subtitle = this.subtitleTextBox.Text;
                    }
                    if (this.chbSepAudio.Checked && MyIO.Exists(this.tbSepAudio.Text))
                    {
                        text = this.tbSepAudio.Text;
                    }
                }
                if (jobConfig.VideoMode != StreamProcessMode.None)
                {
                    writeVideoScript = true;
                    if (jobConfig.VideoMode == StreamProcessMode.Encode)
                    {
                        if ((subtitle != string.Empty) && subtitleConfig.UsingStyle)
                        {
                            new SubStyleWriter(subtitle, subtitleConfig).Write();
                        }
                    }
                    else if (jobConfig.VideoMode == StreamProcessMode.Copy)
                    {
                        avsConfig.UsingSourceFrameRate = true;
                        avsConfig.UsingSourceResolution = true;
                        avsConfig.ConvertFPS = true;
                    }
                    new VideoAvsWriter(this._jobItem.SourceFile, avsConfig, subtitle).WriteScript("video.avs");
                    contents += "video = import(\"video.avs\")";
                }
                if (text != string.Empty)
                {
                    sourceFile = text;
                }
                else
                {
                    sourceFile = this._jobItem.SourceFile;
                }
                if (jobConfig.AudioMode != StreamProcessMode.None)
                {
                    writeAudioScript = true;
                    if (jobConfig.AudioMode == StreamProcessMode.Copy)
                    {
                        avsConfig.Normalize = false;
                        avsConfig.DownMix = false;
                    }
                    new AudioAvsWriter(sourceFile, avsConfig).WriteScript("audio.avs");
                    contents += "\r\naudio = import(\"audio.avs\")";
                }
                if (writeVideoScript && writeAudioScript)
                {
                    contents += "\r\nAudioDub(video, audio)";
                }
                else if (writeVideoScript)
                {
                    contents += "\r\nvideo";
                }
                else if (writeAudioScript)
                {
                    contents += "\r\naudio";
                }
                if (contents != string.Empty)
                {
                    File.WriteAllText("preview.avs", contents, Encoding.Default);
                    Process process = new Process();
                    process.StartInfo.FileName = ProgramConfig.Get().PlayerPath;
                    process.StartInfo.Arguments = ("\"" + Path.GetFullPath("preview.avs")) + "\"";
                    process.Start();
                }
            }
        }

        private void ProfileBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Profile profile;
            try
            {
                profile = new Profile(this.profileBox.Text);
            }
            catch (ProfileNotFoundException)
            {
                MessageBox.Show("预设文件不存在或已损坏。将刷新预设列表。", "预设读取失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.profileBox.SelectedIndexChanged -= new EventHandler(this.ProfileBoxSelectedIndexChanged);
                this.profileBox.Items.Clear();
                this.profileBox.Items.AddRange(Profile.GetExistingProfilesNamesOnHardDisk());
                if (!this.profileBox.Items.Contains("Default"))
                {
                    Profile.RebuildDefault("Default");
                    this.profileBox.Items.Add("Default");
                }
                this.profileBox.SelectedItem = "Default";
                profile = new Profile("Default");
                this.profileBox.SelectedIndexChanged += new EventHandler(this.ProfileBoxSelectedIndexChanged);
            }
            this.InitializeJobConfig(profile.JobConfig);
            this.InitializeAvsConfig(profile.AvsConfig);
            this.InitializeSubtitleConfig(profile.SubtitleConfig);
            this._videoEncConfig = profile.VideoEncConfig;
            this._audioEncConfig = profile.AudioEncConfig;
            this.InitializeEncConfig();
        }

        private void RateControlBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            x264Config config = this._videoEncConfig as x264Config;
            if (this.rateControlBox.SelectedIndex == 0)
            {
                config.SetNumOption("crf", (double)0x17);
            }
            else if (this.rateControlBox.SelectedIndex == 1)
            {
                config.SetNumOption("qp", (double)0x17);
            }
            else
            {
                config.SetNumOption("bitrate", (double)700);
                config.TotalPass = this.rateControlBox.SelectedIndex - 1;
                config.CurrentPass = 1;
            }
            this.RefreshX264UI();
        }

        private void RateFactorBoxValidating(object sender, CancelEventArgs e)
        {
            string str = "";
            double num;
            x264Config config = this._videoEncConfig as x264Config;
            if (this.rateControlBox.SelectedIndex == 0)
            {
                str = "crf";
            }
            else if (this.rateControlBox.SelectedIndex == 1)
            {
                str = "qp";
            }
            else
            {
                if (this.rateControlBox.SelectedIndex > 1)
                {
                    str = "bitrate";
                }
            }
            try
            {
                num = double.Parse(this.rateFactorBox.Text);
            }
            catch (Exception)
            {
                this.rateFactorBox.Text = config.GetNode(str).Num.ToString();
                return;
            }
            if (str != "crf")
            {
                num = Math.Floor(num);
            }
            config.SetNumOption(str, num);
            this.rateFactorBox.Text = config.GetNode(str).Num.ToString();
        }

        private void RefreshNeroAac()
        {
            NeroAacConfig config = this._audioEncConfig as NeroAacConfig;
            if (config.Quality > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 0;
                this.neroAacRateFactorBox.Text = config.Quality.ToString();
            }
            else if (config.BitRate > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 1;
                this.neroAacRateFactorBox.Text = config.BitRate.ToString();
            }
            else if (config.ConstantBitRate > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 2;
                this.neroAacRateFactorBox.Text = config.ConstantBitRate.ToString();
            }
        }

        private void RefreshResolution(object caller)
        {
            if (caller != this.heightBox)
            {
                this.heightBox.Text = this._resolutionCal.Height.ToString();
            }
            if (caller != this.widthBox)
            {
                this.widthBox.Text = this._resolutionCal.Width.ToString();
            }
            if (caller != this.aspectRatioBox)
            {
                this.aspectRatioBox.Text = this._resolutionCal.AspectRatio.ToString();
            }
            this.modBox.SelectedIndexChanged -= new EventHandler(this.ModBoxSelectedIndexChanged);
            this.modBox.Text = this._resolutionCal.Mod.ToString();
            this.modBox.SelectedIndexChanged += new EventHandler(this.ModBoxSelectedIndexChanged);
        }

        private void RefreshX264UI()
        {
            x264Config config = this._videoEncConfig as x264Config;
            IEnumerator enumerator = this.groupBox4.Controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Control current = (Control)enumerator.Current;
                x264ConfigNode node = config.GetNode(current.Name.Replace('_', '-'));
                if (node != null)
                {
                    current.Enabled = !node.Locked;
                    if (node.Type == NodeType.Bool)
                    {
                        CheckBox box = current as CheckBox;
                        box.CheckedChanged -= new EventHandler(this.BoolChanged);
                        box.Checked = node.Bool;
                        box.CheckedChanged += new EventHandler(this.BoolChanged);
                    }
                    else
                    {
                        if (node.Type == NodeType.Num)
                        {
                            current.Text = node.Num.ToString();
                            continue;
                        }
                        if (node.Type == NodeType.Str)
                        {
                            current.Text = node.Str;
                            continue;
                        }
                        if (node.Type == NodeType.StrOptionIndex)
                        {
                            ComboBox box2 = current as ComboBox;
                            box2.SelectedIndexChanged -= new EventHandler(this.StringOptionChanged);
                            box2.SelectedIndex = node.StrOptionIndex;
                            box2.SelectedIndexChanged += new EventHandler(this.StringOptionChanged);
                        }
                    }
                }
            }
            this.rateControlBox.SelectedIndexChanged -= new EventHandler(this.RateControlBoxSelectedIndexChanged);
            if (config.GetNode("crf").InUse)
            {
                this.rateControlBox.SelectedIndex = 0;
                this.rateFactorBox.Text = config.GetNode("crf").Num.ToString();
                this.label9.Text = "量化器";
            }
            else if (config.GetNode("qp").InUse)
            {
                this.rateControlBox.SelectedIndex = 1;
                this.rateFactorBox.Text = config.GetNode("qp").Num.ToString();
                this.label9.Text = "质量";
            }
            else if (config.GetNode("bitrate").InUse)
            {
                this.rateControlBox.SelectedIndex = config.TotalPass + 1;
                this.rateFactorBox.Text = config.GetNode("bitrate").Num.ToString();
                this.label9.Text = "码率";
            }
            this.rateControlBox.SelectedIndexChanged += new EventHandler(this.RateControlBoxSelectedIndexChanged);
            this.useCustomCmdBox.Checked = this._videoEncConfig.UsingCustomCmd;
            this.UseCustomCmdBoxCheckedChanged(null, null);
        }

        private void ResolutionValidating(object sender, CancelEventArgs e)
        {
            this.RefreshResolution(null);
        }

        private void SaveProfileButtonClick(object sender, EventArgs e)
        {
            AvisynthConfig avsConfig = null;
            JobItemConfig jobConfig = null;
            SubtitleConfig subtitleConfig = null;
            if (this._videoInfo.Format == "avs")
            {
                avsConfig = this._jobItem.AvsConfig;
                subtitleConfig = this._jobItem.SubtitleConfig;
                jobConfig = new JobItemConfig();
                this.AvsInputSaveConfig(jobConfig);
            }
            else
            {
                avsConfig = new AvisynthConfig();
                subtitleConfig = new SubtitleConfig();
                jobConfig = new JobItemConfig();
                this.SaveToAvsConfig(avsConfig);
                this.SaveToJobConfig(jobConfig);
                this.SaveToSubtitleConfig(subtitleConfig);
            }
            Profile.Save(this.profileBox.Text, jobConfig, avsConfig, this._videoEncConfig, this._audioEncConfig, subtitleConfig);
            if (!this.profileBox.Items.Contains(this.profileBox.Text))
            {
                this.profileBox.Items.Add(this.profileBox.Text);
            }
        }

        private void SaveToAvsConfig(AvisynthConfig avsConfig)
        {
            if (this._videoInfo.HasVideo)
            {
                if (this.sourceResolutionCheckBox.Checked)
                {
                    avsConfig.UsingSourceResolution = true;
                    avsConfig.Width = 0;
                    avsConfig.Height = 0;
                    avsConfig.AspectRatio = 0;
                }
                else
                {
                    avsConfig.UsingSourceResolution = false;
                    avsConfig.Width = int.Parse(this.widthBox.Text);
                    avsConfig.Height = int.Parse(this.heightBox.Text);
                    avsConfig.AspectRatio = double.Parse(this.aspectRatioBox.Text);
                }
                avsConfig.LockAspectRatio = !this.allowAutoChangeARCheckBox.Checked;
                avsConfig.LockToSourceAR = this.lockToSourceARCheckBox.Checked;
                avsConfig.Mod = int.Parse(this.modBox.Text);
                avsConfig.Resizer = (ResizeFilter)Enum.Parse(typeof(ResizeFilter), this.resizerBox.Text);
                avsConfig.VideoSourceFilter = (VideoSourceFilter)Enum.Parse(typeof(VideoSourceFilter), this.videoSourceBox.Text);
                if (this.sourceFrameRateCheckBox.Checked)
                {
                    avsConfig.UsingSourceFrameRate = true;
                    avsConfig.FrameRate = 0;
                }
                else
                {
                    avsConfig.UsingSourceFrameRate = false;
                    avsConfig.FrameRate = double.Parse(this.frameRateBox.Text);
                }
                avsConfig.ConvertFPS = this.convertFPSCheckBox.Checked;
            }
            if (this._audioInfo.StreamsCount != 0)
            {
                avsConfig.AudioSourceFilter = (AudioSourceFilter)Enum.Parse(typeof(AudioSourceFilter), this.audioSourceComboBox.Text);
                avsConfig.DownMix = this.downMixBox.Checked;
                avsConfig.Normalize = this.normalizeBox.Checked;
            }
        }

        private void SaveToJobConfig(JobItemConfig jobConfig)
        {
            jobConfig.VideoMode = (StreamProcessMode)this.cbVideoMode.SelectedIndex;
            jobConfig.AudioMode = (StreamProcessMode)this.cbAudioMode.SelectedIndex;
            if (jobConfig.VideoMode == ~StreamProcessMode.Encode)
            {
                jobConfig.VideoMode = StreamProcessMode.None;
            }
            if (jobConfig.AudioMode == ~StreamProcessMode.Encode)
            {
                jobConfig.AudioMode = StreamProcessMode.None;
            }
            if (this.muxerComboBox.Text == "MKV")
            {
                jobConfig.Container = OutputContainer.MKV;
            }
            else if (this.muxerComboBox.Text == "MP4")
            {
                jobConfig.Container = OutputContainer.MP4;
            }
        }

        private void SaveToSubtitleConfig(SubtitleConfig subtitleConfig)
        {
            subtitleConfig.Fontname = this.fontDialog1.Font.Name;
            int.TryParse(this.fontSizeBox.Text, out subtitleConfig.Fontsize);
            int.TryParse(this.fontBottomBox.Text, out subtitleConfig.MarginV);
            subtitleConfig.UsingStyle = this.customSubCheckBox.Checked;
        }

        private void SettleAudioControls()
        {
            if ((this._audioInfo.StreamsCount != 0) || this._usingSepAudio)
            {
                this.cbAudioMode.Enabled = true;
            }
            else
            {
                this.cbAudioMode.Enabled = false;
                this.cbAudioMode.SelectedIndex = -1;
            }
            if (((this._audioInfo.StreamsCount != 0) || this._usingSepAudio) && (this.cbAudioMode.SelectedIndex == 0))
            {
                this.audioSourceComboBox.Enabled = true;
                this.downMixBox.Enabled = true;
                this.normalizeBox.Enabled = true;
            }
            else
            {
                this.audioSourceComboBox.Enabled = false;
                this.downMixBox.Enabled = false;
                this.normalizeBox.Enabled = false;
            }
        }

        public void SetUpFormForItem(JobItem jobItem)
        {
            this._jobItem = jobItem;
            this.destFileBox.Text = this._jobItem.DestFile;
            this.tbSepAudio.Text = this._jobItem.ExternalAudio;
            this.subtitleTextBox.Text = jobItem.SubtitleFile;
            this._videoInfo = new VideoInfo(this._jobItem.SourceFile);
            this._audioInfo = new AudioInfo(this._jobItem.SourceFile);
            if (this._videoInfo.Format != "avs")
            {
                if (this.tabControl1.Controls.Count != 3)
                {
                    this.tabControl1.Controls.Clear();
                    TabPage[] tabPages = new TabPage[] { this.videoEditTabPage, this.audioEditTabPage, this.encTabPage, this.subtitleTabPage };
                    this.tabControl1.Controls.AddRange(tabPages);
                }
                this.InitializeJobConfig(this._jobItem.JobConfig);
                this.InitializeAvsConfig(this._jobItem.AvsConfig);
                this.InitializeSubtitleConfig(this._jobItem.SubtitleConfig);
            }
            else if (this.tabControl1.Controls.Count != 2)
            {
                this.tabControl1.Controls.Clear();
                TabPage[] tabPages = new TabPage[] { this.avsInputTabPage, this.encTabPage };
                this.tabControl1.Controls.AddRange(tabPages);
                this.AvsInputInitializeConfig(jobItem);
            }
            this._videoEncConfig = MyIO.Clone<VideoEncConfigBase>(this._jobItem.VideoEncConfig);
            this._audioEncConfig = MyIO.Clone<AudioEncConfigBase>(this._jobItem.AudioEncConfig);
            this.InitializeEncConfig();
        }

        private void SourceFrameRateCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (this.sourceFrameRateCheckBox.Checked)
            {
                this.frameRateBox.Text = this._videoInfo.FrameRate.ToString();
                this.frameRateBox.Enabled = false;
                this.convertFPSCheckBox.Enabled = false;
            }
            else
            {
                this.frameRateBox.Enabled = true;
                this.convertFPSCheckBox.Enabled = true;
            }
        }

        private void SourceResolutionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (this.sourceResolutionCheckBox.Checked)
            {
                if (this._videoInfo.HasVideo)
                {
                    this._resolutionCal.Mod = 2;
                    this._resolutionCal.AspectRatio = this._videoInfo.DisplayAspectRatio;
                    this._resolutionCal.Height = this._videoInfo.Height;
                    this._resolutionCal.Width = this._videoInfo.Width;
                    this.RefreshResolution(null);
                }
                IEnumerator enumerator = this.gbResolution.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control current = (Control)enumerator.Current;
                    current.Enabled = false;
                }
                this.sourceResolutionCheckBox.Enabled = true;
            }
            else
            {
                IEnumerator enumerator2 = this.gbResolution.Controls.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    Control control2 = (Control)enumerator2.Current;
                    control2.Enabled = true;
                }
                if (this.lockToSourceARCheckBox.Checked)
                {
                    this.aspectRatioBox.Enabled = false;
                }
            }
        }

        private void StringOptionChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.Enabled)
            {
                string name = box.Name.Replace('_', '-');
                (this._videoEncConfig as x264Config).SelectStringOption(name, box.SelectedIndex);
                this.RefreshX264UI();
            }
        }

        private void SubtitleButtonClick(object sender, EventArgs e)
        {
            if (this.subtitleTextBox.Text.Length > 0)
                this.openFileDialog2.FileName = this.subtitleTextBox.Text;
            else
                this.openFileDialog2.FileName = "";
                this.openFileDialog2.InitialDirectory = Path.GetDirectoryName(this._jobItem.SourceFile);
            this.openFileDialog2.ShowDialog();
            this.subtitleTextBox.Text = this.openFileDialog2.FileName;
        }

        public void UpdateProfiles(string[] profileNames, string selectedProfile)
        {
            this.profileBox.SelectedIndexChanged -= new EventHandler(this.ProfileBoxSelectedIndexChanged);
            this.profileBox.Items.Clear();
            this.profileBox.Items.AddRange(profileNames);
            this.profileBox.SelectedItem = selectedProfile;
            this.profileBox.SelectedIndexChanged += new EventHandler(this.ProfileBoxSelectedIndexChanged);
        }

        private void UseCustomCmdBoxCheckedChanged(object sender, EventArgs e)
        {
            if (this._cmdLineBox == null)
            {
                this._cmdLineBox = new CommandLineBox();
            }
            if (this.useCustomCmdBox.Checked)
            {
                IEnumerator enumerator = this.groupBox4.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control current = (Control)enumerator.Current;
                    current.Enabled = false;
                }
                this.useCustomCmdBox.Enabled = true;
                this.editCmdButton.Enabled = true;
                this._cmdLineBox.CmdLine = this._videoEncConfig.GetArgument();
                this._videoEncConfig.UsingCustomCmd = true;
            }
            else
            {
                IEnumerator enumerator2 = this.groupBox4.Controls.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    Control control2 = (Control)enumerator2.Current;
                    control2.Enabled = true;
                }
                this.editCmdButton.Enabled = false;
                this._cmdLineBox.CmdLine = string.Empty;
                this._videoEncConfig.UsingCustomCmd = false;
            }
        }

        private void UseSourceARCheckedChanged(object sender, EventArgs e)
        {
            this._resolutionCal.LockToSourceAR = this.lockToSourceARCheckBox.Checked;
            if (this.lockToSourceARCheckBox.Checked)
            {
                this.aspectRatioBox.Text = this._videoInfo.DisplayAspectRatio.ToString();
                this.aspectRatioBox.Enabled = false;
                this._resolutionCal.AspectRatio = this._videoInfo.DisplayAspectRatio;
                this.RefreshResolution(this.aspectRatioBox);
            }
            else
            {
                this.aspectRatioBox.Enabled = true;
            }
        }

        private void WidthBoxKeyUp(object sender, KeyEventArgs e)
        {
            int result = new int();
            int.TryParse(this.widthBox.Text, out result);
            if ((result > 0) && (result <= 0x780))
            {
                int width = this._resolutionCal.Width;
                this._resolutionCal.Width = result;
                if (this._resolutionCal.Height > 0x438)
                {
                    this._resolutionCal.Width = width;
                }
                else
                {
                    this.RefreshResolution(this.widthBox);
                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.destFileBox.Text = this.saveFileDialog1.FileName;
        }
    }
}


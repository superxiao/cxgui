namespace Cxgui.Gui
{
    using Clinky;
    using Cxgui;
    using Cxgui.AudioEncoding;
    using Cxgui.Avisynth;
    using Cxgui.Config;
    using Cxgui.Job;
    using Cxgui.VideoEncoding;
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
        protected AudioEncConfigBase audioEncConfig;
        /// <summary>
        /// 当选用外部音轨时，更新audioInfo。
        /// </summary>
        protected AudioInfo audioInfo;
        protected CommandLineBox cmdLineBox;
        protected JobItem jobItem;
        protected ControlResetter resetter;
        protected ResolutionCalculator resolutionCal;
        protected VideoEncConfigBase videoEncConfig;
        protected VideoInfo videoInfo;
        protected bool avsInputScriptEdited;

        public JobSettingForm()
        {
            this.InitializeComponent();
        }

        private void AllowAutoChangeARCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this.resolutionCal.LockAspectRatio = !this.allowAutoChangeARCheckBox.Checked;
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
                double aspectRatio = this.resolutionCal.AspectRatio;
                this.resolutionCal.AspectRatio = result;
                if ((this.resolutionCal.Width <= 0x780) && (this.resolutionCal.Height <= 0x438))
                {
                    this.RefreshResolution(this.aspectRatioBox);
                }
                else
                {
                    this.resolutionCal.AspectRatio = aspectRatio;
                }
            }
        }

        private void AvsInputInitializeConfig(JobItem jobItem)
        {
            // TODO 错误处理，读到无效文件
            this.avsInputScriptEditTextBox.Text = File.ReadAllText(this.jobItem.SourceFile, Encoding.Default);
            if (this.videoInfo.HasVideo)
            {
                this.avsVideoModeComboBox.Enabled = true;
                if (jobItem.JobConfig.VideoMode == StreamProcessMode.None && this.audioInfo.StreamsCount > 0)
                    this.avsVideoModeComboBox.SelectedIndex = 1;
                else
                    this.avsVideoModeComboBox.SelectedIndex = 0;
            }
            else
            {
                this.avsVideoModeComboBox.Enabled = false;
                this.avsVideoModeComboBox.SelectedIndex = -1;
            }
            // 当无视频时，也不需要附加音轨
            if (this.avsVideoModeComboBox.SelectedIndex != -1)
            {
                this.sepAudioCheckBox.Enabled = true;
                this.sepAudioCheckBox.Checked = this.jobItem.UsingExternalAudio;
            }
            else
            {
                this.sepAudioCheckBox.Enabled = false;
                this.sepAudioCheckBox.Checked = false;
            }
            if (this.sepAudioCheckBox.Checked || (this.audioInfo.StreamsCount != 0))
            {
                this.avsAudioModeComboBox.Enabled = true;
                if (jobItem.JobConfig.AudioMode == StreamProcessMode.None && this.videoInfo.HasVideo)
                    this.avsAudioModeComboBox.SelectedIndex = 2;
                else if (jobItem.JobConfig.AudioMode == StreamProcessMode.Copy)
                    this.avsAudioModeComboBox.SelectedIndex = 1;
                else
                    this.avsAudioModeComboBox.SelectedIndex = 0;
            }
            else
            {
                this.avsAudioModeComboBox.Enabled = false;
                this.avsAudioModeComboBox.SelectedIndex = -1;
            }
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
            if (this.avsVideoModeComboBox.SelectedIndex == 0)
                jobConfig.VideoMode = StreamProcessMode.Encode;
            else
                jobConfig.VideoMode = StreamProcessMode.None;
            jobConfig.AudioMode = (StreamProcessMode)this.avsAudioModeComboBox.SelectedIndex;
            if (jobConfig.AudioMode < 0)
                jobConfig.AudioMode = StreamProcessMode.None;
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
                (this.videoEncConfig as x264Config).SetBooleanOption(name, box.Checked);
                this.RefreshX264UI();
            }
        }

        private void BtOutBrowseClick(object sender, EventArgs e)
        {
            if (this.destFileBox.Text == string.Empty)
            {
                this.destFileBox.Text = this.jobItem.DestFile;
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
                    this.sepAudioTextBox.Text = this.openFileDialog1.FileName;
                    this.audioModeComboBox.SelectedIndex = 0;
                    this.audioInfo = info;
                }
                this.SettleAudioControls();
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.resetter.ResetControls();
            this.resetter.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CbAudioModeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Created && (this.audioModeComboBox.SelectedIndex == 2))
            {
                if (this.videoModeComboBox.SelectedIndex == -1 || this.videoModeComboBox.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.audioModeComboBox.SelectedIndex = 0;
                }
                this.SettleAudioControls();
            }
        }

        private void CbVideoModeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.videoModeComboBox.SelectedIndex == 0)
            {
                this.gbResolution.Enabled = true;
                this.gbVideoSource.Enabled = true;
            }
            else if (this.videoModeComboBox.SelectedIndex == 1)
            {
                this.gbResolution.Enabled = false;
                this.gbVideoSource.Enabled = false;
            }
            else if (this.videoModeComboBox.SelectedIndex == 2)
            {
                if (this.audioModeComboBox.SelectedIndex == -1 || this.audioModeComboBox.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.videoModeComboBox.SelectedIndex = 0;
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
            if (this.sepAudioCheckBox.Checked)
            {
                this.sepAudioTextBox.Enabled = true;
                this.sepAudioButton.Enabled = true;
                if (this.sepAudioTextBox.Text != string.Empty)
                {
                    if (this.audioModeComboBox.SelectedIndex == -1 || this.audioModeComboBox.SelectedIndex == 2)
                    {
                        this.audioModeComboBox.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                if (this.videoModeComboBox.SelectedIndex == -1 || this.videoModeComboBox.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.sepAudioCheckBox.Checked = true;
                }
                else
                {
                    this.sepAudioTextBox.Enabled = false;
                    this.sepAudioButton.Enabled = false;
                }
            }
            this.SettleAudioControls();
        }

        public void Clear()
        {
            this.videoEncConfig = null;
            this.audioEncConfig = null;
            this.videoInfo = null;
            this.audioInfo = null;
            this.resolutionCal = null;
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
            DialogResult result = this.cmdLineBox.ShowDialog();
            if (result == DialogResult.OK)
                this.videoEncConfig.CustomCmdLine = this.cmdLineBox.CmdLine;
        }

        private void FontButtonClick(object sender, EventArgs e)
        {
            this.fontDialog.Font = new Font(this.fontButton.Text, float.Parse(this.fontSizeBox.Text));
            this.fontDialog.ShowDialog();
            this.fontButton.Text = this.fontDialog.Font.Name;
            // UNDONE: 为什么返回小数，谜团
            MessageBox.Show(this.fontDialog.Font.Size.ToString());
            this.fontSizeBox.Text = this.fontDialog.Font.Size.ToString();
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
                int height = this.resolutionCal.Height;
                this.resolutionCal.Height = result;
                if (this.resolutionCal.Width > 0x780)
                {
                    this.resolutionCal.Height = height;
                }
                else
                {
                    this.RefreshResolution(this.heightBox);
                }
            }
        }

        private void InitializeAvsConfig(AvisynthConfig avsConfig)
        {
            this.resolutionCal = new ResolutionCalculator();
            this.InitializeResolutionCfg(avsConfig);
            this.InitializeFrameRateCfg(avsConfig);
            if (this.videoInfo.HasVideo)
            {
                this.InitializeResolution(avsConfig, this.videoInfo);
                this.InitializeFrameRate(avsConfig, this.videoInfo);
                this.disableAutoVideoScriptCheckBox.Checked = !avsConfig.PaddingCustomVideoScript;
                this.autoVideoScriptTextBox.Enabled = avsConfig.PaddingCustomVideoScript;
                this.CustomVideoScriptTextBox.Text = avsConfig.CustomVideoScript;
            }
            if (this.audioInfo.StreamsCount > 0)
            {
                this.CustomAudioScriptTextBox.Text = avsConfig.CustomAudioScript;
                this.disableAutoAudioScriptCheckBox.Checked = !avsConfig.PaddingCustomAudioScript;
                this.autoAudioScriptTextBox.Enabled = avsConfig.PaddingCustomAudioScript;
                this.CustomAudioScriptTextBox.Text = avsConfig.CustomAudioScript;
            }
            this.audioSourceComboBox.Text = avsConfig.AudioSourceFilter.ToString();
            this.downMixBox.Checked = avsConfig.DownMix;
            this.normalizeBox.Checked = avsConfig.Normalize;
            this.autoLoadSubtitleCheckBox.Checked = avsConfig.AutoLoadSubtitle;
        }

        private void InitializeEncConfig()
        {
            this.RefreshX264UI();
            this.RefreshNeroAac();
        }

        private void InitializeFrameRate(AvisynthConfig avsConfig, VideoInfo videoInfo)
        {
            if (this.jobItem.AvsConfig.UsingSourceFrameRate)
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
            this.videoModeComboBox.SelectedIndex = 1;
            this.audioModeComboBox.SelectedIndex = 1;
            if (this.videoInfo.HasVideo)
            {
                this.videoModeComboBox.Enabled = true;
                this.videoModeComboBox.SelectedIndex = (int)jobConfig.VideoMode;
            }
            else
            {
                this.videoModeComboBox.Enabled = false;
                this.videoModeComboBox.SelectedIndex = -1;
            }
            // 当无视频时，也不需要附加音轨
            if (this.videoModeComboBox.SelectedIndex != -1)
            {
                this.sepAudioCheckBox.Enabled = true;
                this.sepAudioCheckBox.Checked = this.jobItem.UsingExternalAudio;
            }
            else
            {
                this.sepAudioCheckBox.Enabled = false;
                this.sepAudioCheckBox.Checked = false;
            }
            if ((this.sepAudioCheckBox.Enabled && this.sepAudioCheckBox.Checked) || (this.audioInfo.StreamsCount != 0))
            {
                this.audioModeComboBox.Enabled = true;
                this.audioModeComboBox.SelectedIndex = (int)jobConfig.AudioMode;
            }
            else
            {
                this.audioModeComboBox.Enabled = false;
                this.audioModeComboBox.SelectedIndex = -1;
            }
            this.SettleAudioControls();
            if (this.videoModeComboBox.SelectedIndex != 0)
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
            this.resolutionCal.Mod = avsConfig.Mod;
            this.resolutionCal.LockAspectRatio = avsConfig.LockAspectRatio;
            this.resolutionCal.LockToSourceAR = avsConfig.LockToSourceAR;
            if (!avsConfig.LockToSourceAR && (avsConfig.AspectRatio > 0))
            {
                this.resolutionCal.AspectRatio = avsConfig.AspectRatio;
            }
            else
            {
                this.resolutionCal.AspectRatio = videoInfo.DisplayAspectRatio;
            }
            if (avsConfig.Height > 0)
            {
                this.resolutionCal.Height = avsConfig.Height;
            }
            else
            {
                this.resolutionCal.Height = videoInfo.Height;
            }
            if (avsConfig.Width > 0)
            {
                this.resolutionCal.Width = avsConfig.Width;
            }
            else
            {
                this.resolutionCal.Width = videoInfo.Width;
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
            if (this.resetter == null)
            {
                this.resetter = new ControlResetter();
            }
            this.resetter.SaveControls(this);
            // 这两个文本框是进入编辑脚本标签页才初始化，是故不计之
            this.resetter.RemoveControls(new Control[] { this.autoVideoScriptTextBox, this.autoAudioScriptTextBox });
        }

        private void MediaSettingFormFormClosed(object sender, FormClosedEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && this.resetter.Changed())
            {
                //foreach (Control control in this.resetter.GetChangedControls())
                //    MessageBox.Show(control.Name);
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
            if (this.videoInfo.HasVideo)
            {
                this.resolutionCal.Mod = int.Parse(this.modBox.Text);
                this.RefreshResolution(this.modBox);
            }
        }

        private void MuxerComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            string extension = this.muxerComboBox.Text.ToLower();
            this.destFileBox.Text = MyIO.GetUniqueName(Path.ChangeExtension(this.jobItem.DestFile, extension));
        }

        private void NeroAacRateControlBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            NeroAacConfig config = this.audioEncConfig as NeroAacConfig;
            switch (this.neroAacRateControlBox.SelectedIndex)
            {
                case 0:
                    this.label14.Text = "质量";
                    if (config.Quality == 0)
                    {
                        this.neroAacRateFactorBox.DecimalPlaces = 1;
                        this.neroAacRateFactorBox.Increment = (decimal)0.1;
                        this.neroAacRateFactorBox.Text = "0.5";
                        config.Quality = 0.5;
                    }
                    break;

                case 1:
                    this.label14.Text = "码率";
                    if (config.BitRate == 0)
                    {
                        this.neroAacRateFactorBox.DecimalPlaces = 0;
                        this.neroAacRateFactorBox.Increment = (decimal)1;
                        this.neroAacRateFactorBox.Text = "96";
                        config.BitRate = 96;
                    }
                    break;

                case 2:
                    this.label14.Text = "码率";
                    if (config.ConstantBitRate == 0)
                    {
                        this.neroAacRateFactorBox.DecimalPlaces = 0;
                        this.neroAacRateFactorBox.Increment = (decimal)1;
                        this.neroAacRateFactorBox.Text = "96";
                        config.ConstantBitRate = 96;
                    }
                    break;
            }
        }

        private void NeroAacRateFactorBoxValidating(object sender, CancelEventArgs e)
        {
            NeroAacConfig config = this.audioEncConfig as NeroAacConfig;
            if (this.neroAacRateControlBox.SelectedIndex == 0)
            {
                try
                {
                    config.Quality = double.Parse(this.neroAacRateFactorBox.Text);
                }
                catch (Exception)
                {
                    this.neroAacRateFactorBox.Text = config.Quality.ToString();
                }
            }
            else if (this.neroAacRateControlBox.SelectedIndex == 1)
            {
                try
                {
                    config.BitRate = int.Parse(this.neroAacRateFactorBox.Text);
                }
                catch (Exception)
                {
                    this.neroAacRateFactorBox.Text = config.BitRate.ToString();
                }
            }
            else
            {
                try
                {
                    config.ConstantBitRate = int.Parse(this.neroAacRateFactorBox.Text);
                }
                catch (Exception)
                {
                    this.neroAacRateFactorBox.Text = config.ConstantBitRate.ToString();
                }
            }
            this.RefreshNeroAac();
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            //videoEncConfig和audioEncConfig由jobItem克隆得来，跟随GUI即时变化
            this.jobItem.VideoEncConfig = this.videoEncConfig;
            this.jobItem.AudioEncConfig = this.audioEncConfig;
            if (this.jobItem.State != JobState.Waiting && this.jobItem.State != JobState.Working)
                this.jobItem.State = JobState.NotProccessed;
            this.jobItem.ProfileName = this.profileBox.Text;
            if (this.videoInfo.Container == "avs")
            {
                this.AvsInputSaveConfig(this.jobItem.JobConfig);
                this.avsInputScriptSaveButton.PerformClick();
            }

            else
            {
                this.jobItem.SubtitleFile = this.subtitleTextBox.Text;
                this.SaveToAvsConfig(this.jobItem.AvsConfig);
                this.SaveToJobConfig(this.jobItem.JobConfig);
                this.SaveToSubtitleConfig(this.jobItem.SubtitleConfig);
            } 
            
            try
            {
                string destDir = Path.GetDirectoryName(this.destFileBox.Text);
                if (destDir == "" || destDir == null)
                    this.destFileBox.Text = this.jobItem.DestFile;
                else if (MyIO.IsSameFile(this.jobItem.SourceFile, this.destFileBox.Text))
                {
                    MessageBox.Show("与源媒体文件同名。请更改文件名。", "文件重名", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (MyIO.Exists(this.destFileBox.Text))
                {
                    if (MessageBox.Show(Path.GetFileName(this.destFileBox.Text) + " 已存在。\n要替换它吗？", "确认另存为", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        return;
                    this.jobItem.DestFile = this.destFileBox.Text;
                }
                else if (Directory.Exists(destDir))
                {
                    this.jobItem.DestFile = this.destFileBox.Text;
                }
                else if (!Directory.Exists(destDir))
                {
                    if (MessageBox.Show("目标文件夹不存在。是否新建？", "文件夹不存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(this.destFileBox.Text));
                        this.jobItem.DestFile = this.destFileBox.Text;
                    }
                    else
                    {
                        this.destFileBox.Text = this.jobItem.DestFile;
                    }
                }
                else
                {
                    this.jobItem.DestFile = this.destFileBox.Text;
                }
            }
            catch (Exception)
            {
                this.destFileBox.Text = this.jobItem.DestFile;
            }

            if (this.sepAudioCheckBox.Checked)
            {
                if (this.sepAudioTextBox.Text != string.Empty)
                {
                    this.jobItem.UsingExternalAudio = true;
                    this.jobItem.ExternalAudio = this.sepAudioTextBox.Text;
                }
                else
                {
                    DialogResult result = MessageBox.Show("未指定外挂音轨。确定退出吗？", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (result == DialogResult.No)
                        return;
                    else if (result == DialogResult.Yes)
                    {
                        this.sepAudioCheckBox.Checked = false;
                        this.jobItem.UsingExternalAudio = false;
                        this.jobItem.ExternalAudio = string.Empty;
                    }
                }
            }
            else
            {
                this.jobItem.UsingExternalAudio = false;
                this.jobItem.ExternalAudio = string.Empty;
            }
            this.resetter.Clear();
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
                if (this.videoInfo.Container == "avs")
                    if (this.avsInputScriptEdited)
                    {
                        if (MessageBox.Show("是否保存对avs脚本的修改？", "avs脚本未保存", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            File.WriteAllText(this.jobItem.SourceFile, this.avsInputScriptEditTextBox.Text, Encoding.Default);
                        else
                            return;
                    }

                string sourceFile;
                AvisynthConfig avsConfig = new AvisynthConfig();
                JobItemConfig jobConfig = new JobItemConfig();
                SubtitleConfig subtitleConfig = new SubtitleConfig();
                string subtitle = string.Empty;
                string externalAudio = string.Empty;
                string contents = string.Empty;
                bool writeVideoScript = false;
                bool writeAudioScript = false;

                if (this.videoInfo.Container == "avs")
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
                    new VideoAvsWriter(this.jobItem.SourceFile, avsConfig, subtitle, this.videoInfo).WriteScript("video.avs");
                    contents += "video = import(\"video.avs\")";
                }

                if (this.sepAudioCheckBox.Checked && MyIO.Exists(this.sepAudioTextBox.Text))
                {
                    externalAudio = this.sepAudioTextBox.Text;
                }
                if (externalAudio != string.Empty)
                {
                    sourceFile = externalAudio;
                }
                else
                {
                    sourceFile = this.jobItem.SourceFile;
                }
                if (jobConfig.AudioMode != StreamProcessMode.None)
                {
                    writeAudioScript = true;
                    if (jobConfig.AudioMode == StreamProcessMode.Copy)
                    {
                        avsConfig.Normalize = false;
                        avsConfig.DownMix = false;
                    }
                    new AudioAvsWriter(sourceFile, avsConfig, this.audioInfo).WriteScript("audio.avs");
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
            this.videoEncConfig = profile.VideoEncConfig;
            this.audioEncConfig = profile.AudioEncConfig;
            this.InitializeEncConfig();
        }

        private void RateControlBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            x264Config config = this.videoEncConfig as x264Config;
            if (this.rateControlBox.SelectedIndex == 0)
            {
                config.SetNumOption("crf", (double)23);
                this.rateFactorBox.DecimalPlaces = 1;
                this.rateFactorBox.Increment = (decimal)0.1;
            }
            else if (this.rateControlBox.SelectedIndex == 1)
            {
                config.SetNumOption("qp", (double)23);
                this.rateFactorBox.DecimalPlaces = 0;
                this.rateFactorBox.Increment = (decimal)1;
            }
            else
            {
                config.SetNumOption("bitrate", (double)700);
                config.TotalPass = this.rateControlBox.SelectedIndex - 1;
                config.CurrentPass = 1;
                this.rateFactorBox.DecimalPlaces = 0;
                this.rateFactorBox.Increment = (decimal)1;
            }
            this.RefreshX264UI();
        }

        private void RateFactorBoxValidating(object sender, CancelEventArgs e)
        {
            string rateOption = "";
            double rateFactor;
            x264Config config = this.videoEncConfig as x264Config;
            if (this.rateControlBox.SelectedIndex == 0)
            {
                rateOption = "crf";
            }
            else if (this.rateControlBox.SelectedIndex == 1)
            {
                rateOption = "qp";
            }
            else
            {
                if (this.rateControlBox.SelectedIndex > 1)
                {
                    rateOption = "bitrate";
                }
            }
            try
            {
                rateFactor = double.Parse(this.rateFactorBox.Text);
            }
            catch (Exception)
            {
                this.rateFactorBox.Text = config.GetNode(rateOption).Num.ToString();
                return;
            }
            if (rateOption != "crf")
            {
                rateFactor = Math.Floor(rateFactor);
            }
            config.SetNumOption(rateOption, rateFactor);
            this.rateFactorBox.Text = config.GetNode(rateOption).Num.ToString();
        }

        private void RefreshNeroAac()
        {
            NeroAacConfig config = this.audioEncConfig as NeroAacConfig;
            if (config.Quality > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 0; 
                this.neroAacRateFactorBox.DecimalPlaces = 1;
                this.neroAacRateFactorBox.Increment = (decimal)0.1;
                this.neroAacRateFactorBox.Text = config.Quality.ToString();
            }
            else if (config.BitRate > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 1;
                this.neroAacRateFactorBox.DecimalPlaces = 0;
                this.neroAacRateFactorBox.Increment = (decimal)1;
                this.neroAacRateFactorBox.Text = config.BitRate.ToString();
            }
            else if (config.ConstantBitRate > 0)
            {
                this.neroAacRateControlBox.SelectedIndex = 2;
                this.neroAacRateFactorBox.DecimalPlaces = 0;
                this.neroAacRateFactorBox.Increment = (decimal)1;
                this.neroAacRateFactorBox.Text = config.ConstantBitRate.ToString();
            }
        }

        private void RefreshResolution(object caller)
        {
            if (caller != this.heightBox)
            {
                this.heightBox.Text = this.resolutionCal.Height.ToString();
            }
            if (caller != this.widthBox)
            {
                this.widthBox.Text = this.resolutionCal.Width.ToString();
            }
            if (caller != this.aspectRatioBox)
            {
                this.aspectRatioBox.Text = this.resolutionCal.AspectRatio.ToString();
            }
            this.modBox.SelectedIndexChanged -= new EventHandler(this.ModBoxSelectedIndexChanged);
            this.modBox.Text = this.resolutionCal.Mod.ToString();
            this.modBox.SelectedIndexChanged += new EventHandler(this.ModBoxSelectedIndexChanged);
        }

        private void RefreshX264UI()
        {
            x264Config config = this.videoEncConfig as x264Config;
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
                this.rateFactorBox.DecimalPlaces = 1;
                this.rateFactorBox.Increment = (decimal)0.1;
            }
            else if (config.GetNode("qp").InUse)
            {
                this.rateControlBox.SelectedIndex = 1;
                this.rateFactorBox.Text = config.GetNode("qp").Num.ToString();
                this.label9.Text = "质量";
                this.rateFactorBox.DecimalPlaces = 0;
                this.rateFactorBox.Increment = (decimal)1;
            }
            else if (config.GetNode("bitrate").InUse)
            {
                this.rateControlBox.SelectedIndex = config.TotalPass + 1;
                this.rateFactorBox.Text = config.GetNode("bitrate").Num.ToString();
                this.label9.Text = "码率";
                this.rateFactorBox.DecimalPlaces = 0;
                this.rateFactorBox.Increment = (decimal)1;
            }
            this.rateControlBox.SelectedIndexChanged += new EventHandler(this.RateControlBoxSelectedIndexChanged);

            this.cmdLineBox = new CommandLineBox();
            bool checkChanged = (this.useCustomCmdBox.Checked != this.videoEncConfig.UsingCustomCmd);
            this.useCustomCmdBox.Checked = this.videoEncConfig.UsingCustomCmd;
            if (!checkChanged)
            {
                this.UseCustomCmdBoxCheckedChanged(null, null);
            }
        }

        private void ResolutionValidating(object sender, CancelEventArgs e)
        {
            this.RefreshResolution(null);
        }

        private void SaveProfileButtonClick(object sender, EventArgs e)
        {
            if (this.profileBox.Text == string.Empty || this.profileBox.Text.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                MessageBox.Show("预设名为空或含非法字符。", "错误的预设名", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AvisynthConfig avsConfig = null;
            JobItemConfig jobConfig = null;
            SubtitleConfig subtitleConfig = null;
            if (this.videoInfo.Container == "avs")
            {
                avsConfig = this.jobItem.AvsConfig;
                subtitleConfig = this.jobItem.SubtitleConfig;
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
            Profile.Save(this.profileBox.Text, jobConfig, avsConfig, this.videoEncConfig, this.audioEncConfig, subtitleConfig);
            if (!this.profileBox.Items.Contains(this.profileBox.Text))
            {
                this.profileBox.Items.Add(this.profileBox.Text);
            }
        }

        private void SaveToAvsConfig(AvisynthConfig avsConfig)
        {
            if (this.videoInfo.HasVideo)
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
                avsConfig.AutoLoadSubtitle = this.autoLoadSubtitleCheckBox.Checked;
                avsConfig.PaddingCustomVideoScript = !this.disableAutoVideoScriptCheckBox.Checked;
                avsConfig.PaddingCustomAudioScript = !this.disableAutoAudioScriptCheckBox.Checked;
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
                avsConfig.CustomVideoScript = this.CustomVideoScriptTextBox.Text;
            }
            if (this.audioInfo.StreamsCount != 0)
            {
                avsConfig.AudioSourceFilter = (AudioSourceFilter)Enum.Parse(typeof(AudioSourceFilter), this.audioSourceComboBox.Text);
                avsConfig.DownMix = this.downMixBox.Checked;
                avsConfig.Normalize = this.normalizeBox.Checked;
                avsConfig.CustomAudioScript = this.CustomAudioScriptTextBox.Text;
            }
        }

        private void SaveToJobConfig(JobItemConfig jobConfig)
        {
            if (this.videoInfo.Container == "avs")
            {
                if (!this.videoInfo.HasVideo)
                    jobConfig.VideoMode = StreamProcessMode.None;
                if (this.audioInfo.StreamsCount==0)
                    jobConfig.AudioMode = StreamProcessMode.None;
            }
            else
            {
                jobConfig.VideoMode = (StreamProcessMode)this.videoModeComboBox.SelectedIndex;
                jobConfig.AudioMode = (StreamProcessMode)this.audioModeComboBox.SelectedIndex;
                if ((int)jobConfig.VideoMode == -1)
                {
                    jobConfig.VideoMode = StreamProcessMode.None;
                }
                if ((int)jobConfig.AudioMode == -1)
                {
                    jobConfig.AudioMode = StreamProcessMode.None;
                }
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
            subtitleConfig.Fontname = this.fontDialog.Font.Name;
            int.TryParse(this.fontSizeBox.Text, out subtitleConfig.Fontsize);
            int.TryParse(this.fontBottomBox.Text, out subtitleConfig.MarginV);
            subtitleConfig.UsingStyle = this.customSubCheckBox.Checked;
        }

        private void SettleAudioControls()
        {
            if ((this.audioInfo.StreamsCount != 0) || this.sepAudioCheckBox.Checked)
            {
                this.audioModeComboBox.Enabled = true;
            }
            else
            {
                this.audioModeComboBox.Enabled = false;
                this.audioModeComboBox.SelectedIndex = -1;
            }
            if (this.audioModeComboBox.Enabled && this.audioModeComboBox.SelectedIndex == 0)
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
            this.jobItem = jobItem;
            this.destFileBox.Text = this.jobItem.DestFile;
            this.sepAudioTextBox.Text = this.jobItem.ExternalAudio;
            this.subtitleTextBox.Text = jobItem.SubtitleFile;
            this.videoInfo = this.jobItem.VideoInfo;
            this.audioInfo = this.jobItem.AudioInfo;
            if (this.videoInfo.Container != "avs")
            {
                this.mainTabControl.Controls.Clear();
                TabPage[] tabPages = new TabPage[] { this.videoEditTabPage, this.audioEditTabPage, this.encTabPage, this.subtitleTabPage, this.scriptTabPage };
                this.mainTabControl.Controls.AddRange(tabPages);
                this.audioEditTabPage.Controls.AddRange(new Control[] { this.sepAudioButton, this.sepAudioTextBox, this.sepAudioCheckBox });
                this.scriptTabControl.Controls.Clear();
                tabPages = new TabPage[] { this.videoScriptTabPage, this.audioScriptTabPage };
                if (this.jobItem.JobConfig.VideoMode == StreamProcessMode.Encode)
                    this.scriptTabControl.Controls.Add(this.videoScriptTabPage);
                if (this.jobItem.JobConfig.AudioMode == StreamProcessMode.Encode)
                    this.scriptTabControl.Controls.Add(this.audioScriptTabPage);

                this.InitializeJobConfig(this.jobItem.JobConfig);
                this.InitializeAvsConfig(this.jobItem.AvsConfig);
                this.InitializeSubtitleConfig(this.jobItem.SubtitleConfig);
            }
            else
            {
                this.mainTabControl.Controls.Clear();
                TabPage[] tabPages = new TabPage[] { this.avsInputTabPage, this.encTabPage, this.scriptTabPage };
                this.mainTabControl.Controls.AddRange(tabPages);
                this.avsInputTabPage.Controls.AddRange(new Control[] { this.sepAudioButton, this.sepAudioTextBox, this.sepAudioCheckBox });
                this.scriptTabControl.Controls.Clear();
                if (this.jobItem.JobConfig.VideoMode == StreamProcessMode.Encode || this.jobItem.JobConfig.AudioMode == StreamProcessMode.Encode)
                    this.scriptTabControl.Controls.Add(this.inputScriptTabPage);

                this.AvsInputInitializeConfig(jobItem);
            }
            this.videoEncConfig = MyIO.Clone<VideoEncConfigBase>(this.jobItem.VideoEncConfig);
            this.audioEncConfig = MyIO.Clone<AudioEncConfigBase>(this.jobItem.AudioEncConfig);
            this.InitializeEncConfig();
        }

        private void SourceFrameRateCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (this.sourceFrameRateCheckBox.Checked)
            {
                this.frameRateBox.Text = this.videoInfo.FrameRate.ToString();
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
                if (this.videoInfo.HasVideo)
                {
                    this.resolutionCal.Mod = 2;
                    this.resolutionCal.AspectRatio = this.videoInfo.DisplayAspectRatio;
                    this.resolutionCal.Height = this.videoInfo.Height;
                    this.resolutionCal.Width = this.videoInfo.Width;
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
                (this.videoEncConfig as x264Config).SelectStringOption(name, box.SelectedIndex);
                this.RefreshX264UI();
            }
        }

        private void SubtitleButtonClick(object sender, EventArgs e)
        {
            if (this.subtitleTextBox.Text.Length > 0)
                this.openFileDialog2.FileName = this.subtitleTextBox.Text;
            else
                this.openFileDialog2.FileName = "";
                this.openFileDialog2.InitialDirectory = Path.GetDirectoryName(this.jobItem.SourceFile);
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
            if (this.useCustomCmdBox.Checked)
            {
                foreach(Control control in this.groupBox4.Controls)
                {
                    control.Enabled = false;
                }
                this.useCustomCmdBox.Enabled = true;
                this.editCmdButton.Enabled = true;
                if (this.videoEncConfig.CustomCmdLine == null)
                    this.cmdLineBox.CmdLine = this.videoEncConfig.GetArgument();
                else
                    this.cmdLineBox.CmdLine = this.videoEncConfig.CustomCmdLine;
                this.videoEncConfig.UsingCustomCmd = true;
            }
            else
            {
                foreach (Control control in this.groupBox4.Controls)
                {
                    control.Enabled = true;
                }
                this.editCmdButton.Enabled = false;
                this.cmdLineBox.CmdLine = string.Empty;
                this.videoEncConfig.UsingCustomCmd = false;
                this.videoEncConfig.CustomCmdLine = null;
            }
        }

        private void UseSourceARCheckedChanged(object sender, EventArgs e)
        {
            this.resolutionCal.LockToSourceAR = this.lockToSourceARCheckBox.Checked;
            if (this.lockToSourceARCheckBox.Checked)
            {
                this.aspectRatioBox.Text = this.videoInfo.DisplayAspectRatio.ToString();
                this.aspectRatioBox.Enabled = false;
                this.resolutionCal.AspectRatio = this.videoInfo.DisplayAspectRatio;
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
                int width = this.resolutionCal.Width;
                this.resolutionCal.Width = result;
                if (this.resolutionCal.Height > 0x438)
                {
                    this.resolutionCal.Width = width;
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

        private void comboBoxAvsVideoMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Created && this.avsVideoModeComboBox.SelectedIndex == 1)
            {
                if (this.avsAudioModeComboBox.SelectedIndex == -1 || this.avsAudioModeComboBox.SelectedIndex == 2)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.avsVideoModeComboBox.SelectedIndex = 0;
                }
            }
        }

        private void comboBoxAvsAudioMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Created && this.avsAudioModeComboBox.SelectedIndex == 2)
            {
                if (this.avsVideoModeComboBox.SelectedIndex != 0)
                {
                    MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.avsAudioModeComboBox.SelectedIndex = 0;
                }
            }
            else if (this.Created && this.avsAudioModeComboBox.SelectedIndex == 1)
            {
                if (!this.sepAudioCheckBox.Checked)
                {
                    MessageBox.Show("avs脚本音频无法复制。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.avsAudioModeComboBox.SelectedIndex = 0;
                }
                
            }
        }

        private void sepAudioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.sepAudioCheckBox.Checked)
            {
                this.sepAudioButton.Enabled = true;
                this.sepAudioTextBox.Enabled = true;
                if (!this.avsAudioModeComboBox.Enabled)
                {
                    this.avsAudioModeComboBox.Enabled = true;
                    this.avsAudioModeComboBox.SelectedIndex = 0;
                }
                if (!this.audioModeComboBox.Enabled)
                {
                    this.audioModeComboBox.Enabled = true;
                    this.audioModeComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                this.sepAudioButton.Enabled = false;
                this.sepAudioTextBox.Enabled = false;
                if (this.audioInfo.StreamsCount == 0)
                {
                    this.avsAudioModeComboBox.Enabled = false;
                    this.avsAudioModeComboBox.SelectedIndex = -1;
                    this.audioModeComboBox.Enabled = false;
                    this.audioModeComboBox.SelectedIndex = -1;
                }
                else if (this.avsAudioModeComboBox.SelectedIndex == 1)
                    this.avsAudioModeComboBox.SelectedIndex = 0;
            }
        }

        private void autoLoadSubtitleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Created && this.autoLoadSubtitleCheckBox.Checked)
            {
                this.subtitleTextBox.Text = this.jobItem.FindFirstSubtitleFile();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.subtitleTextBox.Text = string.Empty;
        }

        private void delProfileButton_Click(object sender, EventArgs e)
        {
            if (this.profileBox.SelectedIndex != -1)
            {
                DialogResult result = MessageBox.Show("确定要删除预设项 " + this.profileBox.Text + "?", "确认操作", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    Profile.DeleteProfile(this.profileBox.Text);
                    this.profileBox.Items.Remove(this.profileBox.SelectedItem);
                }
            }

        }

        private void disableAutoVideoScriptCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Created)
            {
                AvisynthConfig avsConfig = new AvisynthConfig();
                this.SaveToAvsConfig(avsConfig);
                VideoAvsWriter writer = new VideoAvsWriter(this.videoInfo.FilePath, avsConfig, this.subtitleTextBox.Text, this.videoInfo);
                if (this.disableAutoVideoScriptCheckBox.Checked)
                {
                    this.autoVideoScriptTextBox.Enabled = false;
                    DialogResult result = MessageBox.Show("是否增加源滤镜语句到自定义脚本的开头？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                        this.CustomVideoScriptTextBox.Text = writer.GetFilterStatement("SourceFilter") + Environment.NewLine + this.CustomVideoScriptTextBox.Text;
                    this.autoVideoScriptTextBox.Text = "";

                }
                else
                {
                    this.autoVideoScriptTextBox.Enabled = true;
                    this.autoVideoScriptTextBox.Text = writer.GetScriptContent(false);
                }
            }
        }

        private void avsInputScriptSaveButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText(this.jobItem.SourceFile, this.avsInputScriptEditTextBox.Text, Encoding.Default);
            this.avsInputScriptEdited = false;
        }

        private void scriptTabPage_Enter(object sender, EventArgs e)
        {
            
            AvisynthConfig avsConfig = new AvisynthConfig();
            if (this.videoInfo.Container!="avs")
                    this.SaveToAvsConfig(avsConfig);
            if (!this.disableAutoVideoScriptCheckBox.Checked)
                this.autoVideoScriptTextBox.Text = new VideoAvsWriter(this.videoInfo.FilePath, avsConfig, this.subtitleTextBox.Text, this.videoInfo).GetScriptContent(false);
            if (!this.disableAutoAudioScriptCheckBox.Checked)
                this.autoAudioScriptTextBox.Text = new AudioAvsWriter(this.audioInfo.FilePath, avsConfig, this.audioInfo).GetScriptContent(false);
        }

        private void disableAutoAudioScriptCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Created)
            {
                AvisynthConfig avsConfig = new AvisynthConfig();
                this.SaveToAvsConfig(avsConfig);
                AudioAvsWriter writer = new AudioAvsWriter(this.audioInfo.FilePath, avsConfig, this.audioInfo);
                if (this.disableAutoAudioScriptCheckBox.Checked)
                {
                    this.autoAudioScriptTextBox.Enabled = false;
                    DialogResult result = MessageBox.Show("是否增加源滤镜语句到自定义脚本的开头？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                        // 去除"audio="
                        this.CustomAudioScriptTextBox.Text = writer.GetFilterStatement("SourceFilter").Remove(0, 6) + Environment.NewLine + this.CustomAudioScriptTextBox.Text;
                    this.autoAudioScriptTextBox.Text = "";

                }
                else
                {
                    this.autoAudioScriptTextBox.Enabled = true;
                    this.autoAudioScriptTextBox.Text = writer.GetScriptContent(false);
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.A)
                {
                    (sender as TextBox).SelectAll();
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
                if (e.KeyCode == System.Windows.Forms.Keys.S)
                    this.avsInputScriptSaveButton.PerformClick();
            }
        }

        private void avsInputScriptEditTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.Created)
                this.avsInputScriptEdited = true;
        }

        private void forbidEmptyValidating(object sender, CancelEventArgs e)
        {
            NumericUpDown upDown = sender as NumericUpDown;
            upDown.Text = upDown.Value.ToString();
        }
    }
}


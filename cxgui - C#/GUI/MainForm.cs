namespace CXGUI.GUI
{
    using Clinky;
    using CXGUI;
    using CXGUI.AudioEncoding;
    using CXGUI.Avisynth;
    using CXGUI.Config;
    using CXGUI.Job;
    using CXGUI.VideoEncoding;
    using External;
    using DirectShowLib.DES;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public partial class MainForm : Form
    {
        protected ProgramConfigForm _configForm;
        protected JobSettingForm _jobSettingForm;
        protected AboutForm _aboutForm;
        protected bool _workerReporting;
        protected JobItem _workingJobItem;
        protected List<JobItem> _workingJobItems;
        private delegate void ReportInvoke(JobItem jobItem, IMediaProcessor encoder, DoWorkEventArgs e);
        
        [STAThread]
        public static void Main(string[] argv)
        {
            if (!Directory.Exists("tools"))
            {
                Directory.CreateDirectory("tools");
            }
            Directory.SetCurrentDirectory("tools");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public MainForm()
        {
            this.InitializeComponent();
            ProgramConfig configSection = ProgramConfig.Get();
            this._configForm = new ProgramConfigForm(configSection);
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void BackgroundWorker1DoWork(object sender, DoWorkEventArgs e)
        {
            JobItem jobItem = null;
            try
            {
                jobItem = (JobItem)e.Argument; 
                if (jobItem.JobConfig.VideoMode == StreamProcessMode.None && jobItem.JobConfig.AudioMode == StreamProcessMode.None)
                {
                    jobItem.Event = JobEvent.Error;
                    this.JobEventReport(jobItem);
                    MessageBox.Show(jobItem.SourceFile + "\n源文件不是视频文件，或设置有错误。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (this._workingJobItems[this._workingJobItems.Count - 1] == jobItem)
                    {
                        jobItem.Event = JobEvent.AllDone;
                        this.JobEventReport(jobItem);
                    }
                    e.Result = jobItem;
                }
                else if (MyIO.Exists(jobItem.DestFile) && (MessageBox.Show(new StringBuilder().Append(jobItem.DestFile).Append("\n目标文件已存在。决定覆盖吗？").ToString(), "文件已存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel))
                {
                    jobItem.Event = JobEvent.OneJobItemCancelled;
                    this.JobEventReport(jobItem);
                    if (this._workingJobItems[this._workingJobItems.Count - 1] == jobItem)
                    {
                        jobItem.Event = JobEvent.AllDone;
                        this.JobEventReport(jobItem);
                    }
                    e.Result = jobItem;
                }
                else
                {
                    jobItem.Event = JobEvent.OneJobItemProcessing;
                    this.JobEventReport(jobItem);
                    if (jobItem.JobConfig.VideoMode == StreamProcessMode.Encode)
                    {
                        this.ProcessVideo(jobItem, e);
                        if (this.DidUserPressStopButton(jobItem, e))
                        {
                            return;
                        }
                    }
                    if (jobItem.JobConfig.AudioMode == StreamProcessMode.Encode)
                    {
                        this.ProcessAudio(jobItem, e);
                        if (this.DidUserPressStopButton(jobItem, e))
                        {
                            return;
                        }
                    }
                    if (jobItem.Muxer != null)
                    {
                        this.DoMuxStuff(jobItem, e);
                        if (this.DidUserPressStopButton(jobItem, e))
                        {
                            return;
                        }
                    }
                    if (jobItem.State != JobState.Error)
                    {
                        jobItem.Event = JobEvent.OneJobItemDone;
                        this.JobEventReport(jobItem);
                    }
                }
            }
            // BOOKMARK: BackgroundWorker1DoWork统一错误处理
            //catch (Exception exception)
            //{
            //MessageBox.Show("发生了一个错误。\n" + exception.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //jobItem.Event = JobEvent.Error;
            //this.JobEventReport(jobItem);
            //}
            finally
            {
                if (this._workingJobItems[this._workingJobItems.Count-1] == jobItem)
                {
                    jobItem.Event = JobEvent.AllDone;
                    this.JobEventReport(jobItem);
                }
                e.Result = jobItem;
            }
        }

        private void BackgroundWorker1ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            JobItem jobItem = (JobItem)e.UserState;
            if (jobItem.Event == JobEvent.VideoEncoding)
            {
                if (jobItem.VideoEncoder.ProcessingFrameRate != 0)
                {
                    this.videoProgressBar.Value = jobItem.VideoEncoder.Progress;
                    this.videoTimeUsedLable.Text = jobItem.VideoEncoder.TimeUsed.ToString();
                    this.videoTimeLeftLable.Text = jobItem.VideoEncoder.TimeLeft.ToString();
                    this.videoEncodingFpsLable.Text = jobItem.VideoEncoder.ProcessingFrameRate.ToString(".00' fps'");
                    this.videoEstimatedFileSizeLable.Text = ((double)jobItem.VideoEncoder.EstimatedFileSize / 1024).ToString(".00' MB'");
                    this.videoAvgBitRateLable.Text = jobItem.VideoEncoder.AvgBitRate.ToString(".00' kbps");
                }
            }
            else if (jobItem.Event == JobEvent.AudioEncoding)
            {
                this.audioProgressBar.Value = jobItem.AudioEncoder.Progress;
                this.audioTimeUsed.Text = jobItem.AudioEncoder.TimeUsed.ToString();
                this.audioTimeLeft.Text = jobItem.AudioEncoder.TimeLeft.ToString();

            }
            else if (jobItem.Event == JobEvent.Muxing)
            {
                this.muxProgressBar.Value = jobItem.Muxer.Progress;
                this.muxTimeUsed.Text = jobItem.Muxer.TimeUsed.ToString();
                this.muxTimeLeft.Text = jobItem.Muxer.TimeLeft.ToString();
            }
            else
            {
                int index;
                if (jobItem.Event == JobEvent.OneJobItemProcessing)
                {
                    this.ResetProgress();
                    jobItem.State = JobState.Working;
                    this.startButton.Enabled = false;
                    index = this._workingJobItems.IndexOf(jobItem);
                    this.statusLable.Text = new StringBuilder("正在处理第").Append(index + 1).Append("个文件，共").Append(this._workingJobItems.Count).Append("个文件").ToString();
                }
                else if (jobItem.Event == JobEvent.OneJobItemDone)
                {
                    jobItem.FilesToDeleteWhenProcessingFails.Clear();
                    jobItem.State = JobState.Done;
                    index = this._workingJobItems.IndexOf(jobItem);
                    this.statusLable.Text = new StringBuilder("第").Append(index + 1).Append("个文件处理完毕，共").Append(this._workingJobItems.Count).Append("个文件").ToString();
                }
                else if (jobItem.Event == JobEvent.AllDone)
                {
                    this.statusLable.Text = new StringBuilder().Append(this._workingJobItems.Count).Append("个文件处理完成").ToString();
                    this.startButton.Enabled = true;
                }
                else if (jobItem.Event == JobEvent.QuitAllProcessing)
                {
                    this.ResetProgress();
                    jobItem.State = JobState.Stop;
                    this.startButton.Enabled = true;
                    this.statusLable.Text = "中止";
                    this.tabControl1.SelectTab(this.inputPage);
                    foreach (string file in jobItem.FilesToDeleteWhenProcessingFails)
                    {
                        File.Delete(file);
                    }
                    jobItem.FilesToDeleteWhenProcessingFails.Clear();
                }
                else if (jobItem.Event == JobEvent.OneJobItemCancelled)
                {
                    jobItem.State = JobState.Stop;
                    this.statusLable.Text = "中止";
                    foreach (string file in jobItem.FilesToDeleteWhenProcessingFails)
                    {
                        File.Delete(file);
                    }
                    jobItem.FilesToDeleteWhenProcessingFails.Clear();
                }
                else if (jobItem.Event == JobEvent.Error)
                {
                    jobItem.State = JobState.Error;
                    this.statusLable.Text = "错误";
                    this.backgroundWorker1.CancelAsync();
                    foreach (string file in jobItem.FilesToDeleteWhenProcessingFails)
                    {
                        File.Delete(file);
                    }
                    jobItem.FilesToDeleteWhenProcessingFails.Clear();
                }
            }
            this._workerReporting = false;
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ListViewItem current = (ListViewItem) enumerator.Current;
                current.Selected = true;
            }
            this.delButton.PerformClick();
            this.settingButton.Enabled = false;
        }

        private void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
        {
            if (this.jobItemListView.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
            else if (this.jobItemListView.SelectedItems.Count == 1)
            {
                this.listViewMenu.Items["设置ToolStripMenuItem"].Enabled = true;
                this.listViewMenu.Items["打开目录ToolStripMenuItem"].Enabled = true;
            }
            else
            {
                this.listViewMenu.Items["设置ToolStripMenuItem"].Enabled = false;
                this.listViewMenu.Items["打开目录ToolStripMenuItem"].Enabled = false;
            }
        }

        private void DelButtonClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                this.jobItemListView.Items.Remove(current);
                if (((this._workingJobItems != null) && this._workingJobItems.Contains(current.JobItem)) && (this._workingJobItems.IndexOf(current.JobItem) > this._workingJobItems.IndexOf(this._workingJobItem)))
                {
                    this._workingJobItems.Remove(current.JobItem);
                }
            }
        }

        private bool DidUserPressStopButton(JobItem jobItem, DoWorkEventArgs e)
        {
            if (this.backgroundWorker1.CancellationPending)
            {
                jobItem.Event = JobEvent.QuitAllProcessing;
                this.JobEventReport(jobItem);
                return true;
            }
            return false;
        }

        private void DoMuxStuff(JobItem jobItem, DoWorkEventArgs e)
        {
            string encodedAudio;
            string encodedVideo;
            if (jobItem.JobConfig.AudioMode == StreamProcessMode.Encode)
            {
                encodedAudio = jobItem.EncodedAudio;
                jobItem.EncodedAudio = string.Empty;
            }
            else if (jobItem.JobConfig.AudioMode == StreamProcessMode.Copy)
            {
                if (jobItem.UsingExternalAudio && (jobItem.ExternalAudio != string.Empty))
                {
                    encodedAudio = jobItem.ExternalAudio;
                }
                else
                {
                    encodedAudio = jobItem.SourceFile;
                }
            }
            else
            {
                encodedAudio = string.Empty;
            }
            if (jobItem.JobConfig.VideoMode == StreamProcessMode.Encode)
            {
                encodedVideo = jobItem.EncodedVideo;
                jobItem.EncodedVideo = string.Empty;
            }
            else if (jobItem.JobConfig.VideoMode == StreamProcessMode.Copy)
            {
                encodedVideo = jobItem.SourceFile;
            }
            else
            {
                encodedVideo = string.Empty;
            }
            jobItem.FilesToDeleteWhenProcessingFails.Add(jobItem.DestFile);
            this.Mux(encodedVideo, encodedAudio, jobItem.DestFile, e);
        }

        private void EncodeAudio(string avsFile, string destFile, AudioEncConfigBase config, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem) e.Argument;
            try
            {
                NeroAacHandler encoder = new NeroAacHandler(avsFile, destFile);
                encoder.Config = config as NeroAacConfig;
                jobItem.AudioEncoder = encoder;
                jobItem.Event = JobEvent.AudioEncoding;
                ReportInvoke encodingReport = this.EncodingReport;
                IAsyncResult result = encodingReport.BeginInvoke(jobItem, encoder, e, null, null);
                if (!this.backgroundWorker1.CancellationPending)
                {
                    encoder.Start();
                }
                encodingReport.EndInvoke(result);
                jobItem.AudioEncoder = null;
            }
            // 有效脚本，但不含音频。注意仅当源文件原本含音频流或输入avs脚本含音频时当前函数可能被调用。
            catch (AvisynthAudioStreamNotFoundException)
            {
                string err;
                if (jobItem.UsingExternalAudio && jobItem.ExternalAudio.Length > 0)
                    err = jobItem.ExternalAudio;
                else
                    err = jobItem.SourceFile;
                if (jobItem.VideoInfo.Container == "avs")
                    // 输入音频脚本被改为不含音频的有效的脚本（视频脚本）
                    err += "\n音频编码失败，原因是输入avs脚本有错误。";
                else
                    // 由于对于非avs脚本的媒体文件采用内部编写音频脚本的方式，如果出错则该脚本必然无效，因此不会出现以下错误
                    err += "\n音频编码失败";
                MessageBox.Show(err, "检测失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            catch (AviSynthException exception)
            {
                string err;
                if (jobItem.UsingExternalAudio && jobItem.ExternalAudio.Length > 0)
                    err = jobItem.ExternalAudio + "\n";
                else
                    err = jobItem.SourceFile + "\n";
                // 输入avs脚本被更改为无效的avs脚本
                if (jobItem.VideoInfo.Container == "avs")
                    err += "音频编码失败，原因是输入avs脚本有错误。\n\n" + exception.ToString();
                // 对于非avs脚本的媒体文件，可能虽然源文件含音频流，但无对应DShow滤镜而使avs脚本无效；或源文件被更改为非媒体文件
                else if (jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.DirectShowSource || jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.DSS2)
                {
                    err += "音频编码失败。\n";
                    if (jobItem.VideoInfo.Format.Length > 0)
                        err += "\n视频格式：" + jobItem.VideoInfo.Format;
                    if (jobItem.AudioInfo.Format.Length > 0)
                        err += "\n音频格式：" + jobItem.AudioInfo.Format;
                    if (jobItem.VideoInfo.Container.Length > 0)
                        err += "\n容器：" + jobItem.VideoInfo.Container;
                    err += "\n可能是没有为该音频安装正确的DirectShow解码器或分离器。\n可以尝试安装解码包，或ffdshow解码器和Haali分离器等。\n也可以在转换设置中改用FFAudioSource源滤镜，则不需要安装相关的DirectShow解码器。";
                }
                else
                    err += "音频编码失败。请尝试更改音频源滤镜。";
                MessageBox.Show(err, "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            catch (Exception exception)
            {
                string err;
                if (jobItem.UsingExternalAudio && jobItem.ExternalAudio.Length > 0)
                    err = jobItem.ExternalAudio;
                else
                    err = jobItem.SourceFile;
                MessageBox.Show(err + "\n音频编码失败。" + exception.ToString());
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
        }

        private void EncodeVideo(string avsFile, string destFile, VideoEncConfigBase config, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem)e.Argument;
            try
            {
                x264Handler encoder = new x264Handler(avsFile, destFile);
                encoder.Config = config as x264Config;
                jobItem.VideoEncoder = encoder;
                jobItem.Event = JobEvent.VideoEncoding;
                ReportInvoke encodingReport = this.EncodingReport;
                IAsyncResult result = encodingReport.BeginInvoke(jobItem, encoder, e, null, null);
                if (!this.backgroundWorker1.CancellationPending)
                    encoder.Start();
                encodingReport.EndInvoke(result);
                jobItem.VideoEncoder = null;
            }
            catch (BadEncoderCmdException)
            {
                MessageBox.Show("视频编码失败。是否使用了不正确的命令行？", "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.backgroundWorker1.CancelAsync();
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            // 有效脚本，但不含视频。注意仅当源文件原本含视频流或输入avs脚本含视频时当前函数可能被调用。
            catch (AvisynthVideoStreamNotFoundException)
            {
                string err = jobItem.SourceFile+"\n";
                if (jobItem.VideoInfo.Container == "avs")
                {
                    // 输入视频脚本被改为不含视频的有效的脚本（音频脚本）
                    err += "视频编码失败，原因是输入avs脚本有错误。";
                }
                else 
                { 
                    // 由于对于非avs脚本的媒体文件采用内部编写视频脚本的方式，如果出错则该脚本必然无效，因此不会出现以下错误
                    err += "视频编码失败。";
                }
                MessageBox.Show(err, "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            // 无效脚本。注意仅当源文件原本含视频流或输入avs脚本含视频时本函数可能被调用。
            catch (AviSynthException exception)
            {
                string err = jobItem.SourceFile+"\n";
                // 输入avs脚本被更改为无效的avs脚本
                if (jobItem.VideoInfo.Container == "avs")
                    err += "视频编码失败，原因是输入avs脚本有错误。\n\n" + exception.ToString();
                // 对于非avs脚本的媒体文件，可能虽然源文件含视频流，但无对应DShow滤镜而使avs脚本无效；或源文件被更改为非媒体文件
                else if (jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.DirectShowSource || jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.DSS2)
                {
                    err += "视频编码失败。\n";
                    if (jobItem.VideoInfo.Format.Length > 0)
                        err += "\n视频格式：" + jobItem.VideoInfo.Format;
                    if (jobItem.AudioInfo.Format.Length > 0)
                        err += "\n音频格式：" + jobItem.AudioInfo.Format;
                    if (jobItem.VideoInfo.Container.Length > 0)
                        err += "\n容器：" + jobItem.VideoInfo.Container;
                    err += "\n可能是没有为该视频安装正确的DirectShow解码器或分离器。\n可以尝试安装解码包，或ffdshow解码器和Haali分离器等。\n也可以在转换设置中改用FFVideoSource源滤镜，则不需要安装相关的DirectShow解码器。";
                }
                else
                    err += "视频编码失败。请尝试更改视频源滤镜。";
                MessageBox.Show(err, "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            catch (Exception exception)
            {
                MessageBox.Show(jobItem.SourceFile + "\n视频编码失败。" + exception.ToString());
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
        }

        private void EncodingReport(JobItem jobItem, IMediaProcessor encoder, DoWorkEventArgs e)
        {
            while (1 != 0)
            {
                Thread.Sleep(500);
                if (this.backgroundWorker1.CancellationPending)
                {
                    this.StopWorker(encoder, e);
                    break;
                }
                if (encoder.ProcessingDone)
                {
                    this.JobEventReport(jobItem);
                    break;
                }
                this.JobEventReport(jobItem);
            }
        }

        private List<JobItem> GetWorkingJobItems()
        {
            JobItem jobItem;
            List<JobItem> list = new List<JobItem>();
            IEnumerator enumerator = this.jobItemListView.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                jobItem = current.JobItem;
                if (jobItem.State == JobState.NotProccessed)
                {
                    jobItem.State = JobState.Waiting;
                }
                if (jobItem.State == JobState.Stop)
                {
                    if (this._configForm.chbSilentRestart.Checked)
                    {
                        jobItem.State = JobState.Waiting;
                    }
                    else if (MessageBox.Show(current.SubItems[1].Text + "\n该项已经中止。是否重新开始？\n", "项目中止", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        jobItem.State = JobState.Waiting;
                    }
                }
            }
            IEnumerator enumerator2 = this.jobItemListView.Items.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                CxListViewItem item3 = (CxListViewItem) enumerator2.Current;
                jobItem = item3.JobItem;
                if (jobItem.State == JobState.Waiting)
                {
                    list.Add(jobItem);
                }
            }
            return list;
        }

        private void JobEventReport(JobItem jobItem)
        {
            this._workerReporting = true;
            this.backgroundWorker1.ReportProgress(0, jobItem);
            while (this._workerReporting)
            {
                Thread.Sleep(1);
            }
        }

        private void ListView1ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.jobItemListView.DoDragDrop(this.jobItemListView.SelectedItems, DragDropEffects.Move);
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this._workingJobItem != null) && (this._workingJobItem.State == JobState.Working))
            {
                if (MessageBox.Show("正在工作中，是否中止并退出？", "工作中", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    this.StopButtonClick(null, null);
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            System.Collections.Generic.List<JobItem> graph = new System.Collections.Generic.List<JobItem>();
            IEnumerator enumerator = this.jobItemListView.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                JobItem jobItem = current.JobItem;
                graph.Add(jobItem);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream serializationStream = new FileStream("JobItems.bin", FileMode.Create);
            formatter.Serialize(serializationStream, graph);
            serializationStream.Close();
            this.SaveProfileSelection();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            this.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), ProgramConfig.Get().ProfileName);
            System.Collections.Generic.List<JobItem> list = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (MyIO.Exists("JobItems.bin"))
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream("JobItems.bin", FileMode.Open);
                    list = (System.Collections.Generic.List<JobItem>) formatter.Deserialize(stream);
                    stream.Close();
                }
                catch (Exception)
                {
                    stream.Close();
                }
                if (list != null)
                {
                    foreach (JobItem item in list)
                    {
                        this.jobItemListView.Items.Add(item.CxListViewItem);
                    }
                }
            }
        }

        private void Mux(string video, string audio, string dstFile, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem)e.Argument;
            jobItem.Muxer.VideoFile = video;
            jobItem.Muxer.AudioFile = audio;
            jobItem.Muxer.DstFile = dstFile;
            jobItem.Event = JobEvent.Muxing;
            ReportInvoke muxingReport = this.EncodingReport;
            IAsyncResult result = muxingReport.BeginInvoke(jobItem, jobItem.Muxer, e, null, null);
            if (!this.backgroundWorker1.CancellationPending)
            {
                try
                {
                    jobItem.Muxer.Start();
                }
                catch (FormatNotSupportedException)
                {
                    MessageBox.Show("合成MP4失败。可能源媒体流中有不支持的格式。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    jobItem.Event = JobEvent.Error;
                    this.JobEventReport(jobItem);
                }
                catch (FFmpegBugException)
                {
                    MessageBox.Show("合成MP4失败。这是由于FFmpeg的一些Bug, 对某些流无法使用复制。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    jobItem.Event = JobEvent.Error;
                    this.JobEventReport(jobItem);
                }
            }
            muxingReport.EndInvoke(result);
            if ((jobItem.JobConfig.AudioMode == StreamProcessMode.Encode) && !MyIO.IsSameFile(audio, dstFile))
            {
                File.Delete(audio);
            }
        }

        private void NextJobOrExist(object sender, RunWorkerCompletedEventArgs e)
        {
            this._workingJobItem = null;
            if (e.Result != null)
            {
                JobItem result = (JobItem) e.Result;
                if (result.Event == JobEvent.AllDone || result.Event == JobEvent.QuitAllProcessing)
                {
                    result.Event = JobEvent.None;
                    this._workingJobItems.Clear();
                }
                else if (this._workingJobItems[this._workingJobItems.Count - 1] != result)
                {
                    int num = this._workingJobItems.IndexOf(result) + 1;
                    JobItem argument = this._workingJobItems[num];
                    this._workingJobItem = argument;
                    this.backgroundWorker1.RunWorkerAsync(argument);
                }
            }
        }

        private void OpenFileDialog1FileOk(object sender, CancelEventArgs e)
        {
            ListViewItem item = null;
            int index = 0;
            string[] fileNames = this.openFileDialog1.FileNames;
            int length = fileNames.Length;
            while (index < length)
            {
                ListViewItem item2 = this.AddNewJobItem(fileNames[index], true);
                if ((item == null) && (item2 != null))
                {
                    item = item2;
                }
                index++;
            }
            if (item != null)
            {
                this.jobItemListView.SelectedItems.Clear();
                item.Selected = true;
            }
        }

        private void ProcessAudio(JobItem jobItem, DoWorkEventArgs e)
        {
            string audio = "";
            if (jobItem.UsingExternalAudio && File.Exists(jobItem.ExternalAudio))
            {
                audio = jobItem.ExternalAudio;
            }
            else
            {
                audio = jobItem.SourceFile;
            }
            new AudioAvsWriter(audio, jobItem.AvsConfig, jobItem.AudioInfo).WriteScript("audio.avs");
            string destAudio = string.Empty;
            if (jobItem.JobConfig.VideoMode != StreamProcessMode.None)
            {
                destAudio = Path.ChangeExtension(jobItem.DestFile, "m4a");
                destAudio = MyIO.GetUniqueName(destAudio);
                jobItem.FilesToDeleteWhenProcessingFails.Add(destAudio);
            }
            else
            {
                destAudio = jobItem.DestFile;
            }
            try
            {
                this.EncodeAudio("audio.avs", destAudio, jobItem.AudioEncConfig, e);
                jobItem.EncodedAudio = destAudio;
            }
            finally
            {
                if (jobItem.AvsConfig.AudioSourceFilter == AudioSourceFilter.FFAudioSource)
                {
                    File.Delete(audio + ".ffindex");
                }
            }
        }

        private void ProcessVideo(JobItem jobItem, DoWorkEventArgs e)
        {
            SubStyleWriter writer = null;
            new VideoAvsWriter(jobItem.SourceFile, jobItem.AvsConfig, jobItem.SubtitleFile, jobItem.VideoInfo).WriteScript("video.avs");
            bool usingSubtitleStyle = false;
            if (MyIO.Exists(jobItem.SubtitleFile) && jobItem.SubtitleConfig.UsingStyle)
            {
                writer = new SubStyleWriter(jobItem.SubtitleFile, jobItem.SubtitleConfig);
                writer.Write();
                usingSubtitleStyle = true;
            }
            jobItem.FilesToDeleteWhenProcessingFails.Add(jobItem.DestFile);
            try
            {
                this.EncodeVideo("video.avs", jobItem.DestFile, jobItem.VideoEncConfig, e);
                jobItem.EncodedVideo = jobItem.DestFile;
            }
            finally
            {
                if (usingSubtitleStyle)
                    writer.DeleteTempFiles();
                if (jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.FFVideoSource)
                    File.Delete(jobItem.SourceFile + ".ffindex");
            }
        }

        private void ProfileBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否应用更改到所有项目？", "预设更改", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Profile profile = new Profile(this.profileBox.Text);
                string extension = string.Empty;
                if (profile != null)
                {
                    extension = profile.GetExtByContainer();
                }
                IEnumerator enumerator = this.jobItemListView.Items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    CxListViewItem current = (CxListViewItem) enumerator.Current;
                    JobItem jobItem = current.JobItem;
                    jobItem.ProfileName = this.profileBox.Text;
                    if (extension != string.Empty)
                    {
                        jobItem.DestFile = Path.ChangeExtension(jobItem.DestFile, extension);
                        jobItem.DestFile = MyIO.GetUniqueName(jobItem.DestFile);
                    }
                }
            }
        }

        private void ResetProgress()
        {
            this.videoProgressBar.Value = 0;
            this.audioProgressBar.Value = 0;
            this.muxProgressBar.Value = 0;
            this.videoTimeLeftLable.Text = string.Empty;
            this.videoTimeUsedLable.Text = string.Empty;
            this.videoAvgBitRateLable.Text = string.Empty;
            this.videoEncodingFpsLable.Text = string.Empty;
            this.videoEstimatedFileSizeLable.Text = string.Empty;
            this.audioTimeLeft.Text = string.Empty;
            this.audioTimeUsed.Text = string.Empty;
            this.muxTimeLeft.Text = string.Empty;
            this.muxTimeUsed.Text = string.Empty;
        }

        private void SaveProfileSelection()
        {
            ProgramConfig.Get().ProfileName = this.profileBox.Text;
            ProgramConfig.Save();
        }

        private void SettingButtonClick(object sender, EventArgs e)
        {
            CxListViewItem item = (CxListViewItem) this.jobItemListView.SelectedItems[0];
            JobItem jobItem = item.JobItem;
            JobItem[] items = new JobItem[] { jobItem };
            this.SetUpJobItems(items);
            if (this._jobSettingForm == null)
            {
                this._jobSettingForm = new JobSettingForm();
            }
            this._jobSettingForm.UpdateProfiles(ArrayBuilder.Build<string>(this.profileBox.Items), jobItem.ProfileName);
            this._jobSettingForm.SetUpFormForItem(jobItem);
            DialogResult result = this._jobSettingForm.ShowDialog();
            this.UpdateProfileBox(this._jobSettingForm.GetProfiles(), this.profileBox.Text);
            this._jobSettingForm.Clear();
        }

        private void SetUpJobItems(JobItem[] items)
        {
            int index = 0;
            JobItem[] itemArray = items;
            int length = itemArray.Length;
            while (index < length)
            {
                try
                {
                    itemArray[index].SetUp();
                }
                catch (ProfileNotFoundException)
                {
                    this.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), this.profileBox.Text);
                    itemArray[index].ProfileName = this.profileBox.Text;
                    itemArray[index].SetUp();
                }
                index++;
            }
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            this._workingJobItems = this.GetWorkingJobItems();
            string str = string.Empty;
            foreach (JobItem item in this._workingJobItems)
            {
                if (!MyIO.Exists(item.SourceFile))
                {
                    str += new StringBuilder("\n").Append(item.SourceFile).ToString();
                    item.State = JobState.Error;
                    this._workingJobItems.Remove(item);
                }
            }
            if ((str != string.Empty) && (MessageBox.Show(new StringBuilder("以下媒体文件不存在：").Append(str).Append("\n单击“确定”将处理其他文件。").ToString(), "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel))
            {
                this._workingJobItems.Clear();
            }
            else if (this._workingJobItems.Count > 0)
            {
                this.SetUpJobItems(this._workingJobItems.ToArray());
                this._workingJobItem = this._workingJobItems[0];
                this.backgroundWorker1.RunWorkerAsync(this._workingJobItem);
                this.tabControl1.SelectTab(this.progressPage);
            }
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            try
            {
                this.backgroundWorker1.CancelAsync();
            }
            catch (Exception)
            {
            }
        }

        private void StopWorker(IMediaProcessor encoder, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem) e.Argument;
            encoder.Stop();
            jobItem.Event = JobEvent.QuitAllProcessing;
            this.JobEventReport(jobItem);
        }

        private void UpdateProfileBox(string[] newProfileNames, string selectedProfile)
        {
            this.profileBox.SelectedIndexChanged -= new EventHandler(this.ProfileBoxSelectedIndexChanged);
            this.profileBox.Items.Clear();
            this.profileBox.Items.AddRange(newProfileNames);
            if (!this.profileBox.Items.Contains("Default"))
            {
                Profile.RebuildDefault("Default");
                this.profileBox.Items.Add("Default");
            }
            if (this.profileBox.Items.Contains(selectedProfile))
            {
                this.profileBox.SelectedItem = selectedProfile;
            }
            else
            {
                this.profileBox.SelectedItem = "Default";
            }
            this.profileBox.SelectedIndexChanged += new EventHandler(this.ProfileBoxSelectedIndexChanged);
        }

        private void 打开目录ToolStripMenuItemClick(object sender, EventArgs e)
        {
            JobItem jobItem = (this.jobItemListView.SelectedItems[0] as CxListViewItem).JobItem;
            Process.Start("explorer.exe", "/select, " + jobItem.SourceFile);
        }

        private void 未处理ToolStripMenuItemClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                current.JobItem.State = JobState.NotProccessed;
            }
        }

        private void 选项ToolStripMenuItemClick(object sender, EventArgs e)
        {
            this._configForm.ShowDialog();
        }

        private void jobItemListView_DoubleClick(object sender, EventArgs e)
        {
            this.settingButton.PerformClick();
        }

        private void jobItemListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                this.jobItemListView.SelectedItems.Clear();
                foreach (string path in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (File.Exists(path))
                    {
                        ListViewItem newListViewItem = AddNewJobItem(path, true);
                        if (newListViewItem != null)
                            newListViewItem.Selected = true;
                    }
                    else if (Directory.Exists(path))
                    {
                        foreach (string file in Directory.GetFiles(path))
                        {
                            ListViewItem newListViewItem = AddNewJobItem(path, false);
                            if (newListViewItem != null)
                                newListViewItem.Selected = true;
                        }
                    }
                }
                return;
            }
            Point p = this.jobItemListView.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = this.jobItemListView.GetItemAt(p.X, p.Y);
            if (dragToItem == null)
                return;
            int dragToIndex = dragToItem.Index;
            foreach (ListViewItem item in this.jobItemListView.SelectedItems)
                this.jobItemListView.Items.Remove(item);
            if (this.jobItemListView.Items.Count < dragToIndex)
                dragToIndex = this.jobItemListView.Items.Count;
            foreach (ListViewItem item in this.jobItemListView.SelectedItems)
            {
                this.jobItemListView.Items.Insert(dragToIndex, item);
                dragToIndex++;
            }
            if (this._workingJobItems.Count > 1)
            {
                List<JobItem> newWorkingItems = new List<JobItem>(this._workingJobItems.Count);
                foreach (JobItem jobItem in this._workingJobItems)
                    newWorkingItems[jobItem.CxListViewItem.Index] = jobItem;
                this._workingJobItems = newWorkingItems;
            }
        }

        private void ListView1DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) || e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private ListViewItem AddNewJobItem(string filePath, bool checkMedia)
        {
            string fileName = filePath;
            if (!this._configForm.chbInputDir.Checked)
            {
                fileName = Path.GetFileName(fileName);
            }
            if (checkMedia&&Path.GetExtension(fileName).ToLower()!=".avs")
            {
                int errorCode = ((IMediaDet)new MediaDet()).put_Filename(filePath);
                try
                {
                    Marshal.ThrowExceptionForHR(errorCode);
                }
                catch (Exception)
                {
                    if (!(MessageBox.Show((filePath + "\n在文件中找不到视频流或音频流，可能是没有安装对应的DirectShow滤镜。\n") + "仍然要添加该文件吗？", "检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK))
                    {
                        return null;
                    }
                }
            }
            JobItem jobItem = null;
            Profile profile = new Profile(this.profileBox.Text);
            string ext = ".mp4";
            if (profile != null)
            {
                ext = profile.GetExtByContainer();
            }
            string destFile = string.Empty;
            if (this._configForm.destDirComboBox.Text == string.Empty)
                destFile = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ext);
            else
                destFile = Path.Combine(this._configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(filePath) + ext);
            destFile = MyIO.GetUniqueName(destFile);
            jobItem = new JobItem(filePath, destFile, this.profileBox.Text);
            if (Path.GetExtension(filePath).ToLower()==".avs"&&jobItem.VideoInfo.Container!="avs")
                if (MessageBox.Show((filePath + "\n错误的avs脚本，或没有安装Avisynth。\n") + "仍然要添加该文件吗？", "检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return null;
                }
            this.jobItemListView.Items.Add(jobItem.CxListViewItem);
            return jobItem.CxListViewItem;
        }

        private void JobItemListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.jobItemListView.SelectedItems.Count == 1)
            {
                this.settingButton.Enabled = true;
            }
            else
            {
                this.settingButton.Enabled = false;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._aboutForm == null)
                this._aboutForm = new AboutForm();
            this._aboutForm.ShowDialog();
        }
    }
}


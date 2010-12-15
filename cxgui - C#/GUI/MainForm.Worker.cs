namespace Cxgui.Gui
{
    using Clinky;
    using Cxgui;
    using Cxgui.AudioEncoding;
    using Cxgui.Avisynth;
    using Cxgui.Config;
    using Cxgui.Job;
    using Cxgui.VideoEncoding;
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

    partial class MainForm
    {
        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
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
                    if (this.workingJobItems[this.workingJobItems.Count - 1] == jobItem)
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
                    if (this.workingJobItems[this.workingJobItems.Count - 1] == jobItem)
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
            catch (Exception exception)
            {
                MessageBox.Show("发生了一个错误。\n" + exception.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            finally
            {
                if (this.workingJobItems[this.workingJobItems.Count - 1] == jobItem)
                {
                    jobItem.Event = JobEvent.AllDone;
                    this.JobEventReport(jobItem);
                }
                e.Result = jobItem;
            }
        }

        private void BackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
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
                        index = this.workingJobItems.IndexOf(jobItem);
                        this.statusLable.Text = new StringBuilder("正在处理第").Append(index + 1).Append("个文件，共").Append(this.workingJobItems.Count).Append("个文件").ToString();
                    }
                    else if (jobItem.Event == JobEvent.OneJobItemDone)
                    {
                        jobItem.FilesToDeleteWhenProcessingFails.Clear();
                        jobItem.State = JobState.Done;
                        index = this.workingJobItems.IndexOf(jobItem);
                        this.statusLable.Text = new StringBuilder("第").Append(index + 1).Append("个文件处理完毕，共").Append(this.workingJobItems.Count).Append("个文件").ToString();
                    }
                    else if (jobItem.Event == JobEvent.AllDone)
                    {
                        this.statusLable.Text = new StringBuilder().Append(this.workingJobItems.Count).Append("个文件处理完成").ToString();
                        this.startButton.Enabled = true;
                    }
                    else if (jobItem.Event == JobEvent.QuitAllProcessing)
                    {
                        this.ResetProgress();
                        jobItem.State = JobState.Stop;
                        this.startButton.Enabled = true;
                        this.statusLable.Text = "中止";
                        this.mainTabControl.SelectTab(this.inputPage);
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
                        this.backgroundWorker.CancelAsync();
                        foreach (string file in jobItem.FilesToDeleteWhenProcessingFails)
                        {
                            File.Delete(file);
                        }
                        jobItem.FilesToDeleteWhenProcessingFails.Clear();
                    }
                }
            }
            finally
            {
                this.workerReporting = false;
            }
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
            JobItem jobItem = (JobItem)e.Argument;
            ReportInvoke encodingReport = this.EncodingReport;
            IAsyncResult result = null;
            try
            {
                NeroAacHandler encoder = new NeroAacHandler(avsFile, destFile);
                encoder.Config = config as NeroAacConfig;
                jobItem.AudioEncoder = encoder;
                jobItem.Event = JobEvent.AudioEncoding;
                result = encodingReport.BeginInvoke(jobItem, encoder, e, null, null);
                if (!this.backgroundWorker.CancellationPending)
                {
                    encoder.Start();
                }
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
            finally
            {
                if (result != null)
                    encodingReport.EndInvoke(result);
                jobItem.AudioEncoder = null;
            }
        }

        private void EncodeVideo(string avsFile, string destFile, VideoEncConfigBase config, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem)e.Argument;
            IAsyncResult result = null;
            ReportInvoke encodingReport = this.EncodingReport;
            try
            {
                x264Handler encoder = new x264Handler(avsFile, destFile);
                encoder.Config = config as x264Config;
                jobItem.VideoEncoder = encoder;
                jobItem.Event = JobEvent.VideoEncoding;
                result = encodingReport.BeginInvoke(jobItem, encoder, e, null, null);
                if (!this.backgroundWorker.CancellationPending)
                    encoder.Start();
            }
            catch (BadEncoderCmdException)
            {
                MessageBox.Show("视频编码失败。是否使用了不正确的命令行？", "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.backgroundWorker.CancelAsync();
                jobItem.Event = JobEvent.Error;
                this.JobEventReport(jobItem);
            }
            // 有效脚本，但不含视频。注意仅当源文件原本含视频流或输入avs脚本含视频时当前函数可能被调用。
            catch (AvisynthVideoStreamNotFoundException)
            {
                string err = jobItem.SourceFile + "\n";
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
                string err = jobItem.SourceFile + "\n";
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
            finally
            {
                if (result != null)
                    encodingReport.EndInvoke(result);
                jobItem.VideoEncoder = null;
            }
        }

        private void EncodingReport(JobItem jobItem, IMediaProcessor processor, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(500);
                if (this.backgroundWorker.CancellationPending)
                {
                    this.StopWorker(processor, e);
                    break;
                }
                if (processor.ProcessingDone)
                {
                    if (processor.Progress != 100)
                    {
                        jobItem.Event = JobEvent.Error;
                        this.JobEventReport(jobItem);
                        MessageBox.Show("发生了一个错误。编码器/混流器未完成工作就退出了。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
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
                CxListViewItem current = (CxListViewItem)enumerator.Current;
                jobItem = current.JobItem;
                if (jobItem.State == JobState.NotProccessed)
                {
                    jobItem.State = JobState.Waiting;
                }
                if (jobItem.State == JobState.Stop)
                {
                    if (this.configForm.chbSilentRestart.Checked)
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
                CxListViewItem item3 = (CxListViewItem)enumerator2.Current;
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
            this.workerReporting = true;
            this.backgroundWorker.ReportProgress(0, jobItem);
            while (this.workerReporting)
            {
                Thread.Sleep(1);
            }
        }

        private void SaveJobItemsAndProfiles()
        {
            System.Collections.Generic.List<JobItem> jobItems = new System.Collections.Generic.List<JobItem>();
            foreach (CxListViewItem item in this.jobItemListView.Items)
            {
                JobItem jobItem = item.JobItem;
                jobItems.Add(jobItem);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream serializationStream = new FileStream("JobItems.bin", FileMode.Create);
            formatter.Serialize(serializationStream, jobItems);
            serializationStream.Close();
            this.SaveProfileSelection();
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
            if (!this.backgroundWorker.CancellationPending)
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

        private void NextJobOrExit(object sender, RunWorkerCompletedEventArgs e)
        {
            this.workingJobItem = null;
            if (e.Result != null)
            {
                JobItem result = (JobItem)e.Result;
                if (result.Event == JobEvent.AllDone || result.Event == JobEvent.QuitAllProcessing)
                {
                    result.Event = JobEvent.None;
                    foreach (JobItem jobItem in this.workingJobItems)
                        if (jobItem.State == JobState.Waiting)
                            jobItem.State = JobState.NotProccessed;
                    this.workingJobItems.Clear();
                    if (this.formClosing)
                    {
                        this.SaveJobItemsAndProfiles();
                        this.Close();
                    }

                }
                else if (this.workingJobItems[this.workingJobItems.Count - 1] != result)
                {
                    int newIndex = this.workingJobItems.IndexOf(result) + 1;
                    this.workingJobItem = this.workingJobItems[newIndex];
                    this.backgroundWorker.RunWorkerAsync(this.workingJobItem);
                }
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

        private void SetUpJobItems(JobItem[] items)
        {
            int index = 0;
            JobItem[] itemArray = items;
            int length = itemArray.Length;
            while (index < length)
            {
                try
                {
                    itemArray[index].SetUp(false);
                }
                catch (ProfileNotFoundException)
                {
                    this.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), this.profileBox.Text);
                    itemArray[index].ProfileName = this.profileBox.Text;
                    itemArray[index].SetUp(false);
                }
                index++;
            }
        }

        private void StopWorker(IMediaProcessor encoder, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem)e.Argument;
            encoder.Stop();
            jobItem.Event = JobEvent.QuitAllProcessing;
            this.JobEventReport(jobItem);
        }

    }
}

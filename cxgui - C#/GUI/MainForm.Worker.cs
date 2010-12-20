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
        protected bool workerReporting;
        protected JobItem workingJobItem;
        protected List<JobItem> workingJobItems;
        private delegate bool ReportInvoke(JobItem jobItem, IMediaProcessingInfo encoder, DoWorkEventArgs e);
        private bool formClosing;

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            JobItem jobItem = (JobItem)e.Argument;
            try
            {
                // 预先的检查与错误处理
                if (jobItem.JobConfig.VideoMode == StreamProcessMode.None && jobItem.JobConfig.AudioMode == StreamProcessMode.None)
                {
                    this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                    MessageBox.Show(jobItem.SourceFile + "\n源文件不是视频文件，或设置有错误。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MyIO.Exists(jobItem.DestFile) && (MessageBox.Show(jobItem.DestFile + "\n目标文件已存在。决定覆盖吗？", "文件已存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel))
                {
                    this.SetJobEventAndReportProgress(jobItem, JobEvent.OneJobItemCancelled);
                    return;
                }
                // 工作过程
                this.SetJobEventAndReportProgress(jobItem, JobEvent.OneJobItemProcessing);
                if (jobItem.JobConfig.VideoMode == StreamProcessMode.Encode)
                {
                    if (this.EncodeVideo(jobItem, e) == false)
                        return;
                }
                if (jobItem.JobConfig.AudioMode == StreamProcessMode.Encode)
                {
                    if (this.EncodeAudio(jobItem, e) == false)
                        return;
                }
                if (jobItem.MuxingInfo != null)
                {
                    if (this.Mux(jobItem, e) == false)
                        return;
                }
                if (jobItem.State != JobState.Error)
                    this.SetJobEventAndReportProgress(jobItem, JobEvent.OneJobItemDone);
            }
            catch (Exception exception)
            {
                MessageBox.Show("发生了一个错误。\n" + exception.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
            }
            finally
            {
                jobItem.ClearTempFiles();
                if (this.workingJobItems[this.workingJobItems.Count - 1] == jobItem)
                    this.SetJobEventAndReportProgress(jobItem, JobEvent.AllDone);
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
                    if (jobItem.VideoEncInfo.ProcessingFrameRate != 0)
                    {
                        this.videoProgressBar.Value = jobItem.VideoEncInfo.Progress;
                        this.videoTimeUsedLable.Text = jobItem.VideoEncInfo.TimeUsed.ToString();
                        this.videoTimeLeftLable.Text = jobItem.VideoEncInfo.TimeLeft.ToString();
                        this.videoEncodingFpsLable.Text = jobItem.VideoEncInfo.ProcessingFrameRate.ToString(".00' fps'");
                        this.videoEstimatedFileSizeLable.Text = ((double)jobItem.VideoEncInfo.EstimatedFileSize / 1024).ToString(".00' MB'");
                        this.videoAvgBitRateLable.Text = jobItem.VideoEncInfo.AvgBitRate.ToString(".00' kbps");
                    }
                }
                else if (jobItem.Event == JobEvent.AudioEncoding)
                {
                    this.audioProgressBar.Value = jobItem.AudioEncInfo.Progress;
                    this.audioTimeUsed.Text = jobItem.AudioEncInfo.TimeUsed.ToString();
                    this.audioTimeLeft.Text = jobItem.AudioEncInfo.TimeLeft.ToString();

                }
                else if (jobItem.Event == JobEvent.Muxing)
                {
                    this.muxProgressBar.Value = jobItem.MuxingInfo.Progress;
                    this.muxTimeUsed.Text = jobItem.MuxingInfo.TimeUsed.ToString();
                    this.muxTimeLeft.Text = jobItem.MuxingInfo.TimeLeft.ToString();
                }
                else
                {
                    if (jobItem.Event == JobEvent.OneJobItemProcessing)
                    {
                        jobItem.State = JobState.Working;
                        this.ResetProgress();
                        this.startButton.Enabled = false;
                        int index = this.workingJobItems.IndexOf(jobItem);
                        this.statusLable.Text = "正在处理第" + (index + 1).ToString() + "个项目，共" + this.workingJobItems.Count.ToString() + "个项目";
                    }
                    else if (jobItem.Event == JobEvent.OneJobItemDone)
                    {
                        jobItem.State = JobState.Done;
                        jobItem.FilesToDeleteWhenProcessingFails.Clear();
                        int index = this.workingJobItems.IndexOf(jobItem);
                        this.statusLable.Text = "第" + (index + 1).ToString() + "个项目处理完毕，共" + this.workingJobItems.Count.ToString() + "个项目";
                    }
                    else if (jobItem.Event == JobEvent.AllDone)
                    {
                        this.statusLable.Text = this.workingJobItems.Count.ToString() + "个文件处理完成";
                        this.startButton.Enabled = true;
                    }
                    else if (jobItem.Event == JobEvent.QuitAllProcessing)
                    {
                        jobItem.State = JobState.Stop;
                        this.ResetProgress();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avsFile"></param>
        /// <param name="destFile"></param>
        /// <param name="config"></param>
        /// <param name="e"></param>
        /// <returns>如果编码顺利完成，返回true；如果被用户中止或在过程中出错，返回false。</returns>
        private bool EncodeVideo(JobItem jobItem, DoWorkEventArgs e)
        {
            try
            {
                jobItem.Event = JobEvent.VideoEncoding;
                jobItem.ProcessVideo();
                return this.ProcessingReport(jobItem, jobItem.VideoEncInfo, e);
            }
            catch (BadEncoderCmdException)
            {
                MessageBox.Show("视频编码失败。是否使用了不正确的命令行？", "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
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
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
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
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(jobItem.SourceFile + "\n视频编码失败。" + exception.ToString());
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avsFile"></param>
        /// <param name="destFile"></param>
        /// <param name="config"></param>
        /// <param name="e"></param>
        /// <returns>如果编码顺利完成，返回true；如果被用户中止或在过程中出错，返回false。</returns>
        private bool EncodeAudio(JobItem jobItem, DoWorkEventArgs e)
        {
            try
            {
                jobItem.Event = JobEvent.AudioEncoding;
                jobItem.ProcessAudio();
                return this.ProcessingReport(jobItem, jobItem.AudioEncInfo, e);
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
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
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
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
            catch (Exception exception)
            {
                string err;
                if (jobItem.UsingExternalAudio && jobItem.ExternalAudio.Length > 0)
                    err = jobItem.ExternalAudio;
                else
                    err = jobItem.SourceFile;
                MessageBox.Show(err + "\n音频编码失败。" + exception.ToString());
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="video"></param>
        /// <param name="audio"></param>
        /// <param name="dstFile"></param>
        /// <param name="e"></param>
        /// <returns>如果混流顺利完成，返回true；如果被用户中止或在过程中出错，返回false。</returns>
        private bool Mux(JobItem jobItem, DoWorkEventArgs e)
        {
            try
            {
                jobItem.Event = JobEvent.Muxing;
                jobItem.ProcessMuxing();
                return this.ProcessingReport(jobItem, jobItem.MuxingInfo, e);
            }
            catch (FormatNotSupportedException)
            {
                MessageBox.Show("合成MP4失败。可能源媒体流中有不支持的格式。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
            catch (FFmpegBugException)
            {
                MessageBox.Show("合成MP4失败。这是由于FFmpeg的一些Bug, 对某些流无法使用复制。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                return false;
            }
            finally
            {
                if ((jobItem.JobConfig.AudioMode == StreamProcessMode.Encode) && !MyIO.IsSameFile(jobItem.MuxingInfo.AudioFile, jobItem.DestFile))
                {
                    File.Delete(jobItem.MuxingInfo.AudioFile);
                }
            }
        }

        /// <summary>
        /// 报告processor的处理进度。
        /// </summary>
        /// <param name="jobItem"></param>
        /// <param name="processingInfo"></param>
        /// <param name="e"></param>
        /// <returns>如果处理顺利完成，返回true；如果被用户中止或中途出错，返回false。</returns>
        private bool ProcessingReport(JobItem jobItem, IMediaProcessingInfo processingInfo, DoWorkEventArgs e)
        {
            while (true)
            {
                if (this.backgroundWorker.CancellationPending)
                {
                    jobItem.QuitProcessing();
                    this.SetJobEventAndReportProgress(jobItem, JobEvent.QuitAllProcessing);
                    return false;
                }
                if (processingInfo.HasExited)
                {
                    if (processingInfo.Progress != 100)
                    {
                        this.SetJobEventAndReportProgress(jobItem, JobEvent.Error);
                        MessageBox.Show("发生了一个错误。编码器/混流器未完成工作就退出了。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    this.SynchReportProgress(jobItem);
                    return true;
                }
                this.SynchReportProgress(jobItem);
                Thread.Sleep(500);
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
        
        /// <summary>
        /// 向GUI线程报告进度，在GUI线程处理完成前阻塞当前线程。
        /// </summary>
        /// <param name="jobItem"></param>
        private void SynchReportProgress(JobItem jobItem)
        {
            this.workerReporting = true;
            this.backgroundWorker.ReportProgress(0, jobItem);
            while (this.workerReporting)
            {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 向GUI线程报告进度，在GUI线程处理完成前阻塞当前线程。
        /// </summary>
        /// <param name="jobItem"></param>
        /// <param name="jobEvent">赋与jobItem的Event属性，以新的工作事件。</param>
        private void SetJobEventAndReportProgress(JobItem jobItem, JobEvent jobEvent)
        {
            jobItem.Event = jobEvent;
            this.SynchReportProgress(jobItem);
        }

        private void SaveJobItemsAndProfiles()
        {
            List<JobItem> jobItems = new List<JobItem>();
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

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.workingJobItem = null;
            if (e.Result != null)
            {
                JobItem previousJobItem = (JobItem)e.Result;
                // 对于全部处理完成的情形，可能有条目的移动使部分条目为“等待”的状态。
                if (previousJobItem.Event == JobEvent.AllDone || previousJobItem.Event == JobEvent.QuitAllProcessing)
                {
                    previousJobItem.Event = JobEvent.None;
                    foreach (JobItem jobItem in this.workingJobItems)
                        if (jobItem.State == JobState.Waiting)
                            jobItem.State = JobState.NotProccessed;
                    this.workingJobItems.Clear();
                    // 当关闭程序时，如果worker正在运行，设此旗标为true并取消worker的运行，在此退出程序
                    if (this.formClosing)
                    {
                        this.SaveJobItemsAndProfiles();
                        this.Close();
                    }
                }
                else
                {
                    this.workingJobItem = this.workingJobItems[this.workingJobItems.IndexOf(previousJobItem) + 1];
                    this.backgroundWorker.RunWorkerAsync(this.workingJobItem);
                }
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
    }
}

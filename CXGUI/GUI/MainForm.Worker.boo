namespace CXGUI.GUI

import System
import System.IO
import System.Windows.Forms
import System.ComponentModel
import CXGUI.Job

partial class MainForm(System.Windows.Forms.Form):

	private def SetUpJobItems(items as (JobItem)):
	"""
	仅当JobItem的某设置属性为null时，根据ProfileName属性为其读取相应设置实例。
	如对应Profile文件不存在，则刷新预设列表，将Default Profile应用到JobItem。
	"""
		for item in items:
			try:
				item.SetUp()
			except as ProfileNotFoundException:
				self.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), self.profileBox.Text)
				item.ProfileName = self.profileBox.Text
				item.SetUp()

	private def GetWorkingJobItems() as Boo.Lang.List[of JobItem]:
		jobItems = Boo.Lang.List[of JobItem]()
		
		for listItem as CxListViewItem in self.jobItemListView.Items:
			jobItem = listItem.JobItem
			if jobItem.State == JobState.Stop:
				if _configForm.chbSilentRestart.Checked:
					jobItem.State = JobState.Waiting
				else:
					result = MessageBox.Show(listItem.SubItems[1].Text + "\n该项已经中止。是否重新开始？\n", 
					"项目中止", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
					if result == DialogResult.OK:
						jobItem.State = JobState.Waiting
						
		for listItem as CxListViewItem in self.jobItemListView.Items:
			jobItem = listItem.JobItem
			if jobItem.State == JobState.Waiting:
				jobItems.Add(jobItem)
		return jobItems

	private def BackgroundWorker1DoWork(sender as object, e as DoWorkEventArgs):
	"""一个worker过程处理一个JobItem的全部任务。"""
		try:
			jobItem as JobItem = e.Argument
			if File.Exists(jobItem.DestFile):
				r = MessageBox.Show("${jobItem.DestFile}\n目标文件已存在。决定覆盖吗？", "文件已存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
				if r == DialogResult.Cancel:
					jobItem.Event = JobEvent.OneJobItemCancelled
					JobEventReport(jobItem)
					if self._workingJobItems[-1] is jobItem:
						jobItem.Event = JobEvent.AllDone
						JobEventReport(jobItem)
					e.Result = jobItem
					return
		
			jobItem.Event = JobEvent.OneJobItemProcessing
			JobEventReport(jobItem)
	
			if jobItem.JobConfig.VideoMode == StreamProcessMode.Encode:
				ProcessVideo(jobItem, e)
				return if DidUserPressStopButton(jobItem, e)
	
			if jobItem.JobConfig.AudioMode == StreamProcessMode.Encode:
				ProcessAudio(jobItem, e)
				return if DidUserPressStopButton(jobItem, e)
	
			if jobItem.JobConfig.Muxer != Muxer.None:
				DoMuxStuff(jobItem, e)
				return if DidUserPressStopButton(jobItem, e)

			if jobItem.State != JobState.Error:
				jobItem.Event = JobEvent.OneJobItemDone
				JobEventReport(jobItem)

		except e as Exception:
			MessageBox.Show("发生了一个错误。\n"+e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
			jobItem.Event = JobEvent.Error
			JobEventReport(jobItem)
		ensure:
			
			if self._workingJobItems[-1] is jobItem:
				jobItem.Event = JobEvent.AllDone
				JobEventReport(jobItem)
			
			e.Result = jobItem

	private def DidUserPressStopButton(jobItem as JobItem, e as DoWorkEventArgs):
		//User stoped
		if self.backgroundWorker1.CancellationPending:
			jobItem.Event = JobEvent.QuitAllProcessing
			JobEventReport(jobItem)
			return true
		else:
			return false
				
	private def ProcessVideo(jobItem as JobItem, e as DoWorkEventArgs):
		AvisynthWriter.WriteVideoAvs(jobItem.SourceFile, 'video.avs', jobItem.Subtitle, jobItem.AvsConfig)
		usingSubStyleWriter = false
		if File.Exists(jobItem.Subtitle) and jobItem.SubtitleConfig.UsingStyle:
			substyleWriter = SubStyleWriter(jobItem.Subtitle, jobItem.SubtitleConfig)
			substyleWriter.Write()
			usingSubStyleWriter = true
				
		jobItem.FilesToDeleteWhenProcessingFails.Add(jobItem.DestFile)
		try:
			self.EncodeVideo('video.avs', jobItem.DestFile, jobItem.VideoEncConfig, e)
			jobItem.EncodedVideo = jobItem.DestFile
		except e as Exception:
			if usingSubStyleWriter:
				substyleWriter.CleanUp()
			raise e
		if usingSubStyleWriter:
			substyleWriter.CleanUp()
		if jobItem.AvsConfig.VideoSource == VideoSourceFilter.FFVideoSource:
			File.Delete(jobItem.SourceFile+'.ffindex')
			
	private def ProcessAudio(jobItem as JobItem, e as DoWorkEventArgs):
		if jobItem.JobConfig.UseSeparateAudio and File.Exists(jobItem.ExternalAudio):
			sourceAudio = jobItem.ExternalAudio
		else:
			sourceAudio = jobItem.SourceFile
		AvisynthWriter.WriteAudioAvs(sourceAudio, 'audio.avs', jobItem.AvsConfig)
		if jobItem.JobConfig.VideoMode != StreamProcessMode.None:
			destAudio = Path.ChangeExtension(jobItem.DestFile, 'm4a')
			destAudio = GetUniqueName(destAudio)
		else:
			destAudio = jobItem.DestFile

		jobItem.FilesToDeleteWhenProcessingFails.Add(destAudio)
		try:
			self.EncodeAudio('audio.avs', destAudio, jobItem.AudioEncConfig, e)
			jobItem.EncodedAudio = destAudio
		except as InvalidAudioAvisynthScriptException:
			ChangeSourceAndRetry = do:
				source = jobItem.AvsConfig.AudioSource
				source = source + 1 if source == 0
				source = source - 1 if source == 1 
				AvisynthWriter.WriteAudioAvs(jobItem.SourceFile, 'audio.avs', jobItem.AvsConfig)
				try:
					self.EncodeAudio('audio.avs', destAudio, jobItem.AudioEncConfig, e)
					jobItem.EncodedAudio = destAudio
				except as InvalidAudioAvisynthScriptException:
					MessageBox.Show(jobItem.SourceFile + "\n音频脚本无法读取。", 
			"检测失败", MessageBoxButtons.OK, MessageBoxIcon.Error)
			if self._configForm.cbAudioAutoSF.Checked:
				ChangeSourceAndRetry()
			else:
				result = MessageBox.Show(jobItem.SourceFile + "\n该文件的音频脚本无法读取。是否尝试更改源滤镜？", 
				"检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
				jobItem.JobConfig.AudioMode = StreamProcessMode.None
				if result == DialogResult.OK:
					ChangeSourceAndRetry()
				else:
					jobItem.Event = JobEvent.Error
					JobEventReport(jobItem)
		if jobItem.AvsConfig.AudioSource == AudioSourceFilter.FFAudioSource:
			File.Delete(sourceAudio + '.ffindex')
			
	private def DoMuxStuff(jobItem as JobItem, e as DoWorkEventArgs):
		if jobItem.JobConfig.AudioMode == StreamProcessMode.Encode:
			muxAudio = jobItem.EncodedAudio
			jobItem.EncodedAudio = ""
		elif jobItem.JobConfig.AudioMode == StreamProcessMode.Copy:
			if jobItem.JobConfig.UseSeparateAudio and jobItem.ExternalAudio != "":
				muxAudio = jobItem.ExternalAudio
			else:
				muxAudio = jobItem.SourceFile
		else:
			muxAudio = ""
				
		if jobItem.JobConfig.VideoMode == StreamProcessMode.Encode:
			muxVideo = jobItem.EncodedVideo
			jobItem.EncodedVideo = ""
		elif jobItem.JobConfig.VideoMode == StreamProcessMode.Copy:
			muxVideo = jobItem.SourceFile
		else:
			muxVideo = ""
		jobItem.FilesToDeleteWhenProcessingFails.Add(jobItem.DestFile)
		self.Mux(muxVideo, muxAudio, jobItem.DestFile, e)
			
	
	private def JobEventReport(jobItem as JobItem):
		self._workerReporting = true
		self.backgroundWorker1.ReportProgress(0, jobItem)
		while self._workerReporting:
			Threading.Thread.Sleep(1)
	
	private def BackgroundWorker1ProgressChanged(sender as object, e as ProgressChangedEventArgs):
		try:
			jobItem = cast(JobItem, e.UserState)
			//video
			if jobItem.Event == JobEvent.VideoEncoding:
				self.videoProgressBar.Value = cast(int, (jobItem.VideoEncoder.Progress))
				self.videoTimeUsed.Text = jobItem.VideoEncoder.TimeUsed.ToString()
				self.videoTimeLeft.Text = jobItem.VideoEncoder.TimeLeft.ToString()
			//audio
			elif jobItem.Event == JobEvent.AudioEncoding:
				self.audioProgressBar.Value = cast(int, (jobItem.AudioEncoder.Progress))
				self.audioTimeUsed.Text = jobItem.AudioEncoder.TimeUsed.ToString()
				self.audioTimeLeft.Text = jobItem.AudioEncoder.TimeLeft.ToString()
			//mux
			elif jobItem.Event == JobEvent.Muxing:
				self.muxProgressBar.Value = cast(int, jobItem.Muxer.Progress)
				self.muxTimeUsed.Text = jobItem.Muxer.TimeUsed.ToString()
				self.muxTimeLeft.Text = jobItem.Muxer.TimeLeft.ToString()
			//One job starts
			elif jobItem.Event == JobEvent.OneJobItemProcessing:
				ResetProgress()
				jobItem.State = JobState.Working
				self.startButton.Enabled = false
				i = self._workingJobItems.IndexOf(jobItem)
				self.statusLable.Text = "正在处理第${i+1}个文件，共${self._workingJobItems.Count}个文件"
			//One job done
			elif jobItem.Event == JobEvent.OneJobItemDone:
				jobItem.FilesToDeleteWhenProcessingFails.Clear()
				jobItem.State = JobState.Done
				i = self._workingJobItems.IndexOf(jobItem)
				self.statusLable.Text = "第${i+1}个文件处理完毕，共${self._workingJobItems.Count}个文件"
				
			//all done
			elif jobItem.Event == JobEvent.AllDone:
				self.statusLable.Text = "${self._workingJobItems.Count}个文件处理完成"
				self.startButton.Enabled = true
			//all stop
			elif jobItem.Event == JobEvent.QuitAllProcessing:
				ResetProgress()
				jobItem.State = JobState.Stop
				jobItem.VideoEncoder = null
				jobItem.AudioEncoder = null
				jobItem.Muxer = null
				self.startButton.Enabled = true
				self.statusLable.Text = "中止"
				self.tabControl1.SelectTab(self.inputPage)
				for file in jobItem.FilesToDeleteWhenProcessingFails:
					File.Delete(file)
				jobItem.FilesToDeleteWhenProcessingFails.Clear()
			//one stop
			elif jobItem.Event == JobEvent.OneJobItemCancelled:
				jobItem.State = JobState.Stop
				self.statusLable.Text = "中止"
				for file in jobItem.FilesToDeleteWhenProcessingFails:
					File.Delete(file)
				jobItem.FilesToDeleteWhenProcessingFails.Clear()
			//Error
			elif jobItem.Event == JobEvent.Error:
				jobItem.State = JobState.Error
				self.statusLable.Text = "错误"
				for file in jobItem.FilesToDeleteWhenProcessingFails:
					File.Delete(file)
				jobItem.FilesToDeleteWhenProcessingFails.Clear()
			self._workerReporting = false
		except e:
			MessageBox.Show("发生了一个错误。\nBackgroundWorker1ProgressChanged:\n"+e.ToString())

	private def EncodeVideo(avsFile as string, destFile as string, config as VideoEncConfigBase, e as DoWorkEventArgs):
		try:
			jobItem = cast(JobItem, e.Argument)
			encoder = X264(avsFile, destFile)
			encoder.Config = config as X264Config
			jobItem.VideoEncoder = encoder
			jobItem.Event = JobEvent.VideoEncoding
			result = EncodingReport.BeginInvoke(jobItem, encoder, e)
			if not self.backgroundWorker1.CancellationPending:
				try:
					encoder.Start()
				except as BadEncoderCmdException:
					MessageBox.Show("视频编码失败。是否使用了不正确的命令行？", "编码失败", MessageBoxButtons.OK, MessageBoxIcon.Error)
					self.backgroundWorker1.CancelAsync()
			result.AsyncWaitHandle.WaitOne()
			jobItem.VideoEncoder = null
		except e:
			MessageBox.Show("发生了一个错误。\nEncodeVideo:\n"+e.ToString())

	private def EncodeAudio(avsFile as string, destFile as string, config as AudioEncConfigBase, e as DoWorkEventArgs):
		jobItem = cast(JobItem, e.Argument)
		encoder = NeroAac(avsFile, destFile)
		encoder.Config = config as NeroAacConfig
		jobItem.AudioEncoder = encoder
		jobItem.Event = JobEvent.AudioEncoding
		result = EncodingReport.BeginInvoke(jobItem, encoder, e)
		if not self.backgroundWorker1.CancellationPending:
			encoder.Start()
		result.AsyncWaitHandle.WaitOne()
		jobItem.AudioEncoder = null

	private def Mux(video as string, audio as string, dstFile as string, e as DoWorkEventArgs):
		jobItem = cast(JobItem, e.Argument)
		
		muxer as MuxerBase
		if jobItem.JobConfig.Muxer == Muxer.MP4Box:
			muxer = MP4Box()
		elif jobItem.JobConfig.Muxer == Muxer.FFMP4:
			muxer = FFMP4()
		elif jobItem.JobConfig.Muxer == Muxer.MKVMerge:
			muxer = MKVMerge()
		if muxer != null:
			muxer.VideoFile = video
			muxer.AudioFile = audio
			muxer.DstFile = dstFile
			jobItem.Muxer = muxer
			jobItem.Event = JobEvent.Muxing
			result = EncodingReport.BeginInvoke(jobItem, muxer, e)
			if not self.backgroundWorker1.CancellationPending:
				try:
					muxer.Start()
				except exc as FormatNotSupportedException:
					MessageBox.Show("合成MP4失败。可能源媒体流中有不支持的格式。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Warning)
					jobItem.Event = JobEvent.Error
					JobEventReport(jobItem)
				except exc as FFmpegBugException:
					MessageBox.Show("合成MP4失败。这是由于FFmpeg的一些Bug, 对某些流无法使用复制。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Warning)
					jobItem.Event = JobEvent.Error
					JobEventReport(jobItem)
			result.AsyncWaitHandle.WaitOne()
			jobItem.Muxer = null
			if jobItem.JobConfig.AudioMode == StreamProcessMode.Encode and not IsSameFile(audio, dstFile):
				File.Delete(audio)
		
	private def EncodingReport(jobItem as JobItem, encoder as IEncoder, e as DoWorkEventArgs):
		
		while true:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				StopWorker(encoder, e)
				break
			if jobItem.State == JobState.Error:
				break
			if encoder.Progress == 100:
				JobEventReport(jobItem)
				break
			JobEventReport(jobItem)
	
	private def StopWorker(encoder as IEncoder, e as DoWorkEventArgs):
			jobItem = cast(JobItem, e.Argument)
			encoder.Stop()
			jobItem.Event = JobEvent.QuitAllProcessing
			JobEventReport(jobItem)

	private def NextJobOrExist(sender as object, e as RunWorkerCompletedEventArgs):
		
		self._workingJobItem = null
		if e.Result != null:
			lastJobItem = cast(JobItem, e.Result)
			if lastJobItem.Event in (JobEvent.AllDone, JobEvent.QuitAllProcessing):
				lastJobItem.Event = JobEvent.None
				self._workingJobItems.Clear()
				return
			
			if _workingJobItems[-1] is not lastJobItem:
				nextIndex = _workingJobItems.IndexOf(lastJobItem) + 1
				nextJobItem = _workingJobItems[nextIndex]
				self._workingJobItem = nextJobItem
				self.backgroundWorker1.RunWorkerAsync(nextJobItem) 
		


	private def SaveProfileSelection():
		config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		configSection as ProgramConfig = config.Sections["programConfig"]
		if configSection == null:
			configSection = ProgramConfig()
			config.Sections.Add("programConfig", configSection)
			config.Save()
		configSection.ProfileName = self.profileBox.Text
		config.Save()
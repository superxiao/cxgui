// Methods
namespace CXGUI.GUI

import System
import System.IO
import System.Threading
import System.Runtime.InteropServices
import System.Collections
import System.Collections.Generic
import System.Windows.Forms
import System.Drawing
import System.Runtime.Serialization.Formatters.Binary
import System.Configuration
import DirectShowLib
import System.ComponentModel
import CXGUI
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer
import CXGUI.Avisynth
import DirectShowLib.DES
import My

partial class MainForm(System.Windows.Forms.Form):
	
	_configForm as ProgramConfigForm
	_mediaSettingForm as MediaSettingForm
	_programConfig as ProgramConfig
	_jobItems = Dictionary[of ListViewItem, JobItem]()
	_workingItem as JobItem 
	_workingItems as Boo.Lang.List[of JobItem]
	_workerReporting as bool

	public def constructor():
		self.InitializeComponent()

		config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		_programConfig = config.Sections["programConfig"]
		if _programConfig == null:
			_programConfig = ProgramConfig()
			config.Sections.Add("programConfig", _programConfig)
			config.Save()
		self._configForm = ProgramConfigForm(_programConfig)

	private def AddButtonClick(sender as object, e as EventArgs):
		self.openFileDialog1.ShowDialog()

	private def OpenFileDialog1FileOk(sender as object, e as CancelEventArgs):
		firstAddItem as ListViewItem
		for fileName in self.openFileDialog1.FileNames:
			item = AddFile(fileName)
			if firstAddItem == null and item != null:
				firstAddItem = item
		if firstAddItem != null:
			self.listView1.SelectedItems.Clear()
			firstAddItem.Selected = true

	private def AddFile(filePath as string) as ListViewItem:
		if _configForm.chbInputDir.Checked:
			fileName = filePath
		else:
			fileName = Path.GetFileName(filePath)
		item as ListViewItem
		addFile = do:
			if self._configForm.destDirComboBox.Text != "":
				destFile = Path.Combine(self._configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(fileName)+".mp4")
			else:
				destFile = Path.ChangeExtension(filePath, "mp4")
			if IsSameFile(filePath, destFile):
				destFile = Path.Combine(Path.GetDirectoryName(destFile), Path.GetFileNameWithoutExtension(destFile) + '1' + Path.GetExtension(destFile))
			item = ListViewItem(("等待", fileName, destFile))
			self.listView1.Items.Add(item)
			jobItem = JobItem(filePath, destFile, item, self.profileBox.Text)
			self._jobItems.Add(item, jobItem)
				
		o as IMediaDet = MediaDet()
		errorCode as int = o.put_Filename(filePath)
		try:
			Marshal.ThrowExceptionForHR(errorCode)
			addFile()
		except:
			result = MessageBox.Show(filePath + "\n在文件中找不到视频流或音频流，可能是没有安装对应的DirectShow滤镜。\n" + "仍然要添加该文件吗？", 
			"检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
			if result == DialogResult.OK:
				addFile()
		return item


	private def SetUpItems(items as (JobItem)):
	"""
	根据JobItem对象的ProfileName属性为其读取各设置实例。
	如对应Profile文件不存在，则显示警告信息，并决定是否刷新预设列表。
	如刷新预设列表后预设数为0，则自动重建Default预设。
	"""
	
		for item in items:
			try:
				item.SetUp()
			except as ProfileNotFoundException:
				self.UpdateProfileBox(Profile.GetProfileNames(), self.profileBox.Text)
				item.ProfileName = self.profileBox.Text
				item.SetUp()
				
	private def StartButtonClick(sender as object, e as EventArgs):
		_workingItems = GetWorkingJobItems()
		SetUpItems(_workingItems.ToArray())
		if _workingItems.Count > 0:
			self.backgroundWorker1.RunWorkerAsync(_workingItems[0])
			self.tabControl1.SelectTab(self.progressPage)
		
	private def GetWorkingJobItems() as Boo.Lang.List[of JobItem]:
		jobItems = Boo.Lang.List[of JobItem]()
		
		for listItem as ListViewItem in self.listView1.Items:
			if listItem.SubItems[0].Text == "中止":
				if _configForm.chbSilentRestart.Checked:
					self._jobItems[listItem].State = JobState.Waiting
					listItem.SubItems[0].Text = "等待"
				else:
					result = MessageBox.Show(listItem.SubItems[1].Text + "\n该项已经中止。是否重新开始？\n", 
					"项目中止", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
					if result == DialogResult.OK:
						self._jobItems[listItem].State = JobState.Waiting
						listItem.SubItems[0].Text = "等待"
						
		for listItem as ListViewItem in self.listView1.Items:
			if listItem.SubItems[0].Text == "等待":
				jobItem = self._jobItems[listItem]
				jobItems.Add(jobItem)
		return jobItems

	private def BackgroundWorker1DoWork(sender as object, e as DoWorkEventArgs):
		jobItem as JobItem = e.Argument
		jobItem.Event = JobEvent.OneStart
		SyncReport(jobItem)
		avsConfig = jobItem.AvsConfig
		jobConfig = jobItem.JobConfig

		if jobConfig.VideoMode == JobMode.Encode:
			self.WriteVideoAvs(jobItem.SourceFile, 'video.avs', avsConfig)
			self.EncodeVideo('video.avs', jobItem.DestFile, jobItem.VideoEncConfig, e)
		
		return if IsCancelled(e)

		if jobConfig.AudioMode == JobMode.Encode:
			if jobConfig.UseSeparateAudio and File.Exists(jobItem.SeparateAudio):
				sourceAudio = jobItem.SeparateAudio
			else:
				sourceAudio = jobItem.SourceFile
			self.WriteAudioAvs(sourceAudio, 'audio.avs', avsConfig)
			if jobConfig.VideoMode != JobMode.None:
				destAudio = Path.ChangeExtension(jobItem.DestFile, 'm4a')
			else:
				destAudio = jobItem.DestFile

			try:
				self.EncodeAudio('audio.avs', destAudio, jobItem.AudioEncConfig, e)
			except InvalidAudioAvisynthScriptException:
				ChangeSourceAndRetry = do:
					source = avsConfig.AudioSource
					source = source + 1 if source == 0
					source = source - 1 if source == 1 
					self.WriteAudioAvs(jobItem.SourceFile, 'audio.avs', avsConfig)
					try:
						self.EncodeAudio('audio.avs', destAudio, jobItem.AudioEncConfig, e)
					except InvalidAudioAvisynthScriptException:
						MessageBox.Show(jobItem.SourceFile + "\n音频脚本无法读取。", 
				"检测失败", MessageBoxButtons.OK, MessageBoxIcon.Error)
				if self._configForm.cbAudioAutoSF.Checked:
					ChangeSourceAndRetry()
				else:
					result = MessageBox.Show(jobItem.SourceFile + "\n该文件的音频脚本无法读取。是否尝试更改源滤镜？", 
					"检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
					jobConfig.AudioMode = JobMode.None
					if result == DialogResult.OK:
						ChangeSourceAndRetry()
			

		return if IsCancelled(e)

		if jobConfig.Muxer != Muxer.None:
			if jobConfig.AudioMode == JobMode.Encode:
				muxAudio = destAudio
			elif jobConfig.AudioMode == JobMode.Copy:
				if jobConfig.UseSeparateAudio and jobItem.SeparateAudio != "":
					muxAudio = jobItem.SeparateAudio
				else:
					muxAudio = jobItem.SourceFile
			else:
				muxAudio = ""
					
			if jobConfig.VideoMode == JobMode.Encode:
				muxVideo = jobItem.DestFile
			elif jobConfig.VideoMode == JobMode.Copy:
				muxVideo = jobItem.SourceFile
			else:
				muxVideo = ""
			self.Mux(muxVideo, muxAudio, jobItem.DestFile, e)

		return if IsCancelled(e)
		jobItem.Event = JobEvent.OneDone
		SyncReport(jobItem)
		if self._workingItems[-1] is jobItem:
			jobItem.Event = JobEvent.AllDone
			SyncReport(jobItem)
		else:
			index = self._workingItems.IndexOf(jobItem)
			e.Result = self._workingItems[index+1]
			
	private def IsCancelled(e as DoWorkEventArgs):
		if self.backgroundWorker1.CancellationPending:
			e.Cancel = true
			return true
		else:
			return false
	
	private def SyncReport(jobItem as JobItem):
		self._workerReporting = true
		self.backgroundWorker1.ReportProgress(0, jobItem)
		while self._workerReporting:
			Threading.Thread.Sleep(1)
	
	private def BackgroundWorker1ProgressChanged(sender as object, e as ProgressChangedEventArgs):
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
		elif jobItem.Event == JobEvent.OneStart:
			self._workingItem = jobItem
			ResetProgress()
			jobItem.State = JobState.Working
			jobItem.UIItem.SubItems[0].Text = "工作中"
			self.startButton.Enabled = false
			i = self._workingItems.IndexOf(jobItem)
			self.statusLable.Text = "正在处理第${i+1}个文件，共${self._workingItems.Count}个文件"
		//One job done
		elif jobItem.Event == JobEvent.OneDone:
			self._workingItem = null
			if not jobItem.KeepingCfg:
				jobItem.Clear()
			jobItem.State = JobState.Done
			jobItem.UIItem.SubItems[0].Text = "完成"
			i = self._workingItems.IndexOf(jobItem)
			self.statusLable.Text = "第${i+1}个文件处理完毕，共${self._workingItems.Count}个文件"
			
		//all done
		elif jobItem.Event == JobEvent.AllDone:
			self._workingItem = null
			self.statusLable.Text = "${self._workingItems.Count}个文件处理完成"
			self._workingItems.Clear()
			self.startButton.Enabled = true
		//Stop
		elif jobItem.Event == JobEvent.Stop:
			self._workingItem = null
			self._workingItems.Clear()
			ResetProgress()
			jobItem.State = JobState.Stop
			jobItem.UIItem.SubItems[0].Text = "中止"
			jobItem.VideoEncoder = null
			jobItem.AudioEncoder = null
			jobItem.Muxer = null //TODO 删除文件
			self.startButton.Enabled = true
			self.statusLable.Text = "中止"
			self.tabControl1.SelectTab(self.inputPage)
		self._workerReporting = false

	private def EncodeVideo(avsFile as string, destFile as string, config as VideoEncConfigBase, e as DoWorkEventArgs):
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
					self.backgroundWorker1.CancelAsync()
				except exc as FFmpegBugException:
					MessageBox.Show("合成MP4失败。这是由于FFmpeg的一些Bug, 对某些流无法使用复制。", "合成失败", MessageBoxButtons.OK, MessageBoxIcon.Warning)
					self.backgroundWorker1.CancelAsync()
			result.AsyncWaitHandle.WaitOne()
			jobItem.Muxer = null
			if jobItem.JobConfig.AudioMode == JobMode.Encode:
				File.Delete(audio)
		
	private def EncodingReport(jobItem as JobItem, encoder as IEncoder, e as DoWorkEventArgs):
		while true:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				StopWorker(encoder, e)
				break
			if encoder.Progress == 100:
				SyncReport(jobItem)
				break
			SyncReport(jobItem)
	
	private def StopWorker(encoder as IEncoder, e as DoWorkEventArgs):
			jobItem = cast(JobItem, e.Argument)
			e.Cancel = true
			encoder.Stop()
			jobItem.Event = JobEvent.Stop
			SyncReport(jobItem)

	private def NextJob(sender as object, e as RunWorkerCompletedEventArgs):
		if e.Cancelled or e.Result == null:
			return
		nextJobItem = e.Result
		self.backgroundWorker1.RunWorkerAsync(nextJobItem) 

	private def ClearButtonClick(sender as object, e as EventArgs):
		self.listView1.Items.Clear()
		self._jobItems.Clear()
	
	private def ContextMenuStrip1Opening(sender as object, e as CancelEventArgs):
		if self.listView1.SelectedItems.Count == 0:
			e.Cancel = true
		elif self.listView1.SelectedItems.Count == 1:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = true
			self.listViewMenu.Items['打开目录ToolStripMenuItem'].Enabled = true
		else:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = false
			self.listViewMenu.Items['打开目录ToolStripMenuItem'].Enabled = false
	
	private def DelButtonClick(sender as object, e as EventArgs):
		for item as ListViewItem in self.listView1.SelectedItems:
			self.listView1.Items.Remove(item)
			self._jobItems.Remove(item)
	
	private def MainFormActivated(sender as object, e as System.EventArgs):

		if _configForm.chbInputDir.Checked:
			for item as ListViewItem in self.listView1.Items:
				item.SubItems[1].Text = self._jobItems[item].SourceFile
		else:
			for item as ListViewItem in self.listView1.Items:
				item.SubItems[1].Text = Path.GetFileName(self._jobItems[item].SourceFile)
			
	private def ListView1ItemSelectionChanged(sender as object, e as ListViewItemSelectionChangedEventArgs):
		if self.listView1.SelectedItems.Count == 1:
			self.settingButton.Enabled = true
		else:
			self.settingButton.Enabled = false
	
	private def ResetProgress():
		self.videoProgressBar.Value = 0
		self.audioProgressBar.Value = 0
		self.muxProgressBar.Value = 0
		self.videoTimeLeft.Text = string.Empty
		self.videoTimeUsed.Text = string.Empty
		self.audioTimeLeft.Text = string.Empty
		self.audioTimeUsed.Text = string.Empty
		self.muxTimeLeft.Text = string.Empty
		self.muxTimeUsed.Text = string.Empty

	private def SettingButtonClick(sender as object, e as EventArgs):
		item as ListViewItem = self.listView1.SelectedItems[0]
		jobItem = self._jobItems[item]
		SetUpItems((jobItem,))
		if self._mediaSettingForm == null:
			self._mediaSettingForm = MediaSettingForm()
		self._mediaSettingForm.ImportProfiles(array(string, self.profileBox.Items), jobItem.ProfileName)
		self._mediaSettingForm.SetUpForItem(jobItem)
		result = self._mediaSettingForm.ShowDialog()
		if result == DialogResult.OK and self._mediaSettingForm.Changed:
			self._mediaSettingForm.Changed = false
			jobItem.State = JobState.Waiting
			jobItem.UIItem.Text = "等待"
			jobItem.DestFile = self._mediaSettingForm.DestFile
			jobItem.AvsConfig = self._mediaSettingForm.AvsConfig
			jobItem.VideoEncConfig = self._mediaSettingForm.VideoEncConfig
			jobItem.AudioEncConfig = self._mediaSettingForm.AudioEncConfig
			jobItem.JobConfig = self._mediaSettingForm.JobConfig
			jobItem.ProfileName = self._mediaSettingForm.UsingProfile //TODO 因为可能在SettingForm里更改了profile,无论如何这里要重导入settingForm的列表，并先保存选定项，而后决定选择哪个
			jobItem.KeepingCfg = true
			if jobItem.JobConfig.UseSeparateAudio:
				jobItem.SeparateAudio = self._mediaSettingForm.SepAudio
		else:
			jobItem.Clear()

		
		UpdateProfileBox(self._mediaSettingForm.GetProfiles(), self.profileBox.Text)

		self._mediaSettingForm.Clear()
		
	private def UpdateProfileBox(profileNames as (string), selectedProfile as string):
		self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged
		self.profileBox.Items.Clear()
		self.profileBox.Items.AddRange(profileNames)
		if not self.profileBox.Items.Contains("Default"):
			Profile.RebuildDefault("Default")
			self.profileBox.Items.Add("Default")
		if self.profileBox.Items.Contains(selectedProfile):
			self.profileBox.SelectedItem = selectedProfile
		else:
			self.profileBox.SelectedItem = "Default"
		self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged
		

	private def StopButtonClick(sender as object, e as EventArgs):
		try:
			self.backgroundWorker1.CancelAsync()
		except:
			pass

	private def WriteAudioAvs(sourceFile as string, avsFile as string, avsConfig as AvisynthConfig):
		writer = AvisynthWriter(sourceFile)
		audioConfig as AudioScriptConfig = writer.AudioConfig
		audioConfig.DownMix = avsConfig.DownMix
		audioConfig.Normalize = avsConfig.Normalize
		writer.AudioScriptFile = avsFile
		audioConfig.SourceFilter = avsConfig.AudioSource
		writer.WriteAudioScript()

	private def WriteVideoAvs(sourceFile as string, avsFile as string, avsConfig as AvisynthConfig):
		writer = AvisynthWriter(sourceFile)
		videoConfig as VideoScriptConfig = writer.VideoConfig
		if avsConfig.Width > 0:
			videoConfig.Width = avsConfig.Width
		if avsConfig.Height > 0:
			videoConfig.Height = avsConfig.Height
		videoConfig.ConvertFPS = avsConfig.ConvertFPS
		if avsConfig.FrameRate > 0:
			videoConfig.FrameRate = avsConfig.FrameRate
		videoConfig.Resizer = avsConfig.Resizer
		videoConfig.SourceFilter = avsConfig.VideoSource
		writer.VideoScriptFile = avsFile
		writer.WriteVideoScript()

	private def 等待ToolStripMenuItemClick(sender as object, e as EventArgs):
		for item as ListViewItem in self.listView1.SelectedItems:
			self._jobItems[item].State = JobState.Waiting
			item.SubItems[0].Text = '等待'

	
	private def 清空ToolStripMenuItemClick(sender as object, e as EventArgs):
		self.clearButton.PerformClick()

	
	private def 删除ToolStripMenuItemClick(sender as object, e as EventArgs):
		self.delButton.PerformClick()

	
	private def 设置ToolStripMenuItemClick(sender as object, e as EventArgs):
		self.settingButton.PerformClick()

	
	private def 添加ToolStripMenuItemClick(sender as object, e as EventArgs):
		self.addButton.PerformClick()
	
	private def 添加ToolStripMenuItem1Click(sender as object, e as EventArgs):
		self.addButton.PerformClick()

	private def 选项ToolStripMenuItemClick(sender as object, e as EventArgs):
		self._configForm.ShowDialog()
	
	private def ListView1ItemDrag(sender as object, e as System.Windows.Forms.ItemDragEventArgs):
		self.listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move)
		
	private def ListView1DragEnter(sender as object, e as System.Windows.Forms.DragEventArgs):
		if e.Data.GetDataPresent(ListView.SelectedListViewItemCollection) or e.Data.GetDataPresent(DataFormats.FileDrop):
			e.Effect = DragDropEffects.Move

	private def ListView1DragDrop(sender as object, e as System.Windows.Forms.DragEventArgs):
		if e.Data.GetDataPresent(DataFormats.FileDrop):
			self.listView1.SelectedItems.Clear()
			for path in (e.Data.GetData(DataFormats.FileDrop) as (string)):
				if IO.File.Exists(path):
					addItem = AddFile(path)
					if addItem != null:
						addItem.Selected = true
				elif IO.Directory.Exists(path):
					for file in IO.Directory.GetFiles(path):
						addItem = AddFile(file)
						if addItem != null:
							addItem.Selected = true
			return

		dragItems = array(self.listView1.SelectedItems)
		cp as Point = listView1.PointToClient(Point(e.X, e.Y))
		dragToItem = self.listView1.GetItemAt(cp.X, cp.Y)
		if dragToItem == null:
			return
		dragToIndex = dragToItem.Index
		for item as ListViewItem in dragItems:
			self.listView1.Items.Remove(item)
		if self.listView1.Items.Count < dragToIndex:
			dragToIndex = self.listView1.Items.Count
		for item as ListViewItem in dragItems:
			self.listView1.Items.Insert(dragToIndex, item)
			dragToIndex++
	
	private def MainFormLoad(sender as object, e as System.EventArgs):
		jobItems as Dictionary[of ListViewItem, JobItem]
		formater = BinaryFormatter()
		if not File.Exists("JobItems.bin"):
			return
		try:
			stream = FileStream("JobItems.bin", FileMode.Open)
			jobItems = formater.Deserialize(stream)
			stream.Close()
		except:
			stream.Close()
			return
		if jobItems != null:
			for jobItem as JobItem in jobItems.Values:
				self.listView1.Items.Add(jobItem.UIItem)
				self._jobItems.Add(jobItem.UIItem, jobItem)
		self.UpdateProfileBox(Profile.GetProfileNames(), _programConfig.ProfileName)
			
	private def MainFormFormClosing(sender as object, e as System.Windows.Forms.FormClosingEventArgs):
		if self._workingItem != null and self._workingItem.State == JobState.Working:
			result = MessageBox.Show("正在工作中，是否中止并退出？", "工作中", MessageBoxButtons.YesNo, 
			MessageBoxIcon.Information)
			if result == DialogResult.Yes:
				StopButtonClick(null, null)
			else:
				e.Cancel = true
				return
		for item in self._jobItems.Values:
			item.VideoEncoder = null
			item.AudioEncoder = null
			item.Muxer = null
		formater = BinaryFormatter()
		stream = FileStream("JobItems.bin", FileMode.Create)
		formater.Serialize(stream, self._jobItems)
		stream.Close()
		SaveConfig()
	
	private def 打开目录ToolStripMenuItemClick(sender as object, e as System.EventArgs):
		jobItem = self._jobItems[self.listView1.SelectedItems[0]]
		System.Diagnostics.Process.Start("explorer.exe", "/select, " + jobItem.SourceFile)
	
	private def ListView1DoubleClick(sender as object, e as System.EventArgs):
		self.settingButton.PerformClick()

	private def SaveConfig():
		config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		configSection as ProgramConfig = config.Sections["programConfig"]
		if configSection == null:
			configSection = ProgramConfig()
			config.Sections.Add("programConfig", configSection)
			config.Save()
		configSection.ProfileName = self.profileBox.Text
		config.Save()
	
	private def ProfileBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		result = MessageBox.Show("是否应用更改到所有项目？", "预设更改", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
		if result == DialogResult.Yes:
			for item in self._jobItems.Values:
				item.ProfileName = self.profileBox.Text
[STAThread]
public def Main(argv as (string)) as void:
	Application.EnableVisualStyles()
	Application.SetCompatibleTextRenderingDefault(false)
	Application.Run(MainForm())
	//VideoEncoding.vetest()
	//CXGUI.AudioEncoding.aebtest()
	//CXGUI.GUI.cfgtest()
	//test()
	//CXGUI.StreamMuxer.fftest()
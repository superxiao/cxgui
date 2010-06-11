// Methods
namespace CXGUI.GUI

import System
import System.IO
import System.Threading
import System.Runtime.InteropServices
import System.Collections.Generic
import System.Windows.Forms
import System.Drawing
import System.Runtime.Serialization.Formatters.Binary
import DirectShowLib
import System.ComponentModel
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer
import CXGUI.Avisynth
import DirectShowLib.DES
import My

partial class MainForm(System.Windows.Forms.Form):
	
	_configForm as ProgramConfigForm
	_mediaSettingForm as MediaSettingForm
	_jobItems = Dictionary[of ListViewItem, JobItem]()
	_workingItem as JobItem 
	_workingItems as Boo.Lang.List[of JobItem]
	_workerReporting as bool

	public def constructor():
		self.InitializeComponent()
		self._configForm = ProgramConfigForm()

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
			jobItem = JobItem(filePath, destFile, item, false)
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

	private def StartButtonClick(sender as object, e as EventArgs):
		_workingItems = GetWorkingJobItems()
		SetUpItems(_workingItems)
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
	
	private def SetUpItems(jobItems as IEnumerable[of JobItem]):
		for item in jobItems:
			if item.AvsConfig == null:
				item.ReadAvsConfig()
			if item.VideoEncConfig == null:
				item.ReadVideoEncConfig()
			if item.AudioEncConfig == null:
				item.ReadAudioEncConfig()
		
	private def BackgroundWorker1DoWork(sender as object, e as DoWorkEventArgs):
		jobItem as JobItem = e.Argument
		jobItem.Event = JobEvent.OneStart
		SyncReport(jobItem)
		avsConfig = jobItem.AvsConfig

		if not avsConfig.Mode == JobMode.Audio:
			self.WriteVideoAvs(jobItem.SourceFile, 'video.avs', avsConfig)
			self.EncodeVideo('video.avs', jobItem.DestFile, jobItem.VideoEncConfig, e)
		if not avsConfig.Mode == JobMode.Video:
			self.WriteAudioAvs(jobItem.SourceFile, 'audio.avs', avsConfig)
			if avsConfig.Mode == JobMode.Audio:
				destAudio = Path.ChangeExtension(jobItem.DestFile, 'm4a')
			else:
				destAudio = jobItem.DestFile
			try:
				self.EncodeAudio('audio.avs', destAudio, jobItem.AudioEncConfig, e)
			except InvalidAudioAvisynthScriptException:

				ChangeSourceAndRetry = do:
					if avsConfig.AudioSource == AudioScriptConfig.AudioSourceFilter.FFAudioSource:
						avsConfig.AudioSource = AudioScriptConfig.AudioSourceFilter.DirectShowSource
					else:
						avsConfig.AudioSource = AudioScriptConfig.AudioSourceFilter.FFAudioSource
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
					if result == DialogResult.OK:
						ChangeSourceAndRetry()
		if avsConfig.Mode == JobMode.Normal:
			self.Mux(jobItem.DestFile, destAudio, "", jobItem.AvsConfig.Muxer, e)
			File.Delete(destAudio)

		jobItem.Event = JobEvent.OneDone
		SyncReport(jobItem)
		if self._workingItems[-1] is jobItem:
			jobItem.Event = JobEvent.AllDone
			SyncReport(jobItem)
		else:
			index = self._workingItems.IndexOf(jobItem)
			e.Result = self._workingItems[index+1]
			
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
		EncodeAndReport(jobItem, encoder, e)
		jobItem.VideoEncoder = null

	private def EncodeAudio(avsFile as string, destFile as string, config as AudioEncConfigBase, e as DoWorkEventArgs):
		jobItem = cast(JobItem, e.Argument)
		encoder = NeroAac(avsFile, destFile)
		encoder.Config = config as NeroAacConfig
		jobItem.AudioEncoder = encoder
		jobItem.Event = JobEvent.AudioEncoding
		EncodeAndReport(jobItem, encoder, e)
		jobItem.AudioEncoder = null

	private def Mux(video as string, audio as string, dstFile as string, streamMuxer as Muxer, e as DoWorkEventArgs):
		jobItem = cast(JobItem, e.Argument)
		box = MP4Box()
		box.VideoFile = video
		box.AudioFile = audio
		box.DstFile = dstFile
		jobItem.Muxer = box
		jobItem.Event = JobEvent.Muxing
		EncodeAndReport(jobItem, box, e)
		jobItem.Muxer = null
		
	private def EncodeAndReport(jobItem as JobItem, encoder as IEncoder, e as DoWorkEventArgs):
		thread = Thread(ThreadStart(encoder.Start))
		thread.Start()
		while true:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				e.Cancel = true
				encoder.Stop()
				jobItem.Event = JobEvent.Stop
				SyncReport(jobItem)
				break
			if encoder.Progress >= 100:
				SyncReport(jobItem)
				break
			SyncReport(jobItem)
		thread.Join()

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
			self._mediaSettingForm = MediaSettingForm(jobItem.SourceFile, jobItem.DestFile,
			jobItem.AvsConfig, jobItem.VideoEncConfig, jobItem.AudioEncConfig)
		else:
			self._mediaSettingForm.SourceFile = jobItem.SourceFile
			self._mediaSettingForm.DestFile = jobItem.DestFile
			self._mediaSettingForm.AvsConfig = jobItem.AvsConfig
			self._mediaSettingForm.VideoEncConfig = jobItem.VideoEncConfig
			self._mediaSettingForm.AudioEncConfig = jobItem.AudioEncConfig
		result = self._mediaSettingForm.ShowDialog()
		if result == DialogResult.OK and self._mediaSettingForm.Changed:
			jobItem.State = JobState.Waiting
			jobItem.UIItem.Text = "等待"
			self._mediaSettingForm.Changed = false
			jobItem.DestFile = self._mediaSettingForm.DestFile
			jobItem.AvsConfig = self._mediaSettingForm.AvsConfig
			jobItem.VideoEncConfig = self._mediaSettingForm.VideoEncConfig
			jobItem.AudioEncConfig = self._mediaSettingForm.AudioEncConfig
			jobItem.KeepingCfg = true
		else:
			jobItem.Clear()

	private def StopButtonClick(sender as object, e as EventArgs):
		try:
			self.backgroundWorker1.CancelAsync()
		except:
			pass

	private def WriteAudioAvs(sourceFile as string, avsFile as string, avsConfig as AvsConfig):
		writer = AvisynthWriter(sourceFile)
		audioConfig as AudioScriptConfig = writer.AudioConfig
		audioConfig.DownMix = avsConfig.DownMix
		audioConfig.Normalize = avsConfig.Normalize
		writer.AudioScriptFile = avsFile
		audioConfig.SourceFilter = avsConfig.AudioSource
		writer.WriteAudioScript()

	private def WriteVideoAvs(sourceFile as string, avsFile as string, avsConfig as AvsConfig):
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

	
	private def MainFormFormClosing(sender as object, e as System.Windows.Forms.FormClosingEventArgs):
		if self._workingItem != null and self._workingItem.State == JobState.Working:
			result = MessageBox.Show("正在工作中，是否中止并退出？", "工作中", MessageBoxButtons.YesNo, 
			MessageBoxIcon.Information)
			if result == DialogResult.Yes:
				StopButtonClick(null, null)
			else:
				e.Cancel = true
				return
		formater = BinaryFormatter()
		stream = FileStream("JobItems.bin", FileMode.Create)
		formater.Serialize(stream, self._jobItems)
		stream.Close()
	
	private def 打开目录ToolStripMenuItemClick(sender as object, e as System.EventArgs):
		jobItem = self._jobItems[self.listView1.SelectedItems[0]]
		System.Diagnostics.Process.Start("explorer.exe", "/select, " + jobItem.SourceFile)

		
[STAThread]
public def Main(argv as (string)) as void:
	Application.EnableVisualStyles()
	Application.SetCompatibleTextRenderingDefault(false)
	Application.Run(MainForm())
	//VideoEncoding.vetest()
	//CXGUI.AudioEncoding.aebtest()
	//CXGUI.GUI.cfgtest()
	//test()
#	CXGUI.StreamMuxer.test()
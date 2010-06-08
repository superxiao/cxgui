// Methods
namespace CXGUI.GUI

import System
import System.IO
import System.Threading
import System.Runtime.InteropServices
import System.Windows.Forms
import System.Drawing
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
	_workingProcess as IStopable 
	_itemSettingForms = {}
	_workingJobItem as JobItem
	_workingJobItems as List[of JobItem]
	thread as Threading.Thread
	_userStop as bool
	_workerReporting as bool

	public def constructor():
		self.InitializeComponent()
		self._configForm = ProgramConfigForm()

	private def AddButtonClick(sender as object, e as EventArgs):
		self.openFileDialog1.ShowDialog()

	private def OpenFileDialog1FileOk(sender as object, e as CancelEventArgs):
		for fileName in self.openFileDialog1.FileNames:
			item = AddFile(fileName)
			if item != null:
				self.listView1.SelectedItems.Clear()
				item.Selected = true

	private def AddFile(filePath as string) as ListViewItem:
		if _configForm.chbInputDir.Checked:
			fileName = filePath
		else:
			fileName = Path.GetFileName(filePath)
		item as ListViewItem
		addFile = do:
			if self._configForm.destDirComboBox.Text != "":
				destFile = Path.Combine(self._configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(filePath)+".mp4")
			else:
				destFile = Path.ChangeExtension(filePath, "mp4")
			if IsSameFile(filePath, destFile):
				destFile = Path.Combine(Path.GetDirectoryName(destFile), Path.GetFileNameWithoutExtension(destFile) + '1' + Path.GetExtension(destFile))
			item = ListViewItem(array(("等待", fileName, destFile)))
			self.listView1.Items.Add(item)

			form = MediaSettingForm(filePath, destFile)
			self._itemSettingForms.Add(item, form)
				
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
		if _workingJobItems != null:
			_workingJobItems.Clear()
		_workingJobItems = GetWorkingJobItems()
		if _workingJobItems.Count > 0:
			self.StartNewWork(null, null)
			self.tabControl1.SelectTab(self.progressPage)
		
	private def GetWorkingJobItems() as List[of JobItem]:
		jobItems = List[of JobItem]()
		
		for listItem as ListViewItem in self.listView1.Items:
			if listItem.SubItems[0].Text == "中止":
				if _configForm.chbSilentRestart.Checked:
					listItem.SubItems[0].Text = "等待"
				else:
					result = MessageBox.Show(listItem.SubItems[1].Text + "\n该项已经中止。是否重新开始？\n", 
					"项目中止", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
					if result = DialogResult.OK:
						listItem.SubItems[0].Text = "等待"
						
		for listItem as ListViewItem in self.listView1.Items:
			if listItem.SubItems[0].Text == "等待":
				form = self._itemSettingForms[listItem] as MediaSettingForm
				jobItem = JobItem()
				jobItem.SourceFile = form.SourceFile
				jobItem.DestinationFile = form.DestinationFile
				jobItem.AvsConfig = form.AvsConfig
				jobItem.VideoEncConfig = form.VideoEncConfig
				jobItem.AudioEncConfig = form.AudioEncConfig
				jobItem.WriteAudioScript = form.WriteAudioScript
				jobItem.WriteVideoScript = form.WriteVideoScript
				jobItem.UIItem = listItem
				jobItems.Add(jobItem)
		return jobItems

	private def StartNewWork(sender as object, e as RunWorkerCompletedEventArgs):
		if _userStop:
			_userStop = false //TODO
			return
		for jobItem as JobItem in _workingJobItems:
			if _workingJobItems.IndexOf(self._workingJobItem) < _workingJobItems.IndexOf(jobItem):
				self._workingJobItem = jobItem
				self.backgroundWorker1.RunWorkerAsync()
				break 
		
	private def BackgroundWorker1DoWork(sender as object, e as DoWorkEventArgs):
		_workingJobItem.Event = JobEvent.OneStart
		ExclusivelyReporteport(_workingJobItem)
		destinationFile as string 
		avsConfig as AvsConfigSection = _workingJobItem.AvsConfig
		if avsConfig is null:
			avsConfig = AvsConfigSection()

		if _workingJobItem.WriteVideoScript and not self.backgroundWorker1.CancellationPending:
			self.WriteVideoAvs(_workingJobItem.SourceFile, 'video.avs', avsConfig)
			self.EncodeVideo('video.avs', _workingJobItem.DestinationFile, _workingJobItem.VideoEncConfig)

		if _workingJobItem.WriteAudioScript and not self.backgroundWorker1.CancellationPending:
			self.WriteAudioAvs(_workingJobItem.SourceFile, 'audio.avs', avsConfig)
			if _workingJobItem.WriteVideoScript:
				destAudio = Path.ChangeExtension(_workingJobItem.DestinationFile, 'm4a')
			else:
				destAudio = _workingJobItem.DestinationFile
			try:
				self.EncodeAudio('audio.avs', destAudio, _workingJobItem.AudioEncConfig)
			except InvalidAudioAvisynthScriptException:

				ChangeSourceAndRetry = do:
					if avsConfig.AudioSource == AudioScriptConfig.AudioSourceFilter.FFAudioSource:
						avsConfig.AudioSource = AudioScriptConfig.AudioSourceFilter.DirectShowSource
					else:
						avsConfig.AudioSource = AudioScriptConfig.AudioSourceFilter.FFAudioSource
					self.WriteAudioAvs(_workingJobItem.SourceFile, 'audio.avs', avsConfig)
					try:
						self.EncodeAudio('audio.avs', destAudio, _workingJobItem.AudioEncConfig)
					except InvalidAudioAvisynthScriptException:
						MessageBox.Show(_workingJobItem.SourceFile + "\n音频脚本无法读取。", 
				"检测失败", MessageBoxButtons.OK, MessageBoxIcon.Error)
						_workingJobItem.WriteAudioScript = false

				if self._configForm.cbAudioAutoSF.Checked:
					ChangeSourceAndRetry()
				else:
					result = MessageBox.Show(_workingJobItem.SourceFile + "\n该文件的音频脚本无法读取。是否尝试更改源滤镜？", 
					"检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
					if result == DialogResult.OK:
						ChangeSourceAndRetry()

		if _workingJobItem.WriteVideoScript and _workingJobItem.WriteAudioScript and not self.backgroundWorker1.CancellationPending:
			self.Mux(_workingJobItem.DestinationFile, destAudio, "", _workingJobItem.AvsConfig.Muxer)
			File.Delete(destAudio)
		
		if not self.backgroundWorker1.CancellationPending:
			self._workingJobItem.Event = JobEvent.OneDone
			ExclusivelyReporteport(_workingJobItem)
			if self._workingJobItems[-1] is self._workingJobItem:
				self._workingJobItem.Event = JobEvent.AllDone
				ExclusivelyReporteport(_workingJobItem)
			
	private def ExclusivelyReporteport(userData as object):
		self._workerReporting = true
		self.backgroundWorker1.ReportProgress(0, userData)
		while self._workerReporting:
			Threading.Thread.Sleep(50)
	
	private def BackgroundWorker1ProgressChanged(sender as object, e as ProgressChangedEventArgs):
		jobItem = cast(JobItem, e.UserState)
		//video
		if jobItem.Event == JobEvent.VideoEncoding:
			userState = jobItem.VideoEncoder
			self.videoProgressBar.Value = cast(int, (userState.Progress * 100))
			self.videoTimeUsed.Text = userState.TimeUsed.ToString()
			self.videoTimeLeft.Text = userState.TimeLeft.ToString()
		//audio
		elif jobItem.Event == JobEvent.AudioEncoding:
			base3 = jobItem.AudioEncoder
			self.audioProgressBar.Value = cast(int, (base3.Progress * 100))
			self.audioTimeUsed.Text = base3.TimeUsed.ToString()
			self.audioTimeLeft.Text = base3.TimeLeft.ToString()
		//mux
		elif jobItem.Event == JobEvent.Muxing:
			base4 = jobItem.Muxer
			self.muxProgressBar.Value = cast(int, base4.Progress)
			self.muxTimeUsed.Text = base4.TimeUsed.ToString()
			self.muxTimeLeft.Text = base4.TimeLeft.ToString()
		//One job starts
		elif jobItem.Event == JobEvent.OneStart:
			ResetProgress()
			jobItem.UIItem.SubItems[0].Text = "工作中"
			self.startButton.Enabled = false
			i = self._workingJobItems.IndexOf(jobItem)
			self.statusLable.Text = "正在处理第${i+1}个文件，共${self._workingJobItems.Count}个文件"
		//One job done
		elif jobItem.Event == JobEvent.OneDone: 
			jobItem.UIItem.SubItems[0].Text = "完成"
			i = self._workingJobItems.IndexOf(jobItem)
			self.statusLable.Text = "第${i+1}个文件处理完毕，共${self._workingJobItems.Count}个文件"
		//all done
		elif jobItem.Event == JobEvent.AllDone: 
			self.statusLable.Text = "${self._workingJobItems.Count}个文件处理完成"
			self._workingJobItems.Clear()
			self.startButton.Enabled = true
		self._workerReporting = false

	private def EncodeAudio(avsFile as string, destFile as string, config as AudioEncConfigBase):
		
		encoder = NeroAac(avsFile, destFile)
		encoder.Config = config as NeroAacConfig
		_workingJobItem.AudioEncoder = encoder
		_workingJobItem.Event = JobEvent.AudioEncoding
		self.thread = Thread(ThreadStart(encoder.StartEncoding))
		self.thread.Start()
		while 1 != 0:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				break 
			self.backgroundWorker1.ReportProgress(0, _workingJobItem)
			if encoder.Progress >= 1:
				ExclusivelyReporteport(_workingJobItem)
				break 
		self.thread.Join()

	private def EncodeVideo(avsFile as string, destFile as string, config as VideoEncConfigBase):
		encoder = X264(avsFile, destFile)
		self._workingProcess = encoder
		encoder.Config = config as X264Config
		_workingJobItem.VideoEncoder = encoder
		_workingJobItem.Event = JobEvent.VideoEncoding
		self.thread = Thread(ThreadStart(encoder.StartEncoding))
		self.thread.Start()
		while true:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				break 
			self.backgroundWorker1.ReportProgress(0, _workingJobItem)
			if encoder.Progress >= 1:
				ExclusivelyReporteport(_workingJobItem)
				break 
		self.thread.Join()

	private def Mux(video as string, audio as string, dstFile as string, streamMuxer as Muxer):
		box = MP4Box()
		self._workingProcess = box
		box.VideoFile = video
		box.AudioFile = audio
		box.DstFile = dstFile
		_workingJobItem.Muxer = box
		_workingJobItem.Event = JobEvent.Muxing
		self.thread = Thread(ThreadStart(box.StartMuxing))
		self.thread.Start()
		while 1 != 0:
			Thread.Sleep(500)
			if self.backgroundWorker1.CancellationPending:
				break 
			self.backgroundWorker1.ReportProgress(0, _workingJobItem)
			if box.Progress >= 100:
				ExclusivelyReporteport(_workingJobItem)
				break
		self.thread.Join()

	private def ClearButtonClick(sender as object, e as EventArgs):
		self.listView1.Items.Clear()
		self._itemSettingForms.Clear()

	
	private def ContextMenuStrip1Opening(sender as object, e as CancelEventArgs):
		if self.listView1.SelectedItems.Count == 0:
			e.Cancel = true
		elif 1 != 0:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = true
		else:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = false

	
	private def DelButtonClick(sender as object, e as EventArgs):
		for item as ListViewItem in self.listView1.SelectedItems:
			self.listView1.Items.Remove(item)
			self._itemSettingForms.Remove(item)
	
	private def MainFormActivated(sender as object, e as System.EventArgs):
		items = self.listView1.SelectedItems
		if items.Count > 0:
			settingForm as MediaSettingForm = _itemSettingForms[items[0]]
			if settingForm.Changed:
				self.listView1.SelectedItems[0].SubItems[0].Text = "等待"
				settingForm.Changed = false

		if _configForm.chbInputDir.Checked:
			for item as ListViewItem in self.listView1.Items:
				item.SubItems[1].Text = (_itemSettingForms[item] as MediaSettingForm).SourceFile
		else:
			for item as ListViewItem in self.listView1.Items:
				item.SubItems[1].Text = Path.GetFileName((_itemSettingForms[item] as MediaSettingForm).SourceFile)
			
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
		key as ListViewItem = self.listView1.SelectedItems[0]
		(self._itemSettingForms[key] as MediaSettingForm).ShowDialog()

	private def StopButtonClick(sender as object, e as EventArgs):
		try:
			self.backgroundWorker1.CancelAsync()
		except:
			pass
		_userStop = true
		self.ResetProgress() //TODO 修改状态
		_workingJobItem.UIItem.SubItems[0].Text = "中止"
		self._workingJobItems.Clear() //TODO 将_jobItem设为null 将开始新一轮StartWork
		self.startButton.Enabled = true
		self.statusLable.Text = "中止"

		if self._workingProcess is not null:
			self._workingProcess.Stop()

	private def WriteAudioAvs(sourceFile as string, avsFile as string, avsConfig as AvsConfigSection):
		writer = AvisynthWriter(sourceFile)
		audioConfig as AudioScriptConfig = writer.AudioConfig
		audioConfig.DownMix = avsConfig.DownMix
		audioConfig.Normalize = avsConfig.Normalize
		writer.AudioScriptFile = avsFile
		audioConfig.SourceFilter = avsConfig.AudioSource
		writer.WriteAudioScript()

	private def WriteVideoAvs(sourceFile as string, avsFile as string, avsConfig as AvsConfigSection):
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
	
	private def 完成ToolStripMenuItemClick(sender as object, e as EventArgs):
		for item as ListViewItem in self.listView1.SelectedItems:
			item.SubItems[0].Text = '完成'

	
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
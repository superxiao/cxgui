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
import CXGUI.Config
import CXGUI.Job
import DirectShowLib.DES
import My


partial class MainForm(System.Windows.Forms.Form):
	_configForm as ProgramConfigForm
	_jobSettingForm as JobSettingForm
	_programConfig as ProgramConfig
	_workingJobItem as JobItem
	"""仅在StartButtonClick和NextJobOrExist方法中更改"""
	_workingJobItems as Boo.Lang.List[of JobItem]
	"""在StartButtonClick方法中创建，在NextJobOrExist方法中清空。其他位置不允许修改。"""
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
		firstAddedItem as ListViewItem
		for fileName in self.openFileDialog1.FileNames:
			item as ListViewItem = AddNewJobItem(fileName)
			if firstAddedItem == null and item != null:
				firstAddedItem = item
		if firstAddedItem != null:
			self.jobItemListView.SelectedItems.Clear()
			firstAddedItem.Selected = true

	private def AddNewJobItem(filePath as string) as ListViewItem:
		if _configForm.chbInputDir.Checked: //TODO 显示输入路径还是文件名
			fileName = filePath
		else:
			fileName = Path.GetFileName(filePath)
		
		jobItem as JobItem
		addFile = do:
			profile = Profile(self.profileBox.Text)
			ext = ".mp4"
			if profile != null:
				ext = profile.GetExtByMuxer()
				
			if self._configForm.destDirComboBox.Text != "":
				destFile = Path.Combine(self._configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(fileName)+ext)
			else:
				destFile = Path.ChangeExtension(filePath, ext)
			destFile = GetUniqueName(destFile)
			
			jobItem = JobItem(filePath, destFile, self.profileBox.Text)
			self.jobItemListView.Items.Add(jobItem.CxListViewItem)
				
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
		return jobItem.CxListViewItem
			
	private def StartButtonClick(sender as object, e as EventArgs):
		_workingJobItems = GetWorkingJobItems()

		unavailableFiles = ""
		for item in _workingJobItems:
			if not File.Exists(item.SourceFile):
				unavailableFiles += "\n${item.SourceFile}"
				item.State = JobState.Error
				_workingJobItems.Remove(item)
				
		if unavailableFiles != "":
			result = MessageBox.Show("以下媒体文件不存在：${unavailableFiles}\n单击“确定”将处理其他文件。", "错误", MessageBoxButtons.OKCancel,
			MessageBoxIcon.Warning)
			if result == DialogResult.Cancel:
				_workingJobItems.Clear()
				return

		if _workingJobItems.Count > 0:
					
			SetUpJobItems(_workingJobItems.ToArray())
			self._workingJobItem = _workingJobItems[0]
			self.backgroundWorker1.RunWorkerAsync(self._workingJobItem)
			self.tabControl1.SelectTab(self.progressPage)
			

		

	private def ClearButtonClick(sender as object, e as EventArgs):
		self.jobItemListView.Items.Clear()
		self.settingButton.Enabled = false
	
	private def ContextMenuStrip1Opening(sender as object, e as CancelEventArgs):
		if self.jobItemListView.SelectedItems.Count == 0:
			e.Cancel = true
		elif self.jobItemListView.SelectedItems.Count == 1:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = true
			self.listViewMenu.Items['打开目录ToolStripMenuItem'].Enabled = true
		else:
			self.listViewMenu.Items['设置ToolStripMenuItem'].Enabled = false
			self.listViewMenu.Items['打开目录ToolStripMenuItem'].Enabled = false
	
	private def DelButtonClick(sender as object, e as EventArgs):
		for item as ListViewItem in self.jobItemListView.SelectedItems:
			self.jobItemListView.Items.Remove(item)
	
	private def MainFormActivated(sender as object, e as System.EventArgs):
		pass
#		if _configForm.chbInputDir.Checked:
#			for item as ListViewItem in self.jobItemListView.Items:
#				item.SubItems[1].Text = self._jobItems[item].SourceFile
#		else:
#			for item as ListViewItem in self.jobItemListView.Items:
#				item.SubItems[1].Text = Path.GetFileName(self._jobItems[item].SourceFile)
#			
	private def JobItemListViewItemSelectionChanged(sender as object, e as ListViewItemSelectionChangedEventArgs):
		if self.jobItemListView.SelectedItems.Count == 1:
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
		item as CxListViewItem = self.jobItemListView.SelectedItems[0]
		jobItem = item.JobItem
		SetUpJobItems((jobItem,))
		if self._jobSettingForm == null:
			self._jobSettingForm = JobSettingForm()
		self._jobSettingForm.UpdateProfiles(array(string, self.profileBox.Items), jobItem.ProfileName)
		self._jobSettingForm.SetUpFormForItem(jobItem)
		result = self._jobSettingForm.ShowDialog()
		
		if result == DialogResult.OK and self._jobSettingForm.Changed:
			self._jobSettingForm.Changed = false
			jobItem.State = JobState.Waiting
			jobItem.DestFile = self._jobSettingForm.DestFile
			jobItem.AvsConfig = self._jobSettingForm.AvsConfig
			jobItem.VideoEncConfig = self._jobSettingForm.VideoEncConfig
			jobItem.AudioEncConfig = self._jobSettingForm.AudioEncConfig
			jobItem.JobConfig = self._jobSettingForm.JobConfig
			jobItem.SubtitleConfig = self._jobSettingForm.SubtitleConfig
			jobItem.ProfileName = self._jobSettingForm.UsingProfileName
			if jobItem.JobConfig.UseSeparateAudio:
				jobItem.ExternalAudio = self._jobSettingForm.SepAudio
			jobItem.SubtitleFile = self._jobSettingForm.Subtitle
			
		self.UpdateProfileBox(self._jobSettingForm.GetProfiles(), self.profileBox.Text)
		self._jobSettingForm.Clear()
		


	private def UpdateProfileBox(newProfileNames as (string), selectedProfile as string):
		self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged
		self.profileBox.Items.Clear()
		self.profileBox.Items.AddRange(newProfileNames)
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

	private def 等待ToolStripMenuItemClick(sender as object, e as EventArgs):
		for item as CxListViewItem in self.jobItemListView.SelectedItems:
			item.JobItem.State = JobState.Waiting

	
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
	
	private def 打开目录ToolStripMenuItemClick(sender as object, e as System.EventArgs):
		jobItem = (self.jobItemListView.SelectedItems[0] as CxListViewItem).JobItem
		System.Diagnostics.Process.Start("explorer.exe", "/select, " + jobItem.SourceFile)
	
	private def ListView1ItemDrag(sender as object, e as System.Windows.Forms.ItemDragEventArgs):
		self.jobItemListView.DoDragDrop(jobItemListView.SelectedItems, DragDropEffects.Move)
		
	private def ListView1DragEnter(sender as object, e as System.Windows.Forms.DragEventArgs):
		if e.Data.GetDataPresent(ListView.SelectedListViewItemCollection) or e.Data.GetDataPresent(DataFormats.FileDrop):
			e.Effect = DragDropEffects.Move

	private def ListView1DragDrop(sender as object, e as System.Windows.Forms.DragEventArgs):
		if e.Data.GetDataPresent(DataFormats.FileDrop):
			self.jobItemListView.SelectedItems.Clear()
			for path in (e.Data.GetData(DataFormats.FileDrop) as (string)):
				if IO.File.Exists(path):
					addItem = AddNewJobItem(path)
					if addItem != null:
						addItem.Selected = true
				elif IO.Directory.Exists(path):
					for file in IO.Directory.GetFiles(path):
						addItem = AddNewJobItem(file)
						if addItem != null:
							addItem.Selected = true
			return

		dragItems = array(self.jobItemListView.SelectedItems)
		cp as Point = jobItemListView.PointToClient(Point(e.X, e.Y))
		dragToItem = self.jobItemListView.GetItemAt(cp.X, cp.Y)
		if dragToItem == null:
			return
		dragToIndex = dragToItem.Index
		for item as ListViewItem in dragItems:
			self.jobItemListView.Items.Remove(item)
		if self.jobItemListView.Items.Count < dragToIndex:
			dragToIndex = self.jobItemListView.Items.Count
		for item as ListViewItem in dragItems:
			self.jobItemListView.Items.Insert(dragToIndex, item)
			dragToIndex++
	
	private def MainFormLoad(sender as object, e as System.EventArgs):
		self.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), _programConfig.ProfileName)
		jobItems as List[of JobItem]
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
			for jobItem as JobItem in jobItems:
				self.jobItemListView.Items.Add(jobItem.CxListViewItem)
				
			
	private def MainFormFormClosing(sender as object, e as System.Windows.Forms.FormClosingEventArgs):
		if self._workingJobItem != null and self._workingJobItem.State == JobState.Working:
			result = MessageBox.Show("正在工作中，是否中止并退出？", "工作中", MessageBoxButtons.YesNo, 
			MessageBoxIcon.Information)
			if result == DialogResult.Yes:
				StopButtonClick(null, null)
			else:
				e.Cancel = true
				return
		
		jobItems = List[of JobItem]()
		for item as CxListViewItem in self.jobItemListView.Items:
			jobItem = item.JobItem
			jobItem.VideoEncoder = null
			jobItem.AudioEncoder = null
			jobItem.Muxer = null
			jobItems.Add(jobItem)
		formater = BinaryFormatter()
		stream = FileStream("JobItems.bin", FileMode.Create)
		formater.Serialize(stream, jobItems)
		stream.Close()
		SaveProfileSelection()
	
	private def ListView1DoubleClick(sender as object, e as System.EventArgs):
		self.settingButton.PerformClick()

	private def ProfileBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		result = MessageBox.Show("是否应用更改到所有项目？", "预设更改", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
		if result == DialogResult.Yes:
			profile = Profile(self.profileBox.Text)
			ext = ""
			if profile != null:
				ext = profile.GetExtByMuxer()
			for item as CxListViewItem in self.jobItemListView.Items:
				jobItem = item.JobItem
				jobItem.ProfileName = self.profileBox.Text
				if ext != "":
					jobItem.DestFile = Path.ChangeExtension(jobItem.DestFile, ext)
					jobItem.DestFile = GetUniqueName(jobItem.DestFile)

[STAThread]
public def Main(argv as (string)) as void:
	if not Directory.Exists("tools"):
		Directory.CreateDirectory("tools")
	Directory.SetCurrentDirectory("tools")
	Application.EnableVisualStyles()
	Application.SetCompatibleTextRenderingDefault(false)
	Application.Run(MainForm())
#	X264CONFIG2TEST()
	//VideoEncoding.vetest()
	//CXGUI.AudioEncoding.aebtest()
	//CXGUI.GUI.cfgtest()
	//test()
	//CXGUI.StreamMuxer.fftest()
	//CXGUI.StreamMuxer.MKVtest()
	//My.FileModule.MyTest()
	
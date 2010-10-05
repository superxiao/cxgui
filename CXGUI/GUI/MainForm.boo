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
			profile = Profile(self.profileBox.Text)
			ext = ".mp4"
			if profile != null:
				ext = profile.GetExtByMuxer()
				
			if self._configForm.destDirComboBox.Text != "":
				destFile = Path.Combine(self._configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(fileName)+ext)
			else:
				destFile = Path.ChangeExtension(filePath, ext)
			destFile = GetUniqueName(destFile)
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
			
	private def StartButtonClick(sender as object, e as EventArgs):
		_workingItems = GetWorkingJobItems()
		SetUpItems(_workingItems.ToArray())
		if _workingItems.Count > 0:
			self.backgroundWorker1.RunWorkerAsync(_workingItems[0])
			self.tabControl1.SelectTab(self.progressPage)

	private def ClearButtonClick(sender as object, e as EventArgs):
		self.listView1.Items.Clear()
		self._jobItems.Clear()
		self.settingButton.Enabled = false
	
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
			jobItem.DestFile = self._mediaSettingForm.DestFile
			jobItem.UIItem.SubItems[2].Text = jobItem.DestFile
			jobItem.UIItem.SubItems[0].Text = "等待"
			jobItem.AvsConfig = self._mediaSettingForm.AvsConfig
			jobItem.VideoEncConfig = self._mediaSettingForm.VideoEncConfig
			jobItem.AudioEncConfig = self._mediaSettingForm.AudioEncConfig
			jobItem.JobConfig = self._mediaSettingForm.JobConfig
			jobItem.ProfileName = self._mediaSettingForm.UsingProfile
			jobItem.KeepingCfg = true
			if jobItem.JobConfig.UseSeparateAudio:
				jobItem.SeparateAudio = self._mediaSettingForm.SepAudio
			jobItem.Subtitle = self._mediaSettingForm.Subtitle
		else:
			jobItem.Clear()
		UpdateProfileBox(self._mediaSettingForm.GetProfiles(), self.profileBox.Text)
		self._mediaSettingForm.Clear()
		
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
	
	private def 打开目录ToolStripMenuItemClick(sender as object, e as System.EventArgs):
		jobItem = self._jobItems[self.listView1.SelectedItems[0]]
		System.Diagnostics.Process.Start("explorer.exe", "/select, " + jobItem.SourceFile)
	
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
		self.UpdateProfileBox(Profile.GetProfileNames(), _programConfig.ProfileName)
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
		for item in self._jobItems.Values:
			item.VideoEncoder = null
			item.AudioEncoder = null
			item.Muxer = null
		formater = BinaryFormatter()
		stream = FileStream("JobItems.bin", FileMode.Create)
		formater.Serialize(stream, self._jobItems)
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
			for item in self._jobItems.Values:
				item.ProfileName = self.profileBox.Text
				if ext != "":
					item.DestFile = Path.ChangeExtension(item.DestFile, ext)
					item.DestFile = GetUniqueName(item.DestFile)
					item.UIItem.SubItems[2].Text = item.DestFile

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
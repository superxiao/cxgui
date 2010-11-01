namespace CXGUI.GUI

import System
import System.IO
import System.Collections
import System.Drawing
import System.Windows.Forms
import System.Configuration
import CXGUI.Config

partial class ProgramConfigForm:
"""Description of ProgramConfigForm."""
	
	_resetter as ControlResetter
	
	public def constructor(configSection as ProgramConfig):
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		self.destDirComboBox.Text = configSection.DestDir
		self.chbInputDir.Checked = configSection.OmitInputDir
		self.chbSilentRestart.Checked = configSection.SilentRestart
		self.previewPlayerComboBox.Text = configSection.PlayerPath
		self.cbAudioAutoSF.Checked = configSection.AutoChangeAudioSourceFilter

	private def BrowseButtonClick(sender as object, e as System.EventArgs):
		self.folderBrowserDialog1.ShowDialog()
		self.destDirComboBox.Text = self.folderBrowserDialog1.SelectedPath
	
	private def OKButtonClick(sender as object, e as System.EventArgs):
		if not IO.Directory.Exists(self.destDirComboBox.Text):
			_resetter.ResetControls((self.destDirComboBox,))
		if not IO.File.Exists(self.previewPlayerComboBox.Text):
			_resetter.ResetControls((self.previewPlayerComboBox,))
		SaveConfig()
		_resetter.Clear()
		self.Close()
	
	private def SaveConfig():
		//TODO 错误处理
		configSection = ProgramConfig.Get()
		configSection.DestDir = self.destDirComboBox.Text
		configSection.OmitInputDir = self.chbInputDir.Checked
		configSection.SilentRestart = self.chbSilentRestart.Checked
		configSection.PlayerPath = self.previewPlayerComboBox.Text
		configSection.AutoChangeAudioSourceFilter = self.cbAudioAutoSF.Checked
		ProgramConfig.Save()
		
		
		
	private def ProgramConfigFormLoad(sender as object, e as System.EventArgs):
		_resetter = ControlResetter()
		_resetter.SaveControls(self.groupBox1.Controls as IEnumerable)
		_resetter.SaveControls(self.groupBox2.Controls as IEnumerable)
		_resetter.SaveControls(self.editGroupBox.Controls as IEnumerable)
	
	private def CacelButtonClick(sender as object, e as System.EventArgs):
		_resetter.ResetControls()
		_resetter.Clear()
		self.Close()
	
	private def PreviewPlayerButtonClick(sender as object, e as System.EventArgs):
		if File.Exists(self.previewPlayerComboBox.Text):
			self.openFileDialog1.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(self.previewPlayerComboBox.Text))
			self.openFileDialog1.FileName = Path.GetFileName(self.previewPlayerComboBox.Text)
		self.openFileDialog1.ShowDialog()
		self.previewPlayerComboBox.Text = self.openFileDialog1.FileName
		
			


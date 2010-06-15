namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms
import System.Configuration

partial class ProgramConfigForm:
"""Description of ProgramConfigForm."""
	
	_resetter as ControlResetter
	
	public def constructor(configSection as ProgramConfig):
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		self.destDirComboBox.Text = configSection.DestDir
		self.chbInputDir.Checked = configSection.InputDir
		self.chbSilentRestart.Checked = configSection.SilentRestart

	private def BrowseButtonClick(sender as object, e as System.EventArgs):
		self.folderBrowserDialog1.ShowDialog()
		self.destDirComboBox.Text = self.folderBrowserDialog1.SelectedPath
	
	private def OKButtonClick(sender as object, e as System.EventArgs):
		if not IO.Directory.Exists(self.destDirComboBox.Text):
			_resetter.ResetControls((self.destDirComboBox,))
			
		SaveConfig()
		_resetter.Clear()
		self.Close()
	
	private def SaveConfig():
		config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		configSection as ProgramConfig = config.Sections["programConfig"]
		if configSection == null:
			configSection = ProgramConfig()
			config.Sections.Add("programConfig", configSection)
			config.Save()
		configSection.DestDir = self.destDirComboBox.Text
		configSection.InputDir = self.chbInputDir.Checked
		configSection.SilentRestart = self.chbSilentRestart.Checked
		config.Save()
		
		
		
	private def ProgramConfigFormLoad(sender as object, e as System.EventArgs):
		_resetter = ControlResetter()
		_resetter.SaveControls(self.groupBox1.Controls as IEnumerable)
	
	private def CacelButtonClick(sender as object, e as System.EventArgs):
		_resetter.ResetControls()
		_resetter.Clear()
		self.Close()



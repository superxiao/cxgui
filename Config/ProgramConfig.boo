namespace CXGUI.Config

import System
import System.Configuration
import System.Windows.Forms
import Microsoft.Win32

class ProgramConfig(System.Configuration.ConfigurationSection):
"""Description of ProgramConfig"""

	//Fields
	static _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

	private def constructor():
		self.Item["playerPath"] = Registry.GetValue("""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\wmplayer.exe""", "", null)
		self.Item["playerPath"] = "" if self.Item["playerPath"] == null
		
	public static def Get() as ProgramConfig:
	"""获取从硬盘读取本类设置实例，或当设置文件不存在时，获取新建的本类实例，且新建设置文件。"""
		
		configSection as ProgramConfig = self._config.Sections["programConfig"]
		if configSection == null:
			configSection = ProgramConfig()
			self._config.Sections.Add("programConfig", configSection)
			self._config.Save()
		return configSection

	public static def Save():
		self._config.Save()

	[ConfigurationProperty("destDir", DefaultValue : "")]
	public DestDir as string:
	    get:
	    	return self.Item["destDir"]
	    set:
	    	self.Item["destDir"] = value
	    	
	[ConfigurationProperty("silentRestart", DefaultValue : false)]
	public SilentRestart as bool:
	    get:
	    	return self.Item["silentRestart"]
	    set:
	    	self.Item["silentRestart"] = value
	    	
	[ConfigurationProperty("inputDir", DefaultValue : false)]
	public OmitInputDir as bool:
	    get:
	    	return self.Item["inputDir"]
	    set:
	    	self.Item["inputDir"] = value
	    	
	[ConfigurationProperty("profileName", DefaultValue : "")]
	public ProfileName as string:
	    get:
	    	return self.Item["profileName"]
	    set:
	    	self.Item["profileName"] = value
	    	
	[ConfigurationProperty("playerPath", DefaultValue : "")]
	public PlayerPath as string:
	    get:
	    	return self.Item["playerPath"]
	    set:
	    	self.Item["playerPath"] = value
	    	
	[ConfigurationProperty("autoChangeAudioSourceFilter", DefaultValue : true)]
	public AutoChangeAudioSourceFilter as bool:
	    get:
	    	return self.Item["autoChangeAudioSourceFilter"]
	    set:
	    	self.Item["autoChangeAudioSourceFilter"] = value
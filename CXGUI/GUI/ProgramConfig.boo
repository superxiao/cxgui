namespace CXGUI.GUI

import System
import System.Configuration

class ProgramConfig(System.Configuration.ConfigurationSection):
"""Description of ProgramConfig"""
	public def constructor():
		pass
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
	public InputDir as bool:
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
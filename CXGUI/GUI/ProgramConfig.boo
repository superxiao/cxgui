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
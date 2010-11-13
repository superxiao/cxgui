namespace CXGUI.VideoEncoding

import System

class VideoEncConfigBase:
"""Description of VideoEncConfigBase"""
	public def constructor():
		pass


	[Property(CustomCmdLine)]
	_customCmdLine as string

	[Property(UsingCustomCmd)]
	_usingCustomCmd as bool
	
	public abstract def GetArgument() as string:
		pass
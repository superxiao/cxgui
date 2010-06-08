namespace CXGUI.GUI

import System
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding

class Profile:
"""Description of Profile"""
	public def constructor():
		_avsConfig = AvsConfig()
		_videoEncConfig = X264Config()
		_audioEncConfig = NeroAacConfig()
	[Property(AvsConfig)]
	_avsConfig as AvsConfig
	[Property(VideoEncConfig)]
	_videoEncConfig as X264Config
	[Property(AudioEncConfig)]
	_audioEncConfig as NeroAacConfig

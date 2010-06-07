namespace CXGUI.GUI

import System
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding

class Profile:
"""Description of Profile"""
	public def constructor():
		_avsConfig = AvsConfigSection()
		_videoEncConfig = X264Config()
		_audioEncConfig = NeroAacConfig()
	[Property(AvsConfig)]
	_avsConfig as AvsConfigSection
	[Property(VideoEncConfig)]
	_videoEncConfig as X264Config
	[Property(AudioEncConfig)]
	_audioEncConfig as NeroAacConfig

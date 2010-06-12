namespace CXGUI.GUI

import System
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding

class Profile:
"""Description of Profile"""
	public def constructor():
		_avsConfig = AvisynthConfig()
		_videoEncConfig = X264Config()
		_audioEncConfig = NeroAacConfig()
		_jobConfig = JobItemConfig()
	[Property(AvsConfig)]
	_avsConfig as AvisynthConfig
	[Property(VideoEncConfig)]
	_videoEncConfig as X264Config
	[Property(AudioEncConfig)]
	_audioEncConfig as NeroAacConfig
	[Property(JobConfig)]
	_jobConfig as JobItemConfig

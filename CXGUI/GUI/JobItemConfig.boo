namespace CXGUI.GUI

import System
import System.Windows.Forms//test
import System.Configuration
import CXGUI.Avisynth
import CXGUI.StreamMuxer
	
enum JobMode:
	Encode
	Copy
	None

class JobItemConfig:
"""Description of JobItemConfig"""
	public def constructor():
		pass

	[Property(Muxer)]
	_muxer as Muxer
	[Property(VideoMode)]
	_videoMode as JobMode
	[Property(AudioMode)]
	_audioMode as JobMode
	[Property(UseSeparateAudio)]
	_sepAudio as bool
namespace CXGUI.Job

import System
import System.Windows.Forms//test
import System.Configuration
import CXGUI.Avisynth
import CXGUI.StreamMuxer
	
enum StreamProcessMode:
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
	_videoMode as StreamProcessMode
	[Property(AudioMode)]
	_audioMode as StreamProcessMode
	[Property(UseSeparateAudio)]
	_sepAudio as bool
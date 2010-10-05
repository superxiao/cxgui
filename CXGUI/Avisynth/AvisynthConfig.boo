namespace CXGUI.Avisynth

import System

public class AvisynthConfig():

	public def constructor():
		_lockAspectRatio = true
		_lockToSourceAR = true
		_mod = 2
		_downMix = true
		
	[Property(Width)]
	_width as int
	[Property(Height)]
	_height as int
	[Property(AspectRatio)]
	_aspectRatio as double
	[Property(LockAspectRatio)]
	_lockAspectRatio as bool
	[Property(LockToSourceAR)]
	_lockToSourceAR as bool
	[Property(Mod)]
	_mod as int
	[Property(Resizer)]
	_resizer as ResizeFilter
	[Property(VideoSource)]
	_videoSourceFilter as VideoSourceFilter
	[Property(FrameRate)]
	_frameRate as double
	[Property(ConvertFPS)]
	_convertFPS as bool
	[Property(AudioSource)]
	_audioSourceFilter as AudioSourceFilter
	[Property(DownMix)]
	_downMix as bool
	[Property(Normalize)]
	_normalize as bool
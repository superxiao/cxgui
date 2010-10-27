namespace CXGUI.Avisynth

import System

public class AvisynthConfig():
"""
这个类面向GUI，其对象接收GUI的Avisynth设置信息，
并作为AvsWriter对象创建的实参。
此外，作为一般性的设置数据导出和导入，因此不应有特殊性数据，
且应与GUI元素一一对应。
"""

	public def constructor():
		self._lockAspectRatio = true
		self._lockToSourceAR = true
		self._mod = 2
		self._downMix = true
		self._usingSourceFrameRate = true
		self._usingSourceResolution = true
		
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
	
	[Property(UsingSourceResolution)]
	_usingSourceResolution as bool
	
	[Property(Mod)]
	_mod as int
	
	[Property(Resizer)]
	_resizer as ResizeFilter
	
	[Property(VideoSourceFilter)]
	_videoSourceFilter as VideoSourceFilter
	
	[Property(FrameRate)]
	_frameRate as double
	
	[Property(UsingSourceFrameRate)]
	_usingSourceFrameRate as bool
	
	[Property(ConvertFPS)]
	_convertFpsForDS as bool
	
	[Property(AudioSourceFilter)]
	_audioSourceFilter as AudioSourceFilter
	
	[Property(DownMix)]
	_downMix as bool
	
	[Property(Normalize)]
	_normalize as bool
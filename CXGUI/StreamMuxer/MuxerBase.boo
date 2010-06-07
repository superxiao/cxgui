namespace CXGUI.StreamMuxer

import System
import  System.Diagnostics
import My

enum Muxer:
	MP4
	MKV

class MuxerBase(IStopable):
"""Description of Class1"""
	public def constructor():
		pass
	[Property(VideoFile)]
	_videoFile as string
	[Property(AudioFile)]
	_audioFile as string
	[Property(DstFile)]
	_dstFile as string
	[Getter(TimeUsed)]
	_timeUsed as timespan
	[Getter(TimeLeft)]
	_timeLeft as timespan
	[Getter(Progress)]
	_progress as single
	
	_process = Process()
	
	
	private abstract def StartMuxing():
		pass

	abstract def Stop():
		pass
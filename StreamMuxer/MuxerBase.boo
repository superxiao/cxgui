namespace CXGUI.StreamMuxer

import System
import System.Diagnostics
import CXGUI

class MuxerBase(IMediaProcessor):
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
	_progress as int
	[Getter(ProcessingDone)]
	processingDone as bool
	
	_process as Process = Process()
	
	[Property(ErrorOccured)]
	_errOccured as bool	
	
	abstract def Start():
		pass

	abstract def Stop():
		pass
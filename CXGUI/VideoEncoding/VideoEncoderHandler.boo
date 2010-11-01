namespace CXGUI.VideoEncoding

import System
import System.IO
import System.Diagnostics
import System.Windows.Forms
import CXGUI
import CXGUI.External
import My

class VideoEncoderHandler(IMediaProcessor):
"""Description of VideoEncoder"""
	public def constructor(avisynthScriptFile as string, destinationFile as string):
		if not File.Exists(avisynthScriptFile):
			raise FileNotFoundException("", avisynthScriptFile)
		avisynthScriptFile = Path.GetFullPath(avisynthScriptFile)
		using _scriptInfo = AviSynthScriptEnvironment().OpenScriptFile(avisynthScriptFile)
		if not _scriptInfo.HasVideo:
			raise InvalidVideoAvisynthScriptException(avisynthScriptFile)
		_avisynthScriptFile = Path.GetFullPath(avisynthScriptFile)
		_destinationFile = destinationFile
		_scriptFrameRate = cast(double, _scriptInfo.raten) / _scriptInfo.rated
		_totalLength = cast(double, _scriptInfo.num_frames) / _scriptFrameRate
		_totalFrame = _scriptInfo.num_frames
		
	abstract def Start():
		pass	
	abstract def Stop():
		pass
	

	//Fields
	_scriptInfo as AviSynthClip
	_scriptFrameRate as double
	_encodingProcess = Process()
	//Properties
	[Getter(AvisynthScriptFile)]
	_avisynthScriptFile as string
	[Property(DestinationFile)]
	_destinationFile as string
	EncoderPath:
		get:
			return _encoderPath
		set:
			if File.Exists(value):
				_encoderPath = value
			else:
				raise FileNotFoundException(value)
	_encoderPath as string
	[Getter(CurrentPosition)]
	_currentPosition as int
	[Getter(TotalLength)]
	_totalLength as double
	[Getter(CurrentFrame)]
	_currentFrame as int
	[Getter(TotalFrames)]
	_totalFrame as int
	[Getter(CurrentFileSize)]
	_currentFileSize as long
	[Getter(EstimatedFileSize)]
	_estimatedFileSize as long
	[Getter(AvgBitRate)]
	_avgBitRate as double
	[Getter(ProcessingFrameRate)]
	_ProcessingFrameRate as double
	[Getter(TimeUsed)]
	_timeUsed as timespan
	[Getter(TimeLeft)]
	_timeLeft as timespan
	[Getter(Progress)]
	_progress as int
	[Getter(Log)]
	_log as string
	[Getter(ProcessingDone)]
	processingDone as bool
	
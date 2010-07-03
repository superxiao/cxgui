namespace CXGUI.AudioEncoding

import System
import System.IO
import System.Diagnostics
import System.Windows.Forms
import CXGUI
import My
import MeGUI


public abstract class AudioEncoderBase(IEncoder):
	
	// Fields
	protected _avisynthScriptFile as string

	protected _currentFileSize as long

	protected _currentPosition as int

	protected _destinationFile as string

	protected _encoderPath as string

	protected _encodingProcess = Process()

	protected _estimatedFileSize as long

	protected _log as string

	protected _progress as int

	protected _scriptFrameRate as double

	protected _scriptInfo as AviSynthClip

	protected _stantardError as string

	protected _timeLeft as timespan

	protected _timeUsed as timespan

	protected _totalLength as double
	
	_avsEn as AviSynthScriptEnvironment

	
	// Methods
	public def constructor(avisynthScriptFile as string, destinationFile as string):
		if not File.Exists(avisynthScriptFile):
			raise FileNotFoundException(string.Empty, avisynthScriptFile)
		avisynthScriptFile = Path.GetFullPath(avisynthScriptFile)
		using self._scriptInfo = AviSynthScriptEnvironment().OpenScriptFile(avisynthScriptFile)
		if self._scriptInfo.ChannelsCount == 0 or self._scriptInfo.SamplesCount == 0:
			raise InvalidAudioAvisynthScriptException(avisynthScriptFile)
		self._avisynthScriptFile = avisynthScriptFile
		self._destinationFile = destinationFile
		self._totalLength = (cast(double, self._scriptInfo.SamplesCount) / cast(double, self._scriptInfo.AudioSampleRate))
		self._encodingProcess.StartInfo.UseShellExecute = false
		self._encodingProcess.StartInfo.RedirectStandardInput = true
		self._encodingProcess.StartInfo.CreateNoWindow = true

	
	public abstract def Start():
		pass

	public abstract def Stop():
		pass

	
	// Properties
	public AvisynthScriptFile as string:
		get:
			return self._avisynthScriptFile

	
	public CurrentFileSize as long:
		get:
			return self._currentFileSize

	
	public CurrentPosition as int:
		get:
			return self._currentPosition

	
	public DestinationFile as string:
		get:
			return self._destinationFile
		set:
			self._destinationFile = value

	
	public EncoderPath as string:
		get:
			return self._encoderPath
		set:
			if not File.Exists(value):
				raise FileNotFoundException(value)
			self._encoderPath = value

	
	public EstimatedFileSize as long:
		get:
			return self._estimatedFileSize

	
	public Log as string:
		get:
			return self._log

	
	public Progress as int:
		get:
			return self._progress

	
	public StantardError as string:
		get:
			return self._stantardError

	
	public TimeLeft as timespan:
		get:
			return self._timeLeft

	
	public TimeUsed as timespan:
		get:
			return self._timeUsed

	
	public TotalLength as double:
		get:
			return self._totalLength





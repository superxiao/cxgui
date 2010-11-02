namespace CXGUI.AudioEncoding

import System
import System.IO
import System.Diagnostics
import System.Windows.Forms
import CXGUI
import CXGUI.External


public abstract class AudioEncoderHandler(IMediaProcessor):
	
	// Fields
	protected avisynthScriptFile as string

	protected currentFileSize as long

	protected currentPosition as int

	protected destFile as string

	protected encodingProcess as Process

	protected estimatedFileSize as long

	protected log as string

	protected progress as double

	protected scriptInfo as AviSynthClip

	protected stantardError as string

	protected timeLeft as timespan

	protected timeUsed as timespan

	protected length as double //秒
	
	protected processingDone as bool
	

	public def constructor(encoderPath as string, avisynthScriptFile as string, destFile as string):
		if not File.Exists(avisynthScriptFile):
			raise FileNotFoundException(string.Empty, avisynthScriptFile)
		avisynthScriptFile = Path.GetFullPath(avisynthScriptFile)
		using self.scriptInfo = AviSynthScriptEnvironment().OpenScriptFile(avisynthScriptFile)
		if self.scriptInfo.ChannelsCount == 0 or self.scriptInfo.SamplesCount == 0:
			raise InvalidAudioAvisynthScriptException(avisynthScriptFile)
		self.avisynthScriptFile = avisynthScriptFile
		self.destFile = destFile
		self.length = (cast(double, self.scriptInfo.SamplesCount) / cast(double, self.scriptInfo.AudioSampleRate))
		self.encodingProcess = Process()
		self.encodingProcess.StartInfo.UseShellExecute = false
		self.encodingProcess.StartInfo.RedirectStandardInput = true
		self.encodingProcess.StartInfo.CreateNoWindow = true
		if File.Exists(encoderPath):
			encodingProcess.StartInfo.FileName = encoderPath

	// Methods
	public abstract def Start():
		pass

	public abstract def Stop():
		pass

	
	// Properties
	public AvisynthScriptFile as string:
		get:
			return self.avisynthScriptFile

	
	public CurrentFileSize as long:
		get:
			return self.currentFileSize

	
	public CurrentPosition as int:
		get:
			return self.currentPosition

	
	public DestFile as string:
		get:
			return self.destFile
		set:
			self.destFile = value

	
	public EstimatedFileSize as long:
		get:
			return self.estimatedFileSize

	
	public Log as string:
		get:
			return self.log

	
	public Progress as int:
		get:
			return cast(int, self.progress)

	
	public StantardError as string:
		get:
			return self.stantardError

	
	public TimeLeft as timespan:
		get:
			return self.timeLeft

	
	public TimeUsed as timespan:
		get:
			return self.timeUsed

	
	public Length as double:
		get:
			return self.length
			
	public ProcessingDone as bool:
		get:
			return self.processingDone






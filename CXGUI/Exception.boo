namespace CXGUI

import System

class VideoStreamNotFoundException(Exception):
	public def constructor(path as string):
		super("Video stream not found:\n${path}")
		
class AudioStreamNotFoundException(Exception):
	public def constructor(path as string):
		super("Audio stream not found:\n${path}")
		
class InvalidVideoAvisynthScriptException(Exception):
	public def constructor(path as string):
		if [".avs", ".avis"].Contains(IO.Path.GetExtension(path).ToLower()):
			super("Video stream not found from the script. Either its not correctly written, or"+
			" you don't have needed filters installed.\n${path}")
		else:
			super("Not an avisynth script:\n${path}")
			
class InvalidAudioAvisynthScriptException(Exception):
	public def constructor(path as string):
		if [".avs", ".avis"].Contains(IO.Path.GetExtension(path).ToLower()):
			super("Audio stream not found from the script. Either its not correctly written, or"+
			" you don't have needed filters installed.\n${path}")
		else:
			super("Not an avisynth script:\n${path}")
			
class FormatNotSupportedException(Exception):
	public def constructor():
		pass
		
class FFmpegBugException(Exception):
	public def constructor():
		pass	

class ProfileNotFoundException(Exception):
	public def constructor(message as string):
		super(message)

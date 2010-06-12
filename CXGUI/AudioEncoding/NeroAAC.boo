namespace CXGUI.AudioEncoding

import System
import System.IO
import System.Windows.Forms
import System.Runtime.InteropServices
import MeGUI

class NeroAac(AudioEncoderBase):
"""Description of NeroAAC"""

	_startTime as date
	
	public def constructor(avisynthScriptFile as string, destinationFile as string):
		super(avisynthScriptFile, destinationFile)
		if File.Exists("neroAacEnc.exe"):
			_encoderPath = "neroAacEnc.exe"
			_encodingProcess.StartInfo.FileName = _encoderPath
			
	def Start():
		_encodingProcess.StartInfo.Arguments = "${_config.GetSettings()} -if - -of \"${_destinationFile}\""
		_encodingProcess.Start()
		target = _encodingProcess.StandardInput.BaseStream
		WriteWaveHeader().writeHeader(target, _scriptInfo)
		currentSample = 0
		bufferSize = 4096 * _scriptInfo.ChannelsCount * _scriptInfo.BytesPerSample
		buffer as (byte) = array(byte, bufferSize)
		handle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
		address as IntPtr = handle.AddrOfPinnedObject()
		_startTime = date.Now
		try:
			using self._scriptInfo = AviSynthScriptEnvironment().OpenScriptFile(self._avisynthScriptFile):
				while true:
					samplesAmount = Math.Min(cast(int, (_scriptInfo.SamplesCount - currentSample)), 4096)
					_scriptInfo.ReadAudio(address, currentSample, samplesAmount)
					target.Write(buffer, 0, samplesAmount * _scriptInfo.ChannelsCount * _scriptInfo.BytesPerSample)
					target.Flush()
					currentSample += samplesAmount
					UpdateProgress(currentSample)
					if currentSample == _scriptInfo.SamplesCount:
						break
				target.Write(buffer, 0, 10)
		ensure:
			handle.Free()
		_encodingProcess.WaitForExit()

	def UpdateProgress(currentSample as int):
		_currentPosition = currentSample / _scriptInfo.AudioSampleRate //s
		_currentFileSize = currentSample * _scriptInfo.BytesPerSample //bytes
		_progress = cast(double, currentSample) / _scriptInfo.SamplesCount * 100 //per
		_estimatedFileSize = cast(double, _currentFileSize) * 100 / _progress if _progress != 0  //bytes
		_timeUsed = date.Now - _startTime
		_timeLeft = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds * (100.0-_progress) / _progress)) if _progress != 0 //s 
		_timeUsed = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds))
	def Stop():	
		try:
			_encodingProcess.Kill()
		except:
			pass
	//Properties	
	[Property(Config)]
	_config as NeroAacConfig



def aebtest():
	t = NeroAac("""C:\Users\clinky\Documents\SharpDevelop Projects\CXGUI\CXGUI\bin\Debug\audio.avs""", "c:\\test.mp4")
	t.Start()
#	using w = StreamWriter("test.txt"):
#		w.WriteLine("Hello there!")
#		a = 1
	
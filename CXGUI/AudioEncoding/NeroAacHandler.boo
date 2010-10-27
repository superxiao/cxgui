namespace CXGUI.AudioEncoding

import System
import System.Runtime.InteropServices
import CXGUI.External

class NeroAacHandler(AudioEncoderHandler):
"""Description of NeroAAC"""

	//Fields
	_startTime as date
	
	_config as NeroAacConfig
	
	public def constructor(avisynthScriptFile as string, destFile as string):
		super("neroAacEnc.exe", avisynthScriptFile, destFile)

	//Methods		
	public def Start():
		self.encodingProcess.StartInfo.Arguments = "${self._config.GetSettings()} -if - -of \"${self.destFile}\""
		self.encodingProcess.Start()
		target = self.encodingProcess.StandardInput.BaseStream
		WriteWavHeader.Write(target, self.scriptInfo)
		currentSample = 0
		bufferSize = 4096 * self.scriptInfo.ChannelsCount * self.scriptInfo.BytesPerSample
		buffer as (byte) = array(byte, bufferSize)
		handle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
		address as IntPtr = handle.AddrOfPinnedObject()
		self._startTime = date.Now
		try:
			using self.scriptInfo = AviSynthScriptEnvironment().OpenScriptFile(self.avisynthScriptFile):
				while true:
					samplesAmount = Math.Min(cast(int, (self.scriptInfo.SamplesCount - currentSample)), 4096)
					self.scriptInfo.ReadAudio(address, currentSample, samplesAmount)
					target.Write(buffer, 0, samplesAmount * self.scriptInfo.ChannelsCount * self.scriptInfo.BytesPerSample)
					target.Flush()
					currentSample += samplesAmount
					self.UpdateProgress(currentSample)
					if currentSample == self.scriptInfo.SamplesCount:
						break
				target.Write(buffer, 0, 10)
		except:
			pass		
		ensure:
			handle.Free()
			target.Flush()
			target.Close()
		encodingProcess.WaitForExit()

	private def UpdateProgress(currentSample as int):
		self.currentPosition = currentSample / self.scriptInfo.AudioSampleRate //s
		self.currentFileSize = currentSample * self.scriptInfo.BytesPerSample //bytes
		self.progress = cast(double, currentSample) / self.scriptInfo.SamplesCount * 100 //per
		self.estimatedFileSize = self.currentFileSize / self.progress * 100 if self.progress != 0  //bytes
		self.timeUsed = date.Now - self._startTime
		self.timeLeft = timespan.FromSeconds(cast(int, self.timeUsed.TotalSeconds * (100.0 - self.progress) / self.progress)) if self.progress != 0
		self.timeUsed = timespan.FromSeconds(cast(int, timeUsed.TotalSeconds))

	public def Stop():	
		try:
			self.encodingProcess.Kill()
			self.encodingProcess.WaitForExit()
		except:
			pass
			
	//Properties
	Config as NeroAacConfig:
		get:
			return _config
		set:
			_config = value
			


	
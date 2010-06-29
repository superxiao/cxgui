namespace CXGUI.StreamMuxer

import System
import System.IO
import System.Threading
import System.Windows.Forms
import CXGUI

class MKVMerge(MuxerBase):
"""Description of MKVMerge"""
	
	_startTime as date


	public def constructor():
		_process.StartInfo.FileName = "mkvmerge.exe"
	
	
	def Start():
		if self._videoFile == self._dstFile:
			tempFile =Path.Combine(Path.GetDirectoryName(self._videoFile),"temp"+Path.GetExtension(self._videoFile))
			File.Move(self._videoFile, tempFile)
			self._videoFile = tempFile
			
			
		elif self._audioFile == self._dstFile:
			tempFile =Path.Combine(Path.GetDirectoryName(self._audioFile),"temp"+Path.GetExtension(self._audioFile))
			File.Move(self._audioFile, tempFile)
			self._audioFile = tempFile
		
		vInfo = VideoInfo(self._videoFile)
		aInfo = AudioInfo(self._audioFile)
		argument = GetArgument(vInfo as VideoInfo, aInfo as AudioInfo)
		_process.StartInfo.Arguments = argument
		_process.StartInfo.UseShellExecute = false
		_process.StartInfo.RedirectStandardOutput = true
		_process.StartInfo.CreateNoWindow = true
		_startTime = date.Now
		_process.Start()
		ReadStdErr()
		_process.WaitForExit()
		File.Delete(tempFile)
		
	private def GetArgument(vInfo as VideoInfo, aInfo as AudioInfo) as string:
		if vInfo.HasVideo and aInfo.StreamsCount:
			argument = "-o \"${_dstFile}\" -A -d ${vInfo.ID} \"${_videoFile}\" -D -a ${aInfo.ID} \"${_audioFile}\""
		elif not vInfo.HasVideo:
			argument = "-o \"${_dstFile}\" -D -a ${aInfo.ID} \"${_audioFile}\""
		elif not aInfo.StreamsCount:
			argument = "-o \"${_dstFile}\" -A -d ${vInfo.ID} \"${_videoFile}\""
		
		return argument

	private def ReadStdErr():
		sr = _process.StandardOutput
		line = ""
		while true:
			if _progress == 100:
				break
			line = sr.ReadLine()
			line = "" if line == null
			if line.Length:
#				File.AppendAllText("c:\\test.txt", line+'\r\n')
				UpdateProgress(line)

	private def UpdateProgress(line as string):
		if line.StartsWith("Progress"):
			_progress = int.Parse(line[10:-1])
		
		_timeUsed = date.Now - _startTime	
		_timeLeft = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds * (100-_progress) / _progress)) if _progress != 0
		_timeUsed = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds))

	def Stop():
		try:
			_process.Kill()
			_process.WaitForExit()
		except:
			pass


def MKVtest():
	m = MKVMerge()
	m.VideoFile = """G:\Movie\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD-SAMPLE.mkv"""
	m.AudioFile = """G:\Movie\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD-SAMPLE.mkv"""
	m.DstFile = """d:\test.mp4"""
	m.Start()
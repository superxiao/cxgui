namespace CXGUI.StreamMuxer

import System
import System.IO
import System.Threading
import System.Windows.Forms
import CXGUI

class FFMP4(MuxerBase):
"""Description of FFMP4"""
	
	_startTime as date


	public def constructor():
		_process.StartInfo.FileName = "ffmpeg.exe"
	
	
	def Start():
		vInfo = VideoInfo(self._videoFile)
		aInfo = AudioInfo(self._audioFile)
		argument = GetArgument(vInfo as VideoInfo, aInfo as AudioInfo)
		_process.StartInfo.Arguments = argument
		_process.StartInfo.UseShellExecute = false
		_process.StartInfo.RedirectStandardError = true
		_process.StartInfo.CreateNoWindow = true
		_startTime = date.Now
		if vInfo.HasVideo:
			length = vInfo.Length
		elif aInfo.StreamsCount:
			length = aInfo.Length
		_process.Start()
		ReadStdErr(length)
		_process.WaitForExit()
		
	private def GetArgument(vInfo as VideoInfo, aInfo as AudioInfo) as string:
		if vInfo.HasVideo and aInfo.StreamsCount:
			argument = "-i \"${_videoFile}\" -i \"${_audioFile}\""+\
		" -y -vcodec copy -acodec copy -sn \"${_dstFile}\" -map 0.${vInfo.ID} -map 1.${aInfo.ID}"
		elif not vInfo.HasVideo:
			argument = "-i \"${_audioFile}\""+\
		" -y -vn -acodec copy -sn \"${_dstFile}\""
		elif not aInfo.StreamsCount:
			argument = "-i \"${_videoFile}\""+\
		" -y -vcodec copy -an -sn \"${_dstFile}\""
		
		return argument

	private def ReadStdErr(length as double):
		sr = _process.StandardError
		line = ""
		while true:
			if _progress == 100:
				break
			line = sr.ReadLine()
			line = "" if line == null
			if line.Length:
#				File.AppendAllText("d:\\test.txt", line+'\r\n')
				UpdateProgress(line, length)

	private def UpdateProgress(line as string, length as double):
		if line.Contains("time="):
			timeString = line[line.IndexOf("time="):line.IndexOf("bitrate=")]
			currentTime = double.Parse(timeString[5:])
			_progress = currentTime * 100 / length
		elif line.Contains("incorrect codec"):
			raise FormatNotSupportedException()
		elif line.Contains("non monotone"):
			raise FFmpegBugException()
		elif line.StartsWith("video:"):
			_progress = 100
		
		_timeUsed = date.Now - _startTime	
		_timeLeft = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds * (100-_progress) / _progress)) if _progress != 0
		_timeUsed = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds))

	def Stop():
		try:
			_process.Kill()
		except:
			pass


def fftest():
	m = FFMP4()
	m.VideoFile = """G:\Movie\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD-SAMPLE.mkv"""
	m.AudioFile = """G:\Movie\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD\Cashback.2006.720p.HDTV.AC3.5.1.x264-XSHD-SAMPLE.mkv"""
	m.DstFile = """d:\test.mp4"""
	m.Start()
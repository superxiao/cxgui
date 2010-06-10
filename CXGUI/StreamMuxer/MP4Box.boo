namespace CXGUI.StreamMuxer

import System
import System.Threading
import System.Windows.Forms

class MP4Box(MuxerBase):
"""Description of MP4Box"""
	public def constructor():
		_process.StartInfo.FileName = "MP4box.exe"
	
	_startTime as date
	
	def Start():
		_process.StartInfo.Arguments = "-add \"${_audioFile}\"#1 \"${_videoFile}\""
		_process.StartInfo.UseShellExecute = false
		_process.StartInfo.RedirectStandardOutput = true
		_process.StartInfo.CreateNoWindow = true
		readThread = Thread(ThreadStart(ReadStdErr))
		_startTime = date.Now
		_process.Start()
		readThread.Start()
		while true:
			if _progress >= 99:
				_progress = 100
				_timeLeft = timespan(0)
				break
			Threading.Thread.Sleep(250)
		readThread.Abort()
		_process.WaitForExit()
	
	def ReadStdErr():
		sr = _process.StandardOutput
		line = ""
		while true:
			line = sr.ReadLine()
			line = "" if line == null
			UpdateProgress(line)
				
			
	def UpdateProgress(line as string):
		_timeUsed = date.Now - _startTime
		_timeLeft = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds * (100-_progress) / 100))
		_timeUsed = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds))
		if line.Contains("Writing"):
			m = line.LastIndexOf(char('('))
			n = line.LastIndexOf(char('/'))
			_progress = int.Parse(line[m+1:n])

	def Stop():
		try:
			_process.Kill()
		except:
			pass
def test():
	a = MP4Box()
	a.VideoFile = "c:\\12.mp4"
	a.AudioFile = "c:\\test.mp4"
	a.Start()

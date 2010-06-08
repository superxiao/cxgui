namespace CXGUI.VideoEncoding

import System
import System.IO
import System.Threading
import CXGUI
import System.Windows.Forms

class X264(VideoEncoderBase):
"""Description of X264"""
	_startTime as date
	_errOccured = false
	public def constructor(avisynthScriptFile as string, destinationFile as string):
		super(avisynthScriptFile, destinationFile)
		_encoderPath = "x264.exe"
		_encodingProcess.StartInfo.FileName = _encoderPath
	def StartEncoding():
		_encodingProcess.StartInfo.Arguments = "${_config.GetSettings()} --output \"${_destinationFile}\" \"${_avisynthScriptFile}\""
		IO.File.WriteAllText("C:\\TEST.TXT", _encodingProcess.StartInfo.Arguments)
		_encodingProcess.StartInfo.UseShellExecute = false
		_encodingProcess.StartInfo.RedirectStandardError = true
		_encodingProcess.StartInfo.CreateNoWindow = true
		readThread = Thread(ThreadStart(ReadStdErr))
		_startTime = date.Now
		_encodingProcess.Start()
		readThread.Start()
		_encodingProcess.WaitForExit()
		if _errOccured:
			raise InvalidVideoAvisynthScriptException(_avisynthScriptFile)
		else:
			count = 0
			while true:
				if _progress >= 0.99:
					_progress = 1
					_timeLeft = timespan(0)
					readThread.Abort()
					break
				elif count <= 10:	
					count += 1
					Threading.Thread.Sleep(250)
				else:
					readThread.Abort()
					_timeLeft = timespan(0)
					break

	def ReadStdErr():
		sr = _encodingProcess.StandardError
		_line = ""
		while true:
			_line = sr.ReadLine()
			_line = "" if _line == null
			if _line.StartsWith("["):
				UpdateProgress(_line)
			elif _line.Contains("error"):
				_errOccured = true
			
	def UpdateProgress(line as string):
		info = line.Split(char(','))
		frame = info[0]
		_currentFrame = int.Parse(frame[frame.IndexOf("]")+1:frame.IndexOf("/")])
		_progress = double.Parse(frame[frame.IndexOf("[")+1:frame.IndexOf("%")]) / 100
		bitRate = info[2]
		_currentPosition = cast(double, _currentFrame) / _scriptFrameRate
		double.TryParse(bitRate[:bitRate.IndexOf("k")], _avgBitRate)
		_currentFileSize = _avgBitRate * _currentPosition / 8
		_estimatedFileSize = _avgBitRate * _totalLength / 8
		pFrameRate = info[1]
		_ProcessingFrameRate = double.Parse(pFrameRate[:pFrameRate.IndexOf("f")])
		timeLeft = info[3]
		_timeLeft = timespan.Parse(timeLeft[timeLeft.IndexOf('a')+1:])
		_timeUsed = date.Now - _startTime
		_timeUsed = timespan.FromSeconds(cast(int, _timeUsed.TotalSeconds))


	def Stop():
		try:
			_encodingProcess.Kill()
		except:
			pass
	//Properties
	[Property(Config)]
	_config = X264Config()
	
	
public def vetest():
	t = X264("""C:\Users\Public\Videos\Sample Videos\Wildlife.avs""", """c:\.mp4""")
	t.StartEncoding()


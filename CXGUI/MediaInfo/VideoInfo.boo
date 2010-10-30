namespace CXGUI

import System
import System.IO
import System.Windows.Forms
import CXGUI.External
import System.Drawing


public class VideoInfo:

	// Fields
	protected _audioStreamsCount as int

	protected _displayAspectRatio as double

	protected _filePath as string

	protected _format as string

	protected _frameRate as double

	protected _hasVideo as bool

	protected _height as int

	protected _width as int

	protected _streamID as int
	
	protected _frameCount as int
	
	// Methods
	public def constructor(path as string):
		self.InitializeProperties(path)

	private def InitializeProperties(path as string):
		if Path.GetExtension(path).ToLower() == ".avs":
			self.AvisynthInfo(path)
			return
		info = MediaInfo()
		info.Open(path)
		self._filePath = path
		if info.Count_Get(StreamKind.Video) > 0:
			self._hasVideo = true
			self._format = info.Get(StreamKind.Video, 0, 'Format')
			audioID = info.Get(StreamKind.Audio, 0, "ID")
			videoID = info.Get(StreamKind.Video, 0, "ID")
			if audioID == "0" or videoID == "0":
				_id = 0
			elif audioID == "1" or videoID == "1":
				_id = 1
			double.TryParse(info.Get(StreamKind.Video, 0, "Duration"), _length)
			_length = _length / 1000
			int.TryParse(info.Get(StreamKind.Video, 0, 'ID', InfoKind.Text), self._streamID)
			_streamID = _streamID - _id if _streamID > 0
			int.TryParse(info.Get(StreamKind.Video, 0, 'Width', InfoKind.Text), self._width)
			int.TryParse(info.Get(StreamKind.Video, 0, 'Height', InfoKind.Text), self._height)
			double.TryParse(info.Get(StreamKind.Video, 0, 'FrameRate', InfoKind.Text), self._frameRate)
			int.TryParse(info.Get(StreamKind.Video, 0, 'FrameCount', InfoKind.Text), self._frameCount)
			s as string = info.Get(StreamKind.Video, 0, 'DisplayAspectRatio/String', InfoKind.Text)
			if s.IndexOf(':') != -1:
				self._displayAspectRatio = (double.Parse(s.Split(char(':'))[0]) / (double.Parse(s.Split(char(':'))[1])))
			if self._width != 0 and self._displayAspectRatio == 0:
				self._displayAspectRatio = (cast(double, self._width) / cast(double, self._height))
		else:
			self._hasVideo = false

		self._audioStreamsCount = info.Count_Get(StreamKind.Audio)
		info.Close()
		
	private def AvisynthInfo(path as string):
		using info = AviSynthScriptEnvironment().OpenScriptFile(path)
		self._hasVideo = info.HasVideo
		self._filePath = path
		if info.HasVideo:
			if info.ChannelsCount:
				self._audioStreamsCount = 1
			else:
				self._audioStreamsCount = 0
			self._width = info.VideoWidth
			self._height = info.VideoHeight
			self._displayAspectRatio = (cast(double, self._width) / self._height)
			self._frameRate = Math.Round(cast(double, info.raten) / info.rated, 3 , MidpointRounding.AwayFromZero)
			self._frameCount = info.num_frames
			self._streamID = 0
			self._length = cast(double, info.num_frames) / self._frameRate
			self._id = 0
		else:
			self._hasVideo = false
		if info.HasVideo or info.ChannelsCount:
			self._format = "avs"
		
	public static def GetVideoInfo(path as string, *videoParameters as (string)) as Hash:
		hash = Hash()
		index = 0
		strArray as (string) = videoParameters
		length as int = strArray.Length
		while index < length:
			videoInfo as string = GetVideoInfo(path, strArray[index])
			hash.Add(strArray[index], videoInfo)
			index += 1
		return hash

	
	public static def GetVideoInfo(path as string, videoParameter as string) as string:
		info = MediaInfo()
		info.Open(path)
		str as string = info.Get(StreamKind.Video, 0, videoParameter, InfoKind.Text)
		info.Close()
		return str
		
		
		

	


	
	// Properties
	public AudioStreamsCount as int:
		get:
			return self._audioStreamsCount

	
	public DisplayAspectRatio as double:
		get:
			return self._displayAspectRatio

	
	public FilePath as string:
		get:
			return self._filePath

	
	public Format as string:
		get:
			return self._format

	
	public FrameRate as double:
		get:
			return self._frameRate

	
	public HasVideo as bool:
		get:
			return self._hasVideo

	
	public Height as int:
		get:
			return self._height

	
	public Width as int:
		get:
			return self._width	

	public StreamID as int:
	"""
	流序号。仅用于FFMPEG MP4合成。
	"""
		get:
			return self._streamID

	public FrameCount as int:
		get:
			return self._frameCount

	[Getter(Length)]
	_length as double
	"""
	秒。
	"""
	
	[Getter(ID)]
	_id as int
	"""
	流ID，mkv和mp4从1起，其他多从0起。
	"""
	
	
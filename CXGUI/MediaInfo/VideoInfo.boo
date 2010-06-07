namespace CXGUI

import System
import MediaInfoLib


[Serializable]
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

	
	// Methods
	public def constructor(path as string):
		self.InitializeProperties(path)

	
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
		info = MediaInfoLib.MediaInfo()
		info.Open(path)
		str as string = info.Get(StreamKind.Video, 0, videoParameter, InfoKind.Text)
		info.Close()
		return str

	
	private def InitializeProperties(path as string):
		info = MediaInfoLib.MediaInfo()
		info.Open(path)
		self._filePath = path
		if info.Count_Get(StreamKind.Video) != 0:
			self._hasVideo = true
		if self._hasVideo:
			self._format = info.Get(StreamKind.Video, 0, 'Format')
			int.TryParse(info.Get(StreamKind.Video, 0, 'Width', InfoKind.Text), self._width)
			int.TryParse(info.Get(StreamKind.Video, 0, 'Height', InfoKind.Text), self._height)
			double.TryParse(info.Get(StreamKind.Video, 0, 'FrameRate', InfoKind.Text), self._frameRate)
			s as string = info.Get(StreamKind.Video, 0, 'DisplayAspectRatio/String', InfoKind.Text)
			if s.IndexOf(':') != -1:
				self._displayAspectRatio = (double.Parse(s.Split(char(':'))[0]) / (double.Parse(s.Split(char(':'))[1])))
			if self._width != 0 and self._displayAspectRatio == 0:
				self._displayAspectRatio = (cast(double, self._width) / cast(double, self._height))
		self._audioStreamsCount = info.Count_Get(StreamKind.Audio)
		info.Close()

	
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





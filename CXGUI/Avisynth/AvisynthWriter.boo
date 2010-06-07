namespace CXGUI.Avisynth

import System
import System.Windows.Forms//test
import System.IO
import CXGUI

class AvisynthWriter():
"""
编写视频或音频的Avisynth脚本。
Remarks: 使用VideoConfig和AudioConfig进行脚本内容的设置。
"""
	public def constructor(sourceFile as string):
	"""
	建立AvisynthWriter类的对象。
	Param path: 源媒体文件路径。
	Raises FileNotFoundException: 无效路径，或文件不存在。
	"""
		InitializeProperties(sourceFile)
	private def InitializeProperties(sourceFile as string):
		if not File.Exists(sourceFile):
			raise IO.FileNotFoundException(sourceFile)
		_sourceFile = sourceFile
		_videoScriptFile = "video.avs"
		_audioScriptFile = "audio.avs"
		_videoInfo = VideoInfo(sourceFile)
		_audioInfo = AudioInfo(sourceFile)
		_hasVideo = _videoInfo.HasVideo
		_audioStreamsCount = _videoInfo.AudioStreamsCount
		_videoConfig = VideoScriptConfig(_videoInfo)
		_audioConfig = AudioScriptConfig(_audioInfo)
	//Methods
	def WriteVideoScript():
	"""
	创建视频脚本。
	Raises VideoStreamNotFoundException: 找不到视频流。
	"""
		File.WriteAllText(_videoScriptFile, _videoConfig.GetScriptContent(), Text.Encoding.Default)
		
	def WriteAudioScript():
	"""
	创建音频脚本。
	Raises AudioStreamNotFoundException: 找不到音频流。
	"""
		File.WriteAllText(_audioScriptFile, _audioConfig.GetScriptContent(), Text.Encoding.Default)
	//Properties
	SourceFile as string:
	"""
	源媒体文件路径。
	Raises FileNotFoundException: 无效路径，或文件不存在。
	"""
		get:
			return _sourceFile
		set:
			InitializeProperties(value)
			_sourceFile = value
	_sourceFile as string
	[Getter(HasVideo)]
	_hasVideo as bool
	"""媒体文件是否包含视频流。"""
	[Getter(AudioStreamsCount)]
	_audioStreamsCount as int
	"""媒体文件包含音频流的数量。"""
	[Property(VideoScriptFile)]
	_videoScriptFile as string
	"""将被创建的视频脚本的路径。"""
	[Property(AudioScriptFile)]
	_audioScriptFile as string
	"""将被创建的音频脚本的路径。"""
	[Getter(VideoInfo)]
	_videoInfo as VideoInfo
	"""视频信息。"""
	[Getter(AudioInfo)]
	_audioInfo as AudioInfo
	"""音频信息。"""
	VideoConfig as VideoScriptConfig:
	"""
	视频脚本设置。
	Raises VideoStreamNotFoundException: 没有视频流时，本属性禁止访问。
	"""
		get:
			if not _hasVideo:
				raise VideoStreamNotFoundException(_sourceFile)
			return _videoConfig
		set:
			_videoConfig = value
	_videoConfig as VideoScriptConfig		
	AudioConfig as AudioScriptConfig:
	"""
	音频脚本设置。
	Raises AudioStreamNotFoundException: 没有音频时，本属性禁止访问。
	"""
		get:
			if not _audioStreamsCount:
				raise AudioStreamNotFoundException(_sourceFile)
			return _audioConfig
		set:
			_audioConfig = value
	_audioConfig as AudioScriptConfig



def avstest():
	i = AvisynthWriter("""G:\Movie\The.Princess.and.the.Frog.720p.Bluray.x264-CBGB\cbgb-princessfrog.mkv""")
	i.VideoConfig.Width = 30
	MessageBox.Show(i.VideoConfig.GetScriptContent())
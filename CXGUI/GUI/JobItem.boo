namespace CXGUI.GUI

import System
import System.IO
import System.Windows.Forms
import System.Runtime.Serialization.Formatters.Binary
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer

enum JobState:
	Waiting
	Working
	Done
	Stop
	Error

enum JobEvent:
	None
	VideoEncoding
	AudioEncoding
	Muxing
	OneStart
	OneDone
	AllDone
	Stop
	
[Serializable()]
class JobItem:
"""Description of JobItem"""
	[Getter(SourceFile)]
	_sourceFile as string
	[Getter(DestFile)]
	_destFile as string
	[Property(AvsConfig)]
	_avsConfig as AvsConfig
	[Property(DoVideo)]
	_writeVideoScript as bool
	[Property(DoAudio)]
	_writeAudioScript as bool
	[Property(VideoEncConfig)]
	_videoEncConfig as VideoEncConfigBase
	[Property(AudioEncConfig)]
	_audioEncConfig as AudioEncConfigBase
	[Property(VideoEncoder)]
	_videoEncoder as VideoEncoderBase
	[Property(AudioEncoder)]
	_audioEncoder as AudioEncoderBase
	[Property(Muxer)]
	_muxer as MuxerBase
	[Property(State)]
	_state as JobState
	[Property(Event)]
	_event as JobEvent
	[Property(UIItem)]
	_uiItem as ListViewItem
	
	public def constructor(sourceFile as string, destFile as string, uiItem as ListViewItem):
		self._sourceFile = sourceFile
		self._destFile = destFile
		self._uiItem = uiItem
		ReadProfile("default.profile")
	
	private def ReadProfile(path as string):
	"""
	从profile文件中读取VideoEncConfig AudioEncConfig AvsConfig对象到本类的相关属性。
	"""
		formater = BinaryFormatter()
		CreatNewProfile = do():
			profile = Profile()
			ReadProfile(profile)
			stream = FileStream(path, FileMode.Create)
			formater.Serialize(stream, profile)
			stream.Close()
		if not File.Exists(path):
			CreatNewProfile()
		else:
			try:
				stream = FileStream(path, FileMode.Open)
				profile = formater.Deserialize(stream) as Profile
				ReadProfile(profile)
				stream.Close()
			except:
				stream.Close()
				CreatNewProfile()

	private def ReadProfile(profile as Profile):
			_videoEncConfig = profile.VideoEncConfig
			_audioEncConfig = profile.AudioEncConfig
			_avsConfig = profile.AvsConfig
			videoInfo = VideoInfo(_sourceFile)
			if videoInfo.HasVideo:
				self._writeVideoScript = true
			if videoInfo.AudioStreamsCount:
				self._writeAudioScript = true
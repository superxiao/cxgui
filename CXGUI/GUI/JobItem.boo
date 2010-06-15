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

	[Property(DestFile)]
	_destFile as string

	[Property(AvsConfig)]
	_avsConfig as AvisynthConfig

	[Property(VideoEncConfig)]
	_videoEncConfig as VideoEncConfigBase

	[Property(AudioEncConfig)]
	_audioEncConfig as AudioEncConfigBase

	JobConfig as JobItemConfig:
		get:
			return _jobConfig
		set:
			if (not _videoInfo.AudioStreamsCount and not value.AudioMode == JobMode.None)\
				or (not _videoInfo.HasVideo and not value.VideoMode == JobMode.None):
					raise ArgumentException("Incorrect JobMode.")
			_jobConfig = value
	_jobConfig as JobItemConfig

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

	[Property(KeepingCfg)]
	_KeepingCfg as bool

	[Property(SeparateAudio)]
	_separateAudio as string

	[Property(ProfileName)]
	_profileName as string

	[Property(UsingCustomCfgInsteadOfProfile)]
	_usingCustomCfgInsteadOfProfile as bool

	_readAvsCfg as bool
	_readVideoCfg as bool
	_readAudioCfg as bool
	_readJobCfg as bool
	_videoInfo as VideoInfo
	
	public def constructor(sourceFile as string, destFile as string, uiItem as ListViewItem, profileName as string):
		self._sourceFile = sourceFile
		self._destFile = destFile
		self._uiItem = uiItem
		self._profileName = profileName
		_videoInfo = VideoInfo(sourceFile)
	
	public def SetUp():
	"""
	根据JobItem对象的ProfileName属性为其读取各设置实例。
	如对应Profile文件不存在，则报错。
	"""
		if self._avsConfig == null:
			_readAvsCfg = true
		if self._videoEncConfig == null:
			_readVideoCfg = true
		if self._audioEncConfig == null:
			_readAudioCfg = true
		if self.JobConfig == null:
			_readJobCfg = true
		ReadProfile(self._profileName)

	private def ReadProfile(profileName as string):
	"""
	从profile文件中读取VideoEncConfig AudioEncConfig AvsConfig对象到本类的相关属性。
	"""
		if _readAvsCfg or _readVideoCfg or _readAudioCfg or _readJobCfg:
			profile = Profile(profileName)
		if self._readAvsCfg:
			_avsConfig = profile.AvsConfig
			_readAvsCfg = false
		if self._readVideoCfg:
			_videoEncConfig = profile.VideoEncConfig
			_readVideoCfg = false
		if self._readAudioCfg:
			_audioEncConfig = profile.AudioEncConfig
			_readAudioCfg = false
		if self._readJobCfg:
			_jobConfig = profile.JobConfig
			_readJobCfg = false

	
	public def Clear():
	"""
	清理内部存储设置数据的对象。
	如果KeepingCfg属性设置为true, 则不执行任何操作。
	"""
		if not self._KeepingCfg:
			self._avsConfig = null
			self._audioEncConfig = null
			self._videoEncConfig = null
			self._jobConfig = null
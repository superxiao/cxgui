namespace CXGUI.Job

import System
import System.Windows.Forms
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer
import CXGUI.Config

enum JobState:
	//工作队列中，等待编码
	Waiting
	Working
	//不在工作队列
	NotProccessed
	Done
	Stop
	Error

enum JobEvent:
	None
	VideoEncoding
	AudioEncoding
	Muxing
	OneJobItemProcessing
	OneJobItemDone
	AllDone
	OneJobItemCancelled
	QuitAllProcessing
	Error

[Serializable()]
class JobItem():
"""]
创建一个JobItem实例，将附送一个CxListViewItem实例，可添加到ListView中。
更改JobItem的源文件、目标文件、工作状态，CxListViewItem相应自动更改。
"""
	
	
	_sourceFile as string
	SourceFile as string:
		get:
			return _sourceFile
		set:
			_sourceFile = value
			_cxListViewItem.SubItems[1].Text = value
			

	_destFile as string
	DestFile as string:
		get:
			return _destFile
		set:
			_destFile = value
			_cxListViewItem.SubItems[2].Text = value
	

	_state as JobState
	State as JobState:
		get:
			return _state
		set:
			_state = value
			if value == JobState.NotProccessed:
				_cxListViewItem.SubItems[0].Text = "未处理"
			elif value == JobState.Waiting:
				_cxListViewItem.SubItems[0].Text = "等待"
			elif value == JobState.Working:
				_cxListViewItem.SubItems[0].Text = "工作中"
			elif value == JobState.Done:
				_cxListViewItem.SubItems[0].Text = "完成"
			elif value == JobState.Stop:
				_cxListViewItem.SubItems[0].Text = "中止"
			elif value == JobState.Error:
				_cxListViewItem.SubItems[0].Text = "错误"

	[Property(AvsConfig)]
	_avsConfig as AvisynthConfig

	[Property(VideoEncConfig)]
	_videoEncConfig as VideoEncConfigBase

	[Property(AudioEncConfig)]
	_audioEncConfig as AudioEncConfigBase
	
	[Property(SubtitleConfig)]
	_subtitleConfig as SubtitleConfig

	JobConfig as JobItemConfig:
		get:
			return _jobConfig
		set:
			if (not _videoInfo.AudioStreamsCount and not value.AudioMode == StreamProcessMode.None)\
				or (not _videoInfo.HasVideo and not value.VideoMode == StreamProcessMode.None):
					raise ArgumentException("Incorrect JobMode.")
			_jobConfig = value
	_jobConfig as JobItemConfig

	[Property(VideoEncoder)]
	_videoEncoder as VideoEncoderBase

	[Property(AudioEncoder)]
	_audioEncoder as AudioEncoderHandler

	[Property(Muxer)]
	_muxer as MuxerBase

	[Property(Event)]
	_event as JobEvent
	"""
	仅当向从工作线程向UI线程汇报前更改此属性。
	"""

	[Property(CxListViewItem)]
	_cxListViewItem as CxListViewItem

	[Property(ExternalAudio)]
	_externalAudio as string
	
	[Property(EncodedVideo)]
	_encodedVideo as string
	
	[Property(EncodedAudio)]
	_encodedAudio as string

	[Property(ProfileName)]
	_profileName as string
	
	[Property(SubtitleFile)]
	_subtitleFile as string

	[Property(FilesToDeleteWhenProcessingFails)]
	_createdFiles = List[of string](3)

	_readAvsCfg as bool
	_readVideoCfg as bool
	_readAudioCfg as bool
	_readJobCfg as bool
	_readSubCfg as bool
	_videoInfo as VideoInfo
	
	public def constructor(sourceFile as string, destFile as string, profileName as string):
	"""创建对象时内部各设置属性都为null，要用必须SetUp()一下"""
		self._sourceFile = sourceFile
		self._destFile = destFile
		self._profileName = profileName
		self._videoInfo = VideoInfo(sourceFile)
		self._cxListViewItem = CxListViewItem(("未处理", sourceFile, destFile))
		self._cxListViewItem.JobItem = self
		self._state = JobState.NotProccessed
	
	public def SetUp():
	"""
	根据JobItem对象的ProfileName属性为其读取各设置实例，并应用到值为null的设置属性。
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
		if self.SubtitleConfig == null:
			_readSubCfg = true
		ReadProfile(self._profileName)

	private def ReadProfile(profileName as string):
	"""
	从profile文件中读取VideoEncConfig AudioEncConfig AvsConfig对象到本类的相关属性。
	"""
		if _readAvsCfg or _readVideoCfg or _readAudioCfg or _readJobCfg or _readSubCfg:
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
		if self._readSubCfg:
			_subtitleConfig = profile.SubtitleConfig
			_readSubCfg = false
	
	public def Clear():
	"""
	将内部各设置属性设为null。
	"""
			self._avsConfig = null
			self._audioEncConfig = null
			self._videoEncConfig = null
			self._jobConfig = null
			self._subtitleConfig = null
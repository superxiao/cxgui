namespace CXGUI.Job

import System
import System.IO
import System.Windows.Forms
import System.Runtime.Serialization
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

class JobItem():
"""
创建一个JobItem实例，将附送一个CxListViewItem实例，可添加到ListView中。
更改JobItem的源文件、目标文件、工作状态，CxListViewItem相应自动更改。
"""

	//Fields	
	_sourceFile as string
	
	_destFile as string
	
	_videoInfo as VideoInfo
	
	_avsConfig as AvisynthConfig
	
	_videoEncConfig as VideoEncConfigBase
	
	_audioEncConfig as AudioEncConfigBase
	
	_subtitleConfig as SubtitleConfig
	
	_jobConfig as JobItemConfig
	
	[NonSerialized]
	_videoEncoder as VideoEncoderHandler
	
	[NonSerialized]
	_audioEncoder as AudioEncoderHandler
	
	[NonSerialized]
	_muxer as MuxerBase
	
	_usingExternalAudio as bool
	
	_externalAudio as string
	
	_state as JobState
	
	_usingCustomVideoScript as bool
	
	_customVideoScript as string
	
	_usingCustomAudioScript as bool
	
	_customAudioScript as string

	_readAvsCfg as bool
	
	_readVideoCfg as bool
	
	_readAudioCfg as bool
	
	_readJobCfg as bool
	
	_readSubCfg as bool
	
	
	//Properties
	SourceFile as string:
		get:
			return _sourceFile
			

	DestFile as string:
		get:
			return _destFile
		set:
			_destFile = value
			_cxListViewItem.SubItems[2].Text = value
	

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

	AvsConfig as AvisynthConfig:
		get:
			return self._avsConfig
		set:
			self._avsConfig = value

	VideoEncConfig as VideoEncConfigBase:
		get:
			return self._videoEncConfig
		set:
			self._videoEncConfig = value

	AudioEncConfig as AudioEncConfigBase:
		get:
			return self._audioEncConfig
		set:
			self._audioEncConfig = value
	
	SubtitleConfig as SubtitleConfig:
		get:
			return self._subtitleConfig
		set:
			self._subtitleConfig = value

	JobConfig as JobItemConfig:
		get:
			return _jobConfig
		set:
			if (not _videoInfo.AudioStreamsCount and not value.AudioMode == StreamProcessMode.None)\
				or (not _videoInfo.HasVideo and not value.VideoMode == StreamProcessMode.None):
					raise ArgumentException("Incorrect JobMode.")
			_jobConfig = value
	

	VideoEncoder as VideoEncoderHandler:
		get:
			return self._videoEncoder
		set:
			self._videoEncoder = value
			

	AudioEncoder as AudioEncoderHandler:
		get:
			return self._audioEncoder
		set:
			self._audioEncoder = value
	
	
	Muxer as MuxerBase:
	"""
	使用此属性前应先JobItem.SetUp()，否则返回null。
	如不需要混流器，仍返回null。
	可能保留有上此混流过程的信息，调用JobItem.SetUp()
	可以清除这些信息。
	"""
		get:
			return self._muxer

	[Property(Event)]
	_event as JobEvent
	"""
	仅当向从工作线程向UI线程汇报前更改此属性。
	"""

	[Property(CxListViewItem)]
	_cxListViewItem as CxListViewItem	
	
	UsingExternalAudio as bool:
		get:
			return self._usingExternalAudio
		set:
			self._usingExternalAudio = value

	ExternalAudio as string:
		get:
			return self._externalAudio
		set:
			self._externalAudio = value
	
	[Property(EncodedVideo)]
	_encodedVideo as string
	
	[Property(EncodedAudio)]
	_encodedAudio as string

	[Property(ProfileName)]
	_profileName as string
	"""
	当JobItem某设置属性为空时，调用SetUp()，ProfileName决定这个设置属性。
	但当某设置属性经过更改，则与ProfileName属性失去实际关联。
	"""
	
	[Property(SubtitleFile)]
	_subtitleFile as string

	[Property(FilesToDeleteWhenProcessingFails)]
	_createdFiles = List[of string](3)
	
	UsingCustomVideoScript as bool:
		get:
			return self._usingCustomVideoScript
		set:
			self._usingCustomVideoScript = value
			
	CustomVideoScript as string:
		get:
			return self._customVideoScript
		set:
			self._customVideoScript = value
			
	UsingCustomAudioScript as bool:
		get:
			return self._usingCustomAudioScript
		set:
			self._usingCustomAudioScript = value
			
	CustomAudioScript as string:
		get:
			return self._customAudioScript
		set:
			self._customAudioScript = value
			

	
	


	
	public def constructor(sourceFile as string, destFile as string, profileName as string):
	"""创建对象时内部各设置属性都为null，要用必须SetUp()一下。"""
		self._sourceFile = sourceFile
		self._destFile = destFile
		self._profileName = profileName
		self._videoInfo = VideoInfo(sourceFile)
		self._cxListViewItem = CxListViewItem(("未处理", sourceFile, destFile))
		self._cxListViewItem.JobItem = self
		self._state = JobState.NotProccessed
	
	public def SetUp():
	"""
	仅对于值为null的设置属性，根据JobItem.ProfileName属性为其读取各设置实例。
	并且根据JobItem.JobConfig.Container和JobItem.DestFile，创建新的JobItem.Muxer实例。
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
		if not self._videoInfo.HasVideo:
			self._jobConfig.VideoMode = StreamProcessMode.None
		if not self._videoInfo.AudioStreamsCount:
			self._jobConfig.AudioMode = StreamProcessMode.None
			
		if self._videoInfo.Format == "avs":
			if self._jobConfig.VideoMode == StreamProcessMode.Copy:
				self._jobConfig.VideoMode = StreamProcessMode.Encode
			if self._jobConfig.AudioMode == StreamProcessMode.Copy:
				self._jobConfig.AudioMode = StreamProcessMode.Encode
		self.CreateNewMuxer()

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

	private def CreateNewMuxer():
		if self._jobConfig == null:
			self._muxer = null
		else:
			container = self.JobConfig.Container
			if container == OutputContainer.MKV:
				self._muxer = MKVMerge()
			elif container == OutputContainer.MP4:
				if self.JobConfig.VideoMode == StreamProcessMode.Copy or self.JobConfig.AudioMode == StreamProcessMode.Copy:
					self._muxer = FFMP4()
				elif Path.GetExtension(self._destFile).ToLower() not in ('.mp4', '.m4v', '.m4a'):
					self._muxer = FFMP4()
				elif self.JobConfig.VideoMode == StreamProcessMode.Encode and self.JobConfig.AudioMode == StreamProcessMode.Encode:
					self._muxer = MP4Box()
				//以下条件：音频、视频处理模式都是‘None’，即源文件无媒体流；或一为编码，一为None，且输出MP4，则出品已是MP4，无需再混
				else:
					self._muxer = null

	[OnDeserialized]	
	private def OnDeserialize(context as StreamingContext):
		self.CxListViewItem.JobItem = self


	//Properties
﻿namespace CXGUI.GUI

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
	
enum JobMode:
	Normal
	Video
	Audio
	
[Serializable()]
class JobItem:
"""Description of JobItem"""
	[Getter(SourceFile)]
	_sourceFile as string
	[Property(DestFile)]
	_destFile as string
	AvsConfig as AvsConfig:
		get:
			return _avsConfig
		set:
			if (not _videoInfo.AudioStreamsCount and not value.Mode == JobMode.Video)\
				or (not _videoInfo.HasVideo and not value.Mode == JobMode.Audio):
					raise ArgumentException("Incorrect JobMode.")
			_avsConfig = value
	_avsConfig as AvsConfig
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
	[Property(KeepingCfg)]
	_KeepingCfg as bool
	
	_readAvsCfg as bool
	_readVideoCfg as bool
	_readAudioCfg as bool
	_videoInfo as VideoInfo
	
	public def constructor(sourceFile as string, destFile as string, uiItem as ListViewItem, readProfile as bool):
		self._sourceFile = sourceFile
		self._destFile = destFile
		self._uiItem = uiItem
		_videoInfo = VideoInfo(sourceFile)
		if readProfile:
			_readAvsCfg = true
			_readVideoCfg = true
			_readAudioCfg = true
			ReadProfile("default.profile")
	
	public def ReadAvsConfig():
		_readAvsCfg = true
		ReadProfile("default.profile")
		
	public def ReadVideoEncConfig():
		_readVideoCfg = true
		ReadProfile("default.profile")
		
	public def ReadAudioEncConfig():
		_readAudioCfg = true
		ReadProfile("default.profile")
	
	private def ReadProfile(path as string):
	"""
	从profile文件中读取VideoEncConfig AudioEncConfig AvsConfig对象到本类的相关属性。
	"""
		formater = BinaryFormatter()
		CreatProfile = do():
			profile = Profile()
			ReadProfile(profile)
			stream = FileStream(path, FileMode.Create)
			formater.Serialize(stream, profile)
			stream.Close()
		if not File.Exists(path):
			CreatProfile()
		else:
			try:
				stream = FileStream(path, FileMode.Open)
				profile = formater.Deserialize(stream) as Profile
				ReadProfile(profile)
				stream.Close()
			except:
				stream.Close()
				CreatProfile()

	private def ReadProfile(profile as Profile):
		if self._readAvsCfg:
			_avsConfig = profile.AvsConfig
			_readAvsCfg = false
		if self._readVideoCfg:
			_videoEncConfig = profile.VideoEncConfig
			_readVideoCfg = false
		if self._readAudioCfg:
			_audioEncConfig = profile.AudioEncConfig
			_readAudioCfg = false
	
	public def Clear():
	"""
	清理内部存储设置数据的对象。
	如果KeepingCfg属性设置为true, 则不执行任何操作。
	"""
		if not self._KeepingCfg:
			self._avsConfig = null
			self._audioEncConfig = null
			self.VideoEncConfig = null
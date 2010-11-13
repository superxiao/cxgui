﻿namespace CXGUI.Config

import System
import System.IO
import System.Windows.Forms
import System.Runtime.Serialization.Formatters.Binary
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer
import CXGUI.Job

class Profile:	
	
	[Property(AvsConfig)]
	_avsConfig as AvisynthConfig
	[Property(VideoEncConfig)]
	_videoEncConfig as x264Config
	[Property(AudioEncConfig)]
	_audioEncConfig as NeroAacConfig
	[Property(JobConfig)]
	_jobConfig as JobItemConfig	
	[Property(SubtitleConfig)]
	_subtitleConfig as SubtitleConfig
	
	static final _profileDir as string 
	
	
	
	static def constructor():
		_profileDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "profile")
		if not Directory.Exists(_profileDir):
			Directory.CreateDirectory(_profileDir)
		
	public def constructor(initializeConfig as bool):
	"""
	使用默认的各个设置对象。
	"""
		if initializeConfig:
			_avsConfig = AvisynthConfig()
			_videoEncConfig = x264Config()
			_audioEncConfig = NeroAacConfig()
			_jobConfig = JobItemConfig()
			_subtitleConfig = SubtitleConfig()
		
	public def constructor(profileName as string):
	"""
	如果profile文件不存在或损坏将引起异常。
	"""
		path = Path.Combine(_profileDir, profileName+".profile") //TODO profile文件夹
		formater = BinaryFormatter()
		if not File.Exists(path):
			raise ProfileNotFoundException("profile文件未找到")
		else:
			try:
				stream = FileStream(path, FileMode.Open)
				profile = formater.Deserialize(stream) as Profile
				stream.Close()
			except:
				stream.Close()
				raise ProfileNotFoundException("profile文件损坏")

		self._avsConfig = profile._avsConfig
		self._videoEncConfig = profile._videoEncConfig
		self._audioEncConfig = profile._audioEncConfig
		self._jobConfig = profile._jobConfig
		self._subtitleConfig = profile._subtitleConfig
		
	public static def GetExistingProfilesNamesOnHardDisk() as (string):
		profileNames = List[of string]()
		files = Directory.GetFiles(_profileDir, "*.profile")
		formater = BinaryFormatter()
		for file in files:
			try:
				stream = FileStream(file, FileMode.Open)
				profile = formater.Deserialize(stream) as Profile
				profileNames.Add(Path.GetFileNameWithoutExtension(file))
				stream.Close()
			except:
				stream.Close()
		return profileNames.ToArray()
	
	public static def RebuildDefault(defaultProfileName as string):
		formater = BinaryFormatter()
		path = Path.Combine(_profileDir, defaultProfileName+".profile")
		profile = Profile(true)
		stream = FileStream(path, FileMode.Create)
		formater.Serialize(stream, profile)
		stream.Close()
		
	public static def Save(profileName as string, jobConfig as JobItemConfig,
	avsConfig as AvisynthConfig, videoEncConfig as VideoEncConfigBase, audioEncConfig as AudioEncConfigBase,
	subtitleConfig as SubtitleConfig):
		formater = BinaryFormatter()
		path = Path.Combine(_profileDir, profileName+".profile")
		profile = Profile(false)
		profile._jobConfig = jobConfig
		profile._videoEncConfig = videoEncConfig
		profile._audioEncConfig = audioEncConfig
		profile._avsConfig = avsConfig
		profile._subtitleConfig = subtitleConfig
		stream = FileStream(path, FileMode.Create)
		formater.Serialize(stream, profile)
		stream.Close()
		
	public def GetExtByContainer() as string:
		if self.JobConfig.Container == OutputContainer.MP4:
			ext = ".mp4"
		elif self.JobConfig.Container == OutputContainer.MKV:
			ext = ".mkv"
		return ext
		
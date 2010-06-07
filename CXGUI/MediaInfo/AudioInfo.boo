﻿namespace CXGUI

import System
import MediaInfoLib

class AudioInfo():
"""
获取媒体文件的音频信息。
Remarks: 可以创建对象并访问其属性，也可以使用静态方法来获取信息。
"""
	public def constructor(path as string):
	"""
	Remarks: 从本类申请对象时，须以媒体文件路径作为实参。之后即可访问对象属性以
	获取音频信息。
	"""
		InitializeProperties(path, 0)
	private def InitializeProperties(path as string, streamNumber as int):
		_currentStream = streamNumber
		MI = MediaInfoLib.MediaInfo()
		MI.Open(path)
		_filePath = path
		_format = MI.Get(StreamKind.Audio, _currentStream, "Format")
		_streamsCount = MI.Count_Get(StreamKind.Audio)
		MI.Close()

	//Methods
	public static def GetAudioInfo(path as string, streamNumber as int, audioParameter as string) as string:
	"""
	静态方法,获取音频信息的字符串形式。
	Param path: 媒体文件路径。
	Param streamNumber: 音频流序号，不计视频流。
	Param audioParameter: 音频参数名。参见MediaInfo相关文档。
	Returns: 音频信息的字符串形式。
	"""
		MI = MediaInfoLib.MediaInfo()
		MI.Open(path)
		audioinfo = MI.Get(StreamKind.Audio, streamNumber, audioParameter, MediaInfoLib.InfoKind.Text)
		MI.Close()
		return audioinfo
	public static def GetAudioInfo(path as string, streamNumber as int, *audioParameters as (string)) as Hash:
	"""
	静态方法,获取多种音频信息的字符串形式。
	Param path: 媒体文件路径。
	Param streamNumber: 音频流序号，不计视频流。
	Param *videoParameter: 多个音频参数名。参见MediaInfo相关文档。
	Returns: 内容为“参数名:参数值”的Hashtable。参数值为音频信息的字符串形式。
	"""
		audioinfo = {}
		for audioParameter in audioParameters:
			infovalue =AudioInfo.GetAudioInfo(path, streamNumber, audioParameter)
			audioinfo.Add(audioParameter, infovalue)
		return audioinfo

	//Properties
	[Getter(FilePath)]
	_filePath as string
	"""
	媒体文件路径。
	"""
	CurrentStream as int:
	"""音频流的序号，不计视频流。
	Remarks: 创建对象时默认为0，重设时将为新流更新对象属性。"""
		get:
			return _currentStream
		set:
			InitializeProperties(_filePath, value)
	_currentStream as int
	[Getter(StreamsCount)]
	_streamsCount as int
	"""音频流的数量，不计视频流。"""
	[Getter(Format)]
	_format as string
	"""音频格式。"""
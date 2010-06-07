namespace CXGUI.AudioEncoding

import System
import System.Windows.Forms //test

class NeroAacConfig(AudioEncConfigBase):
	Quality as double:
		get:
			return _quality
		set:
			value = 1 if value > 1
			value = 0.01 if value < 0.01
			_quality = value
			_bitRate = 0
			_constantBitRate = 0
	_quality as double
	BitRate as int:
		get:
			return _bitRate
		set:
			value = 1 if value < 1
			_bitRate = value
			_quality = 0
			_constantBitRate = 0
	_bitRate as int
	ConstantBitRate as int:
		get:
			return _constantBitRate
		set:
			value = 1 if value < 1
			_constantBitRate = value
			_quality = 0
			_bitRate = 0
	_constantBitRate as int
	
	public def constructor():
		_quality = 0.5

	//methods
	def GetSettings() as string:
		settings as string
		if _quality > 0:
			settings = " -q ${_quality}"
		elif _bitRate > 0:
			settings = " -br ${_bitRate}"
		elif _constantBitRate > 0:
			settings = " -cbr ${_constantBitRate}"
		return settings

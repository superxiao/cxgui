namespace CXGUI.Avisynth

import System

class ResolutionCalculator:
"""Description of ResolutionCalculator"""
	public def constructor():
		self._fixAspectRatio = true
		self._mod = 2
	Width as int:
	"""
	视频宽。
	Raises ArgumentException: Width必须是正整数。
	Remarks: 所设值不是Mod正整数倍的，自动改为最接近的Mod正整数倍。
	"""
		get:
			return _width
		set:
			if  value <= 0:
				raise ArgumentException("Width must be positive.")
			if value % _mod != 0:
				value = CalculateMod(value)
			_width = value	
			if _fixAspectRatio:
				_height = CalculateMod(_width / _aspectRatio)
			else:
				_aspectRatio = cast(double, _width) / _height
	_width as int
	Height as int:
	"""
	视频高。
	Raises ArgumentException: Height必须是MOD的正整数倍。
	Remarks: 所设值不是Mod正整数倍的，自动改为最接近的Mod正整数倍。
	"""
		get:
			return _height
		set:
			if  value <= 0:
				raise ArgumentException("Height must be positive.")
			if value % _mod != 0:
				value = CalculateMod(value)
			_height = value
			if _fixAspectRatio:
				_width = CalculateMod(_height*_aspectRatio)
			else:
				_aspectRatio = cast(double, _width) / _height
	_height as int
	AspectRatio as double:
	"""
	视频宽高比。
	Raises ArgumentOutOfRangeException: AspectRatio必须是正数。
	"""
		get:
			return _aspectRatio
		set:
			if  value <= 0:
				raise ArgumentOutOfRangeException("AspectRatio")
			_aspectRatio = value
			if _fixHeightInsteadOfWidth:
				_width = CalculateMod(_height*_aspectRatio)
			else:
				_height = CalculateMod(_width / _aspectRatio)
	_aspectRatio as double
	[Property(FixAspectRatio)]
	_fixAspectRatio as bool
	"""
	在更改Height或Width时，是否锁定AspectRatio。默认为true。
	Remarks: 如为true，将更改对应的Width或Height；如为false，将更改AspectRatio。
	"""
	[Property(FixHeightInsteadOfWidth)]
	_fixHeightInsteadOfWidth as bool
	"""
	当发生必须更改Width或Height的操作时，如改变AspectRatio或MOD值时，固定哪一个。
	默认为false。
	"""
	Mod as int:
	"""高或宽必须是MOD的整数倍。默认为2。
	Raises ArgumentOutOfRangeException: MOD必须是正数。
	"""
		get:
			return _mod
		set:
			if value <= 0:
				raise ArgumentOutOfRangeException("Mod")
			_mod = value
			_width = CalculateMod(_width)
			_height = CalculateMod(_height)
	_mod as int
	private def CalculateMod(number as double) as int:
		if number%_mod >= _mod / 2.0:
			i = 1
			j = cast(int, number) + 1
		else:
			i = -1
			j = cast(int, number)
		j+=i while j%_mod!=0
		j = 1 if j == 0
		return j
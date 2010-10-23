namespace CXGUI.VideoEncoding

import System

enum NodeType:
	Num
	StrOptionIndex
	Bool
	Str
	
class X264ConfigNode:
"""Description of X264ConfigNode"""
	public def constructor():
		_locked = false
		_inUse = true
	
	[Property(Name)]
	_name as string
	
	[Property(Locked)]
	_locked as bool
	
	[Property(InUse)]
	_inUse as bool

	[Property(Type)]
	_valueType as NodeType

	[Property(Num)]
	_num as double

	[Property(StrOptionIndex)]
	_strOptionIndex as int
	
	[Property(Bool)]
	_bool as bool

	[Property(DefaultNum)]
	_defaultNum as double

	[Property(DefaultStrOptionIndex)]
	_defaultStrOptionIndex as int
	
	[Property(DefaultBool)]
	_defaultBool as bool
	
	[Property(MinNum)]
	_minNumValue as double
	
	[Property(MaxNum)]
	_maxNumValue as double
	
	[Property(StrOptions)]
	_strOptions as (string)
	
	[Property(Str)]
	_str as string
	
	[Property(DefaultStr)]
	_defaultStr as string
	
	[Property(Special)]
	_notCmdOption as bool
	

	public def GetValueAsStr():
		if self._valueType == NodeType.Num:
			return self._num.ToString()
		elif self._valueType == NodeType.Bool:
			return 
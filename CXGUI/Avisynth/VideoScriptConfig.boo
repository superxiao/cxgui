namespace CXGUI.Avisynth

import System
import System.Windows.Forms//test
import System.Collections
import System.Collections.Specialized
import System.IO
import CXGUI

enum VideoSourceFilter:
"""用于VideoScriptConfig类的SourceFilter属性。"""
	DirectShowSource
	DSS2
	FFVideoSource
	//DGAVCDec//TODO 了解用法
	None

enum ResizeFilter:
"""用于VideoScriptConfig类的Resizer属性。"""
	LanczosResize
	Lanczos4Resize
	BicubicResize
	BilinearResize
	BlackmanResize
	GaussResize
	PointResize
	SincResize
	Spline16Resize
	Spline36Resize
	Spline64Resize
	None
		
class VideoScriptConfig():
"""
用于AvisynthWriter类的VideoConfig属性。
"""
	public def constructor(videoInfo as VideoInfo):
		_videoInfo = videoInfo
		InitializeProperties()
	private def InitializeProperties():
		//SourceFilter
		_frameRate = _videoInfo.FrameRate
		//ConvertToYV12
		_autoConvertToYV12 = true
		//ColorMatrix
		_autoConvertColorMatrix = true
		//Resizer
		_width = _videoInfo.Width
		_height = _videoInfo.Height
		//_filters和UpdateFilters方法是本类的核心。
		//在每次使用_filters前必先调用UpdateFilters。
		UpdateFilters()

	#region methods
	
	//Methods
	def GetScriptContent() as string:
	"""
	使用这个方法来获取完整的脚本内容。
	Returns：视频脚本完整内容。
	"""
		UpdateFilters()
		content as string
		for _importedModule as string in _importedModules:
			content = content + "${_importedModule}\r\n"
		for _filter as DictionaryEntry in _filters:
			content = content + "${_filter.Value}\r\n"
		return content
	def SetCustomFilter(filterName as string, statement as string):
	"""
	设定一个滤镜的语句。
	Param filterName：滤镜的标识。存在四个预定义的滤镜标识："SourceFilter", 
	"ConvertToYV12","ColorMatrix","Resizer"。
	Param statement:所要更改或添加的语句。
	Remarks: 如filterName已存在，将更改其语句。如不存在，在末尾添加语句。
	对于预定义的filterName无任何效果。如果确实要更改预定义的滤镜，先使用
	RemoveFilter方法删除改滤镜，然后以不同的标识重新创建。
	"""
		UpdateFilters()
		if not ["SourceFilter", "ConvertToYV12", "Resizer", "ColorMatrix"].Contains(filterName):
			SetFilter(filterName, statement)
	def GetFilterStatement(filterName as string) as string:
	"""
	获取一个滤镜的语句。
	Param filterName: 滤镜的标识。存在四个预定义的滤镜标识："SourceFilter", 
	"ConvertToYV12","ColorMatrix","Resizer"。
	Returns: 滤镜的语句。
	"""
		UpdateFilters()
		return _filters[filterName]
	def RemoveFilter(filterName as string):
	"""
	删除一个滤镜的语句。
	Param filterName: 滤镜的标识。存在四个预定义的滤镜标识："SourceFilter", 
	"ConvertToYV12","ColorMatrix","Resizer"。
	"""
		UpdateFilters()
		_filters.Remove(filterName)
		if filterName == "SourceFilter":
			_sourceFilter = VideoSourceFilter.None
		elif filterName == "ConvertToYV12":
			_autoConvertToYV12 = false
		elif filterName == "Resizer":
			_resizeFilter = ResizeFilter.None
		elif filterName == "ColorMatrix":
			_autoConvertColorMatrix = false
	def ContainsFilter(filterName as string) as bool:
	"""
	判断一个滤镜是否已存在。
	Param filterName: 滤镜的标识。存在四个预定义的滤镜标识："SourceFilter", 
	"ConvertToYV12","ColorMatrix","Resizer"。
	Returns: bool,一个滤镜是否已存在。
	"""
		UpdateFilters()
		return _filters.Contains(filterName)
	def SetFilterOrder(*filterNames as (string)):
	"""
	依照给出的滤镜表示顺序，对其排序。
	Param filterName: 滤镜的标识。存在四个预定义的滤镜标识："SourceFilter", 
	"ConvertToYV12","ColorMatrix","Resizer"。
	Remarks: 如果给出的滤镜标识不是存在的全部，将只更改相应位置的滤镜顺序。
	"""
		UpdateFilters()
		filterstosort = []
		filterstemp = OrderedDictionary()
		for filterName in filterNames:
			if _filters.Contains(filterName):
				filterstosort.AddUnique(filterName)
		i as int = 0
		for _filter as DictionaryEntry in _filters:
			if filterstosort.Contains(_filter.Key):
				filterstemp.Add(filterstosort[i], _filters[filterstosort[i]])
				i++
			else:
				filterstemp.Add(_filter.Key, _filter.Value)
		_filters = filterstemp
	def GetFilterOrder() as List:
	"""
	获取一个滤镜名List，顺序为滤镜顺序。
	Returns: List，包含依序排列的滤镜名。
	"""	
		UpdateFilters()
		return List(_filters.Keys)
	def GetImport() as List:
	"""
	获取导入外部脚本和DLL的语句List。
	Returns: 导入外部脚本和DLL的语句List。
	"""
		return _importedModules
	//模块一旦导入，目前不能删除
	def SetImport(*externalFilters as (string)):
	"""
	设置导入外部脚本和DLL的语句。
	Param *externalFilters: 外部脚本或DLL的路径。
	"""
		for externalFilter in externalFilters:
			ext = IO.Path.GetExtension(externalFilter).ToLower()
			if ext == ".dll":
				_importedModules.AddUnique("LoadPlugin(\"${externalFilter}\")")
			elif ext == ".avs" or ext == "avis":
				_importedModules.AddUnique("Import(\"${externalFilter}\")")
	private def UpdateFilters():
	"""
	本类的核心方法，每次调用_filters前先调用此方法，以使之和对应的属性
	同步。
	"""
		//SourceFilter语句
		if _sourceFilter == VideoSourceFilter.None:
			_filters.Remove("SourceFilter")
		else:
			fps = ""
			if _frameRate != _videoInfo.FrameRate:
				fps = ", fps=${_frameRate}"
			if _sourceFilter == VideoSourceFilter.DirectShowSource:
				convertfps = ""
				withaudio = ""
				if _convertFPS:
					convertfps = ", convertfps=true"
				if not _withAudio:
					withaudio = ", audio=false"
				SetFilter("SourceFilter", """${_sourceFilter}("${_videoInfo.FilePath}"${fps}${convertfps}${withaudio})""")
			elif _sourceFilter == VideoSourceFilter.DSS2:
				SetImport("avss.dll") //TODO 设置路径的位置
				SetFilter("SourceFilter", """${_sourceFilter}("${_videoInfo.FilePath}"${fps})""")
			elif _sourceFilter == VideoSourceFilter.FFVideoSource:
				SetImport("ffms2.dll")
				fps = ""
				if _frameRate != _videoInfo.FrameRate:
					fpsnum as int = Math.Round(_frameRate * 1000)
					fps = ", fpsnum=${fpsnum}, fpsden=1000"
				SetFilter("SourceFilter", """${_sourceFilter}("${_videoInfo.FilePath}"${fps})""")
				
		//ConvertToYV12语句
		if _autoConvertToYV12:
			SetFilter("ConvertToYV12", "IsYV12 ? Last : ConvertToYV12()")
		else:
			_filters.Remove("ConvertToYV12")
		//ColorMartrix语句
		if _autoConvertColorMatrix and _width <= 1000 and _videoInfo.Width >= 1280://TODO:ColorMatrix识别标准还需要调整
			SetFilter("ColorMatrix", "ColorMatrix()")
			SetImport("ColorMatrix.dll") //TODO 设置路径的位置
		else:
			_filters.Remove("ColorMatrix")
		//Resizer语句
		if _resizeFilter != ResizeFilter.None and (_width != _videoInfo.Width or _height != _videoInfo.Height):
			SetFilter("Resizer", "${_resizeFilter}(${_width}, ${_height})")
		else:
			_filters.Remove("Resizer")
			
		//Subtitle
		if File.Exists(self._subtitle):
			SetImport("VSFilter.dll")
			SetFilter("TextSub", "TextSub(\"${self._subtitle}\")")
		else:
			_filters.Remove("TextSub")

			
	private def SetFilter(filterName as string, statement as string):
		if _filters.Contains(filterName):
			_filters[filterName] = statement
		else:
			_filters.Add(filterName, statement)
			
	#endregion
	
	//Fields
	_videoInfo as VideoInfo
	_filters = OrderedDictionary() //本类核心
	_importedModules = [] //导入模块语句，只于GetImport和SetImport,UpdateFilter,GetScriptContent有关

	//Properties(Filters)
	//SourceFilter
	SourceFilter as VideoSourceFilter:
	"""
	视频源滤镜，默认为DirectShowSource。
	Remarks: 和ConvertFPS, WithAudio等属性有关联，会相互影响。
	如设SourceFilter为DSS2时,ConvertFPS锁定为true。
	"""
		get:
			return _sourceFilter
		set:
			_sourceFilter = value
			if value == VideoSourceFilter.DSS2:
				_convertFPS = true
				_withAudio = false
			elif value == VideoSourceFilter.FFVideoSource:
				_withAudio = false
	_sourceFilter as VideoSourceFilter
	[Property(FrameRate, value > 0)]
	_frameRate as double
	"""输出帧率，默认与源视频相同。"""
	ConvertFPS as bool:
	"""是否转化为恒定帧率，默认为false。
	Remarks: 当SourceFilter为DSS2时锁定为true。"""
		get:
			return _convertFPS
		set:
			if _sourceFilter == VideoSourceFilter.DSS2:
				pass
			else:
				_convertFPS = value
	_convertFPS as bool
	WithAudio as bool:
	"""
	是否包含音频流，默认为false。
	Remarks: 当SourceFilter为DSS2时锁定为false。目前只支持同一视频的音频流。"""
		get:
			return _withAudio
		set:
			if  _sourceFilter == VideoSourceFilter.DirectShowSource:
				_withAudio = value
			else:
				pass
	_withAudio as bool
	//ConvertToYV12
	[Property(AutoConvertToYV12)]
	_autoConvertToYV12 as bool
	"""
	是否自动转换成YV12。
	Remarks: 如此项为true,将添加"IsYV12 ? Last : ConvertToYV12()"语句。
	"""
	//ColorMatrix
	[Property(AutoConvertColorMatrix)]
	_autoConvertColorMatrix as bool
	"""是否自动转换色域，默认为true。
	Remarks: 目前只支持高清转标清的识别。"""
	//Resizer
	[Property(Resizer)]
	_resizeFilter as ResizeFilter
	"""缩放滤镜，默认为LanczosResize。"""
	Width as int:
	"""
	视频目标宽，默认与源相同。
	Raises ArgumentException: Width必须是正整数。
	"""
		get:
			return _width
		set:
			if  value <= 0:
				raise ArgumentException("Width must be positive.")
			_width = value
	_width as int
	Height as int:
	"""
	视频目标高，默认与源相同。
	Raises ArgumentException: Height必须是正整数。
	"""
		get:
			return _height
		set:
			if  value <= 0:
				raise ArgumentException("Height must be positive.")
			_height = value
	_height as int
	
	[Property(Subtitle)]
	_subtitle as string
	"""字幕路径。"""

	//增添预定义视频滤镜注意：1.整理好属性间关系,添加注释 2.确定UpdateFilters中的位置 
	//3.编写UpdateFilters代码 4 SetCustomFilter RemoveFilter扩充 5.InitializeProperties
	//6更改各处注释
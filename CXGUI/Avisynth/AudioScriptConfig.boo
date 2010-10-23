namespace CXGUI.Avisynth

import System
import System.Windows.Forms//test
import System.Collections
import System.Collections.Specialized
import CXGUI


enum AudioSourceFilter:
"""用于AudioScriptConfig类的SourceFilter属性。"""
	FFAudioSource
	DirectShowSource
	//NicAudio 仅支持独立音轨
	None
		
class AudioScriptConfig():
"""
用于AvisynthWriter类的AudioConfig属性。
"""
	public def constructor(audioInfo as AudioInfo):
		_audioInfo = audioInfo
		InitializeProperties()
	private def InitializeProperties()://暂时无需初始化属性
		UpdateFilters()
		
	//Methods
	def GetScriptContent() as string:
	"""
	使用这个方法来获取完整的脚本内容。
	Returns: 音频脚本完整内容。
	"""
		UpdateFilters()
		content as string
		for _importedModule as string in _importedModules:
			content = content + "${_importedModule}\r\n"
		for _filter as DictionaryEntry in _filters:
			content = content + "${_filter.Value}\r\n"
		//AudioScriptConfig和VideoScriptConfig相比，其结构唯一不同之处，必须在结尾加"audio"
		content += "audio\r\n"
		return content
	def SetCustomFilter(filterName as string, statement as string):
	"""
	设定一个滤镜的语句。
	Param filterName：滤镜的标识。存在三个预定义的滤镜标识："SourceFilter", 
	"DownMix","Normalize"。
	Param statement:所要更改或添加的语句。
	Remarks: 如filterName已存在，将更改其语句。如不存在，在末尾添加语句。
	对于预定义的filterName无任何效果。如果确实要更改预定义的滤镜，先使用
	RemoveFilter方法删除改滤镜，然后以不同的标识重新创建。
	"""
		UpdateFilters()
		if not ["SourceFilter", "DownMix", "Normalize"].Contains(filterName):
			SetFilter(filterName, statement)
	def GetFilterStatement(filterName as string) as string:
	"""
	获取一个滤镜的语句。
	Param filterName: 滤镜的标识。存在三个预定义的滤镜标识："SourceFilter", 
	"DownMix","Normalize"。
	Returns: 滤镜的语句。
	"""
		UpdateFilters()
		return _filters[filterName]
	def RemoveFilter(filterName as string):
	"""
	删除一个滤镜的语句。
	Param filterName: 滤镜的标识。存在三个预定义的滤镜标识："SourceFilter", 
	"DownMix","Normalize"。
	"""
		UpdateFilters()
		_filters.Remove(filterName)
		if filterName == "SourceFilter":
			_sourceFilter = AudioSourceFilter.None
		elif filterName == "DownMix":
			_downMix = false
		elif filterName == "Normalize":
			_normalize = false
	def ContainsFilter(filterName as string) as bool:
	"""
	判断一个滤镜是否已存在。
	Param filterName: 滤镜的标识。存在三个预定义的滤镜标识："SourceFilter", 
	"DownMix","Normalize"。
	Returns: bool,一个滤镜是否已存在。
	"""
		UpdateFilters()
		return _filters.Contains(filterName)
	def SetFilterOrder(*filterNames as (string)):
	"""
	依照给出的滤镜表示顺序，对其排序。
	Param filterName: 滤镜的标识。存在三个预定义的滤镜标识："SourceFilter", 
	"DownMix","Normalize"。
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
		if _sourceFilter == AudioSourceFilter.None:
			_filters.Remove("SourceFilter")
		else:
			if _sourceFilter == AudioSourceFilter.DirectShowSource:
				withvideo = ""
				if not _withVideo:
					withvideo = ", video=false"
				SetFilter("SourceFilter", """audio=${_sourceFilter}("${_audioInfo.FilePath}"${withvideo})""")
			elif _sourceFilter == AudioSourceFilter.FFAudioSource:
				SetFilter("SourceFilter", """audio=${_sourceFilter}("${_audioInfo.FilePath}", track=-1)""")
				SetImport("ffms2.dll")
			

		//DownMix
		if _downMix:
			SetFilter("DownMix", "audio=(audio.AudioChannels>2) ? DownMix(audio, audio.AudioChannels) : audio")
			SetImport(("Downmix.avs"))
		else:
			_filters.Remove("DownMix")
		//Normalize
		if _normalize:
			SetFilter("Normalize", "audio.Normalize()")
		else:
			_filters.Remove("Normalize")

	private def SetFilter(filterName as string, statement as string):
		if _filters.Contains(filterName):
			_filters[filterName] = statement
		else:
			_filters.Add(filterName, statement)

	//Fields
	_audioInfo as AudioInfo
	_filters = OrderedDictionary() //本类核心
	_importedModules = [] //导入模块语句，只于GetImport和SetImport,UpdateFilter,GetScriptContent有关
	
	//Properties(Filters)
	//SourceFilter
	[Property(SourceFilter)]
	_sourceFilter as AudioSourceFilter
	"""音频源滤镜，默认为DirectShowSource。和WithVideo属性有关联。"""
	[Property(WithVideo)]
	_withVideo as bool
	"""是否包含视频流，默认为false。目前只支持同一媒体文件的视频频流。"""
	//DownMix
	[Property(DownMix)]
	_downMix as bool
	"""是否自动混音为2声道。"""
	//Normalize
	[Property(Normalize)]
	_normalize as bool
	"""是否进行音量规格化。"""
	//增添预定义视频滤镜注意：1.整理好属性间关系,添加注释 2.确定UpdateFilters中的位置 
	//3.编写UpdateFilters代码 4 SetCustomFilter RemoveFilter扩充 5.InitializeProperties
	//6更改各处注释


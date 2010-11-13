namespace CXGUI.Avisynth

import System
import System.IO
import System.Collections
import System.Collections.Specialized
import CXGUI
import CXGUI.External


enum AudioSourceFilter:
"""用于AudioScriptConfig类的SourceFilter属性。"""
	DirectShowSource
	FFAudioSource
	None
		
class AudioAvsWriter():
"""编写avs脚本。如sourceFile为avs脚本，则各滤镜设置暂时不生效。"""


	//Fields
	_sourceFilter as AudioSourceFilter
	
	_downMix as bool
	
	_normalize as bool
	
	_filters as OrderedDictionary[of string, string]
	
	_loadingsAndImportings as List[of string]
	
	_audioInfo as AudioInfo
	
	_avsConfig as AvisynthConfig


	//Methods
	public def constructor(sourceFile as string, avsConfig as AvisynthConfig):
		self._avsConfig = avsConfig
		self._filters = OrderedDictionary[of string, string]()
		self._loadingsAndImportings = List[of string]()
		self._audioInfo = AudioInfo(sourceFile)
		if self._audioInfo.Format == "avs":
			self.AvsInputInitialize(sourceFile)
		else:
			self.SourceFilter = self._avsConfig.AudioSourceFilter
			self.DownMix = self._avsConfig.DownMix
			self.Normalize = self._avsConfig.Normalize
		
	private def AvsInputInitialize(sourceFile as string):
		self.SetImport(sourceFile)
		self.SetFilter("KillVideo", "KillVideo()")

	def GetScriptContent() as string:
	"""
	使用这个方法来获取avs脚本内容。
	Returns: 音频avs脚本内容。
	"""
		content as string
		for loadingAndImporting in self._loadingsAndImportings:
			content += "${loadingAndImporting}\r\n"
		for filter in self._filters:
			content += "${filter.Value}\r\n"
		if self._audioInfo.Format != "avs":
			content += "audio\r\n"
		return content
		
	def WriteScript(avsDestFile as string):
		File.WriteAllText(avsDestFile, self.GetScriptContent(), Text.Encoding.Default)

	private def SetFilter(filterName as string, statement as string):
		_filters[filterName] = statement

	def GetFilterStatement(filterName as string) as string:
	"""
	获取一个滤镜的语句。
	Param filterName: 滤镜的标识。
	Returns: 滤镜的语句。
	"""
		return self._filters[filterName]

	def RemoveFilter(filterName as string):
	"""
	删除一个滤镜的语句。
	Param filterName: 滤镜的标识。
	"""
		if self._filters.ContainsKey(filterName):
			self._filters.Remove(filterName)

	def ContainsFilter(filterName as string) as bool:
	"""
	判断一个滤镜是否已存在。
	Param filterName: 滤镜的标识。
	Returns: 滤镜是否已存在。
	"""
		return self._filters.ContainsKey(filterName)

	def SetImport(*externalFilters as (string)):
	"""
	设置导入外部脚本和DLL的语句。
	Param *externalFilters: 外部脚本或DLL的路径。
	"""
		for externalFilter in externalFilters:
			ext = IO.Path.GetExtension(externalFilter).ToLower()
			if ext == ".dll":
				_loadingsAndImportings.AddUnique("LoadPlugin(\"${externalFilter}\")")
			elif ext == ".avs" or ext == "avis":
				_loadingsAndImportings.AddUnique("Import(\"${externalFilter}\")")
	


	//Properties
	SourceFilter as AudioSourceFilter:
		get:
			return _sourceFilter
		set:
			_sourceFilter = value
			if self._sourceFilter == AudioSourceFilter.DirectShowSource:
				SetFilter("SourceFilter", "audio=DirectShowSource(\"${_audioInfo.FilePath}\", video=false)")
			elif self._sourceFilter == AudioSourceFilter.FFAudioSource:
				SetImport(Path.GetFullPath("ffms2.dll"))
				SetFilter("SourceFilter", "audio=FFAudioSource(\"${_audioInfo.FilePath}\", track=-1)")
			elif self._sourceFilter == AudioSourceFilter.None:
				RemoveFilter("SourceFilter")
					
	DownMix as bool:
	"""是否自动混音为2声道。"""
		get:
			return _downMix
		set:
			_downMix = value
			if value:
				SetImport(Path.GetFullPath("Downmix.avs"))
				SetFilter("DownMix", "audio=(audio.AudioChannels>2) ? DownMix(audio, audio.AudioChannels) : audio")
			else:
				RemoveFilter("DownMix")
	
	Normalize as bool:
		get:
			return _normalize
		set:
			_normalize = value
			if value:
				SetFilter("Normalize", "audio.Normalize()")
			else:
				RemoveFilter("Normalize")


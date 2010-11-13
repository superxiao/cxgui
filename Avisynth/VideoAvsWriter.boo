namespace CXGUI.Avisynth

import System
import System.Windows.Forms//test
import System.Collections
import System.Collections.Specialized
import System.IO
import CXGUI
import CXGUI.External

enum VideoSourceFilter:
"""用于VideoScriptConfig类的SourceFilter属性。"""
	DirectShowSource
	DSS2
	FFVideoSource
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


//TODO 对avs脚本源，重新做一个路径	
class VideoAvsWriter():
"""编写avs脚本。如sourceFile为avs脚本，则各滤镜设置暂时不生效。"""

	//Fields
	_filters as OrderedDictionary[of string, string]
	
	_loadingsAndImportings as List[of string]
	
	_videoInfo as VideoInfo
	
	_avsConfig as AvisynthConfig


	//Methods
	public def constructor(sourceFile as string, avsConfig as AvisynthConfig, subtitleFile as string):
		self._filters = OrderedDictionary[of string, string]()
		self._loadingsAndImportings = List[of string]()
		self._avsConfig = avsConfig
		self._videoInfo = VideoInfo(sourceFile)
		
		if self._videoInfo.Format == "avs":
			AvsInputInitialize(sourceFile)
		else:
			self.SetSourceFilter(self._avsConfig.VideoSourceFilter)
			
			if not self._avsConfig.UsingSourceFrameRate:
				self.SetFrameRate(self._avsConfig.FrameRate, self._avsConfig.ConvertFPS)
				
			SetFilter("ConvertToYV12", "IsYV12 ? Last : ConvertToYV12()")
			
			if not self._avsConfig.UsingSourceResolution and (self._avsConfig.Width != self._videoInfo.Width or self._avsConfig.Height != self._videoInfo.Height):
				if self._avsConfig.Width <= 1000 and self._videoInfo.Width >= 1280: 
					SetImport(Path.GetFullPath("ColorMatrix.dll"))
					SetFilter("ColorMatrix", "ColorMatrix()")
				SetFilter("Resizer", "${self._avsConfig.Resizer}(${self._avsConfig.Width},${self._avsConfig.Height})")
			
			if File.Exists(subtitleFile):
				SetImport(Path.GetFullPath("VSFilter.dll"))
				SetFilter("TextSub", "TextSub(\"${subtitleFile}\")")
	
	private def AvsInputInitialize(sourceFile as string):
		self.SetImport(sourceFile)
		self.SetFilter("KillAudio", "KillAudio()")

	def GetScriptContent() as string:
	"""
	使用这个方法来获取avs脚本内容。
	Returns：视频avs脚本内容。
	"""
		content as string
			
		for loadingAndImporting as string in self._loadingsAndImportings:
			content += "${loadingAndImporting}\r\n"
		for filter in self._filters:
			content += "${filter.Value}\r\n"
		return content
		
	def WriteScript(avsDestFile as string):
		File.WriteAllText(avsDestFile, self.GetScriptContent(), Text.Encoding.Default)
		
	def GetFilterStatement(filterName as string) as string:
	"""
	获取一个滤镜的语句。
	Param filterName: 滤镜的标识。
	Returns: 滤镜的语句。
	"""
		return self._filters[filterName]

	def SetFilter(filterName as string, statement as string):
		_filters[filterName] = statement

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
	Returns: 一个滤镜是否已存在。
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
		
	def SetSourceFilter(sourceFilter as VideoSourceFilter):
		if sourceFilter == VideoSourceFilter.DirectShowSource:
			SetFilter("SourceFilter", "DirectShowSource(\"${self._videoInfo.FilePath}\", audio = false)")
		elif sourceFilter == VideoSourceFilter.DSS2:
			SetImport(Path.GetFullPath("avss.dll"))
			SetFilter("SourceFilter" ,"DSS2(\"${self._videoInfo.FilePath}\")")
		elif sourceFilter == VideoSourceFilter.FFVideoSource:
			SetImport(Path.GetFullPath("ffms2.dll"))
			//FFVideoSource读某些RMVB，不写明帧率会不同步，写明帧率seeking会crash
			SetFilter("SourceFilter", "FFVideoSource(\"${self._videoInfo.FilePath}\")")
		elif sourceFilter == VideoSourceFilter.None:
			RemoveFilter("SourceFilter")
	
	def SetFrameRate(frameRate as double, convertFPS as bool):
		if frameRate != self._videoInfo.FrameRate:
			if convertFPS:
				SetFilter("FPS", "ConvertFPS(${frameRate})")
			else:
				SetFilter("FPS", "AssumeFPS(${frameRate})")
		else:
			RemoveFilter("FPS")
			
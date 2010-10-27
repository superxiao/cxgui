//1 不检查partitions的错误，GUI必须提供正确的，无任何冲突partition字串给这个类，并自行解决partition之间，partition和no-8x8dct的冲突
//2 通过psy-rd1 psy-rd2指示psy的两个值是否处于不可设状态。其他的都是选项名
namespace CXGUI.VideoEncoding

import System
import System.Collections
import System.Collections.Specialized
import System.Collections.Generic
import System.Windows.Forms
import CXGUI.External

class X264Config(VideoEncConfigBase):
"""Description of X264Config"""
	//variables
	_presets = {}
	
	
	_optionDict = OrderedDictionary[of string, X264ConfigNode]()
	
	TotalPass as int:
		set:
			_totalPass = value
			if value > 1:
				_optionDict["stats"].Locked = false
				_optionDict["slow-firstpass"].Locked = false
			else:
				_optionDict["stats"].Locked = true
				_optionDict["slow-firstpass"].Locked = true
		get:
			return _totalPass
	_totalPass as int

	[Property(CurrentPass)]
	_currentPass as int
	
	public def constructor():
		_totalPass = 1
		_currentPass = 1
		for nodeData as Array in \
		(
		("profile",		1, 0,		(null, "baseline", "main", "high")),
		("level",		1, 0,		(null, '1', '1.1', '1.2', '1.3', '2', '2.1', '2.2', '3', '3.1', '3.2', '4', '4.1', '4.2', '5', '5.1')),
		("preset",		1, 5,		("ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo")),
		("tune",		1, 0,		(null, "film", "animation", "grain", "stillimage", "psnr", "ssim", "fastdecode", "zerolatency", "touhou")),
		("qp",			0, 0,		1, 64),
		("crf",			0, 23, 	0.1, 64.0),
		("bitrate",		0, 0, 	1, 100000),
		("pass",		0, 0, 		1, 3),
	("slow-firstpass",	2, false),
		("keyint",		0, 250, 	0, 999),
		("min-keyint",	0, 25, 		0, 100),
		("no-scenecut",	2, false),
		("scenecut",	0, 40, 		0, 100),
	("intra-refresh",	2, false),
		("bframes",		0, 3, 		0, 16),
		("b-adapt",		0, 1, 		0, 2),
		("b-bias",		0, 0, 		-100, 100),
		("b-pyramid",	1, 2, 		("none", "strict", "normal")),
		("no-cabac",	2, false),
		("ref",			0, 3,		0, 16),
		("no-deblock",	2, false),
		("deblock1",	0, 0, 		-6, 6),
		("deblock2",	0, 0, 		-6, 6),
		("slices",		0, 0,		0, 100),
	("slice-max-size",	0, 0,		0, 250),
	("slice-max-mbs",	0, 0,		0, 100),
		("tff",			2, false),
		("bff",			2, false),
("constrained-intra",	2, false),
	("rc-lookahead",	0, 40,		0, 250),
		("vbv-maxrate",	0, 0,		0, 100000),
		("vbv-bufsize",	0, 0,		0, 100000),
		("vbv-init",	0, 0.9,		0, 1.0),
		("crf-max",		0, 0,		0.1, 64.0),
		("qpmin",		0, 10,		1, 51),
		("qpmax",		0, 51,		1, 51),
		("qpstep",		0, 4,		1, 51),
		("ratetol",		0, 1.00,	0.01, 100.00),
		("ipratio",		0, 1.4,		1.0, 10.0),
		("pbratio",		0, 1.3,		1.0, 10.0),
("chroma-qp-offset",	0, 0,		-12, 12),
		("aq-mode",		0, 1,		0, 2),
		("aq-strength",	0, 1.0,		0, 2.0),
		("stats",		3, "x264_2pass.log"),
		("no-mbtree",	2, false),
		("qcomp",		0, 0.6, 	0, 1.0),
		("cplxblur",	0, 20.0,	0, 999.0),
		("qblur",		0, 0.5,		0, 99.0),
		//("zones",) TODO:zone
		("qpfile",		3, ""),
		("partitions",	1, 3,		("All", "None", "Custom", "Default")),
		("p8x8",		2, true),
		("p4x4",		2, false),
		("b8x8",		2, true),
		("i8x8",		2, true),
		("i4x4",		2, true),
		("direct",		1, 1, 		("none", "spatial", "temporal", "auto")),
		("no-weightb",	2, false),
		("weightp",		0, 2,		0, 2),
		("me", 			1, 1, 		("dia", "hex", "umh", "esa", "tesa")),
		("merange",		0, 16,		4, 64),
		//("mvrange",)
		//("mvrange-thread")
		("subme",		0, 7,		0, 10),
		("psy-rd1",		0, 1,		0, 10.00),
		("psy-rd2",		0, 0, 		0, 10.00),
		("no-psy",		2, false),
	("no-mixed-refs",	2, false),
	("no-chroma-me",	2, false),
		("no-8x8dct",	2, false),
		("trellis",		0, 1,		0, 2),
	("no-fast-pskip",	2, false),
	("no-dct-decimate",	2, false),
		("nr",			0, 0,		0, 10000),
	("deadzone-inter",	0, 21,		0, 32),
	("deadzone-intra",	0, 11,		0, 32),
		("cqm",			1, 1,	("jvt", "flat")),
		("cqmfile",		3, ""),
		//cqm4 cqm8...
		("psnr",		2, false),
		("ssim",		2, false),
		("threads",		0, 0, 		1, 128), //0 == "auto"
	("thread-input",	2, false),
		//(loseless,(false,)),
		//...
		):
			node = X264ConfigNode()
			node.Name = nodeData[0]
			node.Type = Enum.Parse(NodeType, nodeData[1].ToString())

			if node.Type == NodeType.Num:
				node.DefaultNum = nodeData[2]
				node.Num = node.DefaultNum
				node.MinNum = nodeData[3]
				node.MaxNum = nodeData[4]

			elif node.Type == NodeType.StrOptionIndex:
				node.StrOptionIndex = nodeData[2]
				node.DefaultStrOptionIndex = node.StrOptionIndex
				node.StrOptions = nodeData[3] as (string)
				
			elif node.Type == NodeType.Bool:
				node.DefaultBool = nodeData[2]
				node.Bool = node.DefaultBool
			elif node.Type == NodeType.Str:
				node.Str = nodeData[2]
				node.DefaultStr = node.Str
			
			_optionDict.Add(node.Name, node)
			
		_optionDict["crf"].InUse = true
		_optionDict["qp"].InUse = false
		_optionDict["bitrate"].InUse = false

		for name in ("partitions", "p8x8", "p4x4", "b8x8", "i8x8", "i4x4", "deblock1", "deblock2", "psy-rd1", "psy-rd2"):
			_optionDict[name].Special = true
		for name in ("pass", "stats", "slow-firstpass", "ratetol", "cplxblur", "qblur"):
			_optionDict[name].Locked = true
		
		_presets = {"no-8x8dct":false, "aq-mode":1, "b-adapt":1, "bframes":3, "no-cabac":false,
						"no-deblock":false, "no-mbtree":false, "direct":1, "no-fast-pskip":false,
						"me":1, "no-mixed-refs":false, "partitions":3, "p8x8":true, "p4x4":false, "b8x8":true, "i8x8":true,
						"i4x4":true, "merange":16, "rc-lookahead":40, "ref":3, "no-scenecut":false, 
						"subme":7, "trellis":1, "no-weightb":false, "weightp":2}
	//methods
	
	def GetNode(name as string) as X264ConfigNode:
		try:
			return _optionDict[name]
		except:
			return null
		
	def GetArgument() as string: //TODO 2 3 PASS不应用slow-firstpass?
		argument = ""
		passNode = _optionDict["pass"]
		deblock = false
		psy_rd = false
		partitions = false
		if self._totalPass == 1:
			passNode.Num = 0
		elif self._totalPass == 2:
			passNode.Num = self._currentPass
		elif self._totalPass == 3:
			passNode.Num = self._currentPass
			passNode.Num = 2 if passNode.Num == 3
			passNode.Num = 3 if passNode.Num == 2

		for node as X264ConfigNode in self._optionDict.Values:
			
			try:
				if not node.Locked and node.InUse:
					if node.Type == NodeType.Num and node.Num != node.DefaultNum:
						if not node.Special:
							argument += " --${node.Name} ${node.Num}"
		
						else:
							if not deblock and node.Name.StartsWith("deblock"):
								num1 = _optionDict["deblock1"].Num
								num2 = _optionDict["deblock2"].Num
								argument += " --deblock ${num1}:${num2}"
								deblock = true
							
							elif not psy_rd and node.Name.StartsWith("psy-rd"):
								num1 = _optionDict["psy-rd1"].Num
								num2 = _optionDict["psy-rd2"].Num
								argument += " --psy-rd ${num1}:${num2}"
								psy_rd = true
				
					elif node.Type == NodeType.Bool and node.Bool != node.DefaultBool:
						if not node.Special:
							argument += " --${node.Name}"
						else: //p8x8 p4x4...
							pass
		
						
					elif node.Type == NodeType.StrOptionIndex and node.StrOptionIndex != node.DefaultStrOptionIndex:
						assert node.StrOptions != null
						if not node.Special and node.StrOptions[node.StrOptionIndex] != null:
							argument += " --${node.Name} ${node.StrOptions[node.StrOptionIndex]}"
						elif not partitions:
							if _optionDict["partitions"].StrOptionIndex == 0:
								argument += " --partitions all"
							elif _optionDict["partitions"].StrOptionIndex == 1:
								argument += " --partitions none"
							elif _optionDict["partitions"].StrOptionIndex == 2:
								argument += " --partitions "
								for name in ("p8x8", "p4x4", "b8x8", "i8x8", "i4x4"):
									pnode = _optionDict[name]
									if not pnode.Locked and pnode.Bool:
										argument += "${name},"
								if argument.EndsWith(','):
									argument = argument[:-1]
							partitions = true
							
					elif node.Str != node.DefaultStr:
						argument += " --${node.Name} ${node.Str}"
			except e:
				MessageBox.Show("发生了一个错误。\n"+node.StrOptionIndex.ToString() + '\n' + e.ToString())
		return argument

	def GetDisabledOptions() as (string):
		return (node.Name for node in self._optionDict.Values if node.Locked)

	def SetBooleanOption(name as string, value as bool):
		node = self._optionDict[name]
		if not node.Locked:
			node.Bool = value
			RefreshEnable(name)
			
	def SetNumOption(name as string, value as double):
		node = self._optionDict[name]
		if not node.Locked:
			value = node.MinNum if node.MinNum > value
			value = node.MaxNum if node.MaxNum < value
			node.Num = value
			SettleConflicts(name, value)
		
	def SetStringOption(name as string, value as string):
		node = self._optionDict[name]
		if not node.Locked:
			node.Str = value
		
	def SelectStringOption(name as string, index as int):
		node = self._optionDict[name]
		if not node.Locked:
			node.StrOptionIndex = index
			SettleConflicts(name, index)

	private def SettleConflicts(lastSetName as string, lastSetValue as double):
		try:
			if lastSetName == "preset": 
				_presets = {"no-8x8dct":false, "aq-mode":1, "b-adapt":1, "bframes":3, "no-cabac":false,
							"no-deblock":false, "no-mbtree":false, "direct":1, "no-fast-pskip":false,
							"me":1, "no-mixed-refs":false, "partitions":3, "p8x8":true, "p4x4":false, "b8x8":true, "i8x8":true,
							"i4x4":true, "merange":16, "rc-lookahead":40, "ref":3, "no-scenecut":false, 
							"subme":7, "trellis":1, "no-weightb":false, "weightp":2}
				if lastSetValue == 0://ultrafast
					for d as DictionaryEntry in {
								"no-8x8dct":true, "aq-mode":0, "b-adapt":0, "bframes":0, "no-cabac":true,
								"no-deblock":true, "no-mbtree":true, "me":0, "no-mixed-refs":true, "partitions":1,
								 "p8x8":false, "b8x8":false, "p4x4":false, "i8x8":false, "i4x4":false, "ref":1,
								 "no-scenecut":true, "subme":0, "trellis":0, "no-weightb":true,
								 "weightp":0}:
						_presets[d.Key] = d.Value
								
				elif lastSetValue == 1://superfast
					for d as DictionaryEntry in {
								"no-mbtree":true, "me":0, "no-mixed-refs":true, "partitions":3, "p8x8":false, "b8x8":false,
								"p4x4":false, "i8x8":true, "i4x4":true, "ref":1, "subme":1, "trellis":0, "weightp":0}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 2://veryfast
					for d as DictionaryEntry in {
								"no-mbtree":true, "no-mixed-refs":true, "ref":1, "subme":2, "trellis":0,
								"weightp":0}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 3://faster
					for d as DictionaryEntry in {
								"no-mixed-refs":true, "rc-lookahead":20, "ref":2, "subme":4, "weightp":1}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 4://fast
					for d as DictionaryEntry in {
								"rc-lookahead":30, "ref":2, "subme":6}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 5://medium
					pass
				elif lastSetValue == 6://slow
					for d as DictionaryEntry in {
								"b-adapt":2, "direct":3, "me":2, "rc-lookahead":50,"ref":5,
								"subme":8}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 7://slower
					for d as DictionaryEntry in {
							"b-adapt":2, "direct":3, "me":2, "partitions":0, "p8x8":true, "p4x4":true, "b8x8":true, "i8x8":true,
							"i4x4":true, "rc-lookahead":60, "ref":8, "subme":9, "trellis":2}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 8://veryslow
					for d as DictionaryEntry in {
							"b-adapt":2, "bframes":8, "direct":3, "me":2, "merange":24, "partitions":0, "p8x8":true,
							"p4x4":true, "b8x8":true, "i8x8":true, "i4x4":true, "ref":16, "subme":10,
							"trellis":2, "rc-lookahead":60}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 9://placebo
					for d as DictionaryEntry in  {
							"b-adapt":2, "bframes":16, "direct":3, "no-fast-pskip":true,
							"me":4, "merange":24, "partitions":0, "p4x4":true, "b8x8":true, "i8x8":true, "i4x4":true,
							"ref":16, "subme":10, "trellis":2, "rc-lookahead":60}://slow-firstpass
						_presets[d.Key] = d.Value 
				SetOptionsAndDefaults(_presets)
				if _optionDict["tune"].StrOptionIndex > 1:
					SettleConflicts("tune", _optionDict["tune"].StrOptionIndex)	
			elif lastSetName == "tune": //以下全部检查完毕
				//恢复与preset冲突的项
				SetOptionsAndDefaults(
				{"bframes":_presets["bframes"], "no-cabac":_presets["no-cabac"], "ref":_presets["ref"],
				 "no-deblock":_presets["no-deblock"], "deblock1":0, "deblock2":0, "rc-lookahead":_presets["rc-lookahead"],
				 "ipratio":1.4, "pbratio":1.3, "aq-mode":_presets["aq-mode"], "aq-strength":1.0, 
				 "qcomp":0.6, "no-weightb":_presets["no-weightb"], "weightp":_presets["weightp"],
				 "psy-rd1":1, "psy-rd2":0,"no-psy":false, "no-dct-decimate":false, "deadzone-inter":21,
				 "deadzone-intra":11}
									  )
				if lastSetValue == 0:
					pass
				elif lastSetValue == 1://film
					SetOptionsAndDefaults({"deblock1":-1, "deblock2":-1, "psy-rd2":0.15})
				elif lastSetValue == 2://animation
		
					bframes = cast(int, _presets["bframes"]) + 2
					bframes = 16 if bframes > 16
					refNum = cast(int, _presets["ref"]) * 2
					refNum = 16 if refNum > 16
					SetOptionsAndDefaults({"bframes":bframes, "ref":refNum, "deblock1":1, "deblock2":1, "aq-strength":0.6, "psy-rd1":0.4})
	
				elif lastSetValue == 3://grain
					SetOptionsAndDefaults({"deblock1":-2, "deblock2":-2, "ipratio":1.1, "pbratio":1.1, "aq-strength":0.5,
										   "qcomp":0.8, "psy-rd2":0.25, "no-dct-decimate":true, "deadzone-inter":6,
										   "deadzone-intra":6})
				elif lastSetValue == 4://stillimage
					SetOptionsAndDefaults({"deblock1":-3, "deblock2":-3, "aq-strength":1.2, "psy-rd1":2, "psy-rd2":0.7})
				elif lastSetValue == 5://psnr
					SetOptionsAndDefaults({"aq-mode":0, "no-psy":true})
				elif lastSetValue == 6://ssim
					SetOptionsAndDefaults({"aq-mode":2, "no-psy":true})
				elif lastSetValue == 7://fastdecode
					SetOptionsAndDefaults({"no-cabac":true, "no-deblock":true, "no-weightb":true, "weightp":0})
				elif lastSetValue == 8://zerolatency
					SetOptionsAndDefaults({"bframes":0, "rc-lookahead":0})//TODO --force-cfr --sync-lookahead 0 --sliced-threads
				elif lastSetValue == 9://TODO touhou
					SetOptionsAndDefaults({})
					
			elif lastSetName == "qp":
				self._optionDict["qp"].InUse = true
				self._optionDict["crf"].InUse = false
				self._optionDict["bitrate"].InUse = false
			elif lastSetName == "crf":
				self._optionDict["crf"].InUse = true
				self._optionDict["qp"].InUse = false
				self._optionDict["bitrate"].InUse = false
			elif lastSetName == "bitrate":
				self._optionDict["bitrate"].InUse = true
				self._optionDict["crf"].InUse = false
				self._optionDict["qp"].InUse = false
					
			elif lastSetName == "subme" and lastSetValue == 10: 
				if self._optionDict["trellis"].Locked or\
				   self._optionDict["aq-mode"].Locked or\
				   self._optionDict["trellis"].Num != 2 or self._optionDict["aq-mode"].Num == 0:
					MessageBox.Show("subme 10 requires trellis = 2, aq-mode > 0 to take effect.")
			elif lastSetName == "partitions":
				if lastSetValue == 0:
					for name in ("p8x8", "p4x4", "b8x8", "i8x8", "i4x4"):
						if not _optionDict[name].Locked:
							_optionDict[name].Bool = true			
				elif lastSetValue == 1:
					for name in ("p8x8", "p4x4", "b8x8", "i8x8", "i4x4"):
						if not _optionDict[name].Locked:
							_optionDict[name].Bool = false
				elif lastSetValue == 3:
					for name in ("p8x8", "p4x4", "b8x8", "i8x8", "i4x4"):
						if not _optionDict[name].Locked:
							_optionDict[name].Bool = _optionDict[name].DefaultBool
				RefreshEnable(lastSetName)
	
			elif lastSetName == "p8x8":
				if not _optionDict["p8x8"].Bool:
					_optionDict["p4x4"].Bool = false
				RefreshEnable(lastSetName)
			elif lastSetName == "no-8x8dct":
				if _optionDict["no-8x8dct"].Bool:
					_optionDict["i8x8"].Bool = false
				RefreshEnable(lastSetName)
		except e:
			MessageBox.Show("发生了一个错误。\n"+lastSetName +'\n'+e.ToString())
			
		//TODO thread-input

	private def RefreshEnable(lastSetOption as string): 
		try:
			//Options related to rate control		
			bitrateOptions = ("pass", "ratetol", "cplxblur", "qblur")
			bitrateAndPassOptions = bitrateOptions + ("stats", "slow-firstpass")
			variableQuantizerOptions = ("qpmin", "qpmax", "qpstep", "ipratio", "pbratio", "chroma-qp-offset", "qcomp")
			if not _optionDict["bitrate"].Locked:
				Enable(bitrateOptions)
				Enable(variableQuantizerOptions)
				Disable("crf-max")
				
			elif not _optionDict["crf"].Locked:
				Enable("crf-max")
				Enable(variableQuantizerOptions)
				Disable(bitrateAndPassOptions)
				
			elif not _optionDict["qp"].Locked:
				Disable(variableQuantizerOptions)
				Disable(bitrateAndPassOptions)
				Disable("crf-max")
	
			//scenecut
			if _optionDict["no-scenecut"]:
				Disable("scenecut")
			else:
				Enable("scenecut")
			
			//Options related to profile
			//no-cabac->trellis->psy-rd2
			if _optionDict["profile"].StrOptionIndex == 1:
				_optionDict["no-8x8dct"].Bool = true
				_optionDict["bframes"].Num = 0
				_optionDict["no-cabac"].Bool = true
				_optionDict["cqm"].StrOptionIndex = 1
				_optionDict["weightp"].Num = 0
				_optionDict["tff"].Bool = false
				_optionDict["bff"].Bool = false
				Disable("no-8x8dct", "bframes", "no-cabac", "cqm", 
							  "weightp", "tff", "bff")
			elif _optionDict["profile"].StrOptionIndex == 2:
				_optionDict["no-8x8dct"].Bool = true
				_optionDict["cqm"].StrOptionIndex = 1
				Disable("no-8x8dct", "cqm")
			else:
				Enable(true, "no-8x8dct", "bframes", "no-cabac", "cqm", 
							    "weightp", "tff", "bff")
	
			//trellis
			if _optionDict["no-cabac"].Bool:
				_optionDict["trellis"].Num = 0
				Disable("trellis")
			else:
				Enable(true, "trellis")
				
			//psy-rd
			if _optionDict["subme"].Num < 6 or _optionDict["no-psy"].Bool:
				Disable("psy-rd1")
			else:
				Enable("psy-rd1")
			if _optionDict["trellis"].Num == 0 or _optionDict["no-psy"].Bool:
				Disable("psy-rd2")
			else:
				Enable("psy-rd2")
	
			//no-deblock
			if _optionDict["no-deblock"].Bool:
				Disable("deblock1", "deblock2")
			else:
				Enable("deblock1", "deblock2")
	
			//aq-strength
			if _optionDict["aq-mode"].Num == 0:
				Disable("aq-strength")
			else:
				Enable("aq-strength")
				
			//partitions
			allDisable = (self._optionDict["partitions"].StrOptionIndex in (0,1,3))
			if allDisable:
				Disable("p8x8", "b8x8", "i4x4")
			else:
				Enable("p8x8", "b8x8", "i4x4")
			if not _optionDict["p8x8"].Bool or allDisable:
				Disable("p4x4")
			else:
				Enable("p4x4")
			if _optionDict["no-8x8dct"].Bool or allDisable:
				Disable("i8x8")
			else:
				Enable("i8x8")
		except e:
			MessageBox.Show("发生了一个错误。\n"+e.ToString())
		
	private def Disable(options as IEnumerable[of string]):
		for option in options:
			_optionDict[option].Locked = true
	private def Disable(*options as (string)):
		for option in options:
			_optionDict[option].Locked = true

	private def Enable(options as IEnumerable[of string]):
		for option in options:
			_optionDict[option].Locked = false
			
	private def Enable(*options as (string)):
		for option in options:
			_optionDict[option].Locked = false

	private def Enable(restoreDefault as bool, *options as (string)):
		if restoreDefault:
			for option in options:
				if _optionDict[option].Locked:
					RestoreDefault(option)
		Enable(options)

					
	private def SetOptionsAndDefaults(namesAndValues as Hash): //有不检查FreezedOptions而直接修改参数的权利，但只能用于preset和tune
		for nameAndValue as DictionaryEntry in namesAndValues:
			SetOptionAndDefault(nameAndValue.Key, nameAndValue.Value)
			RefreshEnable(nameAndValue.Key)
			
	private def SetOptionAndDefault(name as string, value as object):
		node = self._optionDict[name]
		if node.Type == NodeType.Bool:
			node.Bool = value
			node.DefaultBool = node.Bool
		elif node.Type == NodeType.Num:
			node.Num = value
			node.DefaultNum = node.Num
		elif node.Type == NodeType.StrOptionIndex:
			node.StrOptionIndex = value
			node.DefaultStrOptionIndex = node.StrOptionIndex
		elif node.Type == NodeType.Str:
			node.Str = value
			node.DefaultStr = node.Str
			
#	private def SetOption(name as string, value as object):
#		node = self._optionDict[name]
#		if node.Type == NodeType.Bool:
#			node.Bool = value
#		elif node.Type == NodeType.Num:
#			node.Num = value
#		elif node.Type == NodeType.StrOptionIndex:
#			node.StrOptionIndex = value
#		elif node.Type == NodeType.Str:
#			node.Str = value
			
	private def RestoreDefault(name as string):
		node = self._optionDict[name]
		if node.Type == NodeType.Bool:
			node.Bool = node.DefaultBool
		elif node.Type == NodeType.Num:
			node.Num = node.DefaultNum
		elif node.Type == NodeType.StrOptionIndex:
			node.StrOptionIndex = node.DefaultStrOptionIndex
		elif node.Type == NodeType.Str:
			node.Str = node.DefaultStr

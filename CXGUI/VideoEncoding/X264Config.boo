//1 不检查partitions的错误，GUI必须提供正确的，无任何冲突partition字串给这个类，并自行解决partition之间，partition和no-8x8dct的冲突
//2 通过psy-rd1 psy-rd2指示psy的两个值是否处于不可设状态。其他的都是选项名
namespace CXGUI.VideoEncoding

import System
import System.Collections
import System.Collections.Specialized
import System.Windows.Forms //test

class X264Config(VideoEncConfigBase):
"""Description of X264Config"""
	//variables
	_defaultsAndRanges = OrderedDictionary()
	_settingsInUse = OrderedDictionary()
	_freezedOptions = []
	_presets = {}
	
	public def constructor():
		for option, defaultAndRange in \
		(
		("profile", (null ,(null, "baseline", "main", "high"))),
		("level", (null, (null, '1', '1.1', '1.2', '1.3', '2', '2.1', '2.2', '3', '3.1', '3.2', '4', '4.1', '4.2', '5', '5.1'))),
		("preset", ("medium", ("ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"))),
		("tune", (null, (null, "film", "animation", "grain", "stillimage", "psnr", "ssim", "fastdecode", "zerolatency", "touhou"))),
		("qp", (null, 1, 64)),
		("crf", (23.0, 0.1, 64.0)),
		("bitrate", (null, 1, 100000)),
		("pass", (null, 1, 3)),
		("_pass", (1, 1, 3)),
		("slow-firstpass", (false,)),
		("keyint", (250, 0, 999)),
		("min-keyint", (25, 0, 100)),
		("no-scenecut", (false,)),
		("scenecut", (40, 0, 100)), //TODO: 实际是keyint
		("intra-refresh", (false,)),
		("bframes", (3, 0, 16)),
		("b-adapt", (1, 0, 2)),
		("b-bias", (0, -100, 100)),
		("b-pyramid", ("normal", ("none", "strict", "normal"))),
		("no-cabac", (false,)),
		("ref", (3, 0, 16)),
		("no-deblock", (false,)),
		("deblock", ("0:0", 0, 6)),
		("_deblock", ((0, 0),)),
		("slices", (0, 0, 100)),
		("slice-max-size", (0, 0, 250)),
		("slice-max-mbs", (0, 0, 100)),
		("tff", (false,)),
		("bff", (false,)),
		("constrained-intra", (false,)),
		("rc-lookahead", (40, 0, 250)),
		("vbv-maxrate", (0, 0, 100000)),
		("vbv-bufsize", (0, 0, 100000)),
		("vbv-init", (0.9, 0, 1.0)),
		("crf-max", (null, 0.1, 64.0)),
		("qpmin", (10, 1, 51)),
		("qpmax", (51, 1, 51)),
		("qpstep", (4, 1, 51)),
		("ratetol", (1.00, 0.01, 100.00)),
		("ipratio", (1.4, 1.0, 10.0)),
		("pbratio", (1.3, 1.0, 10.0)),
		("chroma-qp-offset", (0, -12, 12)),
		("aq-mode", (1, 0, 2)),
		("aq-strength", (1.0, 0, 2.0)),
		("stats", ("x264_2pass.log",)),
		("no-mbtree",(false,)),
		("qcomp", (0.6, 0, 1.0)),
		("cplxblur", (20.0, 0, 999.0)),
		("qblur", (0.5, 0, 99.0)),
		//("zones",) TODO:zone
		("qpfile", (null,)),
		("partitions", ("p8x8,b8x8,i8x8,i4x4",)),
		("_partitions", (["p8x8", "b8x8", "i8x8", "i4x4"],)),
		("direct", ("spatial",("none", "spatial", "temporal", "auto"))),
		("no-weightb", (false,)),
		("weightp", (2, 0, 2)),
		("me", ("hex", ("dia", "hex", "umh", "esa", "tesa"))),
		("merange", (16, 4, 64)),
		//("mvrange",)
		//("mvrange-thread")
		("subme", (7, 0, 10)),
		("psy-rd", ("1:0", 0, 10.00)),
		("_psy-rd", ((1.0, 0),)),
		("no-psy", (false,)),
		("no-mixed-refs", (false,)),
		("no-chroma-me", (false,)),
		("no-8x8dct", (false,)),
		("trellis", (1, 0, 2)),
		("no-fast-pskip", (false,)),
		("no-dct-decimate", (false,)),
		("nr", (0, 0, 10000)),
		("deadzone-inter", (21, 0, 32)),
		("deadzone-intra", (11, 0, 32)),
		("cqm", ("flat",("jvt", "flat"))),
		("cqmfile", (null,)),
		//cqm4 cqm8...
		("psnr", (false,)),
		("ssim", (false,)),
		("threads", (null, 1, 128)), //null == "auto"
		("thread-input", (false,)),
		//(loseless,(false,)),
		//...
		):
			_defaultsAndRanges.Add(option, defaultAndRange) //TODO 有否更合适的处理数据的方法

		for d as DictionaryEntry in _defaultsAndRanges:
			_settingsInUse.Add(d.Key, (d.Value as Array)[0])
	
		Freeze("pass", "stats", "slow-firstpass", "ratetol", "cplxblur", "qblur")
		
		_presets = {"no-8x8dct":false, "aq-mode":1, "b-adapt":1, "bframes":3, "no-cabac":false,
					"no-deblock":false, "no-mbtree":false, "direct":"spatial", "no-fast-pskip":false,
					"me":"hex", "no-mixed-refs":false, "partitions":"p8x8,b8x8,i8x8,i4x4", "merange":16,
					"rc-lookahead":40, "ref":3, "no-scenecut":false, "subme":7, "trellis":1,
					"no-weightb":false, "weightp":2, "_partitions":["p8x8","b8x8","i8x8","i4x4"]}
	
	//methods
	def GetSettingsDict() as Hash:
		dict = {}
		for entry as DictionaryEntry in _settingsInUse:
			dict.Add(entry.Key, entry.Value)
		return dict

	def GetSettings() as string:
		settings = ""
		for _settingInUse as DictionaryEntry in _settingsInUse:
			if _settingInUse.Value != null\
			   and _settingInUse.Value != (_defaultsAndRanges[_settingInUse.Key] as Array)[0]\
			   and not _freezedOptions.Contains(_settingInUse.Key)\
			   and _settingInUse.Value != false\
			   and not (_settingInUse.Key as string).StartsWith("_"):
				if _settingInUse.Value == true:
					settings += " --${_settingInUse.Key}"
				else:
					settings += " --${_settingInUse.Key} ${_settingInUse.Value}"
		return settings

	def GetFreezedOptions() as Array:
		return array(_freezedOptions)

	def SetBooleanOption(name as string, value as bool?):
		if len(_defaultsAndRanges[name])==1:
			SetOption(name, value)
			SettleConflicts(name, value)
			
	def SetIntegerOption(name as string, value as int?)://TODO null的情况
		defaultAndRange = _defaultsAndRanges[name] as Array
		if len(defaultAndRange) == 3 and defaultAndRange[1].GetType() == int:
			min = cast(int, defaultAndRange[1])
			max = cast(int, defaultAndRange[2])
			value = min if min > value
			value = max if max < value
			SetOption(name, value)
			SettleConflicts(name, value)
				
	def SetFloatOption(name as string, value as double?):
		defaultAndRange = _defaultsAndRanges[name] as Array
		if len(defaultAndRange) == 3 and defaultAndRange[1].GetType() == double:
			min = cast(double, defaultAndRange[1])
			max = cast(double, defaultAndRange[2])
			value = min if min > value
			value = max if max < value
			SetOption(name, value)
			SettleConflicts(name, value)
			
	def SetStringOption(name as string, value as string):
		SetOption(name, value)
		SettleConflicts(name, value)	
			
	def SelectStringOption(name as string, index as int):
		defaultAndRange = _defaultsAndRanges[name] as Array
		if len(defaultAndRange) == 2:
			value = (defaultAndRange[1] as Array)[index]
		SetOption(name, value)
		SettleConflicts(name, index)
			
	def SetDeblock(alpha as int, beta as int):
		defaultAndRange = _defaultsAndRanges["deblock"] as Array
		min = defaultAndRange[1]
		max = defaultAndRange[2]
		alpha = My.Middle(min, max, alpha)
		beta = My.Middle(min, max, beta)
		value = "${alpha}:${beta}"
		SetOption("deblock", value)
		SetOption("_deblock", (alpha, beta))
		SettleConflicts("deblock", value)
		
	def SetPsyRD(rd as double, trellis as double):
		defaultAndRange = _defaultsAndRanges["psy-rd"] as Array
		min = defaultAndRange[1]
		max = defaultAndRange[2]
		rd = My.Middle(min, max, rd)
		trellis = My.Middle(min, max, trellis)
		value = "${rd}:${trellis}"
		SetOption("psy-rd", value)
		SetOption("_psy-rd", (rd, trellis))
		SettleConflicts("psy-rd", value)
	
	def SetPartitions(*values as (string)):
		value = join(values, ',')
		SetOption("partitions", value)
		SetOption("_partitions", List(values))
		SettleConflicts("_partitions", value)
		
	private def SetOption(name as string, value as object):
		if not _freezedOptions.Contains(name):
			if _settingsInUse.Contains(name):
				_settingsInUse[name] = value
				RefreshFreezedOptions(name, value)

	private def RefreshFreezedOptions(lastSetOption as string, lastSetValue): 

		//Options related to rate control		
		bitrateOptions = ("pass", "ratetol", "cplxblur", "qblur")
		bitrateAndPassOptions = bitrateOptions + ("stats", "slow-firstpass")
		variableQuantizerOptions = ("qpmin", "qpmax", "qpstep", "ipratio", "pbratio", "chroma-qp-offset", "qcomp")
		if _settingsInUse["bitrate"] != null:
			Unfreeze(bitrateOptions)
			Unfreeze(variableQuantizerOptions)
			_settingsInUse["crf-max"] = null
			Freeze("crf-max")
			
		elif _settingsInUse["crf"] != null:
			Unfreeze("crf-max")
			Unfreeze(variableQuantizerOptions)
			_settingsInUse["pass"] = null
			Freeze(bitrateAndPassOptions)
			
		elif _settingsInUse["qp"] != null:
			Freeze(variableQuantizerOptions)
			_settingsInUse["pass"] = null
			Freeze(bitrateAndPassOptions)
			_settingsInUse["crf-max"] = null
			Freeze("crf-max")

		if _settingsInUse["pass"] != null:
			Unfreeze("stats")
			if _settingsInUse["pass"] == 1:
				Unfreeze("slow-firstpass")
			else:
				Freeze("slow-firstpass")
		else:
			Freeze("stats")

		//scenecut
		if _settingsInUse["no-scenecut"]:
			Freeze("scenecut")
		else:
			Unfreeze("scenecut")
		
		//Options related to profile
		//no-cabac->trellis->psy-rd2
		if _settingsInUse["profile"] == "baseline":
			_settingsInUse["no-8x8dct"] = true
			_settingsInUse["bframes"] = 0
			_settingsInUse["no-cabac"] = true
			_settingsInUse["cqm"] = "flat"
			_settingsInUse["weightp"] = 0
			_settingsInUse["tff"] = false
			_settingsInUse["bff"] = false
			Freeze("no-8x8dct", "bframes", "no-cabac", "cqm", 
						  "weightp", "tff", "bff")
		elif _settingsInUse["profile"] == "main":
			_settingsInUse["no-8x8dct"] = true
			_settingsInUse["cqm"] = "flat"
			Freeze("no-8x8dct", "cqm")
		else:
			Unfreeze(true, "no-8x8dct", "bframes", "no-cabac", "cqm", 
						    "weightp", "tff", "bff")

		//trellis
		if _settingsInUse["no-cabac"]:
			_settingsInUse["trellis"] = 0
			Freeze("trellis")
		else:
			Unfreeze(true, "trellis")
			
		//psy-rd
		if cast(int, _settingsInUse["subme"]) < 6 or _settingsInUse["no-psy"]:
			Freeze("psy-rd1")
		else:
			Unfreeze("psy-rd1")
		if _settingsInUse["trellis"] == 0 or _settingsInUse["no-psy"]:
			Freeze("psy-rd2")
		else:
			Unfreeze("psy-rd2")

		//no-deblock
		if _settingsInUse["no-deblock"]:
			Freeze("deblock", "_deblock")
		else:
			Unfreeze("deblock", "_deblock")

		//aq-strength
		if _settingsInUse["aq-mode"] == 0:
			Freeze("aq-strength")
		else:
			Unfreeze("aq-strength")
			
		//partitions
		partitions as List = _settingsInUse["_partitions"]
		freezeAll = (partitions.Contains("none") or partitions.Contains("all"))
		if freezeAll:
			Freeze("p8x8", "b8x8", "i4x4")
		else:
			Unfreeze("p8x8", "b8x8", "i4x4")
		if not partitions.Contains("p8x8") or freezeAll:
			Freeze("p4x4")
		else:
			Unfreeze("p4x4")
		if _settingsInUse["no-8x8dct"] or freezeAll:
			Freeze("i8x8")
		else:
			Unfreeze("i8x8")
		_settingsInUse["partitions"] = join((p for p in partitions if p not in _freezedOptions), ',')
		
	private def Freeze(options as IEnumerable):
		for option in options:
			_freezedOptions.AddUnique(option)
	private def Freeze(*options as (string)):
		Freeze(options)	

	private def Unfreeze(options as IEnumerable):
		for option in options:
			if _freezedOptions.Contains(option):
				_freezedOptions.Remove(option)
	private def Unfreeze(*options as (string)):
		Unfreeze(options)
	private def Unfreeze(restoreDefault as bool, *options as (string)):
		if restoreDefault:
			for option in options:
				if _freezedOptions.Contains(option) and _defaultsAndRanges.Contains(option):
					_settingsInUse[option] = (_defaultsAndRanges[option] as Array)[0]
		Unfreeze(options)

	private def SettleConflicts(lastSetName as string, lastSetValue as object):
		if not _freezedOptions.Contains(lastSetName) and _defaultsAndRanges.Contains(lastSetName):
			if lastSetName == "preset": //TODO 整理一个总表
				_presets = {"no-8x8dct":false, "aq-mode":1, "b-adapt":1, "bframes":3, "no-cabac":false,
							"no-deblock":false, "no-mbtree":false, "direct":"spatial", "no-fast-pskip":false,
							"me":"hex", "no-mixed-refs":false, "partitions":"p8x8,b8x8,i8x8,i4x4", "merange":16,
							"rc-lookahead":40, "ref":3, "no-scenecut":false, "subme":7, "trellis":1,
							"no-weightb":false, "weightp":2, "_partitions":["p8x8","b8x8","i8x8","i4x4"]}
				if lastSetValue == 0://ultrafast
					for d as DictionaryEntry in {
								"no-8x8dct":true, "aq-mode":0, "b-adapt":0, "bframes":0, "no-cabac":true,
								"no-deblock":true, "no-mbtree":true, "me":"dia", "no-mixed-refs":true,
								"partitions":"none", "ref":1, "no-scenecut":true, "subme":0, "trellis":0,
								"no-weightb":true, "weightp":0, "_partitions":["none"]}:
						_presets[d.Key] = d.Value
								
				elif lastSetValue == 1://superfast
					for d as DictionaryEntry in {
								"no-mbtree":true, "me":"dia", "no-mixed-refs":true, "partitions":"i8x8,i4x4",
								"ref":1, "subme":1, "trellis":0, "weightp":0, "_partitions":["i8x8","i4x4"]}:
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
								"b-adapt":2, "direct":"auto", "me":"umh", "rc-lookahead":50,"ref":5,
								"subme":8}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 7://slower
					for d as DictionaryEntry in {
							"b-adapt":2, "direct":"auto", "me":"umh", "partitions":"all",
							"rc-lookahead":60, "ref":8, "subme":9, "trellis":2, "_partitions":["all"]}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 8://veryslow
					for d as DictionaryEntry in {
							"b-adapt":2, "bframes":8, "direct":"auto", "me":"umh", "merange":24,
							"partitions":"all", "ref":16, "subme":10, "trellis":2, "rc-lookahead":60,
							"_partitions":["all"]}:
						_presets[d.Key] = d.Value
				elif lastSetValue == 9://placebo
					for d as DictionaryEntry in  {
							"b-adapt":2, "bframes":16, "direct":"auto", "no-fast-pskip":true,
							"me":"tesa", "merange":24, "partitions":"all", "ref":16, "subme":10,
							"trellis":2, "rc-lookahead":60, "_partitions":["all"]}://slow-firstpass
						_presets[d.Key] = d.Value 
				SetOptionsAndDefaults(_presets)
				
			elif lastSetName == "tune": //以下全部检查完毕
				//恢复与preset冲突的项
				SetOptionsAndDefaults(
				{"bframes":_presets["bframes"], "no-cabac":_presets["no-cabac"], "ref":_presets["ref"],
				 "no-deblock":_presets["no-deblock"], "deblock":"0:0", "rc-lookahead":_presets["rc-lookahead"],
				 "ipratio":1.4, "pbratio":1.3, "aq-mode":_presets["aq-mode"], "aq-strength":1.0, 
				 "qcomp":0.6, "no-weightb":_presets["no-weightb"], "weightp":_presets["weightp"],
				 "psy-rd":"1:0", "no-psy":false, "no-dct-decimate":false, "deadzone-inter":21,
				 "deadzone-intra":11, "_deblock":(0, 0), "_psy-rd":(1.0, 0)}
									  )
				if lastSetValue == 1://film
					SetOptionsAndDefaults({"deblock":"-1:-1", "psy-rd":"1:0.15", "_deblock":(-1, -1), "_psy-rd":(1.0, 0.15)})
				elif lastSetValue == 2://animation

					if (newDefault = cast(int, _presets["bframes"])+2) <= 16:
						(_defaultsAndRanges["bframes"] as Array)[0] = newDefault
						_settingsInUse["bframes"] = My.Middle(cast(int, _settingsInUse["bframes"])+2, newDefault, 16)

					if (newDefault = cast(int, _presets["ref"])*2) <= 16 and cast(int, _presets["ref"]) != 1:
						(_defaultsAndRanges["ref"] as Array)[0] = newDefault
						_settingsInUse["ref"] = My.Middle(cast(int, _settingsInUse["ref"])*2, newDefault, 16)

					SetOptionsAndDefaults({"deblock":"1:1", "aq-strength":0.6, "psy-rd":"0.4:0", "_deblock":(1, 1), "_psy-rd":(0.4, 0)})
				elif lastSetValue == 3://grain
					SetOptionsAndDefaults({"deblock":"-2:-2", "ipratio":1.1, "pbratio":1.1, "aq-strength":0.5,
										   "qcomp":0.8, "psy-rd":"1:0.25", "no-dct-decimate":true, "deadzone-inter":6,
										   "deadzone-intra":6, "_deblock":(-2, -2), "_psy-rd":(1.0, 0.25)})
				elif lastSetValue == 4://stillimage
					SetOptionsAndDefaults({"deblock":"-3:-3", "aq-strength":1.2, "psy-rd":"2:0.7", "_deblock":(-3, -3), "_psy-rd":(2.0, 0.7)})
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
				
			elif lastSetName == "qp" and lastSetValue != null:
				SetOption("crf", null)
				SetOption("bitrate", null)
			elif lastSetName == "crf" and lastSetValue != null:
				SetOption("qp", null)
				SetOption("bitrate", null)
			elif lastSetName == "bitrate" and lastSetValue != null:
				SetOption("qp", null)
				SetOption("crf", null)
				
			if lastSetName == "subme" and lastSetValue == 10: //TODO
				if _freezedOptions.Contains("trellis") or\
				   _freezedOptions.Contains("aq-mode") or\
				   _settingsInUse["trellis"] != 2 or _settingsInUse["aq-mode"] == 0:
					MessageBox.Show("subme 10 requires trellis = 2, aq-mode > 0 to take effect.")
			
			if lastSetName == "_pass":
				if cast(int, lastSetValue) > 1:
					SetOption("pass", 1)
				else:
					SetOption("pass", null)
		
		//TODO thread-input
					
	private def SetOptionsAndDefaults(namesAndValues as Hash): //有不检查FreezedOptions而直接修改参数的权利，但只能用于preset和tune
		for nameAndValue as DictionaryEntry in namesAndValues:
			_settingsInUse[nameAndValue.Key] = nameAndValue.Value
			RefreshFreezedOptions(nameAndValue.Key, nameAndValue.Value)
			(_defaultsAndRanges[nameAndValue.Key] as Array)[0] = nameAndValue.Value

namespace CXGUI.Avisynth

import System
import System.IO
import System.Text
import My

class SubStyleWriter:
"""Description of Subtitler"""

	_subtitle as string
	_subConfig as SubtitleConfig
	_tempFiles as List[of string]
	
	public def constructor(subtitle as string, subConfig as SubtitleConfig):
		_subtitle = subtitle
		_subConfig = subConfig
		_tempFiles = List[of string](2)
	public def Write():
		_tempFiles.Clear()
		
		if Path.GetExtension(_subtitle).ToLower() in (".ssa", ".ass"):
			_subtitle = GenerateSrtFromAss(_subtitle)
			_tempFiles.Add(_subtitle)
		styleFile = _subtitle + ".style"
		styles = _subConfig.GetStyles()
		content =\
"""
[Script Info]
ScriptType: v4.00+
Collisions: Normal
Timer: 100.0000
""" + styles
		File.WriteAllText(styleFile, content, Encoding.UTF8)
		_tempFiles.Add(styleFile)
		
	private def GenerateSrtFromAss(assFile as string) as string:
		srt = Path.ChangeExtension(assFile, 'srt')
		srt = GetUniqueName(srt)
		num = 0
		nl = Environment.NewLine

		GetTime = do(assTime as string) as string:
			assTime = assTime.Trim()
			return "0${assTime}0".Replace('.', ',')
		
		srtContent = ""
		for assLine as string in File.ReadAllLines(assFile):
			if assLine.StartsWith("Dialogue:"):
				elems = assLine.Split(char(','))
				line = join(elems[9:], char(','))
				srtContent += "${nl}${num}${nl}${GetTime(elems[1])} --> ${GetTime(elems[2])}${nl}${line}${nl}"
				num++
		File.WriteAllText(srt, srtContent, Encoding.UTF8)
		return srt
	
	public def CleanUp():
		for file in _tempFiles:
			File.Delete(file)
		_tempFiles.Clear()
		
			



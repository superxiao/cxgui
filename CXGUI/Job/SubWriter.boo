namespace CXGUI.Job

import System
import System.IO
import System.Text
import My

class SubWriter:
"""Description of Subtitler"""

	_subtitle as string
	_subConfig as SubtitleConfig
	
	public def constructor(subtitle as string, subConfig as SubtitleConfig, avsConfig as AvisynthConfig):
		_subtitle = subtitle
		_subConfig = subConfig
		
	public def Write():
		if Path.GetExtension(_subtitle).ToLower() in (".ssa", ".ass"):
			_subtitle = GenerateSrtFromAss(_subtitle)
		styleFile = _subtitle + ".style"
		if _subConfig != null:
			styles = _subConfig.GetStyles()
			content =\
"""
[Script Info]
ScriptType: v4.00+
Collisions: Normal
Timer: 100.0000
""" + styles
			File.WriteAllText(styleFile, content, Encoding.UTF8)
		
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
		
			



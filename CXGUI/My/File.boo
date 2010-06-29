namespace My

import System
import System.IO
import System.Windows.Forms
def IsSameFile(path1 as string, path2 as string) as bool:
	path1.Replace("/", "\\")
	path2.Replace("/", "\\")
	return path1.ToLower() == path2.ToLower()
	
def Exists(path as string) as bool:
	if File.Exists(path) or Directory.Exists(path):
		return true
	return false
	
def GetUniqueName(path as string) as string:
	if Exists(path):
		name = Path.GetFileNameWithoutExtension(path)
		re = /(.*)_([0-9]+)\s*$/
		m = re.Match(name)
		if m.Success:
			name = m.Groups[1].Value
			num = int.Parse(m.Groups[2].Value) + 1
		else:
			num = 2
		name = "${name}_${num}"
		path = Path.Combine(Path.GetDirectoryName(path), name + Path.GetExtension(path))
		path = GetUniqueName(path)
	return path

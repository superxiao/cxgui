namespace Clinky.IO

import System
import System.IO
import System.Runtime.Serialization.Formatters.Binary
import System.Windows.Forms

def IsSameFile(path1 as string, path2 as string) as bool:
	path1 = path1.Replace("/", "\\")
	path2 = path2.Replace("/", "\\")
	return path1.ToLower() == path2.ToLower()
	
def Exists(path as string) as bool:
	if File.Exists(path) or Directory.Exists(path):
		return true
	else:
		return false
	
def GetUniqueName(path as string) as string:
"""如path已存在，返回一个“文件名_序号.扩展”形式的不存在的路径。path已经是这种形式时，“序号”递增。"""
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
		if Exists(path):
			path = GetUniqueName(path)
	return path
	
def Clone[of T](obj as T) as T:
	formatter = BinaryFormatter()
	stream = MemoryStream()
	formatter.Serialize(stream, obj)
	stream.Position = 0
	obj = formatter.Deserialize(stream)
	stream.Close()
	return obj
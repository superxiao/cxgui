namespace My

import System

def IsSameFile(path1 as string, path2 as string) as bool:
	path1.Replace("/", "\\")
	path2.Replace("/", "\\")
	return path1.ToLower() == path2.ToLower()
	

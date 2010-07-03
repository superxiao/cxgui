namespace My

import System

def Middle(val1 as duck, val2 as duck, val3 as duck):
	if val1 >= val2:
		return val1 if val1 <= val3
		return val3
	return val2 if val2 <= val3
	return val3
	
def Chop(val as duck, min as duck, max as duck):
	val = min if val < min
	val = max if val > max
	return val
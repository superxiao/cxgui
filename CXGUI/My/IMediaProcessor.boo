namespace My

import System

interface IMediaProcessor:
"""Description of IMediaProcessor"""
	
	Progress as int:
		get

	def Start()
	def Stop()

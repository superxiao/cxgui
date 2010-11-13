namespace CXGUI

import System

interface IMediaProcessor:
"""Description of IMediaProcessor"""
	
	Progress as int:
		get
		
	ProcessingDone as bool:
		get

	def Start()
	def Stop()

namespace CXGUI

import System
import DirectShowLib.DES
import System.Runtime.InteropServices
import System.Windows.Forms

class DirectShowInfo:
"""Description of DirectShowInfo"""
	public def constructor(path as string):
		mediaDet as IMediaDet = MediaDet()
		hr = mediaDet.put_Filename(path)
		try:
			Marshal.ThrowExceptionForHR(hr)
		except:
			MessageBox.Show("")


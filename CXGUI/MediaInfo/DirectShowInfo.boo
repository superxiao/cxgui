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

def drtest():
	t = DirectShowInfo("""C:\Windows\winsxs\x86_microsoft-windows-wmvdspa_31bf3856ad364e35_6.1.7600.16385_none_e6522b866608b0b7\wmvdspa.dll""")
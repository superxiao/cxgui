namespace CXGUI.Avisynth

import System
import CXGUI.External

class FrameReader:
"""Description of FrameReader"""
	
	_avsFile as string
	public def constructor(avsFile as string):
		self._avsFile = avsFile

#	public static def ReadFrameBitmap(clip as AviSynthClip, position as int) as Bitmap:
#		using clip = AviSynthScriptEnvironment().OpenScriptFile(_avsFile)
#			bmp = Bitmap(500, 300, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
#			try:
#				// Lock the bitmap's bits.  
#				rect = Rectangle(0, 0, bmp.Width, bmp.Height)
#				bmpData as System.Drawing.Imaging.BitmapData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat)
#				try:
#					// Get the address of the first line.
#					ptr as IntPtr = bmpData.Scan0
#					// Read data
#					clip.ReadFrame(ptr, bmpData.Stride, position)
#				ensure:
#					// Unlock the bits.
#					bmp.UnlockBits(bmpData)
#				bmp.RotateFlip(RotateFlipType.Rotate180FlipX)
#				return bmp
#			except converterGeneratedName2 as Exception:
#				bmp.Dispose()
#				raise 
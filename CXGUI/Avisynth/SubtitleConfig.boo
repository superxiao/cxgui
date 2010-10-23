namespace CXGUI.Avisynth

import System
import System.Windows.Forms

class SubtitleConfig:
"""Description of SubtitleConfig"""
	public def constructor():
		Fontname = "宋体"
		Fontsize = 32
		MarginV = 20
	public def GetStyles() as string:
		return\
"""
[V4 Styles]
		Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, 
		 \  Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding
""" +\
"		Style: Default,${Fontname},${Fontsize},&Hffffff,&H00ffff,&H000000,&H000000,-1,0,1,2,3,2,20,20,${MarginV},0,1"
		
	public Fontname as string
	public Fontsize as int
	public MarginV as int
	public UsingStyle as bool
	

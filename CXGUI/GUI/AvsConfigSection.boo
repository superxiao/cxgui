namespace CXGUI.GUI

import System
import System.Windows.Forms//test
import System.Configuration
import CXGUI.Avisynth
import CXGUI.StreamMuxer

#	[StringValidator(InvalidCharacters: ' ~!@#$%^&*()[]{}/;\'"|\\', MinLength: 1, MaxLength: 60)]

public class AvsConfigSection():

	public def constructor():
		_fixAspectRatio = true
		_mod = 2
		_downMix = true
		
	[Property(Width)]
	_width as int
	[Property(Height)]
	_height as int
	[Property(AspectRatio)]
	_aspectRatio as double
	[Property(FixAspectRatio)]
	_fixAspectRatio as bool
	[Property(Mod)]
	_mod as int
	[Property(Resizer)]
	_resizer as VideoScriptConfig.ResizeFilter
	[Property(VideoSource)]
	_videoSourceFilter as VideoScriptConfig.VideoSourceFilter
	[Property(FrameRate)]
	_frameRate as double
	[Property(ConvertFPS)]
	_convertFPS as bool
	[Property(AudioSource)]
	_audioSourceFilter as AudioScriptConfig.AudioSourceFilter
	[Property(DownMix)]
	_downMix as bool
	[Property(Normalize)]
	_normalize as bool
	[Property(Muxer)]
	_muxer as Muxer
	
def cfgtest():
	pass
#	avsConfigSection = AvsConfigSection()
#	config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
#	n as AvsConfigSection = config.GetSection("avsConfigSection")
#	MessageBox.Show(n.ToString())
##	config.Sections.Add("avsConfigSection", avsConfigSection)
#
#	MessageBox.Show((config.Sections["avsConfigSection"] as AvsConfigSection).FixAspectRatio.ToString())
#	config.Save(ConfigurationSaveMode.Full)
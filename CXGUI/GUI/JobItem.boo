namespace CXGUI.GUI

import System
import System.Windows.Forms
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer

enum JobState:
	Waiting
	Working
	Done
	Error

enum JobEvent:
	None
	VideoEncoding
	AudioEncoding
	Muxing
	OneStart
	OneDone
	AllDone
	

class JobItem:
"""Description of JobItem"""
	public def constructor():
		pass
	[Property(SourceFile)]
	_sourceFile as string
	[Property(DestinationFile)]
	_destinationFile as string
	[Property(AvsConfig)]
	_privateAvsConfigSection as AvsConfigSection
	[Property(WriteVideoScript)]
	_writeVideoScript as bool
	[Property(WriteAudioScript)]
	_writeAudioScript as bool
	[Property(VideoEncConfig)]
	_videoEncConfig as VideoEncConfigBase
	[Property(AudioEncConfig)]
	_audioEncConfig as AudioEncConfigBase
	[Property(VideoEncoder)]
	_videoEncoder as VideoEncoderBase
	[Property(AudioEncoder)]
	_audioEncoder as AudioEncoderBase
	[Property(Muxer)]
	_muxer as MuxerBase
	[Property(State)]
	_state as JobState
	[Property(Event)]
	_event as JobEvent
	[Property(UIItem)]
	_listItem as ListViewItem


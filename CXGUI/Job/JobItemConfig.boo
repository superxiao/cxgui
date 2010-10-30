namespace CXGUI.Job

import System
import System.IO
import System.Windows.Forms//test
import CXGUI.StreamMuxer
	
enum StreamProcessMode:
	Encode
	Copy
	None

enum OutputContainer:
	MP4
	MKV

class JobItemConfig:
"""Description of JobItemConfig"""

	//_Fileds
	_container as OutputContainer
	
	
	
	//Methods
	public def constructor():
		pass
		
	public def SetContainer(container as OutputContainer, destFile as string):
		if container == OutputContainer.MKV:
			self._muxer = Muxer.MKVMerge
		elif container == OutputContainer.MP4:
			if self._videoMode == StreamProcessMode.Copy or self._audioMode == StreamProcessMode.Copy:
				self._muxer = Muxer.FFMP4
			elif Path.GetExtension(destFile).ToLower() not in ('.mp4', '.m4v', '.m4a'):
				self._muxer = Muxer.FFMP4
			elif self._videoMode == StreamProcessMode.Encode and self._audioMode == StreamProcessMode.Encode:
				self._muxer = Muxer.MP4Box
			//音频、视频处理模式都是‘None’，即源文件无媒体流；一为编码，一为None，且输出MP4，则出品已是MP4，无需再混
			else:
				self._muxer = Muxer.None
		
		
		
	//Properties
	[Getter(Muxer)]
	_muxer as Muxer
	[Property(VideoMode)]
	_videoMode as StreamProcessMode
	[Property(AudioMode)]
	_audioMode as StreamProcessMode
	[Property(UseSeparateAudio)]
	_useSepAudio as bool
	
	Container as OutputContainer:
		get:
			return self._container
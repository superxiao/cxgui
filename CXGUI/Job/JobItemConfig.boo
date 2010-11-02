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
"""与一个工作条目本身相关的公共设置。"""

	//_Fileds
	_container as OutputContainer
	
	_videoMode as StreamProcessMode
	
	_audioMode as StreamProcessMode
	
	_useSepAudio as bool
	
	
	//Methods
	public def constructor():
		pass
		

	//Properties
	Container as OutputContainer:
	"""
	改变此属性，将影响JobItem.SetUp()以后或JobItem.CreateNewMuxer()后
	JobItem.Muxer的类型。
	"""
		get:
			return self._container
		set:
			self._container = value

	VideoMode as StreamProcessMode:
		get:
			return self._videoMode
		set:
			self._videoMode = value
			
	AudioMode as StreamProcessMode:
		get:
			return self._audioMode
		set:
			self._audioMode = value
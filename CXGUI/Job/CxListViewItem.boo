namespace CXGUI.Job

import System
import System.Windows.Forms
import System.Runtime.Serialization

class CxListViewItem(ListViewItem, ISerializable):
"""Description of CxListViewItem"""

	//Methods
	public def constructor(items as (string)):
		super(items)
	
	//_jobItem字段循环引用不知如何序列化，在JobItem类中使用OnDeserialized标签解决
	public def constructor(info as SerializationInfo, context as StreamingContext):
		Deserialize(info, context)
		
	public def GetObjectData(info as SerializationInfo, context as StreamingContext):
		Serialize(info, context)
		

	//Properties
	_jobItem as JobItem
	JobItem as JobItem:
		get:
			return _jobItem
		set:
			_jobItem = value
		


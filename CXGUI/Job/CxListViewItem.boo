namespace CXGUI.Job

import System
import System.Windows.Forms

class CxListViewItem(ListViewItem):
"""Description of CxListViewItem"""
	public def constructor(items as (string)):
		super(items)
		
	//Properties
	
	_jobItem as JobItem
	JobItem as JobItem:
		get:
			return _jobItem
		set:
			_jobItem = value
		


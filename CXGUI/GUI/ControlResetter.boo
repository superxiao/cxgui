namespace CXGUI.GUI

import System
import System.Reflection
import System.Collections
import System.Collections.Generic
import System.Windows.Forms

class ControlResetter:
"""提供控件属性的保存和重设功能。"""

	_savedCheckBox = Dictionary[of CheckBox, bool]()
	_savedTextControl = Dictionary[of Control, string]()

	public def constructor():
		pass

	def SaveControls(controls as IEnumerable):
	"""
	保存控件的属性。
	Param controls: 包含将被保存的控件。
	Remarks: 同一控件再次保存将会覆盖之前保存的属性。
	"""
		for control as Control in controls:
			type = control.GetType()
			if type == TextBox or type == ComboBox or type == DomainUpDown:
				_savedTextControl[control] = control.Text
			elif type == CheckBox:
				_savedCheckBox[control] = (control as CheckBox).Checked
	
	def Changed() as bool:
		for checkBox as CheckBox in _savedCheckBox.Keys:
			if checkBox.Checked != _savedCheckBox[checkBox]:
				return true
		for control as Control in _savedTextControl.Keys:
			if control.Text != _savedTextControl[control]:
				return true
		return false
		

	def ResetControls(controls as IEnumerable) as int:
	"""
	重设控件的属性。
	Param controls: 包含将被重设的控件。
	Remarks: 控件必须先使用SaveControls方法保存，然后重设才会生效。
	"""
		return ResetAndCount(true, controls)
		
	def ResetControls() as int:
		return ResetAndCount(true, _savedCheckBox.Keys) + ResetAndCount(true, _savedTextControl.Keys)

	def ChangedCount(controls as IEnumerable) as int:
		return ResetAndCount(false, controls)

	def ChangedCount() as int:
		return ResetAndCount(false, _savedCheckBox.Keys) + ResetAndCount(false, _savedTextControl.Keys)

	private def ResetAndCount(reset as bool, controls as IEnumerable) as int:
		changedCount = 0
		for control as Control in controls:
			if _savedCheckBox.ContainsKey(control):
				if (control as CheckBox).Checked != _savedCheckBox[control]:
					changedCount++
					if reset == true:
						(control as CheckBox).Checked = _savedCheckBox[control]
			elif _savedTextControl.ContainsKey(control):
				if control.Text != _savedTextControl[control]:
					changedCount++
					if reset == true:
						control.Text = _savedTextControl[control]
		return changedCount

	def Clear():
	"""
	清空被保存的控件属性信息。
	"""
		_savedCheckBox.Clear()
		_savedTextControl.Clear()
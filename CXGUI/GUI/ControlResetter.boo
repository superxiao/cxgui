namespace CXGUI.GUI

import System
import System.Reflection
import System.Collections
import System.Windows.Forms

class ControlResetter:
"""提供控件属性的保存和重设功能。"""
	_savedControls = {}
	public def constructor():
		pass

	def SaveControls(controls as Array):
	"""
	保存控件的属性。
	Param controls: 包含将被保存的控件。
	Remarks: 同一控件再次保存将会覆盖之前保存的属性。
	"""
		for control in controls:
			controlState = {}
			propertiesInfo = control.GetType().GetProperties()
			for propertyInfo in propertiesInfo:
				if propertyInfo.Name == "Checked":
					controlState["Checked"] = propertyInfo.GetValue(control, null)
				if propertyInfo.Name == "Text":
					controlState["Text"] = propertyInfo.GetValue(control, null)
				if propertyInfo.Name == "Enabled":
					controlState["Enabled"] = propertyInfo.GetValue(control, null)
			_savedControls[control] = controlState

	def ResetControls(controls as Array) as int:
	"""
	重设控件的属性。
	Param controls: 包含将被重设的控件。
	Remarks: 控件必须先使用SaveControls方法保存，然后重设才会生效。
	"""
		return ChangedCount(true, controls)
		
	def ResetControls() as int:
		return ChangedCount(true, array(_savedControls.Keys))

	def ChangedCount(controls as Array) as int:
		return ChangedCount(false, controls)

	def ChangedCount() as int:
		return ChangedCount(false, array(_savedControls.Keys))

	private def ChangedCount(reset as bool, controls as Array) as int:
		changedCount = 0
		for control in controls:
			if _savedControls.Contains(control):
				for state as DictionaryEntry in _savedControls[control]:
					propertyInfo = control.GetType().GetProperty(state.Key as string)
					if propertyInfo.GetValue(control, null) != state.Value:
						changedCount++
						if reset:
							propertyInfo.SetValue(control, state.Value, null)
		return changedCount

	def Clear():
	"""
	清空被保存的控件属性信息。
	"""
		_savedControls.Clear()

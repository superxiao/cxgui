namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms

partial final class X264ConfigForm:
"""Description of X264ConfigForm."""

	static Instance as X264ConfigForm:
		get:
			return _instance
	static final _instance = X264ConfigForm()
	static def constructor():
 		pass

	private def constructor():
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		// TODO: Add constructor code after the InitializeComponent() call.


namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms

partial final class x264ConfigForm:
"""Description of x264ConfigForm."""

	static Instance as x264ConfigForm:
		get:
			return _instance
	static final _instance = x264ConfigForm()
	static def constructor():
 		pass

	private def constructor():
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		// TODO: Add constructor code after the InitializeComponent() call.


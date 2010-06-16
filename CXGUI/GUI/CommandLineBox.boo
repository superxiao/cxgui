namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms

partial class CommandLineBox:
"""Description of CommandLineBox."""
	public def constructor():
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		// TODO: Add constructor code after the InitializeComponent() call.
	
	[Property(CmdLine)]
	_cmdLine as string
	
	private def CommandLineBoxLoad(sender as object, e as System.EventArgs):
		self.commandBox.Text = self._cmdLine
	private def OKButtonClick(sender as object, e as System.EventArgs):
		self._cmdLine = self.commandBox.Text
		self.Close()
	
	private def CacelButtonClick(sender as object, e as System.EventArgs):
		self.Close()
		
	



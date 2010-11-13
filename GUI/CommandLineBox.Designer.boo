namespace CXGUI.GUI

partial class CommandLineBox(System.Windows.Forms.Form):
	private components as System.ComponentModel.IContainer = null
	
	protected override def Dispose(disposing as bool) as void:
		if disposing:
			if components is not null:
				components.Dispose()
		super(disposing)
	
	// This method is required for Windows Forms designer support.
	// Do not change the method contents inside the source code editor. The Forms designer might
	// not be able to load this method if it was changed manually.
	private def InitializeComponent():
		self.commandBox = System.Windows.Forms.TextBox()
		self.OKButton = System.Windows.Forms.Button()
		self.CacelButton = System.Windows.Forms.Button()
		self.SuspendLayout()
		# 
		# commandBox
		# 
		self.commandBox.Anchor = cast(System.Windows.Forms.AnchorStyles,(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.commandBox.Location = System.Drawing.Point(12, 12)
		self.commandBox.Multiline = true
		self.commandBox.Name = "commandBox"
		self.commandBox.Size = System.Drawing.Size(260, 172)
		self.commandBox.TabIndex = 0
		# 
		# OKButton
		# 
		self.OKButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.OKButton.Location = System.Drawing.Point(116, 190)
		self.OKButton.Name = "OKButton"
		self.OKButton.Size = System.Drawing.Size(75, 23)
		self.OKButton.TabIndex = 1
		self.OKButton.Text = "确定"
		self.OKButton.UseVisualStyleBackColor = true
		self.OKButton.Click += self.OKButtonClick as System.EventHandler
		# 
		# CacelButton
		# 
		self.CacelButton.Anchor = cast(System.Windows.Forms.AnchorStyles,(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right))
		self.CacelButton.Location = System.Drawing.Point(197, 190)
		self.CacelButton.Name = "CacelButton"
		self.CacelButton.Size = System.Drawing.Size(75, 23)
		self.CacelButton.TabIndex = 2
		self.CacelButton.Text = "取消"
		self.CacelButton.UseVisualStyleBackColor = true
		self.CacelButton.Click += self.CacelButtonClick as System.EventHandler
		# 
		# CommandLineBox
		# 
		self.AcceptButton = self.CacelButton
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.ClientSize = System.Drawing.Size(284, 219)
		self.Controls.Add(self.CacelButton)
		self.Controls.Add(self.OKButton)
		self.Controls.Add(self.commandBox)
		self.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
		self.Name = "CommandLineBox"
		self.Text = "命令行"
		self.Load += self.CommandLineBoxLoad as System.EventHandler
		self.ResumeLayout(false)
		self.PerformLayout()
	private CacelButton as System.Windows.Forms.Button
	private OKButton as System.Windows.Forms.Button
	private commandBox as System.Windows.Forms.TextBox


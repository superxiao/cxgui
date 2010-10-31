namespace CXGUI.GUI

partial class x264ConfigForm(System.Windows.Forms.Form):
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
		self.tabControl1 = System.Windows.Forms.TabControl()
		self.tabPage1 = System.Windows.Forms.TabPage()
		self.groupBox1 = System.Windows.Forms.GroupBox()
		self.deadzone_intra = System.Windows.Forms.DomainUpDown()
		self.deadzone_inter = System.Windows.Forms.DomainUpDown()
		self.label3 = System.Windows.Forms.Label()
		self.pbratio = System.Windows.Forms.DomainUpDown()
		self.ipratio = System.Windows.Forms.DomainUpDown()
		self.label2 = System.Windows.Forms.Label()
		self.qpstep = System.Windows.Forms.DomainUpDown()
		self.qpmax = System.Windows.Forms.DomainUpDown()
		self.qpmin = System.Windows.Forms.DomainUpDown()
		self.label1 = System.Windows.Forms.Label()
		self.EncTabPage = System.Windows.Forms.TabPage()
		self.tabPage3 = System.Windows.Forms.TabPage()
		self.tabPage4 = System.Windows.Forms.TabPage()
		self.label4 = System.Windows.Forms.Label()
		self.chroma_qp_offset = System.Windows.Forms.DomainUpDown()
		self.tabControl1.SuspendLayout()
		self.tabPage1.SuspendLayout()
		self.groupBox1.SuspendLayout()
		self.SuspendLayout()
		# 
		# tabControl1
		# 
		self.tabControl1.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.tabControl1.Controls.Add(self.tabPage1)
		self.tabControl1.Controls.Add(self.EncTabPage)
		self.tabControl1.Controls.Add(self.tabPage3)
		self.tabControl1.Controls.Add(self.tabPage4)
		self.tabControl1.Location = System.Drawing.Point(12, 12)
		self.tabControl1.Name = "tabControl1"
		self.tabControl1.SelectedIndex = 0
		self.tabControl1.Size = System.Drawing.Size(557, 332)
		self.tabControl1.TabIndex = 0
		# 
		# tabPage1
		# 
		self.tabPage1.Controls.Add(self.groupBox1)
		self.tabPage1.Location = System.Drawing.Point(4, 22)
		self.tabPage1.Name = "tabPage1"
		self.tabPage1.Padding = System.Windows.Forms.Padding(3)
		self.tabPage1.Size = System.Drawing.Size(549, 306)
		self.tabPage1.TabIndex = 0
		self.tabPage1.Text = "码率控制"
		self.tabPage1.UseVisualStyleBackColor = true
		# 
		# groupBox1
		# 
		self.groupBox1.Anchor = cast(System.Windows.Forms.AnchorStyles,((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right))
		self.groupBox1.Controls.Add(self.chroma_qp_offset)
		self.groupBox1.Controls.Add(self.label4)
		self.groupBox1.Controls.Add(self.deadzone_intra)
		self.groupBox1.Controls.Add(self.deadzone_inter)
		self.groupBox1.Controls.Add(self.label3)
		self.groupBox1.Controls.Add(self.pbratio)
		self.groupBox1.Controls.Add(self.ipratio)
		self.groupBox1.Controls.Add(self.label2)
		self.groupBox1.Controls.Add(self.qpstep)
		self.groupBox1.Controls.Add(self.qpmax)
		self.groupBox1.Controls.Add(self.qpmin)
		self.groupBox1.Controls.Add(self.label1)
		self.groupBox1.Location = System.Drawing.Point(6, 6)
		self.groupBox1.Name = "groupBox1"
		self.groupBox1.Size = System.Drawing.Size(537, 156)
		self.groupBox1.TabIndex = 0
		self.groupBox1.TabStop = false
		self.groupBox1.Text = "量化器"
		# 
		# deadzone_intra
		# 
		self.deadzone_intra.Location = System.Drawing.Point(212, 70)
		self.deadzone_intra.Name = "deadzone_intra"
		self.deadzone_intra.Size = System.Drawing.Size(50, 21)
		self.deadzone_intra.TabIndex = 9
		# 
		# deadzone_inter
		# 
		self.deadzone_inter.Location = System.Drawing.Point(156, 70)
		self.deadzone_inter.Name = "deadzone_inter"
		self.deadzone_inter.Size = System.Drawing.Size(50, 21)
		self.deadzone_inter.TabIndex = 8
		# 
		# label3
		# 
		self.label3.Location = System.Drawing.Point(6, 68)
		self.label3.Name = "label3"
		self.label3.Size = System.Drawing.Size(133, 23)
		self.label3.TabIndex = 7
		self.label3.Text = "deadzone-inter/intra"
		self.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# pbratio
		# 
		self.pbratio.Location = System.Drawing.Point(212, 43)
		self.pbratio.Name = "pbratio"
		self.pbratio.Size = System.Drawing.Size(50, 21)
		self.pbratio.TabIndex = 6
		# 
		# ipratio
		# 
		self.ipratio.Location = System.Drawing.Point(156, 43)
		self.ipratio.Name = "ipratio"
		self.ipratio.Size = System.Drawing.Size(50, 21)
		self.ipratio.TabIndex = 5
		# 
		# label2
		# 
		self.label2.Location = System.Drawing.Point(6, 38)
		self.label2.Name = "label2"
		self.label2.Size = System.Drawing.Size(117, 26)
		self.label2.TabIndex = 4
		self.label2.Text = "ipratio/pbratio"
		self.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# qpstep
		# 
		self.qpstep.Location = System.Drawing.Point(268, 15)
		self.qpstep.Name = "qpstep"
		self.qpstep.Size = System.Drawing.Size(50, 21)
		self.qpstep.TabIndex = 3
		# 
		# qpmax
		# 
		self.qpmax.Location = System.Drawing.Point(212, 15)
		self.qpmax.Name = "qpmax"
		self.qpmax.Size = System.Drawing.Size(50, 21)
		self.qpmax.TabIndex = 2
		# 
		# qpmin
		# 
		self.qpmin.Location = System.Drawing.Point(156, 15)
		self.qpmin.Name = "qpmin"
		self.qpmin.Size = System.Drawing.Size(50, 21)
		self.qpmin.TabIndex = 1
		# 
		# label1
		# 
		self.label1.Location = System.Drawing.Point(6, 17)
		self.label1.Name = "label1"
		self.label1.Size = System.Drawing.Size(117, 21)
		self.label1.TabIndex = 0
		self.label1.Text = "qpmin/qpmax/qmstep"
		self.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# EncTabPage
		# 
		self.EncTabPage.Location = System.Drawing.Point(4, 22)
		self.EncTabPage.Name = "EncTabPage"
		self.EncTabPage.Padding = System.Windows.Forms.Padding(3)
		self.EncTabPage.Size = System.Drawing.Size(549, 306)
		self.EncTabPage.TabIndex = 1
		self.EncTabPage.Text = "帧"
		self.EncTabPage.UseVisualStyleBackColor = true
		# 
		# tabPage3
		# 
		self.tabPage3.Location = System.Drawing.Point(4, 22)
		self.tabPage3.Name = "tabPage3"
		self.tabPage3.Padding = System.Windows.Forms.Padding(3)
		self.tabPage3.Size = System.Drawing.Size(549, 306)
		self.tabPage3.TabIndex = 2
		self.tabPage3.Text = "分析"
		self.tabPage3.UseVisualStyleBackColor = true
		# 
		# tabPage4
		# 
		self.tabPage4.Location = System.Drawing.Point(4, 22)
		self.tabPage4.Name = "tabPage4"
		self.tabPage4.Padding = System.Windows.Forms.Padding(3)
		self.tabPage4.Size = System.Drawing.Size(549, 306)
		self.tabPage4.TabIndex = 3
		self.tabPage4.Text = "其他"
		self.tabPage4.UseVisualStyleBackColor = true
		# 
		# label4
		# 
		self.label4.Location = System.Drawing.Point(6, 91)
		self.label4.Name = "label4"
		self.label4.Size = System.Drawing.Size(117, 23)
		self.label4.TabIndex = 10
		self.label4.Text = "chroma-qp-offset"
		self.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		# 
		# chroma_qp_offset
		# 
		self.chroma_qp_offset.Location = System.Drawing.Point(156, 97)
		self.chroma_qp_offset.Name = "chroma_qp_offset"
		self.chroma_qp_offset.Size = System.Drawing.Size(50, 21)
		self.chroma_qp_offset.TabIndex = 11
		# 
		# x264ConfigForm
		# 
		self.AutoScaleDimensions = System.Drawing.SizeF(6, 12)
		self.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		self.ClientSize = System.Drawing.Size(581, 531)
		self.Controls.Add(self.tabControl1)
		self.Name = "x264ConfigForm"
		self.Text = "x264ConfigForm"
		self.tabControl1.ResumeLayout(false)
		self.tabPage1.ResumeLayout(false)
		self.groupBox1.ResumeLayout(false)
		self.ResumeLayout(false)
	private chroma_qp_offset as System.Windows.Forms.DomainUpDown
	private label4 as System.Windows.Forms.Label
	private label1 as System.Windows.Forms.Label
	private qpmin as System.Windows.Forms.DomainUpDown
	private qpmax as System.Windows.Forms.DomainUpDown
	private qpstep as System.Windows.Forms.DomainUpDown
	private label2 as System.Windows.Forms.Label
	private ipratio as System.Windows.Forms.DomainUpDown
	private pbratio as System.Windows.Forms.DomainUpDown
	private label3 as System.Windows.Forms.Label
	private deadzone_inter as System.Windows.Forms.DomainUpDown
	private deadzone_intra as System.Windows.Forms.DomainUpDown
	private groupBox1 as System.Windows.Forms.GroupBox
	private tabPage4 as System.Windows.Forms.TabPage
	private tabPage3 as System.Windows.Forms.TabPage
	private EncTabPage as System.Windows.Forms.TabPage
	private tabPage1 as System.Windows.Forms.TabPage
	private tabControl1 as System.Windows.Forms.TabControl


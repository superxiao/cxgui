namespace CXGUI.GUI
{
	partial class CommandLineBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.CacelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.commandBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CacelButton
            // 
            this.CacelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CacelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CacelButton.Location = new System.Drawing.Point(197, 164);
            this.CacelButton.Name = "CacelButton";
            this.CacelButton.Size = new System.Drawing.Size(75, 23);
            this.CacelButton.TabIndex = 5;
            this.CacelButton.Text = "取消";
            this.CacelButton.UseVisualStyleBackColor = true;
            this.CacelButton.Click += new System.EventHandler(this.CacelButtonClick);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(116, 164);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // commandBox
            // 
            this.commandBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commandBox.Location = new System.Drawing.Point(12, 31);
            this.commandBox.Multiline = true;
            this.commandBox.Name = "commandBox";
            this.commandBox.Size = new System.Drawing.Size(260, 127);
            this.commandBox.TabIndex = 3;
            // 
            // CommandLineBox
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CacelButton;
            this.ClientSize = new System.Drawing.Size(284, 217);
            this.Controls.Add(this.CacelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.commandBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CommandLineBox";
            this.Text = "x264 命令行";
            this.Load += new System.EventHandler(this.CommandLineBoxLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button CacelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox commandBox;
	}
}
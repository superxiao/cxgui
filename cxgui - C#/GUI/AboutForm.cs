using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CXGUI.GUI
{
	public partial class AboutForm: Form
	{
		public AboutForm()
		{
			InitializeComponent();
		}

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/cxgui/");
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            this.Hide();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}

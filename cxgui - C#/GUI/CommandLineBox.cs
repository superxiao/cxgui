using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cxgui.Gui
{
	public partial class CommandLineBox: Form
    {
        protected string _cmdLine;

		public CommandLineBox()
		{
			InitializeComponent();
		}

        private void CacelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CommandLineBoxLoad(object sender, EventArgs e)
        {
            this.commandBox.Text = this._cmdLine;
        }
        private void OKButtonClick(object sender, EventArgs e)
        {
            this._cmdLine = this.commandBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string CmdLine
        {
            get
            {
                return this._cmdLine;
            }
            set
            {
                this._cmdLine = value;
            }
        }
	}
}

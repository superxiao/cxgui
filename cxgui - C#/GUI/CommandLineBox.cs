using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CXGUI.GUI
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
            this.Close();
        }

        private void CommandLineBoxLoad(object sender, EventArgs e)
        {
            this.commandBox.Text = this._cmdLine;
        }
        private void OKButtonClick(object sender, EventArgs e)
        {
            this._cmdLine = this.commandBox.Text;
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

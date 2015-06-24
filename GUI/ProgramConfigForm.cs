namespace Cxgui.Gui
{
    using Cxgui.Config;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    [Serializable]
    public partial class ProgramConfigForm : Form
    {
        protected ControlResetter _resetter;

        public ProgramConfigForm(ProgramConfig configSection)
        {
            this.InitializeComponent();
            this.destDirComboBox.Text = configSection.DestDir;
            this.showInputDirCheckBox.Checked = configSection.OmitInputDir;
            this.chbSilentRestart.Checked = configSection.SilentRestart;
            this.previewPlayerComboBox.Text = configSection.PlayerPath;
            this.autoChangeAudioSourceFilterCheckBox.Checked = configSection.AutoChangeAudioSourceFilter;
        }

        private void BrowseButtonClick(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.destDirComboBox.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void CacelButtonClick(object sender, EventArgs e)
        {
            this._resetter.ResetControls();
            this._resetter.Clear();
            this.Close();
        }

        private void OKButtonClick(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.destDirComboBox.Text))
            {
                ComboBox[] controls = new ComboBox[] { this.destDirComboBox };
                this._resetter.ResetControls(controls);
            }
            if (!File.Exists(this.previewPlayerComboBox.Text))
            {
                ComboBox[] boxArray2 = new ComboBox[] { this.previewPlayerComboBox };
                this._resetter.ResetControls(boxArray2);
            }
            this.SaveConfig();
            this._resetter.Clear();
            this.Close();
        }

        private void PreviewPlayerButtonClick(object sender, EventArgs e)
        {
            if (File.Exists(this.previewPlayerComboBox.Text))
            {
                this.openFileDialog1.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(this.previewPlayerComboBox.Text));
                this.openFileDialog1.FileName = Path.GetFileName(this.previewPlayerComboBox.Text);
            }
            this.openFileDialog1.ShowDialog();
            this.previewPlayerComboBox.Text = this.openFileDialog1.FileName;
        }

        private void ProgramConfigFormLoad(object sender, EventArgs e)
        {
            this._resetter = new ControlResetter();
            this._resetter.SaveControls(this.groupBox1.Controls as IEnumerable);
            this._resetter.SaveControls(this.groupBox2.Controls as IEnumerable);
            this._resetter.SaveControls(this.editGroupBox.Controls as IEnumerable);
        }

        private void SaveConfig()
        {
            ProgramConfig config = ProgramConfig.Get();
            config.DestDir = this.destDirComboBox.Text;
            config.OmitInputDir = this.showInputDirCheckBox.Checked;
            config.SilentRestart = this.chbSilentRestart.Checked;
            config.PlayerPath = this.previewPlayerComboBox.Text;
            config.AutoChangeAudioSourceFilter = this.autoChangeAudioSourceFilterCheckBox.Checked;
            ProgramConfig.Save();
        }
    }
}


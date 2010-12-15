namespace Cxgui.Gui
{
    using Clinky;
    using Cxgui;
    using Cxgui.AudioEncoding;
    using Cxgui.Avisynth;
    using Cxgui.Config;
    using Cxgui.Job;
    using Cxgui.VideoEncoding;
    using External;
    using DirectShowLib.DES;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public partial class MainForm : Form
    {
        protected ProgramConfigForm configForm;
        protected JobSettingForm jobSettingForm;
        protected AboutForm aboutForm;
        protected bool workerReporting;
        protected JobItem workingJobItem;
        protected List<JobItem> workingJobItems;
        private delegate void ReportInvoke(JobItem jobItem, IMediaProcessor encoder, DoWorkEventArgs e);
        private bool formClosing;
        
        [STAThread]
        public static void Main(string[] argv)
        {
            if (!Directory.Exists("tools"))
            {
                Directory.CreateDirectory("tools");
            }
            Directory.SetCurrentDirectory("tools");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public MainForm()
        {
            this.InitializeComponent();
            ProgramConfig configSection = ProgramConfig.Get();
            this.configForm = new ProgramConfigForm(configSection);
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ListViewItem current = (ListViewItem) enumerator.Current;
                current.Selected = true;
            }
            this.delButton.PerformClick();
            this.settingButton.Enabled = false;
        }

        private void ContextMenuStrip1Opening(object sender, CancelEventArgs e)
        {
            if (this.jobItemListView.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
            else if (this.jobItemListView.SelectedItems.Count == 1)
            {
                this.listViewMenu.Items["设置ToolStripMenuItem"].Enabled = true;
                this.listViewMenu.Items["打开目录ToolStripMenuItem"].Enabled = true;
            }
            else
            {
                this.listViewMenu.Items["设置ToolStripMenuItem"].Enabled = false;
                this.listViewMenu.Items["打开目录ToolStripMenuItem"].Enabled = false;
            }
        }

        private void DelButtonClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                this.jobItemListView.Items.Remove(current);
                if (((this.workingJobItems != null) && this.workingJobItems.Contains(current.JobItem)) && (this.workingJobItems.IndexOf(current.JobItem) > this.workingJobItems.IndexOf(this.workingJobItem)))
                {
                    this.workingJobItems.Remove(current.JobItem);
                }
            }
        }

        private bool DidUserPressStopButton(JobItem jobItem, DoWorkEventArgs e)
        {
            if (this.backgroundWorker.CancellationPending)
            {
                jobItem.Event = JobEvent.QuitAllProcessing;
                this.JobEventReport(jobItem);
                return true;
            }
            return false;
        }

        private void JobItemListViewItemDrag(object sender, ItemDragEventArgs e)
        {
            this.jobItemListView.DoDragDrop(this.jobItemListView.SelectedItems, DragDropEffects.Move);
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this.workingJobItem != null) && (this.workingJobItem.State == JobState.Working))
            {
                if (MessageBox.Show("正在工作中，是否中止并退出？", "工作中", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    this.StopButtonClick(null, null);
                    this.formClosing = true;
                }
                    e.Cancel = true;
                    return;
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            this.UpdateProfileBox(Profile.GetExistingProfilesNamesOnHardDisk(), ProgramConfig.Get().ProfileName);
            System.Collections.Generic.List<JobItem> list = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (MyIO.Exists("JobItems.bin"))
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream("JobItems.bin", FileMode.Open);
                    list = (System.Collections.Generic.List<JobItem>) formatter.Deserialize(stream);
                    stream.Close();
                }
                catch (Exception)
                {
                    stream.Close();
                }
                if (list != null)
                {
                    foreach (JobItem item in list)
                    {
                        if (item.State == JobState.Waiting)
                            item.State = JobState.NotProccessed;
                        else if (item.State == JobState.Working)
                            item.State = JobState.Stop;
                        this.jobItemListView.Items.Add(item.CxListViewItem);
                    }
                }
            }
        }

        private void OpenFileDialog1FileOk(object sender, CancelEventArgs e)
        {
            ListViewItem item = null;
            int index = 0;
            string[] fileNames = this.openFileDialog1.FileNames;
            int length = fileNames.Length;
            while (index < length)
            {
                ListViewItem item2 = this.AddNewJobItem(fileNames[index], true);
                if ((item == null) && (item2 != null))
                {
                    item = item2;
                }
                index++;
            }
            if (item != null)
            {
                this.jobItemListView.SelectedItems.Clear();
                item.Selected = true;
            }
        }

        private void ProfileBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.jobItemListView.Items.Count > 0 && MessageBox.Show("是否应用更改到所有项目？", "预设更改", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Profile profile = new Profile(this.profileBox.Text);
                string extension = string.Empty;
                extension = profile.GetExtByContainer();
                foreach (CxListViewItem item in this.jobItemListView.Items)
                {
                    JobItem jobItem = item.JobItem;
                    jobItem.ProfileName = this.profileBox.Text;
                    jobItem.SetUp(true);
                    if (extension != string.Empty)
                    {
                        jobItem.DestFile = Path.ChangeExtension(jobItem.DestFile, extension);
                        jobItem.DestFile = MyIO.GetUniqueName(jobItem.DestFile);
                    }
                }
            }
        }

        private void ResetProgress()
        {
            this.videoProgressBar.Value = 0;
            this.audioProgressBar.Value = 0;
            this.muxProgressBar.Value = 0;
            this.videoTimeLeftLable.Text = string.Empty;
            this.videoTimeUsedLable.Text = string.Empty;
            this.videoAvgBitRateLable.Text = string.Empty;
            this.videoEncodingFpsLable.Text = string.Empty;
            this.videoEstimatedFileSizeLable.Text = string.Empty;
            this.audioTimeLeft.Text = string.Empty;
            this.audioTimeUsed.Text = string.Empty;
            this.muxTimeLeft.Text = string.Empty;
            this.muxTimeUsed.Text = string.Empty;
        }

        private void SaveProfileSelection()
        {
            ProgramConfig.Get().ProfileName = this.profileBox.Text;
            ProgramConfig.Save();
        }

        private void SettingButtonClick(object sender, EventArgs e)
        {
            CxListViewItem item = (CxListViewItem) this.jobItemListView.SelectedItems[0];
            JobItem jobItem = item.JobItem;
            JobItem[] items = new JobItem[] { jobItem };
            this.SetUpJobItems(items);
            if (this.jobSettingForm == null)
            {
                this.jobSettingForm = new JobSettingForm();
            }
            this.jobSettingForm.UpdateProfiles(ArrayBuilder.Build<string>(this.profileBox.Items), jobItem.ProfileName);
            this.jobSettingForm.SetUpFormForItem(jobItem);
            DialogResult result = this.jobSettingForm.ShowDialog();
            this.UpdateProfileBox(this.jobSettingForm.GetProfiles(), this.profileBox.Text);
            this.jobSettingForm.Clear();
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            this.workingJobItems = this.GetWorkingJobItems();
            string notExistingSourceFile = string.Empty;
            foreach (JobItem item in this.workingJobItems)
            {
                if (!MyIO.Exists(item.SourceFile))
                {
                    notExistingSourceFile += new StringBuilder("\n").Append(item.SourceFile).ToString();
                    item.State = JobState.Error;
                    this.workingJobItems.Remove(item);
                }
            }
            if ((notExistingSourceFile != string.Empty) && (MessageBox.Show(new StringBuilder("以下媒体文件不存在：").Append(notExistingSourceFile).Append("\n单击“确定”将处理其他文件。").ToString(), "错误", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel))
            {
                this.workingJobItems.Clear();
            }
            else if (this.workingJobItems.Count > 0)
            {
                this.SetUpJobItems(this.workingJobItems.ToArray());
                this.workingJobItem = this.workingJobItems[0];
                this.backgroundWorker.RunWorkerAsync(this.workingJobItem);
                this.mainTabControl.SelectTab(this.progressPage);
            }
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            try
            {
                this.backgroundWorker.CancelAsync();
            }
            catch (Exception)
            {
            }
        }

        private void UpdateProfileBox(string[] newProfileNames, string selectedProfile)
        {
            this.profileBox.SelectedIndexChanged -= new EventHandler(this.ProfileBoxSelectedIndexChanged);
            this.profileBox.Items.Clear();
            this.profileBox.Items.AddRange(newProfileNames);
            if (!this.profileBox.Items.Contains("Default"))
            {
                Profile.RebuildDefault("Default");
                this.profileBox.Items.Add("Default");
            }
            if (this.profileBox.Items.Contains(selectedProfile))
            {
                this.profileBox.SelectedItem = selectedProfile;
            }
            else
            {
                this.profileBox.SelectedItem = "Default";
            }
            this.profileBox.SelectedIndexChanged += new EventHandler(this.ProfileBoxSelectedIndexChanged);
        }

        private void 打开目录ToolStripMenuItemClick(object sender, EventArgs e)
        {
            JobItem jobItem = (this.jobItemListView.SelectedItems[0] as CxListViewItem).JobItem;
            Process.Start("explorer.exe", "/select, " + jobItem.SourceFile);
        }

        private void 未处理ToolStripMenuItemClick(object sender, EventArgs e)
        {
            IEnumerator enumerator = this.jobItemListView.SelectedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CxListViewItem current = (CxListViewItem) enumerator.Current;
                current.JobItem.State = JobState.NotProccessed;
            }
        }

        private void 选项ToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.configForm.ShowDialog();
        }

        private void jobItemListView_DoubleClick(object sender, EventArgs e)
        {
            this.settingButton.PerformClick();
        }

        private void jobItemListView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                this.jobItemListView.SelectedItems.Clear();
                foreach (string path in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (File.Exists(path))
                    {
                        ListViewItem newListViewItem = AddNewJobItem(path, true);
                        if (newListViewItem != null)
                            newListViewItem.Selected = true;
                    }
                    else if (Directory.Exists(path))
                    {
                        foreach (string file in Directory.GetFiles(path))
                        {
                            ListViewItem newListViewItem = AddNewJobItem(path, false);
                            if (newListViewItem != null)
                                newListViewItem.Selected = true;
                        }
                    }
                }
                return;
            }
            Point p = this.jobItemListView.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = this.jobItemListView.GetItemAt(p.X, p.Y);
            if (dragToItem == null)
                return;
            int dragToIndex = dragToItem.Index;
            foreach (ListViewItem item in this.jobItemListView.SelectedItems)
            {
                this.jobItemListView.Items.Remove(item);
                this.jobItemListView.Items.Insert(dragToIndex, item);
            }
            if (this.workingJobItems != null && this.workingJobItems.Count > 0)
            {
                List<JobItem> newWorkingItems = new List<JobItem>(this.workingJobItems.Count);
                foreach (JobItem jobItem in this.workingJobItems)
                    newWorkingItems[jobItem.CxListViewItem.Index] = jobItem;
                this.workingJobItems = newWorkingItems;
            }
        }

        private void ListView1DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)) || e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private ListViewItem AddNewJobItem(string filePath, bool validateMedia)
        {
            string fileName = filePath;
            if (!this.configForm.showInputDirCheckBox.Checked)
            {
                fileName = Path.GetFileName(fileName);
            }
            if (validateMedia&&Path.GetExtension(fileName).ToLower()!=".avs")
            {
                int errorCode = ((IMediaDet)new MediaDet()).put_Filename(filePath);
                try
                {
                    Marshal.ThrowExceptionForHR(errorCode);
                }
                catch (Exception)
                {
                    if (!(MessageBox.Show((filePath + "\n在文件中找不到视频流或音频流，可能是没有安装对应的DirectShow滤镜。\n") + "仍然要添加该文件吗？", "检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK))
                    {
                        return null;
                    }
                }
            }
            JobItem jobItem = null;
            Profile profile = new Profile(this.profileBox.Text);
            string ext = ".mp4";
            if (profile != null)
            {
                ext = profile.GetExtByContainer();
            }
            string destFile = string.Empty;
            if (this.configForm.destDirComboBox.Text == string.Empty)
                destFile = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ext);
            else
                destFile = Path.Combine(this.configForm.destDirComboBox.Text, Path.GetFileNameWithoutExtension(filePath) + ext);
            destFile = MyIO.GetUniqueName(destFile);
            jobItem = new JobItem(filePath, destFile, this.profileBox.Text);
            if (Path.GetExtension(filePath).ToLower()==".avs"&&jobItem.VideoInfo.Container!="avs")
                if (MessageBox.Show((filePath + "\n错误的avs脚本，或没有安装Avisynth。\n") + "仍然要添加该文件吗？", "检测失败", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return null;
                }
            this.jobItemListView.Items.Add(jobItem.CxListViewItem);
            if ((this.workingJobItem != null) && (this.workingJobItem.State == JobState.Working))
                if (MessageBox.Show("是否加入工作队列？", "工作中", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    this.SetUpJobItems(new JobItem[] { jobItem });
                    this.workingJobItems.Add(jobItem);
                    jobItem.State = JobState.Waiting;
                }
            return jobItem.CxListViewItem;
        }

        private void JobItemListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.jobItemListView.SelectedItems.Count == 1)
            {
                this.settingButton.Enabled = true;
            }
            else
            {
                this.settingButton.Enabled = false;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.aboutForm == null)
                this.aboutForm = new AboutForm();
            this.aboutForm.ShowDialog();
        }
    }
}


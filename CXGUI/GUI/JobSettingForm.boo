namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms
import System.Configuration
import System.IO
import System.Runtime.Serialization.Formatters.Binary
import Microsoft.Win32
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer
import CXGUI.Config
import CXGUI.Job
import Clinky.IO

partial class JobSettingForm:
"""Description of JobSettingForm."""

	//Fields
	_videoInfo as VideoInfo
	
	_audioInfo as AudioInfo
	
	_jobItem as JobItem
	"""仅允许在确认退出时更改。"""
	
	_resolutionCal as ResolutionCalculator
	
	_resetter as ControlResetter
	
	_usingSepAudio as bool
	
	_cmdLineBox as CommandLineBox
	
	_videoEncConfig as VideoEncConfigBase
	"""与UI同步，确认退出时要赋予self._jobItem"""
	
	_audioEncConfig as AudioEncConfigBase
	"""与UI同步，确认退出时要赋予self._jobItem"""

	public def constructor():
		InitializeComponent()
		
	public def SetUpFormForItem(jobItem as JobItem):
		self._jobItem = jobItem
		self.destFileBox.Text = self._jobItem.DestFile

		self.tbSepAudio.Text = self._jobItem.ExternalAudio
		self.subtitleTextBox.Text = jobItem.SubtitleFile

		self._videoInfo = VideoInfo(self._jobItem.SourceFile)
		self._audioInfo = AudioInfo(self._jobItem.SourceFile)
		
		if not _videoInfo.Format == "avs":
			if self.tabControl1.Controls.Count != 3:
				self.tabControl1.Controls.Clear()
				self.tabControl1.Controls.AddRange((self.videoEditTabPage, self.audioEditTabPage, self.encTabPage, self.subtitleTabPage))
			InitializeJobConfig(self._jobItem.JobConfig)
			InitializeAvsConfig(self._jobItem.AvsConfig)
			InitializeSubtitleConfig(self._jobItem.SubtitleConfig)
		else:
			if self.tabControl1.Controls.Count != 2:
				self.tabControl1.Controls.Clear()
				self.tabControl1.Controls.AddRange((self.avsInputTabPage, self.encTabPage))
				AvsInputInitializeConfig(jobItem)
				
		self._videoEncConfig = Clone[of VideoEncConfigBase](self._jobItem.VideoEncConfig)
		self._audioEncConfig = Clone[of AudioEncConfigBase](self._jobItem.AudioEncConfig)
		InitializeEncConfig()

	private def AvsInputInitializeConfig(jobItem as JobItem):
		if jobItem.JobConfig.Container == OutputContainer.MKV:
			self.muxerComboBox.Text = "MKV"
		else:
			self.muxerComboBox.Text = "MP4"
	
	private def InitializeJobConfig(jobConfig as JobItemConfig):
		self.cbVideoMode.SelectedIndex = 1
		self.cbAudioMode.SelectedIndex = 1
		if self._videoInfo.HasVideo:
			self.cbVideoMode.Enabled = true
			self.cbVideoMode.SelectedIndex = cast(int, jobConfig.VideoMode)
		else:
			self.cbVideoMode.Enabled = false
			self.cbVideoMode.SelectedIndex = -1
		
		if not self.cbVideoMode.SelectedIndex == -1:
			self.chbSepAudio.Enabled = true
			self.chbSepAudio.Checked = self._jobItem.UsingExternalAudio
		else:
			self.chbSepAudio.Enabled = false
			self.chbSepAudio.Checked = false

		if self.chbSepAudio.Enabled and self.chbSepAudio.Checked and self.tbSepAudio.Text != "":
			self._usingSepAudio = true
		else:
			self._usingSepAudio = false

		if self._usingSepAudio or _audioInfo.StreamsCount:
			self.cbAudioMode.Enabled = true
			self.cbAudioMode.SelectedIndex = cast(int, jobConfig.AudioMode)
		else:
			self.cbAudioMode.Enabled = false
			self.cbAudioMode.SelectedIndex = -1
		
		SettleAudioControls()
		
		if self.cbVideoMode.SelectedIndex != 0:
			self.gbResolution.Enabled = false
			self.gbVideoSource.Enabled = false
		else:
			self.gbResolution.Enabled = true
			self.gbVideoSource.Enabled = true
			
		if jobConfig.Container == OutputContainer.MKV:
			self.muxerComboBox.Text = "MKV"
		else:
			self.muxerComboBox.Text = "MP4"
		

	#region TabPage1
	private def InitializeAvsConfig(avsConfig as AvisynthConfig):
	"""
	从AvsConfig对象导入到UI。
	"""
		_resolutionCal = ResolutionCalculator()

		InitializeResolutionCfg(avsConfig)
		InitializeFrameRateCfg(avsConfig)
		
		if _videoInfo.HasVideo:
			InitializeResolution(avsConfig, _videoInfo)
			InitializeFrameRate(avsConfig, _videoInfo)

		//Audio
		self.audioSourceComboBox.Text = avsConfig.AudioSourceFilter.ToString()
		self.downMixBox.Checked = avsConfig.DownMix
		self.normalizeBox.Checked = avsConfig.Normalize

		 //TODO

	private def InitializeResolutionCfg(avsConfig as AvisynthConfig):
		self.sourceResolutionCheckBox.CheckedChanged -= self.SourceResolutionCheckBoxCheckedChanged
		if avsConfig.UsingSourceResolution:
			self.sourceResolutionCheckBox.Checked = true
			for control as Control in self.gbResolution.Controls:
				control.Enabled = false
			self.sourceResolutionCheckBox.Enabled = true
		else:
			self.sourceResolutionCheckBox.Checked = false
			for control as Control in self.gbResolution.Controls:
				control.Enabled = true
		self.sourceResolutionCheckBox.CheckedChanged += self.SourceResolutionCheckBoxCheckedChanged
		self.allowAutoChangeARCheckBox.Checked = not avsConfig.LockAspectRatio
		self.lockToSourceARCheckBox.CheckedChanged -= self.UseSourceARCheckedChanged
		self.lockToSourceARCheckBox.Checked = avsConfig.LockToSourceAR
		self.lockToSourceARCheckBox.CheckedChanged += self.UseSourceARCheckedChanged
		
		if avsConfig.LockToSourceAR or self.sourceResolutionCheckBox.Checked:
			self.aspectRatioBox.Enabled = false
		else:
			self.aspectRatioBox.Enabled = true
		self.resizerBox.Text = avsConfig.Resizer.ToString()
		if self.resizerBox.SelectedIndex == -1:
			self.resizerBox.SelectedIndex = 0

	private def InitializeResolution(avsConfig as AvisynthConfig, videoInfo as VideoInfo):
		
		if avsConfig.Mod not in (2, 4, 8, 16, 32):
			avsConfig.Mod = 2
		_resolutionCal.Mod = avsConfig.Mod
		_resolutionCal.LockAspectRatio = avsConfig.LockAspectRatio
		_resolutionCal.LockToSourceAR = avsConfig.LockToSourceAR
		if not avsConfig.LockToSourceAR and avsConfig.AspectRatio > 0:
			_resolutionCal.AspectRatio = avsConfig.AspectRatio
		else:
			_resolutionCal.AspectRatio = videoInfo.DisplayAspectRatio
		if avsConfig.Height > 0:
			_resolutionCal.Height = avsConfig.Height
		else:
			_resolutionCal.Height = videoInfo.Height
		if avsConfig.Width > 0:
			_resolutionCal.Width = avsConfig.Width
		else:
			_resolutionCal.Width = videoInfo.Width
		RefreshResolution(null)

	private def InitializeFrameRateCfg(avsConfig as AvisynthConfig):
		self.videoSourceBox.Text = avsConfig.VideoSourceFilter.ToString()
		if self.videoSourceBox.SelectedIndex == -1:
			self.videoSourceBox.SelectedIndex = 0				
		self.convertFPSCheckBox.Checked = avsConfig.ConvertFPS
		//TODO sourceResolutionCheckBox convertFPSCheckBox sourceFrameRateCheckBox的相关方法要改
	
	private def InitializeFrameRate(avsConfig as AvisynthConfig, videoInfo as VideoInfo):
		if self._jobItem.AvsConfig.UsingSourceFrameRate:
			self.frameRateBox.Text = videoInfo.FrameRate.ToString()
			//引发事件
			self.sourceFrameRateCheckBox.Checked = true
		else:
			self.frameRateBox.Text = avsConfig.FrameRate.ToString()
			self.sourceFrameRateCheckBox.Checked = false
		

	private def RefreshResolution(caller as object):
		self.heightBox.Text = _resolutionCal.Height.ToString() if caller is not self.heightBox
		self.widthBox.Text = _resolutionCal.Width.ToString() if caller is not self.widthBox
		self.aspectRatioBox.Text = _resolutionCal.AspectRatio.ToString() if caller is not self.aspectRatioBox
		self.modBox.SelectedIndexChanged -= self.ModBoxSelectedIndexChanged
		self.modBox.Text = _resolutionCal.Mod.ToString()
		self.modBox.SelectedIndexChanged += self.ModBoxSelectedIndexChanged

	private def InitializeSubtitleConfig(subtitleConfig as SubtitleConfig):
		self.fontButton.Text = subtitleConfig.Fontname
		self.fontSizeBox.Text = subtitleConfig.Fontsize.ToString()
		self.fontBottomBox.Text = subtitleConfig.MarginV.ToString()
		self.customSubCheckBox.Checked = subtitleConfig.UsingStyle
		self.CustomSubCheckBoxCheckedChanged(null, null)
		

	private def WidthBoxKeyUp(sender as object, e as System.Windows.Forms.KeyEventArgs):
		width as int
		int.TryParse(self.widthBox.Text, width)
		if width > 0 and width <= 1920:
			temp = _resolutionCal.Width
			_resolutionCal.Width = width
			if _resolutionCal.Height > 1080:
				_resolutionCal.Width = temp
			else:
				RefreshResolution(widthBox)

	//UI设置与AvisynthWriter对象同步
	private def HeightBoxKeyUp(sender as object, e as System.Windows.Forms.KeyEventArgs):
		height as int
		int.TryParse(self.heightBox.Text, height)
		if height > 0 and height <= 1080:
			temp = _resolutionCal.Height
			_resolutionCal.Height = height
			if _resolutionCal.Width > 1920:
				_resolutionCal.Height = temp
			else:
				RefreshResolution(heightBox)

	private def AspectRatioBoxKeyUp(sender as object, e as System.Windows.Forms.KeyEventArgs):
		ar as double
		double.TryParse(self.aspectRatioBox.Text, ar)
		if ar > 0:
			temp = _resolutionCal.AspectRatio
			_resolutionCal.AspectRatio = ar
			if _resolutionCal.Width <= 1920 and _resolutionCal.Height <= 1080:
				RefreshResolution(self.aspectRatioBox)
			else:
				_resolutionCal.AspectRatio = temp
	
	private def ResolutionValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		RefreshResolution(null)

	private def ModBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		if _videoInfo.HasVideo:
			_resolutionCal.Mod = int.Parse(modBox.Text)
			RefreshResolution(modBox)
	
	private def AllowAutoChangeARCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		_resolutionCal.LockAspectRatio = not allowAutoChangeARCheckBox.Checked
	
	private def SourceFrameRateCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		if sourceFrameRateCheckBox.Checked:
			self.frameRateBox.Text = _videoInfo.FrameRate.ToString()
			self.frameRateBox.Enabled = false
			self.convertFPSCheckBox.Enabled = false
		else:
			self.frameRateBox.Enabled = true
			self.convertFPSCheckBox.Enabled = true
		
	private def SourceResolutionCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		if self.sourceResolutionCheckBox.Checked == true:
			if _videoInfo.HasVideo:
				_resolutionCal.Mod = 2
				_resolutionCal.AspectRatio = _videoInfo.DisplayAspectRatio
				_resolutionCal.Height = _videoInfo.Height
				_resolutionCal.Width = _videoInfo.Width
				RefreshResolution(null)
			for control as Control in self.gbResolution.Controls:
				control.Enabled = false
			self.sourceResolutionCheckBox.Enabled = true
		else:
			for control as Control in self.gbResolution.Controls:
				control.Enabled = true
			if self.lockToSourceARCheckBox.Checked:
				self.aspectRatioBox.Enabled = false

	private def BtOutBrowseClick(sender as object, e as System.EventArgs):
		if self.destFileBox.Text == "":
			self.destFileBox.Text = self._jobItem.DestFile
		try:	
			self.saveFileDialog1.InitialDirectory = Path.GetDirectoryName(self.destFileBox.Text)
			self.saveFileDialog1.FileName = Path.GetFileName(self.destFileBox.Text)
			self.saveFileDialog1.ShowDialog()
		except:
			MessageBox.Show("无效路径或含非法字符。", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

	#endregion

	#region EncTabPage x264Config

	private def InitializeEncConfig():
	"""
	从x264Config和NeroAacConfig的对象导入到UI。此后任何操作都是同步的。
	"""
		RefreshX264UI()
		RefreshNeroAac()

	private def RefreshX264UI(): 
	
		x264config = self._videoEncConfig as x264Config
		for control as Control in self.groupBox4.Controls:
			node = x264config.GetNode(control.Name.Replace(char('_'), char('-')))
			if node == null:
				continue
			control.Enabled = not node.Locked
			if node.Type == NodeType.Bool:
				checkBox = control as CheckBox
				checkBox.CheckedChanged -= self.BoolChanged
				checkBox.Checked = node.Bool
				checkBox.CheckedChanged += self.BoolChanged
			elif node.Type == NodeType.Num:
				control.Text = node.Num.ToString()
			elif node.Type == NodeType.Str:
				control.Text = node.Str
			elif node.Type == NodeType.StrOptionIndex:
				comboBox = control as ComboBox
				comboBox.SelectedIndexChanged -= self.StringOptionChanged
				comboBox.SelectedIndex = node.StrOptionIndex
				comboBox.SelectedIndexChanged += self.StringOptionChanged
		self.rateControlBox.SelectedIndexChanged -= self.RateControlBoxSelectedIndexChanged
		if x264config.GetNode("crf").InUse:
			self.rateControlBox.SelectedIndex = 0
			self.rateFactorBox.Text = x264config.GetNode("crf").Num.ToString()
			self.label9.Text = "量化器"
		elif x264config.GetNode("qp").InUse:
			self.rateControlBox.SelectedIndex = 1
			self.rateFactorBox.Text = x264config.GetNode("qp").Num.ToString()
			self.label9.Text = "质量"
		elif x264config.GetNode("bitrate").InUse:
			self.rateControlBox.SelectedIndex = x264config.TotalPass+1
			self.rateFactorBox.Text = x264config.GetNode("bitrate").Num.ToString()
			self.label9.Text = "码率"
		self.rateControlBox.SelectedIndexChanged += self.RateControlBoxSelectedIndexChanged
			
		self.useCustomCmdBox.Checked = self._videoEncConfig.UsingCustomCmd
		UseCustomCmdBoxCheckedChanged(null, null)
			

	private def RateControlBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		_x264config = self._videoEncConfig as x264Config
		if self.rateControlBox.SelectedIndex == 0:
			_x264config.SetNumOption("crf", 23)
		elif self.rateControlBox.SelectedIndex == 1:
			_x264config.SetNumOption("qp", 23)
		else:
			_x264config.SetNumOption("bitrate", 700)
			_x264config.TotalPass = self.rateControlBox.SelectedIndex-1
			_x264config.CurrentPass = 1
		RefreshX264UI()
	
	private def RateFactorBoxValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		config = self._videoEncConfig as x264Config
		if self.rateControlBox.SelectedIndex == 0:
			name = "crf"
		elif self.rateControlBox.SelectedIndex == 1:
			name = "qp"
		elif self.rateControlBox.SelectedIndex in (2, 3, 4):
			name = "bitrate"
		try:
			value = double.Parse(self.rateFactorBox.Text)
		except:
			self.rateFactorBox.Text = config.GetNode(name).Num.ToString()
			return
		if name != "crf":
			value = Math.Floor(value)
		config.SetNumOption(name, value)
		self.rateFactorBox.Text = config.GetNode(name).Num.ToString()
	
	private def BoolChanged(sender as object, e as System.EventArgs):
		checkBox as CheckBox = sender
		if checkBox.Enabled:
			name = checkBox.Name.Replace(char('_'), char('-'))
			(self._videoEncConfig as x264Config).SetBooleanOption(name, checkBox.Checked)
			RefreshX264UI()	

	private def StringOptionChanged(sender as object, e as System.EventArgs):
		box as ComboBox = sender
		if box.Enabled:
			name = box.Name.Replace(char('_'), char('-'))
			(self._videoEncConfig as x264Config).SelectStringOption(name, box.SelectedIndex)
			RefreshX264UI()
	#endregion
	
	#region EncTabPage NeroAacConfig

	private def RefreshNeroAac():
		neroAacConfig = self._audioEncConfig as NeroAacConfig
		if neroAacConfig.Quality > 0:
			self.neroAacRateControlBox.SelectedIndex = 0
			self.neroAacRateFactorBox.Text = neroAacConfig.Quality.ToString()
		elif neroAacConfig.BitRate > 0:
			self.neroAacRateControlBox.SelectedIndex = 1
			self.neroAacRateFactorBox.Text = neroAacConfig.BitRate.ToString()
		elif neroAacConfig.ConstantBitRate > 0:
			self.neroAacRateControlBox.SelectedIndex = 2
			self.neroAacRateFactorBox.Text = neroAacConfig.ConstantBitRate.ToString()

	private def NeroAacRateControlBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		neroAacCfg = self._audioEncConfig as NeroAacConfig
		i = self.neroAacRateControlBox.SelectedIndex
		if i == 0:
			self.label14.Text = "质量"
			if neroAacCfg.Quality == 0:
				self.neroAacRateFactorBox.Text = "0.5"
				neroAacCfg.Quality = 0.5
		elif i == 1:
			self.label14.Text = "码率"
			if neroAacCfg.BitRate == 0:
				self.neroAacRateFactorBox.Text = "96"
				neroAacCfg.BitRate = 96
		elif i == 2:
			self.label14.Text = "码率"
			if neroAacCfg.ConstantBitRate == 0:
				self.neroAacRateFactorBox.Text = "96" 
				neroAacCfg.ConstantBitRate = 96

	private def NeroAacRateFactorBoxValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		neroAacCfg = self._audioEncConfig as NeroAacConfig
		index = self.neroAacRateControlBox.SelectedIndex
		text = self.neroAacRateFactorBox.Text
		if index == 0:
			try:
				neroAacCfg.Quality = double.Parse(text)
			except:
				self.neroAacRateFactorBox.Text = neroAacCfg.Quality.ToString()
		elif index == 1:
			try:
				neroAacCfg.BitRate = int.Parse(text)
			except:
				self.neroAacRateFactorBox.Text = neroAacCfg.BitRate.ToString()
		elif index == 2:
			try:
				neroAacCfg.ConstantBitRate = int.Parse(text)
			except:
				self.neroAacRateFactorBox.Text = neroAacCfg.ConstantBitRate.ToString()
		self.RefreshNeroAac()

	#endregion
	
	
	private def SaveToAvsConfig(avsConfig as AvisynthConfig):
	"""
	从UI导出到AvsConfig对象。
	"""
		if _videoInfo.HasVideo:
			if self.sourceResolutionCheckBox.Checked:
				avsConfig.UsingSourceResolution = true
				avsConfig.Width = 0
				avsConfig.Height = 0
				avsConfig.AspectRatio = 0
			else:
				//未处理可能的错误
				avsConfig.UsingSourceResolution = false
				avsConfig.Width = int.Parse(self.widthBox.Text)
				avsConfig.Height = int.Parse(self.heightBox.Text)
				avsConfig.AspectRatio = double.Parse(self.aspectRatioBox.Text)
			avsConfig.LockAspectRatio = not self.allowAutoChangeARCheckBox.Checked
			avsConfig.LockToSourceAR = self.lockToSourceARCheckBox.Checked
			avsConfig.Mod = int.Parse(self.modBox.Text)
			avsConfig.Resizer = Enum.Parse(ResizeFilter, self.resizerBox.Text)
			avsConfig.VideoSourceFilter = Enum.Parse(VideoSourceFilter, self.videoSourceBox.Text)
			if self.sourceFrameRateCheckBox.Checked:
				avsConfig.UsingSourceFrameRate = true
				avsConfig.FrameRate = 0
			else:
				avsConfig.UsingSourceFrameRate = false
				avsConfig.FrameRate = double.Parse(self.frameRateBox.Text)
			avsConfig.ConvertFPS = self.convertFPSCheckBox.Checked

		if _audioInfo.StreamsCount:
			avsConfig.AudioSourceFilter = Enum.Parse(AudioSourceFilter, self.audioSourceComboBox.Text)
			avsConfig.DownMix = self.downMixBox.Checked
			avsConfig.Normalize = self.normalizeBox.Checked	
		
	private def SaveToJobConfig(jobConfig as JobItemConfig):
		jobConfig.VideoMode = cast(StreamProcessMode, self.cbVideoMode.SelectedIndex)
		jobConfig.AudioMode = cast(StreamProcessMode, self.cbAudioMode.SelectedIndex)
		jobConfig.VideoMode = StreamProcessMode.None if jobConfig.VideoMode == -1
		jobConfig.AudioMode = StreamProcessMode.None if jobConfig.AudioMode == -1
		if self.muxerComboBox.Text == "MKV":
			jobConfig.Container = OutputContainer.MKV
		elif self.muxerComboBox.Text == "MP4":
			jobConfig.Container = OutputContainer.MP4
		
	
	private def SaveToSubtitleConfig(subtitleConfig as SubtitleConfig):
		subtitleConfig.Fontname = self.fontDialog1.Font.Name
		int.TryParse(self.fontSizeBox.Text, subtitleConfig.Fontsize)
		int.TryParse(self.fontBottomBox.Text, subtitleConfig.MarginV)
		subtitleConfig.UsingStyle = self.customSubCheckBox.Checked
		
	private def OkButtonClick(sender as object, e as System.EventArgs):	
		self._jobItem.VideoEncConfig = self._videoEncConfig
		self._jobItem.AudioEncConfig = self._audioEncConfig
		
		self._jobItem.State = JobState.NotProccessed
		self._jobItem.ProfileName = self.profileBox.Text
		
		
		if self._videoInfo.Format == "avs":
			self.AvsInputSaveConfig(self._jobItem.JobConfig)
		else:
			try:
				dir = Path.GetDirectoryName(self.destFileBox.Text)
				if dir == "" or dir == null:
					self.destFileBox.Text = self._jobItem.DestFile
					
				elif IsSameFile(self._jobItem.SourceFile, saveFileDialog1.FileName):
					MessageBox.Show("与源媒体文件同名。请更改文件名。", "文件重名", MessageBoxButtons.OK, MessageBoxIcon.Warning)
					return
				elif Exists(saveFileDialog1.FileName):
					result = MessageBox.Show("${Path.GetFileName(saveFileDialog1.FileName)} 已存在。\n要替换它吗？", "确认另存为", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					if result == DialogResult.No:
						return
				elif Directory.Exists(dir):
					self._jobItem.DestFile = self.destFileBox.Text
				elif not Directory.Exists(dir):
					result = MessageBox.Show("目标文件夹不存在。是否新建？", "文件夹不存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
					if result == DialogResult.OK:
						Directory.CreateDirectory(Path.GetDirectoryName(self.destFileBox.Text))
						self._jobItem.DestFile = self.destFileBox.Text
					else:
						self.destFileBox.Text = self._jobItem.DestFile
				else:
					self._jobItem.DestFile = self.destFileBox.Text
			except:
				self.destFileBox.Text = self._jobItem.DestFile
	
			if self.chbSepAudio.Checked:
				if self.tbSepAudio.Text != "":
					self._jobItem.UsingExternalAudio = true
					self._jobItem.ExternalAudio = self.tbSepAudio.Text
				else:
					result = MessageBox.Show("未指定外挂音轨。确定退出吗？", "", MessageBoxButtons.YesNo,
					MessageBoxIcon.Information)
					if result == DialogResult.No:
						return
					elif result == DialogResult.Yes:
						self.chbSepAudio.Checked = false
						self._jobItem.UsingExternalAudio = false
						self._jobItem.ExternalAudio = ""
	
			if self.subtitleTextBox.Text != "":
				self._jobItem.SubtitleFile = self.subtitleTextBox.Text
			SaveToAvsConfig(self._jobItem.AvsConfig)
			SaveToJobConfig(self._jobItem.JobConfig)
			SaveToSubtitleConfig(self._jobItem.SubtitleConfig)
		
		_resetter.Clear()
		self.DialogResult = DialogResult.OK
		self.Close()
		
	private def AvsInputSaveConfig(jobConfig as JobItemConfig):
		if self.muxerComboBox.Text == "MKV":
			jobConfig.Container = OutputContainer.MKV
		elif self.muxerComboBox.Text == "MP4":
			jobConfig.Container = OutputContainer.MP4

	private def CancelButtonClick(sender as object, e as System.EventArgs):
		_resetter.ResetControls()
		_resetter.Clear()
		self.DialogResult = DialogResult.Cancel
		self.Close()

	private def JobSettingFormLoad(sender as object, e as System.EventArgs):
		if _resetter == null:
			_resetter = ControlResetter()
		_resetter.SaveControls(self)
		

	private def MediaSettingFormFormClosed(sender as object, e as System.Windows.Forms.FormClosedEventArgs):
		if e.CloseReason == System.Windows.Forms.CloseReason.UserClosing:
			if _resetter.Changed():
#				for control in _resetter.GetChangedControls():
#					MessageBox.Show(control.Name)
				result = MessageBox.Show("保存更改吗？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				if result == DialogResult.Yes:
					self.OkButtonClick(null, null)
				else:
					self.CancelButtonClick(null, null)

	private def AllowInteger (sender as object, e as System.Windows.Forms.KeyPressEventArgs):
		kc = cast(int, e.KeyChar)
		if (kc < 48 or kc > 57) and kc != 8 and kc != 13:
			e.Handled = true
			
	private def AllowFloat (sender as object, e as System.Windows.Forms.KeyPressEventArgs):
		kc = cast(int, e.KeyChar)
		if (kc < 48 or kc > 57) and kc not in (8, 13, 46):
			e.Handled = true
		if (sender as Control).Text.Contains(".") and kc == 46:
			e.Handled = true
	
	public def Clear ():
		self._videoEncConfig = null
		self._audioEncConfig = null
		self._videoInfo = null
		self._audioInfo = null
		self._resolutionCal = null
		self.widthBox.Text = ""
		self.heightBox.Text = ""
		self.aspectRatioBox.Text = ""
		self.frameRateBox.Text = ""
		self.subtitleTextBox.Text = ""
	
	private def CbVideoModeSelectedIndexChanged(sender as object, e as System.EventArgs):
		if self.cbVideoMode.SelectedIndex == 0:
			self.gbResolution.Enabled = true
			self.gbVideoSource.Enabled = true
		elif self.cbVideoMode.SelectedIndex == 1:
			self.gbResolution.Enabled = false
			self.gbVideoSource.Enabled = false
		elif self.cbVideoMode.SelectedIndex == 2:
			if self.cbAudioMode.SelectedIndex in (-1, 2):
				MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Warning)
				self.cbVideoMode.SelectedIndex = 0
			else:
				self.gbResolution.Enabled = false
				self.gbVideoSource.Enabled = false

	private def CbAudioModeSelectedIndexChanged(sender as object, e as System.EventArgs):
			
		if self.Created and self.cbAudioMode.SelectedIndex == 2:
			if self.cbVideoMode.SelectedIndex in (-1, 2):
				MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Warning)
				self.cbAudioMode.SelectedIndex = 0
		SettleAudioControls()
			

	private def SettleAudioControls():
	"""
	设置有关音频的控件的Enable属性，及其必然后果。
		
	"""
	
		if self._audioInfo.StreamsCount or self._usingSepAudio:
			self.cbAudioMode.Enabled = true
		else:
			self.cbAudioMode.Enabled = false
			self.cbAudioMode.SelectedIndex = -1
			
		if (self._audioInfo.StreamsCount or self._usingSepAudio) and self.cbAudioMode.SelectedIndex == 0:
			self.audioSourceComboBox.Enabled = true
			self.downMixBox.Enabled = true
			self.normalizeBox.Enabled = true
		else:
			self.audioSourceComboBox.Enabled = false
			self.downMixBox.Enabled = false
			self.normalizeBox.Enabled = false

	private def ChbSepAudioCheckedChanged(sender as object, e as System.EventArgs):
		if self.chbSepAudio.Checked:
			self.tbSepAudio.Enabled = true
			self.btSepAudio.Enabled = true
			if self.tbSepAudio.Text != "":
				self._usingSepAudio = true
				if self.cbAudioMode.SelectedIndex in (-1, 2):
					self.cbAudioMode.SelectedIndex = 0
		elif self.cbVideoMode.SelectedIndex in (-1, 2):
			MessageBox.Show("音频与视频必选其一。", "无效操作", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			self.chbSepAudio.Checked = true
		else:
			self.tbSepAudio.Enabled = false
			self.btSepAudio.Enabled = false
			self._usingSepAudio = false
		SettleAudioControls()

	private def BtSepAudioClick(sender as object, e as System.EventArgs):
		self.openFileDialog1.ShowDialog()
		audioInfo = AudioInfo(self.openFileDialog1.FileName)
		if not audioInfo.StreamsCount:
			MessageBox.Show("${self.openFileDialog1.FileName}\n检测不到所选文件中的音频流。", "检测失败",
			MessageBoxButtons.OK)
		else:
			self._usingSepAudio = true
			self.tbSepAudio.Text = self.openFileDialog1.FileName
			self.cbAudioMode.SelectedIndex = 0
		SettleAudioControls()
	
	private def ProfileBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		//TODO 这里的profile列表变更后，主界面也要跟着变
		try:
			_profile = Profile(self.profileBox.Text)
		except as ProfileNotFoundException:
			MessageBox.Show("预设文件不存在或已损坏。将刷新预设列表。", "预设读取失败", 
				MessageBoxButtons.OK, MessageBoxIcon.Warning)

			self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged
			self.profileBox.Items.Clear()
			self.profileBox.Items.AddRange(Profile.GetExistingProfilesNamesOnHardDisk())
			if not self.profileBox.Items.Contains("Default"):
				Profile.RebuildDefault("Default")
				self.profileBox.Items.Add("Default")
			self.profileBox.SelectedItem = "Default"
			_profile = Profile("Default")
			self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged
		
		InitializeJobConfig(_profile.JobConfig)
		InitializeAvsConfig(_profile.AvsConfig)
		InitializeSubtitleConfig(_profile.SubtitleConfig)
		self._videoEncConfig = _profile.VideoEncConfig
		self._audioEncConfig = _profile.AudioEncConfig
		InitializeEncConfig()
	
	private def SaveProfileButtonClick(sender as object, e as System.EventArgs):
		avsConfig as AvisynthConfig
		jobConfig as JobItemConfig
		subtitleConfig as SubtitleConfig
		if self._videoInfo.Format == "avs":
			avsConfig = self._jobItem.AvsConfig
			subtitleConfig = self._jobItem.SubtitleConfig
			jobConfig = JobItemConfig()
			self.AvsInputSaveConfig(jobConfig)
		else:
			avsConfig = AvisynthConfig()
			subtitleConfig = SubtitleConfig()
			jobConfig = JobItemConfig()
			SaveToAvsConfig(avsConfig)
			SaveToJobConfig(jobConfig)
			SaveToSubtitleConfig(subtitleConfig)
		Profile.Save(self.profileBox.Text, jobConfig, avsConfig, self._videoEncConfig, self._audioEncConfig, subtitleConfig)
		self.profileBox.Items.Add(self.profileBox.Text) if self.profileBox.Text not in self.profileBox.Items
		
	public def UpdateProfiles(profileNames as (string), selectedProfile as string):
		self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged
		self.profileBox.Items.Clear()
		self.profileBox.Items.AddRange(profileNames)
		self.profileBox.SelectedItem = selectedProfile
		self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged
		
	public def GetProfiles() as (string):
		return array(string, self.profileBox.Items)
	
	private def UseCustomCmdBoxCheckedChanged(sender as object, e as System.EventArgs):
		//TODO 选项导入导出
		if self._cmdLineBox == null:
			self._cmdLineBox = CommandLineBox()
		if self.useCustomCmdBox.Checked:
			for control as Control in self.groupBox4.Controls:
				control.Enabled = false
			self.useCustomCmdBox.Enabled = true
			self.editCmdButton.Enabled = true
			self._cmdLineBox.CmdLine = self._videoEncConfig.GetArgument()
			self._videoEncConfig.UsingCustomCmd = true
		else:
			for control as Control in self.groupBox4.Controls:
				control.Enabled = true
			self.editCmdButton.Enabled = false
			self._cmdLineBox.CmdLine = ""
			self._videoEncConfig.UsingCustomCmd = false
	
	private def EditCmdButtonClick(sender as object, e as System.EventArgs):
	//TODO 第一次要引入VideoENCcONFIG的数据，之后都使用CusntomConfig的数据
		self._cmdLineBox.ShowDialog()
		self._videoEncConfig.CustomCmdLine = self._cmdLineBox.CmdLine 
	
	private def SaveFileDialog1FileOk(sender as object, e as System.ComponentModel.CancelEventArgs):
			ext = '.' + self.muxerComboBox.Text.ToLower()
			if not saveFileDialog1.FileName.ToLower().EndsWith(ext):
				saveFileDialog1.FileName += ext
			if IsSameFile(self._jobItem.SourceFile, saveFileDialog1.FileName):
				MessageBox.Show("与源媒体文件同名。请更改文件名。", "文件重名", MessageBoxButtons.OK, MessageBoxIcon.Warning)
				e.Cancel = true
				return
			if Exists(saveFileDialog1.FileName):
				result = MessageBox.Show("${Path.GetFileName(saveFileDialog1.FileName)} 已存在。\n要替换它吗？", "确认另存为", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				if result == DialogResult.No:
					return
			self.destFileBox.Text = self.saveFileDialog1.FileName
	
	private def MuxerComboBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		ext = self.muxerComboBox.Text.ToLower()
		self.destFileBox.Text = GetUniqueName(Path.ChangeExtension(self._jobItem.DestFile, ext))
	
	private def SubtitleButtonClick(sender as object, e as System.EventArgs):
		self.openFileDialog2.FileName = self.subtitleTextBox.Text
		self.openFileDialog2.ShowDialog()
		self.subtitleTextBox.Text = self.openFileDialog2.FileName
	
	private def FontButtonClick(sender as object, e as System.EventArgs):
		self.fontDialog1.Font = Font(self.fontButton.Text, 100) //TODO
		self.fontDialog1.ShowDialog()
		self.fontButton.Text = self.fontDialog1.Font.Name
	
	private def CustomSubCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		if self.customSubCheckBox.Checked:
			self.customSubGroupBox.Enabled = true
		else:
			self.customSubGroupBox.Enabled = false
	
	private def UseSourceARCheckedChanged(sender as object, e as System.EventArgs):
		_resolutionCal.LockToSourceAR = self.lockToSourceARCheckBox.Checked
		if self.lockToSourceARCheckBox.Checked:
			self.aspectRatioBox.Text = _videoInfo.DisplayAspectRatio.ToString()
			self.aspectRatioBox.Enabled = false
			_resolutionCal.AspectRatio = _videoInfo.DisplayAspectRatio
			self.RefreshResolution(self.aspectRatioBox)
		else:
			self.aspectRatioBox.Enabled = true
	
	private def FrameRateBoxValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		if self.frameRateBox.Text == '0':
			self.frameRateBox.Text = '1'
	
	private def PreviewButtonClick(sender as object, e as System.EventArgs):
		playerPath = ProgramConfig.Get().PlayerPath
		if not File.Exists(playerPath):
			MessageBox.Show("请在程序设置中指定有效的播放器路径。", "找不到播放器", MessageBoxButtons.OK, MessageBoxIcon.Error)
			return
		avsConfig = AvisynthConfig()
		jobConfig = JobItemConfig()
		subtitleConfig = SubtitleConfig()
		subtitle = ""
		sepAudio = ""
		previewContent = ""
		hasVideo = false
		hasAudio = false
		if self._videoInfo.Format == "avs":
			self.AvsInputSaveConfig(jobConfig)
		else:
			self.SaveToAvsConfig(avsConfig)
			self.SaveToJobConfig(jobConfig)
			self.SaveToSubtitleConfig(subtitleConfig)
			if File.Exists(self.subtitleTextBox.Text):
				subtitle = self.subtitleTextBox.Text
			if self.chbSepAudio.Checked and File.Exists(self.tbSepAudio.Text):
				sepAudio = self.tbSepAudio.Text
		
		if self._jobItem.JobConfig.VideoMode != StreamProcessMode.None:
			hasVideo = true
			if self._jobItem.JobConfig.VideoMode == StreamProcessMode.Encode:
				if subtitle != "" and subtitleConfig.UsingStyle:
					SubStyleWriter(subtitle, subtitleConfig).Write()
			elif self._jobItem.JobConfig.VideoMode == StreamProcessMode.Copy:
				avsConfig.UsingSourceFrameRate = true
				avsConfig.UsingSourceResolution = true
				avsConfig.ConvertFPS = true
			VideoAvsWriter(self._jobItem.SourceFile, avsConfig, subtitle).WriteScript('video.avs')
			previewContent += "video = import(\"video.avs\")"
		
		if sepAudio != "":
			audio = sepAudio
		else:
			audio = self._jobItem.SourceFile
		if self._jobItem.JobConfig.AudioMode != StreamProcessMode.None:
			hasAudio = true
			if self._jobItem.JobConfig.AudioMode == StreamProcessMode.Copy:
				avsConfig.Normalize = false
				avsConfig.DownMix = false
			AudioAvsWriter(audio, avsConfig).WriteScript('audio.avs')
			previewContent += "\r\naudio = import(\"audio.avs\")"
		if hasVideo and hasAudio:
			previewContent += "\r\nAudioDub(video, audio)"
		elif hasVideo:
			previewContent += "\r\nvideo"
		elif hasAudio:
			previewContent += "\r\naudio"
		if previewContent != "":
			File.WriteAllText("preview.avs", previewContent, System.Text.Encoding.Default)
			
			process = System.Diagnostics.Process()
			process.StartInfo.FileName = ProgramConfig.Get().PlayerPath
			process.StartInfo.Arguments = Path.GetFullPath("preview.avs")
			process.Start()
		
			
		
	

#		if jobItem.AvsConfig.VideoSourceFilter == VideoSourceFilter.FFVideoSource:
#			File.Delete(jobItem.SourceFile+'.ffindex')
#			
#		if usingSubStyleWriter:
#			substyleWriter.CleanUp()
		
		



		
#	private def EnableTroubleMakingEvents():
#		
#		self.customSubCheckBox.CheckedChanged += self.CustomSubCheckBoxCheckedChanged as System.EventHandler
#		self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged as System.EventHandler
#		self.cbAudioMode.SelectedIndexChanged += self.CbAudioModeSelectedIndexChanged as System.EventHandler
#		self.cbVideoMode.SelectedIndexChanged += self.CbVideoModeSelectedIndexChanged as System.EventHandler
#		self.rateControlBox.SelectedIndexChanged += self.RateControlBoxSelectedIndexChanged as System.EventHandler
#		self.chbSepAudio.CheckedChanged += self.ChbSepAudioCheckedChanged as System.EventHandler
#		self.sourceFrameRateCheckBox.CheckedChanged += self.SourceFrameRateCheckBoxCheckedChanged as System.EventHandler
#		self.videoSourceBox.SelectedIndexChanged += self.VideoSourceBoxSelectedIndexChanged as System.EventHandler
#		self.lockToSourceARCheckBox.CheckedChanged += self.UseSourceARCheckedChanged as System.EventHandler
#		self.sourceResolutionCheckBox.CheckedChanged += self.SourceResolutionCheckBoxCheckedChanged as System.EventHandler
#		self.modBox.SelectedIndexChanged += self.ModBoxSelectedIndexChanged as System.EventHandler
#		self.allowAutoChangeARCheckBox.CheckedChanged += self.AllowAutoChangeARCheckBoxCheckedChanged as System.EventHandler
#		self.preset.SelectedIndexChanged += self.StringOptionChanged as System.EventHandler
#		self.muxerComboBox.SelectedIndexChanged += self.MuxerComboBoxSelectedIndexChanged as System.EventHandler
#		self.neroAacRateControlBox.SelectedIndexChanged += self.NeroAacRateControlBoxSelectedIndexChanged as System.EventHandler
#		self.useCustomCmdBox.CheckedChanged += self.UseCustomCmdBoxCheckedChanged as System.EventHandler
#		self.slow_firstpass.CheckedChanged += self.BoolChanged as System.EventHandler
#		self.tune.SelectedIndexChanged += self.StringOptionChanged as System.EventHandler
#		self.level.SelectedIndexChanged += self.StringOptionChanged as System.EventHandler
#		self.profile.SelectedIndexChanged += self.StringOptionChanged as System.EventHandler
#		
#	private def DisableTroubleMakingSomeEvents():
#		
#		self.customSubCheckBox.CheckedChanged -= self.CustomSubCheckBoxCheckedChanged as System.EventHandler
#		self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged as System.EventHandler
#		self.cbAudioMode.SelectedIndexChanged -= self.CbAudioModeSelectedIndexChanged as System.EventHandler
#		self.cbVideoMode.SelectedIndexChanged -= self.CbVideoModeSelectedIndexChanged as System.EventHandler
#		self.rateControlBox.SelectedIndexChanged -= self.RateControlBoxSelectedIndexChanged as System.EventHandler
#		self.chbSepAudio.CheckedChanged -= self.ChbSepAudioCheckedChanged as System.EventHandler
#		self.sourceFrameRateCheckBox.CheckedChanged -= self.SourceFrameRateCheckBoxCheckedChanged as System.EventHandler
#		self.videoSourceBox.SelectedIndexChanged -= self.VideoSourceBoxSelectedIndexChanged as System.EventHandler
#		self.lockToSourceARCheckBox.CheckedChanged -= self.UseSourceARCheckedChanged as System.EventHandler
#		self.sourceResolutionCheckBox.CheckedChanged -= self.SourceResolutionCheckBoxCheckedChanged as System.EventHandler
#		self.modBox.SelectedIndexChanged -= self.ModBoxSelectedIndexChanged as System.EventHandler
#		self.allowAutoChangeARCheckBox.CheckedChanged -= self.AllowAutoChangeARCheckBoxCheckedChanged as System.EventHandler
#		self.preset.SelectedIndexChanged -= self.StringOptionChanged as System.EventHandler
#		self.muxerComboBox.SelectedIndexChanged -= self.MuxerComboBoxSelectedIndexChanged as System.EventHandler
#		self.neroAacRateControlBox.SelectedIndexChanged -= self.NeroAacRateControlBoxSelectedIndexChanged as System.EventHandler
#		self.useCustomCmdBox.CheckedChanged -= self.UseCustomCmdBoxCheckedChanged as System.EventHandler
#		self.slow_firstpass.CheckedChanged -= self.BoolChanged as System.EventHandler
#		self.tune.SelectedIndexChanged -= self.StringOptionChanged as System.EventHandler
#		self.level.SelectedIndexChanged -= self.StringOptionChanged as System.EventHandler
#		self.profile.SelectedIndexChanged -= self.StringOptionChanged as System.EventHandler

		

	




namespace CXGUI.GUI

import System
import System.Collections
import System.Drawing
import System.Windows.Forms
import System.Configuration
import System.IO
import System.Runtime.Serialization.Formatters.Binary
import CXGUI
import CXGUI.Avisynth
import CXGUI.VideoEncoding
import CXGUI.AudioEncoding
import CXGUI.StreamMuxer

partial class MediaSettingForm:
"""Description of MediaSettingForm."""

	_videoInfo as VideoInfo
	_audioInfo as AudioInfo
	_resolutionCal as ResolutionCalculator
	_resetter as ControlResetter
	_usingSepAudio as bool

	[Property(SourceFile)]
	_sourceFile as string
	
	[Property(DestFile)]
	_destFile as string
	
	[Property(SepAudio)]
	_sepAudio as string
	
	[Property(AvsConfig)]
	_avsConfig as AvisynthConfig
	
	[Property(VideoEncConfig)]
	_videoEncConfig as VideoEncConfigBase
	
	[Property(AudioEncConfig)]
	_audioEncConfig as AudioEncConfigBase

	[Property(JobConfig)]
	_jobConfig as JobItemConfig

	[Property(Changed)]
	_changed as bool
	
	[Property(UsingProfile)]
	_usingProfile as string


	public def constructor():
		InitializeComponent()
		
	public def SetUpForItem(jobItem as JobItem):
		
		_usingProfile = jobItem.ProfileName
		_sourceFile = jobItem.SourceFile
		_destFile = jobItem.DestFile
		self.destFileBox.Text = _destFile
		_sepAudio = jobItem.SeparateAudio
		self.tbSepAudio.Text = _sepAudio
		self._jobConfig = jobItem.JobConfig
		self._avsConfig = jobItem.AvsConfig
		self._videoEncConfig = jobItem.VideoEncConfig
		self._audioEncConfig = jobItem.AudioEncConfig
		_videoInfo = VideoInfo(_sourceFile)
		_audioInfo = AudioInfo(_sourceFile)
		InitializeJobConfig(_jobConfig)
		InitializeAvsConfig(_avsConfig)
		InitializeEncConfig()

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
			self.chbSepAudio.Checked = jobConfig.UseSeparateAudio
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
		if jobConfig.Muxer == Muxer.MKV:
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
		self.audioSourceComboBox.Text = avsConfig.AudioSource.ToString()
		self.downMixBox.Checked = avsConfig.DownMix
		self.normalizeBox.Checked = avsConfig.Normalize

		 //TODO

	private def InitializeResolutionCfg(avsConfig as AvisynthConfig):
		if avsConfig.Width == 0 and avsConfig.Height == 0:
			self.sourceResolutionCheckBox.Checked = true
			for control as Control in self.gbResolution.Controls:
				control.Enabled = false
			self.sourceResolutionCheckBox.Enabled = true
		else:
			self.sourceResolutionCheckBox.Checked = false
			for control as Control in self.gbResolution.Controls:
				control.Enabled = true
		self.lockARCheckBox.Checked = avsConfig.LockAspectRatio
		self.resizerBox.Text = avsConfig.Resizer.ToString()
		if self.resizerBox.SelectedIndex == -1:
			self.resizerBox.SelectedIndex = 0

	private def InitializeResolution(avsConfig as AvisynthConfig, videoInfo as VideoInfo):
		if avsConfig.Mod not in (2, 4, 8, 16, 32):
			avsConfig.Mod = 2
		_resolutionCal.Mod = avsConfig.Mod
		_resolutionCal.LockAspectRatio = avsConfig.LockAspectRatio
		if avsConfig.AspectRatio > 0:
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
		self.videoSourceBox.Text = avsConfig.VideoSource.ToString()
		if self.videoSourceBox.SelectedIndex == -1:
			self.videoSourceBox.SelectedIndex = 0				
		self.convertFPSCheckBox.Checked = avsConfig.ConvertFPS
		if sourceFrameRateCheckBox.Checked or self.videoSourceBox.Text == "DSS2":
			self.convertFPSCheckBox.Checked = true
			self.convertFPSCheckBox.Enabled = false
		else:
			self.convertFPSCheckBox.Enabled = true
	
	private def InitializeFrameRate(avsConfig as AvisynthConfig, videoInfo as VideoInfo):
		if avsConfig.FrameRate > 0:
			self.frameRateBox.Text = avsConfig.FrameRate.ToString()
			self.sourceFrameRateCheckBox.Checked = false
			self.frameRateBox.Enabled = true
		else:
			self.frameRateBox.Text = videoInfo.FrameRate.ToString()
			self.sourceFrameRateCheckBox.Checked = true
			self.frameRateBox.Enabled = false

	private def RefreshResolution(caller as object):
		self.heightBox.Text = _resolutionCal.Height.ToString() if caller is not self.heightBox
		self.widthBox.Text = _resolutionCal.Width.ToString() if caller is not self.widthBox
		self.aspectRatioBox.Text = _resolutionCal.AspectRatio.ToString() if caller is not self.aspectRatioBox
		self.modBox.Text = _resolutionCal.Mod.ToString()

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
	
	private def LockARCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		_resolutionCal.LockAspectRatio = lockARCheckBox.Checked
	
	private def SourceFrameRateCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		if sourceFrameRateCheckBox.Checked:
			self.frameRateBox.Text = _videoInfo.FrameRate.ToString()
			self.frameRateBox.Enabled = false
			self.convertFPSCheckBox.Enabled = false
		else:
			self.frameRateBox.Enabled = true
			if self.videoSourceBox.Text != "DSS2":
				self.convertFPSCheckBox.Enabled = true
			
	private def VideoSourceBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		if self.videoSourceBox.Text == "DSS2":
			self.convertFPSCheckBox.Checked = true
			self.convertFPSCheckBox.Enabled = false
		else:
			self.convertFPSCheckBox.Checked = _avsConfig.ConvertFPS
			if not self.sourceFrameRateCheckBox.Checked:
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

	private def BtOutBrowseClick(sender as object, e as System.EventArgs):
		if self.destFileBox.Text == "":
			self.destFileBox.Text = self.destFileBox.Text
		try:	
			self.saveFileDialog1.InitialDirectory = Path.GetDirectoryName(self.destFileBox.Text)
			self.saveFileDialog1.FileName = self.destFileBox.Text
			self.saveFileDialog1.ShowDialog()
			self.destFileBox.Text = saveFileDialog1.FileName
		except:
			MessageBox.Show("无效路径或含非法字符。", "", MessageBoxButtons.OK, MessageBoxIcon.Error)

	#endregion

	#region TabPage2 X264Config

	private def InitializeEncConfig():
	"""
	从X264Config和NeroAacConfig的对象导入到UI。此后任何操作都是同步的。
	"""
		RefreshX264UI()
		RefreshNeroAac()

	private def RefreshX264UI(): 
		x264config = _videoEncConfig as X264Config
		settings as Hash = x264config.GetSettingsDict()
		freezedOptions = x264config.GetFreezedOptions()
		for setting as DictionaryEntry in settings:
			setting.Key = (setting.Key as string).Replace(char('-'), char('_'))
			
			try:
				control = self.groupBox4.Controls[setting.Key as string]
				if setting.Value == null:
					if control.GetType() == ComboBox:
						(control as ComboBox).SelectedIndex = 0
				elif setting.Value.GetType() == bool:
					(control as CheckBox).Checked = setting.Value
				elif setting.Value.GetType() == string:
					control.Text = setting.Value
			except NullReferenceException:
				pass

		if settings["crf"] != null:
			self.rateControlBox.SelectedIndex = 0
			self.rateFactorBox.Text = settings["crf"].ToString()
			self.label9.Text = "量化器"
		elif settings["qp"] != null:
			self.rateControlBox.SelectedIndex = 1
			self.rateFactorBox.Text = settings["qp"].ToString()
			self.label9.Text = "质量"
		elif settings["bitrate"] != null:
			self.rateFactorBox.Text = settings["bitrate"].ToString()
			self.label9.Text = "码率"
			self.rateControlBox.SelectedIndex = 2
		
		for control as Control in self.groupBox4.Controls:
			control.Enabled = true
		for option as string in freezedOptions:
			option = option.Replace(char('-'), char('_'))
			try:
				self.groupBox4.Controls[option].Enabled = false
			except NullReferenceException:
				pass

	private def RateControlBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		_x264config = _videoEncConfig as X264Config
		if self.rateControlBox.SelectedIndex == 0:
			_x264config.SetFloatOption("crf", 23)
		elif self.rateControlBox.SelectedIndex == 1:
			_x264config.SetIntegerOption("qp", 23)
		else:
			_x264config.SetIntegerOption("bitrate", 700)
			_x264config.SetIntegerOption("_pass", 1)
		RefreshX264UI()
	
	private def RateFactorBoxValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		try:
			value = double.Parse(self.rateFactorBox.Text)
		except:
			RefreshX264UI()
			return
		config = _videoEncConfig as X264Config
		if self.rateControlBox.SelectedIndex == 0:
			config.SetFloatOption("crf", value)
		elif self.rateControlBox.SelectedIndex == 1:
			config.SetIntegerOption("qp", Math.Floor(value))
		elif self.rateControlBox.SelectedIndex in (2, 3, 4):
			config.SetIntegerOption("bitrate", Math.Floor(value))
		RefreshX264UI()	
	
	private def BoolChanged(sender as object, e as System.EventArgs):
		checkBox as CheckBox = sender
		if checkBox.Enabled:
			name = checkBox.Name.Replace(char('_'), char('-'))
			(_videoEncConfig as X264Config).SetBooleanOption(name, checkBox.Checked)
			RefreshX264UI()	

	private def StringChanged(sender as object, e as System.EventArgs):
		box as ComboBox = sender
		if box.Enabled:
			name = box.Name.Replace(char('_'), char('-'))
			(_videoEncConfig as X264Config).SelectStringOption(name, box.SelectedIndex)
			RefreshX264UI()
	#endregion
	
	#region TabPage2 NeroAacConfig

	private def RefreshNeroAac():
		neroAacConfig = _audioEncConfig as NeroAacConfig
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
				avsConfig.Width = 0
				avsConfig.Height = 0
				avsConfig.AspectRatio = 0
			else:
				//未处理可能的错误
				avsConfig.Width = int.Parse(self.widthBox.Text)
				avsConfig.Height = int.Parse(self.heightBox.Text)
				avsConfig.AspectRatio = double.Parse(self.aspectRatioBox.Text)
			avsConfig.LockAspectRatio = self.lockARCheckBox.Checked
			avsConfig.Mod = int.Parse(self.modBox.Text)
			avsConfig.Resizer = Enum.Parse(VideoScriptConfig.ResizeFilter, self.resizerBox.Text)
			avsConfig.VideoSource = Enum.Parse(VideoScriptConfig.VideoSourceFilter, self.videoSourceBox.Text)
			if self.sourceFrameRateCheckBox.Checked:
				avsConfig.FrameRate = 0
			else:
				avsConfig.FrameRate = double.Parse(self.frameRateBox.Text)
			avsConfig.ConvertFPS = self.convertFPSCheckBox.Checked

		if _audioInfo.StreamsCount:
			avsConfig.AudioSource = Enum.Parse(AudioScriptConfig.AudioSourceFilter, self.audioSourceComboBox.Text)
			avsConfig.DownMix = self.downMixBox.Checked
			avsConfig.Normalize = self.normalizeBox.Checked	
		
	private def SaveToJobConfig(jobConfig as JobItemConfig):
		jobConfig.UseSeparateAudio = self.chbSepAudio.Checked
		jobConfig.VideoMode = cast(JobMode, self.cbVideoMode.SelectedIndex)
		jobConfig.AudioMode = cast(JobMode, self.cbAudioMode.SelectedIndex)
		if self.muxerComboBox.Text == "MP4" and (jobConfig.VideoMode == JobMode.Copy or jobConfig.AudioMode == JobMode.Copy):
			jobConfig.Muxer = Muxer.FFMP4
		elif self.muxerComboBox.Text == "MP4" and (jobConfig.VideoMode == JobMode.Encode or jobConfig.AudioMode == JobMode.Encode):
			jobConfig.Muxer = Muxer.MP4Box
		else:
			jobConfig.Muxer = Muxer.None

		
	private def OkButtonClick(sender as object, e as System.EventArgs):
		if self.chbSepAudio.Checked:
			if self.tbSepAudio.Text != "":
				self._sepAudio = self.tbSepAudio.Text
			else:
				result = MessageBox.Show("未指定外挂音轨。确定退出吗？", "", MessageBoxButtons.YesNo,
				MessageBoxIcon.Information)
				if result == DialogResult.No:
					return
				elif result == DialogResult.Yes:
					self.chbSepAudio.Checked = false //TODO
		if _resetter.Changed:
			_changed = true
		SaveToAvsConfig(_avsConfig)
		SaveToJobConfig(_jobConfig)
		try:
			Path.GetDirectoryName(self.destFileBox.Text)
			self._destFile = self.destFileBox.Text
		except:
			self.destFileBox.Text = self._destFile
		_resetter.Clear()
		self.DialogResult = DialogResult.OK
		self.Close()

	private def CancelButtonClick(sender as object, e as System.EventArgs):
		_resetter.Clear()
		self.DialogResult = DialogResult.Cancel
		self.Close()

	private def MediaSettingFormLoad(sender as object, e as System.EventArgs):
		if _resetter == null:
			_resetter = ControlResetter()
		_resetter.SaveControls(self.gbResolution.Controls)
		_resetter.SaveControls(self.gbVideoSource.Controls)
		_resetter.SaveControls(self.gbAudioAvs.Controls)
		_resetter.SaveControls(self.groupBox4.Controls)
		_resetter.SaveControls(self.groupBox5.Controls)
		_resetter.SaveControls(self.groupBox6.Controls)
		_resetter.SaveControls(self.tabPage1.Controls)
		_resetter.SaveControls(self.Controls)

	private def MediaSettingFormFormClosed(sender as object, e as System.Windows.Forms.FormClosedEventArgs):
		if e.CloseReason == System.Windows.Forms.CloseReason.UserClosing:
			if _resetter.Changed():
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
		self._avsConfig = null
		self._videoEncConfig = null
		self._audioEncConfig = null
		self._jobConfig = null
		self._videoInfo = null
		self._audioInfo = null
		self._resolutionCal = null
		self._sourceFile = ""
		self._destFile = ""
		self._sepAudio = ""
		self.widthBox.Text = ""
		self.heightBox.Text = ""
		self.aspectRatioBox.Text = ""
		self.frameRateBox.Text = ""
	
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
		if self.cbAudioMode.SelectedIndex == 2:
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
			self.profileBox.Items.AddRange(Profile.GetProfileNames())
			if not self.profileBox.Items.Contains("Default"):
				Profile.RebuildDefault("Default")
				self.profileBox.Items.Add("Default")
			if self.profileBox.Items.Contains(self._usingProfile):
				self.profileBox.SelectedItem = self._usingProfile
				_profile = Profile(self._usingProfile)
			else:
				self.profileBox.SelectedItem = "Default"
				_profile = Profile("Default")
			self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged
		
		self._usingProfile = self.profileBox.Text
		self.AvsConfig = _profile.AvsConfig
		self.VideoEncConfig = _profile.VideoEncConfig
		self.AudioEncConfig = _profile.AudioEncConfig
		self.JobConfig = _profile.JobConfig
		InitializeJobConfig(_jobConfig)
		InitializeAvsConfig(_avsConfig)
		InitializeEncConfig()
	
	private def SaveProfileButtonClick(sender as object, e as System.EventArgs):
		SaveToAvsConfig(_avsConfig)
		SaveToJobConfig(_jobConfig)
		Profile.Save(self.profileBox.Text, _jobConfig, _avsConfig, _videoEncConfig, _audioEncConfig)
		self.profileBox.Items.Add(self.profileBox.Text)
		self._usingProfile = self.profileBox.Text
		
	public def ImportProfiles(profileNames as (string), selectedProfile as string):
		self.profileBox.SelectedIndexChanged -= self.ProfileBoxSelectedIndexChanged
		self.profileBox.Items.Clear()
		self.profileBox.Items.AddRange(profileNames)
		self.profileBox.SelectedItem = selectedProfile
		self.profileBox.SelectedIndexChanged += self.ProfileBoxSelectedIndexChanged
		
	public def GetProfiles() as (string):
		return array(string, self.profileBox.Items)
		

	




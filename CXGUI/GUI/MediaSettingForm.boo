//TODO 剥离AvisynthWriter类

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

partial class MediaSettingForm:
"""Description of MediaSettingForm."""

	_videoInfo as VideoInfo
	_audioInfo as AudioInfo
	_resolutionCal as ResolutionCalculator
	_resolutionCalSerialized as Stream
	_videoEncConfigSerialized as Stream
	_audioEncConfigSerialized as Stream
	tabPage1Resetter as ControlResetter
	tabPage2Resetter as ControlResetter

	[Property(SourceFile)]
	_sourceFile as string
	[Property(DestinationFile)]
	_destinationFile as string
	[Property(WriteVideoScript)]
	_writeVideoScript as bool
	[Property(WriteAudioScript)]
	_writeAudioScript as bool

	AvsConfig as AvsConfig:
		get:
			return _avsConfig
		set:
			_avsConfig = value
			InitializeTabPage1(_avsConfig)
	_avsConfig as AvsConfig
	
	VideoEncConfig as VideoEncConfigBase:
		get:
			return _videoEncConfig
		set:
			_videoEncConfig = value
			RefreshX264UI()
	_videoEncConfig as VideoEncConfigBase

	AudioEncConfig as AudioEncConfigBase:
		get:
			return _audioEncConfig
		set:
			_audioEncConfig = value
			RefreshNeroAac()
	_audioEncConfig as AudioEncConfigBase

	[Property(Changed)]
	_changed as bool

	public def constructor(sourceFile as string, destFile as string):
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		_sourceFile = sourceFile
		_destinationFile = destFile
		ReadProfile("default.profile")
		InitializeTabPage1(_avsConfig)
		InitializeTabPage2()

	private def ReadProfile(path as string):
	"""
	从profile文件中读取VideoEncConfig AudioEncConfig AvsConfig对象到本类的相关字段。
	"""
		formater = BinaryFormatter()
		CreatNewProfile = do():
			_profile = Profile()
			_videoEncConfig = _profile.VideoEncConfig
			_audioEncConfig = _profile.AudioEncConfig
			_avsConfig = _profile.AvsConfig
			stream = FileStream(path, FileMode.Create)
			formater.Serialize(stream, _profile)
			stream.Close()
		if not File.Exists(path):
			CreatNewProfile()
		else:
			try:
				stream = FileStream(path, FileMode.Open)
				_profile = formater.Deserialize(stream) as Profile
				_videoEncConfig = _profile.VideoEncConfig
				_audioEncConfig = _profile.AudioEncConfig
				_avsConfig = _profile.AvsConfig
				stream.Close()
			except:
				stream.Close()
				CreatNewProfile()

	public def constructor(sourceFile as string, destFile as string, avsConfig as AvsConfig, 
	videoEncConfig as VideoEncConfigBase, audioEncConfig as AudioEncConfigBase):
		// The InitializeComponent() call is required for Windows Forms designer support.
		InitializeComponent()
		_sourceFile = sourceFile
		_destinationFile = destFile
		self._avsConfig = avsConfig
		self._videoEncConfig = videoEncConfig
		self._audioEncConfig = audioEncConfig
		InitializeTabPage1(_avsConfig)
		InitializeTabPage2()
		

	#region TabPage1
	private def InitializeTabPage1(avsConfig as AvsConfig):
	"""
	从AvsConfig对象导入到UI。
	"""
		_videoInfo = VideoInfo(_sourceFile)
		_audioInfo = AudioInfo(_sourceFile)
		_resolutionCal = ResolutionCalculator()
		
		if not _videoInfo.HasVideo:
			_writeVideoScript = false
			self.groupBox1.Enabled = false
			self.groupBox2.Enabled = false
		else:
			_writeVideoScript = true
			//根据配置，计算和显示宽，高，宽高比，帧率
			if avsConfig.Mod not in (2, 4, 8, 16, 32):
				avsConfig.Mod = 2
			_resolutionCal.Mod = avsConfig.Mod
			_resolutionCal.FixAspectRatio = avsConfig.LockAspectRatio
			if avsConfig.AspectRatio > 0:
				_resolutionCal.AspectRatio = avsConfig.AspectRatio
			else:
				_resolutionCal.AspectRatio = _videoInfo.DisplayAspectRatio
			if avsConfig.Height > 0:
				_resolutionCal.Height = avsConfig.Height
			else:
				_resolutionCal.Height = _videoInfo.Height
			if avsConfig.Width > 0:
				_resolutionCal.Width = avsConfig.Width
			else:
				_resolutionCal.Width = _videoInfo.Width

			if avsConfig.FrameRate > 0:
				self.frameRateBox.Text = avsConfig.FrameRate.ToString()
				self.sourceFrameRateCheckBox.Checked = false
			else:
				self.sourceFrameRateCheckBox.Checked = true
				self.frameRateBox.Text = _videoInfo.FrameRate.ToString()
				self.frameRateBox.Enabled = false
			RefreshResolutionGroupBox(null)
		//groupBox1的其他内容
		if avsConfig.Width == 0 and avsConfig.Height == 0:
			self.sourceResolutionCheckBox.Checked = true
			for control as Control in self.groupBox1.Controls:
				control.Enabled = false
			self.sourceResolutionCheckBox.Enabled = true
		else:
			self.sourceResolutionCheckBox.Checked = false
		self.lockARCheckBox.Checked = avsConfig.LockAspectRatio
		self.resizerBox.Text = avsConfig.Resizer.ToString()
		if self.resizerBox.SelectedIndex == -1:
			self.resizerBox.SelectedIndex = 0

		//groupBox2
		self.videoSourceBox.Text = avsConfig.VideoSource.ToString()
		if self.videoSourceBox.SelectedIndex == -1:
			self.videoSourceBox.SelectedIndex = 0				
		self.convertFPSCheckBox.Checked = avsConfig.ConvertFPS
		if sourceFrameRateCheckBox.Checked or self.videoSourceBox.Text == "DSS2":
			self.convertFPSCheckBox.Enabled = false
		else:
			self.convertFPSCheckBox.Enabled = true

		//groupBox3
		if not _audioInfo.StreamsCount:
			_writeAudioScript = false
			self.groupBox3.Enabled = false
		else:
			_writeAudioScript = true
		
		self.audioSourceComboBox.Text = avsConfig.AudioSource.ToString()
		self.downMixBox.Checked = avsConfig.DownMix
		self.normalizeBox.Checked = avsConfig.Normalize
		
		//groupBox6
		self.muxerComboBox.Text = avsConfig.Muxer.ToString()
		
		//destFile
		self.destFileBox.Text = _destinationFile

	private def RefreshResolutionGroupBox(caller as object):
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
				RefreshResolutionGroupBox(widthBox)

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
				RefreshResolutionGroupBox(heightBox)

	private def AspectRatioBoxKeyUp(sender as object, e as System.Windows.Forms.KeyEventArgs):
		ar as double
		double.TryParse(self.aspectRatioBox.Text, ar)
		if ar > 0:
			temp = _resolutionCal.AspectRatio
			_resolutionCal.AspectRatio = ar
			if _resolutionCal.Width <= 1920 and _resolutionCal.Height <= 1080:
				RefreshResolutionGroupBox(self.aspectRatioBox)
			else:
				_resolutionCal.AspectRatio = temp
	
	private def ResolutionValidating(sender as object, e as System.ComponentModel.CancelEventArgs):
		RefreshResolutionGroupBox(null)

	private def ModBoxSelectedIndexChanged(sender as object, e as System.EventArgs):
		if _videoInfo.HasVideo:
			_resolutionCal.Mod = int.Parse(modBox.Text)
			RefreshResolutionGroupBox(modBox)
	
	private def LockARCheckBoxCheckedChanged(sender as object, e as System.EventArgs):
		_resolutionCal.FixAspectRatio = lockARCheckBox.Checked
	
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
			_resolutionCal.Mod = 2
			_resolutionCal.AspectRatio = _videoInfo.DisplayAspectRatio
			_resolutionCal.Height = _videoInfo.Height
			_resolutionCal.Width = _videoInfo.Width
			if _videoInfo.HasVideo:
				RefreshResolutionGroupBox(null)
			for control as Control in self.groupBox1.Controls:
				control.Enabled = false
			self.sourceResolutionCheckBox.Enabled = true
		else:
			for control as Control in self.groupBox1.Controls:
				control.Enabled = true
	
	private def BrowseButtonClick(sender as object, e as System.EventArgs):
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

	private def InitializeTabPage2():
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
	
	
	private def SaveToAvsConfig(avsConfig as AvsConfig):
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
		avsConfig.Muxer = Enum.Parse(StreamMuxer.Muxer, self.muxerComboBox.Text)

	private def MediaSettingFormLoad(sender as object, e as System.EventArgs):
		tabPage1Resetter = ControlResetter()
		tabPage1Resetter.SaveControls(array(self.groupBox1.Controls))
		tabPage1Resetter.SaveControls(array(self.groupBox2.Controls))
		tabPage1Resetter.SaveControls(array(self.groupBox3.Controls))
		tabPage1Resetter.SaveControls(array(self.tabPage1.Controls))
		tabPage2Resetter = ControlResetter()
		tabPage2Resetter.SaveControls(array(self.groupBox4.Controls))
		tabPage2Resetter.SaveControls(array(self.groupBox5.Controls))
		tabPage2Resetter.SaveControls(array(self.groupBox6.Controls))
		
		formatter = BinaryFormatter()
		_resolutionCalSerialized = MemoryStream()
		_videoEncConfigSerialized = MemoryStream()
		_audioEncConfigSerialized = MemoryStream()
		formatter.Serialize(_resolutionCalSerialized, _resolutionCal)
		formatter.Serialize(_videoEncConfigSerialized, _videoEncConfig)
		formatter.Serialize(_audioEncConfigSerialized, _audioEncConfig)
		
		
	private def SetDefaultButtonClick(sender as object, e as System.EventArgs):
		SaveToAvsConfig(_avsConfig)
		stream = FileStream("default.profile", FileMode.Create)
		formater = BinaryFormatter()
		_profile = Profile()
		_profile.AvsConfig = _avsConfig
		_profile.VideoEncConfig = self._videoEncConfig
		_profile.AudioEncConfig = self._audioEncConfig
		formater.Serialize(stream, _profile)
		stream.Close()
		
	private def OkButtonClick(sender as object, e as System.EventArgs):
		if tabPage1Resetter.ChangedCount() > 0 or tabPage2Resetter.ChangedCount() > 0:
			_changed = true
		tabPage1Resetter.Clear()
		tabPage2Resetter.Clear()
		_resolutionCalSerialized.Close()
		_videoEncConfigSerialized.Close()
		_audioEncConfigSerialized.Close()
		_avsConfig = AvsConfig()
		SaveToAvsConfig(_avsConfig)
		try:
			Path.GetDirectoryName(self.destFileBox.Text)
			self._destinationFile = self.destFileBox.Text
		except:
			self.destFileBox.Text = self._destinationFile
		self.Close()

	private def CancelButtonClick(sender as object, e as System.EventArgs):
		tabPage1Resetter.ResetControls()
		tabPage2Resetter.ResetControls()
		tabPage1Resetter.Clear()
		tabPage2Resetter.Clear()
		formatter = BinaryFormatter()
		_resolutionCalSerialized.Seek(0, SeekOrigin.Begin)
		_resolutionCal = formatter.Deserialize(_resolutionCalSerialized)
		_resolutionCalSerialized.Close()
		_videoEncConfigSerialized.Seek(0, SeekOrigin.Begin)
		_videoEncConfig = formatter.Deserialize(_videoEncConfigSerialized)
		_videoEncConfigSerialized.Close()
		_audioEncConfigSerialized.Seek(0, SeekOrigin.Begin)
		_audioEncConfig = formatter.Deserialize(_audioEncConfigSerialized)
		_audioEncConfigSerialized.Close()
		self.Close()

	private def MediaSettingFormFormClosed(sender as object, e as System.Windows.Forms.FormClosedEventArgs):
		if e.CloseReason == System.Windows.Forms.CloseReason.UserClosing:
			if tabPage1Resetter.ChangedCount() > 0 or tabPage2Resetter.ChangedCount() > 0:
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
	

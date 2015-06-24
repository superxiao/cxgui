namespace Cxgui.Job
{
    using Cxgui;
    using Cxgui.AudioEncoding;
    using Cxgui.Avisynth;
    using Cxgui.Config;
    using Cxgui.StreamMuxer;
    using Cxgui.VideoEncoding;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Windows.Forms;
    using Clinky;

    [Serializable]
    public class JobItem
    {
        private AudioEncConfigBase _audioEncConfig;
        [NonSerialized]
        private AudioEncoderHandlerBase _audioEncoder;
        private AvisynthConfig _avsConfig;
        private List<string> _filesToDeleteWhenProcessingFails = new List<string>(3);
        private Cxgui.Job.CxListViewItem _cxListViewItem;
        private string _destFile;
        private JobEvent _event;
        private string _externalAudio;
        private JobItemConfig _jobConfig;
        [NonSerialized]
        private MuxerHandlerBase _muxer;
        private string _profileName;
        private bool readAudioCfg;
        private bool readAvsCfg;
        private bool readJobCfg;
        private bool readSubCfg;
        private bool readVideoCfg;
        private string _sourceFile;
        private JobState _state;
        private Cxgui.Avisynth.SubtitleConfig _subtitleConfig;
        private string _subtitleFile;
        private bool _usingExternalAudio;
        private VideoEncConfigBase _videoEncConfig;
        [NonSerialized]
        private VideoEncoderHandlerBase _videoEncoder;
        private VideoInfo _videoInfo;
        private AudioInfo _audioInfo;
        private SubStyleWriter subStyleWriter;
        private bool _userConfirmedOverwriteFile;

        public JobItem(string sourceFile, string destFile, string profileName)
        {
            this._sourceFile = sourceFile;
            this._destFile = destFile;
            this._profileName = profileName;
            this._videoInfo = new VideoInfo(sourceFile);
            this._audioInfo = new AudioInfo(sourceFile);
            string[] items = new string[] { "未处理", sourceFile, destFile };
            this._cxListViewItem = new Cxgui.Job.CxListViewItem(items);
            this._cxListViewItem.JobItem = this;
            this._state = JobState.NotProccessed;
        }

        private void BuildMuxer()
        {
            if (this._jobConfig == null)
            {
                this._muxer = null;
            }
            else
            {
                switch (this.JobConfig.Container)
                {
                    case OutputContainer.MKV:
                        this._muxer = new MkvMergeHandler();
                        break;

                    case OutputContainer.MP4:
                        if ((this.JobConfig.VideoMode == StreamProcessMode.Copy) || (this.JobConfig.AudioMode == StreamProcessMode.Copy))
                        {
                            this._muxer = new FFMp4Handler();
                        }
                        else
                        {
                            string ext = Path.GetExtension(this._destFile).ToLower();
                            if (ext != ".mp4" && ext != ".m4v" && ext != ".m4a")
                            {
                                this._muxer = new FFMp4Handler();
                            }
                            else if ((this.JobConfig.VideoMode == StreamProcessMode.Encode) && (this.JobConfig.AudioMode == StreamProcessMode.Encode))
                            {
                                this._muxer = new Mp4BoxHandler();
                            }
                            else
                            {
                                this._muxer = null;
                            }
                        }
                        break;
                }
            }
        }

        private void BuildEncoder()
        { 
            this._videoEncoder = new X264Handler();
            this._audioEncoder = new NeroAacHandler();
        }

        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            this.CxListViewItem.JobItem = this;
        }

        private void ReadProfile(string profileName)
        {
            Profile profile = null;
            if ((this.readAvsCfg || this.readVideoCfg) || ((this.readAudioCfg || this.readJobCfg) || this.readSubCfg))
            {
                profile = new Profile(profileName);
            }
            if (this.readAvsCfg)
            {
                this._avsConfig = profile.AvsConfig;
                this.readAvsCfg = false;
            }
            if (this.readVideoCfg)
            {
                this._videoEncConfig = profile.VideoEncConfig;
                this.readVideoCfg = false;
            }
            if (this.readAudioCfg)
            {
                this._audioEncConfig = profile.AudioEncConfig;
                this.readAudioCfg = false;
            }
            if (this.readJobCfg)
            {
                this._jobConfig = profile.JobConfig;
                this.readJobCfg = false;
            }
            if (this.readSubCfg)
            {
                this._subtitleConfig = profile.SubtitleConfig;
                this.readSubCfg = false;
            }
        }

        /// <summary>
        /// 根据ProfileName获取各设置，附加混流控制器，并根据源文件内容修正音频和视频处理模式
        /// </summary>
        /// <param name="overWriteConfig">是否覆盖已存在的设置项。如为false，则仅当该设置项为null时读取。</param>
        public void SetUp(bool overWriteConfig)
        {
            if (this._avsConfig == null || overWriteConfig)
            {
                this.readAvsCfg = true;
            }
            if (this._videoEncConfig == null || overWriteConfig)
            {
                this.readVideoCfg = true;
            }
            if (this._audioEncConfig == null || overWriteConfig)
            {
                this.readAudioCfg = true;
            }
            if (this.JobConfig == null || overWriteConfig)
            {
                this.readJobCfg = true;
            }
            if (this.SubtitleConfig == null || overWriteConfig)
            {
                this.readSubCfg = true;
            }
            this.ReadProfile(this._profileName);
            if (!this._videoInfo.HasVideo)
            {
                this._jobConfig.VideoMode = StreamProcessMode.None;
            }
            if (this._videoInfo.AudioStreamsCount==0&&!this.UsingExternalAudio)
            {
                this._jobConfig.AudioMode = StreamProcessMode.None;
            }
            if (this._videoInfo.Format == "avs")
            {
                if (this._jobConfig.VideoMode == StreamProcessMode.Copy)
                {
                    this._jobConfig.VideoMode = StreamProcessMode.Encode;
                }
            }
            this.BuildMuxer();
            this.BuildEncoder();
            if (this._avsConfig.AutoLoadSubtitle&&!File.Exists(this._subtitleFile))
            {
                this._subtitleFile = FindFirstSubtitleFile();
            }
        }

        /// <summary>
        /// 在文件所在目录查找第一个字幕文件。如果不存在，返回空字串。
        /// </summary>
        /// <returns></returns>
        public string FindFirstSubtitleFile()
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(this._sourceFile));

            foreach (string file in files)
            {
                if (Path.GetFileName(file).ToLower().StartsWith(Path.GetFileNameWithoutExtension(this._sourceFile).ToLower()))
                {
                    switch (Path.GetExtension(file).ToLower())
                    {
                        case ".srt":
                        case ".ass":
                        case ".ssa":
                            return file;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 启动视频编码过程，这个过程是异步的。每次调用前必须先SetUp()。
        /// </summary>
        public void ProcessVideo()
        {
            new VideoAvsWriter(this._sourceFile, this._avsConfig, this._subtitleFile, this._videoInfo).WriteScript("video.avs");
            if (MyIO.Exists(this._subtitleFile) && this._subtitleConfig.UsingStyle)
            {
                this.subStyleWriter = new SubStyleWriter(this._subtitleFile, this._subtitleConfig);
                this.subStyleWriter.Write();
            }
            this._filesToDeleteWhenProcessingFails.Add(this._destFile);

            this._videoEncoder.AvsFile = "video.avs";
            this._videoEncoder.DestFile = this._destFile;
            X264Handler encoder = this._videoEncoder as X264Handler;
            encoder.Config = this._videoEncConfig as x264Config;
            encoder.Start();
        }

        /// <summary>
        /// 启动音频编码过程，这个过程是异步的。每次调用前必须先SetUp()。
        /// </summary>
        public void ProcessAudio()
        {
            string audio = string.Empty;
            if (this._usingExternalAudio && File.Exists(this._externalAudio))
            {
                audio = this._externalAudio;
            }
            else
            {
                audio = this._sourceFile;
            }
            new AudioAvsWriter(audio, this._avsConfig, this._audioInfo).WriteScript("audio.avs");
            string destAudio = string.Empty;
            if (this.JobConfig.VideoMode == StreamProcessMode.None)
            {
                destAudio = this.DestFile;
            }
            else
            {
                destAudio = Path.ChangeExtension(this.DestFile, "m4a");
                destAudio = MyIO.GetUniqueName(destAudio);
            }
            this.FilesToDeleteWhenProcessingFails.Add(destAudio);
            this._audioEncoder.AvsFile = "audio.avs";
            this._audioEncoder.DestFile = destAudio;
            NeroAacHandler encoder = this._audioEncoder as NeroAacHandler;
            encoder.Config = this._audioEncConfig as NeroAacConfig;
            this._audioEncoder.Start();
        }

        /// <summary>
        /// 启动混流过程，这个过程是异步的。每次调用前必须先SetUp()。
        /// </summary>
        public void ProcessMuxing()
        {
            string audioToMux, videoToMux;
            if (this._jobConfig.AudioMode == StreamProcessMode.Encode)
            {
                audioToMux = this._audioEncoder.DestFile;
            }
            else if (this._jobConfig.AudioMode == StreamProcessMode.Copy)
            {
                if (this._usingExternalAudio && (this._externalAudio != string.Empty))
                {
                    audioToMux = this._externalAudio;
                }
                else
                {
                    audioToMux = this._sourceFile;
                }
            }
            else
            {
                audioToMux = string.Empty;
            }
            if (this._jobConfig.VideoMode == StreamProcessMode.Encode)
            {
                videoToMux = this._videoEncoder.DestFile;
            }
            else if (this.JobConfig.VideoMode == StreamProcessMode.Copy)
            {
                videoToMux = this._sourceFile;
            }
            else
            {
                videoToMux = string.Empty;
            }
            this.FilesToDeleteWhenProcessingFails.Add(this.DestFile);
            this._muxer.VideoFile = videoToMux;
            this._muxer.AudioFile = audioToMux;
            this._muxer.DstFile = this.DestFile;
            this._muxer.Start();
        }

        //TODO: 线程不安全。如果ProcessVideo是异步的，其同步操作保证至少启动编码进程，则在其后同步关闭进程是安全的；如果ProcessVideo是同步的，必须使用异步操作来检查是否调用QuitProcessing，则难以保证检查成立时编码进程已开始，或检查不成立时编码进程未开始。
        public void QuitProcessing()
        {
            if (this._videoEncoder != null)
                this._videoEncoder.Stop();
            if (this._audioEncoder != null)
                this._audioEncoder.Stop();
            if (this._muxer != null)
                this._muxer.Stop();
        }

        public void ClearTempFiles()
        {
            if (this.subStyleWriter != null)
                this.subStyleWriter.DeleteTempFiles();
            // 可设置项
            File.Delete(this._sourceFile + ".ffindex");
        }

        public AudioEncConfigBase AudioEncConfig
        {
            get
            {
                return this._audioEncConfig;
            }
            set
            {
                this._audioEncConfig = value;
            }
        }

        /// <summary>
        /// 只应用作获取进度信息之用。
        /// </summary>
        public IAudioEncodingInfo AudioEncInfo
        {
            get
            {
                return this._audioEncoder;
            }
        }

        public AvisynthConfig AvsConfig
        {
            get
            {
                return this._avsConfig;
            }
            set
            {
                this._avsConfig = value;
            }
        }

        public Cxgui.Job.CxListViewItem CxListViewItem
        {
            get
            {
                return this._cxListViewItem;
            }
            set
            {
                this._cxListViewItem = value;
            }
        }

        public string DestFile
        {
            get
            {
                return this._destFile;
            }
            set
            {
                if (this._destFile != value)
                    this._userConfirmedOverwriteFile = false;
                this._destFile = value;
                this._cxListViewItem.SubItems[2].Text = value;
            }
        }

        public JobEvent Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
            }
        }

        public string ExternalAudio
        {
            get
            {
                return this._externalAudio;
            }
            set
            {
                this._externalAudio = value;
                if (File.Exists(value) && this._usingExternalAudio)
                    this._audioInfo = new AudioInfo(value);
            }
        }

        public List<string> FilesToDeleteWhenProcessingFails
        {
            get
            {
                return this._filesToDeleteWhenProcessingFails;
            }
            set
            {
                this._filesToDeleteWhenProcessingFails = value;
            }
        }

        public JobItemConfig JobConfig
        {
            get
            {
                return this._jobConfig;
            }
            set
            {
                if (((this._videoInfo.AudioStreamsCount == 0) && (value.AudioMode != StreamProcessMode.None)) || (!this._videoInfo.HasVideo && (value.VideoMode != StreamProcessMode.None)))
                {
                    throw new ArgumentException("Incorrect JobMode.");
                }
                this._jobConfig = value;
            }
        }

        public IMuxingInfo MuxingInfo
        {
            get
            {
                return this._muxer;
            }
        }

        public string ProfileName
        {
            get
            {
                return this._profileName;
            }
            set
            {
                this._profileName = value;
            }
        }

        public string SourceFile
        {
            get
            {
                return this._sourceFile;
            }
        }

        public JobState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                if (value == JobState.NotProccessed)
                {
                    this._cxListViewItem.SubItems[0].Text = "未处理";
                }
                else if (value == JobState.Waiting)
                {
                    this._cxListViewItem.SubItems[0].Text = "等待";
                }
                else if (value == JobState.Working)
                {
                    this._cxListViewItem.SubItems[0].Text = "工作中";
                }
                else if (value == JobState.Done)
                {
                    this._cxListViewItem.SubItems[0].Text = "完成";
                }
                else if (value == JobState.Stop)
                {
                    this._cxListViewItem.SubItems[0].Text = "中止";
                }
                else if (value == JobState.Error)
                {
                    this._cxListViewItem.SubItems[0].Text = "错误";
                }
            }
        }

        public Cxgui.Avisynth.SubtitleConfig SubtitleConfig
        {
            get
            {
                return this._subtitleConfig;
            }
            set
            {
                this._subtitleConfig = value;
            }
        }

        public string SubtitleFile
        {
            get
            {
                return this._subtitleFile;
            }
            set
            {
                this._subtitleFile = value;
            }
        }

        public bool UsingExternalAudio
        {
            get
            {
                return this._usingExternalAudio;
            }
            set
            {
                this._usingExternalAudio = value;
            }
        }

        public VideoEncConfigBase VideoEncConfig
        {
            get
            {
                return this._videoEncConfig;
            }
            set
            {
                this._videoEncConfig = value;
            }
        }

        /// <summary>
        /// 只应用作获取进度信息之用。
        /// </summary>
        public IVideoEncodingInfo VideoEncInfo
        {
            get
            {
                return this._videoEncoder;
            }
        }

        public VideoInfo VideoInfo
        {
            get
            {
                return this._videoInfo;
            }
        }

        /// <summary>
        /// 当更改JobItem的ExternalAudio属性时，如文件存在且UsingExternalAudio为true，则更新AudioInfo属性。
        /// </summary>
        public AudioInfo AudioInfo
        {
            get 
            {
                return this._audioInfo;
            }
        }

        public bool UserConfirmedOverwriteFile
        { get { return this._userConfirmedOverwriteFile; } set { this._userConfirmedOverwriteFile = value; } }
    }
}


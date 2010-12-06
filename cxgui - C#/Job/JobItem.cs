namespace CXGUI.Job
{
    using CXGUI;
    using CXGUI.AudioEncoding;
    using CXGUI.Avisynth;
    using CXGUI.Config;
    using CXGUI.StreamMuxer;
    using CXGUI.VideoEncoding;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class JobItem
    {
        protected AudioEncConfigBase _audioEncConfig;
        [NonSerialized]
        protected AudioEncoderHandler _audioEncoder;
        protected AvisynthConfig _avsConfig;
        protected List<string> _createdFiles = new List<string>(3);
        protected string _customAudioScript;
        protected string _customVideoScript;
        protected CXGUI.Job.CxListViewItem _cxListViewItem;
        protected string _destFile;
        protected string _encodedAudio;
        protected string _encodedVideo;
        protected JobEvent _event;
        protected string _externalAudio;
        protected JobItemConfig _jobConfig;
        [NonSerialized]
        protected MuxerBase _muxer;
        protected string _profileName;
        protected bool _readAudioCfg;
        protected bool _readAvsCfg;
        protected bool _readJobCfg;
        protected bool _readSubCfg;
        protected bool _readVideoCfg;
        protected string _sourceFile;
        protected JobState _state;
        protected CXGUI.Avisynth.SubtitleConfig _subtitleConfig;
        protected string _subtitleFile;
        protected bool _usingCustomAudioScript;
        protected bool _usingCustomVideoScript;
        protected bool _usingExternalAudio;
        protected VideoEncConfigBase _videoEncConfig;
        [NonSerialized]
        protected VideoEncoderHandler _videoEncoder;
        protected VideoInfo _videoInfo;
        protected AudioInfo _audioInfo;

        public JobItem(string sourceFile, string destFile, string profileName)
        {
            this._sourceFile = sourceFile;
            this._destFile = destFile;
            this._profileName = profileName;
            this._videoInfo = new VideoInfo(sourceFile);
            this._audioInfo = new AudioInfo(sourceFile);
            string[] items = new string[] { "未处理", sourceFile, destFile };
            this._cxListViewItem = new CXGUI.Job.CxListViewItem(items);
            this._cxListViewItem.JobItem = this;
            this._state = JobState.NotProccessed;
        }

        private void CreateNewMuxer()
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
                        this._muxer = new MKVMerge();
                        break;

                    case OutputContainer.MP4:
                        if ((this.JobConfig.VideoMode == StreamProcessMode.Copy) || (this.JobConfig.AudioMode == StreamProcessMode.Copy))
                        {
                            this._muxer = new FFMP4();
                        }
                        else
                        {
                            string ext = Path.GetExtension(this._destFile).ToLower();
                            if (ext != ".mp4" && ext != ".m4v" && ext != ".m4a")
                            {
                                this._muxer = new FFMP4();
                            }
                            else if ((this.JobConfig.VideoMode == StreamProcessMode.Encode) && (this.JobConfig.AudioMode == StreamProcessMode.Encode))
                            {
                                this._muxer = new MP4Box();
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

        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            this.CxListViewItem.JobItem = this;
        }

        private void ReadProfile(string profileName)
        {
            Profile profile = null;
            if ((this._readAvsCfg || this._readVideoCfg) || ((this._readAudioCfg || this._readJobCfg) || this._readSubCfg))
            {
                profile = new Profile(profileName);
            }
            if (this._readAvsCfg)
            {
                this._avsConfig = profile.AvsConfig;
                this._readAvsCfg = false;
            }
            if (this._readVideoCfg)
            {
                this._videoEncConfig = profile.VideoEncConfig;
                this._readVideoCfg = false;
            }
            if (this._readAudioCfg)
            {
                this._audioEncConfig = profile.AudioEncConfig;
                this._readAudioCfg = false;
            }
            if (this._readJobCfg)
            {
                this._jobConfig = profile.JobConfig;
                this._readJobCfg = false;
            }
            if (this._readSubCfg)
            {
                this._subtitleConfig = profile.SubtitleConfig;
                this._readSubCfg = false;
            }
        }
        /// <summary>
        /// 根据ProfileName获取各设置，附加混流控制器
        /// </summary>
        public void SetUp()
        {
            if (this._avsConfig == null)
            {
                this._readAvsCfg = true;
            }
            if (this._videoEncConfig == null)
            {
                this._readVideoCfg = true;
            }
            if (this._audioEncConfig == null)
            {
                this._readAudioCfg = true;
            }
            if (this.JobConfig == null)
            {
                this._readJobCfg = true;
            }
            if (this.SubtitleConfig == null)
            {
                this._readSubCfg = true;
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
            this.CreateNewMuxer();
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

        public AudioEncoderHandler AudioEncoder
        {
            get
            {
                return this._audioEncoder;
            }
            set
            {
                this._audioEncoder = value;
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

        public string CustomAudioScript
        {
            get
            {
                return this._customAudioScript;
            }
            set
            {
                this._customAudioScript = value;
            }
        }

        public string CustomVideoScript
        {
            get
            {
                return this._customVideoScript;
            }
            set
            {
                this._customVideoScript = value;
            }
        }

        public CXGUI.Job.CxListViewItem CxListViewItem
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
                this._destFile = value;
                this._cxListViewItem.SubItems[2].Text = value;
            }
        }

        public string EncodedAudio
        {
            get
            {
                return this._encodedAudio;
            }
            set
            {
                this._encodedAudio = value;
            }
        }

        public string EncodedVideo
        {
            get
            {
                return this._encodedVideo;
            }
            set
            {
                this._encodedVideo = value;
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
                return this._createdFiles;
            }
            set
            {
                this._createdFiles = value;
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
        
        public MuxerBase Muxer
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

        public CXGUI.Avisynth.SubtitleConfig SubtitleConfig
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

        public bool UsingCustomAudioScript
        {
            get
            {
                return this._usingCustomAudioScript;
            }
            set
            {
                this._usingCustomAudioScript = value;
            }
        }

        public bool UsingCustomVideoScript
        {
            get
            {
                return this._usingCustomVideoScript;
            }
            set
            {
                this._usingCustomVideoScript = value;
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

        public VideoEncoderHandler VideoEncoder
        {
            get
            {
                return this._videoEncoder;
            }
            set
            {
                this._videoEncoder = value;
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
    }
}


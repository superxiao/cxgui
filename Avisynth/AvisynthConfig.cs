namespace Cxgui.Avisynth
{
    using System;

    [Serializable]
    public class AvisynthConfig
    {
        protected double _aspectRatio;
        protected Cxgui.Avisynth.AudioSourceFilter _audioSourceFilter;
        protected bool _convertFpsForDS;
        protected bool _downMix = true;
        protected double _frameRate;
        protected int _height;
        protected bool _lockAspectRatio = true;
        protected bool _lockToSourceAR = true;
        protected int _mod = 2;
        protected bool _normalize;
        protected ResizeFilter _resizer;
        protected bool _usingSourceFrameRate = true;
        protected bool _usingSourceResolution = true;
        protected Cxgui.Avisynth.VideoSourceFilter _videoSourceFilter;
        protected int _width;
        protected bool _autoLoadSubtitle;
        protected string _customVideoScript;
        protected string _customAudioScript;
        protected bool _paddingCustomVideoScript = true;
        protected bool _paddingCustomAudioScript = true;

        public double AspectRatio
        {
            get
            {
                return this._aspectRatio;
            }
            set
            {
                this._aspectRatio = value;
            }
        }

        public Cxgui.Avisynth.AudioSourceFilter AudioSourceFilter
        {
            get
            {
                return this._audioSourceFilter;
            }
            set
            {
                this._audioSourceFilter = value;
            }
        }

        public bool ConvertFPS
        {
            get
            {
                return this._convertFpsForDS;
            }
            set
            {
                this._convertFpsForDS = value;
            }
        }

        public bool DownMix
        {
            get
            {
                return this._downMix;
            }
            set
            {
                this._downMix = value;
            }
        }

        public double FrameRate
        {
            get
            {
                return this._frameRate;
            }
            set
            {
                this._frameRate = value;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public bool LockAspectRatio
        {
            get
            {
                return this._lockAspectRatio;
            }
            set
            {
                this._lockAspectRatio = value;
            }
        }

        public bool LockToSourceAR
        {
            get
            {
                return this._lockToSourceAR;
            }
            set
            {
                this._lockToSourceAR = value;
            }
        }

        public int Mod
        {
            get
            {
                return this._mod;
            }
            set
            {
                this._mod = value;
            }
        }

        public bool Normalize
        {
            get
            {
                return this._normalize;
            }
            set
            {
                this._normalize = value;
            }
        }

        public ResizeFilter Resizer
        {
            get
            {
                return this._resizer;
            }
            set
            {
                this._resizer = value;
            }
        }

        public bool UsingSourceFrameRate
        {
            get
            {
                return this._usingSourceFrameRate;
            }
            set
            {
                this._usingSourceFrameRate = value;
            }
        }

        public bool UsingSourceResolution
        {
            get
            {
                return this._usingSourceResolution;
            }
            set
            {
                this._usingSourceResolution = value;
            }
        }

        public Cxgui.Avisynth.VideoSourceFilter VideoSourceFilter
        {
            get
            {
                return this._videoSourceFilter;
            }
            set
            {
                this._videoSourceFilter = value;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        public bool AutoLoadSubtitle
        {
            get
            {
                return this._autoLoadSubtitle;
            }
            set
            {
                this._autoLoadSubtitle = value;
            }
        }

        /// <summary>
        /// 自定义视频avs脚本内容。默认值为null。如果设为任何字符串（包括空字符串），将覆盖其他avs设置。对于avs脚本输入暂不生效。
        /// </summary>
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

        /// <summary>
        /// 自定义音频avs脚本内容。默认值为null。如果设为任何字符串（包括空字符串），将覆盖其他avs设置。对于avs脚本输入暂不生效。
        /// </summary>
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

        public bool PaddingCustomVideoScript
        { get { return this._paddingCustomVideoScript; } set { this._paddingCustomVideoScript = value; } }

        public bool PaddingCustomAudioScript
        { get { return this._paddingCustomAudioScript; } set { this._paddingCustomAudioScript = value; } }

    }
}


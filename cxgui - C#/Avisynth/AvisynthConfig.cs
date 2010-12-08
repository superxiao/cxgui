namespace CXGUI.Avisynth
{
    using System;

    [Serializable]
    public class AvisynthConfig
    {
        protected double _aspectRatio;
        protected CXGUI.Avisynth.AudioSourceFilter _audioSourceFilter;
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
        protected CXGUI.Avisynth.VideoSourceFilter _videoSourceFilter;
        protected int _width;
        protected bool _autoLoadSubtitle;

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

        public CXGUI.Avisynth.AudioSourceFilter AudioSourceFilter
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

        public CXGUI.Avisynth.VideoSourceFilter VideoSourceFilter
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
    }
}


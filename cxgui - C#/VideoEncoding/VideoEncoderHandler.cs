namespace CXGUI.VideoEncoding
{
    using CXGUI;
    using CXGUI.External;
    using System;
    using System.Diagnostics;
    using System.IO;

    [Serializable]
    public abstract class VideoEncoderHandler : IMediaProcessor
    {
        protected double _avgBitRate;
        protected string _avisynthScriptFile;
        protected long _currentFileSize;
        protected int _currentFrame;
        protected int _currentPosition;
        protected string _destinationFile;
        protected string _encoderPath;
        protected Process _encodingProcess = new Process();
        protected long _estimatedFileSize;
        protected string _log;
        protected double _processingFrameRate;
        protected int _progress;
        protected double _scriptFrameRate;
        protected AviSynthClip _scriptInfo;
        protected TimeSpan _timeLeft;
        protected TimeSpan _timeUsed;
        protected int _totalFrame;
        protected double _totalLength;
        protected bool processingDone;
        /// <summary>
        /// 如脚本有错误，引发AviSynthException；如脚本有效但不含视频，引发AvisynthVideoStreamNotFoundException
        /// </summary>
        /// <param name="avisynthScriptFile"></param>
        /// <param name="destinationFile"></param>
        public VideoEncoderHandler(string avisynthScriptFile, string destinationFile)
        {
            if (!File.Exists(avisynthScriptFile))
            {
                throw new FileNotFoundException(string.Empty, avisynthScriptFile);
            }
            avisynthScriptFile = Path.GetFullPath(avisynthScriptFile);
            IDisposable disposable = (this._scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(avisynthScriptFile)) as IDisposable;
            if (disposable != null)
                {
                    disposable.Dispose();
                    disposable = null;
                }
            if (!this._scriptInfo.HasVideo)
            {
                throw new AvisynthVideoStreamNotFoundException(avisynthScriptFile);
            }
            this._avisynthScriptFile = Path.GetFullPath(avisynthScriptFile);
            this._destinationFile = destinationFile;
            this._scriptFrameRate = ((double) this._scriptInfo.raten) / ((double) this._scriptInfo.rated);
            this._totalLength = ((double) this._scriptInfo.num_frames) / this._scriptFrameRate;
            this._totalFrame = this._scriptInfo.num_frames;
        }

        public abstract void Start();
        public abstract void Stop();

        public double AvgBitRate
        {
            get
            {
                return this._avgBitRate;
            }
        }

        public string AvisynthScriptFile
        {
            get
            {
                return this._avisynthScriptFile;
            }
        }

        public long CurrentFileSize
        {
            get
            {
                return this._currentFileSize;
            }
        }

        public int CurrentFrame
        {
            get
            {
                return this._currentFrame;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return this._currentPosition;
            }
        }

        public string DestinationFile
        {
            get
            {
                return this._destinationFile;
            }
            set
            {
                this._destinationFile = value;
            }
        }

        public string EncoderPath
        {
            get
            {
                return this._encoderPath;
            }
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException(value);
                }
                this._encoderPath = value;
            }
        }

        public long EstimatedFileSize
        {
            get
            {
                return this._estimatedFileSize;
            }
        }

        public string Log
        {
            get
            {
                return this._log;
            }
        }

        public bool ProcessingDone
        {
            get
            {
                return this.processingDone;
            }
        }

        public double ProcessingFrameRate
        {
            get
            {
                return this._processingFrameRate;
            }
        }

        public int Progress
        {
            get
            {
                return this._progress;
            }
        }

        public TimeSpan TimeLeft
        {
            get
            {
                return this._timeLeft;
            }
        }

        public TimeSpan TimeUsed
        {
            get
            {
                return this._timeUsed;
            }
        }

        public int TotalFrames
        {
            get
            {
                return this._totalFrame;
            }
        }

        public double TotalLength
        {
            get
            {
                return this._totalLength;
            }
        }
    }
}


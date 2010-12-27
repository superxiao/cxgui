namespace Cxgui.AudioEncoding
{
    using Cxgui;
    using Cxgui.External;
    using System;
    using System.Diagnostics;
    using System.IO;

    public abstract class AudioEncoderHandlerBase : IAudioEncodingInfo
    {
        protected string _avsFile;
        protected long _currentFileSize;
        protected int _currentPosition;
        protected string _destFile;
        protected Process encodingProcess;
        protected long _estimatedFileSize;
        protected double _length;
        protected string _log;
        protected bool _hasExisted;
        protected double _progress;
        protected AviSynthClip scriptInfo;
        protected TimeSpan _timeLeft;
        protected TimeSpan _timeUsed;

        /// <summary>
        /// 如脚本有错误或未安装AviSynth，引发AviSynthException；如脚本有效但不含音频，引发AvisynthVideoStreamNotFoundException
        /// </summary>
        /// <param name="encoderPath"></param>
        /// <param name="avsFile"></param>
        /// <param name="destFile"></param>
        protected AudioEncoderHandlerBase(string avsFile, string destFile)
            : this()
        {
            this.SetUpForAvsFile(avsFile);
            this._destFile = destFile;
        }

        protected AudioEncoderHandlerBase()
        {
            this.encodingProcess = new Process();
            this.encodingProcess.StartInfo.UseShellExecute = false;
            this.encodingProcess.StartInfo.RedirectStandardInput = true;
            this.encodingProcess.StartInfo.CreateNoWindow = true;
            this.encodingProcess.EnableRaisingEvents = true;
        }

        private void SetUpForAvsFile(string avsFile)
        {
            avsFile = Path.GetFullPath(avsFile);
            // 如果不是有效的avs脚本，则AvsSynthException
            using (this.scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(avsFile))
            { }
            // 是有效的avs脚本，但不包含音频内容，AvisynthVideoStreamNotFoundException
            if ((this.scriptInfo.ChannelsCount == 0) || (((int)this.scriptInfo.SamplesCount) == 0))
            {
                throw new AvisynthAudioStreamNotFoundException(avsFile);
            }
            this._avsFile = avsFile;
            this._length = ((double)this.scriptInfo.SamplesCount) / ((double)this.scriptInfo.AudioSampleRate);
        }

        public abstract void Start();

        public void Stop()
        {
            try
            {
                this.encodingProcess.Kill();
                this.encodingProcess.WaitForExit();
            }
            catch
            {
            }
        }

        public string AvsFile
        { get { return this._avsFile; } set { this.SetUpForAvsFile(value); } }

        public long CurrentFileSize
        {
            get
            {
                return this._currentFileSize;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return this._currentPosition;
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
            }
        }

        public long EstimatedFileSize
        {
            get
            {
                return this._estimatedFileSize;
            }
        }

        public double Length
        {
            get
            {
                return this._length;
            }
        }

        public string Log
        {
            get
            {
                return this._log;
            }
        }

        public bool HasExited
        {
            get
            {
                return this._hasExisted;
            }
        }

        public int Progress
        {
            get
            {
                return (int) this._progress;
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
    }
}


namespace Cxgui.AudioEncoding
{
    using Cxgui;
    using Cxgui.External;
    using System;
    using System.Diagnostics;
    using System.IO;

    [Serializable]
    public abstract class AudioEncoderHandlerBase : IAudioEncodingInfo
    {
        protected string _avsFile;
        protected long currentFileSize;
        protected int currentPosition;
        protected string _destFile;
        protected Process encodingProcess;
        protected long estimatedFileSize;
        protected double _length;
        protected string log;
        protected bool _hasExisted;
        protected double progress;
        protected AviSynthClip scriptInfo;
        protected TimeSpan timeLeft;
        protected TimeSpan timeUsed;

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
            catch (Exception)
            {
            }
        }

        public string AvsFile
        { get { return this._avsFile; } set { this.SetUpForAvsFile(value); } }

        public long CurrentFileSize
        {
            get
            {
                return this.currentFileSize;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return this.currentPosition;
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
                return this.estimatedFileSize;
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
                return this.log;
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
                return (int) this.progress;
            }
        }

        public TimeSpan TimeLeft
        {
            get
            {
                return this.timeLeft;
            }
        }

        public TimeSpan TimeUsed
        {
            get
            {
                return this.timeUsed;
            }
        }
    }
}


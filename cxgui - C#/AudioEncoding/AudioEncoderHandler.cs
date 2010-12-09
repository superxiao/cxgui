namespace Cxgui.AudioEncoding
{
    using Cxgui;
    using Cxgui.External;
    using System;
    using System.Diagnostics;
    using System.IO;

    [Serializable]
    public abstract class AudioEncoderHandler : IMediaProcessor
    {
        protected string avisynthScriptFile;
        protected long currentFileSize;
        protected int currentPosition;
        protected string destFile;
        protected Process encodingProcess;
        protected long estimatedFileSize;
        protected double length;
        protected string log;
        protected bool processingDone;
        protected double progress;
        protected AviSynthClip scriptInfo;
        protected string stantardError;
        protected TimeSpan timeLeft;
        protected TimeSpan timeUsed;

        /// <summary>
        /// 如脚本有错误，引发AviSynthException；如脚本有效但不含音频，引发AvisynthVideoStreamNotFoundException
        /// </summary>
        /// <param name="encoderPath"></param>
        /// <param name="avisynthScriptFile"></param>
        /// <param name="destFile"></param>
        public AudioEncoderHandler(string encoderPath, string avisynthScriptFile, string destFile)
        {
            if (!File.Exists(avisynthScriptFile))
            {
                throw new FileNotFoundException(string.Empty, avisynthScriptFile);
            }
            avisynthScriptFile = Path.GetFullPath(avisynthScriptFile);
            // 如果不是有效的avs脚本，则AvsSynthException
            IDisposable disposable = (this.scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(avisynthScriptFile)) as IDisposable;
            if (disposable != null)
                {
                    disposable.Dispose();
                    disposable = null;
                }
            // 是有效的avs脚本，但不包含音频内容，AvisynthVideoStreamNotFoundException
            if ((this.scriptInfo.ChannelsCount == 0) || (((int) this.scriptInfo.SamplesCount) == 0))
            {
                throw new AvisynthAudioStreamNotFoundException(avisynthScriptFile);
            }
            this.avisynthScriptFile = avisynthScriptFile;
            this.destFile = destFile;
            this.length = ((double) this.scriptInfo.SamplesCount) / ((double) this.scriptInfo.AudioSampleRate);
            this.encodingProcess = new Process();
            this.encodingProcess.StartInfo.UseShellExecute = false;
            this.encodingProcess.StartInfo.RedirectStandardInput = true;
            this.encodingProcess.StartInfo.CreateNoWindow = true;
            if (File.Exists(encoderPath))
            {
                this.encodingProcess.StartInfo.FileName = encoderPath;
            }
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

        public string AvisynthScriptFile
        {
            get
            {
                return this.avisynthScriptFile;
            }
        }

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
                return this.destFile;
            }
            set
            {
                this.destFile = value;
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
                return this.length;
            }
        }

        public string Log
        {
            get
            {
                return this.log;
            }
        }

        public bool ProcessingDone
        {
            get
            {
                return this.processingDone;
            }
        }

        public int Progress
        {
            get
            {
                return (int) this.progress;
            }
        }

        public string StantardError
        {
            get
            {
                return this.stantardError;
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


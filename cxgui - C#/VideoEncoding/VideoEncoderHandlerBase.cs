namespace Cxgui.VideoEncoding
{
    using Cxgui;
    using Cxgui.External;
    using System;
    using System.Diagnostics;
    using System.IO;

    [Serializable]
    public abstract class VideoEncoderHandlerBase : IVideoEncodingInfo
    {
        protected double _avgBitRate;
        protected string _avsFile;
        protected long _currentFileSize;
        protected int _currentFrame;
        protected int _currentPosition;
        protected string _destFile;
        protected Process encodingProcess;
        protected long _estimatedFileSize;
        protected string _log;
        protected double _processingFrameRate;
        protected int _progress;
        protected double _scriptFrameRate;
        protected AviSynthClip _scriptInfo;
        protected TimeSpan timeLeft;
        protected TimeSpan _timeUsed;
        protected int _totalFrame;
        protected double _totalLength;
        protected bool _hasExisted;
        /// <summary>
        /// 如脚本有错误或未安装AviSynth，引发AviSynthException；如脚本有效但不含视频，引发AvisynthVideoStreamNotFoundException
        /// </summary>
        /// <param name="avisynthScriptFile"></param>
        /// <param name="destinationFile"></param>
        protected VideoEncoderHandlerBase(string avsFile, string destFile)
            : this()
        {
            this.SetUpForAvsFile(avsFile);
            this._destFile = destFile;
        }

        protected VideoEncoderHandlerBase()
        {
            encodingProcess = new Process();
            this.encodingProcess.StartInfo.UseShellExecute = false;
            this.encodingProcess.StartInfo.CreateNoWindow = true;
            this.encodingProcess.EnableRaisingEvents = true;
        }

        private void SetUpForAvsFile(string avsFile)
        {
            avsFile = Path.GetFullPath(avsFile);
            using (this._scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(avsFile)) { }
            if (!this._scriptInfo.HasVideo)
                throw new AvisynthVideoStreamNotFoundException(avsFile);
            this._avsFile = avsFile;
            this._scriptFrameRate = ((double)this._scriptInfo.raten) / ((double)this._scriptInfo.rated);
            this._totalLength = ((double)this._scriptInfo.num_frames) / this._scriptFrameRate;
            this._totalFrame = this._scriptInfo.num_frames;
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

        public double AvgBitRate
        { get { return this._avgBitRate; } }

        /// <summary>
        /// 如脚本有错误或未安装AviSynth，引发AviSynthException；如脚本有效但不含视频，引发AvisynthVideoStreamNotFoundException
        /// </summary>
        public string AvsFile
        { get { return this._avsFile; } set { this.SetUpForAvsFile(value); } }

        public long CurrentFileSize
        { get { return this._currentFileSize; } }

        public int CurrentFrame
        { get { return this._currentFrame; } }

        public int CurrentPosition 
        { get { return this._currentPosition; } }

        public string DestFile 
        { get { return this._destFile; } set { this._destFile = value; } }

        public long EstimatedFileSize 
        { get { return this._estimatedFileSize; } }

        public string Log 
        { get { return this._log; } }

        public bool HasExited 
        { get { return this._hasExisted; } }

        public double ProcessingFrameRate 
        { get { return this._processingFrameRate; } }

        public int Progress 
        { get { return this._progress; } }

        public TimeSpan TimeLeft 
        { get { return this.timeLeft; } }

        public TimeSpan TimeUsed 
        { get { return this._timeUsed; } }

        public int TotalFrames 
        { get { return this._totalFrame; } }

        public double TotalLength 
        { get { return this._totalLength; } }
    }
}


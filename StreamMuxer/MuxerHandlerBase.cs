namespace Cxgui.StreamMuxer
{
    using Cxgui;
    using System;
    using System.Diagnostics;

    [Serializable]
    public abstract class MuxerHandlerBase : IMuxingInfo
    {
        protected string _audioFile;
        protected string _dstFile;
        protected bool _errOccured;
        protected Process muxerProcess;
        protected int _progress;
        protected TimeSpan _timeLeft;
        protected TimeSpan _timeUsed;
        protected string _videoFile;
        protected bool _hasExisted;

        protected MuxerHandlerBase()
        {
            this.muxerProcess = new Process();
            this.muxerProcess.StartInfo.UseShellExecute = false;
            this.muxerProcess.StartInfo.CreateNoWindow = true;
            this.muxerProcess.EnableRaisingEvents = true;
        }

        public abstract void Start();

        public void Stop()
        {
            try
            {
                this.muxerProcess.Kill();
                this.muxerProcess.WaitForExit();
            }
            catch (Exception)
            {
            }
        }

        public string AudioFile
        {
            get
            {
                return this._audioFile;
            }
            set
            {
                this._audioFile = value;
            }
        }

        public string DstFile
        {
            get
            {
                return this._dstFile;
            }
            set
            {
                this._dstFile = value;
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

        public string VideoFile
        {
            get
            {
                return this._videoFile;
            }
            set
            {
                this._videoFile = value;
            }
        }
    }
}


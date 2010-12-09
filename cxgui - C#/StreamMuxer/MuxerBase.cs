namespace Cxgui.StreamMuxer
{
    using Cxgui;
    using System;
    using System.Diagnostics;

    [Serializable]
    public abstract class MuxerBase : IMediaProcessor
    {
        protected string _audioFile;
        protected string _dstFile;
        protected bool _errOccured;
        protected Process muxerProcess = new Process();
        protected int _progress;
        protected TimeSpan _timeLeft;
        protected TimeSpan _timeUsed;
        protected string _videoFile;
        protected bool processingDone;

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

        public bool ErrorOccured
        {
            get
            {
                return this._errOccured;
            }
            set
            {
                this._errOccured = value;
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


namespace Cxgui.StreamMuxer
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;

    [Serializable]
    public class Mp4BoxHandler : MuxerHandlerBase
    {
        protected DateTime _startTime;

        public Mp4BoxHandler()
        {
            base.muxerProcess.StartInfo.FileName = "MP4box.exe"; 
            base.muxerProcess.StartInfo.RedirectStandardOutput = true;
            base.muxerProcess.OutputDataReceived += new DataReceivedEventHandler(this.UpdateProgress);
            base.muxerProcess.Exited += this.muxerProgress_Exited;
        }

        private void muxerProgress_Exited(object sender, EventArgs e)
        {
            base._hasExisted = true;
            if (base._progress >= 99)
                base._progress = 100;
            base._timeLeft = TimeSpan.Zero;
            this.muxerProcess.Dispose();
        }

        public override void Start()
        {
            base._hasExisted = false;
            base.muxerProcess.StartInfo.Arguments = new StringBuilder("-add \"").Append(base._audioFile).Append("\"#1 \"").Append(base._videoFile).Append("\"").ToString();
            this._startTime = DateTime.Now;
            base.muxerProcess.Start();
            this.muxerProcess.BeginOutputReadLine();
        }

        private void UpdateProgress(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                // File.AppendAllText("d:\\mp4box.log", e.Data);
                if (e.Data.Contains("Writing"))
                {
                    int num = e.Data.LastIndexOf('(');
                    int end = e.Data.LastIndexOf('/');
                    base._progress = int.Parse(e.Data.Substring(num + 1, end - num - 1));
                }
                base._timeUsed = (TimeSpan)(DateTime.Now - this._startTime);
                if (base._progress != 0)
                {
                    base._timeLeft = TimeSpan.FromSeconds((double)((int)((this._timeUsed.TotalSeconds * (100 - base._progress)) / ((double)base._progress))));
                }
                base._timeUsed = TimeSpan.FromSeconds((double)((int)this._timeUsed.TotalSeconds));
            }
        }
    }
}


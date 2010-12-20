namespace Cxgui.StreamMuxer
{
    using Clinky;
    using Cxgui;
    using System;
    using System.IO;
    using System.Text;
    using System.Diagnostics;

    [Serializable]
    public class MkvMergeHandler : MuxerHandlerBase
    {
        protected DateTime startTime;
        private string tempVideoOrAudio;

        public MkvMergeHandler()
        {
            base.muxerProcess.StartInfo.FileName = "mkvmerge.exe";
            base.muxerProcess.StartInfo.RedirectStandardOutput = true;
            base.muxerProcess.OutputDataReceived += new DataReceivedEventHandler(this.UpdateProgress);
            base.muxerProcess.Exited += this.muxerProgress_Exited;
        }

        private void muxerProgress_Exited(object sender, EventArgs e)
        {
            base._hasExisted = true;
            if (File.Exists(this.tempVideoOrAudio))
            {
                File.Delete(this.tempVideoOrAudio);
                this.tempVideoOrAudio = string.Empty;
            }
            base._timeLeft = TimeSpan.Zero;
        }

        private string GetArgument(VideoInfo vInfo, AudioInfo aInfo)
        {
            string str = "";
            if (vInfo.HasVideo && (aInfo.StreamsCount != 0))
            {
                return new StringBuilder("-o \"").Append(base._dstFile).Append("\" -A -d ").Append(vInfo.ID).Append(" \"").Append(base._videoFile).Append("\" -D -a ").Append(aInfo.ID).Append(" \"").Append(base._audioFile).Append("\"").ToString();
            }
            if (!vInfo.HasVideo)
            {
                return new StringBuilder("-o \"").Append(base._dstFile).Append("\" -D -a ").Append(aInfo.ID).Append(" \"").Append(base._audioFile).Append("\"").ToString();
            }
            if (aInfo.StreamsCount == 0)
            {
                str = new StringBuilder("-o \"").Append(base._dstFile).Append("\" -A -d ").Append(vInfo.ID).Append(" \"").Append(base._videoFile).Append("\"").ToString();
            }
            return str;
        }

        public override void Start()
        {
            base._hasExisted = false;
            if (base._videoFile == base._dstFile)
            {
                this.tempVideoOrAudio = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._videoFile), "temp" + Path.GetExtension(base._videoFile)));
                File.Move(base._videoFile, this.tempVideoOrAudio);
                base._videoFile = this.tempVideoOrAudio;
            }
            else if (base._audioFile == base._dstFile)
            {
                this.tempVideoOrAudio = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._audioFile), "temp" + Path.GetExtension(base._audioFile)));
                File.Move(base._audioFile, this.tempVideoOrAudio);
                base._audioFile = this.tempVideoOrAudio;
            }
            VideoInfo info = new VideoInfo(base._videoFile);
            AudioInfo info2 = new AudioInfo(base._audioFile);
            string argument = this.GetArgument(info as VideoInfo, info2 as AudioInfo);
            base.muxerProcess.StartInfo.Arguments = argument;
            this.startTime = DateTime.Now;
            base.muxerProcess.Start();
            this.muxerProcess.BeginOutputReadLine();
        }

        private void UpdateProgress(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.StartsWith("Progress"))
                {
                    base._progress = int.Parse(e.Data.Substring(10, e.Data.Length - 11));
                }
                base._timeUsed = (TimeSpan)(DateTime.Now - this.startTime);
                if (base._progress != 0)
                {
                    base._timeLeft = TimeSpan.FromSeconds((double)((int)((this._timeUsed.TotalSeconds * (100 - base._progress)) / ((double)base._progress))));
                }
                base._timeUsed = TimeSpan.FromSeconds((double)((int)this._timeUsed.TotalSeconds));
            }
        }
    }
}


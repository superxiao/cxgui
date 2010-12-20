namespace Cxgui.StreamMuxer
{
    using Cxgui;
    using System;
    using System.IO;
    using System.Text;
    using Clinky;
    using System.Diagnostics;

    [Serializable]
    public class FFMp4Handler : MuxerHandlerBase
    {
        protected DateTime startTime;
        private double length;
        private string tempVideoOrAudio;

        public FFMp4Handler()
        {
            base.muxerProcess.StartInfo.FileName = "ffmpeg.exe";
            base.muxerProcess.StartInfo.RedirectStandardError = true;
            base.muxerProcess.ErrorDataReceived += new DataReceivedEventHandler(this.UpdateProgress);
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
                return (new StringBuilder("-i \"").Append(base._videoFile).Append("\" -i \"").Append(base._audioFile).Append("\"").ToString() + new StringBuilder(" -y -vcodec copy -acodec copy -sn \"").Append(base._dstFile).Append("\" -map 0.").Append(vInfo.StreamID).Append(" -map 1.").Append(aInfo.StreamID).ToString());
            }
            if (!vInfo.HasVideo)
            {
                return (new StringBuilder("-i \"").Append(base._audioFile).Append("\"").ToString() + new StringBuilder(" -y -vn -acodec copy -sn \"").Append(base._dstFile).Append("\"").ToString());
            }
            if (aInfo.StreamsCount == 0)
            {
                str = new StringBuilder("-i \"").Append(base._videoFile).Append("\"").ToString() + new StringBuilder(" -y -vcodec copy -an -sn \"").Append(base._dstFile).Append("\"").ToString();
            }
            return str;
        }

        public override void Start()
        {
            base._hasExisted = false;
            if (base._videoFile == base._dstFile)
            {
                this.tempVideoOrAudio = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._videoFile), "temp" + Path.GetExtension(base._videoFile)));
                File.Move(base._videoFile, tempVideoOrAudio);
                base._videoFile = tempVideoOrAudio;
            }
            else if (base._audioFile == base._dstFile)
            {
                this.tempVideoOrAudio = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._audioFile), "temp" + Path.GetExtension(base._audioFile)));
                File.Move(base._audioFile, tempVideoOrAudio);
                base._audioFile = tempVideoOrAudio;
            }
            VideoInfo videoInfo = new VideoInfo(base._videoFile);
            AudioInfo audioInfo = new AudioInfo(base._audioFile);
            base.muxerProcess.StartInfo.Arguments = this.GetArgument(videoInfo, audioInfo);
            this.startTime = DateTime.Now;
            if (videoInfo.HasVideo)
            {
                this.length = videoInfo.Length;
            }
            else if (audioInfo.StreamsCount != 0)
            {
                this.length = audioInfo.Length;
            }
            base.muxerProcess.Start();
            base.muxerProcess.BeginErrorReadLine();
        }

        private void UpdateProgress(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.Contains("time="))
                {
                    int index = e.Data.IndexOf("time=") + 5;
                    string numstring = e.Data.Substring(e.Data.IndexOf("time=") + 5, e.Data.IndexOf("bitrate=") - e.Data.IndexOf("time=") - 5);
                    double num = double.Parse(numstring);
                    base._progress = (int)((num * 100) / this.length);
                }
                else
                {
                    if (e.Data.Contains("incorrect codec"))
                    {
                        throw new FormatNotSupportedException();
                    }
                    if (e.Data.Contains("non monotone"))
                    {
                        throw new FFmpegBugException();
                    }
                    if (e.Data.StartsWith("video:"))
                    {
                        base._progress = 100;
                    }
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


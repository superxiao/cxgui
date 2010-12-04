namespace CXGUI.StreamMuxer
{
    using CXGUI;
    using System;
    using System.IO;
    using System.Text;
    using Clinky;

    [Serializable]
    public class FFMP4 : MuxerBase
    {
        protected DateTime _startTime;

        public FFMP4()
        {
            base._process.StartInfo.FileName = "ffmpeg.exe";
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

        private void ReadStdErr(double length)
        {
            StreamReader standardError = base._process.StandardError;
            string line = string.Empty;
            while (1 != 0)
            {
                if (base._progress == 100)
                {
                    break;
                }
                line = standardError.ReadLine();
                if (line == null)
                {
                    line = string.Empty;
                }
                if (line.Length != 0)
                {
                    this.UpdateProgress(line, length);
                }
            }
        }

        public override void Start()
        {
            string uniqueName = "";
            if (base._videoFile == base._dstFile)
            {
                uniqueName = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._videoFile), "temp" + Path.GetExtension(base._videoFile)));
                File.Move(base._videoFile, uniqueName);
                base._videoFile = uniqueName;
            }
            else if (base._audioFile == base._dstFile)
            {
                uniqueName = MyIO.GetUniqueName(Path.Combine(Path.GetDirectoryName(base._audioFile), "temp" + Path.GetExtension(base._audioFile)));
                File.Move(base._audioFile, uniqueName);
                base._audioFile = uniqueName;
            }
            double length = 0;
            VideoInfo videoInfo = new VideoInfo(base._videoFile);
            AudioInfo audioInfo = new AudioInfo(base._audioFile);
            string argument = this.GetArgument(videoInfo, audioInfo);
            base._process.StartInfo.Arguments = argument;
            base._process.StartInfo.UseShellExecute = false;
            base._process.StartInfo.RedirectStandardError = true;
            base._process.StartInfo.CreateNoWindow = true;
            this._startTime = DateTime.Now;
            if (videoInfo.HasVideo)
            {
                length = videoInfo.Length;
            }
            else if (audioInfo.StreamsCount != 0)
            {
                length = audioInfo.Length;
            }
            base._process.Start();
            this.ReadStdErr(length);
            base._process.WaitForExit();
            base.processingDone = true; 
            File.Delete(uniqueName);
        }

        public override void Stop()
        {
            try
            {
                base._process.Kill();
                base._process.WaitForExit();
            }
            catch (Exception)
            {
            }
        }

        private void UpdateProgress(string line, double length)
        {
            if (line.Contains("time="))
            {
                int index = line.IndexOf("time=") + 5;
                string numstring = line.Substring(line.IndexOf("time=") + 5, line.IndexOf("bitrate=") - line.IndexOf("time=") - 5);
                double num = double.Parse(numstring);
                base._progress = (int) ((num * 100) / length);
            }
            else
            {
                if (line.Contains("incorrect codec"))
                {
                    throw new FormatNotSupportedException();
                }
                if (line.Contains("non monotone"))
                {
                    throw new FFmpegBugException();
                }
                if (line.StartsWith("video:"))
                {
                    base._progress = 100;
                }
            }
            base._timeUsed = (TimeSpan) (DateTime.Now - this._startTime);
            if (base._progress != 0)
            {
                base._timeLeft = TimeSpan.FromSeconds((double) ((int) ((this._timeUsed.TotalSeconds * (100 - base._progress)) / ((double) base._progress))));
            }
            base._timeUsed = TimeSpan.FromSeconds((double) ((int) this._timeUsed.TotalSeconds));
        }
    }
}


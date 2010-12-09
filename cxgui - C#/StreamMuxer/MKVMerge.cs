namespace Cxgui.StreamMuxer
{
    using Clinky;
    using Cxgui;
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class MKVMerge : MuxerBase
    {
        protected DateTime _startTime;

        public MKVMerge()
        {
            base.muxerProcess.StartInfo.FileName = "mkvmerge.exe";
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

        private void ReadStdErr()
        {
            StreamReader standardOutput = base.muxerProcess.StandardOutput;
            string line = string.Empty;
            while (1 != 0)
            {
                if (base._progress == 100)
                {
                    break;
                }
                line = standardOutput.ReadLine();
                if (line == null)
                {
                    line = string.Empty;
                }
                if (line.Length != 0)
                {
                    this.UpdateProgress(line);
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
            VideoInfo info = new VideoInfo(base._videoFile);
            AudioInfo info2 = new AudioInfo(base._audioFile);
            string argument = this.GetArgument(info as VideoInfo, info2 as AudioInfo);
            base.muxerProcess.StartInfo.Arguments = argument;
            base.muxerProcess.StartInfo.UseShellExecute = false;
            base.muxerProcess.StartInfo.RedirectStandardOutput = true;
            base.muxerProcess.StartInfo.CreateNoWindow = true;
            this._startTime = DateTime.Now;
            base.muxerProcess.Start();
            this.ReadStdErr();
            base.muxerProcess.WaitForExit();
            base.processingDone = true;
            if (File.Exists(uniqueName))
                File.Delete(uniqueName);
        }

        private void UpdateProgress(string line)
        {
            if (line.StartsWith("Progress"))
            {
                base._progress = int.Parse(line.Substring(10, line.Length - 11));
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


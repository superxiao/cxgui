namespace CXGUI.StreamMuxer
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    [Serializable]
    public class MP4Box : MuxerBase
    {
        protected DateTime _startTime;

        public MP4Box()
        {
            base._process.StartInfo.FileName = "MP4box.exe";
        }

        private void ReadStdErr()
        {
            StreamReader standardOutput = base._process.StandardOutput;
            string line = string.Empty;
            while (1 != 0)
            {
                line = standardOutput.ReadLine();
                if (line == null)
                {
                    line = string.Empty;
                }
                this.UpdateProgress(line);
            }
        }

        public override void Start()
        {
            base._process.StartInfo.Arguments = new StringBuilder("-add \"").Append(base._audioFile).Append("\"#1 \"").Append(base._videoFile).Append("\"").ToString();
            base._process.StartInfo.UseShellExecute = false;
            base._process.StartInfo.RedirectStandardOutput = true;
            base._process.StartInfo.CreateNoWindow = true;
            Thread thread = new Thread(new ThreadStart(this.ReadStdErr));
            this._startTime = DateTime.Now;
            base._process.Start();
            thread.Start();
            base._process.WaitForExit();
            base.processingDone = true;
            if (base._progress >= 0x63)
            {
                base._progress = 100;
                base._timeLeft = new TimeSpan((long) 0);
            }
            thread.Abort();
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

        private void UpdateProgress(string line)
        {
            if (line.Contains("Writing"))
            {
                int num = line.LastIndexOf('(');
                int end = line.LastIndexOf('/');
                base._progress = int.Parse(line.Substring(num + 1, end-num-1));
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


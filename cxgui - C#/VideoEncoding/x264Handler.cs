namespace Cxgui.VideoEncoding
{
    using Cxgui;
    using External;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    [Serializable]
    public class x264Handler : VideoEncoderHandler
    {
        protected x264Config _config;
        protected bool errOccured;
        protected DateTime startTime;

        public x264Handler(string avisynthScriptFile, string destinationFile) : base(avisynthScriptFile, destinationFile)
        {
            this.errOccured = false;
            base.encodingProcess.StartInfo.FileName = "x264.exe";
            base.encodingProcess.StartInfo.RedirectStandardError = true;
            base.encodingProcess.StartInfo.CreateNoWindow = false;
        }

        private void ReadStdErr()
        {
            StreamReader standardError = base.encodingProcess.StandardError;
            string line = string.Empty;
            while (true)
            {
                line = standardError.ReadLine();
                if (line == null)
                {
                    line = string.Empty;
                }
                if (line.StartsWith("["))
                {
                    this.UpdateProgress(line);
                }
                else if (line.Contains("error"))
                {
                    this.errOccured = true;
                }
            }
        }

        public override void Start()
        {
            if (this._config.UsingCustomCmd)
            {
                this.Start(1);
                return;
            }
            this.Start(1);
            if (this._config.TotalPass > 1)
            {
                this.Start(2);
            }
            if (this._config.TotalPass == 3)
            {
                this.Start(3);
            }
            base.processingDone = true;
        }

        private void Start(int currentPass)
        {
            string outputFile;
            this._config.CurrentPass = currentPass;
            if (this._config.CurrentPass < this._config.TotalPass)
            {
                outputFile = "NUL";
            }
            else
            {
                outputFile = "\""+base._destinationFile+"\"";
            }
            base.encodingProcess.StartInfo.Arguments = new StringBuilder().Append(this._config.GetArgument()).Append(" --output ").Append(outputFile).Append(" \"").Append(base._avisynthScriptFile).Append("\"").ToString();
            File.WriteAllText("c:\\t.txt", base.encodingProcess.StartInfo.FileName+" "+base.encodingProcess.StartInfo.Arguments);
            Thread thread = new Thread(new ThreadStart(this.ReadStdErr));
            this.startTime = DateTime.Now;
            base.encodingProcess.Start();
            thread.Start();
            base.encodingProcess.WaitForExit();
            if (this.errOccured)
            {
                if (this._config.UsingCustomCmd)
                {
                    throw new BadEncoderCmdException("Encoding failed due to bad custom command line.");
                }
                else
                {
                    throw new AviSynthException(base._avisynthScriptFile);
                }
            }
            if (base._progress >= 99)
            {
                base._progress = 100;
            }
            base.timeLeft = new TimeSpan((long) 0);
            thread.Abort();
        }

        private void UpdateProgress(string line)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = line.Split(separator);
            string s = strArray[0];
            base._currentFrame = int.Parse(s.Substring(s.IndexOf("]")+1, s.IndexOf("/")-s.IndexOf("]")-1));
            base._progress = (int)double.Parse(s.Substring(s.IndexOf("[") + 1, s.IndexOf("%") - s.IndexOf("[")-1));
            string str2 = strArray[2];
            base._currentPosition = (int) (((double) base._currentFrame) / base._scriptFrameRate);
            double.TryParse(str2.Substring(0, str2.IndexOf("k")), out this._avgBitRate);
            base._currentFileSize = (long)((base._avgBitRate * base._currentPosition) / (8 * 1.024));
            base._estimatedFileSize = (long) ((base._avgBitRate * base._totalLength) / (8 * 1.024));
            string str3 = strArray[1];
            base._processingFrameRate = double.Parse(str3.Substring(0, str3.IndexOf("f")));
            string str4 = strArray[3];
            base.timeLeft = TimeSpan.Parse(str4.Substring(str4.IndexOf("a") + 1));
            base._timeUsed = (TimeSpan) (DateTime.Now - this.startTime);
            base._timeUsed = TimeSpan.FromSeconds((double) ((int) this._timeUsed.TotalSeconds));
        }

        public x264Config Config
        {
            get
            {
                return this._config;
            }
            set
            {
                this._config = value;
            }
        }
    }
}


namespace Cxgui.VideoEncoding
{
    using Cxgui;
    using External;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;
    using System.Windows.Forms;

    [Serializable]
    public class X264Handler : VideoEncoderHandlerBase
    {
        protected x264Config _config;
        protected bool errOccured;
        protected DateTime startTime;

        public X264Handler(string avsFile, string destFile) : base(avsFile, destFile)
        {
            this.Initialize();
        }

        public X264Handler()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            base.encodingProcess.StartInfo.FileName = "x264.exe";
            base.encodingProcess.StartInfo.RedirectStandardError = true;
            base.encodingProcess.ErrorDataReceived += this.UpdateProgress;
            base.encodingProcess.Exited += this.encodingProcess_Exited;
        }

        public override void Start()
        {
            base._hasExisted = false;
            this.errOccured = false;
            this.Start(1);
        }

        private void Start(int currentPass)
        {
            // MARK 应当检查是否正确设置了AvsFile DestFile和Config属性。
            string outputFile;
            this._config.CurrentPass = currentPass;
            if (this._config.CurrentPass < this._config.TotalPass)
            {
                outputFile = "NUL";
            }
            else
            {
                outputFile = "\"" + base._destFile + "\"";
            }
            base.encodingProcess.StartInfo.Arguments = this._config.GetArgument() + " --output " + outputFile + " \"" + base._avsFile + "\"";
            this.startTime = DateTime.Now;
            base.encodingProcess.Start();
            base.encodingProcess.BeginErrorReadLine();
        }

  

        private void UpdateProgress(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;
            if (e.Data.StartsWith("["))
            {
                string[] strArray = e.Data.Split(new char[] { ',' });
                string s = strArray[0];
                base._currentFrame = int.Parse(s.Substring(s.IndexOf("]") + 1, s.IndexOf("/") - s.IndexOf("]") - 1));
                base._progress = (int)double.Parse(s.Substring(s.IndexOf("[") + 1, s.IndexOf("%") - s.IndexOf("[") - 1));
                string str2 = strArray[2];
                base._currentPosition = (int)(((double)base._currentFrame) / base._scriptFrameRate);
                double.TryParse(str2.Substring(0, str2.IndexOf("k")), out this._avgBitRate);
                base._currentFileSize = (long)((base._avgBitRate * base._currentPosition) / (8 * 1.024));
                base._estimatedFileSize = (long)((base._avgBitRate * base._totalLength) / (8 * 1.024));
                string str3 = strArray[1];
                base._processingFrameRate = double.Parse(str3.Substring(0, str3.IndexOf("f")));
                string str4 = strArray[3];
                base.timeLeft = TimeSpan.Parse(str4.Substring(str4.IndexOf("a") + 1));
                base._timeUsed = (TimeSpan)(DateTime.Now - this.startTime);
                base._timeUsed = TimeSpan.FromSeconds((double)((int)this._timeUsed.TotalSeconds));
            }
            else if (e.Data.Contains("error"))
            {
                this.errOccured = true;
            }
        }

        private void encodingProcess_Exited(object sender, EventArgs e)
        {
            base._hasExisted = true;
            base.timeLeft = TimeSpan.Zero;
            if (base._progress == 99)
                base._progress = 100;
            if (this.errOccured)
            {
                if (this._config.UsingCustomCmd)
                    throw new BadEncoderCmdException("Encoding failed due to bad custom command line.");
                else
                    throw new AviSynthException(base._avsFile);
            }
            if (!this._config.UsingCustomCmd && this._config.CurrentPass < this._config.TotalPass)
                this.Start(this._config.CurrentPass + 1);
        }

        public x264Config Config
        { get { return this._config; } set { this._config = value; } }
    }
}


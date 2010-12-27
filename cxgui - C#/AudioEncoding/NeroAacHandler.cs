namespace Cxgui.AudioEncoding
{
    using Cxgui.External;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.Threading;

    [Serializable]
    public class NeroAacHandler : AudioEncoderHandlerBase
    {
        protected NeroAacConfig _config;
        protected DateTime startTime;

        public NeroAacHandler(string avsFile, string destFile)
            : base(avsFile, destFile)
        {
            this.Initialize();
        }

        public NeroAacHandler()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.encodingProcess.StartInfo.FileName = "neroAacEnc.exe";
            this.encodingProcess.Exited += this.encodingProcess_Existed;
        }

        private void encodingProcess_Existed(object sender, EventArgs e)
        {
            base._hasExisted = true;
            base._timeLeft = TimeSpan.Zero;
            this.encodingProcess.Dispose();
        }

        public override void Start()
        {
            base._hasExisted = false;
            base.encodingProcess.StartInfo.Arguments = new StringBuilder().Append(this._config.GetSettings()).Append(" -if - -of \"").Append(base._destFile).Append("\"").ToString();
            base.encodingProcess.Start();
            new Thread(
                   delegate()
                   {
                       Stream baseStream = base.encodingProcess.StandardInput.BaseStream;
                       WriteWavHeader.Write(baseStream, base.scriptInfo);
                       int currentSample = 0;
                       int num2 = (0x1000 * base.scriptInfo.ChannelsCount) * base.scriptInfo.BytesPerSample;
                       byte[] buffer = new byte[num2];
                       GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                       IntPtr addr = handle.AddrOfPinnedObject();
                       this.startTime = DateTime.Now;


                       using (base.scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(base._avsFile))
                       {
                           try
                           {
                               while (true)
                               {
                                   int count = Math.Min(((int)base.scriptInfo.SamplesCount) - currentSample, 0x1000);
                                   base.scriptInfo.ReadAudio(addr, (long)currentSample, count);
                                   baseStream.Write(buffer, 0, (count * base.scriptInfo.ChannelsCount) * base.scriptInfo.BytesPerSample);
                                   baseStream.Flush();
                                   currentSample += count;
                                   this.UpdateProgress(currentSample);
                                   if (currentSample == base.scriptInfo.SamplesCount)
                                       break;
                               }
                               baseStream.Write(buffer, 0, 10);
                           }
                           catch (Exception)
                           {
                           }
                           finally
                           {
                               handle.Free();
                               baseStream.Flush();
                               baseStream.Close();
                           }
                       }
                   }
            ).Start();
        }

        private void UpdateProgress(int currentSample)
        {
            base._currentPosition = currentSample / base.scriptInfo.AudioSampleRate;
            base._currentFileSize = (long)((double)currentSample * base.scriptInfo.BytesPerSample / 1024);
            base._progress = (((double) currentSample) / ((double) base.scriptInfo.SamplesCount)) * 100;
            if (base._progress != 0)
            {
                base._estimatedFileSize = (long) ((((double) base._currentFileSize) / base._progress) * 100);
            }
            base._timeUsed = (TimeSpan) (DateTime.Now - this.startTime);
            if (base._progress != 0)
            {
                base._timeLeft = TimeSpan.FromSeconds((double) ((int) ((this._timeUsed.TotalSeconds * (100.0 - base._progress)) / base._progress)));
            }
            base._timeUsed = TimeSpan.FromSeconds((double) ((int) this._timeUsed.TotalSeconds));
        }

        public NeroAacConfig Config
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


namespace CXGUI.AudioEncoding
{
    using CXGUI.External;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    [Serializable]
    public class NeroAacHandler : AudioEncoderHandler
    {
        protected NeroAacConfig _config;
        protected DateTime _startTime;

        public NeroAacHandler(string avisynthScriptFile, string destFile) : base("neroAacEnc.exe", avisynthScriptFile, destFile)
        {
        }

        public override void Start()
        {
            base.encodingProcess.StartInfo.Arguments = new StringBuilder().Append(this._config.GetSettings()).Append(" -if - -of \"").Append(base.destFile).Append("\"").ToString();
            base.encodingProcess.Start();
            Stream baseStream = base.encodingProcess.StandardInput.BaseStream;
            WriteWavHeader.Write(baseStream, base.scriptInfo);
            int currentSample = 0;
            int num2 = (0x1000 * base.scriptInfo.ChannelsCount) * base.scriptInfo.BytesPerSample;
            byte[] buffer = new byte[num2];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr addr = handle.AddrOfPinnedObject();
            this._startTime = DateTime.Now;
            try
            {
                IDisposable disposable = (base.scriptInfo = new AviSynthScriptEnvironment().OpenScriptFile(base.avisynthScriptFile)) as IDisposable;
                try
                {
                    while (1 != 0)
                    {
                        int count = Math.Min(((int) base.scriptInfo.SamplesCount) - currentSample, 0x1000);
                        base.scriptInfo.ReadAudio(addr, (long) currentSample, count);
                        baseStream.Write(buffer, 0, (count * base.scriptInfo.ChannelsCount) * base.scriptInfo.BytesPerSample);
                        baseStream.Flush();
                        currentSample += count;
                        this.UpdateProgress(currentSample);
                        if (currentSample == base.scriptInfo.SamplesCount)
                        {
                            break;
                        }
                    }
                    baseStream.Write(buffer, 0, 10);
                }
                finally
                {
                    if (disposable != null)
                    {
                        disposable.Dispose();
                        disposable = null;
                    }
                }
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
            base.encodingProcess.WaitForExit();
            base.processingDone = true;
        }

        public override void Stop()
        {
            try
            {
                base.encodingProcess.Kill();
                base.encodingProcess.WaitForExit();
            }
            catch (Exception)
            {
            }
        }

        private void UpdateProgress(int currentSample)
        {
            base.currentPosition = currentSample / base.scriptInfo.AudioSampleRate;
            base.currentFileSize = currentSample * base.scriptInfo.BytesPerSample;
            base.progress = (((double) currentSample) / ((double) base.scriptInfo.SamplesCount)) * 100;
            if (base.progress != 0)
            {
                base.estimatedFileSize = (long) ((((double) base.currentFileSize) / base.progress) * 100);
            }
            base.timeUsed = (TimeSpan) (DateTime.Now - this._startTime);
            if (base.progress != 0)
            {
                base.timeLeft = TimeSpan.FromSeconds((double) ((int) ((this.timeUsed.TotalSeconds * (100.0 - base.progress)) / base.progress)));
            }
            base.timeUsed = TimeSpan.FromSeconds((double) ((int) this.timeUsed.TotalSeconds));
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


namespace Cxgui
{
    using Cxgui.External;
    using System;
    using System.IO;
    using System.Collections.Generic;

    [Serializable]
    public class AudioInfo
    {
        protected int _currentStream;
        protected string _filePath;
        protected string _format;
        protected bool _hasVideo;
        protected int _id;
        protected double _length;
        protected int _streamID;
        protected int _streamsCount;

        public AudioInfo(string path)
        {
            this.InitializeProperties(path, 0);
        }

        private void AvisynthInfo(string path)
        {
            AviSynthClip clip;
            IDisposable disposable = null;
            try
            {
                disposable = (clip = new AviSynthScriptEnvironment().OpenScriptFile(path)) as IDisposable;
            }
            catch (Exception)
            { 
                return;
            }
            finally
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                    disposable = null;
                }
            }
            this._filePath = path;
            if (clip.ChannelsCount > 0)
            {
                this._currentStream = 0;
                this._streamsCount = 1;
                this._format = "avs";
                this._streamID = 0;
                this._id = 0;
                this._length = ((double) clip.SamplesCount) / ((double) clip.AudioSampleRate);
                this._hasVideo = clip.HasVideo;
            }
        }

        public static string GetAudioInfo(string path, int streamNumber, string audioParameter)
        {
            MediaInfo info = new MediaInfo();
            info.Open(path);
            string str = info.Get(StreamKind.Audio, streamNumber, audioParameter, InfoKind.Text);
            info.Close();
            return str;
        }

        public static Dictionary<string, string> GetAudioInfo(string path, int streamNumber, params string[] audioParameters)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            string[] strArray = audioParameters;
            foreach (string parameter in audioParameters)
            {
                string value = GetAudioInfo(path, streamNumber, parameter);
                hash.Add(parameter, value);
            }
            return hash;
        }

        private void InitializeProperties(string path, int streamNum)
        {
            if (Path.GetExtension(path).ToLower() == ".avs")
            {
                this.AvisynthInfo(path);
            }
            else
            {
                MediaInfo info = new MediaInfo();
                info.Open(path);
                this._filePath = path;
                if (info.Count_Get(StreamKind.Audio) > 0)
                {
                    this._format = info.Get(StreamKind.Audio, streamNum, "Format");
                    string str = info.Get(StreamKind.Audio, 0, "ID");
                    string str2 = info.Get(StreamKind.Video, 0, "ID");
                    if ((str == "0") || (str2 == "0"))
                    {
                        this._id = 0;
                    }
                    else if ((str == "1") || (str2 == "1"))
                    {
                        this._id = 1;
                    }
                    int.TryParse(info.Get(StreamKind.Audio, streamNum, "ID"), out this._streamID);
                    if (this._streamID > 0)
                    {
                        this._streamID -= this._id;
                    }
                    double.TryParse(info.Get(StreamKind.Audio, streamNum, "Duration"), out this._length);
                    this._length /= (double) 0x3e8;
                    this._streamsCount = info.Count_Get(StreamKind.Audio);
                    if (info.Count_Get(StreamKind.Video) != 0)
                    {
                        this._hasVideo = true;
                    }
                }
                info.Close();
            }
        }

        public int CurrentStream
        {
            get
            {
                return this._currentStream;
            }
            set
            {
                this.InitializeProperties(this._filePath, value);
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
        }

        public string Format
        {
            get
            {
                return this._format;
            }
        }

        public bool HasVideo
        {
            get
            {
                return this._hasVideo;
            }
        }

        public int ID
        {
            get
            {
                return this._id;
            }
        }

        public double Length
        {
            get
            {
                return this._length;
            }
        }

        public int StreamID
        {
            get
            {
                return this._streamID;
            }
        }

        public int StreamsCount
        {
            get
            {
                return this._streamsCount;
            }
        }
    }
}


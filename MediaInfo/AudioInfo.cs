namespace Cxgui
{
    using Cxgui.External;
    using System;
    using System.IO;
    using System.Collections.Generic;

    [Serializable]
    public class AudioInfo
    {
        private int _currentStream;
        private string _filePath;
        private string _format;
        private bool _hasVideo;
        private int _trackId;
        private double _length;
        private int _streamId;
        private int _streamsCount;

        public AudioInfo(string path)
        {
            this.InitializeProperties(path, 0);
        }

        private void AvisynthInfo(string path){
            AviSynthClip clip;
            try
            {
                using (clip = new AviSynthScriptEnvironment().OpenScriptFile(path)) { }
            }
            catch
            { 
                return;
            }
            this._filePath = path;
            if (clip.ChannelsCount > 0)
            {
                this._currentStream = 0;
                this._streamsCount = 1;
                this._format = "avs";
                this._streamId = 0;
                this._trackId = 0;
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
                    int.TryParse(info.Get(StreamKind.Audio, streamNum, "ID"), out this._trackId);
                    if ((str == "0") || (str2 == "0"))
                    {
                        this._streamId = this._trackId;
                    }
                    else if ((str == "1") || (str2 == "1"))
                    {
                        this._streamId = this._trackId - 1;
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

        public int TrackId
        {
            get
            {
                return this._trackId;
            }
        }

        public double Length
        {
            get
            {
                return this._length;
            }
        }

        public int StreamId
        {
            get
            {
                return this._streamId;
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


namespace Cxgui
{
    using Cxgui.External;
    using System;
    using System.IO;
    using System.Collections.Generic;

    [Serializable]
    public class VideoInfo
    {
        protected int _audioStreamsCount;
        protected double _displayAspectRatio;
        protected string _filePath;
        protected string _format;
        protected int _frameCount;
        protected double _frameRate;
        protected bool _hasVideo;
        protected int _height;
        protected int _id;
        protected double _length;
        protected int _streamID;
        protected int _width;
        protected string _container;

        public VideoInfo(string path)
        {
            this.InitializeProperties(path);
        }

        private void AvisynthInfo(string path)
        {
            AviSynthClip clip;
            IDisposable disposable = null;
            // 如果出错说明不是正确的avs脚本
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
            // 是正确的avs脚本
            this._container = "avs";
            this._hasVideo = clip.HasVideo;
            this._filePath = path;
            if (clip.ChannelsCount != 0)
            {
                this._audioStreamsCount = 1;
            }
            else
            {
                this._audioStreamsCount = 0;
            }
            if (clip.HasVideo)
            {
                this._width = clip.VideoWidth;
                this._height = clip.VideoHeight;
                this._displayAspectRatio = ((double) this._width) / ((double) this._height);
                this._frameRate = Math.Round((double) (((double) clip.raten) / ((double) clip.rated)), 3, MidpointRounding.AwayFromZero);
                this._frameCount = clip.num_frames;
                this._streamID = 0;
                this._length = ((double) clip.num_frames) / this._frameRate;
                this._id = 0;
                this._format = "avs";
            }
            else
            {
                this._hasVideo = false;
            }
        }

        public static Dictionary<string, string> GetVideoInfo(string path, params string[] videoParameters)
        {
            Dictionary<string, string> hash = new Dictionary<string, string>();
            foreach (string videoParameter in videoParameters)
            {
                string videoInfo = GetVideoInfo(path, videoParameter);
                hash.Add(videoParameter, videoInfo);
            }
            return hash;
        }

        public static string GetVideoInfo(string path, string videoParameter)
        {
            MediaInfo info = new MediaInfo();
            info.Open(path);
            string str = info.Get(StreamKind.Video, 0, videoParameter, InfoKind.Text);
            info.Close();
            return str;
        }

        private void InitializeProperties(string path)
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
                if (info.Count_Get(StreamKind.Video) > 0)
                {
                    this._hasVideo = true;
                    this._format = info.Get(StreamKind.Video, 0, "Format");
                    this._container = info.Get(StreamKind.General, 0, "Format");
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
                    double.TryParse(info.Get(StreamKind.Video, 0, "Duration"), out this._length);
                    this._length /= (double) 0x3e8;
                    int.TryParse(info.Get(StreamKind.Video, 0, "ID", InfoKind.Text), out this._streamID);
                    if (this._streamID > 0)
                    {
                        this._streamID -= this._id;
                    }
                    int.TryParse(info.Get(StreamKind.Video, 0, "Width", InfoKind.Text), out this._width);
                    int.TryParse(info.Get(StreamKind.Video, 0, "Height", InfoKind.Text), out this._height);
                    double.TryParse(info.Get(StreamKind.Video, 0, "FrameRate", InfoKind.Text), out this._frameRate);
                    int.TryParse(info.Get(StreamKind.Video, 0, "FrameCount", InfoKind.Text), out this._frameCount);
                    string str3 = info.Get(StreamKind.Video, 0, "DisplayAspectRatio/String", InfoKind.Text);
                    if (str3.IndexOf(":") != -1)
                    {
                        char[] separator = new char[] { ':' };
                        char[] chArray2 = new char[] { ':' };
                        this._displayAspectRatio = double.Parse(str3.Split(separator)[0]) / double.Parse(str3.Split(chArray2)[1]);
                    }
                    if ((this._width != 0) && (this._displayAspectRatio == 0))
                    {
                        this._displayAspectRatio = ((double) this._width) / ((double) this._height);
                    }
                }
                else
                {
                    this._hasVideo = false;
                }
                this._audioStreamsCount = info.Count_Get(StreamKind.Audio);
                info.Close();
            }
        }

        public int AudioStreamsCount
        {
            get
            {
                return this._audioStreamsCount;
            }
        }

        public double DisplayAspectRatio
        {
            get
            {
                return this._displayAspectRatio;
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
        }

        /// <summary>
        /// 视频格式，对于包含视频流的avs脚本是"avs"。如不含视频流，为空
        /// </summary>
        public string Format
        {
            get
            {
                return this._format;
            }
        }

        public int FrameCount
        {
            get
            {
                return this._frameCount;
            }
        }

        public double FrameRate
        {
            get
            {
                return this._frameRate;
            }
        }

        public bool HasVideo
        {
            get
            {
                return this._hasVideo;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
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

        public int Width
        {
            get
            {
                return this._width;
            }
        }

        /// <summary>
        /// 视频容器，对于有效的avs脚本是"avs"
        /// </summary>
        public string Container
        {
            get
            {
                return this._container;

            }
        }
    }
}


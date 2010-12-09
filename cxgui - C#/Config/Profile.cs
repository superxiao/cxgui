namespace Cxgui.Config
{
    using Cxgui;
    using Cxgui.AudioEncoding;
    using Cxgui.Avisynth;
    using Cxgui.Job;
    using Cxgui.VideoEncoding;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class Profile
    {
        protected NeroAacConfig _audioEncConfig;
        protected AvisynthConfig _avsConfig;
        protected JobItemConfig _jobConfig;
        protected static readonly string _profileDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "profile");
        protected Cxgui.Avisynth.SubtitleConfig _subtitleConfig;
        protected x264Config _videoEncConfig;

        static Profile()
        {
            if (!Directory.Exists(_profileDir))
            {
                Directory.CreateDirectory(_profileDir);
            }
        }

        public Profile(bool initializeConfig)
        {
            if (initializeConfig)
            {
                this._avsConfig = new AvisynthConfig();
                this._videoEncConfig = new x264Config();
                this._audioEncConfig = new NeroAacConfig();
                this._jobConfig = new JobItemConfig();
                this._subtitleConfig = new Cxgui.Avisynth.SubtitleConfig();
            }
        }

        public Profile(string profileName)
        {
            FileStream stream = null;
            Profile profile;
            string path = Path.Combine(_profileDir, profileName + ".profile");
            BinaryFormatter formatter = new BinaryFormatter();
            if (!File.Exists(path))
            {
                throw new ProfileNotFoundException("profile文件未找到");
            }
            try
            {
                stream = new FileStream(path, FileMode.Open);
                profile = formatter.Deserialize(stream) as Profile;
                stream.Close();
            }
            catch (Exception)
            {
                stream.Close();
                throw new ProfileNotFoundException("profile文件损坏");
            }
            this._avsConfig = profile._avsConfig;
            this._videoEncConfig = profile._videoEncConfig;
            this._audioEncConfig = profile._audioEncConfig;
            this._jobConfig = profile._jobConfig;
            this._subtitleConfig = profile._subtitleConfig;
        }

        public static string[] GetExistingProfilesNamesOnHardDisk()
        {
            List<string> list = new List<string>();
            string[] files = Directory.GetFiles(_profileDir, "*.profile");
            BinaryFormatter formatter = new BinaryFormatter();
            int index = 0;
            string[] strArray2 = files;
            int length = strArray2.Length;
            while (index < length)
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(strArray2[index], FileMode.Open);
                    Profile profile = formatter.Deserialize(stream) as Profile;
                    list.Add(Path.GetFileNameWithoutExtension(strArray2[index]));
                    stream.Close();
                }
                catch (Exception)
                {
                    stream.Close();
                }
                index++;
            }
            return list.ToArray();
        }

        public string GetExtByContainer()
        {
            string str = "";
            if (this.JobConfig.Container == OutputContainer.MP4)
            {
                return ".mp4";
            }
            if (this.JobConfig.Container == OutputContainer.MKV)
            {
                str = ".mkv";
            }
            return str;
        }

        public static void RebuildDefault(string defaultProfileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(_profileDir, defaultProfileName + ".profile");
            Profile graph = new Profile(true);
            FileStream serializationStream = new FileStream(path, FileMode.Create);
            formatter.Serialize(serializationStream, graph);
            serializationStream.Close();
        }

        public static void Save(string profileName, JobItemConfig jobConfig, AvisynthConfig avsConfig, VideoEncConfigBase videoEncConfig, AudioEncConfigBase audioEncConfig, Cxgui.Avisynth.SubtitleConfig subtitleConfig)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(_profileDir, profileName + ".profile");
            Profile graph = new Profile(false);
            graph._jobConfig = jobConfig;
            graph._videoEncConfig = (x264Config) videoEncConfig;
            graph._audioEncConfig = (NeroAacConfig) audioEncConfig;
            graph._avsConfig = avsConfig;
            graph._subtitleConfig = subtitleConfig;
            FileStream serializationStream = new FileStream(path, FileMode.Create);
            formatter.Serialize(serializationStream, graph);
            serializationStream.Close();
        }

        public static void DeleteProfile(string profileName)
        {
            File.Delete(Path.Combine(Profile._profileDir, profileName+".profile"));
        }
        public NeroAacConfig AudioEncConfig
        {
            get
            {
                return this._audioEncConfig;
            }
            set
            {
                this._audioEncConfig = value;
            }
        }

        public AvisynthConfig AvsConfig
        {
            get
            {
                return this._avsConfig;
            }
            set
            {
                this._avsConfig = value;
            }
        }

        public JobItemConfig JobConfig
        {
            get
            {
                return this._jobConfig;
            }
            set
            {
                this._jobConfig = value;
            }
        }

        public Cxgui.Avisynth.SubtitleConfig SubtitleConfig
        {
            get
            {
                return this._subtitleConfig;
            }
            set
            {
                this._subtitleConfig = value;
            }
        }

        public x264Config VideoEncConfig
        {
            get
            {
                return this._videoEncConfig;
            }
            set
            {
                this._videoEncConfig = value;
            }
        }
    }
}


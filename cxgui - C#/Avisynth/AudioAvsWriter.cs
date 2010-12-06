namespace CXGUI.Avisynth
{
    using CXGUI;
    using CXGUI.External;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [Serializable]
    public class AudioAvsWriter
    {
        protected AudioInfo _audioInfo;
        protected AvisynthConfig _avsConfig;
        protected bool _downMix;
        protected OrderedDictionary<string, string> _filters;
        protected List<string> _loadingsAndImportings;
        protected bool _normalize;
        protected AudioSourceFilter _sourceFilter;

        public AudioAvsWriter(string sourceFile, AvisynthConfig avsConfig, AudioInfo audioInfo)
        {
            this._avsConfig = avsConfig;
            this._filters = new OrderedDictionary<string, string>();
            this._loadingsAndImportings = new List<string>();
            this._audioInfo = audioInfo;
            if (this._audioInfo.Format == "avs")
            {
                this.AvsInputInitialize(sourceFile);
            }
            else
            {
                this.SourceFilter = this._avsConfig.AudioSourceFilter;
                this.DownMix = this._avsConfig.DownMix;
                this.Normalize = this._avsConfig.Normalize;
            }
        }

        private void AvsInputInitialize(string sourceFile)
        {
            string[] externalFilters = new string[] { sourceFile };
            this.SetImport(externalFilters);
            this.SetFilter("KillVideo", "KillVideo()");
        }

        public bool ContainsFilter(string filterName)
        {
            return this._filters.ContainsKey(filterName);
        }

        public string GetFilterStatement(string filterName)
        {
            return this._filters[filterName];
        }

        public string GetScriptContent()
        {
            string str = null;
            foreach (string str2 in this._loadingsAndImportings)
            {
                str += new StringBuilder().Append(str2).Append("\r\n").ToString();
            }
            foreach (KeyValuePair<string, string> filter in this._filters)
            {
                str += new StringBuilder().Append(filter.Value).Append("\r\n").ToString();
            }
            if (this._audioInfo.Format != "avs")
            {
                str += "audio\r\n";
            }
            return str;
        }

        public void RemoveFilter(string filterName)
        {
            if (this._filters.ContainsKey(filterName))
            {
                this._filters.Remove(filterName);
            }
        }

        private void SetFilter(string filterName, string statement)
        {
            this._filters[filterName] = statement;
        }

        public void SetImport(params string[] externalFilters)
        {
            int index = 0;
            string[] strArray = externalFilters;
            int length = strArray.Length;
            while (index < length)
            {
                switch (Path.GetExtension(strArray[index]).ToLower())
                {
                    case ".dll":
                        string str = new StringBuilder("LoadPlugin(\"").Append(strArray[index]).Append("\")").ToString();
                        if (!this._loadingsAndImportings.Contains(str))
                            this._loadingsAndImportings.Add(str);
                        break;

                    case ".avs":
                    case "avis":
                        str = new StringBuilder("Import(\"").Append(strArray[index]).Append("\")").ToString();
                        if (!this._loadingsAndImportings.Contains(str))
                            this._loadingsAndImportings.Add(str);
                        break;
                }
                index++;
            }
        }

        public void WriteScript(string avsDestFile)
        {
            File.WriteAllText(avsDestFile, this.GetScriptContent(), Encoding.Default);
        }

        public bool DownMix
        {
            get
            {
                return this._downMix;
            }
            set
            {
                this._downMix = value;
                if (value)
                {
                    string[] externalFilters = new string[] { Path.GetFullPath("Downmix.avs") };
                    this.SetImport(externalFilters);
                    this.SetFilter("DownMix", "audio=(audio.AudioChannels>2) ? DownMix(audio, audio.AudioChannels) : audio");
                }
                else
                {
                    this.RemoveFilter("DownMix");
                }
            }
        }

        public bool Normalize
        {
            get
            {
                return this._normalize;
            }
            set
            {
                this._normalize = value;
                if (value)
                {
                    this.SetFilter("Normalize", "audio.Normalize()");
                }
                else
                {
                    this.RemoveFilter("Normalize");
                }
            }
        }

        public AudioSourceFilter SourceFilter
        {
            get
            {
                return this._sourceFilter;
            }
            set
            {
                this._sourceFilter = value;
                if (this._sourceFilter == AudioSourceFilter.DirectShowSource)
                {
                    this.SetFilter("SourceFilter", new StringBuilder("audio=DirectShowSource(\"").Append(this._audioInfo.FilePath).Append("\", video=false)").ToString());
                }
                else if (this._sourceFilter == AudioSourceFilter.FFAudioSource)
                {
                    string[] externalFilters = new string[] { Path.GetFullPath("ffms2.dll") };
                    this.SetImport(externalFilters);
                    this.SetFilter("SourceFilter", new StringBuilder("audio=FFAudioSource(\"").Append(this._audioInfo.FilePath).Append("\", track=-1)").ToString());
                }
                else if (this._sourceFilter == AudioSourceFilter.None)
                {
                    this.RemoveFilter("SourceFilter");
                }
            }
        }
    }
}


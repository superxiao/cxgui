namespace Cxgui.Avisynth
{
    using Cxgui;
    using Cxgui.External;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [Serializable]
    public class AudioAvsWriter
    {
        protected AudioInfo audioInfo;
        protected AvisynthConfig avsConfig;
        protected bool _downMix;
        protected OrderedDictionary<string, string> filters;
        protected List<string> loadingsAndImportings;
        protected bool _normalize;
        protected AudioSourceFilter _sourceFilter;

        public AudioAvsWriter(string sourceFile, AvisynthConfig avsConfig, AudioInfo audioInfo)
        {
            this.avsConfig = avsConfig;
            this.filters = new OrderedDictionary<string, string>();
            this.loadingsAndImportings = new List<string>();
            this.audioInfo = audioInfo;
            if (this.audioInfo.Format == "avs")
            {
                this.AvsInputInitialize(sourceFile);
            }
            else
            {
                this.SourceFilter = this.avsConfig.AudioSourceFilter;
                this.DownMix = this.avsConfig.DownMix;
                this.Normalize = this.avsConfig.Normalize;
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
            return this.filters.ContainsKey(filterName);
        }

        public string GetFilterStatement(string filterName)
        {
            return this.filters[filterName];
        }

        /// <summary>
        /// 获取音频avs脚本内容。
        /// </summary>
        /// <returns>音频avs脚本内容，当初始化时的avsConfig的CustomAudioScript非null时，为该属性。否则由avsConfig其他设置决定。</returns>
        public string GetScriptContent(bool useCustomScript)
        {
            string content = null;
            if (this.avsConfig.PaddingCustomAudioScript)
            {
                foreach (string loadingAndImporting in this.loadingsAndImportings)
                {
                    content += new StringBuilder().Append(loadingAndImporting).Append("\r\n").ToString();
                }
                foreach (KeyValuePair<string, string> filter in this.filters)
                {
                    content += new StringBuilder().Append(filter.Value).Append("\r\n").ToString();
                }
                if (this.audioInfo.Format != "avs")
                {
                    content += "audio\r\n";
                }
            }
            if (this.audioInfo.Format != "avs" && useCustomScript)
            {
                content += this.avsConfig.CustomAudioScript;
            }
            return content;
        }

        public void RemoveFilter(string filterName)
        {
            if (this.filters.ContainsKey(filterName))
            {
                this.filters.Remove(filterName);
            }
        }

        private void SetFilter(string filterName, string statement)
        {
            this.filters[filterName] = statement;
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
                        if (!this.loadingsAndImportings.Contains(str))
                            this.loadingsAndImportings.Add(str);
                        break;

                    case ".avs":
                    case "avis":
                        str = new StringBuilder("Import(\"").Append(strArray[index]).Append("\")").ToString();
                        if (!this.loadingsAndImportings.Contains(str))
                            this.loadingsAndImportings.Add(str);
                        break;
                }
                index++;
            }
        }

        public void WriteScript(string avsDestFile)
        {
            File.WriteAllText(avsDestFile, this.GetScriptContent(true), Encoding.Default);
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
                    this.SetFilter("SourceFilter", new StringBuilder("audio=DirectShowSource(\"").Append(this.audioInfo.FilePath).Append("\", video=false)").ToString());
                }
                else if (this._sourceFilter == AudioSourceFilter.FFAudioSource)
                {
                    string[] externalFilters = new string[] { Path.GetFullPath("ffms2.dll") };
                    this.SetImport(externalFilters);
                    this.SetFilter("SourceFilter", new StringBuilder("audio=FFAudioSource(\"").Append(this.audioInfo.FilePath).Append("\", track=-1)").ToString());
                }
                else if (this._sourceFilter == AudioSourceFilter.None)
                {
                    this.RemoveFilter("SourceFilter");
                }
            }
        }
    }
}


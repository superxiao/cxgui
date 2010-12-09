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
    public class VideoAvsWriter
    {
        protected AvisynthConfig _avsConfig;
        protected OrderedDictionary<string, string> _filters = new OrderedDictionary<string, string>();
        protected List<string> _loadingsAndImportings = new List<string>();
        protected VideoInfo _videoInfo;

        public VideoAvsWriter(string sourceFile, AvisynthConfig avsConfig, string subtitleFile, VideoInfo videoInfo)
        {
            this._avsConfig = avsConfig;
            this._videoInfo = videoInfo;
            if (this._videoInfo.Container == "avs")
            {
                this.AvsInputInitialize(sourceFile);
            }
            else
            {
                this.SetSourceFilter(this._avsConfig.VideoSourceFilter);
                if (!this._avsConfig.UsingSourceFrameRate)
                {
                    this.SetFrameRate(this._avsConfig.FrameRate, this._avsConfig.ConvertFPS);
                }
                this.SetFilter("ConvertToYV12", "IsYV12 ? Last : ConvertToYV12()");
                if (!this._avsConfig.UsingSourceResolution && ((this._avsConfig.Width != this._videoInfo.Width) || (this._avsConfig.Height != this._videoInfo.Height)))
                {
                    if ((this._avsConfig.Width <= 0x3e8) && (this._videoInfo.Width >= 0x500))
                    {
                        string[] externalFilters = new string[] { Path.GetFullPath("ColorMatrix.dll") };
                        this.SetImport(externalFilters);
                        this.SetFilter("ColorMatrix", "ColorMatrix()");
                    }
                    this.SetFilter("Resizer", new StringBuilder().Append(this._avsConfig.Resizer).Append("(").Append(this._avsConfig.Width).Append(",").Append(this._avsConfig.Height).Append(")").ToString());
                }
                if (File.Exists(subtitleFile))
                {
                    string[] textArray2 = new string[] { Path.GetFullPath("VSFilter.dll") };
                    this.SetImport(textArray2);
                    this.SetFilter("TextSub", new StringBuilder("TextSub(\"").Append(subtitleFile).Append("\")").ToString());
                }
            }
        }

        private void AvsInputInitialize(string sourceFile)
        {
            string[] externalFilters = new string[] { sourceFile };
            this.SetImport(externalFilters);
            this.SetFilter("KillAudio", "KillAudio()");
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
            return str;
        }

        public void RemoveFilter(string filterName)
        {
            if (this._filters.ContainsKey(filterName))
            {
                this._filters.Remove(filterName);
            }
        }

        public void SetFilter(string filterName, string statement)
        {
            this._filters[filterName] = statement;
        }

        public void SetFrameRate(double frameRate, bool convertFPS)
        {
            if (frameRate != this._videoInfo.FrameRate)
            {
                if (convertFPS)
                {
                    this.SetFilter("FPS", new StringBuilder("ConvertFPS(").Append(frameRate).Append(")").ToString());
                }
                else
                {
                    this.SetFilter("FPS", new StringBuilder("AssumeFPS(").Append(frameRate).Append(")").ToString());
                }
            }
            else
            {
                this.RemoveFilter("FPS");
            }
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
                        this._loadingsAndImportings.Add(str);
                        break;
                }
                index++;
            }
        }

        public void SetSourceFilter(VideoSourceFilter sourceFilter)
        {
            if (sourceFilter == VideoSourceFilter.DirectShowSource)
            {
                this.SetFilter("SourceFilter", new StringBuilder("DirectShowSource(\"").Append(this._videoInfo.FilePath).Append("\", audio = false)").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.DSS2)
            {
                string[] externalFilters = new string[] { Path.GetFullPath("avss.dll") };
                this.SetImport(externalFilters);
                this.SetFilter("SourceFilter", new StringBuilder("DSS2(\"").Append(this._videoInfo.FilePath).Append("\")").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.FFVideoSource)
            {
                string[] textArray2 = new string[] { Path.GetFullPath("ffms2.dll") };
                this.SetImport(textArray2);
                this.SetFilter("SourceFilter", new StringBuilder("FFVideoSource(\"").Append(this._videoInfo.FilePath).Append("\")").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.None)
            {
                this.RemoveFilter("SourceFilter");
            }
        }

        public void WriteScript(string avsDestFile)
        {
            File.WriteAllText(avsDestFile, this.GetScriptContent(), Encoding.Default);
        }
    }
}


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
        protected AvisynthConfig avsConfig;
        protected OrderedDictionary<string, string> filters = new OrderedDictionary<string, string>();
        protected List<string> loadingsAndImportings = new List<string>();
        protected VideoInfo videoInfo;

        public VideoAvsWriter(string sourceFile, AvisynthConfig avsConfig, string subtitleFile, VideoInfo videoInfo)
        {
            this.avsConfig = avsConfig;
            this.videoInfo = videoInfo;
            if (this.videoInfo.Container == "avs")
            {
                this.AvsInputInitialize(sourceFile);
            }
            else
            {
                this.SetSourceFilter(this.avsConfig.VideoSourceFilter);
                if (!this.avsConfig.UsingSourceFrameRate)
                {
                    this.SetFrameRate(this.avsConfig.FrameRate, this.avsConfig.ConvertFPS);
                }
                this.SetFilter("ConvertToYV12", "IsYV12 ? Last : ConvertToYV12()");
                if (!this.avsConfig.UsingSourceResolution && ((this.avsConfig.Width != this.videoInfo.Width) || (this.avsConfig.Height != this.videoInfo.Height)))
                {
                    if ((this.avsConfig.Width <= 0x3e8) && (this.videoInfo.Width >= 0x500))
                    {
                        string[] externalFilters = new string[] { Path.GetFullPath("ColorMatrix.dll") };
                        this.SetImport(externalFilters);
                        this.SetFilter("ColorMatrix", "ColorMatrix()");
                    }
                    this.SetFilter("Resizer", new StringBuilder().Append(this.avsConfig.Resizer).Append("(").Append(this.avsConfig.Width).Append(",").Append(this.avsConfig.Height).Append(")").ToString());
                }
                if (File.Exists(subtitleFile))
                {
                    string[] textArray2 = new string[] { Path.GetFullPath("VSFilter.dll") };
                    this.SetImport(textArray2);
                    string filter;
                    switch (Path.GetExtension(subtitleFile).ToLower()) { case ".sub": case ".idx": filter = "VobSub"; break; default: filter = "TextSub"; break; }
                    this.SetFilter(filter, filter + "(\"" + subtitleFile + "\")");
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
            return this.filters.ContainsKey(filterName);
        }

        public string GetFilterStatement(string filterName)
        {
            return this.filters[filterName];
        }
        /// <summary>
        /// 获取视频avs脚本内容。
        /// </summary>
        /// <returns>视频avs脚本内容，当初始化时的avsConfig的CustomVideoScript非null时，为该属性。否则由avsConfig其他设置决定。</returns>
        
        public string GetScriptContent(bool useCustomScript)
        {
            
            string content = null;
            if (this.avsConfig.PaddingCustomVideoScript)
            {
                foreach (string loadingAndImporting in this.loadingsAndImportings)
                {
                    content += new StringBuilder().Append(loadingAndImporting).Append("\r\n").ToString();
                }
                foreach (KeyValuePair<string, string> filter in this.filters)
                {
                    content += new StringBuilder().Append(filter.Value).Append("\r\n").ToString();
                }
            }
            if (this.videoInfo.Format != "avs" && useCustomScript)
                content += this.avsConfig.CustomVideoScript;
            return content;
        }

        public void RemoveFilter(string filterName)
        {
            if (this.filters.ContainsKey(filterName))
            {
                this.filters.Remove(filterName);
            }
        }

        public void SetFilter(string filterName, string statement)
        {
            this.filters[filterName] = statement;
        }

        public void SetFrameRate(double frameRate, bool convertFPS)
        {
            if (frameRate != this.videoInfo.FrameRate)
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
                        if (!this.loadingsAndImportings.Contains(str))
                        this.loadingsAndImportings.Add(str);
                        break;

                    case ".avs":
                    case "avis":
                        str = new StringBuilder("Import(\"").Append(strArray[index]).Append("\")").ToString();
                        this.loadingsAndImportings.Add(str);
                        break;
                }
                index++;
            }
        }

        public void SetSourceFilter(VideoSourceFilter sourceFilter)
        {
            if (sourceFilter == VideoSourceFilter.DirectShowSource)
            {
                this.SetFilter("SourceFilter", new StringBuilder("DirectShowSource(\"").Append(this.videoInfo.FilePath).Append("\", audio = false)").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.DSS2)
            {
                string[] externalFilters = new string[] { Path.GetFullPath("avss.dll") };
                this.SetImport(externalFilters);
                this.SetFilter("SourceFilter", new StringBuilder("DSS2(\"").Append(this.videoInfo.FilePath).Append("\")").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.FFVideoSource)
            {
                string[] textArray2 = new string[] { Path.GetFullPath("ffms2.dll") };
                this.SetImport(textArray2);
                this.SetFilter("SourceFilter", new StringBuilder("FFVideoSource(\"").Append(this.videoInfo.FilePath).Append("\")").ToString());
            }
            else if (sourceFilter == VideoSourceFilter.None)
            {
                this.RemoveFilter("SourceFilter");
            }
        }

        public void WriteScript(string avsDestFile)
        {
            File.WriteAllText(avsDestFile, this.GetScriptContent(true), Encoding.Default);
        }
    }
}


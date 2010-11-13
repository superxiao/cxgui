namespace CXGUI.Avisynth
{
    using Clinky;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class SubStyleWriter
    {
        protected string _subtitle;
        protected SubtitleConfig _subtitleConfig;
        protected List<string> _tempFiles;

        public SubStyleWriter(string subtitle, SubtitleConfig subtitleConfig)
        {
            this._subtitle = subtitle;
            this._subtitleConfig = subtitleConfig;
            this._tempFiles = new List<string>(2);
        }

        public void DeleteTempFiles()
        {
            foreach (string str in this._tempFiles)
            {
                File.Delete(str);
            }
            this._tempFiles.Clear();
        }

        private string GetSrtTimeFromAssTime(string assTime)
        {
            assTime = assTime.Trim();
            return new StringBuilder("0").Append(assTime).Append("0").ToString().Replace(".", ",");
        }

        private string GenerateSrtFromAss(string assFile)
        {
            string uniqueName = MyIO.GetUniqueName(Path.ChangeExtension(assFile, "srt"));
            int index = 0;
            string newLine = Environment.NewLine;
            string contents = string.Empty;
            string[] assLines = File.ReadAllLines(assFile);
            foreach(string assLine in assLines)
            {
                if (assLine.StartsWith("Dialogue:"))
                {
                    string[] assTimeElements = assLine.Split(new char[] { ',' });
                    //UNDONE: 双语字幕对话行中另有字体设定未处理
                    Match match = Regex.Match(assLine, @"[^,]*,([^,]*),([^,]*),[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,(.*)");
                    string assStartTime = match.Captures[0].Value;
                    string assEndTime = match.Captures[1].Value;
                    string dialogue = match.Captures[2].Value;
                    contents += new StringBuilder().Append(newLine).Append(index).Append(newLine).Append(this.GetSrtTimeFromAssTime(assStartTime)).Append(" --> ").Append(this.GetSrtTimeFromAssTime(assEndTime)).Append(newLine).Append(dialogue).Append(newLine).ToString();
                    index++;
                }
            }
            File.WriteAllText(uniqueName, contents, Encoding.UTF8);
            return uniqueName;
        }

        public void Write()
        {
            string[] rhs = new string[] { ".ssa", ".ass" };
            string ext = Path.GetExtension(this._subtitle).ToLower();
            if (ext == ".ssa" || ext == ".ass")
            {
                this._subtitle = this.GenerateSrtFromAss(this._subtitle);
                this._tempFiles.Add(this._subtitle);
            }
            string styleFile = this._subtitle + ".style";
            string styles = this._subtitleConfig.GetStyles();
            string contents = "\r\n[Script Info]\r\nScriptType: v4.00+\r\nCollisions: Normal\r\nTimer: 100.0000\r\n" + styles;
            File.WriteAllText(styleFile, contents, Encoding.UTF8);
            this._tempFiles.Add(styleFile);
        }
    }
}


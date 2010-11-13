namespace CXGUI.Avisynth
{
    using System;
    using System.Text;

    [Serializable]
    public class SubtitleConfig
    {
        public string Fontname = "宋体";
        public int Fontsize = 0x20;
        public int MarginV = 20;
        public bool UsingStyle;

        public string GetStyles()
        {
            return ("\r\n[V4 Styles]\r\n\t\tFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, TertiaryColour, BackColour, Bold, Italic, BorderStyle, Outline, \r\n\t\t \\  Shadow, Alignment, MarginL, MarginR, MarginV, AlphaLevel, Encoding\r\n" + new StringBuilder("\t\tStyle: Default,").Append(this.Fontname).Append(",").Append(this.Fontsize).Append(",&Hffffff,&H00ffff,&H000000,&H000000,-1,0,1,2,3,2,20,20,").Append(this.MarginV).Append(",0,1").ToString());
        }
    }
}


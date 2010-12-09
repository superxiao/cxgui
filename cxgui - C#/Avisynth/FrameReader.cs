namespace Cxgui.Avisynth
{
    using System;

    [Serializable]
    public class FrameReader
    {
        protected string _avsFile;

        public FrameReader(string avsFile)
        {
            this._avsFile = avsFile;
        }
    }
}


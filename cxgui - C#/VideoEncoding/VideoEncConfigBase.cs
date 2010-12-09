namespace Cxgui.VideoEncoding
{
    using System;

    [Serializable]
    public abstract class VideoEncConfigBase
    {
        protected string _customCmdLine = null;
        protected bool _usingCustomCmd;

        public abstract string GetArgument();

        public string CustomCmdLine
        {
            get
            {
                return this._customCmdLine;
            }
            set
            {
                this._customCmdLine = value;
            }
        }

        public bool UsingCustomCmd
        {
            get
            {
                return this._usingCustomCmd;
            }
            set
            {
                this._usingCustomCmd = value;
            }
        }
    }
}


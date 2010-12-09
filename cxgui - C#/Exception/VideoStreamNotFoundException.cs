namespace Cxgui
{
    using System;
    using System.Text;

    [Serializable]
    public class VideoStreamNotFoundException : Exception
    {
        public VideoStreamNotFoundException(string path) : base(new StringBuilder("Video stream not found:\n").Append(path).ToString())
        {
        }
    }
}


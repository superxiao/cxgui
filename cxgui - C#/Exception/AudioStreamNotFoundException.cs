namespace Cxgui
{
    using System;
    using System.Text;

    [Serializable]
    public class AudioStreamNotFoundException : Exception
    {
        public AudioStreamNotFoundException(string path) : base(new StringBuilder("Audio stream not found:\n").Append(path).ToString())
        {
        }
    }
}


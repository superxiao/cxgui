namespace Cxgui
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class AvisynthAudioStreamNotFoundException : Exception
    {
        public AvisynthAudioStreamNotFoundException(string path)
        {
        }
    }
}


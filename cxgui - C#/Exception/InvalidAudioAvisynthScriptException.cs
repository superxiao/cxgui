namespace CXGUI
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class InvalidAudioAvisynthScriptException : Exception
    {
        public InvalidAudioAvisynthScriptException(string path)
        {
        }
    }
}


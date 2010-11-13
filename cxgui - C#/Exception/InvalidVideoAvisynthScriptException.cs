namespace CXGUI
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class InvalidVideoAvisynthScriptException : Exception
    {
        public InvalidVideoAvisynthScriptException(string path)
        {
        }
    }
}


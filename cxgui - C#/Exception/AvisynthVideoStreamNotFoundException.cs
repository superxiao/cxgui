namespace CXGUI
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class AvisynthVideoStreamNotFoundException : Exception
    {
        public AvisynthVideoStreamNotFoundException(string path)
        {
        }
    }
}


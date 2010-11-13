namespace CXGUI
{
    using DirectShowLib.DES;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Serializable]
    public class DirectShowInfo
    {
        public DirectShowInfo(string path)
        {
            int errorCode = ((IMediaDet) new MediaDet()).put_Filename(path);
            try
            {
                Marshal.ThrowExceptionForHR(errorCode);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Empty);
            }
        }
    }
}


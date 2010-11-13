namespace CXGUI.StreamMuxer
{
    using System;
    using System.Runtime.CompilerServices;

    [CompilerGlobalScope]
    public sealed class MP4BoxModule
    {
        private MP4BoxModule()
        {
        }

        public static void test()
        {
            MP4Box box = new MP4Box();
            box.VideoFile = @"c:\12.mp4";
            box.AudioFile = @"c:\test.mp4";
            box.Start();
        }
    }
}


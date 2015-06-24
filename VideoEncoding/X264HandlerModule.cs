namespace CXGUI.VideoEncoding
{
    using System;
    using System.Runtime.CompilerServices;

    [CompilerGlobalScope]
    public sealed class X264HandlerModule
    {
        private X264HandlerModule()
        {
        }

        public static void vetest()
        {
            new x264Handler(@"C:\Users\Public\Videos\Sample Videos\Wildlife.avs", @"c:\.mp4").Start();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace Cxgui.StreamMuxer
{
    public interface IMuxingInfo : IMediaProcessingInfo
    {
        string AudioFile
        { get; }

        string DstFile
        { get; }

        TimeSpan TimeLeft
        { get; }

        TimeSpan TimeUsed
        { get; }

        string VideoFile
        { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Cxgui.AudioEncoding
{
    public interface IAudioEncodingInfo : IMediaProcessingInfo
    {
        string AvsFile
        { get; }

        long CurrentFileSize
        { get; }

        int CurrentPosition
        { get; }

        string DestFile
        { get; }

        long EstimatedFileSize
        { get; }

        double Length
        { get; }

        string Log
        { get; }

        TimeSpan TimeLeft
        { get; }

        TimeSpan TimeUsed
        { get; }
    }
}

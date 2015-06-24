using System;
using System.Collections.Generic;
using System.Text;

namespace Cxgui.VideoEncoding
{
    public interface IVideoEncodingInfo : IMediaProcessingInfo
    {
        double AvgBitRate
        { get; }

        string AvsFile
        { get; }

        long CurrentFileSize
        { get; }

        int CurrentFrame
        { get; }

        int CurrentPosition
        { get; }

        string DestFile
        { get; }

        long EstimatedFileSize
        { get; }

        string Log
        { get; }

        double ProcessingFrameRate
        { get; }

        TimeSpan TimeLeft
        { get; }

        TimeSpan TimeUsed
        { get; }

        int TotalFrames
        { get; }

        double TotalLength
        { get; }
    }
}

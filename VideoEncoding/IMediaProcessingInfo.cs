namespace Cxgui
{
    using System;

    public interface IMediaProcessingInfo
    {
        bool HasExited { get; }

        int Progress { get; }
    }
}


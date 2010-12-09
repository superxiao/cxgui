namespace Cxgui
{
    using System;

    public interface IMediaProcessor
    {
        void Start();
        void Stop();

        bool ProcessingDone { get; }

        int Progress { get; }
    }
}


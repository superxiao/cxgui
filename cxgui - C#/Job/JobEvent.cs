namespace CXGUI.Job
{
    using System;

    [Serializable]
    public enum JobEvent
    {
        None,
        VideoEncoding,
        AudioEncoding,
        Muxing,
        OneJobItemProcessing,
        OneJobItemDone,
        AllDone,
        OneJobItemCancelled,
        QuitAllProcessing,
        Error
    }
}


﻿namespace Cxgui.Job
{
    using System;

    [Serializable]
    public enum JobState
    {
        Waiting,
        Working,
        NotProccessed,
        Done,
        Stop,
        Error
    }
}


﻿namespace Cxgui
{
    using System;

    [Serializable]
    public class BadEncoderCmdException : Exception
    {
        public BadEncoderCmdException(string message) : base(message)
        {
        }
    }
}


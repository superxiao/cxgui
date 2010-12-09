namespace Cxgui
{
    using System;

    [Serializable]
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException(string message) : base(message)
        {
        }
    }
}


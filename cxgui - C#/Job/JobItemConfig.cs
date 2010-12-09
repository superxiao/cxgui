namespace Cxgui.Job
{
    using System;

    [Serializable]
    public class JobItemConfig
    {
        protected StreamProcessMode _audioMode;
        protected OutputContainer _container;
        protected bool _useSepAudio;
        protected StreamProcessMode _videoMode;

        public StreamProcessMode AudioMode
        {
            get
            {
                return this._audioMode;
            }
            set
            {
                this._audioMode = value;
            }
        }

        public OutputContainer Container
        {
            get
            {
                return this._container;
            }
            set
            {
                this._container = value;
            }
        }

        public StreamProcessMode VideoMode
        {
            get
            {
                return this._videoMode;
            }
            set
            {
                this._videoMode = value;
            }
        }
    }
}


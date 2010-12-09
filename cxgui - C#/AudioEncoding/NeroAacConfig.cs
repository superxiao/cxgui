namespace Cxgui.AudioEncoding
{
    using System;
    using System.Text;

    [Serializable]
    public class NeroAacConfig : AudioEncConfigBase
    {
        protected int _bitRate;
        protected int _constantBitRate;
        protected double _quality = 0.5;

        public string GetSettings()
        {
            string str = null;
            if (this._quality > 0)
            {
                return new StringBuilder(" -q ").Append(this._quality).ToString();
            }
            if (this._bitRate > 0)
            {
                return new StringBuilder(" -br ").Append(this._bitRate).ToString();
            }
            if (this._constantBitRate > 0)
            {
                str = new StringBuilder(" -cbr ").Append(this._constantBitRate).ToString();
            }
            return str;
        }

        public int BitRate
        {
            get
            {
                return this._bitRate;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                this._bitRate = value;
                this._quality = 0;
                this._constantBitRate = 0;
            }
        }

        public int ConstantBitRate
        {
            get
            {
                return this._constantBitRate;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                this._constantBitRate = value;
                this._quality = 0;
                this._bitRate = 0;
            }
        }

        public double Quality
        {
            get
            {
                return this._quality;
            }
            set
            {
                if (value > 1)
                {
                    value = 1;
                }
                if (value < 0.01)
                {
                    value = 0.01;
                }
                this._quality = value;
                this._bitRate = 0;
                this._constantBitRate = 0;
            }
        }
    }
}


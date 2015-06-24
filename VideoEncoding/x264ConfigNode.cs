namespace Cxgui.VideoEncoding
{
    using System;

    [Serializable]
    public class x264ConfigNode
    {
        protected bool _bool;
        protected bool _defaultBool;
        protected double _defaultNum;
        protected string _defaultStr;
        protected int _defaultStrOptionIndex;
        protected bool _inUse = true;
        protected bool _locked = false;
        protected double _maxNumValue;
        protected double _minNumValue;
        protected string _name;
        protected bool _notCmdOption;
        protected double _num;
        protected string _str;
        protected int _strOptionIndex;
        protected string[] _strOptions;
        protected NodeType _valueType;

        public string GetValueAsStr()
        {
            if (this._valueType == NodeType.Bool)
            {
            }
            return ((this._valueType != NodeType.Num) ? null : this._num.ToString());
        }

        public bool Bool
        {
            get
            {
                return this._bool;
            }
            set
            {
                this._bool = value;
            }
        }

        public bool DefaultBool
        {
            get
            {
                return this._defaultBool;
            }
            set
            {
                this._defaultBool = value;
            }
        }

        public double DefaultNum
        {
            get
            {
                return this._defaultNum;
            }
            set
            {
                this._defaultNum = value;
            }
        }

        public string DefaultStr
        {
            get
            {
                return this._defaultStr;
            }
            set
            {
                this._defaultStr = value;
            }
        }

        public int DefaultStrOptionIndex
        {
            get
            {
                return this._defaultStrOptionIndex;
            }
            set
            {
                this._defaultStrOptionIndex = value;
            }
        }

        public bool InUse
        {
            get
            {
                return this._inUse;
            }
            set
            {
                this._inUse = value;
            }
        }

        public bool Locked
        {
            get
            {
                return this._locked;
            }
            set
            {
                this._locked = value;
            }
        }

        public double MaxNum
        {
            get
            {
                return this._maxNumValue;
            }
            set
            {
                this._maxNumValue = value;
            }
        }

        public double MinNum
        {
            get
            {
                return this._minNumValue;
            }
            set
            {
                this._minNumValue = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public double Num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        public bool Special
        {
            get
            {
                return this._notCmdOption;
            }
            set
            {
                this._notCmdOption = value;
            }
        }

        public string Str
        {
            get
            {
                return this._str;
            }
            set
            {
                this._str = value;
            }
        }

        public int StrOptionIndex
        {
            get
            {
                return this._strOptionIndex;
            }
            set
            {
                this._strOptionIndex = value;
            }
        }

        public string[] StrOptions
        {
            get
            {
                return this._strOptions;
            }
            set
            {
                this._strOptions = value;
            }
        }

        public NodeType Type
        {
            get
            {
                return this._valueType;
            }
            set
            {
                this._valueType = value;
            }
        }
    }
}


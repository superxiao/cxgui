namespace CXGUI.Avisynth
{
    using System;

    [Serializable]
    public class ResolutionCalculator
    {
        protected double _aspectRatio;
        protected bool _fixAspectRatio = true;
        protected bool _fixHeightInsteadOfWidth;
        protected int _height;
        protected bool _lockToSourceAR;
        protected int _mod = 2;
        protected int _width;

        private int CalculateMod(double number)
        {
            int num;
            int num2;
            if ((number % ((double) this._mod)) >= (((double) this._mod) / 2.0))
            {
                num = 1;
                num2 = ((int) number) + 1;
            }
            else
            {
                num = -1;
                num2 = (int) number;
            }
            while ((num2 % this._mod) != 0)
            {
                num2 += num;
            }
            if (num2 == 0)
            {
                num2 = 1;
            }
            return num2;
        }

        public double AspectRatio
        {
            get
            {
                return this._aspectRatio;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("AspectRatio");
                }
                this._aspectRatio = value;
                if (this._fixHeightInsteadOfWidth)
                {
                    this._width = this.CalculateMod(this._height * this._aspectRatio);
                }
                else
                {
                    this._height = this.CalculateMod(((double) this._width) / this._aspectRatio);
                }
            }
        }

        public bool FixHeightInsteadOfWidth
        {
            get
            {
                return this._fixHeightInsteadOfWidth;
            }
            set
            {
                this._fixHeightInsteadOfWidth = value;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Height must be positive.");
                }
                if ((value % this._mod) != 0)
                {
                    value = this.CalculateMod((double) value);
                }
                this._height = value;
                if (this._fixAspectRatio || this._lockToSourceAR)
                {
                    this._width = this.CalculateMod(this._height * this._aspectRatio);
                }
                else
                {
                    this._aspectRatio = ((double) this._width) / ((double) this._height);
                }
            }
        }

        public bool LockAspectRatio
        {
            get
            {
                return this._fixAspectRatio;
            }
            set
            {
                this._fixAspectRatio = value;
            }
        }

        public bool LockToSourceAR
        {
            get
            {
                return this._lockToSourceAR;
            }
            set
            {
                this._lockToSourceAR = value;
            }
        }

        public int Mod
        {
            get
            {
                return this._mod;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Mod");
                }
                this._mod = value;
                this._width = this.CalculateMod((double) this._width);
                this._height = this.CalculateMod((double) this._height);
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Width must be positive.");
                }
                if ((value % this._mod) != 0)
                {
                    value = this.CalculateMod((double) value);
                }
                this._width = value;
                if (this._fixAspectRatio || this._lockToSourceAR)
                {
                    this._height = this.CalculateMod(((double) this._width) / this._aspectRatio);
                }
                else
                {
                    this._aspectRatio = ((double) this._width) / ((double) this._height);
                }
            }
        }
    }
}


namespace CXGUI.GUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    [Serializable]
    public class ControlResetter
    {
        protected Dictionary<Control, bool> _savedCheckBox = new Dictionary<Control, bool>();
        protected Dictionary<Control, string> _savedTextControl = new Dictionary<Control, string>();

        public bool Changed()
        {
            foreach (CheckBox box in this._savedCheckBox.Keys)
            {
                if (box.Checked != this._savedCheckBox[box])
                {
                    return true;
                }
            }
            foreach (Control control in this._savedTextControl.Keys)
            {
                if (control.Text != this._savedTextControl[control])
                {
                    return true;
                }
            }
            return false;
        }

        public int ChangedCount()
        {
            return (this.ResetAndCount(false, this._savedCheckBox.Keys) + this.ResetAndCount(false, this._savedTextControl.Keys));
        }

        public int ChangedCount(IEnumerable controls)
        {
            return this.ResetAndCount(false, controls);
        }

        public void Clear()
        {
            this._savedCheckBox.Clear();
            this._savedTextControl.Clear();
        }

        public Control[] GetChangedControls()
        {
            List<Control> list = new List<Control>();
            foreach (CheckBox box in this._savedCheckBox.Keys)
            {
                if (box.Checked != this._savedCheckBox[box])
                {
                    list.Add(box);
                }
            }
            foreach (Control control in this._savedTextControl.Keys)
            {
                if (control.Text != this._savedTextControl[control])
                {
                    list.Add(control);
                }
            }
            return list.ToArray();
        }

        private int ResetAndCount(bool reset, IEnumerable controls)
        {
            int num = 0;
            IEnumerator enumerator = controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Control current = (Control) enumerator.Current;
                if (this._savedCheckBox.ContainsKey(current))
                {
                    if ((current as CheckBox).Checked != this._savedCheckBox[current])
                    {
                        num++;
                        if (reset)
                        {
                            (current as CheckBox).Checked = this._savedCheckBox[current];
                        }
                    }
                }
                else if (this._savedTextControl.ContainsKey(current) && (current.Text != this._savedTextControl[current]))
                {
                    num++;
                    if (reset)
                    {
                        current.Text = this._savedTextControl[current];
                    }
                }
            }
            return num;
        }

        public int ResetControls()
        {
            return (this.ResetAndCount(true, this._savedCheckBox.Keys) + this.ResetAndCount(true, this._savedTextControl.Keys));
        }

        public int ResetControls(IEnumerable controls)
        {
            return this.ResetAndCount(true, controls);
        }

        public void SaveControls(IEnumerable controls)
        {
            foreach (Control control in controls)
            {
                System.Type type = control.GetType();
                if (control.GetType() == typeof(TextBox) || control.GetType() == typeof(ComboBox) || control.GetType() == typeof(DomainUpDown))
                {
                    this._savedTextControl[control] = control.Text;
                }
                else if (type == typeof(CheckBox))
                {
                    this._savedCheckBox[control] = ((CheckBox)control).Checked;
                }
            }
        }

        public void SaveControls(Control rootControl)
        {
            this.SaveControls(rootControl.Controls);
            IEnumerator enumerator = rootControl.Controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Control current = (Control) enumerator.Current;
                this.SaveControls(current);
            }
        }
    }
}


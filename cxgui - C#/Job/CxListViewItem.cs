namespace Cxgui.Job
{
    using System;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class CxListViewItem : ListViewItem, ISerializable
    {
        protected Cxgui.Job.JobItem _jobItem;

        public CxListViewItem(string[] items) : base(items)
        {
        }

        public CxListViewItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Serialize(info, context);
        }

        public Cxgui.Job.JobItem JobItem
        {
            get
            {
                return this._jobItem;
            }
            set
            {
                this._jobItem = value;
            }
        }
    }
}


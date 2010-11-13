namespace CXGUI.Job
{
    using System;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class CxListViewItem : ListViewItem, ISerializable
    {
        protected CXGUI.Job.JobItem _jobItem;

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

        public CXGUI.Job.JobItem JobItem
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


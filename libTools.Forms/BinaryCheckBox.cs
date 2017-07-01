using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libTools.Forms
{
    public partial class BinaryCheckBox : UserControl
    {
        private uint mValue;
        private int mCount;

        public uint Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                for (int i = 0; i < flowLayoutPanel.Controls.Count; i++)
                {
                    var cb = flowLayoutPanel.Controls[i] as CheckBox;
                    cb.Checked = (mValue & (1 << i)) != 0;
                }
                if (OnValueChanged != null)
                    OnValueChanged(this, Value);
            }
        }

        public int Count
        {
            get { return mCount; }
            set
            {
                mCount = Math.Max(0, Math.Min(32, value));
                flowLayoutPanel.Controls.Clear();
                for (int i = 0; i < mCount; i++)
                {
                    var checkBox = new CheckBox();
                    checkBox.Name = string.Format("comboBox{0}", i + 1);
                    checkBox.Tag = i;
                    checkBox.Text = checkBox.Name.Clone() as string;
                    checkBox.CheckedChanged += CheckBox_CheckedChanged;
                    flowLayoutPanel.Controls.Add(checkBox);
                }
                Value = Value;
            }
        }

        public string[] Names
        {
            get
            {
                var strArray = new string[Count];
                for (int i = 0; i < flowLayoutPanel.Controls.Count; i++)
                {
                    var cb = flowLayoutPanel.Controls[i] as CheckBox;
                    strArray[i] = cb.Text;
                }
                return strArray;
            }
            set
            {
                int count = Math.Min(Count, value.Length);
                for (int i = 0; i < count; i++)
                {
                    var cb = flowLayoutPanel.Controls[i] as CheckBox;
                    cb.Text = value[i];
                }
                for (int i = value.Length; i < Count; i++)
                {
                    var cb = flowLayoutPanel.Controls[i] as CheckBox;
                    cb.Text = "null";
                }
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            int index = (int)cb.Tag;
            if (cb.Checked) mValue |= 1U << index;
            else mValue &= ~(1U << index);
            if (OnValueChanged != null)
                OnValueChanged(this, Value);
        }
        
        public override string Text
        {
            get { return groupBox.Text; }
            set { groupBox.Text = value; }
        }

        public delegate void ValueChanged(BinaryCheckBox sender, uint value);
        public event ValueChanged OnValueChanged;

        public BinaryCheckBox()
        {
            InitializeComponent();
        }

        public BinaryCheckBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}

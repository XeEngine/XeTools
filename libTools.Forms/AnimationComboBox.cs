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
    public partial class AnimationComboBox : ComboBox
    {
        private string mCurName;
        private uint mCurValue;
        private Dictionary<uint, string> mAnims = new Dictionary<uint, string>();

        [Browsable(false)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentName
        {
            get { return mCurName; }
            set
            {
                mCurName = value;
                mCurValue = Xe.Security.Crc32.CalculateDigestAscii(mCurName);
                Text = value;
            }
        }
        [Browsable(false)]
        [DefaultValue(typeof(uint), "0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public uint CurrentValue
        {
            get { return CurrentName != null ? Xe.Security.Crc32.CalculateDigestAscii(CurrentName) : 0U; }
            set
            {
                string name;
                if (value != 0)
                {
                    if (!mAnims.TryGetValue(value, out name))
                        name = value.ToString("X08");
                }
                else
                    name = "<null>";
                CurrentName = name;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ObjectCollection Items
        {
            get { return base.Items; }
        }

        public AnimationComboBox()
        {
            SelectedIndexChanged += AnimationComboBox_SelectedIndexChanged;
            SelectedItem = "null";
            AddAnimation("AtkCombo1");
            AddAnimation("AtkCombo2");
            AddAnimation("AtkCombo3");
            AddAnimation("AtkComboFinisher");
            AddAnimation("Fall");
            AddAnimation("FightRun");
            AddAnimation("FightStand");
            AddAnimation("Guard");
            AddAnimation("HitBack");
            AddAnimation("HitFront");
            AddAnimation("Lie");
            AddAnimation("Rise");
            AddAnimation("Run");
            AddAnimation("Stand");
            AddAnimation("Walk");
            foreach (var name in mAnims.Values)
                Items.Add(name);
            CurrentValue = 0;
        }

        private void AnimationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex >= 0)
                CurrentName = Items[SelectedIndex] as string;
        }

        private void AddAnimation(string name)
        {
            SubAddAnimation(name);
            SubAddAnimation(name + "_d");
            SubAddAnimation(name + "_r");
            SubAddAnimation(name + "_u");
        }
        private void SubAddAnimation(string name)
        {
            mAnims.Add(Xe.Security.Crc32.CalculateDigestAscii(name), name);
        }
    }
}

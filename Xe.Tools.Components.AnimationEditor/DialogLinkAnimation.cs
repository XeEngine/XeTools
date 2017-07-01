using libTools.Anim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimEditor
{
    public partial class DialogLinkAnimation : Form
    {
        AnimationsGroup _AnimationsGroup;
        string _AnimationName;

        public AnimationsGroup AnimationsGroup
        {
            get { return _AnimationsGroup; }
            set
            {
                _AnimationsGroup = value;
                comboBoxAnimations.DataSource = _AnimationsGroup.Animations;
            }
        }
        public string AnimationName
        {
            get { return _AnimationName; }
            set { _AnimationName = value; }
        }

        public DialogLinkAnimation()
        {
            InitializeComponent();
        }

        private void comboBoxAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (sender as ComboBox).SelectedIndex;
            if (index >= 0)
                _AnimationName = _AnimationsGroup.Animations[index].Name;
        }
    }
}

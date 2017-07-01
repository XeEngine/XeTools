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
    public partial class DialogAnimationName : Form
    {
        public string AnimationName
        {
            get { return animationComboBox.Text; }
            set { animationComboBox.CurrentName = value; }
        }

        public DialogAnimationName()
        {
            InitializeComponent();
        }
    }
}

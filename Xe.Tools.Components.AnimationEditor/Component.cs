using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.AnimationEditor
{
    public class Component : IComponent
    {
        public ComponentSettings Settings { get; private set; }
        public Project Project { get; private set; }

        public Component(ComponentSettings settings)
        {
            Settings = settings;
        }

        public bool? ShowDialog()
        {
            var dialog = new AnimEditor.FormAnim();
            var result = dialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                case System.Windows.Forms.DialogResult.Yes:
                    return true;
                case System.Windows.Forms.DialogResult.No:
                    return false;
                case System.Windows.Forms.DialogResult.Cancel:
                    return null;
                default:
                    return null;
            }
        }
        

        public static ComponentInfo GetComponentInfo()
        {
            return new ComponentInfo()
            {
                Name = "Animation",
                ModuleName = "animation",
                Editor = "Animation editor",
                Description = "Used to create simple 2d animation with sprites. The creation of simple hitboxes is supported too."
            };
        }
    }
}

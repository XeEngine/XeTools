using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.AnimationEditor
{
    public class Component : IComponent
    {
        public ComponentProperties Settings { get; private set; }
        public Project Project { get; private set; }

        public bool IsSettingsAvailable => true;

        public Component(ComponentProperties settings)
        {
            Settings = settings;
        }

        public bool? ShowDialog()
        {
            var dialog = new MainWindow();
            return dialog.ShowDialog();
        }
        public void ShowSettings()
        {
            var dialog = new Windows.WindowMain(Settings.Project, Settings.Container, Settings.Item);
            dialog.ShowDialog();
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

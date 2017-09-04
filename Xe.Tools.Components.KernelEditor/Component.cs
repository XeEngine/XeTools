using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.KernelEditor
{
    public class Component : IComponent
    {
        public ComponentProperties Properties { get; private set; }

        public bool IsSettingsAvailable => false;

        public Component(ComponentProperties properties)
        {
            Properties = properties;
        }

        public void Show()
        {
            var dialog = new MainWindow(Properties.Project, Properties.Container, Properties.Item);
            dialog.Show();
        }
        public bool? ShowDialog()
        {
            var dialog = new MainWindow(Properties.Project, Properties.Container, Properties.Item);
            return dialog.ShowDialog();
        }
        public void ShowSettings()
        {
            throw new NotImplementedException();
        }

        public static ComponentInfo GetComponentInfo()
        {
            return new ComponentInfo()
            {
                Name = "Kernel",
                ModuleName = "kernel",
                Editor = "Kernel editor",
                Description = "Used to modify the core of Swords of Calengal game."
            };
        }
    }
}

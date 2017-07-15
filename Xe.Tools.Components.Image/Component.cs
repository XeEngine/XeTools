using System;

namespace Xe.Tools.Components.Image
{
    public class Component : IComponent
    {
        public ComponentProperties Settings { get; private set; }

        public bool IsSettingsAvailable => true;

        public Component(ComponentProperties settings)
        {
            Settings = settings;
        }

        public bool? ShowDialog()
        {
            return new MainWindow().ShowDialog();
        }


        public static ComponentInfo GetComponentInfo()
        {
            return new ComponentInfo()
            {
                Name = "Image",
                ModuleName = "image",
                Editor = "Image properties",
                Description = "Used to process an image in a specific way."
            };
        }

        public void ShowSettings()
        {
            throw new NotImplementedException();
        }
    }
}

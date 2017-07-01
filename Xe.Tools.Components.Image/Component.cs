using System;

namespace Xe.Tools.Components.Image
{
    public class Component : IComponent
    {
        public ComponentSettings Settings { get; private set; }

        public Component(ComponentSettings settings)
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
    }
}

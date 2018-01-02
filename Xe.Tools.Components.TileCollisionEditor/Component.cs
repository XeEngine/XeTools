using System;
using Xe.Tools.Components.TileCollisionEditor.Windows;

namespace Xe.Tools.Components.TileCollisionEditor
{
    public class Component : IComponent
    {
        public ComponentProperties Properties { get; private set; }

        public bool IsSettingsAvailable => false;

        public Component(ComponentProperties settings)
        {
            Properties = settings;
        }

        public CollisionEditor CreateMainWindow()
        {
            var window = new CollisionEditor();
            window.Open(Properties.Project, Properties.File);
            return window;
        }

        public void Show()
        {
            CreateMainWindow().Show();
        }
        public bool? ShowDialog()
        {
            return CreateMainWindow().ShowDialog();
        }


        public static ComponentInfo GetComponentInfo()
        {
            return new ComponentInfo()
            {
                Name = "TileCollisionEditor",
                ModuleName = "tilecollision",
                Editor = "Tile collision editor",
                Description = "Edit collision for tile maps."
            };
        }

        public void ShowSettings()
        {
            throw new NotImplementedException();
        }
    }
}

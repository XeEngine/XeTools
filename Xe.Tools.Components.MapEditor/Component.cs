using System;
using Xe.Tools.Components.MapEditor.ViewModels;
using Xe.Tools.Components.MapEditor.Windows;

namespace Xe.Tools.Components.MapEditor
{
    public class Component : IComponent
    {
        public ComponentProperties Properties { get; private set; }

        public bool IsSettingsAvailable => false;

        public Component(ComponentProperties settings)
        {
            Properties = settings;
        }

        public MainWindow CreateMainWindow()
        {
            var vm = MapEditorViewModel.Instance;
            vm.Project = Properties.Project;
            vm.OpenTileMap(Properties.File.FullPath);
            return new MainWindow(vm);
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
                Name = "Tilemap",
                ModuleName = "tiledmap",
                Editor = "Tile map editor",
                Description = "Map composed by fixed-size tiles."
            };
        }

        public void ShowSettings()
        {
            throw new NotImplementedException();
        }
    }
}

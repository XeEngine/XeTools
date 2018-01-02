using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using Xe.Tools.Components.TileCollisionEditor.ViewModels;
using Xe.Tools.Projects;

namespace Xe.Tools.Components.TileCollisionEditor.Windows
{
    /// <summary>
    /// Interaction logic for CollisionEditor.xaml
    /// </summary>
    public partial class CollisionEditor : Window
    {
        public CollisionEditorViewModel ViewModel => DataContext as CollisionEditorViewModel;

        private IProjectFile _projectFile;

        public CollisionEditor()
        {
            InitializeComponent();
            DataContext = new CollisionEditorViewModel(this);
        }

        public void Open(IProject project, IProjectFile projectFile)
        {
            var path = projectFile.FullPath;
            if (File.Exists(path))
            {
                using (var stream = new StreamReader(path))
                {
                    ViewModel.CollisionSystem = 
                        JsonConvert.DeserializeObject<Xe.Game.Collisions.CollisionSystem>(
                            stream.ReadToEnd()
                        );
                    if (ViewModel.CollisionSystem == null)
                        ViewModel.CollisionSystem = new Game.Collisions.CollisionSystem();
                    _projectFile = projectFile;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_projectFile != null && ViewModel.CollisionSystem != null)
            {
                var path = _projectFile.FullPath;
                using (var stream = new StreamWriter(path))
                {
                    ViewModel.SaveChanges();
                    var str = JsonConvert.SerializeObject(ViewModel.CollisionSystem, Formatting.Indented);
                    stream.Write(str);
                }
            }
            base.OnClosed(e);
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Xe.Game;
using Xe.Game.Animations;
using static Xe.Tools.Project;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        public Project Project { get; private set; }
        public Container Container { get; private set; }
        public Item Item { get; private set; }

        public AnimationData AnimationData { get; private set; }

        private string WorkingFileName { get; set; }
        private string BasePath { get => Path.GetDirectoryName(WorkingFileName); }

        public WindowMain(Project project, Container container, Item item)
        {
            InitializeComponent();

            Project = project;
            Container = container;
            Item = item;

            WorkingFileName = Path.Combine(Path.Combine(Project.ProjectPath, Container.Name), item.Input);
            using (var reader = File.OpenText(WorkingFileName))
            {
                AnimationData = JsonConvert.DeserializeObject<AnimationData>(reader.ReadToEnd());
                if (AnimationData.Textures == null)
                    AnimationData.Textures = new List<Texture>();
                if (AnimationData.Frames == null)
                    AnimationData.Frames = new Dictionary<string, Frame>();
                if (AnimationData.Animations == null)
                    AnimationData.Animations = new Dictionary<string, Animation>();
                if (AnimationData.AnimationGroups == null)
                    AnimationData.AnimationGroups = new List<AnimationRef>();

                Log.Message($"Animation file {WorkingFileName} opened.");
            }
        }


        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            using (var writer = File.CreateText(WorkingFileName))
            {
                var json = JsonConvert.SerializeObject(AnimationData);
                writer.Write(json);
                Log.Message($"Animation file {WorkingFileName} saved.");
            }
        }

        private void MenuViewTextures_Click(object sender, RoutedEventArgs e)
        {
            new WindowTextures(AnimationData.Textures, BasePath).ShowDialog();
        }

        private void MenuToolsAnimationList_Click(object sender, RoutedEventArgs e)
        {
            new WindowSettings(Project).ShowDialog();
        }
    }
}

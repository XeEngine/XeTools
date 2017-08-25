using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.Services;
using Xe.Tools.Components.AnimationEditor.ViewModels;
using Xe.Tools.Wpf.Dialogs;
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
        public AnimationViewModel ViewModel => DataContext as AnimationViewModel;

        private string WorkingFileName { get; set; }
        private string BasePath { get => Path.GetDirectoryName(WorkingFileName); }

        public WindowMain(Project project, Container container, Item item)
        {
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
                    AnimationData.Frames = new List<Frame>();

                if (AnimationData.Animations == null)
                    AnimationData.Animations = new List<Animation>();

                if (AnimationData.AnimationDefinitions == null)
                    AnimationData.AnimationDefinitions = new List<AnimationDefinition>();

                Log.Message($"Animation file {WorkingFileName} opened.");
            }
            SpriteService.Instance.BasePath = BasePath;

            InitializeComponent();
            DataContext = new AnimationViewModel(AnimationData, BasePath);
        }


        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            using (var writer = File.CreateText(WorkingFileName))
            {
                var json = JsonConvert.SerializeObject(AnimationData, Formatting.Indented);
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

        private void MenuViewFrames_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WindowFrames(AnimationData.Frames.ToList(), AnimationData.Textures, BasePath);
            dialog.ShowDialog();
            AnimationData.Frames.Clear();
            foreach (var item in dialog.Frames
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .OrderBy(x => x.Name))
            {
                AnimationData.Frames.Add(item);
            }
        }

        private async void MenuViewAnimMap_Click(object sender, RoutedEventArgs e)
        {
            var settings = await Modules.Animation.Settings.OpenAsync(Project);
            var dialog = new WindowMapping()
            {
                DataContext = new AnimationsMappingViewModel(
                    AnimationData.AnimationDefinitions,
                    AnimationData.Animations,
                    settings.AnimationNames)
            }.ShowDialog();
        }

        private void MenuFileImportOldAnimation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(FileDialog.Behavior.Open, FileDialog.Type.XeAnimation);
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                var oldAnim = new libTools.Anim.AnimationsGroup(fileName);
                Utilities.ImportOldAnimation(AnimationData, oldAnim);
                DataContext = new AnimationViewModel(AnimationData, BasePath);
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.ViewWidth = e.NewSize.Width;
            ViewModel.ViewHeight = e.NewSize.Height;
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Zoom += 0.25;
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Zoom -= 0.25;
        }

        private void Canvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ViewModel.Zoom += (e.Delta / 120) * 0.25;
        }

        private void MenuToolsImportFrames_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuToolsExportFrames_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

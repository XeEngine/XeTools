using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.Services;
using Xe.Tools.Components.AnimationEditor.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Controls;
using Xe.Tools.Wpf.Dialogs;
using static Xe.Tools.Project;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        public IProject Project { get; private set; }
        public IProjectFile ProjectFile { get; private set; }

        public AnimationData AnimationData { get; private set; }
        public AnimationViewModel ViewModel => DataContext as AnimationViewModel;

        private string WorkingFileName { get; set; }
        private string BasePath { get => Path.GetDirectoryName(WorkingFileName); }

        public WindowMain(IProject project, IProjectFile file)
        {
            Project = project;
            ProjectFile = file;

            WorkingFileName = ProjectFile.FullPath;
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
            using (var writer = System.IO.File.CreateText(WorkingFileName))
            {
                ViewModel.SaveChanges();
                var json = JsonConvert.SerializeObject(AnimationData, Formatting.Indented);
                writer.Write(json);
                Log.Message($"Animation file {WorkingFileName} saved.");
            }
        }

        private void MenuViewTextures_Click(object sender, RoutedEventArgs e)
        {
            new WindowTextures(AnimationData, BasePath).ShowDialog();
        }

        private void MenuToolsAnimationList_Click(object sender, RoutedEventArgs e)
        {
            new WindowSettings(Project).ShowDialog();
        }

        private void MenuViewFrames_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WindowFrames(AnimationData, BasePath);
            dialog.ShowDialog();
        }

        private async void MenuViewAnimMap_Click(object sender, RoutedEventArgs e)
        {
            var settings = await Modules.Animation.Settings.OpenAsync(Project);
            var animationMapping = new AnimationsMappingViewModel(
                    AnimationData.AnimationDefinitions,
                    AnimationData.Animations,
                    settings.AnimationNames);
            var dialog = new WindowMapping()
            {
                DataContext = animationMapping
            }.ShowDialog();
            animationMapping.SaveChanges();
            /*AnimationData.AnimationDefinitions.Clear();
            AnimationData.AnimationDefinitions.AddRange(animationMapping.AnimationDefs);*/
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Animations.Add(new Animation()
            {
                Name = "<new animation>",
                FieldHitbox = new Hitbox()
                {
                    Left = -8, Top = -8, Right = 8, Bottom = 8
                },
                Frames = new List<FrameRef>(),
                Speed = 0,
                Loop = 0,
                Texture = Guid.Empty
            });
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Animations.Remove(ViewModel.SelectedAnimation);
        }

        private void List_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ViewModel.SelectedAnimation;
            if (item == null)
                return;

            var dialog = new SingleInputDialog()
            {
                Text = item.Name,
                Description = "Insert a friendly name for selected animation"
            };

            if (dialog.ShowDialog() == true)
            {
                var newName = dialog.Text;
                if (!ViewModel.Animations.Any(x => x.Name == newName))
                {
                    var selectedIndex = List.SelectedIndex;
                    ViewModel.Animations.RemoveAt(selectedIndex);
                    item.Name = newName;
                    ViewModel.Animations.Insert(selectedIndex, item);
                    List.SelectedIndex = selectedIndex;
                }
                else
                {
                    MessageBox.Show($"An animation called {newName} is already used.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddFrame();
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveFrame();
        }
    }
}

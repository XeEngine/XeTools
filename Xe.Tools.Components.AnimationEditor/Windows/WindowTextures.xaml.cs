using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.ViewModels;
using Xe.Tools.Services;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowTextures.xaml
    /// </summary>
    public partial class WindowTextures : Window, INotifyPropertyChanged
    {
        private AnimationData _animationData { get; }

        public string BasePath { get; private set; }

        public WindowTexturesViewModel ViewModel => DataContext as WindowTexturesViewModel;

        public int SelectedIndex
        {
            get => ViewModel.SelectedIndex;
            set => ViewModel.SelectedIndex = value;
        }

        public WindowTextures(AnimationData animationData, string basePath)
        {
            InitializeComponent();
            BasePath = basePath;

            _animationData = animationData;
            DataContext = new WindowTexturesViewModel(animationData.Textures, basePath);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(this, FileDialog.Behavior.Open, FileDialog.Type.ImagePng);
            if (dialog.ShowDialog() == true)
            {
                var fileName = AddTextureToDirectory(dialog.FileName);
                if (fileName != null)
                {
                    var index = SelectedIndex;
                    ViewModel.ReplaceTexture(SelectedIndex, fileName);
                    SelectedIndex = index;
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(this, FileDialog.Behavior.Open, FileDialog.Type.ImagePng);
            if (dialog.ShowDialog() == true)
            {
                var fileName = AddTextureToDirectory(dialog.FileName);
                if (fileName != null)
                {
                    ViewModel.AddTexture(fileName);
                    SelectedIndex = ViewModel.Count - 1;
                }
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            const string message = "Do you want to delete the file? If you click No, it will be deleted only from this application.";

            var index = SelectedIndex;
            switch (MessageBox.Show(message, "Delete confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:
                    ViewModel.RemoveTexture(SelectedIndex, true);
                    break;
                case MessageBoxResult.No:
                    ViewModel.RemoveTexture(SelectedIndex, false);
                    break;
                default:
                    // Do nothing.
                    index = -1;
                    break;
            }
            if (index >= 0)
            {
                if (index >= ViewModel.Count)
                    index--;
                SelectedIndex = index;
            }
        }

        private void ButtonFramesImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(this, FileDialog.Behavior.Open, FileDialog.Type.ImagePng, true);
            if (dialog.ShowDialog() == true)
            {
                var spriteService = new SpriteService(ViewModel.SelectedValue.Image, _animationData.Frames);
                spriteService.ImportFrames(dialog.FileNames, 1);
                spriteService.Texture.Save(ViewModel.SelectedValue.FileName);
                ViewModel.SelectedValue.Image = spriteService.Texture;
            }
        }

        private void ButtonFramesExport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(this, FileDialog.Behavior.Folder, FileDialog.Type.ImagePng);
            if (dialog.ShowDialog() == true)
            {
                var spriteService = new SpriteService(ViewModel.SelectedValue.Image, _animationData.Frames);
                spriteService.ExportFrames(dialog.FileName, (frameName, fileName) =>
                {
                    return true;
                });
            }
        }

        /// <summary>
        /// Copy the specified file to animation project's directory
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>File name only</returns>
        private string AddTextureToDirectory(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var outputPath = Path.Combine(BasePath, fileName);
            if (Path.GetFullPath(filePath) != outputPath)
            {
                try
                {
                    File.Copy(filePath, outputPath);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    fileName = null;
                }
            }
            else
            {
                Log.Message($"Input and output file {filePath} does match; no need to copy.");
            }
            return fileName;
        }
    }
}

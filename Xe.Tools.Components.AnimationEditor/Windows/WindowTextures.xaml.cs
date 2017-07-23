using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Tools.Components.AnimationEditor.ViewModels;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowTextures.xaml
    /// </summary>
    public partial class WindowTextures : Window, INotifyPropertyChanged
    {
        public string BasePath { get; private set; }

        public TexturesViewModel Textures { get; private set; }

        public TextureViewModel CurrentTexture => SelectedIndex >= 0 ? Textures.Textures[SelectedIndex] : null;

        public int SelectedIndex
        {
            get => ListTextures.SelectedIndex;
            set => ListTextures.SelectedIndex = value;
        }

        public WindowTextures(List<Texture> textures, string basePath)
        {
            InitializeComponent();
            BasePath = basePath;

            DataContext = this;
            Textures = new TexturesViewModel(textures, basePath);
            ListTextures.DataContext = Textures;
            SelectedIndex = Textures.Count - 1;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Textures.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TexturesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isValid = SelectedIndex >= 0;
            ButtonChange.IsEnabled = isValid;
            ButtonRemove.IsEnabled = isValid;
            OnPropertyChanged(nameof(CurrentTexture));
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(FileDialog.Behavior.Open, FileDialog.Type.ImagePng);
            if (dialog.ShowDialog() == true)
            {
                var fileName = AddTextureToDirectory(dialog.FileName);
                if (fileName != null)
                {
                    var index = SelectedIndex;
                    Textures.ReplaceTexture(SelectedIndex, fileName);
                    SelectedIndex = index;
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = FileDialog.Factory(FileDialog.Behavior.Open, FileDialog.Type.ImagePng);
            if (dialog.ShowDialog() == true)
            {
                var fileName = AddTextureToDirectory(dialog.FileName);
                if (fileName != null)
                {
                    Textures.AddTexture(fileName);
                    SelectedIndex = Textures.Count - 1;
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
                    Textures.RemoveTexture(SelectedIndex, true);
                    break;
                case MessageBoxResult.No:
                    Textures.RemoveTexture(SelectedIndex, false);
                    break;
                default:
                    // Do nothing.
                    index = -1;
                    break;
            }
            if (index >= 0)
            {
                if (index >= Textures.Count)
                    index--;
                SelectedIndex = index;
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

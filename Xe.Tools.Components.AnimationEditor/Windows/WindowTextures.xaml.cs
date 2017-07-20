using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowTextures.xaml
    /// </summary>
    public partial class WindowTextures : Window, INotifyPropertyChanged
    {
        public string BasePath { get; private set; }

        public ObservableCollection<Texture> Textures { get; set; }

        public TextureViewModel CurrentTexture { get; private set; }

        public WindowTextures(List<Texture> textures, string basePath)
        {
            InitializeComponent();
            BasePath = basePath;
            Textures = new ObservableCollection<Texture>(textures);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox)?.SelectedValue is Texture texture)
            {
                CurrentTexture = new TextureViewModel(texture, BasePath);
            }
        }
    }
}

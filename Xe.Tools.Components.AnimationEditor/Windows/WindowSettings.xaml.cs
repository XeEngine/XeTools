using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xe.Tools.Components.AnimationEditor.Commands;
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class WindowSettings : Window, INotifyPropertyChanged
    {
        private SettingsViewModel _settings;

        internal SettingsViewModel Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public string CurrentAnimationName
        {
            get => TextAnimationName.Text;
            set
            {
                TextAnimationName.Text = value;
                OnPropertyChanged();
                if (ListAnimations.SelectedIndex >= 0)
                {
                    var index = ListAnimations.SelectedIndex;
                    Settings.AnimationNames[ListAnimations.SelectedIndex] = value;
                    ListAnimations.SelectedIndex = index;
                }
                //var index = ListAnimations.SelectedIndex;
                //ListAnimations.SelectedIndex = -1;
                //_settings.AnimationNames.RemoveAt(index);
                //_settings.AnimationNames.Insert(index, value);
                //ListAnimations.SelectedIndex = index;
            }
        }

        public WindowSettings(Project project)
        {
            InitializeComponent();
            DataContext = this;

            Settings = new SettingsViewModel()
            {
                Project = project
            };
            TextAnimationName.DataContext = this;
            ListAnimations.DataContext = Settings;
        }

        protected override async void OnClosed(EventArgs e)
        {
            await Settings.SaveChanges();
            base.OnClosed(e);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedIndex >= 0)
            {
                TextAnimationName.Text = listBox.SelectedValue as string;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Settings.AnimationNames.Add("<new animation>");
            ListAnimations.SelectedIndex = Settings.AnimationNames.Count - 1;
            TextAnimationName.Focus();
            TextAnimationName.SelectAll();
        }
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

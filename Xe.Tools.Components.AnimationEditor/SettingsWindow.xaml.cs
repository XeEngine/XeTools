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

namespace Xe.Tools.Components.AnimationEditor
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private SettingsViewModel _settings;

        internal SettingsViewModel Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public string CurrentAnimationName
        {
            get => ctrlTextAnimationName.Text;
            set
            {
                ctrlTextAnimationName.Text = value;
                OnPropertyChanged();
            }
        }

        public SettingsWindow(Project project)
        {
            InitializeComponent();
            DataContext = this;

            Settings = new SettingsViewModel()
            {
                Project = project
            };
            ctrlTextAnimationName.DataContext = this;
            ctrlListAnimations.DataContext = Settings;
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

        private void ctrlListAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                CurrentAnimationName = e.AddedItems[0].ToString();
                (sender as ListBox).SelectedValue = Guid.NewGuid().ToString();
            }
        }

        private void ctrlButtonAnimAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ctrlButtonAnimRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

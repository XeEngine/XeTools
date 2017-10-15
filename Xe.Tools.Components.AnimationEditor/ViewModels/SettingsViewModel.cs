using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xe.Tools.Components.AnimationEditor.Commands;
using Xe.Tools.Modules;
using Xe.Tools.Projects;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private IProject _project;
        private Animation.Settings settings;
        private ObservableCollection<string> _animationList;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<string> AnimationNames
        {
            get => _animationList;
            set
            {
                _animationList = value;
                OnPropertyChanged();
            }
        }

        public IProject Project
        {
            get => _project;
            set
            {
                _project = value;
                Task.Run(() => ReadSettings(value));
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task SaveChanges()
        {
            settings.AnimationNames = new List<string>(AnimationNames.Distinct().OrderBy(x => x));
            await settings.SaveAsync();
        }
        
        private async Task ReadSettings(IProject project)
        {
            settings = await Animation.Settings.OpenAsync(project);
            AnimationNames = new ObservableCollection<string>(settings.AnimationNames);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

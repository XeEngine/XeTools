﻿using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xe.Tools.Components.AnimationEditor.Commands;
using Xe.Tools.Modules;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Project _project;
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

        public Project Project
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
            await settings.SaveAsync();
        }
        
        private async Task ReadSettings(Project project)
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
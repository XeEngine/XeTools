using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xe.Tools.GameStudio.Commands;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class ItemPropertiesViewModel : BaseNotifyPropertyChanged
    {
        private GameStudioViewModel _vm;
        private IProjectFile _file;
        private IProjectDirectory _directory;

        public Visibility FilePropertiesVisibility => File != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DirectoryPropertiesVisibility => Directory != null ? Visibility.Visible : Visibility.Collapsed;

        public IProjectFile File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(FilePropertiesVisibility));
                if (_file != null)
                {
                    OnPropertyChanged(nameof(FileName));
                    OnPropertyChanged(nameof(FilePath));
					OnPropertyChanged(nameof(FileFormat));
					OnPropertyChanged(nameof(FileTool));
				}
            }
        }

        public IProjectDirectory Directory
        {
            get => _directory;
            set
            {
                _directory = value;
                OnPropertyChanged(nameof(DirectoryPropertiesVisibility));
                if (_directory != null)
                {
                    OnPropertyChanged(nameof(DirectoryName));
                    OnPropertyChanged(nameof(DirectoryPath));
                }
            }
        }

        public string FileName
        {
            get => _file?.Name ?? "<null>";
            set
            {
                if (_file != null)
                {
                    _file.Name = value;
                }
            }
        }
        public string DirectoryName
        {
            get => _directory?.Name ?? "<null>";
            set
            {
                if (_directory != null)
                {
                    _directory.Name = value;
                }
            }
        }

        public string FilePath => _file?.Path;

        public string DirectoryPath => _directory?.Path;

        public string FileFormat
        {
            get => _file?.Format;
            set
            {
                if (_file != null)
                {
                    _file.Format = value;
                }
            }
        }

		public string FileTool
		{
			get
			{
				var component = Globals.Components
					.Where(x => x.ComponentInfo.ModuleName == FileFormat)
					.FirstOrDefault();
				return component?.Name;
			}
		}

		public string RealPath => Path.Combine(_vm.Project?.WorkingDirectory ?? ".", FilePath ?? DirectoryPath ?? ".");

		public OpenFileCommand OpenFileCommand { get; } = new OpenFileCommand();

		public OpenContainingFolderCommand OpenContainingFolderCommand { get; } = new OpenContainingFolderCommand();

		public ShowFileFormatInfoCommand ShowFileFormatInfoCommand { get; } = new ShowFileFormatInfoCommand();

		public ShowFileToolInfoCommand ShowFileToolInfoCommand { get; } = new ShowFileToolInfoCommand();

		public ItemPropertiesViewModel(GameStudioViewModel vm)
        {
            _vm = vm;
            Register();
        }

        public void Register()
        {
            _vm.OnSelectProjectEntry += OnSelectProjectEntry;
        }
        public void Unregister()
        {
            _vm.OnSelectProjectEntry -= OnSelectProjectEntry;
        }

        private void OnSelectProjectEntry(object sender, ProjectEntryViewModel projectEntry)
        {
            var entry = projectEntry.Entry;
            File = entry as IProjectFile;
            Directory = entry as IProjectDirectory;
            OnPropertyChanged(nameof(RealPath));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xe.Game;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.ViewModels.ObjectExtensions.SwordsOfCalengal;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class ObjectPropertiesViewModel: BaseNotifyPropertyChanged
    {
        public delegate void InvalidateEntry(object sender, ObjectEntry entry);
        public event InvalidateEntry OnInvalidateEntry;

        public MainWindowViewModel MainEditor { get; }

        private ObjectEntry _objectEntry;
        public ObjectEntry ObjectEntry
        {
            get => _objectEntry;
            set
            {
                _objectEntry = value;
                OnAllPropertiesChanged();
                UpdateExtension(_objectEntry?.Extension?.Id ?? Guid.Empty);
            }
        }

        #region Object properties

        #region Basic properties

        public string Name
        {
            get => _objectEntry?.Name;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Name = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get => _objectEntry?.Type;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Type = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Appearance

        public string AnimationData
        {
            get => _objectEntry?.AnimationData;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.AnimationData = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public string AnimationName
        {
            get => _objectEntry?.AnimationName;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.AnimationName = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Orientation
        {
            get => (int)(_objectEntry?.Direction ?? Direction.Undefined);
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Direction = (Direction)value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _objectEntry?.Visible ?? false;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Visible = value;
                OnPropertyChanged();
            }
        }

        public bool HasShadow
        {
            get => _objectEntry?.HasShadow ?? false;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.HasShadow = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Layout

        public int X
        {
            get => (int?)_objectEntry?.X ?? 0;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.X = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get => (int?)_objectEntry?.Y ?? 0;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Y = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Z
        {
            get => (int?)_objectEntry?.Z ?? 0;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Z = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => (int?)_objectEntry?.Width ?? 0;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Width = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => (int?)_objectEntry?.Height ?? 0;
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Height = value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        public int Flip
        {
            get => (int)(_objectEntry?.Flip ?? Xe.Flip.None);
            set
            {
                if (_objectEntry == null) return;
                _objectEntry.Flip = (Flip)value;
                OnInvalidateEntry?.Invoke(this, _objectEntry);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Extensions

        public IEnumerable<ObjectExtensionDefinition> Extensions
        {
            get
            {
                var list = new List<ObjectExtensionDefinition>()
                {
                    new ObjectExtensionDefinition() { Id = Guid.Empty, Name = "None", Type = typeof(object) }
                };
                list.AddRange(Modules.ObjectExtensions.SwordsOfCalengal.Extensions);
                return list;
            }
        }

        public Guid ExtensionId
        {
            get => ObjectEntry?.Extension?.Id ?? Guid.Empty;
            set
            {
                if (ObjectEntry?.Extension?.Id != value)
                {
                    UpdateExtension(value);
                    OnPropertyChanged();
                }
            }
        }

        PlayerViewModel _extensionPlayer;
        public PlayerViewModel ExtensionPlayer
        {
            get => _extensionPlayer;
            set
            {
                _extensionPlayer = value;
                _extensionPlayer?.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        EnemyViewModel _extensionEnemy;
        public EnemyViewModel ExtensionEnemy
        {
            get => _extensionEnemy;
            set
            {
                _extensionEnemy = value;
                _extensionEnemy?.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        NpcViewModel _extensionNpc;
        public NpcViewModel ExtensionNpc
        {
            get => _extensionNpc;
            set
            {
                _extensionNpc = value;
                _extensionNpc?.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        MapChangeViewModel _extensionMapChange;
        public MapChangeViewModel ExtensionMapChange
        {
            get => _extensionMapChange;
            set
            {
                _extensionMapChange = value;
                _extensionMapChange.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        ChestViewModel _extensionChest;
        public ChestViewModel ExtensionChest
        {
            get => _extensionChest;
            set
            {
                _extensionChest = value;
                _extensionChest?.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        EventViewModel _extensionEvent;
        public EventViewModel ExtensionEvent
        {
            get => _extensionEvent;
            set
            {
                _extensionEvent = value;
                _extensionEvent?.OnAllPropertiesChanged();
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand SelectAnimationData { get; } = new RelayCommand(o =>
        {
            var sender = o as ObjectPropertiesViewModel;
            var dialog = new ProjectFileDialog(sender.MainEditor.MapEditor.Project,
                new string[] {
                    "animation"
                });
            if (dialog.ShowDialog() == true)
            {
                sender.AnimationData = dialog.SelectedFile?.Path;
            }
        });

        public ICommand SelectAnimationName { get; } = new RelayCommand(o =>
        {
            var sender = o as ObjectPropertiesViewModel;

            var items = sender.MainEditor.MapEditor.AnimationService
                    .GetAnimationDefinitions(sender.AnimationData);
            if (items != null)
            {
                var dialog = new SingleSelectionDialog()
                {
                    Title = "Select an animation...",
                    Description = "Animation name",
                    Items = sender.MainEditor.MapEditor.AnimationService
                        .GetAnimationDefinitions(sender.AnimationData),
                    SelectedItem = sender.AnimationName ?? "Stand"
                };
                if (dialog.ShowDialog() == true)
                {
                    sender.AnimationName = dialog.SelectedItem as string;
                }
            }
        });

        #endregion

        private void UpdateExtension(Guid id)
        {
            if (ObjectEntry != null)
            {
                if (id == Guid.Empty)
                {
                    // No extension specified for the current object.
                    ObjectEntry.Extension = null;
                }
                else if (ObjectEntry.Extension?.Id != id)
                {
                    var ext = Modules.ObjectExtensions.SwordsOfCalengal.Extensions
                        .FirstOrDefault(x => x.Id == id);
                    if (ext != null)
                        ObjectEntry.Extension = Activator.CreateInstance(ext.Type) as IObjectExtension;
                    else if (ObjectEntry != null)
                        ObjectEntry.Extension = null;
                }
                else
                {
                    // Do not change anything.
                }
            }
            var extension = ObjectEntry?.Extension;
            ExtensionPlayer = new PlayerViewModel(extension);
            ExtensionEnemy = new EnemyViewModel(extension);
            ExtensionNpc = new NpcViewModel(extension);
            ExtensionMapChange = new MapChangeViewModel(extension);
            ExtensionChest = new ChestViewModel(extension);
            ExtensionEvent = new EventViewModel(extension);
        }

        public ObjectPropertiesViewModel(MainWindowViewModel vm)
        {
            MainEditor = vm;
            UpdateExtension(Guid.Empty);
        }
    }
}

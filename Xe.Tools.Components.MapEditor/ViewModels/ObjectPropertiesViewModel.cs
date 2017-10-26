using Xe.Game;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class ObjectPropertiesViewModel: BaseNotifyPropertyChanged
    {
        public delegate void InvalidateEntry(object sender, IObjectEntry entry);
        public event InvalidateEntry OnInvalidateEntry;

        private IObjectEntry _objectEntry;
        public IObjectEntry ObjectEntry
        {
            get => _objectEntry;
            set
            {
                _objectEntry = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(AnimationData));
                OnPropertyChanged(nameof(AnimationData));
                OnPropertyChanged(nameof(Orientation));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(HasShadow));
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(Z));
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Flip));
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

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Components.TileCollisionEditor.Windows;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.TileCollisionEditor.ViewModels
{
    public class CollisionTypesViewModel : BaseNotifyPropertyChanged
    {
        private const string MsgRemoveWarningUsage_Title = "Collision type usage";
        private const string MsgRemoveWarningUsage_Msg = "Collision type is used by following id:\n{0}\nDo you want to delete it anyway?";

        private int _selectedIndex;

        public Window Window { get; set; }

        public ObservableCollection<CollisionType> CollisionTypes { get; set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                EditCommand.CanExecute(value);
                RemoveCommand.CanExecute(value);
                MoveUpCommand.CanExecute(value);
                MoveDownCommand.CanExecute(value);
            }
        }

        public CollisionType SelectedCollisionType { get; set; }

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand MoveUpCommand { get; }

        public ICommand MoveDownCommand { get; }

        public CollisionTypesViewModel(Window window, IEnumerable<CollisionType> collisionTypes)
        {
            Window = window;
            CollisionTypes = new ObservableCollection<CollisionType>(collisionTypes);

            AddCommand = new RelayCommand(x =>
            {
                var collisionType = new CollisionType()
                {
                    Item = new Game.Collisions.CollisionType()
                    {
                        Id = Guid.NewGuid()
                    }
                };
                new CollisionTypeEditWindow(collisionType).ShowDialog();
                CollisionTypes.Add(collisionType);
            }, x => true);

            EditCommand = new RelayCommand(x =>
            {
                new CollisionTypeEditWindow(SelectedCollisionType).ShowDialog();
                var selectedIndex = SelectedIndex;
                var item = CollisionTypes[selectedIndex];
                CollisionTypes.RemoveAt(selectedIndex);
                CollisionTypes.Insert(selectedIndex, item);
                SelectedIndex = selectedIndex;
            }, x => SelectedCollisionType != null);

            RemoveCommand = new RelayCommand(x =>
            {
                bool canRemove;
                var usage = CheckUsage(SelectedCollisionType);
                if (usage.Any())
                {
                    var msg = String.Format(MsgRemoveWarningUsage_Msg, 
                        string.Join(",", usage.Select(id => id.ToString("X02"))));
                    canRemove = MessageBox.Show(Window, msg,
                        MsgRemoveWarningUsage_Title, MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes;
                }
                else
                    canRemove = true;

                if (canRemove)
                {
                    CollisionTypes.Remove(SelectedCollisionType);
                }
            }, x => SelectedCollisionType != null);

            MoveUpCommand = new RelayCommand(x =>
            {
                if (SelectedIndex > 0)
                {
                    var selectedIndex = SelectedIndex;
                    var item = CollisionTypes[selectedIndex];
                    CollisionTypes.RemoveAt(selectedIndex);
                    CollisionTypes.Insert(--selectedIndex, item);
                    SelectedIndex = selectedIndex;
                }
            }, x => SelectedIndex > 0);

            MoveDownCommand = new RelayCommand(x =>
            {
                if (SelectedIndex >= 0 && SelectedIndex + 1 < CollisionTypes.Count)
                {
                    var selectedIndex = SelectedIndex;
                    var item = CollisionTypes[selectedIndex];
                    CollisionTypes.RemoveAt(selectedIndex);
                    CollisionTypes.Insert(++selectedIndex, item);
                    SelectedIndex = selectedIndex;
                }
            }, x => SelectedCollisionType != null && SelectedIndex >= 0 && SelectedIndex + 1 < CollisionTypes.Count);
        }

        private IEnumerable<int> CheckUsage(CollisionType collisionType)
        {
            return new int[] { };
        }
    }
}

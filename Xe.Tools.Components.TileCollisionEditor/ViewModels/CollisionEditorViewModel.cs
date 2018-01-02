using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Components.TileCollisionEditor.Windows;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.TileCollisionEditor.ViewModels
{
    public class CollisionEditorViewModel : BaseNotifyPropertyChanged
    {
        private Xe.Game.Collisions.CollisionSystem _collisionSystem;
        private Collision _collision;

        public Window Window { get; set; }

        public Xe.Game.Collisions.CollisionSystem CollisionSystem
        {
            get => _collisionSystem;
            set
            {
                _collisionSystem = value;
                
                CollisionTypes = CollisionSystem?.CollisionTypes?.Select(x => new CollisionType()
                {
                    Item = x
                });

                if (CollisionTypes == null)
                    CollisionTypes = new CollisionType[] { };

                Collisions = new Collision[0x100];
                for (int i = 0; i < Collisions.Length; i++)
                {
                    Collisions[i] = new Collision()
                    {
                        Index = i,
                        CollisionTypes = CollisionTypes,
                        Item = new Game.Collisions.Collision() { }
                    };
                }

                if (CollisionSystem != null)
                {
                    var toTransfer = Math.Min(Collisions.Length, CollisionSystem.Collisions?.Count ?? 0);
                    for (int i = 0; i < toTransfer; i++)
                    {
                        Collisions[i].Item = CollisionSystem.Collisions[i];
                    }
                }

                OnPropertyChanged(nameof(Collisions));
                OnPropertyChanged(nameof(CollisionTypes));
                OnPropertyChanged(nameof(SelectedCollision));
                OnPropertyChanged(nameof(SelectedCollisionType));
            }
        }

        public Collision[] Collisions { get; private set; }

        public IEnumerable<CollisionType> CollisionTypes { get; private set; }

        public Collision SelectedCollision
        {
            get => _collision;
            set
            {
                _collision = value;
                OnPropertyChanged(nameof(SelectedCollisionType));
            }
        }

        public Guid SelectedCollisionType
        {
            get => SelectedCollision?.TypeId ?? Guid.Empty;
            set => SelectedCollision.TypeId = value;
        }

        public ICommand CollisionTypePropertiesCommand { get; }

        public CollisionEditorViewModel(Window window)
        {
            Window = window;

            CollisionTypes = new List<CollisionType>();
            CollisionTypePropertiesCommand = new RelayCommand(x =>
            {
                var dialog = new CollisionTypesWindow(CollisionTypes);
                dialog.ShowDialog();
                CollisionTypes = dialog.CollisionTypes;
                OnPropertyChanged(nameof(CollisionTypes));
            }, x => true);
        }

        public void SaveChanges()
        {
            CollisionSystem.Collisions = Collisions.Select(x => x.Item).ToList();
            CollisionSystem.CollisionTypes = CollisionTypes.Select(x => x.Item).ToList();
        }
    }
}

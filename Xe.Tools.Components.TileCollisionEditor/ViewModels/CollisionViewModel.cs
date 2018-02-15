using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.TileCollisionEditor.ViewModels
{
    public class CollisionViewModel : BaseNotifyPropertyChanged
    {
        public IEnumerable<CollisionType> CollisionTypes { get; set; }

        public Xe.Game.Collisions.Collision Item { get; set; }

        public int Index { get; set; }

        public Guid TypeId
        {
            get => Item.TypeId;
            set
			{
				Item.TypeId = value;
				OnPropertyChanged(nameof(Name));
			}
        }

		public string Name
		{
			get
			{
				var str = Index.ToString("X02");
				var collisionType = CollisionTypes.FirstOrDefault(x => x.Id == TypeId);
				return collisionType != null ? $"{str} - {collisionType.Name}" : str;
			}
		}
    }
}

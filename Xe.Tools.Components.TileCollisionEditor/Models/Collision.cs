using System;
using System.Collections.Generic;
using System.Linq;

namespace Xe.Tools.Components.TileCollisionEditor.Models
{
    public class Collision
    {
        public IEnumerable<CollisionType> CollisionTypes { get; set; }

        public Xe.Game.Collisions.Collision Item { get; set; }

        public int Index { get; set; }

        public Guid TypeId
        {
            get => Item.TypeId;
            set => Item.TypeId = value;
        }

        public override string ToString()
        {
            var str = Index.ToString("X02");
            var collisionType = CollisionTypes.FirstOrDefault(x => x.Id == TypeId);
            return collisionType != null ? $"{str} - {collisionType.Name}" : str;
        }
    }
}

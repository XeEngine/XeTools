using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.TileCollisionEditor.Models
{
    /// <summary>
    /// Defines what happens on collision
    /// </summary>
    public class CollisionType
    {
        public Xe.Game.Collisions.CollisionType Item { get; set; }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid Id => Item.Id;

        /// <summary>
        /// Human readable name of collision type
        /// </summary>
        public string Name
        {
            get => string.IsNullOrEmpty(Item?.Name?.Trim()) ? "<no name>" : Item.Name;
            set => Item.Name = value;
        }

        /// <summary>
        /// Effect used
        /// </summary>
        public Guid Effect
        {
            get => Item.Effect;
            set => Item.Effect = value;
        }

        public int? EffectParamValue
        {
            get => Item.EffectParameterValue;
            set => Item.EffectParameterValue = value;
        }

        public Guid? EffectParamId
        {
            get => Item.EffectParameterValueId;
            set => Item.EffectParameterValueId = value;
        }
    }
}

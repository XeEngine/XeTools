using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Collisions
{
    public class CollisionType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid Effect { get; set; }

        public int? EffectParameterValue { get; set; }

        public Guid? EffectParameterValueId { get; set; }
    }
}

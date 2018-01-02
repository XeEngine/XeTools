using System;
using System.Collections.Generic;

namespace Xe.Tools.Components.TileCollisionEditor.Models
{
    /// <summary>
    /// Describe a collision effect
    /// </summary>
    public class Effect
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ParameterName { get; set; }

        public Func<int, string> ParameterStringFunc { get; set; }

        public bool HasParameter { get; set; }

        public int MinimumValue { get; set; }

        public int MaximumValue { get; set; }

        public IEnumerable<EffectParameter> Parameters { get; set; }
    }

    /// <summary>
    /// Describe a parameter entry
    /// </summary>
    public class EffectParameter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public enum LayerMode
    {
        Default,
        SimpleParallax,
        HorizontalParallax,
        VerticalParallax,
        Mode7,
        Shadow,
        Collision,
        Depth
    }

    public class LayerDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Visible { get; set; }

        public Color Color { get; set; }

        public float HorizontalCameraMultiplier { get; set; }

        public float VerticalCameraMultiplier { get; set; }

        public float HorizontalCameraSpeed { get; set; }

        public float VerticalCameraSpeed { get; set; }
    }
}

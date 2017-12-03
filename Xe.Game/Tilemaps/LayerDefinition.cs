using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public enum LayerRenderingMode
    {
        Default,
        SimpleParallax,
        HorizontalParallax,
        VerticalParallax,
        Mode7,
        Shadow,
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

        public LayerRenderingMode RenderingMode { get; set; }
    }
}

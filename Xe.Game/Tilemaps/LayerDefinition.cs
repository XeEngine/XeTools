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
        Shadow,
        Custom5,
        Custom6,
        Mode7
    }

    public class LayerDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "unknown";

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled { get; set; } = true;

        public Color Color { get; set; } = Color.White;

        public float ParallaxHorizontalMultiplier { get; set; } = 1.0f;

        public float ParallaxVerticalMultiplier { get; set; } = 1.0f;

        public float ParallaxHorizontalSpeed { get; set; } = 0.0f;

        public float ParallaxVerticalSpeed { get; set; } = 0.0f;

        public LayerRenderingMode RenderingMode { get; set; } = LayerRenderingMode.Default;
    }
}

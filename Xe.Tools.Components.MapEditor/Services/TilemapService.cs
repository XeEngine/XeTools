using System.Collections;
using System.Collections.Generic;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Components.MapEditor.Services
{
    public static class TilemapService
    {
        private static readonly string[] LAYER_NAMES = new string[]
        {
            "Programmable",
            "Background 1",
            "Background 2",
            "Low tilemap",
            "Low shadows",
            "High tilemap",
            "High shadows",
            "Highest tilemap",
            "Highest shadows",
            "Foreground 1",
            "Foreground 2",
        };

        public static string[] LayerNames => LAYER_NAMES;

        public static Map Open(string fileName)
        {
            return new TilemapTiled().Open(fileName);
        }

        public static string GetLayerName(int index)
        {
            if (index >= 0 && index < LAYER_NAMES.Length)
                return LAYER_NAMES[index];
            return $"Unknown 0x{index.ToString("X02")}";
        }
    }
}

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

        public static ITileMap Open(string fileName)
        {
            return new TilemapTiled(fileName);
        }

        public static string GetLayerName(int index)
        {
            if (index >= 0 && index < LAYER_NAMES.Length)
                return LAYER_NAMES[index];
            return $"Unknown 0x{index.ToString("X02")}";
        }
        
        public static IEnumerable<ILayerEntry> FlatteredLayers(this IEnumerable<ILayerBase> entries)
        {
            var list = new List<ILayerEntry>();
            foreach (var entry in entries)
            {
                if (entry is ILayerEntry layer)
                {
                    list.Add(layer);
                }
                else if (entry is ILayersGroup group)
                {
                    list.AddRange(FlatteredLayers(group.Layers));
                }
            }
            return list;
        }
    }
}

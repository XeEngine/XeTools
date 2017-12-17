using System;
using System.Collections;
using System.Collections.Generic;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Components.MapEditor.Services
{
    public static class TilemapService
    {
        public static LayerName[] LayerNames => TilemapSettings.LayerNames;

        public static Map Open(string fileName)
        {
            return new TilemapTiled().Open(fileName, Modules.ObjectExtensions.SwordsOfCalengal.Extensions);
        }

        public static string GetLayerName(int index)
        {
            if (index >= 0 && index < LayerNames.Length)
                return LayerNames[index].Name;
            return $"Unknown 0x{index.ToString("X02")}";
        }
    }
}

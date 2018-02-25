using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private class DataLayer
        {
            public int Priority { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public List<LayerTilemap> Sublayers { get; set; }
            public byte[] Data { get; set; }
        }

        private static string WriteCollisionChunk(Map tileMap, BinaryWriter w)
        {
            return WriteGenericChunk(tileMap, w, "collision", LayerProcessingMode.Collision, 0) ?
                "COL\x01" : null;
        }

        private static string WritePriorityChunk(Map tileMap, BinaryWriter w)
        {
            return WriteGenericChunk(tileMap, w, "zdepth", LayerProcessingMode.Depth, 0) ?
                "PRZ\x01" : null;
        }

        private static bool WriteGenericChunk(Map tileMap,
            BinaryWriter w, string tilesetName,
            LayerProcessingMode processingMode,
            byte defaultValue = 0)
        {
            var layers = tileMap.Layers
                .FlatterLayers<LayerTilemap>()
                .Where(x => x.ProcessingMode == processingMode)
                //.Where(x => x.Visible)
                .GroupBy(x => x.DefinitionId)
                .Join(TilemapSettings.LayerNames,
                    x => x.Key,
                    x => x.Id,
                    (layer, def) => new DataLayer
                    {
                        Priority = def.Order,
                        Width = layer.Max(l => l.Width),
                        Height = layer.Max(l => l.Height),
                        Sublayers = layer.ToList()
                    })
                .OrderBy(x => x.Priority)
                .ToList();
            if (layers.Count == 0)
                return false;

            var id = tileMap.Tilesets
                .FirstOrDefault(x => x.Name == tilesetName)
                ?.StartId ?? 0;

            w.Write((byte)layers.Count);
            w.Write((byte)0);
            w.Write((byte)0);
            w.Write((byte)0);
            w.Write((uint)0);
            foreach (var layer in layers)
            {
                w.Write((short)layer.Width);
                w.Write((short)layer.Height);
                w.Write((byte)layer.Priority);
                w.Write((byte)0);
                w.Write((byte)0);
                w.Write((byte)0);
                layer.Data = new byte[layer.Width * layer.Height];
                for (int i = 0; i < layer.Data.Length; i++)
                    layer.Data[i] = defaultValue;
                foreach (var collisionLayer in layer.Sublayers)
                {
                    int width = collisionLayer.Width;
                    int height = collisionLayer.Height;
                    for (int y = 0; y < height; y++)
                    {
                        var index = y * layer.Width;
                        for (int x = 0; x < width; x++)
                        {
                            var data = collisionLayer.Tiles[x, y].Index - id;
							if (data > 0)
							{
								layer.Data[index] = (byte)data;
							}
							index++;
						}
                    }
                }
                w.Write(layer.Data, 0, layer.Data.Length);
            }
            return true;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xe.Game.Tilemaps
{
    public static class TilemapTiled
    {
        private class Ext
        {
            public List<LayerDefinition> LayersDefinition { get; set; }
                = new List<LayerDefinition>();
        }

        public static Map Open(Tiled.Map tiledMap)
        {
            return Map(tiledMap);
        }
        public static Map Open(string tiledFileName)
        {
            return Map(new Tiled.Map(tiledFileName));
        }
        public static Tiled.Map Save(Map map, Tiled.Map tiledMap)
        {
            var r = Map(map, tiledMap);
            r.Save(tiledMap.FileName);
            return r;
        }

        #region Mappings

        #region Map
        private static Map Map(Tiled.Map src, Map dst = null)
        {
            if (dst == null) dst = new Map();
            dst.FileName = src.FileName;
            dst.Size = new System.Drawing.Size(src.Width, src.Height);
            dst.TileSize = new System.Drawing.Size(src.TileWidth, src.TileHeight);
            dst.BackgroundColor = src.BackgroundColor;
            dst.Tilesets = src.Tilesets.Select(x => Map(x)).ToList();
            dst.Layers = src.Entries.Select(x => Map(x)).ToList();

            Uri Extension = GetPropertyValue(src.Properties, default(Uri), nameof(Extension));
            if (Extension != null && !string.IsNullOrEmpty(Extension.OriginalString))
            {
                string fileName;
                if (Path.IsPathRooted(Extension.OriginalString))
                    fileName = Extension.OriginalString;
                else
                    fileName = Path.Combine(Path.GetDirectoryName(src.FileName), Extension.OriginalString);
                if (File.Exists(fileName))
                {
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var strContent = reader.ReadToEnd();
                            var ext = JsonConvert.DeserializeObject<Ext>(strContent);
                            dst.LayersDefinition = ext.LayersDefinition;
                        }
                    }
                }
            }
            return dst;
        }
        private static Tiled.Map Map(Map src, Tiled.Map dst = null)
        {
            var basePath = Path.GetDirectoryName(src.FileName);
            if (dst == null) dst = new Tiled.Map();
            dst.FileName = src.FileName;
            dst.Width = src.Size.Width;
            dst.Height = src.Size.Height;
            dst.TileWidth = src.TileSize.Width;
            dst.TileHeight = src.TileSize.Height;
            dst.BackgroundColor = src.BackgroundColor;
            //dst.Tilesets = src.Tilesets.Select(x => Map(x)).ToList();
            dst.Entries = src.Layers.Select(x => Map(x)).ToList();

            var extFileName = $"{Path.GetFileNameWithoutExtension(src.FileName)}.ext.json";
            var fileName = Path.Combine(Path.GetDirectoryName(src.FileName), extFileName);
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(new Ext()
                    {
                        LayersDefinition = src.LayersDefinition
                    }));
                }
            }
            return dst;
        }
        #endregion
        #region Entries
        public static LayerBase Map(Tiled.IEntry src)
        {
            if (src is Tiled.Group group) return Map(group);
            if (src is Tiled.Layer layer) return Map(layer);
            if (src is Tiled.ObjectGroup objGroup) return Map(objGroup);
            return null;
        }
        public static Tiled.ILayerEntry Map(LayerBase src)
        {
            if (src is LayersGroup group) return Map(group);
            if (src is LayerTilemap layer) return Map(layer);
            if (src is LayerObjects objGroup) return Map(objGroup);
            return null;
        }
        #endregion
        #region Tileset
        public static Tileset Map(Tiled.Tileset src, Tileset dst = null)
        {
            if (dst == null) dst = new Tileset();
            dst.Name = src.Name;
            dst.ExternalTileset = src.Source;
            dst.ImageSource = src.Image?.Source;
            dst.ImagePath = src.FullImagePath;
            dst.StartId = src.FirstGid;
            dst.TileWidth = src.TileWidth;
            dst.TileHeight = src.TileHeight;
            dst.Spacing = src.Spacing ?? 0;
            dst.Margin = src.Margin ?? 0;
            dst.TilesPerRow = src.Columns;
            dst.TilesCount = src.TileCount;
            return dst;
        }
        public static Tiled.Tileset Map(Tileset src, Tiled.Tileset dst = null)
        {
            throw new NotImplementedException();
            //if (dst == null) dst = new Tiled.Tileset();
            //return dst;
        }
        #endregion
        #region Layer
        public static LayerTilemap Map(Tiled.Layer src, LayerTilemap dst = null)
        {
            if (dst == null) dst = new LayerTilemap();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Priority = GetPropertyValue(src.Properties, 0, nameof(LayerTilemap.Priority));
            dst.Opacity = src.Opacity;
            dst.Type = GetPropertyValue(src.Properties, 0, nameof(LayerTilemap.Type));
            dst.Tiles = new Tile[src.Data.GetLength(0), src.Data.GetLength(1)];
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    var data = src.Data[x, y];
                    dst.Tiles[x, y] = new Tile()
                    {
                        Tileset = 0,
                        Index = (int)(data & Tiled.Layer.INDEX_FLAG),
                        IsFlippedX = (data & Tiled.Layer.FLIPPED_HORIZONTALLY_FLAG) != 0,
                        IsFlippedY = (data & Tiled.Layer.FLIPPED_VERTICALLY_FLAG) != 0
                    };
                }
            }
            return dst;
        }
        public static Tiled.Layer Map(LayerTilemap src, Tiled.Layer dst = null)
        {
            if (dst == null) dst = new Tiled.Layer();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Properties[nameof(LayerTilemap.Priority)] = src.Priority;
            dst.Opacity = src.Opacity;
            dst.Properties[nameof(LayerTilemap.Type)] = src.Type;
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.Data = new uint[src.Width, src.Height];
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    var data = src.Tiles[x, y];
                    dst.Data[x, y] = (uint)(
                        (data.Index & Tiled.Layer.INDEX_FLAG) |
                        (data.IsFlippedX ? Tiled.Layer.FLIPPED_HORIZONTALLY_FLAG : 0) |
                        (data.IsFlippedY ? Tiled.Layer.FLIPPED_VERTICALLY_FLAG : 0)
                    );
                }
            }
            dst.Encoding = "base64";
            dst.Compression = "gzip";
            return dst;
        }
        #endregion
        #region Layers group
        public static LayersGroup Map(Tiled.Group src, LayersGroup dst = null)
        {
            if (dst == null) dst = new LayersGroup();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Layers = src.Entries.Select(x => Map(x)).Where(x => x != null).ToList();
            return dst;
        }
        public static Tiled.Group Map(LayersGroup src, Tiled.Group dst = null)
        {
            if (dst == null) dst = new Tiled.Group();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Opacity = 1.0;
            dst.Entries = src.Layers.Select(x => Map(x)).Where(x => x != null).ToList();
            return dst;
        }
        #endregion
        #region Objects group
        public static LayerObjects Map(Tiled.ObjectGroup src, LayerObjects dst = null)
        {
            if (dst == null) dst = new LayerObjects();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Priority = GetPropertyValue(src.Properties, 0, nameof(LayerObjects.Priority));
            dst.Objects = src.Objects.Select(x => Map(x)).ToList();
            return dst;
        }
        public static Tiled.ObjectGroup Map(LayerObjects src, Tiled.ObjectGroup dst = null)
        {
            if (dst == null) dst = new Tiled.ObjectGroup();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Opacity = 1.0;
            dst.Properties[nameof(LayerObjects.Priority)] = src.Priority;
            dst.Objects = src.Objects.Select(x => Map(x)).ToList();
            return dst;
        }
        #endregion
        #region Object entry
        public static ObjectEntry Map(Tiled.Object src, ObjectEntry dst = null)
        {
            if (dst == null) dst = new ObjectEntry();
            // Basic properties
            dst.Name = src.Name;
            dst.Type = src.Type;
            // Appearance
            dst.AnimationData = GetPropertyValue<string>(src.Properties, null, nameof(ObjectEntry.AnimationData));
            dst.AnimationName = GetPropertyValue<string>(src.Properties, null, nameof(ObjectEntry.AnimationName));
            dst.Direction = GetPropertyValue(src.Properties, Direction.Undefined, nameof(ObjectEntry.Direction));
            dst.Visible = src.Visible;
            dst.HasShadow = GetPropertyValue(src.Properties, false, nameof(ObjectEntry.HasShadow));
            // Layout
            dst.X = src.X;
            dst.Y = src.Y;
            dst.Z = GetPropertyValue(src.Properties, 0.0, nameof(ObjectEntry.Z));
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.Flip = GetPropertyValue(src.Properties, Flip.None, nameof(ObjectEntry.Flip));
            return dst;
        }
        public static Tiled.Object Map(ObjectEntry src, Tiled.Object dst = null)
        {
            if (dst == null) dst = new Tiled.Object();
            // Basic properties
            dst.Name = src.Name;
            dst.Type = src.Type;
            // Appearance
            dst.Properties[nameof(ObjectEntry.AnimationData)] = src.AnimationData;
            dst.Properties[nameof(ObjectEntry.AnimationName)] = src.AnimationName;
            dst.Visible = src.Visible;
            dst.Properties[nameof(ObjectEntry.HasShadow)] = src.HasShadow;
            // Layout
            dst.X = src.X;
            dst.Y = src.Y;
            dst.Properties[nameof(ObjectEntry.Z)] = src.Z;
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.Properties[nameof(ObjectEntry.Flip)] = src.Flip;
            return dst;
        }
        #endregion

        #endregion

        #region Utilities

        private static T GetPropertyValue<T>(Tiled.PropertiesDictionary properties, T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            T result;
            if (properties != null && properties.TryGetValue(key, out object value))
            {
                if (typeof(T) == value.GetType())
                    return (T)value;
                try
                {
                    bool isNumeric = IsNumeric(value);
                    var type = typeof(T);
                    if (type.IsEnum)
                    {
                        if (!isNumeric)
                        {
                            foreach (var enumValues in type.GetEnumValues())
                            {
                                if (value.ToString().Equals(enumValues.ToString(), StringComparison.OrdinalIgnoreCase))
                                    return (T)enumValues;
                            }
                            return int.TryParse(value.ToString(), out int r) ? (T)(object)r : defaultValue;
                        }
                        return (T)(object)Convert.ToInt32(value);
                    }
                    result = (T)Convert.ChangeType(value, type, null);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            else
            {
                result = defaultValue;
            }
            return result;
        }

        private static bool IsNumeric(object obj)
        {
            return obj is Byte || obj is SByte ||
                obj is Int16 || obj is UInt16 ||
                obj is Int32 || obj is UInt32 ||
                obj is Int64 || obj is UInt64;
        }

        private static void SetPropertyValue<T>(Tiled.PropertiesDictionary properties, T value, [CallerMemberName] string key = null)
        {
            if (properties != null)
            {
                properties[key] = value?.ToString();
            }
        }

        #endregion
    }
}

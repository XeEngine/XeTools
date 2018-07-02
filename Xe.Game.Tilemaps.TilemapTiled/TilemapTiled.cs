using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xe.Game.Tilemaps
{
    public class TilemapTiled
    {
        private const string Extension = "Extension";
        private const string ExtensionId = "ExtensionId";

        private class Ext
        {
            public List<LayerDefinition> LayersDefinition { get; set; }
                = new List<LayerDefinition>();

            public List<EventDefinition> EventsDefinitions { get; set; }
                = new List<EventDefinition>();
        }

        public Dictionary<Guid, ObjectExtensionDefinition> _objExtensions;

        public Map Open(Tiled.Map tiledMap, IEnumerable<ObjectExtensionDefinition> objExt)
        {
            LoadExtensions(objExt);
            return Map(tiledMap);
        }
        public Map Open(string tiledFileName, IEnumerable<ObjectExtensionDefinition> objExt)
        {
            LoadExtensions(objExt);
            return Map(new Tiled.Map(tiledFileName));
        }
        public Tiled.Map Save(Map map, Tiled.Map tiledMap)
        {
            var r = Map(map, tiledMap);
            r.Save(tiledMap.FileName);
            return r;
        }

        #region Mappings

        #region Map
        private Map Map(Tiled.Map src, Map dst = null)
        {
            if (dst == null) dst = new Map();
            dst.FileName = src.FileName;
            dst.Size = new System.Drawing.Size(src.Width, src.Height);
            dst.TileSize = new System.Drawing.Size(src.TileWidth, src.TileHeight);
            dst.BackgroundColor = src.BackgroundColor;
            dst.Tilesets = src.Tilesets.Select(x => Map(x)).ToList();
            dst.Layers = src.Entries.Select(x => Map(x)).ToList();

			dst.BgmField = GetPropertyValue(src.Properties, Guid.Empty, nameof(dst.BgmField));
			dst.BgmBattle = GetPropertyValue(src.Properties, Guid.Empty, nameof(dst.BgmBattle));

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
                            dst.LayersDefinition = ext?.LayersDefinition ?? new List<LayerDefinition>();
                            dst.EventDefinitions = ext?.EventsDefinitions ?? new List<EventDefinition>();
                        }
                    }
                }
            }
            return dst;
        }
        private Tiled.Map Map(Map src, Tiled.Map dst = null)
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
			dst.Properties[nameof(src.BgmField)] = src.BgmField;
			dst.Properties[nameof(src.BgmBattle)] = src.BgmBattle;

            var extFileName = $"{Path.GetFileNameWithoutExtension(src.FileName)}.ext.json";
            SetPropertyValue(dst.Properties, new Uri(extFileName, UriKind.Relative), "Extension");
            var fileName = Path.Combine(Path.GetDirectoryName(src.FileName), extFileName);
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(new Ext()
                    {
                        LayersDefinition = src.LayersDefinition,
                        EventsDefinitions = src.EventDefinitions?
                            .OrderBy(x => x.Index)
                            .ToList()
                    }, Formatting.Indented));
                }
            }
            return dst;
        }
        #endregion
        #region Entries
        public LayerBase Map(Tiled.IEntry src)
        {
            if (src is Tiled.Group group) return Map(group);
            if (src is Tiled.Layer layer) return Map(layer);
            if (src is Tiled.ObjectGroup objGroup) return Map(objGroup);
            return null;
        }
        public Tiled.ILayerEntry Map(LayerBase src)
        {
            if (src is LayersGroup group) return Map(group);
            if (src is LayerTilemap layer) return Map(layer);
            if (src is LayerObjects objGroup) return Map(objGroup);
            return null;
        }
        #endregion
        #region Tileset
        public Tileset Map(Tiled.Tileset src, Tileset dst = null)
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
        public Tiled.Tileset Map(Tileset src, Tiled.Tileset dst = null)
        {
            throw new NotImplementedException();
            //if (dst == null) dst = new Tiled.Tileset();
            //return dst;
        }
        #endregion
        #region Layer
        public LayerTilemap Map(Tiled.Layer src, LayerTilemap dst = null)
        {
            if (dst == null) dst = new LayerTilemap();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Opacity = src.Opacity;
            dst.DefinitionId = GetPropertyValue(src.Properties, default(Guid), nameof(LayerTilemap.DefinitionId));
            dst.ProcessingMode = GetPropertyValue(src.Properties, LayerProcessingMode.Tilemap,
                nameof(LayerTilemap.ProcessingMode));
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
        public Tiled.Layer Map(LayerTilemap src, Tiled.Layer dst = null)
        {
            if (dst == null) dst = new Tiled.Layer();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Properties[nameof(LayerTilemap.DefinitionId)] = src.DefinitionId;
            dst.Opacity = src.Opacity;
            dst.Properties[nameof(LayerTilemap.ProcessingMode)] = src.ProcessingMode;
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
        public LayersGroup Map(Tiled.Group src, LayersGroup dst = null)
        {
            if (dst == null) dst = new LayersGroup();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Layers = src.Entries.Select(x => Map(x)).Where(x => x != null).ToList();
            return dst;
        }
        public Tiled.Group Map(LayersGroup src, Tiled.Group dst = null)
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
        public LayerObjects Map(Tiled.ObjectGroup src, LayerObjects dst = null)
        {
            if (dst == null) dst = new LayerObjects();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.DefinitionId = GetPropertyValue(src.Properties, default(Guid), nameof(LayerTilemap.DefinitionId));
            dst.Objects = src.Objects.Select(x => Map(x)).ToList();
            return dst;
        }
        public Tiled.ObjectGroup Map(LayerObjects src, Tiled.ObjectGroup dst = null)
        {
            if (dst == null) dst = new Tiled.ObjectGroup();
            dst.Name = src.Name;
            dst.Visible = src.Visible;
            dst.Opacity = 1.0;
            dst.Properties[nameof(LayerTilemap.DefinitionId)] = src.DefinitionId;
            dst.Objects = src.Objects.Select(x => Map(x)).ToList();
            return dst;
        }
        #endregion
        #region Object entry
        public ObjectEntry Map(Tiled.Object src, ObjectEntry dst = null)
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

            var id = GetPropertyValue(src.Properties, Guid.Empty, ExtensionId);
            if (id != Guid.Empty)
            {
                dst.Extension = CreateInstance(id);
                if (dst.Extension != null)
                {
                    foreach (var property in dst.Extension.GetType().GetProperties())
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            var value = GetPropertyValue(src.Properties, property.PropertyType, null, $"{Extension}.{property.Name}");
                            property.SetValue(dst.Extension, value);
                        }
                    }
                }
            }
            return dst;
        }
        public Tiled.Object Map(ObjectEntry src, Tiled.Object dst = null)
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
            dst.Properties[nameof(ObjectEntry.Direction)] = src.Direction;
            // Layout
            dst.X = src.X;
            dst.Y = src.Y;
            dst.Properties[nameof(ObjectEntry.Z)] = src.Z;
            dst.Width = src.Width;
            dst.Height = src.Height;
            dst.Properties[nameof(ObjectEntry.Flip)] = src.Flip;
            if (src.Extension != null)
            {
                var id = src.Extension.Id;
                if (id != Guid.Empty)
                {
                    dst.Properties[ExtensionId] = id;
                    foreach (var property in src.Extension.GetType().GetProperties())
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            dst.Properties[$"{Extension}.{property.Name}"] = property.GetValue(src.Extension);
                        }
                    }
                }
            }
            return dst;
        }
        #endregion

        #endregion

        #region Extension processing

        private void LoadExtensions(IEnumerable<ObjectExtensionDefinition> objExts)
        {
			_objExtensions = objExts?.ToDictionary(x => x.Id) ?? new Dictionary<Guid, ObjectExtensionDefinition>();
        }

        private IObjectExtension CreateInstance(Guid id)
        {
            if (_objExtensions.TryGetValue(id, out var objExt))
                return Activator.CreateInstance(objExt.Type) as IObjectExtension;
            return null;
        }

        #endregion

        #region Utilities

        private static T GetPropertyValue<T>(Tiled.PropertiesDictionary properties, T defaultValue = default(T), [CallerMemberName] string key = null)
        {
            return (T)GetPropertyValue(properties, typeof(T), defaultValue, key);
        }
        private static object GetPropertyValue(Tiled.PropertiesDictionary properties, Type type, object defaultValue, [CallerMemberName] string key = null)
        {
            object result;
            if (properties != null && properties.TryGetValue(key, out object value))
            {
                if (type == value.GetType())
                    return value;
                try
                {
                    bool isNumeric = IsNumeric(value);
                    if (type.IsEnum)
                    {
                        if (!isNumeric)
                        {
                            foreach (var enumValues in type.GetEnumValues())
                            {
                                if (value.ToString().Equals(enumValues.ToString(), StringComparison.OrdinalIgnoreCase))
                                    return enumValues;
                            }
                            return int.TryParse(value.ToString(), out int r) ? r : defaultValue;
                        }
                        return Convert.ToInt32(value);
                    }
                    //result = (T)Convert.ChangeType(value, type, null);
                    result = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value.ToString());
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
            where T : class
        {
            if (properties != null)
            {
                properties[key] = value;
            }
        }

        #endregion
    }
}

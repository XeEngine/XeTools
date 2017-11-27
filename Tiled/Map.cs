using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{
    public class Map
    {
        public enum OrientationType
        {
            Unknown,
            Orthogonal,
            Isometric,
            Staggered,
            Hexagonal
        }

        protected XDocument _xRoot;
        private XElement _xMap;

        public string BasePath { get; }

        /// <summary>
        /// The TMX format version. Was "1.0" so far, and will be incremented to match minor Tiled releases.
        /// </summary>
        public Version FormatVersion { get; private set; }

        /// <summary>
        /// The Tiled version used to save the file. May be a date (for snapshot builds).
        /// </summary>
        public Version TiledVersion { get; private set; }

        /// <summary>
        /// Map orientation. Tiled supports "orthogonal", "isometric", "staggered" and "hexagonal".
        /// </summary>
        public OrientationType Orientation { get; set; }

        /// <summary>
        /// The map width in tiles.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The map height in tiles.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The width of a tile.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The height of a tile.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// The background color of the map.
        /// </summary>
        public Color BackgroundColor { get; }

        /// <summary>
        /// Stores the next available ID for new objects. This number is stored to prevent reuse of the same ID after objects have been removed.
        /// </summary>
        public int NextObjectId { get; set; }

        public PropertiesDictionary Properties { get; private set; }
        
        public List<Tileset> Tilesets { get; }
        public List<ILayerEntry> Entries { get; }


        public Map(string fileName)
        {
            BasePath = Path.GetDirectoryName(Path.GetFullPath(fileName));

            _xRoot = XDocument.Load(fileName);
            _xMap = _xRoot.Element("map") ?? new XElement("map");
            if (_xMap != null)
            {
                Orientation = GetOrientationType((string)_xMap.Attribute("orientation"));
                Width = (int)_xMap.Attribute("width");
                Height = (int)_xMap.Attribute("height");
                TileWidth = (int)_xMap.Attribute("tilewidth");
                TileHeight = (int)_xMap.Attribute("tileheight");
                NextObjectId = (int)_xMap.Attribute("nextobjectid");
                FormatVersion = _xMap.Attribute("version")?.AsVersion();
                TiledVersion = _xMap.Attribute("tiledversion")?.AsVersion();
                BackgroundColor = _xMap.Attribute("backgroundcolor").AsColor();

                Properties = new PropertiesDictionary(_xMap);

                var tilesets = new List<Tileset>();
                var entries = new List<ILayerEntry>();
                foreach (var element in _xMap.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "tileset":
                            tilesets.Add(new Tileset(this, element));
                            break;
                        case "group":
                            entries.Add(new Group(this, element));
                            break;
                        case "layer":
                            entries.Add(new Layer(this, element));
                            break;
                        case "objectgroup":
                            entries.Add(new ObjectGroup(element));
                            break;
                    }
                }
                Tilesets = tilesets;
                Entries = entries;
            }
        }

        public void Save(string fileName)
        {
            SaveChanges();
            Save(_xRoot, fileName);
        }

        private void SaveChanges()
        {
            _xMap.SetAttributeValue("version", FormatVersion.ToString());
            if (FormatVersion != null)
                _xMap.SetAttributeValue("tiledversion", FormatVersion.ToString());
            if (TiledVersion != null)
                _xMap.SetAttributeValue("tiledversion", TiledVersion.ToString());
            _xMap.SetAttributeValue("orientation", GetOrientationType(Orientation));
            _xMap.SetAttributeValue("width", Width);
            _xMap.SetAttributeValue("height", Height);
            _xMap.SetAttributeValue("tilewidth", TileWidth);
            _xMap.SetAttributeValue("tileheight", TileHeight);
            if (BackgroundColor != null)
                _xMap.SetAttributeValue("backgroundcolor", $"#{BackgroundColor.ToString()}");
            _xMap.SetAttributeValue("nextobjectid", NextObjectId);

            _xMap.RemoveNodes();
            if (Properties.Count > 0)
                _xMap.Add(Properties.AsNode());
            foreach (var tileset in Tilesets)
            {
                _xMap.Add(tileset.Element);
            }
            foreach (var item in Entries)
            {
                _xMap.Add(item.AsNode());
            }
        }

        #region Utilities
        private OrientationType GetOrientationType(string orientationType)
        {
            switch (orientationType.ToLower())
            {
                case "orthogonal": return OrientationType.Orthogonal;
                case "isometric": return OrientationType.Isometric;
                case "staggered": return OrientationType.Staggered;
                case "hexagonal": return OrientationType.Hexagonal;
                default: return OrientationType.Unknown;
            }
        }

        private string GetOrientationType(OrientationType orientationType)
        {
            return orientationType.ToString().ToLower();
        }

        internal static void Save(XDocument document, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                document.Save(stream);
        }
        #endregion
    }
}

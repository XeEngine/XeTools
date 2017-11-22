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
        
        public List<Tileset> Tileset { get; }
        public List<ILayerEntry> Entries { get; }
        public IEnumerable<Group> Groups => Entries.Where(x => x is Group).Select(x => x as Group);
        public IEnumerable<Layer> Layers => Entries.Where(x => x is Layer).Select(x => x as Layer);
        public IEnumerable<ObjectGroup> ObjectGroups => Entries.Where(x => x is ObjectGroup).Select(x => x as ObjectGroup);


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

                Version.TryParse((string)_xMap.Attribute("version"), out Version formatVersion);
                Version.TryParse((string)_xMap.Attribute("tiledversion"), out Version tiledVersion);
                Color.TryParse((string)_xMap.Attribute("backgroundcolor"), out Color backgroundColor);
                FormatVersion = formatVersion;
                TiledVersion = tiledVersion;
                BackgroundColor = backgroundColor;

                Properties = new PropertiesDictionary(_xMap);

                var tileset = new List<Tileset>();
                var entries = new List<ILayerEntry>();
                foreach (var element in _xMap.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "tileset":
                            tileset.Add(new Tileset(this, element));
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
                Tileset = tileset;
                Entries = entries;
            }
        }

        public void Save(string fileName)
        {
            SaveChanges();
            _xRoot.Save(fileName);
        }

        private void SaveChanges()
        {
            _xMap.SetAttributeValue("version", FormatVersion.ToString());
            _xMap.SetAttributeValue("tiledversion", TiledVersion.ToString());
            _xMap.SetAttributeValue("orientation", GetOrientationType(Orientation));
            _xMap.SetAttributeValue("width", Width);
            _xMap.SetAttributeValue("height", Height);
            _xMap.SetAttributeValue("tilewidth", TileWidth);
            _xMap.SetAttributeValue("tileheight", TileHeight);
            _xMap.SetAttributeValue("backgroundcolor", $"#{BackgroundColor.ToString()}");
            _xMap.SetAttributeValue("nextobjectid", NextObjectId);
            Properties.SaveChanges();

            foreach (var item in Entries)
                item.SaveChanges();
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
        #endregion
    }
}

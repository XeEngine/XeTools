using System.Collections.Generic;
using System.Drawing;
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

        private const string ElementName = "map";

        protected XDocument _xRoot;

        private string BasePath => Path.GetDirectoryName(FileName);

        /// <summary>
        /// File name of the TMX file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The TMX format version. Was "1.0" so far, and will be incremented
        /// to match minor Tiled releases.
        /// </summary>
        public Version FormatVersion { get; private set; }

        /// <summary>
        /// The Tiled version used to save the file.
        /// May be a date (for snapshot builds).
        /// </summary>
        public Version TiledVersion { get; private set; }

        /// <summary>
        /// Map orientation. Tiled supports "orthogonal", "isometric",
        /// "staggered" and "hexagonal".
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
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Stores the next available ID for new objects. This number is stored
        /// to prevent reuse of the same ID after objects have been removed.
        /// </summary>
        public int NextObjectId { get; set; }

        public PropertiesDictionary Properties { get; private set; }
        
        public List<Tileset> Tilesets { get; set; }
        public List<ILayerEntry> Entries { get; set; }


        public Map() { }
        public Map(string fileName)
        {
            FileName = fileName;

            _xRoot = XDocument.Load(fileName);
            var map = _xRoot.Element(ElementName) ?? new XElement(ElementName);
            if (map != null)
            {
                Orientation = GetOrientationType((string)map.Attribute("orientation"));
                Width = (int)map.Attribute("width");
                Height = (int)map.Attribute("height");
                TileWidth = (int)map.Attribute("tilewidth");
                TileHeight = (int)map.Attribute("tileheight");
                NextObjectId = (int)map.Attribute("nextobjectid");
                FormatVersion = map.Attribute("version")?.AsVersion();
                TiledVersion = map.Attribute("tiledversion")?.AsVersion();
                BackgroundColor = map.Attribute("backgroundcolor").AsColor();

                Properties = new PropertiesDictionary(map);

                var tilesets = new List<Tileset>();
                var entries = new List<ILayerEntry>();
                foreach (var element in map.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "tileset":
                            tilesets.Add(new Tileset(BasePath, element));
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
            XDocument doc = new XDocument();
            doc.Add(AsNode());
            doc.Save(fileName);

            var basePath = Path.GetDirectoryName(fileName);
            foreach (var tileset in Tilesets)
                tileset.SaveChanges(basePath);
        }

        private XElement AsNode(XElement element = null)
        {
            if (element != null)
                element = new XElement(ElementName);

            element.SetAttributeValue("version", FormatVersion.ToString());
            if (FormatVersion != null)
                element.SetAttributeValue("tiledversion", FormatVersion.ToString());
            if (TiledVersion != null)
                element.SetAttributeValue("tiledversion", TiledVersion.ToString());
            element.SetAttributeValue("orientation", GetOrientationType(Orientation));
            element.SetAttributeValue("width", Width);
            element.SetAttributeValue("height", Height);
            element.SetAttributeValue("tilewidth", TileWidth);
            element.SetAttributeValue("tileheight", TileHeight);
            if (BackgroundColor.HasValue)
                element.SetAttributeValue("backgroundcolor", $"#{BackgroundColor.Value.AsString()}");
            element.SetAttributeValue("nextobjectid", NextObjectId);
            
            if (Properties.Count > 0)
                element.Add(Properties.AsNode());
            foreach (var tileset in Tilesets)
                element.Add(tileset.AsNode());
            foreach (var item in Entries)
                element.Add(item.AsNode());
            return element;
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

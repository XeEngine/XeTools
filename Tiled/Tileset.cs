using System.IO;
using System.Xml.Linq;

namespace Tiled
{
    public class Tileset : IEntry
    {
        private Map _map;
        private XElement _xElement;
        private XDocument _xDocTileset;

        /// <summary>
        /// The (maximum) width of the tiles in this tileset.
        /// </summary>
        public int FirstGid
        {
            get => (int?)_xElement.Attribute("firstgid") ?? 0;
            set => _xElement.SetAttributeValue("firstgid", value);
        }

        /// <summary>
        /// If this tileset is stored in an external TSX (Tile Set XML) file,
        /// this attribute refers to that file.
        /// </summary>
        public string Source
        {
            get => _xElement.Attribute("source")?.Value;
            set => _xElement.SetAttributeValue("source", value);
        }

        public string FullImagePath
        {
            get
            {
                var source = Image?.Source;
                if (string.IsNullOrEmpty(source))
                    return null;
                if (Path.IsPathRooted(source))
                    return source;
                return Path.Combine(_map.BasePath, source);
            }
        }

        /// <summary>
        /// The name of this tileset.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The (maximum) width of the tiles in this tileset.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The (maximum) height of the tiles in this tileset.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// The spacing in pixels between the tiles in this tileset.
        /// </summary>
        public int? Spacing { get; set; }

        /// <summary>
        /// The margin around the tiles in this tileset.
        /// </summary>
        public int? Margin { get; set; }

        /// <summary>
        /// The number of tiles in this tileset.
        /// </summary>
        public int TileCount { get; set; }

        /// <summary>
        /// The number of tile columns in the tileset. For image collection tilesets
        /// it is editable and is used when displaying the tileset.
        /// </summary>
        public int Columns { get; set; }

        public Image Image { get; set; }

        public Tileset(Map map, XElement xElement)
        {
            _map = map;
            _xElement = xElement;

            // Load an external tileset, if necessary
            var fileName = GetExternFileName();
            if (!string.IsNullOrEmpty(fileName))
            {
                if (File.Exists(fileName))
                {
                    _xDocTileset = XDocument.Load(fileName);
                    Load(_xDocTileset.Element("tileset"));
                }
            }
            else
            {
                Load(xElement);
            }
        }

        public void SaveChanges()
        {
            var fileName = GetExternFileName();
            if (!string.IsNullOrEmpty(fileName))
            {
                Save(_xDocTileset.Element("tileset"));
                _xDocTileset.Save(fileName);
            }
            else
            {
                Save(_xElement);
            }
        }

        private string GetExternFileName()
        {
            var source = Source;
            return string.IsNullOrEmpty(source) ? null :
                Path.Combine(_map.BasePath, source);
        }

        private void Load(XElement xElement)
        {
            var image = xElement.Element("image");
            if (image == null)
                _xElement.Add(image = new XElement("image"));
            Image = new Image(image);

            Name = xElement.Attribute("name")?.Value;
            TileWidth = (int?)xElement.Attribute("tilewidth") ?? 0;
            TileHeight = (int?)xElement.Attribute("tileheight") ?? 0;
            Spacing = (int?)xElement.Attribute("spacing");
            Margin = (int?)xElement.Attribute("margin");
            TileCount = (int?)xElement.Attribute("tilecount") ?? 0;
            Columns = (int?)xElement.Attribute("columns") ?? 0;
        }
        private void Save(XElement xElement)
        {
            xElement.Attribute("name")?.Remove();
            xElement.Attribute("tilewidth")?.Remove();
            xElement.Attribute("tileheight")?.Remove();
            xElement.Attribute("spacing")?.Remove();
            xElement.Attribute("margin")?.Remove();
            xElement.Attribute("tilecount")?.Remove();
            xElement.Attribute("columns")?.Remove();
        }
    }
}

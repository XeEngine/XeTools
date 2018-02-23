using System.IO;
using System.Xml.Linq;

namespace Tiled
{
    public class Tileset : IEntry
    {
        private const string ElementName = "tileset";

        private string _basePath;

        /// <summary>
        /// The (maximum) width of the tiles in this tileset.
        /// </summary>
        public int FirstGid { get; set; }

        /// <summary>
        /// If this tileset is stored in an external TSX (Tile Set XML) file,
        /// this attribute refers to that file.
        /// </summary>
        public string Source { get; set; }

        public string FullImagePath
		{
			get
			{
				var basePath = _basePath;
				if (!string.IsNullOrEmpty(Source))
					basePath = Path.Combine(basePath, Path.GetDirectoryName(Source));
				return GetFullPath(basePath, Image?.Source);
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
        
        public Tileset(string basePath, XElement xElement)
        {
            _basePath = basePath;
            FirstGid = (int?)xElement.Attribute("firstgid") ?? 0;
            Source = xElement.Attribute("source")?.Value;
            if (!string.IsNullOrEmpty(Source))
            {
                var fileName = GetFullPath(basePath, Source);
                if (File.Exists(fileName))
                {
                    TilesetFromNode(XDocument.Load(fileName).Element(ElementName));
                }
            }
            else
            {
                TilesetFromNode(xElement);
            }
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("firstgid", FirstGid);
            if (string.IsNullOrEmpty(Source))
                element = TilesetAsNode(element);
            else
                element.SetAttributeValue("source", Source);
            return element;
        }

        public void SaveChanges(string basePath)
        {
            if (!string.IsNullOrEmpty(Source))
            {
                var doc = new XDocument();
                doc.Add(TilesetAsNode());
                doc.Save(GetFullPath(basePath, Source));
            }
        }

        private XElement TilesetAsNode(XElement element = null)
        {
            if (element == null)
                element = new XElement(ElementName);
            if (!string.IsNullOrEmpty(Name)) element.SetAttributeValue("name", Name);
            if (TileWidth != 0) element.SetAttributeValue("tilewidth", TileWidth);
            if (TileHeight != 0) element.SetAttributeValue("tileheight", TileHeight);
            if (Spacing != 0) element.SetAttributeValue("spacing", Spacing);
            if (Margin != 0) element.SetAttributeValue("margin", Margin);
            if (TileCount > 0) element.SetAttributeValue("tilecount", TileCount);
            if (Columns > 0) element.SetAttributeValue("columns", Columns);
            element.Add(Image.AsNode());
            return element;
        }

        private void TilesetFromNode(XElement element)
        {
            var image = element.Element("image");
            if (image == null)
                element.Add(image = new XElement("image"));
            Image = new Image(image);

            Name = element.Attribute("name")?.Value;
            TileWidth = (int?)element.Attribute("tilewidth") ?? 0;
            TileHeight = (int?)element.Attribute("tilewidth") ?? 0;
            Spacing = (int?)element.Attribute("spacing");
            Margin = (int?)element.Attribute("margin");
            TileCount = (int?)element.Attribute("tilecount") ?? 0;
            Columns = (int?)element.Attribute("columns") ?? 0;
        }

        private string GetFullPath(string basePath, string source)
        {
            return !Path.IsPathRooted(source) ? Path.Combine(basePath, source) : source;
        }
    }
}

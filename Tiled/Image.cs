using System.Xml.Linq;

namespace Tiled
{
    public class Image
    {
        private XElement _xElement;

        /// <summary>
        /// 
        /// </summary>
        public string Format
        {
            get => _xElement.Attribute("format")?.Value;
            set => _xElement.SetAttributeValue("format", value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            get => _xElement.Attribute("source")?.Value;
            set => _xElement.SetAttributeValue("source", value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Transparency { get; set; }

        /// <summary>
        /// The image width in pixels.
        /// </summary>
        public int? TileWidth
        {
            get => (int?)_xElement.Attribute("width");
            set => _xElement.SetAttributeValue("width", value);
        }

        /// <summary>
        /// The image height in pixels).
        /// </summary>
        public int? TileHeight
        {
            get => (int?)_xElement.Attribute("height");
            set => _xElement.SetAttributeValue("height", value);
        }

        public Image(XElement xElement)
        {
            _xElement = xElement;
            if (Color.TryParse(_xElement.Attribute("trans")?.Value, out Color color))
                Transparency = color;
        }

        public void SaveChanges()
        {
            if (Transparency != null)
                _xElement.SetAttributeValue("trans", Transparency);
            else
                _xElement.Attribute("trans")?.Remove();
        }
    }
}

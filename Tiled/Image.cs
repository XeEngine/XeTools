using System.Drawing;
using System.Xml.Linq;

namespace Tiled
{
    public class Image : INodeItem
    {
        private const string ElementName = "image";

        /// <summary>
        /// Used for embedded images, in combination with a data child element.
        /// Valid values are file extensions like png, gif, jpg, bmp, etc.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The reference to the tileset image file (Tiled supports most common image formats).
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Defines a specific color that is treated as transparent (example value: “#FF00FF” for magenta).
        /// </summary>
        public Color? Transparency { get; set; }

        /// <summary>
        /// The image width in pixels.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// The image height in pixels).
        /// </summary>
        public int? Height { get; set; }

        public Image(XElement xElement)
        {
            Format = xElement.Attribute("format")?.Value;
            Source = xElement.Attribute("source")?.Value;
            Transparency = xElement.Attribute("trans").AsColor();
            Width = (int?)xElement.Attribute("width") ?? 0;
            Height = (int?)xElement.Attribute("height") ?? 0;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("format", Format);
            element.SetAttributeValue("source", Source);
            if (Transparency.HasValue)
                element.SetAttributeValue("trans", Transparency.Value.AsString());
            element.SetAttributeValue("width", Width);
            element.SetAttributeValue("height", Height);
            return element;
        }
    }
}

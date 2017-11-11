using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{
    public class ObjectGroup : ILayerEntry
    {
        private XElement _xElement;

        /// <summary>
        /// The name of the object group.
        /// </summary>
        public string Name
        {
            get => _xElement.Attribute("name")?.Value;
            set => _xElement.SetAttributeValue("name", value);
        }

        /// <summary>
        /// The color used to display the objects in this group.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. Defaults to 1.
        /// </summary>
        public double Opacity
        {
            get
            {
                var strValue = _xElement.Attribute("opacity")?.Value;
                if (strValue != null)
                {
                    if (double.TryParse(strValue, out var value))
                        return value;
                }
                return 1.0;
            }
            set => _xElement?.SetAttributeValue("opacity", value);
        }

        /// <summary>
        /// Whether the layer is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible
        {
            get => (bool?)_xElement.Attribute("visible") ?? true;
            set => _xElement.SetAttributeValue("visible", value);
        }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetX
        {
            get => (int?)_xElement.Attribute("offsetx") ?? 0;
            set => _xElement.SetAttributeValue("offsetx", value);
        }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetY
        {
            get => (int?)_xElement.Attribute("offsety") ?? 0;
            set => _xElement.SetAttributeValue("offsety", value);
        }

        public PropertiesDictionary Properties { get; }

        public List<Object> Objects { get; }

        public ObjectGroup(XElement xElement)
        {
            _xElement = xElement;

            Color.TryParse((string)xElement.Attribute("color"), out var color);
            Color = color;

            Properties = new PropertiesDictionary(_xElement);

            Objects = xElement.Elements("object")
                .Select(x => new Object(x))
                .ToList();
        }

        public void SaveChanges()
        {
            foreach (var item in Objects)
                item.SaveChanges();
            Properties.SaveChanges();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

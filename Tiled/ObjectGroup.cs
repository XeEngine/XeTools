using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{

    public class ObjectGroup : ILayerEntry, INodeItem
    {
        private const string ElementName = "objectgroup";

        /// <summary>
        /// The name of the object group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The color used to display the objects in this group.
        /// </summary>
        public Color? Color { get; }

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. Defaults to 1.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Whether the layer is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetY { get; set; }

        public PropertiesDictionary Properties { get; }

        public List<Object> Objects { get; set; }

        public ObjectGroup() { }
        public ObjectGroup(XElement xElement)
        {
            Name = xElement.Attribute("name")?.Value;
            Color = xElement.Attribute("color")?.AsColor();
            Opacity = (double?)xElement.Attribute("opacity") ?? 1.0;
            Visible = (bool?)xElement.Attribute("visible") ?? true;
            OffsetX = (int?)xElement.Attribute("offsetx") ?? 0;
            OffsetY = (int?)xElement.Attribute("offsety") ?? 0;

            Properties = new PropertiesDictionary(xElement);

            Objects = xElement.Elements("object")
                .Select(x => new Object(x))
                .ToList();
        }

        public XElement AsNode()
        {
            XElement element = new XElement(ElementName);
            element.SetAttributeValue("name", Name);
            if (Color != null) element.SetAttributeValue("color", Color);
            if (Opacity != 1.0) element.SetAttributeValue("opacity", Opacity);
            if (Visible != true) element.SetAttributeValue("visible", Visible ? 1 : 0);
            if (OffsetX != 0) element.SetAttributeValue("offsetx", OffsetX);
            if (OffsetY != 0) element.SetAttributeValue("offsety", OffsetY);
            if (Properties.Count > 0)
                element.Add(Properties.AsNode());
            foreach (var obj in Objects)
            {
                element.Add(obj.AsNode());
            }
            return element;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

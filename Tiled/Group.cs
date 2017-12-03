using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{
    public class Group : ILayerEntry, INodeItem
    {
        private const string ElementName = "group";
        
        /// <summary>
        /// The name of the layer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetY { get; set; }

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. Defaults to 1.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Whether the layer is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible { get; set; }

        public PropertiesDictionary Properties { get; set; }

        public List<ILayerEntry> Entries { get; set; }

        public Group()
        {
            Properties = new PropertiesDictionary();
        }
        public Group(Map map, XElement xElement)
        {
            Name = xElement.Attribute("name")?.Value;
            OffsetX = (int?)xElement.Attribute("offsetx") ?? 0;
            OffsetY = (int?)xElement.Attribute("offsety") ?? 0;
            Opacity = (double?)xElement.Attribute("opacity") ?? 1.0;
            Visible = (bool?)xElement.Attribute("visible") ?? true;
            Properties = new PropertiesDictionary(xElement);

            var entries = new List<ILayerEntry>();
            foreach (var element in xElement.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "group":
                        entries.Add(new Group(map, element));
                        break;
                    case "layer":
                        entries.Add(new Layer(map, element));
                        break;
                    case "objectgroup":
                        entries.Add(new ObjectGroup(element));
                        break;
                }
            }
            Entries = entries;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("name", Name);
            if (OffsetX != 0) element.SetAttributeValue("offsetx", OffsetX);
            if (OffsetY != 0) element.SetAttributeValue("offsety", OffsetY);
            if (Opacity != 1.0) element.SetAttributeValue("opacity", Opacity);
            if (Visible != true) element.SetAttributeValue("visible", Visible ? 1 : 0);

            if (Properties.Count > 0)
                element.Add(Properties.AsNode());
            foreach (var entry in Entries)
                element.Add(entry.AsNode());
            return element;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

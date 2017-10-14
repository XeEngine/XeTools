using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{
    public class Group : IEntry
    {
        private Map _map;
        private XElement _xElement;

        /// <summary>
        /// The name of the layer.
        /// </summary>
        public string Name
        {
            get => _xElement.Attribute("name")?.Value;
            set => _xElement.SetAttributeValue("name", value);
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

        public PropertiesDictionary Properties { get; }

        public List<Group> Groups { get; }
        public List<Layer> Layers { get; }
        public List<ObjectGroup> ObjectGroups { get; }

        public Group(Map map, XElement xElement)
        {
            _map = map;
            _xElement = xElement;

            Properties = new PropertiesDictionary(_xElement);

            Groups = xElement.Elements("group")
                .Select(x => new Group(map, x))
                .ToList();
            Layers = xElement.Elements("layer")
                .Select(x => new Layer(map, x))
                .ToList();
            ObjectGroups = xElement.Elements("objectgroup")
                .Select(x => new ObjectGroup(x))
                .ToList();
        }

        public void SaveChanges()
        {
            foreach (var item in Layers)
                item.SaveChanges();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

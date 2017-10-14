using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Tiled
{
    public class Object : IEntry
    {
        private XElement _xElement;

        /// <summary>
        /// Unique ID of the object. Each object that is placed on a map gets a unique id. Even if an object was deleted, no object gets the same ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of the object. An arbitrary string.
        /// </summary>
        public string Name
        {
            get => _xElement.Attribute("name")?.Value;
            set => _xElement.SetAttributeValue("name", value);
        }

        /// <summary>
        /// The type of the object. An arbitrary string.
        /// </summary>
        public string Type
        {
            get => _xElement.Attribute("type")?.Value;
            set => _xElement.SetAttributeValue("type", value);
        }

        /// <summary>
        /// The x coordinate of the object in pixels.
        /// </summary>
        public int X
        {
            get => (int?)_xElement.Attribute("x") ?? 0;
            set => _xElement.SetAttributeValue("x", value);
        }

        /// <summary>
        /// The y coordinate of the object in pixels.
        /// </summary>
        public int Y
        {
            get => (int?)_xElement.Attribute("y") ?? 0;
            set => _xElement.SetAttributeValue("y", value);
        }

        /// <summary>
        /// The width of the object in pixels (defaults to 0).
        /// </summary>
        public int Width
        {
            get => (int?)_xElement.Attribute("width") ?? 0;
            set => _xElement.SetAttributeValue("width", value);
        }

        /// <summary>
        /// The height of the object in pixels (defaults to 0).
        /// </summary>
        public int Height
        {
            get => (int?)_xElement.Attribute("height") ?? 0;
            set => _xElement.SetAttributeValue("height", value);
        }

        /// <summary>
        /// Whether the object is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible
        {
            get => (bool?)_xElement.Attribute("visible") ?? true;
            set => _xElement.SetAttributeValue("visible", value);
        }

        public PropertiesDictionary Properties { get; }


        public Object(XElement xElement)
        {
            _xElement = xElement;
            Properties = new PropertiesDictionary(_xElement);

            Id = (int)_xElement.Attribute("id");
        }

        public void SaveChanges()
        {

        }
    }
}

using System.Xml.Linq;

namespace Tiled
{
    public class Object : IObjectEntry, ILayerEntry, INodeItem
    {
        private const string ElementName = "object";
        
        /// <summary>
        /// Unique ID of the object. Each object that is placed on a map gets a unique id. Even if an object was deleted, no object gets the same ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of the object. An arbitrary string.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the object. An arbitrary string.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The x coordinate of the object in pixels.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The y coordinate of the object in pixels.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The width of the object in pixels (defaults to 0).
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The height of the object in pixels (defaults to 0).
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Whether the object is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible { get; set; }

        public PropertiesDictionary Properties { get; set; }

        public Object()
        {
            Properties = new PropertiesDictionary();
        }
        public Object(XElement xElement)
        {
            Id = (int)xElement.Attribute("id");
            Name = xElement.Attribute("name")?.Value;
            Type = xElement.Attribute("type")?.Value;
            X = (double?)xElement.Attribute("x") ?? 0;
            Y = (double?)xElement.Attribute("y") ?? 0;
            Width = (double?)xElement.Attribute("width") ?? 0;
            Height = (double?)xElement.Attribute("height") ?? 0;
            Visible = (bool?)xElement.Attribute("visible") ?? true;
            Properties = new PropertiesDictionary(xElement);
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("id", Id);
            if (!string.IsNullOrEmpty(Name)) element.SetAttributeValue("name", Name);
            if (!string.IsNullOrEmpty(Type)) element.SetAttributeValue("type", Type);
            if (X != 0) element.SetAttributeValue("x", X);
            if (Y != 0) element.SetAttributeValue("y", Y);
            if (Width != 0) element.SetAttributeValue("width", Width);
            if (Height != 0) element.SetAttributeValue("height", Height);
            if (Visible != true) element.SetAttributeValue("visible", Visible ? 1 : 0);
            if (Properties.Count > 0)
                element.Add(Properties.AsNode());
            return element;
        }
    }
    
    public class Ellipse : IObjectEntry, INodeItem
    {
        private const string ElementName = "ellipse";

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Ellipse(XElement element)
        {
            X = (double?)element.Attribute("x") ?? 0;
            Y = (double?)element.Attribute("y") ?? 0;
            Width = (double?)element.Attribute("width") ?? 0;
            Height = (double?)element.Attribute("height") ?? 0;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("x", X);
            element.SetAttributeValue("y", Y);
            element.SetAttributeValue("width", Width);
            element.SetAttributeValue("height", Height);
            return element;
        }
    }
    
    public class Point : IObjectEntry, INodeItem
    {
        private const string ElementName = "point";

        public double X { get; set; }

        public double Y { get; set; }

        public Point(XElement element)
        {
            X = (double?)element.Attribute("x") ?? 0;
            Y = (double?)element.Attribute("y") ?? 0;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("x", X);
            element.SetAttributeValue("y", Y);
            return element;
        }
    }

    public class Polyline : IObjectEntry, INodeItem
    {
        private const string ElementName = "polyline";

        public string Points { get; set; }

        public Polyline(XElement element)
        {
            Points = element.Attribute("points")?.Value;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("points", Points);
            return element;
        }
    }

    public class Polygon : IObjectEntry, INodeItem
    {
        private const string ElementName = "polygon";

        public string Points { get; set; }

        public Polygon(XElement element)
        {
            Points = element.Attribute("points")?.Value;
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("points", Points);
            return element;
        }
    }
}

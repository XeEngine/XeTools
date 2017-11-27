using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Tiled
{
    public class PropertiesDictionary : Dictionary<string, object>, INodeItem
    {
        private const string ElementName = "properties";

        private XElement _xElement;

        public PropertiesDictionary(XElement xParent)
        {
            var xElement = _xElement = xParent.Element("properties");
            if (xElement != null)
            {
                _xElement = xElement;
                foreach (var property in xElement.Elements("property"))
                {
                    var name = property.Attribute("name")?.Value;
                    if (!string.IsNullOrEmpty(name))
                    {
                        var attribute = property.Attribute("value");
                        if (attribute != null)
                        {
                            object value;
                            switch (property.Attribute("type")?.Value)
                            {
                                case "int":
                                    value = (int)attribute;
                                    break;
                                case "float":
                                    value = (double)attribute;
                                    break;
                                case "bool":
                                    value = (bool)attribute;
                                    break;
                                case "color":
                                    value = attribute.AsColor();
                                    break;
                                case "file":
                                    value = new Uri(attribute.Value);
                                    break;
                                case "string":
                                default:
                                    value = attribute.Value;
                                    break;
                            }
                            Add(name, value);
                        }
                    }
                }
            }
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            foreach (var property in this)
            {
                var propElement = new XElement("property");
                propElement.SetAttributeValue("name", property.Key);

                object value = property.Value;
                string type = null;
                if (property.Value is Byte || property.Value is SByte ||
                    property.Value is Int16 || property.Value is UInt16 ||
                    property.Value is Int32 || property.Value is UInt32 ||
                    property.Value is Int64 || property.Value is UInt64)
                {
                    type = "int";
                }
                else if (property.Value is Single || property.Value is Double)
                {
                    type = "float";
                }
                else if (property.Value is Boolean)
                {
                    type = "bool";
                    value = (bool)value ? 1 : 0;
                }
                else if (property.Value is Color)
                {
                    type = "color";
                }
                else if (property.Value is Color)
                {
                    type = "color";
                }
                else if (property.Value is Uri uri)
                {
                    type = "file";
                    value = uri.LocalPath;
                }

                if (type != null)
                    propElement.SetAttributeValue("type", type);
                propElement.SetAttributeValue("value", property.Value);

                element.Add(propElement);
            }
            return element;
        }

        private XElement SaveChanges(XElement element)
        {
            foreach (var property in this)
            {
                var propElement = new XElement("property");
                propElement.SetAttributeValue("name", property.Key);
                propElement.SetAttributeValue("value", property.Value);
                element.Add(propElement);
            }
            return element;
        }
    }
}

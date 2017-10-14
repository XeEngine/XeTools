using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tiled
{
    public class PropertiesDictionary : Dictionary<string, string>
    {
        public XElement Element { get; }

        public PropertiesDictionary(XElement xParent)
        {
            var xElement = Element = xParent.Element("properties");
            if (xElement != null)
            {
                Element = xElement;
                foreach (var property in xElement.Elements("property"))
                {
                    var name = property.Attribute("name")?.Value;
                    if (!string.IsNullOrEmpty(name))
                    {
                        var value = property.Attribute("value")?.Value;
                        Add(name, value);
                    }
                }
            }
            else
            {
                xElement = Element = new XElement("properties");
                xParent.Add(xElement);
            }
        }

        public void SaveChanges()
        {
            var insertedElements = new List<string>(Count);
            foreach (var element in Element.Elements("property"))
            {
                var name = element.Attribute("name")?.Value;
                if (name != null && TryGetValue(name, out var value))
                {
                    element.SetAttributeValue("value", value);
                    insertedElements.Add(name);
                }
            }
            if (insertedElements.Count < Count)
            {
                var missingElements = Keys.Except(insertedElements);
                foreach (var key in missingElements)
                {
                    var element = new XElement("property");
                    element.SetAttributeValue("name", key);
                    element.SetAttributeValue("value", this[key]);
                    Element.Add(element);
                }
            }
        }
    }
}

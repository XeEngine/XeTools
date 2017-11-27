using System.Xml.Linq;

namespace Tiled
{
    public interface IEntry
    {
        string Name { get; set; }
    }

    public interface INodeItem
    {
        XElement AsNode();
    }

    public interface ILayerEntry : IEntry, INodeItem
    {
        bool Visible { get; set; }

        PropertiesDictionary Properties { get; }
    }

    public interface IObjectEntry
    {
    }
}

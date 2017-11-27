using System.Xml.Linq;

namespace Tiled
{
    internal static class PrivateExtensions
    {
        public static Color AsColor(this XAttribute attribute)
        {
            if (attribute == null) return null;
            Color.TryParse(attribute.Value, out var color);
            return color;
        }
        
        public static Version AsVersion(this XAttribute attribute)
        {
            if (attribute == null) return null;
            Version.TryParse(attribute.Value, out Version version);
            return version;
        }
    }
}

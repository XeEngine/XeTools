using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

namespace Tiled
{
    internal static class PrivateExtensions
    {
        public static Color? AsColor(this XAttribute attribute)
        {
            return AsColor(attribute?.Value);
        }

        public static string AsString(this Color color)
        {
            return color.A == 255 ? $"#{color.R.ToString("X02")}{color.G.ToString("X02")}{color.B.ToString("X02")}" :
                $"#{color.A.ToString("X02")}{color.R.ToString("X02")}{color.G.ToString("X02")}{color.B.ToString("X02")}";
        }

        public static Color? AsColor(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            if ((str.Length & 1) == 1 && str[0] == '#')
                str = str.Substring(1, str.Length - 1);

            uint.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var c);
            int r, g, b, a;
            switch (str.Length)
            {
                case 3:
                    b = (int)(c >> 0) & 0xF;
                    g = (int)(c >> 4) & 0xF;
                    r = (int)(c >> 8) & 0xF;
                    b |= b << 4;
                    g |= g << 4;
                    r |= r << 4;
                    a = 255;
                    break;
                case 4:
                    b = (int)(c >> 0) & 0xF;
                    g = (int)(c >> 4) & 0xF;
                    r = (int)(c >> 8) & 0xF;
                    a = (int)(c >> 12) & 0xF;
                    b |= b << 4;
                    g |= g << 4;
                    r |= r << 4;
                    a |= a << 4;
                    break;
                case 6:
                    b = (int)(c >> 0) & 0xFF;
                    g = (int)(c >> 8) & 0xFF;
                    r = (int)(c >> 16) & 0xFF;
                    a = 255;
                    break;
                case 8:
                    b = (int)(c >> 0) & 0xFF;
                    g = (int)(c >> 8) & 0xFF;
                    r = (int)(c >> 16) & 0xFF;
                    a = (int)(c >> 24) & 0xFF;
                    break;
                default:
                    return null;
            }
            return Color.FromArgb(a, r, g, b);
        }

        public static Version AsVersion(this XAttribute attribute)
        {
            if (attribute == null) return null;
            Version.TryParse(attribute.Value, out Version version);
            return version;
        }
    }
}

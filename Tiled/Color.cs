using System.Globalization;

namespace Tiled
{
    public class Color
    {
        public byte r, g, b, a;

        public override string ToString()
        {
            return a == 255 ? $"#{r.ToString("X02")}{g.ToString("X02")}{b.ToString("X02")}" :
                $"#{a.ToString("X02")}{r.ToString("X02")}{g.ToString("X02")}{b.ToString("X02")}";
        }

        public static bool TryParse(string str, out Color color)
        {
            if (string.IsNullOrEmpty(str))
            {
                color = new Color();
                return false;
            }
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
                    color = new Color();
                    return false;
            }
            color = new Color()
            {
                r = (byte)r,
                g = (byte)g,
                b = (byte)b,
                a = (byte)a
            };
            return true;
        }
    }
}

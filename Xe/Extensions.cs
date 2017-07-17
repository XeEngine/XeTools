using System.IO;
using System.Text;

namespace Xe
{
    public static class Extensions
    {
        public static int WriteStringData(this BinaryWriter writer, string str)
        {
            return WriteStringData(writer, str, Encoding.ASCII);
        }
        public static int WriteStringData(this BinaryWriter writer, string str, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str ?? string.Empty);
            byte len = (byte)Math.Min(bytes.Length, byte.MaxValue);
            writer.Write(len);
            writer.Write(bytes, 0, len);
            return len;
        }
    }
}

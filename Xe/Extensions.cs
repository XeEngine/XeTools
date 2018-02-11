using System;
using System.Collections.Generic;
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
        public static uint GetXeHash(this string str)
        {
            return Security.Crc32.CalculateDigestAscii(str);
        }

        public static int ToInt(this Guid guid)
        {
            return guid.GetHashCode();
        }
        public static bool IntCollide(this Guid guid, Guid guid2)
        {
            return guid.GetHashCode() == guid2.GetHashCode();
        }


		public static IEnumerable<TResult> Select<TResult>(this Array array, Func<object, TResult> selector)
		{
			foreach (var item in array)
				yield return selector(item);
		}
	}
}

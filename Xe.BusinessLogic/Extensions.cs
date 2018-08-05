using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xe
{
    public static class Extensions
    {
        public static uint GetXeHash(this string str)
        {
            return Security.Crc32.CalculateDigestAscii(str);
        }
	}
}

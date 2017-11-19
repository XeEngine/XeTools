using System;

namespace Xe.Tools.Modules
{
    public class TileSizeException : Exception
    {
        public TileSizeException()
        {
        }

        public TileSizeException(string name, int size, int minimum, int maximum)
        : base($"{name} must fall between {minimum} and {maximum}")
        {

        }

        public static void Assert(int tileWidth, int tileHeight, int minimum, int maximum)
        {
            if (tileWidth < minimum || tileWidth > maximum)
                throw new TileSizeException(nameof(tileWidth), tileWidth, minimum, maximum);
            if (tileHeight < minimum || tileHeight > maximum)
                throw new TileSizeException(nameof(tileHeight), tileHeight, minimum, maximum);
        }
    }
}

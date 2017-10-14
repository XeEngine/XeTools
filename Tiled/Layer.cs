using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace Tiled
{
    public class Layer : IEntry
    {
        // Bits on the far end of the 32-bit global tile ID are used for tile flags
        public const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        public const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        public const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;
        public const uint INDEX_FLAG = ~(FLIPPED_HORIZONTALLY_FLAG |
            FLIPPED_VERTICALLY_FLAG |
            FLIPPED_DIAGONALLY_FLAG);

        private Map _map;
        private XElement _xElement;

        /// <summary>
        /// The name of the layer.
        /// </summary>
        public string Name
        {
            get => _xElement.Attribute("name")?.Value;
            set => _xElement.SetAttributeValue("name", value);
        }

        /// <summary>
        /// The width of the layer in tiles. Always the same as the map width for fixed-size maps.
        /// </summary>
        public int Width => (int?)_xElement.Attribute("height") ?? _map.Width;

        /// <summary>
        /// The height of the layer in tiles. Always the same as the map height for fixed-size maps.
        /// </summary>
        public int Height => (int?)_xElement.Attribute("height") ?? _map.Height;

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. Defaults to 1.
        /// </summary>
        public double Opacity
        {
            get
            {
                var strValue = _xElement.Attribute("opacity")?.Value;
                if (strValue != null)
                {
                    if (double.TryParse(strValue, out var value))
                        return value;
                }
                return 1.0;
            }
            set => _xElement?.SetAttributeValue("opacity", value);
        }

        /// <summary>
        /// Whether the layer is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible
        {
            get => (bool?)_xElement.Attribute("visible") ?? true;
            set => _xElement.SetAttributeValue("visible", value);
        }


        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetX
        {
            get => (int?)_xElement.Attribute("offsetx") ?? 0;
            set => _xElement.SetAttributeValue("offsetx", value);
        }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetY
        {
            get => (int?)_xElement.Attribute("offsety") ?? 0;
            set => _xElement.SetAttributeValue("offsety", value);
        }

        public PropertiesDictionary Properties { get; }

        public uint[,] Data;

        public Layer(Map map, XElement xElement)
        {
            _map = map;
            _xElement = xElement;

            Properties = new PropertiesDictionary(_xElement);

            var xData = xElement.Element("data");
            var encoding = (string)xData.Attribute("encoding");

            int width = Width, height = Height;
            Data = new uint[width, height];
            switch (encoding)
            {
                case "base64":
                    DecodeBase64(xData.Value, (string)xData.Attribute("compression"), Data, width, height);
                    break;
                case "csv":
                    DecodeCsv(xData.Value, Data, width, height);
                    break;
                case null:
                    DecodeXml(xData, Data, width, height);
                    break;
                default:
                    throw new Exception($"Layer data: {encoding} encoding is not supported.");
            }
        }

        public void SaveChanges()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        private static void DecodeXml(XElement xData, uint[,] data, int width, int height)
        {
            int x = 0, y = 0;
            foreach (var e in xData.Elements("tile"))
            {
                data[x, y] = (uint)e.Attribute("gid");
                if (++x >= width)
                {
                    x = 0;
                    y++;
                }
            }
        }
        private void DecodeCsv(string csvData, uint[,] data, int width, int height)
        {
            int x = 0, y = 0;
            foreach (var s in csvData.Split(','))
            {
                data[x, y] = uint.Parse(s.Trim());
                if (++x >= width)
                {
                    x = 0;
                    y++;
                }
            }
        }
        private void DecodeBase64(string base64Data, string compression, uint[,] data, int width, int height)
        {
            var rawData = Convert.FromBase64String(base64Data);
            var memoryStream = new MemoryStream(rawData);

            Stream uncompressedStream;
            switch (compression)
            {
                case "gzip":
                    uncompressedStream = new GZipStream(memoryStream, CompressionMode.Decompress, false);
                    break;
                case "zlib":
                    memoryStream.Position = 2;
                    uncompressedStream = new DeflateStream(memoryStream, CompressionMode.Decompress, false);
                    break;
                case null:
                    uncompressedStream = memoryStream;
                    break;
                default:
                    throw new Exception($"Layer data: {compression} compression is not supported.");
            }

            using (var reader = new BinaryReader(uncompressedStream))
            {
                for (int x = 0, y = 0; y < height;)
                {
                    data[x, y] = reader.ReadUInt32();
                    if (++x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }
    }
}

﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace Tiled
{
    public class Layer : ILayerEntry, INodeItem
    {
        private const string ElementName = "layer";

        // Bits on the far end of the 32-bit global tile ID are used for tile flags
        public const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        public const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        public const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;
        public const uint INDEX_FLAG = ~(FLIPPED_HORIZONTALLY_FLAG |
            FLIPPED_VERTICALLY_FLAG |
            FLIPPED_DIAGONALLY_FLAG);
        
        private XElement _dataElement;

        /// <summary>
        /// The name of the layer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The width of the layer in tiles. Always the same as the map width for fixed-size maps.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the layer in tiles. Always the same as the map height for fixed-size maps.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The opacity of the layer as a value from 0 to 1. Defaults to 1.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Whether the layer is shown (1) or hidden (0). Defaults to 1.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// Rendering offset for this layer in pixels. Defaults to 0.
        /// </summary>
        public int OffsetY { get; set; }

        public PropertiesDictionary Properties { get; }

        public string Encoding { get; set; }

        public string Compression { get; set; }

        public uint[,] Data;

        public Layer()
        {
            Properties = new PropertiesDictionary();
        }
        public Layer(Map map, XElement xElement)
        {
            Name = xElement.Attribute("name")?.Value;
            Width = (int?)xElement.Attribute("width") ?? map.Width;
            Height = (int?)xElement.Attribute("height") ?? map.Height;
            Visible = (bool?)xElement.Attribute("visible") ?? true;
            Opacity = (double?)xElement.Attribute("opacity") ?? 1.0;
            OffsetX = (int?)xElement.Attribute("offsetx") ?? 0;
            OffsetY = (int?)xElement.Attribute("offsety") ?? 0;
            Properties = new PropertiesDictionary(xElement);

            Data = new uint[Width, Height];
            var xData = _dataElement = xElement.Element("data");
            if (xData != null)
            {
                Encoding = xData.Attribute("encoding")?.Value;
                Compression = xData.Attribute("compression")?.Value;

                int width = Width, height = Height;
                switch (Encoding)
                {
                    case "base64":
                        DecodeBase64(xData.Value, Compression, Data, width, height);
                        break;
                    case "csv":
                        DecodeCsv(xData.Value, Data, width, height);
                        break;
                    case null:
                        DecodeXml(xData, Data, width, height);
                        break;
                    default:
                        throw new Exception($"Layer data: {Encoding} decoding is not supported.");
                }
            }
        }

        public XElement AsNode()
        {
            var element = new XElement(ElementName);
            element.SetAttributeValue("name", Name);
            element.SetAttributeValue("width", Width);
            element.SetAttributeValue("height", Height);
            if (Visible != true) element.SetAttributeValue("visible", Visible ? 1 : 0);
            if (Opacity != 1.0) element.SetAttributeValue("opacity", Opacity);
            if (OffsetX != 0) element.SetAttributeValue("offsetx", OffsetX);
            if (OffsetY != 0) element.SetAttributeValue("offsety", OffsetY);
            if (Properties.Count > 0)
                element.Add(Properties.AsNode());

            var dataElement = new XElement("data");
            if (!string.IsNullOrEmpty(Encoding))
                dataElement.SetAttributeValue("encoding", Encoding);
            if (!string.IsNullOrEmpty(Compression))
                dataElement.SetAttributeValue("compression", Compression);

            string strData;
            switch (Encoding)
            {
                case "base64":
                    strData = EncodeBase64(Compression, Data, Width, Height);
                    break;
                case "csv":
                    strData = EncodeCsv(Data, Width, Height);
                    break;
                case null:
                    strData = null;
                    EncodeXml(dataElement, Data, Width, Height);
                    break;
                default:
                    throw new Exception($"Layer data: {Encoding} encoding is not supported.");
            }
            if (strData != null)
            {
                dataElement.Add(strData);
            }
            element.Add(dataElement);

            return element;
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

        private static void EncodeXml(XElement xData, uint[,] data, int width, int height)
        {
            int arrayX = data.GetLength(0);
            int arrayY = data.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    uint uid = 0;
                    if (x < arrayX && y < arrayY)
                        uid = data[x, y];
                    var element = new XElement("tile");
                    element.SetAttributeValue("gid", uid);
                    xData.Add(element);
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

        private string EncodeCsv(uint[,] data, int width, int height)
        {
            var strBuilder = new StringBuilder(width * height * 4);
            int arrayX = data.GetLength(0);
            int arrayY = data.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    uint uid = 0;
                    if (x < arrayX && y < arrayY)
                        uid = data[x, y];
                    strBuilder.Append($"{uid},");
                }
            }
            return strBuilder.Remove(strBuilder.Length - 1, 1).ToString();
        }

        private void DecodeBase64(string base64Data, string compression, uint[,] data, int width, int height)
        {
            var rawData = Convert.FromBase64String(base64Data);
            var memoryStream = new MemoryStream(rawData);
            ushort header;

            Stream uncompressedStream;
            switch (compression)
            {
                case "gzip":
                    uncompressedStream = new GZipStream(memoryStream, CompressionMode.Decompress, false);
                    break;
                case "zlib":
                    header = (ushort)(memoryStream.ReadByte() | (memoryStream.ReadByte() << 8));
                    uncompressedStream = new DeflateStream(memoryStream, CompressionMode.Decompress, false);
                    break;
                case null:
                    uncompressedStream = memoryStream;
                    break;
                default:
                    throw new Exception($"Layer data: {compression} decompression is not supported.");
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

        private string EncodeBase64(string compression, uint[,] data, int width, int height)
        {
            Stream stream;
            var memStream = new MemoryStream(width * height * sizeof(uint));
            uint? checksum = null;
            switch (compression)
            {
                case "gzip":
                    stream = new GZipStream(memStream, CompressionMode.Compress, true);
                    break;
                /*case "zlib":
                    memStream.WriteByte(0x78);
                    memStream.WriteByte(0x9C);
                    stream = new DeflateStream(memStream, CompressionMode.Compress, true);
                    checksum = 0;
                    break;*/
                case null:
                    stream = memStream;
                    break; 
                default:
                    throw new Exception($"Layer data: {compression} compression is not supported.");
            }

            int arrayX = data.GetLength(0);
            int arrayY = data.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    uint uid = 0;
                    if (x < arrayX && y < arrayY)
                        uid = data[x, y];
                    stream.WriteByte((byte)((uid >> 0) & 0xFF));
                    stream.WriteByte((byte)((uid >> 8) & 0xFF));
                    stream.WriteByte((byte)((uid >> 16) & 0xFF));
                    stream.WriteByte((byte)((uid >> 24) & 0xFF));
                }
            }
            stream.Flush();
            if (memStream != stream)
                stream.Close();
            if (checksum.HasValue)
            {
                memStream.WriteByte((byte)((checksum >> 0) & 0xFF));
                memStream.WriteByte((byte)((checksum >> 8) & 0xFF));
                memStream.WriteByte((byte)((checksum >> 16) & 0xFF));
                memStream.WriteByte((byte)((checksum >> 24) & 0xFF));
            }
            return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
        }
    }
}

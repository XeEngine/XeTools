using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Globalization;
using Xe.Drawing;
using Xe.Game.Fonts;

namespace Xe.Game.Drawing
{
    public class FontDrawer : IDisposable
    {
        private IDrawing _drawing;
        private Fonts.Font _font;

        private Color _color;
        private Dictionary<string, ISurface> _surfaces;
        //private ImageAttributes mImgAttribute;

        public Color ForeColor
        {
            get => _color;
            set
            {
                _color = value;
                /*float r = _color.R / 255.0f;
                float g = _color.G / 255.0f;
                float b = _color.B / 255.0f;
                float a = _color.A / 255.0f;
                float[][] coeff = {
                            new float[] { r, 0, 0, 0, 0 },
                            new float[] { 0, g, 0, 0, 0 },
                            new float[] { 0, 0, b, 0, 0 },
                            new float[] { 0, 0, 0, a, 0 },
                            new float[] { 0, 0, 0, 0, 1 }};
                mImgAttribute = new ImageAttributes();
                mImgAttribute.SetColorMatrix(new ColorMatrix(coeff));*/
            }
        }

        public FontDrawer(IDrawing drawing, Fonts.Font font)
        {
            ForeColor = Color.White;

            _drawing = drawing;
            _font = font;
            _surfaces = _font.Tables
                .Select(x => x.Texture)
                .Distinct()
                .ToDictionary(x => x, x => _drawing.CreateSurface(x));
        }
        
        public void DrawString(Graphics g, string str, Rectangle bounds)
        {
            int x = 0, y = -_font.CharSets.YOffset;
            ForeColor = Color.White;
            for (int i = 0; i < str.Length; i++)
            {
                int index, width;
                FontTable table;
                char c = str[i];
                switch (c)
                {
                    case '\r':
                        x = 0;
                        break;
                    case '\n':
                        x = 0;
                        y += _font.CharSets.Height;
                        break;
                    case '{':
                        int end = str.Substring(i).IndexOf("}");
                        if (end > 0)
                        {
                            var cmd = str.Substring(i + 1, end - 1).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (cmd.Length > 0)
                            {
                                switch (cmd[0])
                                {
                                    case "COLOR":
                                        if (cmd.Length == 2)
                                        {
                                            if (int.TryParse(cmd[1], NumberStyles.HexNumber, null, out int hexcolor))
                                            {
                                                int red = ((hexcolor >> 8) & 15) << 4;
                                                int gre = ((hexcolor >> 4) & 15) << 4;
                                                int blu = ((hexcolor >> 0) & 15) << 4;
                                                ForeColor = Color.FromArgb(0xFF, red, gre, blu);
                                                i += end;
                                                continue;
                                            }
                                        }
                                        break;

                                }
                            }
                        }
                        goto default;
                    default:
                        index = ProcessCharacter(c, out table);
                        if (table != null && index >= 0 && index < table.CharCount)
                        {
                            width = table.Spaces[index];
                            if (x > 0 && x + width > bounds.Width)
                            {
                                x = 0;
                                y += _font.CharSets.Height;
                            }
                            DrawChar(g, c, bounds.X + x, bounds.Y + y, ForeColor);
                            x += width;
                        }
                        break;
                }
            }
        }
        public int DrawChar(Graphics g, char c, int x, int y, Color color)
        {
            FontTable table;
            int index = ProcessCharacter(c, out table);
            return DrawChar(g, table, index, x, y, color);
        }
        public int DrawChar(Graphics g, char c, int x, int y, int width, int height, Color color)
        {
            return DrawChar(g, c, new Rectangle(x, y, width, height), color);
        }
        public int DrawChar(Graphics g, char c, Rectangle rect, Color color)
        {
            FontTable table;
            int index = ProcessCharacter(c, out table);
            return DrawChar(g, table, index, rect, color);
        }
        public int DrawChar(Graphics g, FontTable table, int index, int x, int y, Color color)
        {
            return LLDrawChar(g, table, index, new Rectangle(x, y, _font.CharSets.MaximumWidth, _font.CharSets.MaximumHeight), color);
        }
        public int DrawChar(Graphics g, FontTable table, int index, int x, int y, int width, int height, Color color)
        {
            return DrawChar(g, table, index, new Rectangle(x, y, width, height), color);
        }
        public int DrawChar(Graphics g, FontTable table, int index, Rectangle rect, Color color)
        {
            int w = rect.Width;
            int h = rect.Height;
            float fx = w / (float)_font.CharSets.MaximumWidth;
            float fh = h / (float)_font.CharSets.MaximumHeight;
            float zoom = Math.Min(fx, fh);
            rect.Width = (int)Math.Round(_font.CharSets.MaximumWidth * zoom);
            rect.Height = (int)Math.Round(_font.CharSets.MaximumHeight * zoom);
            return LLDrawChar(g, table, index, rect, color);
        }

        private int ProcessCharacter(char c, out FontTable table)
        {
            if (_font.Tables.Count <= 0)
            {
                table = null;
                return 0;
            }
            var data = System.Text.Encoding.UTF8.GetBytes(new char[1] { c });
            int charIndex;
            int tableIndex;
            if (data[0] < 0x80)
            {
                charIndex = data[0];
                tableIndex = 0;
            }
            else
            {
                if (data[0] < 0xE0)
                {
                    charIndex = data[1] - 0x80;
                    tableIndex = (data[0] - 0xC1) / 2 + 1;
                }
                else
                {
                    throw new Exception("Character not supported.");
                }
            }
            foreach (var t in _font.Tables)
            {
                if (t.CharSet == tableIndex)
                {
                    table = t;
                    return charIndex - t.CharStart;
                }
            }
            foreach (var t in _font.Tables)
            {
                if (t.CharSet == 0)
                {
                    table = t;
                    return t.CharDefault;
                }
            }
            table = _font.Tables[0];
            return table.CharDefault;
        }
        private int LLDrawChar(Graphics g, FontTable table, int index, Rectangle dst, Color color)
        {
            if (_surfaces == null)
                return 0;
            if (!_surfaces.TryGetValue(table.Texture, out var surface))
                return 0;
            if (surface == null)
                return 0;
            
            var x = index % table.CharPerRow * _font.CharSets.MaximumWidth;
            var y = index / table.CharPerRow * _font.CharSets.MaximumHeight;
            /*if (_font.CharSet.MaximumWidth * 2 <= rect.Width)
                x -= 0.5f;*/
            var src = new Rectangle(x, y, _font.CharSets.MaximumWidth, _font.CharSets.MaximumHeight);
            _drawing.DrawSurface(surface, src, dst);
            return table.Spaces[index];
        }

        public void Dispose()
        {
            if (_surfaces != null)
            {
                foreach (var surface in _surfaces.Values)
                {
                    surface?.Dispose();
                }
            }
        }
    }
}

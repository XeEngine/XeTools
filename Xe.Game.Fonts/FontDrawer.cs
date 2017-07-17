using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Xe.Game.Fonts
{
    public class FontDrawer : Font, IDisposable
    {
        private Color mColor;
        private ImageAttributes mImgAttribute;

        public Color ForeColor
        {
            get { return mColor; }
            set
            {
                mColor = value;
                float r = mColor.R / 255.0f;
                float g = mColor.G / 255.0f;
                float b = mColor.B / 255.0f;
                float a = mColor.A / 255.0f;
                float[][] coeff = {
                            new float[] { r, 0, 0, 0, 0 },
                            new float[] { 0, g, 0, 0, 0 },
                            new float[] { 0, 0, b, 0, 0 },
                            new float[] { 0, 0, 0, a, 0 },
                            new float[] { 0, 0, 0, 0, 1 }};
                mImgAttribute = new ImageAttributes();
                mImgAttribute.SetColorMatrix(new ColorMatrix(coeff));
            }
        }

        public FontDrawer(string filename) : base(filename)
        {
            ForeColor = Color.White;
        }
        public FontDrawer(FileStream stream) : base(stream)
        {
            ForeColor = Color.White;
        }

        public static void SetNearestInterpolation(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }
        public void DrawString(Graphics g, string str, Rectangle bounds)
        {
            int x = 0, y = -CharSet.YOffset;
            ForeColor = Color.White;
            for (int i = 0; i < str.Length; i++)
            {
                int index, width;
                Table table;
                char c = str[i];
                switch(c)
                {
                    case '\r':
                        x = 0;
                        break;
                    case '\n':
                        x = 0;
                        y += CharSet.Height;
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
                                            int hexcolor;
                                            if (int.TryParse(cmd[1], NumberStyles.HexNumber, null, out hexcolor))
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
                                y += CharSet.Height;
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
            Table table;
            int index = ProcessCharacter(c, out table);
            return DrawChar(g, table, index, x, y, color);
        }
        public int DrawChar(Graphics g, char c, int x, int y, int width, int height, Color color)
        {
            return DrawChar(g, c, new Rectangle(x, y, width, height), color);
        }
        public int DrawChar(Graphics g, char c, Rectangle rect, Color color)
        {
            Table table;
            int index = ProcessCharacter(c, out table);
            return DrawChar(g, table, index, rect, color);
        }
        public int DrawChar(Graphics g, Table table, int index, int x, int y, Color color)
        {
            return LLDrawChar(g, table, index, new Rectangle(x, y, CharSet.MaximumWidth, CharSet.MaximumHeight), color);
        }
        public int DrawChar(Graphics g, Table table, int index, int x, int y, int width, int height, Color color)
        {
            return DrawChar(g, table, index, new Rectangle(x, y, width, height), color);
        }
        public int DrawChar(Graphics g, Table table, int index, Rectangle rect, Color color)
        {
            int w = rect.Width;
            int h = rect.Height;
            float fx = w / (float)CharSet.MaximumWidth;
            float fh = h / (float)CharSet.MaximumHeight;
            float zoom = Math.Min(fx, fh);
            rect.Width = (int)Math.Round(CharSet.MaximumWidth * zoom);
            rect.Height = (int)Math.Round(CharSet.MaximumHeight * zoom);
            return LLDrawChar(g, table, index, rect, color);
        }
        
        private int ProcessCharacter(char c, out Table table)
        {
            if (Table.Count <= 0)
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
            foreach (var t in Table)
            {
                if (t.CharSet == tableIndex)
                {
                    table = t;
                    return charIndex - t.CharStart;
                }
            }
            foreach (var t in Table)
            {
                if (t.CharSet == 0)
                {
                    table = t;
                    return t.CharDefault;
                }
            }
            table = Table[0];
            return table.CharDefault;
        }
        private int LLDrawChar(Graphics g, Table table, int index, Rectangle rect, Color color)
        {
            if (table.Texture == null) return 0;
            float x = index % table.CharPerRow * CharSet.MaximumWidth;
            float y = index / table.CharPerRow * CharSet.MaximumHeight;
            if (CharSet.MaximumWidth * 2 <= rect.Width)
                x -= 0.5f;
            g.DrawImage(table.Texture, rect, x, y, CharSet.MaximumWidth, CharSet.MaximumHeight, GraphicsUnit.Pixel, mImgAttribute);
            return table.Spaces[index];
        }

        public void Dispose()
        {
            if (mImgAttribute != null) mImgAttribute.Dispose();
        }
    }
}

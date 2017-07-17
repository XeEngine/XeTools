﻿using System.Collections.Generic;
using System.IO;

namespace Xe.Game.Fonts
{
    public class Font
    {
        public string FontName { get; set; }

        public FontCharset CharSets { get; set; } = new FontCharset();

        public List<FontTable> Tables { get; set; } = new List<FontTable>();
    }
}

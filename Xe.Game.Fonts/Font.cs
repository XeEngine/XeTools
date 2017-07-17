using System.Collections.Generic;
using System.IO;

namespace Xe.Game.Fonts
{
    public class Font
    {
        public string FontName { get; set; }

        public Charset CharSet { get; set; } = new Charset();

        public List<Table> Table { get; set; } = new List<Table>();
    }
}

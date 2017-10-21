using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Tiled
{
    public class Tile
    {
        private XElement _xElement;

        public Tile(XElement xElement)
        {
            _xElement = xElement;
        }
    }
}

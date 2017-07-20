using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game
{
    /// <summary>
    /// Represent how a single texture is handled
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Object's unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// File name of the texture
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of colors that will turn into transparency
        /// </summary>
        public Color[] Transparencies { get; set; }

        /// <summary>
        /// If the palette cannot be touched during image processing
        /// </summary>
        public bool MaintainPaletteOrder { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Tilemap
{
    public interface ITile
    {
        /// <summary>
        /// Tileset index.
        /// </summary>
        int Tileset { get; }

        /// <summary>
        /// Tile index inside the specified tileset index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Palette index.
        /// </summary>
        int PaletteIndex { get; }

        /// <summary>
        /// If the tile is filipped horizontally
        /// </summary>
        bool IsFlippedX { get; }

        /// <summary>
        /// If the tile is flipped vertically
        /// </summary>
        bool IsFlippedY { get; }

        /// <summary>
        /// If the tile is draw on top.
        /// </summary>
        bool IsPriority { get; }
    }
}
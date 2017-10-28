﻿namespace Xe.Game.Tilemaps
{
    public interface ILayerTilemap : ILayerEntry
    {
        int Width { get; }

        int Height { get; }

        double Opacity { get; set; }

        ITile GetTile(int x, int y);
    }
}
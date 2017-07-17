using System.Drawing.Imaging;
using Xe.Game.Tilemaps;
using Xe.Tools.Tilemap;

namespace Xe.Tools.MapTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var map = new TilemapTiled(@"C:\Users\xeeynamo\Desktop\repo\vladya\soc\data\data\map\island_03.tmx");
            using (var drawing = Drawing.DrawingDirectX.Factory(
                map.Size.Width * map.TileSize.Width,
                map.Size.Height * map.TileSize.Height,
                PixelFormat.Format32bppArgb))
            {
                using (var mapDrawer = new TilemapDrawer(drawing, map))
                {
                    mapDrawer.DrawLayerIndex(drawing, 0);
                    mapDrawer.DrawLayerIndex(drawing, 1);
                    mapDrawer.DrawLayerIndex(drawing, 2);
                    mapDrawer.DrawLayerIndex(drawing, 3);
                    mapDrawer.DrawLayerIndex(drawing, 4);
                    mapDrawer.DrawLayerIndex(drawing, 5);
                }
                drawing.Surface.Save(@"C:\Users\xeeynamo\Desktop\repo\out.png");
            }

        }
    }
}

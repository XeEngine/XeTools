using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Tilemap;

namespace Xe.Tools.MapTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Configurator.Configurator.Initialize();
            var map = new TilemapTiled(@"D:\Xe\Repo\vladya\soc\data\data\map\island_03.tmx");
            using (var drawing = Factory.Resolve<IDrawing>())
            {
                using (var surface = drawing.CreateSurface(map.Size.Width * map.TileSize.Width,
                    map.Size.Height * map.TileSize.Height,
                    PixelFormat.Format32bppArgb))
                {
                    drawing.Surface = surface;
                    using (var mapDrawer = new TilemapDrawer(drawing, map))
                    {
                        mapDrawer.DrawLayerIndex(drawing, 0);
                        mapDrawer.DrawLayerIndex(drawing, 1);
                        mapDrawer.DrawLayerIndex(drawing, 2);
                        mapDrawer.DrawLayerIndex(drawing, 3);
                        mapDrawer.DrawLayerIndex(drawing, 4);
                        mapDrawer.DrawLayerIndex(drawing, 5);
                    }
                    drawing.Surface.Save(@"D:\out.png");
                    drawing.Surface = null;
                }
            }

        }
    }
}

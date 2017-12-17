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
            var map = new TilemapTiled().Open(@"D:\Xe\Repo\vladya\soc\data\data\map\island_03.tmx", new ObjectExtensionDefinition[] { });
            using (var drawing = Factory.Resolve<IDrawing>())
            {
                using (var surface = drawing.CreateSurface(map.Size.Width * map.TileSize.Width,
                    map.Size.Height * map.TileSize.Height,
                    PixelFormat.Format32bppArgb, SurfaceType.Output))
                {
                    drawing.Surface = surface;
                    using (var mapDrawer = new TilemapDrawer(drawing))
                    {
                        mapDrawer.Map = map;
                        mapDrawer.DrawMap(new System.Drawing.RectangleF(0, 0, float.MaxValue, float.MaxValue));
                    }
                    drawing.Surface.Save(@"D:\out.png");
                    drawing.Surface = null;
                }
            }

        }
    }
}

using Xe;

namespace Xe.Tools.Configurator
{
    public static class Configurator
    {
        public static void Initialize()
        {
            RegisterServices();
        }

        private static void RegisterServices()
        {
            Factory.Register<Drawing.IDrawing, Drawing.DrawingDirectX>(Factory.Scope.Lifetime);
        }
    }
}

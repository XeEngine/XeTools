using System;
using static Xe.Drawing.Helpers;

namespace Xe.Drawing
{
    using SharpDX;
    using SharpDX.IO;
    // use namespaces shortcuts to reduce typing and avoid the messing the same class names from different namespaces
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;
    using dw = SharpDX.DirectWrite;
    using System.Drawing.Imaging;

    public partial class DrawingDirectX
    {
        private static d2.PixelFormat d2PixelFormat = new d2.PixelFormat(dxgi.Format.B8G8R8A8_UNorm, d2.AlphaMode.Premultiplied);
        private static Guid wicPixelFormat = wic.PixelFormat.Format32bppPBGRA;

        private d2.Device d2dDevice;
        private d2.DeviceContext d2dContext;

        private void Initialize()
        {
            d2dDevice = device.GetD2DDevice();
            d2dContext = new d2.DeviceContext(d2dDevice, d2.DeviceContextOptions.None);
        }

        public void ResizeRenderTarget(int width, int height)
        {
            _surface?.Dispose();
            _surface = CreateSurfaceAsRenderTarget(width, height);
            d2dContext.Target = _surface.Bitmap;
        }
    }
}

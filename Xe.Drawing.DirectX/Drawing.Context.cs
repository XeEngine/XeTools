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
        private d2.Device d2dDevice;
        private d2.DeviceContext d2dContext;
        private d2.PixelFormat d2PixelFormat;
        private Guid wicPixelFormat;

        private void Initialize()
        {
            d2dDevice = device.Create2dDevice();
            d2dContext = new d2.DeviceContext(d2dDevice, d2.DeviceContextOptions.None);
            
            // specify a pixel format that is supported by both D2D and WIC
            d2PixelFormat = new d2.PixelFormat(dxgi.Format.R8G8B8A8_UNorm, d2.AlphaMode.Premultiplied);
            // if in D2D was specified an R-G-B-A format - use the same for wic
            wicPixelFormat = wic.PixelFormat.Format32bppPRGBA;
        }
        private void CreateRenderTarget(int width, int height, PixelFormat pixelFormat)
        {
            _surface?.Dispose();
            _surface = CreateSurfaceAsRenderTarget(width, height, pixelFormat) as CSurface;
            d2dContext.Target = _surface.Bitmap;
        }
    }
}

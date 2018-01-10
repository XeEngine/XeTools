using System.Drawing;

namespace Xe.Drawing
{
    using dx = SharpDX;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;

    public partial class DrawingDirect3D : Drawing
    {
        private Filter _filter = Filter.Nearest;
        private d3d.RenderTargetView _renderTarget;
        private CSurface _dstSurface;
        private SizeF _viewportSize;

        public override ISurface Surface
        {
            get => _dstSurface;
            set
            {
                if (value is CSurface surface)
                {
                    _renderTarget?.Dispose();
                    _renderTarget = new d3d.RenderTargetView(Device, surface.Texture);
                    Context.OutputMerger.SetRenderTargets(_renderTarget);
                    _dstSurface = surface;

                    var viewport = new dx.Viewport(0, 0, surface.Width, surface.Height);
                    Context.Rasterizer.SetViewport(viewport);
                    _viewportSize = new SizeF(surface.Width, surface.Height);
                }
                else
                {
                    _renderTarget?.Dispose();
                    _renderTarget = null;
                    Context.OutputMerger.SetRenderTargets(_renderTarget);
                }
            }
        }
        public override Filter Filter
        {
            get => _filter;
            set => _filter = value;
        }

        public override void Clear(Color color)
        {
            if (_renderTarget != null)
            {
                Context.ClearRenderTargetView(_renderTarget, new SharpDX.Mathematics.Interop.RawColor4()
                {
                    R = color.R / 255.0f,
                    G = color.G / 255.0f,
                    B = color.B / 255.0f,
                    A = color.A / 255.0f,
                });
            }
        }

        public override void Dispose()
        {
            _renderTarget?.Dispose();
            _device.Dispose();
        }
    }
}

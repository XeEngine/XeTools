namespace Xe.Drawing
{
    using dx = SharpDX;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System;
    using System.Security;

    public partial class DrawingDirect3D : Drawing
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong count);

        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public float X;
            public float Y;
            public float U;
            public float V;
            public float A;
        }


		public override void DrawRectangle(RectangleF rect, Color color, float width = 1)
		{
		}

		public override void FillRectangle(RectangleF rect, Color color)
		{
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip)
		{
			DrawSurface(surface, src, dst, 1.0f, flip);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip)
		{
			var size = surface.Size;
			SetTextureToDraw(surface);

			EnqueueVertex(new Vertex()
			{
				X = dst.Left / _viewportSize.Width * +2.0f - 1.0f,
				Y = dst.Top / _viewportSize.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Top / size.Height,
				A = alpha,
			});
			EnqueueVertex(new Vertex()
			{
				X = dst.Right / _viewportSize.Width * +2.0f - 1.0f,
				Y = dst.Top / _viewportSize.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Top / size.Height,
				A = alpha,
			});
			EnqueueVertex(new Vertex()
			{
				X = dst.Left / _viewportSize.Width * +2.0f - 1.0f,
				Y = dst.Bottom / _viewportSize.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Bottom / size.Height,
				A = alpha,
			});
			EnqueueVertex(new Vertex()
			{
				X = dst.Right / _viewportSize.Width * +2.0f - 1.0f,
				Y = dst.Bottom / _viewportSize.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Bottom / size.Height,
				A = alpha,
			});
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			DrawSurface(surface, src, dst, color.A, flip);
		}

		private CSurface _prevSurface;
        private void SetTextureToDraw(ISurface surface)
        {
            if (_prevSurface != surface)
            {
                Flush();
                var internalSurface = surface as CSurface;
                Context.PixelShader.SetShaderResource(0, internalSurface?.ShaderResourceView);
                _prevSurface = internalSurface;
            }
        }

        private const int MaximumQuadsCount = 65536;
        private d3d.Buffer _vertexBuffer;
        private d3d.Buffer _indexBuffer;
        private Vertex[] _dataBuffer = new Vertex[MaximumQuadsCount];
        private int _pendingVerticesCount;
		
        private void EnqueueVertex(Vertex vertex)
        {
            if (_pendingVerticesCount + 1 >= MaximumQuadsCount)
                Flush();
            _dataBuffer[_pendingVerticesCount++] = vertex;
        }
        private void Flush()
        {
            if (_pendingVerticesCount == 0)
                return;

            if (_vertexBuffer == null)
            {
                var vertexBufferDesc = new d3d.BufferDescription()
                {
                    SizeInBytes = MaximumQuadsCount * dx.Utilities.SizeOf<Vertex>(),
                    Usage = d3d.ResourceUsage.Dynamic,
                    BindFlags = d3d.BindFlags.VertexBuffer,
                    CpuAccessFlags = d3d.CpuAccessFlags.Write,
                    OptionFlags = 0,
                    StructureByteStride = 0
                };
                _vertexBuffer = d3d.Buffer.Create(Device, _dataBuffer, vertexBufferDesc);
                Context.InputAssembler.SetVertexBuffers(0, new d3d.VertexBufferBinding(_vertexBuffer, dx.Utilities.SizeOf<Vertex>(), 0));

                var indexBufferDesc = new d3d.BufferDescription()
                {
                    SizeInBytes = MaximumQuadsCount * 6 * sizeof(ushort),
                    Usage = d3d.ResourceUsage.Immutable,
                    BindFlags = d3d.BindFlags.IndexBuffer,
                    CpuAccessFlags = d3d.CpuAccessFlags.None,
                    OptionFlags = 0,
                    StructureByteStride = 0
                };
                uint[] indices = new uint[MaximumQuadsCount * 6];
                for (uint i = 0, nx = 0x00000000; i < MaximumQuadsCount * 6; nx += 4)
                {
                    indices[i++] = nx + 1;
                    indices[i++] = nx + 0;
                    indices[i++] = nx + 2;
                    indices[i++] = nx + 1;
                    indices[i++] = nx + 2;
                    indices[i++] = nx + 3;
                }
                _indexBuffer = d3d.Buffer.Create(Device, indices, indexBufferDesc);
                Context.InputAssembler.SetIndexBuffer(_indexBuffer, dxgi.Format.R32_UInt, 0);
            }
            else
            {
				var size = _pendingVerticesCount * dx.Utilities.SizeOf<Vertex>();
				var dataBox = Context.MapSubresource(_vertexBuffer, 0, d3d.MapMode.WriteDiscard, d3d.MapFlags.None);
                IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(_dataBuffer, 0);
                CopyMemory(dataBox.DataPointer, addr, (ulong)size);
                Context.UnmapSubresource(_vertexBuffer, 0);
            }
            
            Context.DrawIndexed(_pendingVerticesCount / 4 * 6, 0, 0);
            Context.Flush();
            _pendingVerticesCount = 0;
        }
    }
}

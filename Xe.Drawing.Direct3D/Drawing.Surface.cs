﻿using System;
using System.Drawing;
using System.Linq;

namespace Xe.Drawing
{
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;

    public partial class DrawingDirect3D
    {
        private static class TextureLoader
        {
            /// <summary>
            /// Loads a bitmap using WIC.
            /// </summary>
            /// <param name="deviceManager"></param>
            /// <param name="filename"></param>
            /// <returns></returns>
            public static wic.BitmapSource LoadBitmap(wic.ImagingFactory2 factory, string filename)
            {
                var bitmapDecoder = new wic.BitmapDecoder(
                    factory,
                    filename,
                    wic.DecodeOptions.CacheOnDemand
                    );

                var formatConverter = new wic.FormatConverter(factory);

                formatConverter.Initialize(
                    bitmapDecoder.GetFrame(0),
                    wic.PixelFormat.Format32bppPRGBA,
                    wic.BitmapDitherType.None,
                    null,
                    0.0,
                    wic.BitmapPaletteType.Custom);

                return formatConverter;
            }

            /// <summary>
            /// Creates a <see cref="d3d.Texture2D"/> from a WIC <see cref="BitmapSource"/>
            /// </summary>
            /// <param name="device">The Direct3D11 device</param>
            /// <param name="bitmapSource">The WIC bitmap source</param>
            /// <returns>A Texture2D</returns>
            public static d3d.Texture2D CreateTexture2DFromBitmap(d3d.Device device,
                wic.BitmapSource bitmapSource, Color[] filterColors = null)
            {
                // Allocate DataStream to receive the WIC image pixels
                int stride = bitmapSource.Size.Width * 4;
                using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
                {
                    // Copy the content of the WIC to the buffer
                    bitmapSource.CopyPixels(stride, buffer);
                    if (filterColors.Length > 0)
                    {
                        Xe.Tools.Services.ImageService.MakeTransparent_Bgra32(buffer.DataPointer, stride,
                            bitmapSource.Size.Height, filterColors
                            .Select(x => new Xe.Tools.Services.Color()
                            {
                                a = x.A,
                                r = x.R,
                                g = x.G,
                                b = x.B
                            })
                            .ToArray()
                        );
                    }
                    return new d3d.Texture2D(device, new d3d.Texture2DDescription()
                    {
                        Width = bitmapSource.Size.Width,
                        Height = bitmapSource.Size.Height,
                        ArraySize = 1,
                        BindFlags = d3d.BindFlags.ShaderResource,
                        Usage = d3d.ResourceUsage.Immutable,
                        CpuAccessFlags = d3d.CpuAccessFlags.None,
                        Format = dxgi.Format.R8G8B8A8_UNorm,
                        MipLevels = 1,
                        OptionFlags = d3d.ResourceOptionFlags.None,
                        SampleDescription = new dxgi.SampleDescription(1, 0),
                    }, new SharpDX.DataRectangle(buffer.DataPointer, stride));
                }
            }
        }

        public class MappedResource : IMappedResource
        {
            public MappedResource(d3d.Texture2D texture)
            {
                var dataBox = texture.Device.ImmediateContext.MapSubresource(texture, 0,
                    d3d.MapMode.Read, d3d.MapFlags.None, out var DataStream);
                Texture = texture;
                Stride = dataBox.RowPitch;
                Length = dataBox.SlicePitch;
                Data = dataBox.DataPointer;
            }

            public d3d.Texture2D Texture { get; }

            public SharpDX.DataStream DataStream { get; }

            public IntPtr Data { get; }

            public int Stride { get; }

            public int Length { get; }

            public void Dispose()
            {
                DataStream?.Dispose();
                Texture.Device.ImmediateContext.UnmapSubresource(Texture, 0);
            }
        }


        private class CSurface : ISurface
        {
            private d3d.Texture2D _backTexture;

            internal d3d.Texture2D Texture { get; }
            internal d3d.ShaderResourceView ShaderResourceView { get; }

            public int Width => Texture.Description.Width;

            public int Height => Texture.Description.Height;

            public Size Size => new Size(Width, Height);

            public PixelFormat PixelFormat => PixelFormat.Undefined;

            public CSurface(d3d.Texture2D texture, d3d.ShaderResourceView shaderResourceView)
            {
                Texture = texture;
                ShaderResourceView = shaderResourceView;
            }

            public void Dispose()
            {
				_backTexture.Dispose();
				ShaderResourceView.Dispose();
                Texture.Dispose();
            }

            public IMappedResource Map()
            {
                if (_backTexture == null ||
                    _backTexture.Description.Width != Texture.Description.Width ||
                    _backTexture.Description.Height != Texture.Description.Height)
                {
                    var desc = new d3d.Texture2DDescription
                    {
                        Width = Texture.Description.Width,
                        Height = Texture.Description.Height,
                        MipLevels = Texture.Description.MipLevels,
                        ArraySize = Texture.Description.ArraySize,
                        Format = Texture.Description.Format,
                        Usage = d3d.ResourceUsage.Staging,
                        BindFlags = d3d.BindFlags.None,
                        CpuAccessFlags = d3d.CpuAccessFlags.Read,
                        OptionFlags = d3d.ResourceOptionFlags.None,
                        SampleDescription = new dxgi.SampleDescription(1, 0)
                    };
                    _backTexture?.Dispose();
                    _backTexture = new d3d.Texture2D(Texture.Device, desc);
                }
                Texture.Device.ImmediateContext.CopyResource(Texture, _backTexture);
                return new MappedResource(_backTexture);
            }

            public void Save(string filename)
            {
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type)
        {
            var desc = new d3d.Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = dxgi.Format.B8G8R8A8_UNorm,
                CpuAccessFlags = d3d.CpuAccessFlags.None,
                OptionFlags = d3d.ResourceOptionFlags.None,
                SampleDescription = new dxgi.SampleDescription(1, 0)
            };

            switch (type)
            {
                case SurfaceType.Input:
                    desc.Usage = d3d.ResourceUsage.Immutable;
                    desc.BindFlags = d3d.BindFlags.ShaderResource;
                    break;
                case SurfaceType.Output:
                    desc.Usage = d3d.ResourceUsage.Default;
                    desc.BindFlags = d3d.BindFlags.RenderTarget;
                    break;
                case SurfaceType.InputOutput:
                    desc.Usage = d3d.ResourceUsage.Default;
                    desc.BindFlags = d3d.BindFlags.ShaderResource | d3d.BindFlags.RenderTarget;
                    break;
            }

            var texture = new d3d.Texture2D(Device, desc);
            var shaderResourceView = new d3d.ShaderResourceView(Device, texture);
            return new CSurface(texture, shaderResourceView);
        }

        public override ISurface CreateSurface(string filename, Color[] filterColors = null)
        {
            var bitmap = TextureLoader.LoadBitmap(new wic.ImagingFactory2(), filename);
            var texture = TextureLoader.CreateTexture2DFromBitmap(Device, bitmap, filterColors);
            d3d.ShaderResourceView shaderResourceView = new d3d.ShaderResourceView(Device, texture);
            return new CSurface(texture, shaderResourceView);
        }
    }
}

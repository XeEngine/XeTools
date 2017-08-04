﻿using SharpDX.Direct3D9;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Interop;

namespace Xe.Tools.Wpf.Controls
{
    public abstract partial class D2DControl
    {
        class DX11ImageSource : D3DImage, IDisposable
        {
            #region attributes

            [DllImport("user32.dll", SetLastError = false)]
            public static extern IntPtr GetDesktopWindow();

            private static int ActiveClients;
            private static Direct3DEx D3DContext;
            private static DeviceEx D3DDevice;

            private Texture renderTarget;

            #endregion

            #region properties

            public int RenderWait { get; set; } = 2; // default: 2ms

            #endregion

            #region public methods

            public DX11ImageSource()
            {
                Initialize();
                ActiveClients++;
            }

            public void Dispose()
            {
                SetRenderTarget(null);

                Utility.SafeDispose(ref renderTarget);

                ActiveClients--;
                Destroy();
            }

            public void InvalidateD3DImage()
            {
                if (renderTarget != null)
                {
                    Lock();
                    if (RenderWait != 0)
                    {
                        Thread.Sleep(RenderWait);
                    }
                    AddDirtyRect(new System.Windows.Int32Rect(0, 0, base.PixelWidth, base.PixelHeight));
                    Unlock();
                }
            }

            public void SetRenderTarget(SharpDX.Direct3D11.Texture2D target)
            {
                if (renderTarget != null)
                {
                    renderTarget = null;

                    Lock();
                    SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
                    Unlock();
                }

                if (target == null)
                {
                    return;
                }

                var format = TranslateFormat(target);
                var handle = GetSharedHandle(target);

                if (!IsShareable(target))
                {
                    throw new ArgumentException("Texture must be created with ResouceOptionFlags.Shared");
                }

                if (format == Format.Unknown)
                {
                    throw new ArgumentException("Texture format is not compatible with OpenSharedResouce");
                }

                if (handle == IntPtr.Zero)
                {
                    throw new ArgumentException("Invalid handle");
                }

                renderTarget = new Texture(D3DDevice, target.Description.Width, target.Description.Height, 1, Usage.RenderTarget, format, Pool.Default, ref handle);

                using (var surface = renderTarget.GetSurfaceLevel(0))
                {
                    Lock();
                    SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                    Unlock();
                }
            }

            #endregion

            #region private methods

            private void Initialize()
            {
                if (ActiveClients <= 0)
                {
                    var presentParams = GetPresentParameters();
                    var createFlags = CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve;

                    D3DContext = new Direct3DEx();
                    D3DDevice = new DeviceEx(D3DContext, 0, DeviceType.Hardware, IntPtr.Zero, createFlags, presentParams);
                }
            }

            private void Destroy()
            {
                if (ActiveClients <= 0)
                {
                    Utility.SafeDispose(ref renderTarget);
                    Utility.SafeDispose(ref D3DDevice);
                    Utility.SafeDispose(ref D3DContext);
                }
            }

            private static void ResetD3D()
            {
                if (ActiveClients > 0)
                {
                    var presentParams = GetPresentParameters();
                    D3DDevice.ResetEx(ref presentParams);
                }
            }

            private static PresentParameters GetPresentParameters()
            {
                var presentParams = new PresentParameters()
                {
                    Windowed = true,
                    SwapEffect = SwapEffect.Discard,
                    DeviceWindowHandle = GetDesktopWindow(),
                    PresentationInterval = PresentInterval.Default
                };
                return presentParams;
            }

            private IntPtr GetSharedHandle(SharpDX.Direct3D11.Texture2D texture)
            {
                using (var resource = texture.QueryInterface<SharpDX.DXGI.Resource>())
                {
                    return resource.SharedHandle;
                }
            }

            private static Format TranslateFormat(SharpDX.Direct3D11.Texture2D texture)
            {
                switch (texture.Description.Format)
                {
                    case SharpDX.DXGI.Format.R10G10B10A2_UNorm: return Format.A2B10G10R10;
                    case SharpDX.DXGI.Format.R16G16B16A16_Float: return Format.A16B16G16R16F;
                    case SharpDX.DXGI.Format.B8G8R8A8_UNorm: return Format.A8R8G8B8;
                    default: return Format.Unknown;
                }
            }

            private static bool IsShareable(SharpDX.Direct3D11.Texture2D texture)
            {
                return (texture.Description.OptionFlags & SharpDX.Direct3D11.ResourceOptionFlags.Shared) != 0;
            }

            #endregion
        }
    }
}

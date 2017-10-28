﻿using System;

namespace Xe.Drawing
{
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;

    public partial class DrawingDirectX
    {
        private static Device device = new Device();

        private class Device : IDisposable
        {
            private d3d.Device d3dDevice;
            private d3d.Device1 d3dDevice1;
            private dxgi.Device dxgiDevice;
            private d2.Device d2dDevice;

            internal wic.ImagingFactory2 ImagingFactory { get; private set; }

            internal Device()
            {
                var flags = d3d.DeviceCreationFlags.VideoSupport |
                    d3d.DeviceCreationFlags.BgraSupport;
                d3dDevice = new d3d.Device(SharpDX.Direct3D.DriverType.Hardware, flags);
                d3dDevice1 = d3dDevice.QueryInterface<d3d.Device1>();
                dxgiDevice = d3dDevice.QueryInterface<dxgi.Device>();
                d2dDevice = new d2.Device(dxgiDevice);

                ImagingFactory = new wic.ImagingFactory2();
            }

            internal d2.Device GetD2DDevice()
            {
                return d2dDevice;
            }

            public void Dispose()
            {
                d2dDevice.Dispose();
                dxgiDevice.Dispose();
                d3dDevice1.Dispose();
                d3dDevice.Dispose();
            }
        }
    }
}

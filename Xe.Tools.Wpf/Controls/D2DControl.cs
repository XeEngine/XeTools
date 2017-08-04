using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Xe.Tools.Wpf.Controls
{
    public abstract partial class D2DControl : System.Windows.Controls.Image
    {
        #region dependency properties

        private static readonly DependencyPropertyKey FpsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Fps",
            typeof(int),
            typeof(D2DControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None)
            );

        public static DependencyProperty RenderWaitProperty = DependencyProperty.Register(
            "RenderWait",
            typeof(int),
            typeof(D2DControl),
            new FrameworkPropertyMetadata(2, OnRenderWaitChanged)
            );

        public static readonly DependencyProperty FpsProperty = FpsPropertyKey.DependencyProperty;

        #endregion

        #region attributes
                
        // Direct3D Objects
        private SharpDX.Direct3D11.Device d3dDevice;
        private SharpDX.Direct3D11.DeviceContext1 d3dContext;
        private Texture2D d3dRenderTarget;
        private SharpDX.Direct3D.FeatureLevel featureLevel;
        private float dpi;

        // Direct2D Objects
        private SharpDX.Direct2D1.Factory2 d2dFactory;
        private SharpDX.Direct2D1.Device1 d2dDevice;
        private SharpDX.Direct2D1.DeviceContext1 d2dContext;

        // DirectWrite & Windows Imaging Component Objects
        private SharpDX.DirectWrite.Factory dwriteFactory;
        private SharpDX.WIC.ImagingFactory2 imagingFactory;

        private DX11ImageSource d3DSurface;
        private readonly Stopwatch renderTimer = new Stopwatch();
        private Direct2DResourceCache resourceCache = new Direct2DResourceCache();

        // General attributes
        private long lastFrameTime = 0;
        private long lastRenderTime = 0;
        private int frameCount = 0;
        private int frameCountHistTotal = 0;
        private Queue<int> frameCountHist = new Queue<int>();

        #endregion

        #region properties

        /// <summary>
        /// Check if the control is currently shown in Visual Studio designer.
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                var isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }

        protected SharpDX.Direct3D11.Device Device3D => d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1 Context3D => d3dContext;
        protected SharpDX.Direct2D1.Device Device2D => d2dDevice;
        protected SharpDX.Direct2D1.DeviceContext1 Context2D => d2dContext;
        protected SharpDX.WIC.ImagingFactory2 ImagingFactory => imagingFactory;
        protected Direct2DResourceCache ResourceCache => resourceCache;
        public SharpDX.Direct3D.FeatureLevel FeatureLevel => featureLevel;
        
        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <remarks>
        /// This method will fire the event <see cref="OnDpiChanged"/>
        /// if the dpi is modified.
        /// </remarks>
        public virtual float Dpi
        {
            get => dpi;
            set
            {
                if (dpi != value)
                {
                    dpi = value;
                    Context2D.DotsPerInch = new Size2F(dpi, dpi);
                    OnDpiChanged?.Invoke(this);
                }
            }
        }

        public int Fps
        {
            get { return (int)GetValue(FpsProperty); }
            protected set { SetValue(FpsPropertyKey, value); }
        }

        public int RenderWait
        {
            get { return (int)GetValue(RenderWaitProperty); }
            set { SetValue(RenderWaitProperty, value); }
        }

        /// <summary>
        /// This event is fired when the <see cref="Dpi"/> is called,
        /// </summary>
        public event Action<D2DControl> OnDpiChanged;

        /// <summary>
        /// This event is fired when the control is initialized.
        /// </summary>
        public event Action<D2DControl> OnInitialize;

        /// <summary>
        /// This event is fired when the control is going to be destroyed.
        /// </summary>
        public event Action<D2DControl> OnDestroy;

        #endregion

        #region public methods

        public D2DControl()
        {
            CreateDeviceIndependentResources();

            Loaded += Window_Loaded;
            Unloaded += Window_Closing;

            Stretch = System.Windows.Media.Stretch.Fill;
        }

        public abstract void Render(RenderTarget target);

        #endregion

        #region event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsInDesignMode)
            {
                Initialize();
                StartRendering();
            }
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                StopRendering();
                Destroy();
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (renderTimer.IsRunning)
            {
                PrepareAndCallRender();
                d3DSurface.InvalidateD3DImage();

                lastRenderTime = renderTimer.ElapsedMilliseconds;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            CreateRenderTarget();
            base.OnRenderSizeChanged(sizeInfo);
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (d3DSurface.IsFrontBufferAvailable)
            {
                StartRendering();
            }
            else
            {
                StopRendering();
            }
        }

        private static void OnRenderWaitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as D2DControl;
            control.d3DSurface.RenderWait = (int)e.NewValue;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Creates device independent resources.
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        private void CreateDeviceIndependentResources()
        {
#if DEBUG
            var debugLevel = DebugLevel.Information;
#else
            var debugLevel = DebugLevel.None;
#endif
            DestroyDeviceIndependentResources();

            // Allocate new references
            d2dFactory = new SharpDX.Direct2D1.Factory2(FactoryType.SingleThreaded, debugLevel);
            dwriteFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
            imagingFactory = new SharpDX.WIC.ImagingFactory2();
        }

        /// <summary>
        /// Creates device resources. 
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        private void CreateDeviceResources()
        {
            DestroyDeviceResources();
            // Allocate new references
            // Enable compatibility with Direct2D
            // Retrieve the Direct3D 11.1 device amd device context
            var creationFlags = DeviceCreationFlags.VideoSupport | DeviceCreationFlags.BgraSupport;

            try
            {
                // Try to create it with Video Support
                // If it is not working, we just use BGRA
                // Force to FeatureLevel.Level_9_1
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
            }
            catch
            {
                creationFlags = DeviceCreationFlags.BgraSupport;
                using (var defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, creationFlags))
                    d3dDevice = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
            }
            featureLevel = d3dDevice.FeatureLevel;

            // Get Direct3D 11.1 context
            d3dContext = d3dDevice.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext1>();

            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
                d2dDevice = new SharpDX.Direct2D1.Device1(d2dFactory, dxgiDevice);

            // Create Direct2D context
            d2dContext = new SharpDX.Direct2D1.DeviceContext1(d2dDevice, DeviceContextOptions.None);
        }

        protected void DestroyDeviceIndependentResources()
        {
            // Dispose previous references and set to null
            Utilities.Dispose(ref d2dFactory);
            Utilities.Dispose(ref dwriteFactory);
            Utilities.Dispose(ref imagingFactory);
        }

        protected void DestroyDeviceResources()
        {
            // Dispose previous references and set to null
            Utilities.Dispose(ref d3dDevice);
            Utilities.Dispose(ref d3dContext);
            Utilities.Dispose(ref d2dDevice);
            Utilities.Dispose(ref d2dContext);
        }

        private void Initialize()
        {
            CreateDeviceResources();

            d3DSurface = new DX11ImageSource();
            d3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            CreateRenderTarget();

            base.Source = d3DSurface;

            OnInitialize?.Invoke(this);
        }

        private void Destroy()
        {
            OnDestroy?.Invoke(this);

            DestroyDeviceIndependentResources();
            DestroyDeviceResources();

            d3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            base.Source = null;
            
            Utility.SafeDispose(ref d3DSurface);
            Utility.SafeDispose(ref d3dRenderTarget);
        }

        private void CreateRenderTarget()
        {
            d3DSurface.SetRenderTarget(null);
            
            Utility.SafeDispose(ref d3dRenderTarget);

            var width = Math.Max((int)ActualWidth, 16);
            var height = Math.Max((int)ActualHeight, 16);
            
            d3dRenderTarget = new Texture2D(d3dDevice, new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            });

            var surface = d3dRenderTarget.QueryInterface<Surface>();

            var renderTargetProperties = new RenderTargetProperties()
            {
                Type = RenderTargetType.Default,
                PixelFormat = new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                DpiX = 96.0f,
                DpiY = 96.0f,
                Usage = RenderTargetUsage.None,
                MinLevel = SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT
            };
            resourceCache.RenderTarget = d2dContext;
            d2dContext.Target = new Bitmap1(d2dContext, surface, new BitmapProperties1()
            {
                DpiX = renderTargetProperties.DpiX,
                DpiY = renderTargetProperties.DpiY,
                PixelFormat = renderTargetProperties.PixelFormat,
                BitmapOptions = BitmapOptions.Target
            });

            d3DSurface.SetRenderTarget(d3dRenderTarget);

            d3dDevice.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height, 0.0f, 1.0f);
        }

        private void StartRendering()
        {
            if (!renderTimer.IsRunning)
            {
                System.Windows.Media.CompositionTarget.Rendering += OnRendering;
                renderTimer.Start();
            }
        }

        private void StopRendering()
        {
            if (renderTimer.IsRunning)
            {
                System.Windows.Media.CompositionTarget.Rendering -= OnRendering;
                renderTimer.Stop();
            }
        }

        private void PrepareAndCallRender()
        {
            if (d3dDevice != null)
            {
                d2dContext.BeginDraw();
                Render(d2dContext);
                d2dContext.EndDraw();

                CalculateFps();
                d3dDevice.ImmediateContext.Flush();
            }
        }

        private void CalculateFps()
        {
            frameCount++;
            if (renderTimer.ElapsedMilliseconds - lastFrameTime > 1000)
            {
                frameCountHist.Enqueue(frameCount);
                frameCountHistTotal += frameCount;
                if (frameCountHist.Count > 5)
                {
                    frameCountHistTotal -= frameCountHist.Dequeue();
                }

                Fps = frameCountHistTotal / frameCountHist.Count;

                frameCount = 0;
                lastFrameTime = renderTimer.ElapsedMilliseconds;
            }
        }

        #endregion
    }
}

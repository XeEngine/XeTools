using System;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Controls;
using SharpDX;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor.Controls
{
    public class FramePanel : D2DControl
    {
        private string _fileName;

        #region properties

        public event Action<Bitmap1, System.Windows.Size> OnTextureLoaded;

        public Frame Frame { get; set; }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                ResourceCache.Add("Frame", t => 
                {
                    object o;
                    if (t is DeviceContext1 context)
                    {
                        var texture = Context2D.LoadBitmap2D(ImagingFactory, _fileName);
                        var textureSize = texture.Size;
                        OnTextureLoaded?.Invoke(texture, new System.Windows.Size(textureSize.Width, textureSize.Height));
                        o = texture;
                    }
                    else
                        o = null;
                    return o;
                });
            }
        }

        #endregion properties

        public FramePanel()
        {
            OnInitialize += (sender) =>
            {
                // Initialization
            };
            OnDestroy += (sender) =>
            {
                // Destroy
                ResourceCache.Remove("Frame");
            };
        }

        public override void Render(RenderTarget target)
        {
            var clearColor = new RawColor4(0.0f, 0.0f, 0.0f, 0.0f);

            target.Clear(clearColor);
            if (ResourceCache.TryGetValue("Frame", out object o) && o is Bitmap bitmap)
            {
                if (Frame != null)
                {
                    float centerX = -Frame.CenterX + (float)ActualWidth / 2.0f;
                    float centerY = -Frame.CenterY + (float)ActualHeight / 2.0f;
                    var src = new RawRectangleF(Frame.Left, Frame.Top, Frame.Right, Frame.Bottom);
                    var dst = new RawRectangleF(centerX, centerY,
                        centerX + Frame.Right - Frame.Left, centerY + Frame.Bottom - Frame.Top);
                    target.DrawBitmap(bitmap, src, dst);
                }
                else
                {
                    target.DrawBitmap(bitmap, 0, 0, Flip.None);
                }
            }
        }
    }
}

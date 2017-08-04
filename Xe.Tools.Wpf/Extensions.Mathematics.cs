using SharpDX.Mathematics.Interop;

namespace Xe.Tools.Wpf
{
    public static class ExtensionsMathematics
    {
        public static float GetWidth(this RawRectangleF rect)
        {
            return rect.Right - rect.Left;
        }
        public static float GetHeight(this RawRectangleF rect)
        {
            return rect.Bottom - rect.Top;
        }
    }
}

﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus : Drawing
    {
        private Graphics _graphics;
        private bool _invalidated;
        private CSurface _surface;
        private Filter _filter;

        public override ISurface Surface
        {
            get
            {
                if (_invalidated)
                {
                    _invalidated = false;
                    _graphics.Flush();
                }
                return _surface;
            }
        }

        public override Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                InterpolationMode interpolationMode;
                switch (value)
                {
                    case Filter.Nearest:
                        interpolationMode = InterpolationMode.NearestNeighbor;
                        break;
                    case Filter.Linear:
                        interpolationMode = InterpolationMode.Bilinear;
                        break;
                    case Filter.Cubic:
                        interpolationMode = InterpolationMode.Bicubic;
                        break;
                    default:
                        interpolationMode = InterpolationMode.Invalid;
                        break;
                }
                _graphics.InterpolationMode = interpolationMode;
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat)
        {
            using (var bitmap = new Bitmap(width, height, pixelFormat))
                return new CSurface(bitmap);
        }

        public override void Clear(Color color)
        {
            _graphics.Clear(color);
        }

        public override void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
        {
            var mySurface = surface as CSurface;
            if (mySurface == null)
                throw new ArgumentException("Invalid surface specified", nameof(surface));
            var bitmap = mySurface.Bitmap;
            switch (flip)
            {
                case Flip.None:
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    break;
                case Flip.FlipHorizontal:
                    _graphics.TranslateTransform(dst.Width, 0);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.TranslateTransform(-dst.Width, 0);
                    break;
                case Flip.FlipVertical:
                    _graphics.TranslateTransform(0, dst.Height);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.TranslateTransform(0, -dst.Height);
                    break;
                case Flip.FlipBoth:
                    _graphics.TranslateTransform(dst.Width, dst.Height);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.TranslateTransform(-dst.Width, -dst.Height);
                    break;
            }
            Invalidate();
        }

        public override void Dispose()
        {
            _surface.Dispose();
            _graphics.Dispose();
        }

        private DrawingGdiPlus(int width, int height, PixelFormat pixelFormat)
        {
            using (var bitmap = new Bitmap(width, height, pixelFormat))
            {
                _surface = CreateSurface(bitmap) as CSurface;
                _graphics = Graphics.FromImage(_surface.Bitmap);
            }
        }
        private DrawingGdiPlus(CSurface surface)
        {
            _surface = surface;
            _graphics = Graphics.FromImage(_surface.Bitmap);
        }

        private void Invalidate()
        {
            _invalidated = true;
        }

        public static DrawingGdiPlus Factory(int width, int height, PixelFormat pixelFormat)
        {
            return new DrawingGdiPlus(width, height, pixelFormat);
        }
        public static DrawingGdiPlus Factory(ISurface surface)
        {
            var mySurface = surface as CSurface;
            if (mySurface == null)
                throw new ArgumentException("Invalid surface specified", nameof(surface));
            return new DrawingGdiPlus(mySurface);
        }
    }
}
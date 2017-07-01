using libTools.Anim;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimEditor
{
    public partial class FrameList : ListView, IDisposable
	{
		private static readonly Color COLOR_TRANSPARENT = Color.FromArgb(0xFF, 0x00, 0xFF);
		private static readonly Color COLOR_CURRENT_SELECTION = Color.FromArgb(0xFF, 0x80, 0x00);
		private static readonly Color COLOR_SPECIAL_SELECTION = Color.FromArgb(0xFF, 0x20, 0x10);
		private static readonly Brush BRUSH_CURRENT_SELECTION = new SolidBrush(COLOR_CURRENT_SELECTION);
		private static readonly Brush BRUSH_SPECIAL_SELECTION = new SolidBrush(COLOR_SPECIAL_SELECTION);


		private Bitmap _spriteSheet;
        private Dictionary<string, Frame> _currentFrameDictionary;
        private IList _currentList;

		private int _specialSelection;

        public IList CurrentList
        {
            get { return _currentList; }
            set
            {
                _currentList = value;
                Items.Clear();
                if (_currentList != null)
                {
                    foreach (var item in value)
                        Items.Add(item.ToString());
                }
            }
        }
        public Dictionary<string, Frame> CurrentFrameDictionary
        {
            get { return _currentFrameDictionary; }
            set { _currentFrameDictionary = value; }
        }
        public Bitmap SpriteSheet
        {
            get { return _spriteSheet; }
            set
            {
				if (_spriteSheet != null)
					_spriteSheet.Dispose();
				if (value != null)
				{
					_spriteSheet = new Bitmap(value);
					_spriteSheet.MakeTransparent(COLOR_TRANSPARENT);
				}
				else
				{
					_spriteSheet = null;
				}
                Invalidate();
            }
        }
		public int SelectedIndex
		{
			get { return SelectedIndices.Count > 0 ? SelectedIndices[0] : -1; }
		}
		public int SpecialSelection
		{
			get { return _specialSelection; }
			set
			{
				_specialSelection = value;
				Invalidate();
			}
		}

        public FrameList()
        {
            InitializeComponent();
        }

        public FrameList(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

		public new void Dispose()
		{
			base.Dispose();
			if (_spriteSheet != null)
				_spriteSheet.Dispose();
		}

		private void FrameList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (CurrentList == null) return;
            if (e.ItemIndex < 0 || e.ItemIndex >= CurrentList.Count) return;
            object item = CurrentList[e.ItemIndex];
            Frame frame;
            if (item is string)
            {
                var name = item as string;
                if (CurrentFrameDictionary == null) return;
                CurrentFrameDictionary.TryGetValue(name, out frame);
            }
            else if (item is Frame)
                frame = item as Frame;
            else
                frame = null;

            if ((e.State & ListViewItemStates.Focused) == ListViewItemStates.Focused)
			{
				e.Graphics.FillRectangle(BRUSH_CURRENT_SELECTION, e.Bounds);
			}
			else if (e.ItemIndex == _specialSelection)
			{
				e.Graphics.FillRectangle(BRUSH_SPECIAL_SELECTION, e.Bounds);
			}
            else
			{
				e.DrawBackground();
			}

            if (_spriteSheet != null && frame != null)
                DrawZoom(e.Graphics, _spriteSheet, e.Bounds, frame.Rectangle);
            else
                e.DrawText();

            e.DrawFocusRectangle();
        }
        private void DrawZoom(Graphics g, Image image, Rectangle dstRect, Rectangle srcRect)
        {
            float ratioX = (float)dstRect.Width / srcRect.Width;
            float ratioY = (float)dstRect.Height / srcRect.Height;
            RectangleF rect;
            if (ratioX < 1.0f || ratioY < 1.0f)
            {
                float ratio;
                int destX = dstRect.X, destY = dstRect.Y;
                if (ratioY < ratioX)
                {
                    ratio = ratioY;
                    destX += Convert.ToInt32((dstRect.Width - (srcRect.Width * ratio)) / 2.0f);
                }
                else
                {
                    ratio = ratioX;
                    destY += Convert.ToInt32((dstRect.Height - (srcRect.Height * ratio)) / 2.0f);
                }
                rect = new RectangleF(destX, destY, srcRect.Width * ratio, srcRect.Height * ratio);
            }
            else
            {
                rect = new RectangleF(
                    dstRect.X + (dstRect.Width - srcRect.Width) / 2.0f,
                    dstRect.Y + (dstRect.Height - srcRect.Height) / 2.0f,
                    srcRect.Width, srcRect.Height);
            }
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(image, rect, srcRect, GraphicsUnit.Pixel);
        }
    }
}

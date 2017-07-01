using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using libTools.Anim;
using libTools;
using System.Collections.Generic;

namespace libTools.Forms
{
    public partial class AnimPanel : Control
    {
        private Image mCurrentTexture;
        private Dictionary<string, Frame> mFrameDictionary;
        private FrameSequence mCurrentFrameSequence;
        private int mCurrentFrame;
        private float mZoom = 1.0f;
        private Timer mTimer;
        private bool mIsRunning;
        private bool mIsGridEnabled;
        private bool mIsCenterEnabled;
        private bool mIsHitboxVisible;
        private Helper.HiTimer mHiTimer = new Helper.HiTimer();
        private double mCurTimer;

        public bool Running
        {
            get { return mIsRunning; }
            set { mIsRunning = value; }
        }
        public float Zoom
        {
            get { return mZoom; }
            set { mZoom = Math.Max(value, 0.125f); }
        }
        public bool IsGridEnabled
        {
            get { return mIsGridEnabled; }
            set
            {
                mIsGridEnabled = value;
                Invalidate();
            }
        }
        public bool IsCenterEnabled
        {
            get { return mIsCenterEnabled; }
            set
            {
                mIsCenterEnabled = value;
                Invalidate();
            }
        }
        public bool IsHitboxVisible
        {
            get { return mIsHitboxVisible; }
            set
            {
                mIsHitboxVisible = value;
                Invalidate();
            }
        }

        public Image CurrentTexture
        {
            get { return mCurrentTexture; }
            set { mCurrentTexture = value; }
        }
        public FrameSequence CurrentFrameSequence
        {
            get { return mCurrentFrameSequence; }
            set
            {
                mTimer.Enabled = false;
                mTimer.Stop();

                mCurrentFrameSequence = value;
                Rewind();

                mTimer.Enabled = true;
                mTimer.Start();
            }
        }
        public Dictionary<string, Frame> CurrentFrameDictionary
        {
            get { return mFrameDictionary; }
            set { mFrameDictionary = value; }
        }
        public int CurrentFrameIndex
        {
            get { return mCurrentFrame; }
            set { mCurrentFrame = value; Invalidate(); }
        }

        public void Rewind()
        {
            mHiTimer = new Helper.HiTimer();
            mCurTimer = mHiTimer.GetElapsedTime();
            mCurrentFrame = 0;
        }

        public AnimPanel()
        {
            DoubleBuffered = true;
            Paint += new PaintEventHandler(AnimPanel_Paint);
            mTimer = new Timer();
            mTimer.Tick += OnTimer_Tick;
            mTimer.Interval = 1;
        }

        private void OnTimer_Tick(object sender, EventArgs e)
        {
            if (CurrentFrameSequence != null)
            {
                if (Enabled && Running && CurrentFrameSequence.FramesPerSecond > 0)
                {
                    double fps = 1.0 / CurrentFrameSequence.FramesPerSecond;
                    double deltaTime = mHiTimer.GetElapsedTime();
                    mCurTimer += deltaTime;
                    int framesToAdvance = (int)Math.Floor(mCurTimer / fps);
                    mCurTimer = mCurTimer % fps;
                    mCurrentFrame += framesToAdvance;
                }
                else
                    mCurTimer = mHiTimer.PeekElapsedTime();

                var framelist = CurrentFrameSequence.Frames;
                if (mCurrentFrame >= framelist.Count && framelist.Count > 0)
                {
                    if (CurrentFrameSequence.Loop == 0 ||
                        CurrentFrameSequence.Loop >= framelist.Count)
                        mCurrentFrame %= framelist.Count;
                    else
                        // loop + (curFrame - len) % (len - loop)
                        mCurrentFrame = CurrentFrameSequence.Loop;
                }
                DrawFrame();
            }

        }
        private void DrawFrame()
        {
            var framelist = CurrentFrameSequence.Frames;
            if (mCurrentFrame >= framelist.Count) return;
            DrawFrame(framelist[mCurrentFrame]);
        }
        private void DrawFrame(string name)
        {
            if (CurrentFrameDictionary == null) return;
            Frame frame;
            if (CurrentFrameDictionary.TryGetValue(name, out frame))
                DrawFrame(frame);
        }
        private void DrawFrame(Frame frame)
        {
            if (CurrentTexture == null) return;
            if (frame == null) return;

            using (var graphics = CreateGraphics())
            {
                var sizef = graphics.VisibleClipBounds.Size;
                using (Image image = new Bitmap((int)sizef.Width, (int)sizef.Height))
                {
                    using (var g = Graphics.FromImage(image))
                        DrawFrame(g, frame);
                    graphics.DrawImage(image, new Point(0, 0));
                }
            }
        }
        private void DrawFrame(Graphics g, string name)
        {
            if (CurrentFrameDictionary == null) return;
            Frame frame;
            if (CurrentFrameDictionary.TryGetValue(name, out frame))
                DrawFrame(g, frame);
        }
        private void DrawFrame(Graphics g, Frame frame)
        {
            var clip = g.VisibleClipBounds;
            float cx = clip.Width / 2.0f;
            float cy = clip.Height / 2.0f;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.FillRectangle(new SolidBrush(BackColor), clip);
            if (frame != null)
            {
                var framerect = frame.Rectangle;
                var framesize = new Size(Math.Abs(frame.Left - frame.Right), Math.Abs(frame.Top - frame.Bottom));
                float x = cx - frame.CenterX * mZoom;
                float y = cy - frame.CenterY * mZoom;
                float w = framesize.Width * mZoom;
                float h = framesize.Height * mZoom;
                g.DrawImage(CurrentTexture, new RectangleF(x, y, w, h), framerect, GraphicsUnit.Pixel);
                if (IsHitboxVisible)
                {
                    var hitbox = CurrentFrameSequence.Hitbox;
                    var color = Color.FromArgb(0x80, 0x00, 0x00, 0xE0);
                    var brush = new SolidBrush(color);
                    var rect = new RectangleF(
                        cx + hitbox.X * Zoom, cy + hitbox.Y * Zoom,
                        hitbox.Width * Zoom, hitbox.Height * Zoom);
                    g.FillRectangle(brush, rect);
                }
            }
            if (IsGridEnabled)
            {
                var color = Color.FromArgb(0x80, 0x80, 0x80);
            }
            if (IsCenterEnabled)
            {
                var color = Color.FromArgb(BackColor.ToArgb() ^ 0xffffff);
                var pen = new Pen(new SolidBrush(color), 1.0f);
                g.DrawLine(pen, new PointF(cx, 0.0f), new PointF(cx, clip.Height));
                g.DrawLine(pen, new PointF(0.0f, cy), new PointF(clip.Width, cy));
            }
        }

        private void AnimPanel_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentFrameSequence == null) return;
            var framelist = CurrentFrameSequence.Frames;
            if (mCurrentFrame >= framelist.Count) return;
            DrawFrame(e.Graphics, framelist[mCurrentFrame]);
        }
    }
}

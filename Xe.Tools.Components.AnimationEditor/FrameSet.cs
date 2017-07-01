using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libTools.Anim;

namespace AnimEditor
{
    public partial class FrameSet : GroupBox
    {
        public event OnFrameChanged FrameChanged;
        public delegate void OnFrameChanged(object sender, Frame frame);
        
        Frame mCurrentFrame;
        public Frame CurrentFrame
        {
            get { return mCurrentFrame; }
            set
            {
                mCurrentFrame = null;
                if (value != null)
                {
                    Enabled = true;
                    nPosX.Value = value.Left;
                    nPosY.Value = value.Top;
                    nWidth.Value = value.Right - value.Left;
                    nHeight.Value = value.Bottom - value.Top;
                    nCenterX.Value = value.CenterX;
                    nCenterY.Value = value.CenterY;
                    mCurrentFrame = value;
                }
                else
                {
                    Enabled = false;
                }
            }
        }

        public FrameSet()
        {
            InitializeComponent();
        }

        private void FireFrameChanged()
        {
            if (mCurrentFrame != null && FrameChanged != null)
                FrameChanged.Invoke(this, mCurrentFrame);
        }
        private void ResetWidth()
        {
            if (mCurrentFrame != null)
            {
                int x = (int)nPosX.Value;
                int w = (int)nWidth.Value;
                mCurrentFrame.Left = x;
                mCurrentFrame.Right = x + w;
            }
        }
        private void ResetHeight()
        {
            if (mCurrentFrame != null)
            {
                int y = (int)nPosY.Value;
                int h = (int)nHeight.Value;
                mCurrentFrame.Top = y;
                mCurrentFrame.Bottom = y + h;
            }
        }

        private void nPosX_ValueChanged(object sender, EventArgs e)
        {
            ResetWidth();
            FireFrameChanged();
        }
        private void nPosY_ValueChanged(object sender, EventArgs e)
        {
            ResetHeight();
            FireFrameChanged();
        }
        private void nWidth_ValueChanged(object sender, EventArgs e)
        {
            ResetWidth();
            FireFrameChanged();
        }
        private void nHeight_ValueChanged(object sender, EventArgs e)
        {
            ResetHeight();
            FireFrameChanged();
        }
        private void nCenterX_ValueChanged(object sender, EventArgs e)
        {
            if (mCurrentFrame != null)
            {
                mCurrentFrame.CenterX = (short)nCenterX.Value;
                FireFrameChanged();
            }
        }
        private void nCenterY_ValueChanged(object sender, EventArgs e)
        {
            if (mCurrentFrame != null)
            {
                mCurrentFrame.CenterY = (short)nCenterY.Value;
                FireFrameChanged();
            }
        }
    }
}

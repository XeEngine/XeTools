using libTools.Anim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimEditor
{
    public partial class DialogFramesList : Form
    {
        private AnimationsGroup mAnimationGroup;
        private int mSpriteSheet;
        private string mSpritesheetName;
        private Bitmap mSpritesheetImage;
        private Frame mCurrentFrame;
        private int mCurrentFrameIndex;
        
        public int CurrentSpritesheet
        {
            get { return mSpriteSheet; }
            set
            {
                mSpriteSheet = value;
                if (value < 0 || value >= mAnimationGroup.SpriteSheet.Count)
                    CurrentSpritesheetName = null;
                CurrentSpritesheetName = mAnimationGroup.SpriteSheet[value];
            }
        }
        public string CurrentSpritesheetName
        {
            get { return mSpritesheetName; }
            set
            {
                mSpritesheetName = value;
                if (mSpritesheetImage != null)
                    mSpritesheetImage.Dispose();
                if (File.Exists(mSpritesheetName))
                    mSpritesheetImage = new Bitmap(mSpritesheetName);
                else
                    mSpritesheetImage = null;
                frameList.SpriteSheet = mSpritesheetImage;
            }
        }
        public string CurrentFrameName
        {
            get { return textBoxCurrent.Text; }
            set { textBoxCurrent.Text = value; }
        }

        public DialogFramesList(AnimationsGroup animationsGroup)
        {
            InitializeComponent();
            mAnimationGroup = animationsGroup;
            if (mAnimationGroup != null)
            {
                comboBoxSpritesheets.DataSource = mAnimationGroup.SpriteSheet;
                frameList.CurrentFrameDictionary = mAnimationGroup.Frames;
                frameList.CurrentList = mAnimationGroup.Frames.Keys.ToList();
            }
            else
            {
                comboBoxSpritesheets.DataSource = null;
                frameList.CurrentFrameDictionary = null;
                frameList.CurrentList = null;
            }
        }

        private void frameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frameList.SelectedIndices.Count <= 0) return;
            int index = frameList.SelectedIndices[0];
            if (index < 0) return;
            var items = frameList.SelectedItems;
            if (items.Count > 0)
            {
                var item = items[0];
                mCurrentFrameIndex = item.Index;
                CurrentFrameName = item.Text;
                mCurrentFrame = mAnimationGroup.Frames[CurrentFrameName];
            }
        }
        private void comboBoxSpritesheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBoxSpritesheets.SelectedIndex;
            if (index < 0) return;
            CurrentSpritesheet = index;
            CurrentSpritesheetName = mAnimationGroup.SpriteSheet[index];
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            var text = (sender as TextBox).Text;
            if (text.Length > 0)
            {
                var list = mAnimationGroup.Frames.Keys.Where(x => x.Contains(text));
                frameList.CurrentList = list.ToList();
            }
            else
                frameList.CurrentList = mAnimationGroup.Frames.Keys.ToList();
        }
    }
}

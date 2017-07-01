using libTools;
using libTools.Anim;
using libTools.Forms;
using libTools.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AnimEditor
{
    public partial class FormAnim : Form
    {
        private Dictionary<string, Bitmap> mCacheSpritesheet = new Dictionary<string, Bitmap>();

        Project curProject;
        Project.Container curProjectContainer;
        Project.Item curProjectItem;

        AnimationsGroup mCurAnimGroup;
        Animation mCurAnim;
        Frame mCurFrame;

        public AnimationsGroup CurrentAnimationsGroup
        {
            get { return mCurAnimGroup; }
            set
            {
                mCurAnimGroup = value;
                comboBoxTextures.DataSource = mCurAnimGroup.SpriteSheet;
                listBoxAnimations.CurrentList = mCurAnimGroup.Animations;
                frameList.CurrentFrameDictionary = mCurAnimGroup.Frames;
                animPanel.CurrentFrameDictionary = mCurAnimGroup.Frames;
            }
        }
        public Animation CurrentAnimation
        {
            get { return mCurAnim; }
            set
            {
                mCurAnim = value;
                if (value != null)
                {
                    bool loadStuff = true;
                    if (value.Link != null)
                    {
                        int index = -1;
                        do
                        {
                            index = GetAnimationFromName(mCurAnim.Link, out mCurAnim);
                        } while (mCurAnim != null && mCurAnim.Link != null);
                        if (index < 0)
                            loadStuff = false;
                    }
                    if (loadStuff)
                    {
                        groupBoxAnimation.Enabled = true;
                        if (mCurAnim.Sequence.Frames != null && mCurAnim.Sequence.Frames.Count > 0)
                            CurrentFrameName = mCurAnim.Sequence.Frames[0];

                        nFps.Value = (decimal)mCurAnim.Sequence.FramesPerSecond;
                        nSpeed.Value = mCurAnim.Sequence.Speed;
						if (mCurAnim.Sequence.Loop == 0xFF)
						{
							checkBoxLoop.Checked = false;
							nLoop.Enabled = false;
							nLoop.Maximum = 0xFF;
							nLoop.Value = 0xFF;
						}
						else
						{
							checkBoxLoop.Checked = true;
							nLoop.Enabled = true;
							nLoop.Maximum = mCurAnim.Sequence.Frames.Count - 1;
							nLoop.Value = Math.Min(nLoop.Maximum, mCurAnim.Sequence.Loop);
						}
						if (mCurAnim.Sequence.Event == 0xFF)
						{
							checkBoxEvent.Checked = false;
							nEvent.Enabled = false;
							nEvent.Maximum = 0xFF;
							nEvent.Value = 0xFF;
						}
						else
						{
							checkBoxEvent.Checked = true;
							nEvent.Enabled = true;
							nEvent.Maximum = mCurAnim.Sequence.Frames.Count - 1;
							nEvent.Value = Math.Min(nEvent.Maximum, mCurAnim.Sequence.Event);
						}
						frameList.CurrentList = mCurAnim.Sequence.Frames;
                        animPanel.CurrentFrameSequence = mCurAnim.Sequence;
                        if (mCurAnim.Sequence.Texture < mCurAnimGroup.SpriteSheet.Count)
                        {
                            comboBoxTextures.SelectedIndex = mCurAnim.Sequence.Texture;
                            var filename = mCurAnimGroup.SpriteSheet[mCurAnim.Sequence.Texture];
                            var currentTexture = GetBitmapFromFileName(filename);
                            frameList.SpriteSheet = currentTexture;
                            animPanel.CurrentTexture = currentTexture;
                        }
                        nHitboxLeft.Value = mCurAnim.Sequence.HitboxLeft;
                        nHitboxTop.Value = mCurAnim.Sequence.HitboxTop;
                        nHitboxRight.Value = mCurAnim.Sequence.HitboxRight;
                        nHitboxBottom.Value = mCurAnim.Sequence.HitboxBottom;
                    }
                    else
                    {
                        groupBoxAnimation.Enabled = false;
                    }

                    mCurAnim = value;
                    if (value.Link != null)
                    {
                        frameSet.Enabled = false;
                        frameList.Enabled = false;
                        comboBoxTextures.Enabled = false;
                        nFps.Enabled = false;
                        nSpeed.Enabled = false;
                        nLoop.Enabled = false;
                        checkBoxLoop.Enabled = false;
                        groupBoxHitbox.Enabled = false;
                        labelLinkedAnimation.Enabled = true;
                        labelLinkedAnimation.Text = value.Link;
                        buttonLink.Text = "Unlink";
                    }
                    else
                    {
                        frameSet.Enabled = true;
                        frameList.Enabled = true;
                        comboBoxTextures.Enabled = true;
                        nFps.Enabled = true;
                        nSpeed.Enabled = true;
                        nLoop.Enabled &= true;
                        checkBoxLoop.Enabled = true;
                        groupBoxHitbox.Enabled = true;
                        labelLinkedAnimation.Enabled = false;
                        labelLinkedAnimation.Text = "no animation currently linked";
                        buttonLink.Text = "Link";
                    }
                }
                else
                {
                    groupBoxAnimation.Enabled = false;
                }
            }
        }
        public Frame CurrentFrame
        {
            get { return mCurFrame; }
            set
            {
                mCurFrame = value;
                frameSet.CurrentFrame = mCurFrame;
            }
        }
        public string CurrentFrameName
        {
            get { return CurrentFrame == null ? null : CurrentFrame.Name; }
            set
            {
                if (mCurAnimGroup == null) return;
                if (mCurAnimGroup.Frames == null) return;
                Frame frame;
                if (mCurAnimGroup.Frames.TryGetValue(value, out frame))
                    CurrentFrame = frame;
                else
                    CurrentFrame = null;
            }
        }

        public FormAnim()
        {
            InitializeComponent();

            toolStripAnimPause.Image = animPanel.Running ? Icons.Pause : Icons.Run;
            toolStripAnimRewind.Image = Icons.Refresh;
            toolStripAnimGrid.Image = Icons.AppearanceTabGrid;
            toolStripAnimCenter.Image = Icons.TableInsideBorder;
            toolStripAnimZoomIn.Image = Icons.ZoomIn;
            toolStripAnimZoomOut.Image = Icons.ZoomOut;
            toolStripAnimHitbox.Image = Icons.AppearanceTabSwatch;
            toolStripAnimBackground.Image = Icons.BackgroundColor;
            animPanel.BackColor = Properties.Settings.Default.AnimBgColor;
            animPanel.Zoom = Properties.Settings.Default.AnimZoom;
            animPanel.IsGridEnabled = toolStripAnimGrid.Checked = Properties.Settings.Default.AnimShowGrid;
            animPanel.IsCenterEnabled = toolStripAnimCenter.Checked = Properties.Settings.Default.AnimShowCenter;
            animPanel.IsHitboxVisible = toolStripAnimHitbox.Checked = Properties.Settings.Default.AnimShowHitbox;

            listBoxAnimations.TemplateItem = new Animation();
            listBoxAnimations.ListBox.DoubleClick += ListBox_DoubleClick;
        }

        int GetAnimationFromName(string name, out Animation anim)
        {
            if (name != null)
            {
                for (int i = 0; i < CurrentAnimationsGroup.Animations.Count; i++)
                {
                    var item = CurrentAnimationsGroup.Animations[i];
                    if (string.Compare(name, item.Name) == 0)
                    {
                        anim = item;
                        return i;
                    }
                }
            }
            anim = null;
            return -1;
        }

        private void ListBox_SelectedIndexChanged(object sender, int index)
        {
            if (listBoxAnimations.CurrentItemSelected == null) CurrentAnimation = null;
            else CurrentAnimation = listBoxAnimations.CurrentItemSelected as Animation;
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (CurrentAnimation == null) return;
            var dialog = new DialogAnimationName();
            dialog.AnimationName = CurrentAnimation.Name;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentAnimation.Name = dialog.AnimationName;
                listBoxAnimations.OnItemChanged();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentAnimation = null;
            LoadRecentList();
            OpenRecentItem(Properties.Settings.Default.FileRecent1);
        }

        private Bitmap GetBitmapFromFileName(string filename)
        {
            Bitmap bmp;
            if (!mCacheSpritesheet.TryGetValue(filename, out bmp)) {
                if (!File.Exists(filename)) return null;
                bmp = new Bitmap(filename);
                mCacheSpritesheet.Add(filename, bmp);
            }
            return bmp;
        }

        #region RECENT FILES MANAGEMENT
        private void LoadRecentList()
        {
            SetRecentItemValue(toolStripMenuItemRecent1, Properties.Settings.Default.FileRecent1);
            SetRecentItemValue(toolStripMenuItemRecent2, Properties.Settings.Default.FileRecent2);
            SetRecentItemValue(toolStripMenuItemRecent3, Properties.Settings.Default.FileRecent3);
            SetRecentItemValue(toolStripMenuItemRecent4, Properties.Settings.Default.FileRecent4);
            SetRecentItemValue(toolStripMenuItemRecent5, Properties.Settings.Default.FileRecent5);
            SetRecentItemValue(toolStripMenuItemRecent6, Properties.Settings.Default.FileRecent6);
            SetRecentItemValue(toolStripMenuItemRecent7, Properties.Settings.Default.FileRecent7);
            SetRecentItemValue(toolStripMenuItemRecent8, Properties.Settings.Default.FileRecent8);
            SetRecentItemValue(toolStripMenuItemRecent9, Properties.Settings.Default.FileRecent9);
        }
        private void SetRecentItemValue(ToolStripMenuItem item, string value)
        {
            bool found = value.Length > 0;
            item.Visible = found;
            item.Enabled = found;
            item.Text = string.Format("&{0}: {1}", item.Tag, value);
        }
        private void AddToRecentItems(string projFilename, Project.Container container, Project.Item item)
        {
            var str = string.Format("{0}|{1}|{2}", projFilename, container.Name, item.FileNameInput);
            if (string.Compare(str, Properties.Settings.Default.FileRecent1, false) != 0)
            {
                Properties.Settings.Default.FileRecent9 = Properties.Settings.Default.FileRecent8;
                Properties.Settings.Default.FileRecent8 = Properties.Settings.Default.FileRecent7;
                Properties.Settings.Default.FileRecent7 = Properties.Settings.Default.FileRecent6;
                Properties.Settings.Default.FileRecent6 = Properties.Settings.Default.FileRecent5;
                Properties.Settings.Default.FileRecent5 = Properties.Settings.Default.FileRecent4;
                Properties.Settings.Default.FileRecent4 = Properties.Settings.Default.FileRecent3;
                Properties.Settings.Default.FileRecent3 = Properties.Settings.Default.FileRecent2;
                Properties.Settings.Default.FileRecent2 = Properties.Settings.Default.FileRecent1;
                Properties.Settings.Default.FileRecent1 = str;
                LoadRecentList();
                Properties.Settings.Default.Save();
            }
        }
        private void OpenRecentItem(string value)
        {
            if (value == null || value.Length < 1) return;
            var strArray = value.Split(new char[] { '|' });
            curProject = Project.Open(strArray[0]);
            if (curProject == null) return;
            curProjectContainer = curProject.GetContainer(strArray[1]);
            if (curProjectContainer == null) return;
            var r = curProjectContainer.Items.Where(x => string.Compare(x.FileNameInput, strArray[2], true) == 0);
            if (r.Count() < 1) return;
            curProjectItem = r.First();
            var dialog = new DialogProjectSelection(curProject, "animation");
            Directory.SetCurrentDirectory(curProject.Path);
            var filename = Path.Combine(curProjectContainer.Name, curProjectItem.FileNameInput);
            CurrentAnimationsGroup = new AnimationsGroup();

        }
        #endregion

        #region FILE menu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Filter = "XeEngine project|*.xml";
            if (fd.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    curProject = Project.Open(fd.FileName);
                    var dialog = new DialogProjectSelection(curProject, "animation");
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        var item = dialog.CurrentItem;
                        Directory.SetCurrentDirectory(curProject.Path);

                        var animGroup = new AnimationsGroup(item.FileNameInput);
                        curProjectContainer = dialog.CurrentContainer;
                        curProjectItem = dialog.CurrentItem;
                        AddToRecentItems(fd.FileName, curProjectContainer, curProjectItem);
                        CurrentAnimationsGroup = animGroup;

						Text = string.Format("{0} - Animation Editor", item.RelativeFileNameInput);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentAnimationsGroup.Save();
        }
        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void toolStripMenuRecent_Click(object sender, EventArgs e)
        {
            var tag = ((ToolStripMenuItem)sender).Tag;
            string name;
            switch (tag as string)
            {
                case "1": name = Properties.Settings.Default.FileRecent1; break;
                case "2": name = Properties.Settings.Default.FileRecent2; break;
                case "3": name = Properties.Settings.Default.FileRecent3; break;
                case "4": name = Properties.Settings.Default.FileRecent4; break;
                case "5": name = Properties.Settings.Default.FileRecent5; break;
                case "6": name = Properties.Settings.Default.FileRecent6; break;
                case "7": name = Properties.Settings.Default.FileRecent7; break;
                case "8": name = Properties.Settings.Default.FileRecent8; break;
                case "9": name = Properties.Settings.Default.FileRecent9; break;
                default: return;
            }
            OpenRecentItem(name);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void comboBoxTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTextures.SelectedIndex < 0 || mCurAnim == null) return;
            mCurAnim.Sequence.Texture = comboBoxTextures.SelectedIndex;
            var filename = mCurAnimGroup.SpriteSheet[mCurAnim.Sequence.Texture];
            frameList.SpriteSheet = GetBitmapFromFileName(filename);
        }

        private void frameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frameList.SelectedIndices.Count <= 0) return;
            int index = frameList.SelectedIndices[0];
            if (index < 0) return;
            CurrentFrameName = CurrentAnimation.Sequence.Frames[index];
            if (!animPanel.Running || mCurAnim.Sequence.Speed == 0)
                animPanel.CurrentFrameIndex = index;
        }

        private void frameSet_FrameChanged(object sender, Frame frame)
        {
            frameList.Invalidate();
        }
        private void nHitboxLeft_ValueChanged(object sender, EventArgs e)
        {
            CurrentAnimation.Sequence.HitboxLeft = (int)(sender as NumericUpDown).Value;
        }
        private void nHitboxTop_ValueChanged(object sender, EventArgs e)
        {
            CurrentAnimation.Sequence.HitboxTop = (int)(sender as NumericUpDown).Value;
        }
        private void nHitboxRight_ValueChanged(object sender, EventArgs e)
        {
            CurrentAnimation.Sequence.HitboxRight = (int)(sender as NumericUpDown).Value;
        }
        private void nHitboxBottom_ValueChanged(object sender, EventArgs e)
        {
            CurrentAnimation.Sequence.HitboxBottom = (int)(sender as NumericUpDown).Value;
        }

        private void toolStripAnimPause_Click(object sender, EventArgs e)
        {
            animPanel.Running = !animPanel.Running;
            toolStripAnimPause.Image = animPanel.Running ? Icons.Pause : Icons.Run;
        }
        private void toolStripAnimRewind_Click(object sender, EventArgs e)
        {
            animPanel.Rewind();
        }
        private void toolStripAnimCenter_Click(object sender, EventArgs e)
        {
            animPanel.IsCenterEnabled = !animPanel.IsCenterEnabled;
            toolStripAnimCenter.Checked = animPanel.IsCenterEnabled;
            Properties.Settings.Default.AnimShowCenter = animPanel.IsCenterEnabled;
            Properties.Settings.Default.Save();
        }
        private void toolStripAnimGrid_Click(object sender, EventArgs e)
        {
            animPanel.IsGridEnabled = !animPanel.IsGridEnabled;
            toolStripAnimGrid.Checked = animPanel.IsGridEnabled;
            Properties.Settings.Default.AnimShowGrid = animPanel.IsGridEnabled;
            Properties.Settings.Default.Save();
        }
        private void toolStripAnimHitbox_Click(object sender, EventArgs e)
        {
            animPanel.IsHitboxVisible = !animPanel.IsHitboxVisible;
            toolStripAnimHitbox.Checked = animPanel.IsHitboxVisible;
            Properties.Settings.Default.AnimShowHitbox = animPanel.IsHitboxVisible;
            Properties.Settings.Default.Save();
        }
        private void toolStripAnimZoomIn_Click(object sender, EventArgs e)
        {
            float[] ZOOM = { 0.25f, 0.50f, 1.0f, 1.5f, 2.0f, 4.0f, 6.0f, 8.0f, 12.0f, 16.0f };
            for (int i = 0; i < ZOOM.Length - 1; i++)
            {
                if (animPanel.Zoom <= ZOOM[i])
                {
                    animPanel.Zoom = ZOOM[i + 1];
                    Properties.Settings.Default.AnimZoom = animPanel.Zoom;
                    Properties.Settings.Default.Save();
                    break;
                }
            }
        }
        private void toolStripAnimZoomOut_Click(object sender, EventArgs e)
        {
            float[] ZOOM = { 0.25f, 0.50f, 1.0f, 1.5f, 2.0f, 4.0f, 6.0f, 8.0f, 12.0f, 16.0f };
            for (int i = ZOOM.Length - 1; i > 0; i--)
            {
                if (animPanel.Zoom >= ZOOM[i])
                {
                    animPanel.Zoom = ZOOM[i - 1];
                    Properties.Settings.Default.AnimZoom = animPanel.Zoom;
                    Properties.Settings.Default.Save();
                    break;
                }
            }
        }
        private void toolStripAnimBackground_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = animPanel.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                animPanel.BackColor = colorDialog.Color;
                Properties.Settings.Default.AnimBgColor = animPanel.BackColor;
                Properties.Settings.Default.Save();
            }
        }
        
        private void checkBoxLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLoop.Checked)
            {
                nLoop.Enabled = true;
                nLoop.Value = 0;
                nLoop.Maximum = CurrentAnimation.Sequence.Frames.Count;
            }
            else
            {
                nLoop.Enabled = false;
                nLoop.Maximum = 0xFF;
                nLoop.Value = 0xFF;
            }
		}
		private void checkBoxEvent_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxEvent.Checked)
			{
				nEvent.Enabled = true;
				if (frameList.SelectedIndex >= 0)
					nEvent.Value = frameList.SelectedIndex;
				else
					nEvent.Value = 0;
				nEvent.Maximum = CurrentAnimation.Sequence.Frames.Count;
			}
			else
			{
				nEvent.Enabled = false;
				nEvent.Maximum = 0xFF;
				nEvent.Value = 0xFF;
			}
		}
		private void nLoop_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentAnimation == null) return;
            CurrentAnimation.Sequence.Loop = (int)nLoop.Value;
		}
		private void nEvent_ValueChanged(object sender, EventArgs e)
		{
			if (CurrentAnimation == null) return;
			frameList.SpecialSelection = (int)nEvent.Value;
			CurrentAnimation.Sequence.Event = (int)nEvent.Value;
		}
		private void nFps_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentAnimation == null) return;
            CurrentAnimation.Sequence.FramesPerSecond = (int)nFps.Value;
            nSpeed.Value = CurrentAnimation.Sequence.Speed;
        }
        private void nSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (CurrentAnimation == null) return;
            CurrentAnimation.Sequence.Speed = (int)nSpeed.Value;
        }
        private void buttonLink_Click(object sender, EventArgs e)
        {
            bool isLinked = CurrentAnimation.Link != null;
            if (isLinked)
            {
                CurrentAnimation.Link = null;
                CurrentAnimation = CurrentAnimation;
            }
            else
            {
                var dialog = new DialogLinkAnimation();
                dialog.AnimationsGroup = CurrentAnimationsGroup;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CurrentAnimation.Link = dialog.AnimationName;
                    CurrentAnimation = CurrentAnimation;
                }
            }
        }
        private void labelLinkedAnimation_Click(object sender, EventArgs e)
        {
            Animation anim;
            var index = GetAnimationFromName(CurrentAnimation.Link, out anim);
            if (index >= 0)
                listBoxAnimations.SelectedIndex = index;
        }

        private void frameList_DoubleClick(object sender, EventArgs e)
        {
            if (CurrentAnimationsGroup == null) return;
            var form = new DialogFramesList(CurrentAnimationsGroup)
            {
                CurrentSpritesheet = mCurAnim.Sequence.Texture,
                CurrentFrameName = CurrentFrameName
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (frameList.SelectedIndices.Count <= 0) return;
                int index = frameList.SelectedIndices[0];
                if (index < 0) return;
                mCurAnim.Sequence.Texture = form.CurrentSpritesheet;
                CurrentFrameName = form.CurrentFrameName;
                CurrentAnimation.Sequence.Frames[index] = CurrentFrameName;
            }
        }

        private void atlasToFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();
            if (fd.ShowDialog() != DialogResult.OK)
                return;

            var textures = new Bitmap[mCurAnimGroup.SpriteSheet.Count];
            {
                var index = 0;
                foreach (var image in mCurAnimGroup.SpriteSheet)
                {
                    textures[index++] = new Bitmap(image);
                }
            }

            var path = fd.SelectedPath;
            foreach (var frame in mCurAnimGroup.Frames)
            {
                UpdateFrameFromAtlas(textures, frame.Value, new DirectoryInfo(path));
            }

            foreach (var image in textures)
                image.Dispose();
        }

        private void UpdateFrameFromAtlas(Bitmap[] textures, Frame frame, DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                if (fileName == frame.Name)
                {
                    using (var img = new Bitmap(frame.Size.Width - 1, frame.Size.Height - 1))
                    {
                        using (var g = Graphics.FromImage(img))
                        {
                            var srcRect = new Rectangle(0, 0, img.Size.Width, img.Size.Height);
                            var dstRect = new Rectangle(frame.Rectangle.Left, frame.Rectangle.Top,
                                img.Size.Width, img.Size.Height);
                            g.DrawImage(textures[0], srcRect, dstRect, GraphicsUnit.Pixel);
                        }
                        var fullName = Path.Combine(directory.FullName, fileName) + ".png";
                        img.Save(fullName);
                    }
                }
            }
            foreach (var dir in directory.GetDirectories())
                UpdateFrameFromAtlas(textures, frame, dir);
        }
    }
}

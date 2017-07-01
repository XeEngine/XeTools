using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xe.Tools;

namespace libTools
{
    public partial class DialogItemEdit : Form
    {
        private Project mCurrentProject;
        private Language.Message mMessageName;
        private Language.Message mMessageDesc;
        private Project.Item[] mFontItems;
        //private Font.FontDrawer mFont;

        private int CurrentFontHeight
        {
            get
            {
                return 8;
                /*if (mFont == null) return 0;
                return mFont.CharSet.Height;*/
            }
        }
        public Language.Message MessageName
        {
            get { return mMessageName; }
            set
            {
                mMessageName = value;
                langItemEn.CurrentName = value;
                langItemEn.CurrentName = value;
                langItemIt.CurrentName = value;
                langItemFr.CurrentName = value;
                langItemDe.CurrentName = value;
                langItemSp.CurrentName = value;
            }
        }
        public Language.Message MessageDesc
        {
            get { return mMessageDesc; }
            set
            {
                mMessageDesc = value;
                langItemEn.CurrentDescription = value;
                langItemIt.CurrentDescription = value;
                langItemFr.CurrentDescription = value;
                langItemDe.CurrentDescription = value;
                langItemSp.CurrentDescription = value;
            }
        }

        private Brush mWindowBgBrush;
        private Brush mWindowBorderBrush;
        private Brush mItemIconPlaceholder;
        private Pen mWindowBorder;
        private RectangleF mMenuRect;
        private RectangleF mWindowRect;
        private RectangleF mDescriptionRect;
        private string mCurrentName;
        private string mCurrentDesc;

        public Project CurrentProject
        {
            get { return mCurrentProject; }
            set
            {
                mCurrentProject = value;
                //Directory.SetCurrentDirectory(mCurrentProject.Path);
                //mFontItems = mCurrentProject.GetContainer("data").GetItemsFromType("font").ToArray();
                comboBoxFont.DataSource = mFontItems.ToList();
            }
        }
        public string CurrentName
        {
            get { return mCurrentName; }
            set
            {
                mCurrentName = value;
                panelPreview.Invalidate();
            }
        }
        public string CurrentDescription
        {
            get { return mCurrentDesc; }
            set
            {
                mCurrentDesc = value;
                panelPreview.Invalidate();
            }
        }

        public DialogItemEdit()
        {
            InitializeComponent();
            var bgColor = Color.FromArgb(0xC0, 0x40, 0x40, 0x40);
            mWindowBgBrush = new SolidBrush(bgColor);
            mWindowBorderBrush = new SolidBrush(Color.Black);
            mWindowBorder = new Pen(mWindowBorderBrush, 1.0f);
            mItemIconPlaceholder = new SolidBrush(Color.Cyan);
            CalculateWindowSize();
        }

        private void SelectFont(Project.Item item)
        {
            //mFont = new libTools.Font.FontDrawer(item.FileNameInput);
            CalculateWindowSize();
        }
        private void CalculateWindowSize()
        {
            const float BORDER = 8.0f;
            const float SPACE = 4.0f;
            const float SCREEN_WIDTH = 320.0f;
            const float MENU_WIDTH = 88.0f;

            mMenuRect = new RectangleF(BORDER, BORDER, MENU_WIDTH, CurrentFontHeight + SPACE * 2);
            mWindowRect = new RectangleF(mMenuRect.Right + BORDER, mMenuRect.X,
                SCREEN_WIDTH - BORDER * 2.0f - mMenuRect.Right, mMenuRect.Height);
            mDescriptionRect = new RectangleF(mMenuRect.X, mMenuRect.Bottom + BORDER,
                SCREEN_WIDTH - BORDER * 2.0f, CurrentFontHeight * 2.0f + SPACE * 2.0f);
            panelPreview.Invalidate();
        }

        private void panelPreview_Paint(object sender, PaintEventArgs e)
        {
            //libTools.Font.FontDrawer.SetNearestInterpolation(e.Graphics);

            e.Graphics.FillRectangle(mWindowBgBrush, mMenuRect.X, mMenuRect.Y, mMenuRect.Width, mMenuRect.Height);
            e.Graphics.DrawRectangle(mWindowBorder, mMenuRect.X, mMenuRect.Y, mMenuRect.Width, mMenuRect.Height);

            e.Graphics.FillRectangle(mWindowBgBrush, mWindowRect.X, mWindowRect.Y, mWindowRect.Width, mWindowRect.Height);
            e.Graphics.DrawRectangle(mWindowBorder, mWindowRect.X, mWindowRect.Y, mWindowRect.Width, mWindowRect.Height);

            e.Graphics.FillRectangle(mWindowBgBrush, mDescriptionRect.X, mDescriptionRect.Y, mDescriptionRect.Width, mDescriptionRect.Height);
            e.Graphics.DrawRectangle(mWindowBorder, mDescriptionRect.X, mDescriptionRect.Y, mDescriptionRect.Width, mDescriptionRect.Height);

            e.Graphics.FillRectangle(mItemIconPlaceholder, mWindowRect.X + 4.0f, mWindowRect.Y + 3.0f, 16.0f, 16.0f);

            DrawString(e.Graphics, mMenuRect, 0, "Oggetti");
            DrawString(e.Graphics, mWindowRect, 20.0f, CurrentName);
            DrawString(e.Graphics, mDescriptionRect, 0.0f, CurrentDescription);
        }
        private void DrawString(Graphics g, RectangleF rect, float x, string str)
        {
            const float SPACE = 4.0f;
            Rectangle bounds = new Rectangle(
                (int)(rect.X + x + SPACE),
                (int)(rect.Y + SPACE),
                (int)(rect.Width - SPACE),
                (int)(rect.Height - SPACE));
            if (str == null) str = "<null>";
            //mFont.DrawString(g, str, bounds);
        }

        private void comboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (sender as ComboBox).SelectedIndex;
            if (index >= 0) SelectFont(mFontItems[index]);
        }

        private void langItem_OnTextChanged(LangItem sender, string name, string desc)
        {
            CurrentName = name;
            CurrentDescription = desc;
        }
    }
}

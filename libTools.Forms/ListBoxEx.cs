using System;
using System.Collections;
using System.Windows.Forms;
using Xe;

namespace libTools.Forms
{
    public partial class ListBoxEx : UserControl
    {
        IList mCurrentList;
        object mObjToCopy;
        object mTemplateItem;
        object mItemSelected;
        
        public IList CurrentList
        {
            get { return mCurrentList; }
            set {
                mCurrentList = value;
                listBox.DataSource = mCurrentList;
            }
        }
        public object TemplateItem
        {
            get { return mTemplateItem; }
            set { mTemplateItem = value; }
        }
        public object CurrentItemSelected
        {
            get { return mItemSelected; }
        }

        #region ListBox
        public object DataSource
        {
            get { return CurrentList; }
            set { CurrentList = value as IList; }
        }
        public int SelectedIndex
        {
            get { return listBox.SelectedIndex; }
            set { listBox.SelectedIndex = value; }
        }
        public DrawMode DrawMode
        {
            get { return listBox.DrawMode; }
            set { listBox.DrawMode = value; }
        }
        
        public delegate void DrawItem(object sender, DrawItemEventArgs e);
        public event DrawItem OnDrawItem;
        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (OnDrawItem != null)
                OnDrawItem(sender, e);
        }
        public delegate void SelectedIndexChanged(object sender, int index);
        public event SelectedIndexChanged OnSelectedIndexChanged;

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged(this, SelectedIndex);
        }
        #endregion

        public ListBox ListBox
        {
            get { return listBox; }
        }

        public ListBoxEx()
        {
            InitializeComponent();
            listBox.DrawItem += ListBox_DrawItem;
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
        }

        public void OnItemChanged()
        {
            OnItemChanged(listBox.SelectedIndex);
        }
        public void OnItemChanged(int newindex)
        {
            listBox.SuspendLayout();
            var index = listBox.SelectedIndex;
            var top = listBox.TopIndex;
            var dataSource = listBox.DataSource;
            listBox.DataSource = null;
            listBox.DataSource = dataSource;
            if (index != newindex)
            {
                if (newindex >= 0)
                {
                    if (top > newindex)
                        top = newindex;
                }
            }
            listBox.TopIndex = top;
            listBox.SelectedIndex = (dataSource as IList).Count > 0 ? Xe.Math.Min((dataSource as IList).Count - 1, newindex) : -1;
            listBox.ResumeLayout();
        }

        protected object CreateInstance()
        {
            if (mTemplateItem == null)
            {
                if (mCurrentList.Count <= 0) return null;
                return Activator.CreateInstance(mCurrentList[0].GetType());
            }
            if (mTemplateItem is IDeepCloneable)
                return (mTemplateItem as IDeepCloneable).DeepClone();
            else if (mTemplateItem is ICloneable)
                return (mTemplateItem as ICloneable).Clone();
            else
                return Activator.CreateInstance(mTemplateItem.GetType());
        }

        #region Toolbar
        public delegate void EventAdd(ListBoxEx sender, object item, int pos);
        public event EventAdd OnAdd;
        public delegate void EventMoveUp(ListBoxEx sender, object item, int oldpos, int newpos);
        public event EventMoveUp OnMoveUp;
        public delegate void EventMoveDown(ListBoxEx sender, object item, int oldpos, int newpos);
        public event EventMoveDown OnMoveDown;
        public delegate void EventInsert(ListBoxEx sender, object item, int pos);
        public event EventInsert OnInsert;
        public delegate void EventDuplicate(ListBoxEx sender, object item, int srcpos, int newpos);
        public event EventDuplicate OnDuplicate;
        public delegate void EventRemove(ListBoxEx sender, object item, int pos);
        public event EventRemove OnRemove;
        public delegate void EventCopy(ListBoxEx sender, object item, int pos);
        public event EventCopy OnCopy;
        public delegate void EventPaste(ListBoxEx sender, object item, int pos);
        public event EventPaste OnPaste;

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = CreateInstance();
            if (item != null)
            {
                CurrentList.Add(CreateInstance());
                OnItemChanged(CurrentList.Count - 1);
                if (OnAdd != null) OnAdd(this, item, CurrentList.Count - 1);
            }
        }
        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) index = 0;
            var item = CreateInstance();
            if (item != null)
            {
                CurrentList.Insert(index, CreateInstance());
                OnItemChanged(index);
            }
            if (OnInsert != null) OnInsert(this, item, index);
        }
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            var obj = mCurrentList[index];
            if (obj is IDeepCloneable)
                mCurrentList.Insert(index++, (obj as IDeepCloneable).DeepClone());
            else if (obj is ICloneable)
                mCurrentList.Insert(index++, (obj as ICloneable).Clone());
            OnItemChanged(index);
            if (OnDuplicate != null) OnDuplicate(this, obj, index - 1, index);
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            var item = mCurrentList[index];
            mObjToCopy = item;
            mCurrentList.RemoveAt(index);
            OnItemChanged(-1);
            if (OnCopy != null) OnCopy(this, item, index);
            if (OnRemove != null) OnRemove(this, item, index);
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            var item = mCurrentList[index];
            mObjToCopy = item;
            if (OnCopy != null) OnCopy(this, item, index);
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            if (mObjToCopy is IDeepCloneable)
                mCurrentList.Insert(index, (mObjToCopy as IDeepCloneable).DeepClone());
            else if (mObjToCopy is ICloneable)
                mCurrentList.Insert(index, (mObjToCopy as ICloneable).Clone());
            OnItemChanged(index);
            if (OnPaste != null) OnPaste(this, mObjToCopy, index);
        }
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            object item = mCurrentList[index];
            mCurrentList.RemoveAt(index);
            mCurrentList.Insert(--index, item);
            OnItemChanged(index);
            if (OnMoveUp != null) OnMoveUp(this, item, index + 1, index);
        }
        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0 || index >= mCurrentList.Count) return;
            object item = mCurrentList[index];
            mCurrentList.RemoveAt(index);
            mCurrentList.Insert(++index, item);
            OnItemChanged(index);
            if (OnMoveDown != null) OnMoveDown(this, item, index - 1, index);
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox.SelectedIndex;
            if (index < 0) return;
            var item = mCurrentList[index];
            mCurrentList.RemoveAt(index);
            OnItemChanged(index);
            if (OnRemove != null) OnRemove(this, item, index);
        }
        #endregion

        private void listBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listBox.SelectedIndex = listBox.IndexFromPoint(e.Location);
                contextMenuStrip.Show(sender as Control, e.Location);
            }
        }
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (sender as ListBox).SelectedIndex;
            bool isItemSelected = index >= 0;
            bool canCreate = mCurrentList.Count > 0 || mTemplateItem != null;

            toolStripButtonAdd.Enabled = canCreate;
            toolStripButtonMoveUp.Enabled = index > 0;
            toolStripButtonMoveDown.Enabled = index >= 0 &&index < mCurrentList.Count - 1;
            toolStripButtonRemove.Enabled = isItemSelected;

            addToolStripMenuItem.Enabled = canCreate;
            insertToolStripMenuItem.Enabled = canCreate;
            duplicateToolStripMenuItem.Enabled = isItemSelected;
            cutToolStripMenuItem.Enabled = isItemSelected;
            copyToolStripMenuItem.Enabled = isItemSelected;
            pasteToolStripMenuItem.Enabled = isItemSelected;
            moveUpToolStripMenuItem.Enabled = index > 0;
            moveDownToolStripMenuItem.Enabled = index >= 0 && index < mCurrentList.Count - 1;
            removeToolStripMenuItem.Enabled = isItemSelected;

            if (index < 0) mItemSelected = null;
            else mItemSelected = CurrentList[index];
        }
        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            Action<ToolStripMenuItem> check_shortcut = null;

            check_shortcut = (node) =>
            {
                if (node.ShortcutKeys == e.KeyData)
                {
                    node.PerformClick();
                }
                foreach (ToolStripMenuItem child in node.DropDownItems)
                {
                    check_shortcut(child);
                }
            };

            foreach (ToolStripMenuItem item in contextMenuStrip.Items)
            {
                check_shortcut(item);
            }
        }
    }
}

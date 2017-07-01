namespace AnimEditor
{
    partial class FormAnim
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnim));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("A");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("B");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("C");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("D");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRecent1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRecent9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainerAnimations = new System.Windows.Forms.SplitContainer();
            this.listBoxAnimations = new libTools.Forms.ListBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.animPanel = new libTools.Forms.AnimPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripAnimPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimRewind = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimCenter = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimHitbox = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnimBackground = new System.Windows.Forms.ToolStripButton();
            this.groupBoxAnimation = new System.Windows.Forms.GroupBox();
            this.checkBoxEvent = new System.Windows.Forms.CheckBox();
            this.nEvent = new System.Windows.Forms.NumericUpDown();
            this.groupBoxLink = new System.Windows.Forms.GroupBox();
            this.labelLinkedAnimation = new System.Windows.Forms.Label();
            this.buttonLink = new System.Windows.Forms.Button();
            this.groupBoxHitbox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nHitboxBottom = new System.Windows.Forms.NumericUpDown();
            this.nHitboxRight = new System.Windows.Forms.NumericUpDown();
            this.nHitboxLeft = new System.Windows.Forms.NumericUpDown();
            this.nHitboxTop = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLoop = new System.Windows.Forms.CheckBox();
            this.frameSet = new AnimEditor.FrameSet();
            this.frameList = new AnimEditor.FrameList(this.components);
            this.comboBoxTextures = new System.Windows.Forms.ComboBox();
            this.nLoop = new System.Windows.Forms.NumericUpDown();
            this.nSpeed = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nFps = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.atlasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atlasToFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAnimations)).BeginInit();
            this.splitContainerAnimations.Panel1.SuspendLayout();
            this.splitContainerAnimations.Panel2.SuspendLayout();
            this.splitContainerAnimations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.groupBoxAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nEvent)).BeginInit();
            this.groupBoxLink.SuspendLayout();
            this.groupBoxHitbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLoop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFps)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.atlasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(752, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemRecent1,
            this.toolStripMenuItemRecent2,
            this.toolStripMenuItemRecent3,
            this.toolStripMenuItemRecent4,
            this.toolStripMenuItemRecent5,
            this.toolStripMenuItemRecent6,
            this.toolStripMenuItemRecent7,
            this.toolStripMenuItemRecent8,
            this.toolStripMenuItemRecent9,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveasToolStripMenuItem
            // 
            this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
            this.saveasToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveasToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveasToolStripMenuItem.Text = "Save &as...";
            this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItemRecent1
            // 
            this.toolStripMenuItemRecent1.Name = "toolStripMenuItemRecent1";
            this.toolStripMenuItemRecent1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent1.Tag = "1";
            this.toolStripMenuItemRecent1.Text = "&1:";
            this.toolStripMenuItemRecent1.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent2
            // 
            this.toolStripMenuItemRecent2.Name = "toolStripMenuItemRecent2";
            this.toolStripMenuItemRecent2.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent2.Tag = "2";
            this.toolStripMenuItemRecent2.Text = "&2:";
            this.toolStripMenuItemRecent2.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent3
            // 
            this.toolStripMenuItemRecent3.Name = "toolStripMenuItemRecent3";
            this.toolStripMenuItemRecent3.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent3.Tag = "3";
            this.toolStripMenuItemRecent3.Text = "&3:";
            this.toolStripMenuItemRecent3.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent4
            // 
            this.toolStripMenuItemRecent4.Name = "toolStripMenuItemRecent4";
            this.toolStripMenuItemRecent4.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent4.Tag = "4";
            this.toolStripMenuItemRecent4.Text = "&4:";
            this.toolStripMenuItemRecent4.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent5
            // 
            this.toolStripMenuItemRecent5.Name = "toolStripMenuItemRecent5";
            this.toolStripMenuItemRecent5.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent5.Tag = "5";
            this.toolStripMenuItemRecent5.Text = "&5:";
            this.toolStripMenuItemRecent5.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent6
            // 
            this.toolStripMenuItemRecent6.Name = "toolStripMenuItemRecent6";
            this.toolStripMenuItemRecent6.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent6.Tag = "6";
            this.toolStripMenuItemRecent6.Text = "&6:";
            this.toolStripMenuItemRecent6.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent7
            // 
            this.toolStripMenuItemRecent7.Name = "toolStripMenuItemRecent7";
            this.toolStripMenuItemRecent7.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent7.Tag = "7";
            this.toolStripMenuItemRecent7.Text = "&7:";
            this.toolStripMenuItemRecent7.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent8
            // 
            this.toolStripMenuItemRecent8.Name = "toolStripMenuItemRecent8";
            this.toolStripMenuItemRecent8.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent8.Tag = "8";
            this.toolStripMenuItemRecent8.Text = "&8:";
            this.toolStripMenuItemRecent8.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripMenuItemRecent9
            // 
            this.toolStripMenuItemRecent9.Name = "toolStripMenuItemRecent9";
            this.toolStripMenuItemRecent9.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItemRecent9.Tag = "9";
            this.toolStripMenuItemRecent9.Text = "&9:";
            this.toolStripMenuItemRecent9.Click += new System.EventHandler(this.toolStripMenuRecent_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Visible = false;
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(732, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // splitContainerAnimations
            // 
            this.splitContainerAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAnimations.Location = new System.Drawing.Point(0, 24);
            this.splitContainerAnimations.Name = "splitContainerAnimations";
            // 
            // splitContainerAnimations.Panel1
            // 
            this.splitContainerAnimations.Panel1.Controls.Add(this.listBoxAnimations);
            // 
            // splitContainerAnimations.Panel2
            // 
            this.splitContainerAnimations.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainerAnimations.Size = new System.Drawing.Size(752, 333);
            this.splitContainerAnimations.SplitterDistance = 119;
            this.splitContainerAnimations.TabIndex = 4;
            // 
            // listBoxAnimations
            // 
            this.listBoxAnimations.CurrentList = null;
            this.listBoxAnimations.DataSource = null;
            this.listBoxAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAnimations.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.listBoxAnimations.Location = new System.Drawing.Point(0, 0);
            this.listBoxAnimations.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxAnimations.Name = "listBoxAnimations";
            this.listBoxAnimations.SelectedIndex = -1;
            this.listBoxAnimations.Size = new System.Drawing.Size(119, 333);
            this.listBoxAnimations.TabIndex = 0;
            this.listBoxAnimations.TemplateItem = null;
            this.listBoxAnimations.OnSelectedIndexChanged += new libTools.Forms.ListBoxEx.SelectedIndexChanged(this.ListBox_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.animPanel);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxAnimation);
            this.splitContainer1.Size = new System.Drawing.Size(629, 333);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 6;
            // 
            // animPanel
            // 
            this.animPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.animPanel.CurrentFrameDictionary = null;
            this.animPanel.CurrentFrameIndex = 0;
            this.animPanel.CurrentFrameSequence = null;
            this.animPanel.CurrentTexture = null;
            this.animPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animPanel.IsCenterEnabled = false;
            this.animPanel.IsGridEnabled = false;
            this.animPanel.IsHitboxVisible = false;
            this.animPanel.Location = new System.Drawing.Point(0, 0);
            this.animPanel.Name = "animPanel";
            this.animPanel.Running = true;
            this.animPanel.Size = new System.Drawing.Size(252, 308);
            this.animPanel.TabIndex = 0;
            this.animPanel.Zoom = 1F;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAnimPause,
            this.toolStripAnimRewind,
            this.toolStripAnimCenter,
            this.toolStripAnimGrid,
            this.toolStripAnimHitbox,
            this.toolStripAnimZoomIn,
            this.toolStripAnimZoomOut,
            this.toolStripAnimBackground});
            this.toolStrip2.Location = new System.Drawing.Point(0, 308);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(252, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripAnimPause
            // 
            this.toolStripAnimPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimPause.Image = libTools.Resources.Icons.Pause;
            this.toolStripAnimPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimPause.Name = "toolStripAnimPause";
            this.toolStripAnimPause.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimPause.Text = "Play / Pause";
            this.toolStripAnimPause.Click += new System.EventHandler(this.toolStripAnimPause_Click);
            // 
            // toolStripAnimRewind
            // 
            this.toolStripAnimRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimRewind.Image = libTools.Resources.Icons.Refresh;
            this.toolStripAnimRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimRewind.Name = "toolStripAnimRewind";
            this.toolStripAnimRewind.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimRewind.Text = "Rewind";
            this.toolStripAnimRewind.Click += new System.EventHandler(this.toolStripAnimRewind_Click);
            // 
            // toolStripAnimCenter
            // 
            this.toolStripAnimCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimCenter.Image = libTools.Resources.Icons.TableInsideBorder;
            this.toolStripAnimCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimCenter.Name = "toolStripAnimCenter";
            this.toolStripAnimCenter.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimCenter.Text = "Center guide";
            this.toolStripAnimCenter.Click += new System.EventHandler(this.toolStripAnimCenter_Click);
            // 
            // toolStripAnimGrid
            // 
            this.toolStripAnimGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimGrid.Enabled = false;
            this.toolStripAnimGrid.Image = libTools.Resources.Icons.AppearanceTabGrid;
            this.toolStripAnimGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimGrid.Name = "toolStripAnimGrid";
            this.toolStripAnimGrid.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimGrid.Text = "Tile grid";
            this.toolStripAnimGrid.Click += new System.EventHandler(this.toolStripAnimGrid_Click);
            // 
            // toolStripAnimHitbox
            // 
            this.toolStripAnimHitbox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimHitbox.Image = libTools.Resources.Icons.AppearanceTabSwatch;
            this.toolStripAnimHitbox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimHitbox.Name = "toolStripAnimHitbox";
            this.toolStripAnimHitbox.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimHitbox.Text = "Hitbox";
            this.toolStripAnimHitbox.Click += new System.EventHandler(this.toolStripAnimHitbox_Click);
            // 
            // toolStripAnimZoomIn
            // 
            this.toolStripAnimZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimZoomIn.Image = libTools.Resources.Icons.ZoomIn;
            this.toolStripAnimZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimZoomIn.Name = "toolStripAnimZoomIn";
            this.toolStripAnimZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimZoomIn.Text = "ZoomIn";
            this.toolStripAnimZoomIn.Click += new System.EventHandler(this.toolStripAnimZoomIn_Click);
            // 
            // toolStripAnimZoomOut
            // 
            this.toolStripAnimZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimZoomOut.Image = libTools.Resources.Icons.ZoomOut;
            this.toolStripAnimZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimZoomOut.Name = "toolStripAnimZoomOut";
            this.toolStripAnimZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimZoomOut.Text = "Zoom out";
            this.toolStripAnimZoomOut.Click += new System.EventHandler(this.toolStripAnimZoomOut_Click);
            // 
            // toolStripAnimBackground
            // 
            this.toolStripAnimBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAnimBackground.Image = libTools.Resources.Icons.BackgroundColor;
            this.toolStripAnimBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnimBackground.Name = "toolStripAnimBackground";
            this.toolStripAnimBackground.Size = new System.Drawing.Size(23, 22);
            this.toolStripAnimBackground.Text = "Background Color";
            this.toolStripAnimBackground.Click += new System.EventHandler(this.toolStripAnimBackground_Click);
            // 
            // groupBoxAnimation
            // 
            this.groupBoxAnimation.Controls.Add(this.checkBoxEvent);
            this.groupBoxAnimation.Controls.Add(this.nEvent);
            this.groupBoxAnimation.Controls.Add(this.groupBoxLink);
            this.groupBoxAnimation.Controls.Add(this.groupBoxHitbox);
            this.groupBoxAnimation.Controls.Add(this.checkBoxLoop);
            this.groupBoxAnimation.Controls.Add(this.frameSet);
            this.groupBoxAnimation.Controls.Add(this.frameList);
            this.groupBoxAnimation.Controls.Add(this.comboBoxTextures);
            this.groupBoxAnimation.Controls.Add(this.nLoop);
            this.groupBoxAnimation.Controls.Add(this.nSpeed);
            this.groupBoxAnimation.Controls.Add(this.label6);
            this.groupBoxAnimation.Controls.Add(this.nFps);
            this.groupBoxAnimation.Controls.Add(this.label1);
            this.groupBoxAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAnimation.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAnimation.Name = "groupBoxAnimation";
            this.groupBoxAnimation.Size = new System.Drawing.Size(373, 333);
            this.groupBoxAnimation.TabIndex = 6;
            this.groupBoxAnimation.TabStop = false;
            this.groupBoxAnimation.Text = "Animation";
            // 
            // checkBoxEvent
            // 
            this.checkBoxEvent.AutoSize = true;
            this.checkBoxEvent.Location = new System.Drawing.Point(16, 117);
            this.checkBoxEvent.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxEvent.Name = "checkBoxEvent";
            this.checkBoxEvent.Size = new System.Drawing.Size(54, 17);
            this.checkBoxEvent.TabIndex = 18;
            this.checkBoxEvent.Text = "Event";
            this.checkBoxEvent.UseVisualStyleBackColor = true;
            this.checkBoxEvent.CheckedChanged += new System.EventHandler(this.checkBoxEvent_CheckedChanged);
            // 
            // nEvent
            // 
            this.nEvent.Location = new System.Drawing.Point(75, 116);
            this.nEvent.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nEvent.Name = "nEvent";
            this.nEvent.Size = new System.Drawing.Size(49, 20);
            this.nEvent.TabIndex = 17;
            this.nEvent.ValueChanged += new System.EventHandler(this.nEvent_ValueChanged);
            // 
            // groupBoxLink
            // 
            this.groupBoxLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLink.Controls.Add(this.labelLinkedAnimation);
            this.groupBoxLink.Controls.Add(this.buttonLink);
            this.groupBoxLink.Location = new System.Drawing.Point(201, 257);
            this.groupBoxLink.Name = "groupBoxLink";
            this.groupBoxLink.Size = new System.Drawing.Size(166, 68);
            this.groupBoxLink.TabIndex = 16;
            this.groupBoxLink.TabStop = false;
            this.groupBoxLink.Text = "Link";
            // 
            // labelLinkedAnimation
            // 
            this.labelLinkedAnimation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLinkedAnimation.Enabled = false;
            this.labelLinkedAnimation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLinkedAnimation.Location = new System.Drawing.Point(7, 21);
            this.labelLinkedAnimation.Name = "labelLinkedAnimation";
            this.labelLinkedAnimation.Size = new System.Drawing.Size(152, 13);
            this.labelLinkedAnimation.TabIndex = 1;
            this.labelLinkedAnimation.Text = "no animation currently linked";
            this.labelLinkedAnimation.Click += new System.EventHandler(this.labelLinkedAnimation_Click);
            // 
            // buttonLink
            // 
            this.buttonLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLink.Location = new System.Drawing.Point(6, 37);
            this.buttonLink.Name = "buttonLink";
            this.buttonLink.Size = new System.Drawing.Size(153, 23);
            this.buttonLink.TabIndex = 0;
            this.buttonLink.Text = "Link";
            this.buttonLink.UseVisualStyleBackColor = true;
            this.buttonLink.Click += new System.EventHandler(this.buttonLink_Click);
            // 
            // groupBoxHitbox
            // 
            this.groupBoxHitbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxHitbox.Controls.Add(this.label3);
            this.groupBoxHitbox.Controls.Add(this.label2);
            this.groupBoxHitbox.Controls.Add(this.nHitboxBottom);
            this.groupBoxHitbox.Controls.Add(this.nHitboxRight);
            this.groupBoxHitbox.Controls.Add(this.nHitboxLeft);
            this.groupBoxHitbox.Controls.Add(this.nHitboxTop);
            this.groupBoxHitbox.Location = new System.Drawing.Point(9, 250);
            this.groupBoxHitbox.Name = "groupBoxHitbox";
            this.groupBoxHitbox.Size = new System.Drawing.Size(186, 75);
            this.groupBoxHitbox.TabIndex = 15;
            this.groupBoxHitbox.TabStop = false;
            this.groupBoxHitbox.Text = "Hit box";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "R / B";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "L / T";
            // 
            // nHitboxBottom
            // 
            this.nHitboxBottom.Location = new System.Drawing.Point(121, 45);
            this.nHitboxBottom.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nHitboxBottom.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.nHitboxBottom.Name = "nHitboxBottom";
            this.nHitboxBottom.Size = new System.Drawing.Size(58, 20);
            this.nHitboxBottom.TabIndex = 3;
            this.nHitboxBottom.ValueChanged += new System.EventHandler(this.nHitboxBottom_ValueChanged);
            // 
            // nHitboxRight
            // 
            this.nHitboxRight.Location = new System.Drawing.Point(57, 45);
            this.nHitboxRight.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nHitboxRight.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.nHitboxRight.Name = "nHitboxRight";
            this.nHitboxRight.Size = new System.Drawing.Size(58, 20);
            this.nHitboxRight.TabIndex = 2;
            this.nHitboxRight.ValueChanged += new System.EventHandler(this.nHitboxRight_ValueChanged);
            // 
            // nHitboxLeft
            // 
            this.nHitboxLeft.Location = new System.Drawing.Point(57, 19);
            this.nHitboxLeft.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nHitboxLeft.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.nHitboxLeft.Name = "nHitboxLeft";
            this.nHitboxLeft.Size = new System.Drawing.Size(58, 20);
            this.nHitboxLeft.TabIndex = 1;
            this.nHitboxLeft.ValueChanged += new System.EventHandler(this.nHitboxLeft_ValueChanged);
            // 
            // nHitboxTop
            // 
            this.nHitboxTop.Location = new System.Drawing.Point(121, 19);
            this.nHitboxTop.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.nHitboxTop.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.nHitboxTop.Name = "nHitboxTop";
            this.nHitboxTop.Size = new System.Drawing.Size(58, 20);
            this.nHitboxTop.TabIndex = 0;
            this.nHitboxTop.ValueChanged += new System.EventHandler(this.nHitboxTop_ValueChanged);
            // 
            // checkBoxLoop
            // 
            this.checkBoxLoop.AutoSize = true;
            this.checkBoxLoop.Location = new System.Drawing.Point(16, 91);
            this.checkBoxLoop.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxLoop.Name = "checkBoxLoop";
            this.checkBoxLoop.Size = new System.Drawing.Size(50, 17);
            this.checkBoxLoop.TabIndex = 14;
            this.checkBoxLoop.Text = "Loop";
            this.checkBoxLoop.UseVisualStyleBackColor = true;
            this.checkBoxLoop.CheckedChanged += new System.EventHandler(this.checkBoxLoop_CheckedChanged);
            // 
            // frameSet
            // 
            this.frameSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.frameSet.CurrentFrame = null;
            this.frameSet.Location = new System.Drawing.Point(9, 144);
            this.frameSet.MaximumSize = new System.Drawing.Size(186, 100);
            this.frameSet.MinimumSize = new System.Drawing.Size(186, 100);
            this.frameSet.Name = "frameSet";
            this.frameSet.Size = new System.Drawing.Size(186, 100);
            this.frameSet.TabIndex = 3;
            this.frameSet.TabStop = false;
            this.frameSet.Text = "Frame";
            this.frameSet.FrameChanged += new AnimEditor.FrameSet.OnFrameChanged(this.frameSet_FrameChanged);
            // 
            // frameList
            // 
            this.frameList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameList.CurrentFrameDictionary = null;
            this.frameList.CurrentList = null;
            this.frameList.HideSelection = false;
            this.frameList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.frameList.LabelWrap = false;
            this.frameList.Location = new System.Drawing.Point(201, 64);
            this.frameList.Name = "frameList";
            this.frameList.OwnerDraw = true;
            this.frameList.Size = new System.Drawing.Size(166, 187);
            this.frameList.SpecialSelection = 0;
            this.frameList.SpriteSheet = null;
            this.frameList.TabIndex = 13;
            this.frameList.TileSize = new System.Drawing.Size(48, 48);
            this.frameList.UseCompatibleStateImageBehavior = false;
            this.frameList.View = System.Windows.Forms.View.Tile;
            this.frameList.SelectedIndexChanged += new System.EventHandler(this.frameList_SelectedIndexChanged);
            this.frameList.DoubleClick += new System.EventHandler(this.frameList_DoubleClick);
            // 
            // comboBoxTextures
            // 
            this.comboBoxTextures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTextures.FormattingEnabled = true;
            this.comboBoxTextures.Location = new System.Drawing.Point(9, 37);
            this.comboBoxTextures.Name = "comboBoxTextures";
            this.comboBoxTextures.Size = new System.Drawing.Size(358, 21);
            this.comboBoxTextures.TabIndex = 12;
            this.comboBoxTextures.SelectedIndexChanged += new System.EventHandler(this.comboBoxTextures_SelectedIndexChanged);
            // 
            // nLoop
            // 
            this.nLoop.Location = new System.Drawing.Point(75, 90);
            this.nLoop.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nLoop.Name = "nLoop";
            this.nLoop.Size = new System.Drawing.Size(49, 20);
            this.nLoop.TabIndex = 10;
            this.nLoop.ValueChanged += new System.EventHandler(this.nLoop_ValueChanged);
            // 
            // nSpeed
            // 
            this.nSpeed.Enabled = false;
            this.nSpeed.Location = new System.Drawing.Point(130, 64);
            this.nSpeed.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nSpeed.Name = "nSpeed";
            this.nSpeed.Size = new System.Drawing.Size(58, 20);
            this.nSpeed.TabIndex = 8;
            this.nSpeed.ValueChanged += new System.EventHandler(this.nSpeed_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "FPS";
            // 
            // nFps
            // 
            this.nFps.Location = new System.Drawing.Point(75, 64);
            this.nFps.Maximum = new decimal(new int[] {
            21600,
            0,
            0,
            0});
            this.nFps.Name = "nFps";
            this.nFps.Size = new System.Drawing.Size(49, 20);
            this.nFps.TabIndex = 6;
            this.nFps.ValueChanged += new System.EventHandler(this.nFps_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Spritesheet";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 299);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(732, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // atlasToolStripMenuItem
            // 
            this.atlasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.atlasToFramesToolStripMenuItem});
            this.atlasToolStripMenuItem.Name = "atlasToolStripMenuItem";
            this.atlasToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.atlasToolStripMenuItem.Text = "&Atlas";
            // 
            // atlasToFramesToolStripMenuItem
            // 
            this.atlasToFramesToolStripMenuItem.Name = "atlasToFramesToolStripMenuItem";
            this.atlasToFramesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.atlasToFramesToolStripMenuItem.Text = "Atlas to frames";
            this.atlasToFramesToolStripMenuItem.Click += new System.EventHandler(this.atlasToFramesToolStripMenuItem_Click);
            // 
            // FormAnim
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(752, 357);
            this.Controls.Add(this.splitContainerAnimations);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormAnim";
            this.Text = "Animation Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainerAnimations.Panel1.ResumeLayout(false);
            this.splitContainerAnimations.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAnimations)).EndInit();
            this.splitContainerAnimations.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.groupBoxAnimation.ResumeLayout(false);
            this.groupBoxAnimation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nEvent)).EndInit();
            this.groupBoxLink.ResumeLayout(false);
            this.groupBoxHitbox.ResumeLayout(false);
            this.groupBoxHitbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHitboxTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLoop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRecent9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainerAnimations;
        private libTools.Forms.ListBoxEx listBoxAnimations;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxAnimation;
        private FrameSet frameSet;
        private FrameList frameList;
        private System.Windows.Forms.ComboBox comboBoxTextures;
        private System.Windows.Forms.NumericUpDown nLoop;
        private System.Windows.Forms.NumericUpDown nSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nFps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private libTools.Forms.AnimPanel animPanel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripAnimPause;
        private System.Windows.Forms.ToolStripButton toolStripAnimRewind;
        private System.Windows.Forms.ToolStripButton toolStripAnimCenter;
        private System.Windows.Forms.ToolStripButton toolStripAnimGrid;
        private System.Windows.Forms.ToolStripButton toolStripAnimHitbox;
        private System.Windows.Forms.ToolStripButton toolStripAnimZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripAnimZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripAnimBackground;
        private System.Windows.Forms.CheckBox checkBoxLoop;
        private System.Windows.Forms.GroupBox groupBoxHitbox;
        private System.Windows.Forms.NumericUpDown nHitboxTop;
        private System.Windows.Forms.GroupBox groupBoxLink;
        private System.Windows.Forms.Label labelLinkedAnimation;
        private System.Windows.Forms.Button buttonLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nHitboxBottom;
        private System.Windows.Forms.NumericUpDown nHitboxRight;
        private System.Windows.Forms.NumericUpDown nHitboxLeft;
		private System.Windows.Forms.CheckBox checkBoxEvent;
		private System.Windows.Forms.NumericUpDown nEvent;
        private System.Windows.Forms.ToolStripMenuItem atlasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atlasToFramesToolStripMenuItem;
    }
}


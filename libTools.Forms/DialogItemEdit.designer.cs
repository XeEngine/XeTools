namespace libTools
{
    partial class DialogItemEdit
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxFont = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelPreview = new DoubleBufferedPanel();
            this.langItemEn = new LangItem();
            this.langItemIt = new LangItem();
            this.langItemFr = new LangItem();
            this.langItemDe = new LangItem();
            this.langItemSp = new LangItem();
            this.flowLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Controls.Add(this.langItemEn);
            this.flowLayoutPanel.Controls.Add(this.langItemIt);
            this.flowLayoutPanel.Controls.Add(this.langItemFr);
            this.flowLayoutPanel.Controls.Add(this.langItemDe);
            this.flowLayoutPanel.Controls.Add(this.langItemSp);
            this.flowLayoutPanel.Location = new System.Drawing.Point(12, 147);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(354, 308);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panelPreview);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxFont);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 129);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current font";
            // 
            // comboBoxFont
            // 
            this.comboBoxFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFont.FormattingEnabled = true;
            this.comboBoxFont.Location = new System.Drawing.Point(74, 19);
            this.comboBoxFont.Name = "comboBoxFont";
            this.comboBoxFont.Size = new System.Drawing.Size(274, 21);
            this.comboBoxFont.TabIndex = 0;
            this.comboBoxFont.SelectedIndexChanged += new System.EventHandler(this.comboBoxFont_SelectedIndexChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(291, 461);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(210, 461);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panelPreview
            // 
            this.panelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPreview.BackColor = System.Drawing.Color.Fuchsia;
            this.panelPreview.DoubleBuffered = false;
            this.panelPreview.Location = new System.Drawing.Point(9, 46);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(339, 77);
            this.panelPreview.TabIndex = 2;
            this.panelPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPreview_Paint);
            // 
            // langItemEn
            // 
            this.langItemEn.CurrentDescription = null;
            this.langItemEn.CurrentLanguage = libTools.Languages.English;
            this.langItemEn.CurrentName = null;
            this.langItemEn.Location = new System.Drawing.Point(3, 3);
            this.langItemEn.MinimumSize = new System.Drawing.Size(236, 78);
            this.langItemEn.Name = "langItemEn";
            this.langItemEn.Size = new System.Drawing.Size(326, 93);
            this.langItemEn.TabIndex = 0;
            this.langItemEn.OnTextChanged += new LangItem.TextChanged(this.langItem_OnTextChanged);
            // 
            // langItemIt
            // 
            this.langItemIt.CurrentDescription = null;
            this.langItemIt.CurrentLanguage = libTools.Languages.Italian;
            this.langItemIt.CurrentName = null;
            this.langItemIt.Location = new System.Drawing.Point(3, 102);
            this.langItemIt.MinimumSize = new System.Drawing.Size(236, 78);
            this.langItemIt.Name = "langItemIt";
            this.langItemIt.Size = new System.Drawing.Size(326, 93);
            this.langItemIt.TabIndex = 1;
            this.langItemIt.OnTextChanged += new LangItem.TextChanged(this.langItem_OnTextChanged);
            // 
            // langItemFr
            // 
            this.langItemFr.CurrentDescription = null;
            this.langItemFr.CurrentLanguage = libTools.Languages.French;
            this.langItemFr.CurrentName = null;
            this.langItemFr.Location = new System.Drawing.Point(3, 201);
            this.langItemFr.MinimumSize = new System.Drawing.Size(236, 78);
            this.langItemFr.Name = "langItemFr";
            this.langItemFr.Size = new System.Drawing.Size(326, 93);
            this.langItemFr.TabIndex = 2;
            this.langItemFr.OnTextChanged += new LangItem.TextChanged(this.langItem_OnTextChanged);
            // 
            // langItemDe
            // 
            this.langItemDe.CurrentDescription = null;
            this.langItemDe.CurrentLanguage = libTools.Languages.German;
            this.langItemDe.CurrentName = null;
            this.langItemDe.Location = new System.Drawing.Point(3, 300);
            this.langItemDe.MinimumSize = new System.Drawing.Size(236, 78);
            this.langItemDe.Name = "langItemDe";
            this.langItemDe.Size = new System.Drawing.Size(326, 93);
            this.langItemDe.TabIndex = 3;
            this.langItemDe.OnTextChanged += new LangItem.TextChanged(this.langItem_OnTextChanged);
            // 
            // langItemSp
            // 
            this.langItemSp.CurrentDescription = null;
            this.langItemSp.CurrentLanguage = libTools.Languages.Spanish;
            this.langItemSp.CurrentName = null;
            this.langItemSp.Location = new System.Drawing.Point(3, 399);
            this.langItemSp.MinimumSize = new System.Drawing.Size(236, 78);
            this.langItemSp.Name = "langItemSp";
            this.langItemSp.Size = new System.Drawing.Size(326, 93);
            this.langItemSp.TabIndex = 4;
            this.langItemSp.OnTextChanged += new LangItem.TextChanged(this.langItem_OnTextChanged);
            // 
            // DialogItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 495);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel);
            this.MaximumSize = new System.Drawing.Size(390, 795);
            this.MinimumSize = new System.Drawing.Size(390, 336);
            this.Name = "DialogItemEdit";
            this.Text = "Edit name and description";
            this.flowLayoutPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFont;
        private DoubleBufferedPanel panelPreview;
        private LangItem langItemEn;
        private LangItem langItemIt;
        private LangItem langItemFr;
        private LangItem langItemDe;
        private LangItem langItemSp;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}
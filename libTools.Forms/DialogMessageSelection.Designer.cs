namespace libTools.Forms
{
    partial class DialogMessageSelection
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
            this.comboBoxSegment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonEnglish = new System.Windows.Forms.RadioButton();
            this.radioButtonItalian = new System.Windows.Forms.RadioButton();
            this.radioButtonFrench = new System.Windows.Forms.RadioButton();
            this.radioButtonDetush = new System.Windows.Forms.RadioButton();
            this.radioButtonSpanish = new System.Windows.Forms.RadioButton();
            this.radioButtonJapanese = new System.Windows.Forms.RadioButton();
            this.comboBoxMessage = new libTools.Forms.ComboBoxEx();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxSegment
            // 
            this.comboBoxSegment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSegment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxSegment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSegment.FormattingEnabled = true;
            this.comboBoxSegment.Location = new System.Drawing.Point(12, 28);
            this.comboBoxSegment.Name = "comboBoxSegment";
            this.comboBoxSegment.Size = new System.Drawing.Size(257, 21);
            this.comboBoxSegment.TabIndex = 1;
            this.comboBoxSegment.TextUpdate += new System.EventHandler(this.comboBoxSegment_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Segment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Text";
            // 
            // textBoxText
            // 
            this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxText.Location = new System.Drawing.Point(12, 120);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(257, 20);
            this.textBoxText.TabIndex = 8;
            this.textBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(194, 190);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(113, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.radioButtonEnglish);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonItalian);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonFrench);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonDetush);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonSpanish);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonJapanese);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 146);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(257, 29);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // radioButtonEnglish
            // 
            this.radioButtonEnglish.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEnglish.Location = new System.Drawing.Point(3, 3);
            this.radioButtonEnglish.Name = "radioButtonEnglish";
            this.radioButtonEnglish.Size = new System.Drawing.Size(32, 23);
            this.radioButtonEnglish.TabIndex = 0;
            this.radioButtonEnglish.TabStop = true;
            this.radioButtonEnglish.Text = "EN";
            this.radioButtonEnglish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEnglish.UseVisualStyleBackColor = true;
            this.radioButtonEnglish.CheckedChanged += new System.EventHandler(this.radioButtonEnglish_CheckedChanged);
            // 
            // radioButtonItalian
            // 
            this.radioButtonItalian.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonItalian.Location = new System.Drawing.Point(41, 3);
            this.radioButtonItalian.Name = "radioButtonItalian";
            this.radioButtonItalian.Size = new System.Drawing.Size(32, 23);
            this.radioButtonItalian.TabIndex = 1;
            this.radioButtonItalian.TabStop = true;
            this.radioButtonItalian.Text = "IT";
            this.radioButtonItalian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonItalian.UseVisualStyleBackColor = true;
            this.radioButtonItalian.CheckedChanged += new System.EventHandler(this.radioButtonItalian_CheckedChanged);
            // 
            // radioButtonFrench
            // 
            this.radioButtonFrench.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonFrench.Location = new System.Drawing.Point(79, 3);
            this.radioButtonFrench.Name = "radioButtonFrench";
            this.radioButtonFrench.Size = new System.Drawing.Size(32, 23);
            this.radioButtonFrench.TabIndex = 2;
            this.radioButtonFrench.TabStop = true;
            this.radioButtonFrench.Text = "FR";
            this.radioButtonFrench.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonFrench.UseVisualStyleBackColor = true;
            this.radioButtonFrench.CheckedChanged += new System.EventHandler(this.radioButtonFrench_CheckedChanged);
            // 
            // radioButtonDetush
            // 
            this.radioButtonDetush.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonDetush.Location = new System.Drawing.Point(117, 3);
            this.radioButtonDetush.Name = "radioButtonDetush";
            this.radioButtonDetush.Size = new System.Drawing.Size(32, 23);
            this.radioButtonDetush.TabIndex = 3;
            this.radioButtonDetush.TabStop = true;
            this.radioButtonDetush.Text = "DE";
            this.radioButtonDetush.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonDetush.UseVisualStyleBackColor = true;
            this.radioButtonDetush.CheckedChanged += new System.EventHandler(this.radioButtonDetush_CheckedChanged);
            // 
            // radioButtonSpanish
            // 
            this.radioButtonSpanish.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonSpanish.Location = new System.Drawing.Point(155, 3);
            this.radioButtonSpanish.Name = "radioButtonSpanish";
            this.radioButtonSpanish.Size = new System.Drawing.Size(32, 23);
            this.radioButtonSpanish.TabIndex = 4;
            this.radioButtonSpanish.TabStop = true;
            this.radioButtonSpanish.Text = "SP";
            this.radioButtonSpanish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonSpanish.UseVisualStyleBackColor = true;
            this.radioButtonSpanish.CheckedChanged += new System.EventHandler(this.radioButtonSpanish_CheckedChanged);
            // 
            // radioButtonJapanese
            // 
            this.radioButtonJapanese.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonJapanese.Location = new System.Drawing.Point(193, 3);
            this.radioButtonJapanese.Name = "radioButtonJapanese";
            this.radioButtonJapanese.Size = new System.Drawing.Size(32, 23);
            this.radioButtonJapanese.TabIndex = 5;
            this.radioButtonJapanese.TabStop = true;
            this.radioButtonJapanese.Text = "JP";
            this.radioButtonJapanese.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonJapanese.UseVisualStyleBackColor = true;
            this.radioButtonJapanese.Visible = false;
            this.radioButtonJapanese.CheckedChanged += new System.EventHandler(this.radioButtonJapanese_CheckedChanged);
            // 
            // comboBoxMessage
            // 
            this.comboBoxMessage.FormattingEnabled = true;
            this.comboBoxMessage.Location = new System.Drawing.Point(12, 74);
            this.comboBoxMessage.Name = "comboBoxMessage";
            this.comboBoxMessage.Size = new System.Drawing.Size(257, 21);
            this.comboBoxMessage.TabIndex = 13;
            this.comboBoxMessage.SelectedIndexChanged += new System.EventHandler(this.comboBoxMessage_SelectedIndexChanged);
            // 
            // DialogMessageSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 225);
            this.Controls.Add(this.comboBoxMessage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxSegment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(65535, 500);
            this.MinimumSize = new System.Drawing.Size(195, 229);
            this.Name = "DialogMessageSelection";
            this.Text = "Message selection";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxSegment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButtonEnglish;
        private System.Windows.Forms.RadioButton radioButtonItalian;
        private System.Windows.Forms.RadioButton radioButtonFrench;
        private System.Windows.Forms.RadioButton radioButtonDetush;
        private System.Windows.Forms.RadioButton radioButtonSpanish;
        private System.Windows.Forms.RadioButton radioButtonJapanese;
        private ComboBoxEx comboBoxMessage;
    }
}
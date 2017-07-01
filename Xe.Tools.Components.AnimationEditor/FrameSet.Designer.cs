namespace AnimEditor
{
    partial class FrameSet
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCenter = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.nCenterY = new System.Windows.Forms.NumericUpDown();
            this.nCenterX = new System.Windows.Forms.NumericUpDown();
            this.nHeight = new System.Windows.Forms.NumericUpDown();
            this.nWidth = new System.Windows.Forms.NumericUpDown();
            this.nPosY = new System.Windows.Forms.NumericUpDown();
            this.nPosX = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nCenterY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCenterX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPosX)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCenter
            // 
            this.labelCenter.AutoSize = true;
            this.labelCenter.Location = new System.Drawing.Point(9, 73);
            this.labelCenter.Name = "labelCenter";
            this.labelCenter.Size = new System.Drawing.Size(38, 13);
            this.labelCenter.TabIndex = 7;
            this.labelCenter.Text = "Center";
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(9, 47);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(27, 13);
            this.labelSize.TabIndex = 4;
            this.labelSize.Text = "Size";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(9, 21);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(44, 13);
            this.labelPosition.TabIndex = 1;
            this.labelPosition.Text = "Position";
            // 
            // nCenterY
            // 
            this.nCenterY.Location = new System.Drawing.Point(121, 71);
            this.nCenterY.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nCenterY.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nCenterY.Name = "nCenterY";
            this.nCenterY.Size = new System.Drawing.Size(56, 20);
            this.nCenterY.TabIndex = 9;
            this.nCenterY.ValueChanged += new System.EventHandler(this.nCenterY_ValueChanged);
            // 
            // nCenterX
            // 
            this.nCenterX.Location = new System.Drawing.Point(59, 71);
            this.nCenterX.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nCenterX.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nCenterX.Name = "nCenterX";
            this.nCenterX.Size = new System.Drawing.Size(56, 20);
            this.nCenterX.TabIndex = 8;
            this.nCenterX.ValueChanged += new System.EventHandler(this.nCenterX_ValueChanged);
            // 
            // nHeight
            // 
            this.nHeight.Location = new System.Drawing.Point(121, 45);
            this.nHeight.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nHeight.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nHeight.Name = "nHeight";
            this.nHeight.Size = new System.Drawing.Size(56, 20);
            this.nHeight.TabIndex = 6;
            this.nHeight.ValueChanged += new System.EventHandler(this.nHeight_ValueChanged);
            // 
            // nWidth
            // 
            this.nWidth.Location = new System.Drawing.Point(59, 45);
            this.nWidth.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nWidth.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nWidth.Name = "nWidth";
            this.nWidth.Size = new System.Drawing.Size(56, 20);
            this.nWidth.TabIndex = 5;
            this.nWidth.ValueChanged += new System.EventHandler(this.nWidth_ValueChanged);
            // 
            // nPosY
            // 
            this.nPosY.Location = new System.Drawing.Point(121, 19);
            this.nPosY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nPosY.Name = "nPosY";
            this.nPosY.Size = new System.Drawing.Size(56, 20);
            this.nPosY.TabIndex = 3;
            this.nPosY.ValueChanged += new System.EventHandler(this.nPosY_ValueChanged);
            // 
            // nPosX
            // 
            this.nPosX.Location = new System.Drawing.Point(59, 19);
            this.nPosX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nPosX.Name = "nPosX";
            this.nPosX.Size = new System.Drawing.Size(56, 20);
            this.nPosX.TabIndex = 2;
            this.nPosX.ValueChanged += new System.EventHandler(this.nPosX_ValueChanged);
            // 
            // FrameSet
            // 
            this.Controls.Add(this.labelCenter);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.nCenterY);
            this.Controls.Add(this.nCenterX);
            this.Controls.Add(this.nHeight);
            this.Controls.Add(this.nWidth);
            this.Controls.Add(this.nPosY);
            this.Controls.Add(this.nPosX);
            this.MaximumSize = new System.Drawing.Size(186, 100);
            this.MinimumSize = new System.Drawing.Size(186, 100);
            this.Name = "FrameSet";
            this.Size = new System.Drawing.Size(186, 100);
            ((System.ComponentModel.ISupportInitialize)(this.nCenterY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nCenterX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPosX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCenter;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.NumericUpDown nCenterY;
        private System.Windows.Forms.NumericUpDown nCenterX;
        private System.Windows.Forms.NumericUpDown nHeight;
        private System.Windows.Forms.NumericUpDown nWidth;
        private System.Windows.Forms.NumericUpDown nPosY;
        private System.Windows.Forms.NumericUpDown nPosX;
    }
}

namespace AnimEditor
{
    partial class FrameList
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
            this.SuspendLayout();
            // 
            // FrameList
            // 
            this.HideSelection = false;
            this.LabelWrap = false;
            this.OwnerDraw = true;
            this.TileSize = new System.Drawing.Size(64, 64);
            this.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.FrameList_DrawItem);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

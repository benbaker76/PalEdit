namespace PalEdit
{
    partial class PaletteControl
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
			// PaletteControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
			this.DoubleBuffered = true;
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PaletteControl";
			this.Size = new System.Drawing.Size(256, 256);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaletteControl_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PaletteControl_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PaletteControl_KeyUp);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PaletteControl_MouseDoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PaletteControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaletteControl_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaletteControl_MouseUp);
			this.Resize += new System.EventHandler(this.PaletteControl_Resize);
			this.ResumeLayout(false);

        }

        #endregion

    }
}

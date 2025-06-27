namespace PalEdit
{
    partial class frmBitmap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBitmap));
            this.picBitmap = new PalEdit.ZoomPanPictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbPaint = new System.Windows.Forms.ToolStripButton();
            this.tsbColorPicker = new System.Windows.Forms.ToolStripButton();
            this.tsbSwapColor = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbGrid = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBitmap
            // 
            this.picBitmap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picBitmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBitmap.Image = null;
            this.picBitmap.Location = new System.Drawing.Point(0, 48);
            this.picBitmap.Name = "picBitmap";
            this.picBitmap.SelectRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.picBitmap.Size = new System.Drawing.Size(496, 403);
            this.picBitmap.TabIndex = 0;
            this.picBitmap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBitmap_MouseDown);
            this.picBitmap.MouseLeave += new System.EventHandler(this.picBitmap_MouseLeave);
            this.picBitmap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBitmap_MouseMove);
            this.picBitmap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBitmap_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPaint,
            this.tsbColorPicker,
            this.tsbSwapColor,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.tsbGrid});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(496, 48);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbPaint
            // 
            this.tsbPaint.AutoSize = false;
            this.tsbPaint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaint.Image = ((System.Drawing.Image)(resources.GetObject("tsbPaint.Image")));
            this.tsbPaint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaint.Name = "tsbPaint";
            this.tsbPaint.Size = new System.Drawing.Size(36, 36);
            this.tsbPaint.Text = "Paint";
            this.tsbPaint.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // tsbColorPicker
            // 
            this.tsbColorPicker.AutoSize = false;
            this.tsbColorPicker.Checked = true;
            this.tsbColorPicker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbColorPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbColorPicker.Image = ((System.Drawing.Image)(resources.GetObject("tsbColorPicker.Image")));
            this.tsbColorPicker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbColorPicker.Name = "tsbColorPicker";
            this.tsbColorPicker.Size = new System.Drawing.Size(36, 36);
            this.tsbColorPicker.Text = "Color Picker";
            this.tsbColorPicker.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // tsbSwapColor
            // 
            this.tsbSwapColor.AutoSize = false;
            this.tsbSwapColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSwapColor.Image = ((System.Drawing.Image)(resources.GetObject("tsbSwapColor.Image")));
            this.tsbSwapColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSwapColor.Name = "tsbSwapColor";
            this.tsbSwapColor.Size = new System.Drawing.Size(36, 36);
            this.tsbSwapColor.Text = "Swap Color";
            this.tsbSwapColor.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.AutoSize = false;
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tsbZoomIn.Image")));
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(36, 36);
            this.tsbZoomIn.Text = "Zoom In";
            this.tsbZoomIn.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.AutoSize = false;
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tsbZoomOut.Image")));
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(36, 36);
            this.tsbZoomOut.Text = "Zoom Out";
            this.tsbZoomOut.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // tsbGrid
            // 
            this.tsbGrid.AutoSize = false;
            this.tsbGrid.CheckOnClick = true;
            this.tsbGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGrid.Image = ((System.Drawing.Image)(resources.GetObject("tsbGrid.Image")));
            this.tsbGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGrid.Name = "tsbGrid";
            this.tsbGrid.Size = new System.Drawing.Size(36, 36);
            this.tsbGrid.Text = "Grid";
            this.tsbGrid.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 451);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(496, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmBitmap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 473);
            this.Controls.Add(this.picBitmap);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBitmap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bitmap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBitmap_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZoomPanPictureBox picBitmap;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbPaint;
        private System.Windows.Forms.ToolStripButton tsbColorPicker;
        private System.Windows.Forms.ToolStripButton tsbZoomOut;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tsbSwapColor;
        private System.Windows.Forms.ToolStripButton tsbZoomIn;
        private System.Windows.Forms.ToolStripButton tsbGrid;
    }
}
namespace PalEdit
{
    partial class ZoomPanPictureBox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.picBox = new PalEdit.MyPictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.picBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 256);
            this.panel1.TabIndex = 8;
            // 
            // picBox
            // 
            this.picBox.BackColor = System.Drawing.Color.Black;
            this.picBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Name = "picBox";
            this.picBox.SelectRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.picBox.ShowGrid = false;
            this.picBox.Size = new System.Drawing.Size(128, 133);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox.TabIndex = 7;
            this.picBox.TabStop = false;
            this.picBox.ZoomLevel = 0;
            // 
            // ZoomPanPictureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "ZoomPanPictureBox";
            this.Size = new System.Drawing.Size(256, 256);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MyPictureBox picBox;
        private System.Windows.Forms.Panel panel1;
    }
}

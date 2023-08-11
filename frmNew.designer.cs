namespace PalEdit
{
    partial class frmNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNew));
            this.butOK = new System.Windows.Forms.Button();
            this.lblHeight = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.butCancel = new System.Windows.Forms.Button();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.chkCreateBitmap = new System.Windows.Forms.CheckBox();
            this.rdoFormat8Bpp = new System.Windows.Forms.RadioButton();
            this.rdoFormat4Bpp = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(24, 112);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(111, 29);
            this.butOK.TabIndex = 4;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(170, 9);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 1;
            this.lblHeight.Text = "Height:";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(170, 25);
            this.nudHeight.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(108, 20);
            this.nudHeight.TabIndex = 2;
            this.nudHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(167, 112);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(111, 29);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(24, 25);
            this.nudWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(108, 20);
            this.nudWidth.TabIndex = 0;
            this.nudWidth.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(24, 9);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 0;
            this.lblWidth.Text = "Width:";
            // 
            // chkCreateBitmap
            // 
            this.chkCreateBitmap.AutoSize = true;
            this.chkCreateBitmap.Checked = true;
            this.chkCreateBitmap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateBitmap.Location = new System.Drawing.Point(170, 61);
            this.chkCreateBitmap.Name = "chkCreateBitmap";
            this.chkCreateBitmap.Size = new System.Drawing.Size(92, 17);
            this.chkCreateBitmap.TabIndex = 6;
            this.chkCreateBitmap.Text = "Create Bitmap";
            this.chkCreateBitmap.UseVisualStyleBackColor = true;
            // 
            // rdoFormat8Bpp
            // 
            this.rdoFormat8Bpp.AutoSize = true;
            this.rdoFormat8Bpp.Checked = true;
            this.rdoFormat8Bpp.Location = new System.Drawing.Point(24, 60);
            this.rdoFormat8Bpp.Name = "rdoFormat8Bpp";
            this.rdoFormat8Bpp.Size = new System.Drawing.Size(88, 17);
            this.rdoFormat8Bpp.TabIndex = 7;
            this.rdoFormat8Bpp.TabStop = true;
            this.rdoFormat8Bpp.Text = "8 Bpp Format";
            this.rdoFormat8Bpp.UseVisualStyleBackColor = true;
            // 
            // rdoFormat4Bpp
            // 
            this.rdoFormat4Bpp.AutoSize = true;
            this.rdoFormat4Bpp.Location = new System.Drawing.Point(24, 83);
            this.rdoFormat4Bpp.Name = "rdoFormat4Bpp";
            this.rdoFormat4Bpp.Size = new System.Drawing.Size(88, 17);
            this.rdoFormat4Bpp.TabIndex = 8;
            this.rdoFormat4Bpp.Text = "4 Bpp Format";
            this.rdoFormat4Bpp.UseVisualStyleBackColor = true;
            // 
            // frmNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 163);
            this.Controls.Add(this.rdoFormat4Bpp);
            this.Controls.Add(this.rdoFormat8Bpp);
            this.Controls.Add(this.chkCreateBitmap);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNew";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New";
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.CheckBox chkCreateBitmap;
        private System.Windows.Forms.RadioButton rdoFormat8Bpp;
        private System.Windows.Forms.RadioButton rdoFormat4Bpp;
    }
}
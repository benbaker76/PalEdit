namespace PalEdit
{
    partial class frmQuantize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuantize));
            this.butOK = new System.Windows.Forms.Button();
            this.lblColorCount = new System.Windows.Forms.Label();
            this.nudColorCount = new System.Windows.Forms.NumericUpDown();
            this.butCancel = new System.Windows.Forms.Button();
            this.nudColorOffset = new System.Windows.Forms.NumericUpDown();
            this.lblColorOffset = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudColorCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColorOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(24, 64);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(111, 29);
            this.butOK.TabIndex = 4;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // lblColorCount
            // 
            this.lblColorCount.AutoSize = true;
            this.lblColorCount.Location = new System.Drawing.Point(170, 9);
            this.lblColorCount.Name = "lblColorCount";
            this.lblColorCount.Size = new System.Drawing.Size(65, 13);
            this.lblColorCount.TabIndex = 1;
            this.lblColorCount.Text = "Color Count:";
            // 
            // nudColorCount
            // 
            this.nudColorCount.Location = new System.Drawing.Point(170, 25);
            this.nudColorCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudColorCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudColorCount.Name = "nudColorCount";
            this.nudColorCount.Size = new System.Drawing.Size(108, 20);
            this.nudColorCount.TabIndex = 2;
            this.nudColorCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(173, 64);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(111, 29);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // nudColorOffset
            // 
            this.nudColorOffset.Location = new System.Drawing.Point(24, 25);
            this.nudColorOffset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudColorOffset.Name = "nudColorOffset";
            this.nudColorOffset.Size = new System.Drawing.Size(108, 20);
            this.nudColorOffset.TabIndex = 0;
            // 
            // lblColorOffset
            // 
            this.lblColorOffset.AutoSize = true;
            this.lblColorOffset.Location = new System.Drawing.Point(24, 9);
            this.lblColorOffset.Name = "lblColorOffset";
            this.lblColorOffset.Size = new System.Drawing.Size(65, 13);
            this.lblColorOffset.TabIndex = 0;
            this.lblColorOffset.Text = "Color Offset:";
            // 
            // frmQuantize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 120);
            this.Controls.Add(this.nudColorOffset);
            this.Controls.Add(this.lblColorOffset);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.nudColorCount);
            this.Controls.Add(this.lblColorCount);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQuantize";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quantize";
            ((System.ComponentModel.ISupportInitialize)(this.nudColorCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColorOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label lblColorCount;
        private System.Windows.Forms.NumericUpDown nudColorCount;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.NumericUpDown nudColorOffset;
        private System.Windows.Forms.Label lblColorOffset;
    }
}
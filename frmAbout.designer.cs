namespace PalEdit
{
    partial class frmAbout
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
            this.lblAbout = new System.Windows.Forms.Label();
            this.butOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAbout
            // 
            this.lblAbout.Location = new System.Drawing.Point(106, 23);
            this.lblAbout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(200, 62);
            this.lblAbout.TabIndex = 0;
            this.lblAbout.Text = "PalEdit [VERSION]\r\nby Ben Baker";
            this.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(54, 119);
            this.butOK.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(222, 56);
            this.butOK.TabIndex = 1;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PalEdit.Properties.Resources.App;
            this.pictureBox1.Location = new System.Drawing.Point(26, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 192);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.lblAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "frmAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
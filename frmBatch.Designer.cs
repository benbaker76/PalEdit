namespace PalEdit
{
    partial class frmBatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatch));
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.butGO = new System.Windows.Forms.Button();
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.butBrowseSource = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.chkAddPaletteOffset = new System.Windows.Forms.CheckBox();
            this.chkCreateCombinedImage = new System.Windows.Forms.CheckBox();
            this.butBrowseDestination = new System.Windows.Forms.Button();
            this.chkQuantize = new System.Windows.Forms.CheckBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.lblColorPalette = new System.Windows.Forms.Label();
            this.butBrowsePalette = new System.Windows.Forms.Button();
            this.cboPalette = new System.Windows.Forms.ComboBox();
            this.chkSortSizes = new System.Windows.Forms.CheckBox();
            this.chkSortColors = new System.Windows.Forms.CheckBox();
            this.chkSwapMagentaWithTransparentIndex = new System.Windows.Forms.CheckBox();
            this.nudTransparentIndex = new System.Windows.Forms.NumericUpDown();
            this.lblTransparentIndex = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.grpSource.SuspendLayout();
            this.grpDestination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparentIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(17, 77);
            this.nudWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(108, 20);
            this.nudWidth.TabIndex = 4;
            this.nudWidth.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(17, 61);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 2;
            this.lblWidth.Text = "Width:";
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(426, 307);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(111, 29);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(163, 77);
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
            this.nudHeight.TabIndex = 5;
            this.nudHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(163, 61);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 3;
            this.lblHeight.Text = "Height:";
            // 
            // butGO
            // 
            this.butGO.Location = new System.Drawing.Point(12, 307);
            this.butGO.Name = "butGO";
            this.butGO.Size = new System.Drawing.Size(111, 29);
            this.butGO.TabIndex = 2;
            this.butGO.Text = "GO!";
            this.butGO.UseVisualStyleBackColor = true;
            this.butGO.Click += new System.EventHandler(this.butGO_Click);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.butBrowseSource);
            this.grpSource.Controls.Add(this.txtSource);
            this.grpSource.Location = new System.Drawing.Point(12, 16);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(525, 67);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source";
            // 
            // butBrowseSource
            // 
            this.butBrowseSource.Location = new System.Drawing.Point(478, 26);
            this.butBrowseSource.Name = "butBrowseSource";
            this.butBrowseSource.Size = new System.Drawing.Size(29, 21);
            this.butBrowseSource.TabIndex = 1;
            this.butBrowseSource.Text = "...";
            this.butBrowseSource.UseVisualStyleBackColor = true;
            this.butBrowseSource.Click += new System.EventHandler(this.butBrowseSource_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(20, 26);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(442, 20);
            this.txtSource.TabIndex = 0;
            // 
            // grpDestination
            // 
            this.grpDestination.Controls.Add(this.lblTransparentIndex);
            this.grpDestination.Controls.Add(this.nudTransparentIndex);
            this.grpDestination.Controls.Add(this.chkAddPaletteOffset);
            this.grpDestination.Controls.Add(this.chkCreateCombinedImage);
            this.grpDestination.Controls.Add(this.butBrowseDestination);
            this.grpDestination.Controls.Add(this.chkQuantize);
            this.grpDestination.Controls.Add(this.txtDestination);
            this.grpDestination.Controls.Add(this.lblColorPalette);
            this.grpDestination.Controls.Add(this.butBrowsePalette);
            this.grpDestination.Controls.Add(this.cboPalette);
            this.grpDestination.Controls.Add(this.chkSortSizes);
            this.grpDestination.Controls.Add(this.chkSortColors);
            this.grpDestination.Controls.Add(this.chkSwapMagentaWithTransparentIndex);
            this.grpDestination.Controls.Add(this.lblWidth);
            this.grpDestination.Controls.Add(this.lblHeight);
            this.grpDestination.Controls.Add(this.nudHeight);
            this.grpDestination.Controls.Add(this.nudWidth);
            this.grpDestination.Location = new System.Drawing.Point(12, 89);
            this.grpDestination.Name = "grpDestination";
            this.grpDestination.Size = new System.Drawing.Size(525, 202);
            this.grpDestination.TabIndex = 1;
            this.grpDestination.TabStop = false;
            this.grpDestination.Text = "Destination";
            // 
            // chkAddPaletteOffset
            // 
            this.chkAddPaletteOffset.AutoSize = true;
            this.chkAddPaletteOffset.Location = new System.Drawing.Point(307, 149);
            this.chkAddPaletteOffset.Name = "chkAddPaletteOffset";
            this.chkAddPaletteOffset.Size = new System.Drawing.Size(112, 17);
            this.chkAddPaletteOffset.TabIndex = 10;
            this.chkAddPaletteOffset.Text = "Add Palette Offset";
            this.chkAddPaletteOffset.UseVisualStyleBackColor = true;
            // 
            // chkCreateCombinedImage
            // 
            this.chkCreateCombinedImage.AutoSize = true;
            this.chkCreateCombinedImage.Checked = true;
            this.chkCreateCombinedImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateCombinedImage.Location = new System.Drawing.Point(307, 167);
            this.chkCreateCombinedImage.Name = "chkCreateCombinedImage";
            this.chkCreateCombinedImage.Size = new System.Drawing.Size(139, 17);
            this.chkCreateCombinedImage.TabIndex = 11;
            this.chkCreateCombinedImage.Text = "Create Combined Image";
            this.chkCreateCombinedImage.UseVisualStyleBackColor = true;
            // 
            // butBrowseDestination
            // 
            this.butBrowseDestination.Location = new System.Drawing.Point(478, 27);
            this.butBrowseDestination.Name = "butBrowseDestination";
            this.butBrowseDestination.Size = new System.Drawing.Size(29, 21);
            this.butBrowseDestination.TabIndex = 1;
            this.butBrowseDestination.Text = "...";
            this.butBrowseDestination.UseVisualStyleBackColor = true;
            this.butBrowseDestination.Click += new System.EventHandler(this.butBrowseDestination_Click);
            // 
            // chkQuantize
            // 
            this.chkQuantize.AutoSize = true;
            this.chkQuantize.Checked = true;
            this.chkQuantize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuantize.Location = new System.Drawing.Point(307, 131);
            this.chkQuantize.Name = "chkQuantize";
            this.chkQuantize.Size = new System.Drawing.Size(68, 17);
            this.chkQuantize.TabIndex = 9;
            this.chkQuantize.Text = "Quantize";
            this.chkQuantize.UseVisualStyleBackColor = true;
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(20, 27);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(442, 20);
            this.txtDestination.TabIndex = 0;
            // 
            // lblColorPalette
            // 
            this.lblColorPalette.AutoSize = true;
            this.lblColorPalette.Location = new System.Drawing.Point(17, 108);
            this.lblColorPalette.Name = "lblColorPalette";
            this.lblColorPalette.Size = new System.Drawing.Size(70, 13);
            this.lblColorPalette.TabIndex = 12;
            this.lblColorPalette.Text = "Color Palette:";
            // 
            // butBrowsePalette
            // 
            this.butBrowsePalette.Location = new System.Drawing.Point(150, 128);
            this.butBrowsePalette.Name = "butBrowsePalette";
            this.butBrowsePalette.Size = new System.Drawing.Size(29, 21);
            this.butBrowsePalette.TabIndex = 14;
            this.butBrowsePalette.Text = "...";
            this.butBrowsePalette.UseVisualStyleBackColor = true;
            this.butBrowsePalette.Click += new System.EventHandler(this.butBrowsePalette_Click);
            // 
            // cboPalette
            // 
            this.cboPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPalette.FormattingEnabled = true;
            this.cboPalette.Location = new System.Drawing.Point(17, 128);
            this.cboPalette.Name = "cboPalette";
            this.cboPalette.Size = new System.Drawing.Size(127, 21);
            this.cboPalette.TabIndex = 13;
            // 
            // chkSortSizes
            // 
            this.chkSortSizes.AutoSize = true;
            this.chkSortSizes.Checked = true;
            this.chkSortSizes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSortSizes.Location = new System.Drawing.Point(307, 113);
            this.chkSortSizes.Name = "chkSortSizes";
            this.chkSortSizes.Size = new System.Drawing.Size(73, 17);
            this.chkSortSizes.TabIndex = 8;
            this.chkSortSizes.Text = "Sort Sizes";
            this.chkSortSizes.UseVisualStyleBackColor = true;
            // 
            // chkSortColors
            // 
            this.chkSortColors.AutoSize = true;
            this.chkSortColors.Checked = true;
            this.chkSortColors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSortColors.Location = new System.Drawing.Point(307, 95);
            this.chkSortColors.Name = "chkSortColors";
            this.chkSortColors.Size = new System.Drawing.Size(77, 17);
            this.chkSortColors.TabIndex = 7;
            this.chkSortColors.Text = "Sort Colors";
            this.chkSortColors.UseVisualStyleBackColor = true;
            // 
            // chkSwapMagentaWithTransparentIndex
            // 
            this.chkSwapMagentaWithTransparentIndex.AutoSize = true;
            this.chkSwapMagentaWithTransparentIndex.Checked = true;
            this.chkSwapMagentaWithTransparentIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSwapMagentaWithTransparentIndex.Location = new System.Drawing.Point(307, 77);
            this.chkSwapMagentaWithTransparentIndex.Name = "chkSwapMagentaWithTransparentIndex";
            this.chkSwapMagentaWithTransparentIndex.Size = new System.Drawing.Size(212, 17);
            this.chkSwapMagentaWithTransparentIndex.TabIndex = 6;
            this.chkSwapMagentaWithTransparentIndex.Text = "Swap Magenta With Transparent Index";
            this.chkSwapMagentaWithTransparentIndex.UseVisualStyleBackColor = true;
            // 
            // nudTransparentIndex
            // 
            this.nudTransparentIndex.Location = new System.Drawing.Point(122, 162);
            this.nudTransparentIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTransparentIndex.Name = "nudTransparentIndex";
            this.nudTransparentIndex.Size = new System.Drawing.Size(57, 20);
            this.nudTransparentIndex.TabIndex = 15;
            // 
            // lblTransparentIndex
            // 
            this.lblTransparentIndex.AutoSize = true;
            this.lblTransparentIndex.Location = new System.Drawing.Point(17, 164);
            this.lblTransparentIndex.Name = "lblTransparentIndex";
            this.lblTransparentIndex.Size = new System.Drawing.Size(96, 13);
            this.lblTransparentIndex.TabIndex = 16;
            this.lblTransparentIndex.Text = "Transparent Index:";
            // 
            // frmBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 348);
            this.Controls.Add(this.grpDestination);
            this.Controls.Add(this.grpSource);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butGO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBatch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBatch_FormClosing);
            this.Load += new System.EventHandler(this.frmBatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpDestination.ResumeLayout(false);
            this.grpDestination.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparentIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Button butGO;
        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Button butBrowseSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.GroupBox grpDestination;
        private System.Windows.Forms.CheckBox chkSwapMagentaWithTransparentIndex;
        private System.Windows.Forms.CheckBox chkSortColors;
        private System.Windows.Forms.CheckBox chkSortSizes;
        private System.Windows.Forms.CheckBox chkQuantize;
        private System.Windows.Forms.Label lblColorPalette;
        private System.Windows.Forms.Button butBrowsePalette;
        private System.Windows.Forms.ComboBox cboPalette;
        private System.Windows.Forms.Button butBrowseDestination;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.CheckBox chkCreateCombinedImage;
        private System.Windows.Forms.CheckBox chkAddPaletteOffset;
        private System.Windows.Forms.Label lblTransparentIndex;
        private System.Windows.Forms.NumericUpDown nudTransparentIndex;
    }
}
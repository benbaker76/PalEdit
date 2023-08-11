namespace PalEdit
{
	partial class frmDonate
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDonate));
			this.butOK = new System.Windows.Forms.Button();
			this.txtBitcoin = new System.Windows.Forms.TextBox();
			this.txtEthereum = new System.Windows.Forms.TextBox();
			this.txtRaptoreum = new System.Windows.Forms.TextBox();
			this.tabDonate = new System.Windows.Forms.TabControl();
			this.tabPayPal = new System.Windows.Forms.TabPage();
			this.tabEthereum = new System.Windows.Forms.TabPage();
			this.tabBitcoin = new System.Windows.Forms.TabPage();
			this.tabRaptoreum = new System.Windows.Forms.TabPage();
			this.tabRavencoin = new System.Windows.Forms.TabPage();
			this.txtRavencoin = new System.Windows.Forms.TextBox();
			this.picPayPal = new System.Windows.Forms.PictureBox();
			this.pbEthereum = new System.Windows.Forms.PictureBox();
			this.pbBitcoin = new System.Windows.Forms.PictureBox();
			this.pbRavencoin = new System.Windows.Forms.PictureBox();
			this.pbRaptoreum = new System.Windows.Forms.PictureBox();
			this.tabDonate.SuspendLayout();
			this.tabPayPal.SuspendLayout();
			this.tabEthereum.SuspendLayout();
			this.tabBitcoin.SuspendLayout();
			this.tabRaptoreum.SuspendLayout();
			this.tabRavencoin.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPayPal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbEthereum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbBitcoin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRavencoin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRaptoreum)).BeginInit();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Location = new System.Drawing.Point(97, 344);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(111, 29);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// txtBitcoin
			// 
			this.txtBitcoin.HideSelection = false;
			this.txtBitcoin.Location = new System.Drawing.Point(9, 270);
			this.txtBitcoin.Name = "txtBitcoin";
			this.txtBitcoin.Size = new System.Drawing.Size(256, 20);
			this.txtBitcoin.TabIndex = 6;
			this.txtBitcoin.Text = "bc1q5jnsxgud5ajy86ppxez4pwkzx00920ahqnazkz";
			this.txtBitcoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtBitcoin.Click += new System.EventHandler(this.txtBitcoin_Click);
			// 
			// txtEthereum
			// 
			this.txtEthereum.HideSelection = false;
			this.txtEthereum.Location = new System.Drawing.Point(9, 270);
			this.txtEthereum.Name = "txtEthereum";
			this.txtEthereum.Size = new System.Drawing.Size(256, 20);
			this.txtEthereum.TabIndex = 7;
			this.txtEthereum.Text = "0x3056bc1F8B3cA0c22DAbbE1cdD66EE4947E96e55";
			this.txtEthereum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtEthereum.Click += new System.EventHandler(this.txtEthereum_Click);
			// 
			// txtRaptoreum
			// 
			this.txtRaptoreum.HideSelection = false;
			this.txtRaptoreum.Location = new System.Drawing.Point(9, 270);
			this.txtRaptoreum.Name = "txtRaptoreum";
			this.txtRaptoreum.Size = new System.Drawing.Size(256, 20);
			this.txtRaptoreum.TabIndex = 8;
			this.txtRaptoreum.Text = "RJby6dvr7sta1bMC456UkdqWwnqhmY79Ba";
			this.txtRaptoreum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtRaptoreum.Click += new System.EventHandler(this.txtRaptoreum_Click);
			// 
			// tabDonate
			// 
			this.tabDonate.Controls.Add(this.tabPayPal);
			this.tabDonate.Controls.Add(this.tabEthereum);
			this.tabDonate.Controls.Add(this.tabBitcoin);
			this.tabDonate.Controls.Add(this.tabRavencoin);
			this.tabDonate.Controls.Add(this.tabRaptoreum);
			this.tabDonate.Location = new System.Drawing.Point(12, 12);
			this.tabDonate.Name = "tabDonate";
			this.tabDonate.SelectedIndex = 0;
			this.tabDonate.Size = new System.Drawing.Size(282, 326);
			this.tabDonate.TabIndex = 9;
			// 
			// tabPayPal
			// 
			this.tabPayPal.Controls.Add(this.picPayPal);
			this.tabPayPal.Location = new System.Drawing.Point(4, 22);
			this.tabPayPal.Name = "tabPayPal";
			this.tabPayPal.Size = new System.Drawing.Size(274, 300);
			this.tabPayPal.TabIndex = 3;
			this.tabPayPal.Text = "PayPal";
			this.tabPayPal.UseVisualStyleBackColor = true;
			// 
			// tabEthereum
			// 
			this.tabEthereum.Controls.Add(this.pbEthereum);
			this.tabEthereum.Controls.Add(this.txtEthereum);
			this.tabEthereum.Location = new System.Drawing.Point(4, 22);
			this.tabEthereum.Name = "tabEthereum";
			this.tabEthereum.Padding = new System.Windows.Forms.Padding(3);
			this.tabEthereum.Size = new System.Drawing.Size(274, 300);
			this.tabEthereum.TabIndex = 1;
			this.tabEthereum.Text = "Ethereum";
			this.tabEthereum.UseVisualStyleBackColor = true;
			// 
			// tabBitcoin
			// 
			this.tabBitcoin.Controls.Add(this.txtBitcoin);
			this.tabBitcoin.Controls.Add(this.pbBitcoin);
			this.tabBitcoin.Location = new System.Drawing.Point(4, 22);
			this.tabBitcoin.Name = "tabBitcoin";
			this.tabBitcoin.Padding = new System.Windows.Forms.Padding(3);
			this.tabBitcoin.Size = new System.Drawing.Size(274, 300);
			this.tabBitcoin.TabIndex = 0;
			this.tabBitcoin.Text = "Bitcoin";
			this.tabBitcoin.UseVisualStyleBackColor = true;
			// 
			// tabRaptoreum
			// 
			this.tabRaptoreum.Controls.Add(this.pbRaptoreum);
			this.tabRaptoreum.Controls.Add(this.txtRaptoreum);
			this.tabRaptoreum.Location = new System.Drawing.Point(4, 22);
			this.tabRaptoreum.Name = "tabRaptoreum";
			this.tabRaptoreum.Size = new System.Drawing.Size(274, 300);
			this.tabRaptoreum.TabIndex = 2;
			this.tabRaptoreum.Text = "Raptoreum";
			this.tabRaptoreum.UseVisualStyleBackColor = true;
			// 
			// tabRavencoin
			// 
			this.tabRavencoin.Controls.Add(this.pbRavencoin);
			this.tabRavencoin.Controls.Add(this.txtRavencoin);
			this.tabRavencoin.Location = new System.Drawing.Point(4, 22);
			this.tabRavencoin.Name = "tabRavencoin";
			this.tabRavencoin.Size = new System.Drawing.Size(274, 300);
			this.tabRavencoin.TabIndex = 4;
			this.tabRavencoin.Text = "Ravencoin";
			this.tabRavencoin.UseVisualStyleBackColor = true;
			// 
			// txtRavencoin
			// 
			this.txtRavencoin.HideSelection = false;
			this.txtRavencoin.Location = new System.Drawing.Point(9, 270);
			this.txtRavencoin.Name = "txtRavencoin";
			this.txtRavencoin.Size = new System.Drawing.Size(256, 20);
			this.txtRavencoin.TabIndex = 10;
			this.txtRavencoin.Text = "RM2oU8VykXGP33Jqi4uZn9St9sF5MaWuax";
			this.txtRavencoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtRavencoin.Click += new System.EventHandler(this.txtRavencoin_Click);
			// 
			// picPayPal
			// 
			this.picPayPal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picPayPal.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picPayPal.Image = global::PalEdit.Properties.Resources.PayPal_Logo;
			this.picPayPal.Location = new System.Drawing.Point(9, 8);
			this.picPayPal.Name = "picPayPal";
			this.picPayPal.Size = new System.Drawing.Size(256, 256);
			this.picPayPal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picPayPal.TabIndex = 3;
			this.picPayPal.TabStop = false;
			this.picPayPal.Click += new System.EventHandler(this.picPayPal_Click);
			// 
			// pbEthereum
			// 
			this.pbEthereum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbEthereum.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbEthereum.Image = global::PalEdit.Properties.Resources.Ethereum_Logo;
			this.pbEthereum.Location = new System.Drawing.Point(9, 8);
			this.pbEthereum.Name = "pbEthereum";
			this.pbEthereum.Size = new System.Drawing.Size(256, 256);
			this.pbEthereum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbEthereum.TabIndex = 1;
			this.pbEthereum.TabStop = false;
			this.pbEthereum.Click += new System.EventHandler(this.pbEthereum_Click);
			// 
			// pbBitcoin
			// 
			this.pbBitcoin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbBitcoin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbBitcoin.Image = global::PalEdit.Properties.Resources.Bitcoin_Logo;
			this.pbBitcoin.Location = new System.Drawing.Point(9, 8);
			this.pbBitcoin.Name = "pbBitcoin";
			this.pbBitcoin.Size = new System.Drawing.Size(256, 256);
			this.pbBitcoin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbBitcoin.TabIndex = 0;
			this.pbBitcoin.TabStop = false;
			this.pbBitcoin.Click += new System.EventHandler(this.pbBitcoin_Click);
			// 
			// pbRavencoin
			// 
			this.pbRavencoin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbRavencoin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbRavencoin.Image = global::PalEdit.Properties.Resources.Ravencoin_Logo;
			this.pbRavencoin.Location = new System.Drawing.Point(9, 8);
			this.pbRavencoin.Name = "pbRavencoin";
			this.pbRavencoin.Size = new System.Drawing.Size(256, 256);
			this.pbRavencoin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbRavencoin.TabIndex = 9;
			this.pbRavencoin.TabStop = false;
			this.pbRavencoin.Click += new System.EventHandler(this.pbRavencoin_Click);
			// 
			// pbRaptoreum
			// 
			this.pbRaptoreum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbRaptoreum.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbRaptoreum.Image = global::PalEdit.Properties.Resources.Raptoreum_Logo;
			this.pbRaptoreum.Location = new System.Drawing.Point(9, 8);
			this.pbRaptoreum.Name = "pbRaptoreum";
			this.pbRaptoreum.Size = new System.Drawing.Size(256, 256);
			this.pbRaptoreum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbRaptoreum.TabIndex = 2;
			this.pbRaptoreum.TabStop = false;
			this.pbRaptoreum.Click += new System.EventHandler(this.pbRaptoreum_Click);
			// 
			// frmDonate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(303, 387);
			this.Controls.Add(this.tabDonate);
			this.Controls.Add(this.butOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmDonate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Donate";
			this.tabDonate.ResumeLayout(false);
			this.tabPayPal.ResumeLayout(false);
			this.tabEthereum.ResumeLayout(false);
			this.tabEthereum.PerformLayout();
			this.tabBitcoin.ResumeLayout(false);
			this.tabBitcoin.PerformLayout();
			this.tabRaptoreum.ResumeLayout(false);
			this.tabRaptoreum.PerformLayout();
			this.tabRavencoin.ResumeLayout(false);
			this.tabRavencoin.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPayPal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbEthereum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbBitcoin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRavencoin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRaptoreum)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pbBitcoin;
		private System.Windows.Forms.PictureBox pbEthereum;
		private System.Windows.Forms.PictureBox pbRaptoreum;
		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.TextBox txtBitcoin;
		private System.Windows.Forms.TextBox txtEthereum;
		private System.Windows.Forms.TextBox txtRaptoreum;
		private System.Windows.Forms.TabControl tabDonate;
		private System.Windows.Forms.TabPage tabBitcoin;
		private System.Windows.Forms.TabPage tabEthereum;
		private System.Windows.Forms.TabPage tabRaptoreum;
		private System.Windows.Forms.TabPage tabPayPal;
		private System.Windows.Forms.PictureBox picPayPal;
		private System.Windows.Forms.TabPage tabRavencoin;
		private System.Windows.Forms.PictureBox pbRavencoin;
		private System.Windows.Forms.TextBox txtRavencoin;
	}
}
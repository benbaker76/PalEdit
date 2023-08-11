using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
	public partial class frmDonate : Form
	{
		public frmDonate()
		{
			InitializeComponent();
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pbBitcoin_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtBitcoin.SelectAll();
			Clipboard.SetText(txtBitcoin.Text);
		}

		private void txtBitcoin_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtBitcoin.SelectAll();
			Clipboard.SetText(txtBitcoin.Text);
		}

		private void pbEthereum_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtEthereum.SelectAll();
			Clipboard.SetText(txtEthereum.Text);
		}

		private void txtEthereum_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtEthereum.SelectAll();
			Clipboard.SetText(txtEthereum.Text);
		}

		private void pbRavencoin_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtRavencoin.SelectAll();
			Clipboard.SetText(txtRavencoin.Text);
		}

		private void txtRavencoin_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtRavencoin.SelectAll();
			Clipboard.SetText(txtRavencoin.Text);
		}

		private void pbRaptoreum_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtRaptoreum.SelectAll();
			Clipboard.SetText(txtRaptoreum.Text);
		}

		private void txtRaptoreum_Click(object sender, EventArgs e)
		{
			SelectNone();
			txtRaptoreum.SelectAll();
			Clipboard.SetText(txtRaptoreum.Text);
		}

		private void picPayPal_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=benbaker@headsoft.com.au&item_name=PalEdit&currency_code=USD");
		}

		private void SelectNone()
		{
			txtBitcoin.SelectionLength = 0;
			txtEthereum.SelectionLength = 0;
			txtRavencoin.SelectionLength = 0;
			txtRaptoreum.SelectionLength = 0;
		}
	}
}

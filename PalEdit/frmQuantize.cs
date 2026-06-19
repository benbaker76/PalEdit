using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public partial class frmQuantize : Form
    {
        public frmQuantize(int colorCount)
        {
            InitializeComponent();

            nudColorCount.Maximum = 256;
            nudColorCount.Value = Math.Max(nudColorCount.Minimum, Math.Min(colorCount, nudColorCount.Maximum));
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public int ColorOffset
        {
            get { return (int)nudColorOffset.Value; }
        }

        public int ColorCount
        {
            get { return (int)nudColorCount.Value; }
        }

        public bool MaintainPalette
        {
            get { return chkMaintainPalette.Checked; }
        }
    }
}
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

            nudColorCount.Value = colorCount;
            nudColorCount.Maximum = 255;
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
    }
}
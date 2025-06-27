using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public partial class frmNew : Form
    {
        public frmNew()
        {
            InitializeComponent();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public int BitmapWidth
        {
            get { return (int)nudWidth.Value; }
        }

        public int BitmapHeight
        {
            get { return (int)nudHeight.Value; }
        }

        public bool CreateBitmap
        {
            get { return chkCreateBitmap.Checked; }
        }

        public bool Format8Bpp
        {
            get { return rdoFormat8Bpp.Checked; }
        }
		
		public bool Format4Bpp
        {
            get { return rdoFormat4Bpp.Checked; }
        }
    }
}
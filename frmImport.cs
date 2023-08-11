using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public partial class frmImport : Form
    {
        public frmImport()
        {
            InitializeComponent();

            Initialize();
        }

        public void Initialize()
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public void OpenBitmapFile(string fileName)
        {
            palImport.OpenBitmapFile(fileName);
        }

        public void OpenPaletteFile(string fileName)
        {
            palImport.OpenPaletteFile(fileName);
        }

        private void butOkay_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public PalNode[] Palette
        {
            get { return palImport.Palette; }
        }
    }
}
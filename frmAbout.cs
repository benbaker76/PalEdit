using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            lblAbout.Text = lblAbout.Text.Replace("[VERSION]", Globals.Version);
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PalEdit
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            var version = Assembly.GetExecutingAssembly().GetName().Version;

            lblAbout.Text = lblAbout.Text.Replace("[VERSION]", version.ToString(3));
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
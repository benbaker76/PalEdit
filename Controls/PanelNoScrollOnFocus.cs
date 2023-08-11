using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PalEdit.Controls
{
    partial class PanelNoScrollOnFocus : Panel
    {
        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }
    }
}

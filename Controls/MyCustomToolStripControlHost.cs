using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public class MyCustomToolStripControlHost : ToolStripControlHost
	{
		public MyCustomToolStripControlHost()
			: base(new Control())
		{
		}
		public MyCustomToolStripControlHost(Control c)
			: base(c)
		{
		}
	}
}

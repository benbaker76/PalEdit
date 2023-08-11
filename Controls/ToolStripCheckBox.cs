using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;

namespace PalEdit
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripCheckBox : MyCustomToolStripControlHost
	{
		// Call the base constructor passing in a CheckBox instance.
		public ToolStripCheckBox()
			: base(new CheckBox())
		{
			this.BackColor = Color.Transparent;
		}

		///
		/// Gets the numeric up down control.
		///
		/// The numeric up down control.
		public CheckBox CheckBoxControl
		{
			get
			{
				return Control as CheckBox;
			}
		}



		///
		/// Gets or sets the value.
		///
		/// The value.
		public bool Checked
		{
			get
			{
				return CheckBoxControl.Checked;
			}
			set
			{
				CheckBoxControl.Checked = value;
			}
		}

		///
		/// Subscribe and unsubscribe the control events you wish to expose.
		///
		/// The c.
		protected override void OnSubscribeControlEvents(Control c)
		{
			// Call the base so the base events are connected.
			base.OnSubscribeControlEvents(c);

			// Cast the control to a CheckBox control.
			CheckBox checkBoxControl = (CheckBox)c;

			// Add the event.
			checkBoxControl.CheckedChanged += new EventHandler(OnCheckedChanged);
		}

		///
		/// Subscribe and unsubscribe the control events you wish to expose.
		///
		/// The c.
		protected override void OnUnsubscribeControlEvents(Control c)
		{
			// Call the base method so the basic events are unsubscribed.
			base.OnUnsubscribeControlEvents(c);

			// Cast the control to a CheckBox control.
			CheckBox checkBoxControl = (CheckBox)c;

			// Remove the event.
			checkBoxControl.CheckedChanged -= new EventHandler(OnCheckedChanged);
		}

		// Declare the CheckedChanged event.
		public event EventHandler CheckedChanged;

		// Raise the CheckedChanged event.
		private void OnCheckedChanged(object sender, EventArgs e)
		{
			if (CheckedChanged != null)
			{
				CheckedChanged(this, e);
			}
		}
	}
}

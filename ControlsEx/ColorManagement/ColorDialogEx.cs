using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using ControlsEx.ColorManagement.ColorModels;

namespace ControlsEx.ColorManagement
{
	/// <summary>
	/// Zusammenfassung für ColorDialogEx.
	/// </summary>
	[DefaultProperty("Color")]
	public class ColorDialogEx : Component
	{
		#region variables
		private XYZ _xyz = XYZ.White;
		private int _alpha = 255;
		private ColorPicker.Mode _mode = ColorPicker.Mode.HSV_RGB;
		private ColorPicker.Fader _fader = ColorPicker.Fader.HSV_H;

		private bool _isLocked = false;

		public event EventHandler<ColorEventArgs> SelectedColorChanged;
		public event EventHandler<ColorEventArgs> SelectedColorComplete;

		#endregion
		public ColorDialogEx()
		{
		}
		public DialogResult ShowDialog()
		{
			return ShowDialog(null);
		}
		public DialogResult ShowDialog(IWin32Window owner)
		{
			DialogResult res = DialogResult.Cancel;
			using (ColorPicker frm = new ColorPicker(_mode, _fader))
			{
				frm.Color = _xyz;
				frm.Alpha = _alpha;
				frm.SelectedColorChanged += OnSelectedColorChanged;
				frm.SelectedColorComplete += OnSelectedColorComplete;
				res = frm.ShowDialog(owner);
				if (res == DialogResult.OK)
				{
					_xyz = frm.Color;
					_alpha = frm.Alpha;
					_mode = frm.SecondaryMode;
					_fader = frm.PrimaryFader;
					_isLocked = frm.IsLocked;
				}
			}
			return res;
		}

		private void OnSelectedColorChanged(object sender, ColorEventArgs e)
		{
			SelectedColorChanged?.Invoke(sender, e);
		}

		private void OnSelectedColorComplete(object sender, ColorEventArgs e)
		{
			SelectedColorComplete?.Invoke(sender, e);
		}

		#region properties

		[DefaultValue(typeof(Color), "White")]
		public Color Color
		{
			get { return Color.FromArgb(_alpha, _xyz.ToRGB().ToArgb()); }
			set
			{
				_xyz = XYZ.FromRGB(new RGB(value));
				_alpha = value.A;
			}
		}

		public bool IsLocked { get { return _isLocked; } }

		#endregion
	}
}

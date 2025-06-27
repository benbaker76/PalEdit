using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization;

using ControlsEx.ColorManagement.ColorModels;
using ControlsEx.ColorManagement.ColorModels.Selection;

namespace ControlsEx.ColorManagement
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public sealed partial class ColorPicker : System.Windows.Forms.Form
	{
		public enum Fader
		{
			HSV_H = 0,
			HSV_S = 1,
			HSV_V = 2,

			Second_1 = 3,
			Second_2 = 4,
			Second_3 = 5
		}
		public enum Mode
		{
			HSV_RGB = 0,
			HSV_LAB = 1
		}

		#region variables
		private ShiftKeyFilter filter;
		private ColorSelectionModule _module;
		private ColorSelectionModuleAlpha _alphaModule;
		private XYZ _color = XYZ.White;
		private int _alpha = 255;
		private Mode _mode = Mode.HSV_RGB;
		private Fader _fader = Fader.HSV_H;
		#endregion

		public event EventHandler<ColorEventArgs> SelectedColorChanged;
		public event EventHandler<ColorEventArgs> SelectedColorComplete;
		public ColorPicker() : this(Mode.HSV_RGB, Fader.HSV_H) { }
		public ColorPicker(Mode mode, Fader fader)
		{
			_mode = mode;
			_fader = fader;

			InitializeComponent();

			UpdateUI();
			filter = new ShiftKeyFilter();
			filter.ShiftStateChanged += new EventHandler(filter_ShiftStateChanged);
			Application.AddMessageFilter(filter);
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateUI();
		}

		#region ui updating
		public void UpdateUI()
		{
			ChangeModule();
			SetAlphaModule();
			ChangeDescriptions();
			UpdaterdFader();
			UpdatectxOptions();
			UpdatetbValue(null);

			_module.XYZ = _color;
			_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			lblColorOut.Color = lblColorOut.OldColor = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
		}
		#region module
		private void ChangeModule(ColorSelectionModule value)
		{
			if (value == _module) return;
			if (_module != null)
			{
				_module.ColorChanged -= new EventHandler(_module_ColorChanged);
				_module.ColorSelectionFader = null;
				_module.ColorSelectionPlane = null;
			}
			_module = value;
			if (_module != null)
			{
				_module.ColorChanged += new EventHandler(_module_ColorChanged);
				_module.XYZ = _color;
				_module.ColorSelectionFader = colorSelectionFader1;
				_module.ColorSelectionPlane = colorSelectionPlane1;
			}
		}
		private void ChangeModule()
		{
			switch (_fader)
			{
				case Fader.HSV_H: ChangeModule(new ColorSelectionModuleHSV_H()); break;
				case Fader.HSV_S: ChangeModule(new ColorSelectionModuleHSV_S()); break;
				case Fader.HSV_V: ChangeModule(new ColorSelectionModuleHSV_V()); break;
				case Fader.Second_1:
					if (_mode == Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_R());
					else
						ChangeModule(new ColorSelectionModuleLAB_L());
					break;
				case Fader.Second_2:
					if (_mode == Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_G());
					else
						ChangeModule(new ColorSelectionModuleLAB_a());
					break;
				default:
					if (_mode == Mode.HSV_RGB)
						ChangeModule(new ColorSelectionModuleRGB_B());
					else
						ChangeModule(new ColorSelectionModuleLAB_b()); break;
			}
		}
		private void ChangeDescriptions()
		{
			switch (_mode)
			{
				case Mode.HSV_RGB:
					rdSecond_1.Text = "R";
					rdSecond_2.Text = "G";
					rdSecond_3.Text = "B";
					break;
				default:
					rdSecond_1.Text = "L";
					rdSecond_2.Text = "a*";
					rdSecond_3.Text = "b*";
					break;
			}
		}
		private void SetAlphaModule()
		{
			if (_alphaModule != null) return;
			_alphaModule = new ColorSelectionModuleAlpha();
			_alphaModule.ColorChanged += new EventHandler(_alphaModule_ColorChanged);
			_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			_alphaModule.ColorSelectionFader = colorSelectionFader2;
		}
		#endregion
		#region contextmenu
		private void ctxOptions_Click(object sender, System.EventArgs e)
		{
			Mode newmode = _mode;
			if (sender == ctxPrevColor)
			{
				Color = XYZ.FromRGB(lblColorOut.OldColor);
				Alpha = lblColorOut.OldColor.A;
				return;
			}
			else if (sender == ctxCopy)
			{
				string str = ColorLabel.ColorToHexString(lblColorOut.Color);
				try
				{
					Clipboard.SetDataObject(str, true);
				}
				catch { }
				return;
			}
			//read checkbox
			else if (sender == ctxHSV_RGB)
				newmode = Mode.HSV_RGB;
			else if (sender == ctxHSV_LAB)
				newmode = Mode.HSV_LAB;
			//compare to old
			if (newmode == _mode) return;
			//update ui
			_mode = newmode;
			UpdatectxOptions();
			ChangeDescriptions();
			ChangeModule();
			UpdatetbValue(null);
		}
		private void UpdatectxOptions()
		{
			ctxHSV_RGB.Checked = _mode == Mode.HSV_RGB;
			ctxHSV_LAB.Checked = _mode == Mode.HSV_LAB;
		}
		#endregion
		#region rdFader
		private void UpdaterdFaderedChanged(object sender, System.EventArgs e)
		{
			if (sender == rdHSV_H)
				_fader = Fader.HSV_H;
			else if (sender == rdHSV_S)
				_fader = Fader.HSV_S;
			else if (sender == rdHSV_V)
				_fader = Fader.HSV_V;
			//secondary faders
			else if (sender == rdSecond_1)
				_fader = Fader.Second_1;
			else if (sender == rdSecond_2)
				_fader = Fader.Second_2;
			else//(sender==rdSecond_3)
				_fader = Fader.Second_3;

			ChangeModule();
		}
		private void UpdaterdFader()
		{
			if (_fader == Fader.HSV_H)
				rdHSV_H.Checked = true;
			else if (_fader == Fader.HSV_S)
				rdHSV_S.Checked = true;
			else if (_fader == Fader.HSV_V)
				rdHSV_V.Checked = true;
			else if (_fader == Fader.Second_1)
				rdSecond_1.Checked = true;
			else if (_fader == Fader.Second_2)
				rdSecond_2.Checked = true;
			else if (_fader == Fader.Second_3)
				rdSecond_3.Checked = true;
		}
		#endregion
		#region tbValue
		private void tbValue_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!(sender is TextBox)) return;
			if (e.KeyCode == Keys.Return)
			{
				UpdatetbValue(null);
				e.Handled = true;
				return;
			}
			double value;
			if (!double.TryParse(((TextBox)sender).Text,
				System.Globalization.NumberStyles.Integer,
				null, out value)) return;
			#region hsv  textboxes
			if (sender == tbHSV_H)
			{
				HSV chsv = HSV.FromRGB(_color.ToRGB());
				chsv.H = value / 360.0;
				_color = XYZ.FromRGB(chsv.ToRGB());
			}
			else if (sender == tbHSV_S)
			{
				HSV chsv = HSV.FromRGB(_color.ToRGB());
				chsv.S = value / 100.0;
				_color = XYZ.FromRGB(chsv.ToRGB());
			}
			else if (sender == tbHSV_V)
			{
				HSV chsv = HSV.FromRGB(_color.ToRGB());
				chsv.V = value / 100.0;
				_color = XYZ.FromRGB(chsv.ToRGB());
			}
			#endregion
			#region secondary textboxes
			else if (_mode == Mode.HSV_RGB)
			{
				RGB crgb = _color.ToRGB();
				if (sender == tbSecond_1)
				{
					crgb.R = value / 255.0;
				}
				else if (sender == tbSecond_2)
				{
					crgb.G = value / 255.0;
				}
				else //sender==tbSecond_3
				{
					crgb.B = value / 255.0;
				}
				_color = XYZ.FromRGB(crgb);
			}
			else if (_mode == Mode.HSV_LAB)
			{
				LAB clab = LAB.FromXYZ(_color);
				if (sender == tbSecond_1)
				{
					clab.L = value;
				}
				else if (sender == tbSecond_2)
				{
					clab.a = value;
				}
				else //sender==tbSecond_3
				{
					clab.b = value;
				}
				_color = clab.ToXYZ();
			}
			#endregion
			//update ui
			_module.XYZ = _color;
			lblColorOut.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			UpdatetbValue((TextBox)sender);
		}
		private void tbValue_Leave(object sender, System.EventArgs e)
		{
			UpdatetbValue(null);
		}
		private void UpdatetbValue(TextBox skipupdate)
		{
			#region hsv textboxes
			HSV chsv = HSV.FromRGB(_color.ToRGB());
			if (skipupdate != tbHSV_H)
				tbHSV_H.Text = (chsv.H * 360.0).ToString("0");
			if (skipupdate != tbHSV_S)
				tbHSV_S.Text = (chsv.S * 100.0).ToString("0");
			if (skipupdate != tbHSV_V)
				tbHSV_V.Text = (chsv.V * 100.0).ToString("0");
			#endregion
			#region secondary textboxes
			if (_mode == Mode.HSV_RGB)
			{
				RGB crgb = _color.ToRGB();
				if (skipupdate != tbSecond_1)
					tbSecond_1.Text = (crgb.R * 255.0).ToString("0");
				if (skipupdate != tbSecond_2)
					tbSecond_2.Text = (crgb.G * 255.0).ToString("0");
				if (skipupdate != tbSecond_3)
					tbSecond_3.Text = (crgb.B * 255.0).ToString("0");
			}
			else//(_mode==Mode.HSV_LAB)
			{
				LAB clab = LAB.FromXYZ(_color);
				if (skipupdate != tbSecond_1)
					tbSecond_1.Text = clab.L.ToString("0");
				if (skipupdate != tbSecond_2)
					tbSecond_2.Text = clab.a.ToString("0");
				if (skipupdate != tbSecond_3)
					tbSecond_3.Text = clab.b.ToString("0");
			}
			#endregion
		}

		#endregion
		private void filter_ShiftStateChanged(object sender, EventArgs e)
		{
			colorSelectionPlane1.Refresh();
			colorSelectionFader1.Refresh();
			colorSelectionFader2.Refresh();
		}

		private void eyeDropper1_SelectedColorChanged(object sender, ColorEventArgs e)
		{
			Color = XYZ.FromRGB(new RGB(eyeDropper1.SelectedColor));

			e.IsLocked = chkLock.Checked;

			SelectedColorChanged?.Invoke(sender, e);
		}

		private void eyeDropper1_SelectedColorComplete(object sender, ColorEventArgs e)
		{
			e.IsLocked = chkLock.Checked;

			SelectedColorComplete?.Invoke(sender, e);
		}

		private void chkLock_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;

			checkBox.ImageIndex = (checkBox.Checked ? 1 : 0);
		}
		#region module & lbl

		private void _module_ColorChanged(object sender, EventArgs e)
		{
			if (_module == null) return;
			_color = _module.XYZ;
			_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			lblColorOut.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			UpdatetbValue(null);
		}

		private void _alphaModule_ColorChanged(object sender, EventArgs e)
		{
			if (_alphaModule == null) return;
			_alpha = _alphaModule.Color.A;
			_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			lblColorOut.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			UpdatetbValue(null);
		}

		private void lblColorOut_ColorChanged(object sender, System.EventArgs e)
		{
			_color = XYZ.FromRGB(lblColorOut.Color);
			_alpha = lblColorOut.Color.A;
			_module.XYZ = _color;
			_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
			UpdatetbValue(null);
		}
		#endregion
		#endregion
		#region properties
		/// <summary>
		/// gets or sets the color as device-independent CIE-XYZ color
		/// </summary>
		[Description("gets or sets the color as device-independent CIE-XYZ color")]
		public XYZ Color
		{
			get { return _color; }
			set
			{
				if (value == _color) return;
				_color = _module.XYZ = value;
				_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
				lblColorOut.Color = lblColorOut.OldColor = System.Drawing.Color.FromArgb(_alpha, value.ToRGB());
				UpdatetbValue(null);
			}
		}
		public int Alpha
		{
			get { return _alpha; }
			set
			{
				if (value == _alpha) return;
				_alpha = value;
				_alphaModule.Color = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
				lblColorOut.Color = lblColorOut.OldColor = System.Drawing.Color.FromArgb(_alpha, _color.ToRGB());
				UpdatetbValue(null);
			}
		}
		[Browsable(false)]
		public Fader PrimaryFader
		{
			get { return _fader; }
			//			set
			//			{
			//				if(value==_fader) return;
			//				_fader=value;
			//				UpdaterdFader();
			//				ChangeModule();
			//			}
		}
		[Browsable(false)]
		public Mode SecondaryMode
		{
			get { return _mode; }
			//			set
			//			{
			//				if(value==_mode) return;
			//				_mode=value;
			//				UpdatectxOptions();
			//				ChangeModule();
			//				UpdatetbValue(null);
			//			}
		}
		public bool IsLocked {  get { return chkLock.Checked; } }
		#endregion
	}
}

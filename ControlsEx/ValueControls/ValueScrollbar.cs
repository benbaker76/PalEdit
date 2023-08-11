using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlsEx.ValueControls
{
	public abstract class ValueScrollBar : ValueControl
	{
		#region variablen
		protected ElementInfo[] m_elementArray = new ElementInfo[]
			{
				new ElementInfo(ElementState.normal),new ElementInfo(ElementState.normal),new ElementInfo(ElementState.normal)
			};
		private int m_offsetX, m_offsetY, m_selection = -1;
		private Timer m_timer;
		#endregion
		/// <summary>
		/// ctor
		/// </summary>
		public ValueScrollBar()
		{
			this.SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer, true);

			this.UpdateElementsLayout();
			this.m_timer = new Timer();
			this.m_timer.Tick += new EventHandler(m_timer_Tick);
		}
		#region helper
		/// <summary>
		/// determines, which element is hit by the mouse location
		/// </summary>
		protected int HitElement(int x, int y)
		{
			for (int i = 0; i < 3; i++)
				if (m_elementArray[i].Bounds.Contains(x, y))
					return i;
			return -1;
		}

		/// <summary>
		/// Updates the tracker position according to the value
		/// </summary>
		#endregion
		#region override
		protected abstract void UpdateElementsLayout();
		protected abstract void UpdateTrackerPosition();
		protected abstract void SetPosition(int x, int y);
		#endregion
		#region controller
		#region layout
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.UpdateElementsLayout();
			this.Refresh();
		}
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateElementsLayout();
			this.Refresh();
		}
		protected override void OnSetMaximum()
		{
			this.UpdateElementsLayout();
			this.Refresh();
		}
		protected override void OnSetMinimum()
		{
			this.UpdateElementsLayout();
			this.Refresh();
		}
		protected override void OnBeforeSetValue(int newvalue)
		{
			this.Invalidate(m_elementArray[2].Bounds);
		}
		protected override void OnAfterSetValue()
		{
			UpdateTrackerPosition();
			this.Invalidate(m_elementArray[2].Bounds);
			this.Update();
		}
		#endregion
		#region mouse actions
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.Enabled || e.Button != MouseButtons.Left) return;
			m_selection = this.HitElement(e.X, e.Y);
			switch (m_selection)
			{
				case 0: this.OnSpinDown(); break;
				case 1: this.OnSpinUp(); break;
				case 2:
					m_offsetX = e.X - m_elementArray[2].Bounds.X;
					m_offsetY = e.Y - m_elementArray[2].Bounds.Y;
					break;
				default:
					m_selection = 2;
					m_offsetX = m_elementArray[2].Bounds.Width / 2;
					m_offsetY = m_elementArray[2].Bounds.Height / 2;
					m_elementArray[2].State = ElementState.pushed;
					this.SetPosition(e.X - m_offsetX, e.Y - m_offsetY);
					return;
			}
			m_elementArray[m_selection].State = ElementState.pushed;
			this.Refresh(m_elementArray[m_selection].Bounds);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!this.Enabled) return;
			if (e.Button == MouseButtons.None)
			{
				int sel = this.HitElement(e.X, e.Y);
				if (sel == m_selection) return;
				if (sel != -1)
				{
					m_elementArray[sel].State = ElementState.hot;
					this.Invalidate(m_elementArray[sel].Bounds);
				}
				if (m_selection != -1)
				{
					m_elementArray[m_selection].State = ElementState.normal;
					this.Invalidate(m_elementArray[m_selection].Bounds);
				}
				m_selection = sel;
				this.Update();
			}
			else if (m_selection == 2 && e.Button == MouseButtons.Left)
			{
				this.SetPosition(e.X - m_offsetX, e.Y - m_offsetY);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.Enabled || e.Button != MouseButtons.Left || m_selection == -1) return;
			this.OnButtonUp();
			int sel = this.HitElement(e.X, e.Y);
			if (sel == m_selection)
			{
				m_elementArray[m_selection].State = ElementState.hot;
			}
			else if (sel != -1)
			{
				m_elementArray[m_selection].State = ElementState.normal;
				m_elementArray[sel].State = ElementState.hot;
				this.Invalidate(m_elementArray[sel].Bounds);
			}
			else
			{
				m_elementArray[m_selection].State = ElementState.normal;
			}
			this.Invalidate(m_elementArray[m_selection].Bounds);
			this.Update();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.Enabled) return;
			if (m_selection != -1)
			{
				m_elementArray[m_selection].State = ElementState.normal;
				this.Refresh(m_elementArray[m_selection].Bounds);
				m_selection = -1;
			}
		}
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!this.Enabled) this.m_timer.Stop();
			ElementState state = this.Enabled ? ElementState.normal : ElementState.disabled;
			for (int i = 0; i < m_elementArray.Length; i++)
				m_elementArray[i].State = state;
			m_selection = -1;
			this.Refresh();
		}
		private void OnSpinUp()
		{
			if (SetValueCore(Value + 1))
				RaiseValueChanged();
			m_timer.Interval = 400;
			this.m_timer.Start();
		}
		private void OnSpinDown()
		{
			if (SetValueCore(Value - 1))
				RaiseValueChanged();
			m_timer.Interval = 400;
			this.m_timer.Start();
		}
		private void OnButtonUp()
		{
			this.m_timer.Stop();
		}
		private void m_timer_Tick(object sender, EventArgs e)
		{
			this.m_timer.Interval = Math.Max(10, this.m_timer.Interval / 2);
			switch (m_selection)
			{
				case 1:
					if (!SetValueCore(Value + 1))
						this.m_timer.Stop();
					else
						RaiseValueChanged();
					break;
				case 0:
					if (!SetValueCore(Value - 1))
						this.m_timer.Stop();
					else
						RaiseValueChanged();
					break;
				default:
					this.m_timer.Stop(); break;
			}
		}
		#endregion
		#endregion

	}
	public class HValueScrollBar : ValueScrollBar
	{
		protected override void UpdateElementsLayout()
		{
			m_elementArray[0].Bounds = new Rectangle(0, 0, SystemInformation.HorizontalScrollBarHeight, this.Height);
			m_elementArray[1].Bounds = Rectangle.FromLTRB(
				this.Width - m_elementArray[0].Bounds.Width, 0, this.Width, this.Height);
			m_elementArray[2].Bounds.Height = this.Height;
			using (Graphics gr = this.CreateGraphics())
			{
				float width = Math.Max(
					gr.MeasureString(Maximum.ToString(), base.Font).Width,
					gr.MeasureString(Minimum.ToString(), base.Font).Width) + 4f;
				m_elementArray[2].Bounds.Width = (int)width;
			}
			UpdateTrackerPosition();
		}
		protected override void UpdateTrackerPosition()
		{
			m_elementArray[2].Bounds.Location = new Point(
				GetPercentage(m_elementArray[1].Bounds.Left - m_elementArray[0].Bounds.Right - m_elementArray[2].Bounds.Width) + m_elementArray[0].Bounds.Right,
				0);
		}
		protected override void SetPosition(int x, int y)
		{
			if (SetPercentage(x - m_elementArray[0].Bounds.Right,
				m_elementArray[1].Bounds.Left - m_elementArray[0].Bounds.Right - m_elementArray[2].Bounds.Width))
				RaiseValueChanged();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			IntPtr data = Win32.OpenThemeData2(this.Handle, "Scrollbar");
			if (data != IntPtr.Zero)//mit xp themes
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Win32.RECT rct = new Win32.RECT(0, 0, this.Width, this.Height);
				Win32.DrawThemeBackground2(data, hdc, 4, 4, ref rct);
				rct = base.m_elementArray[0].Bounds; rct.Top--;
				Win32.DrawThemeBackground2(data, hdc, 1, (int)base.m_elementArray[0].State + 8, ref rct);
				rct = base.m_elementArray[1].Bounds; rct.Top--;
				Win32.DrawThemeBackground2(data, hdc, 1, (int)base.m_elementArray[1].State + 12, ref rct);
				rct = base.m_elementArray[2].Bounds; rct.Top--;
				Win32.DrawThemeBackground2(data, hdc, 2, (int)base.m_elementArray[2].State, ref rct);
				e.Graphics.ReleaseHdc(hdc);
				Win32.CloseThemeData2(data);
			}
			else//ohne xpthemes, einfache rechtecke
			{
				e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, this.ClientRectangle);
				ControlPaint.DrawScrollButton(e.Graphics, base.m_elementArray[0].Bounds, ScrollButton.Left, base.m_elementArray[0].ToButtonState());
				ControlPaint.DrawScrollButton(e.Graphics, base.m_elementArray[1].Bounds, ScrollButton.Right, base.m_elementArray[1].ToButtonState());
				ControlPaint.DrawButton(e.Graphics, base.m_elementArray[2].Bounds, ButtonState.Normal);
			}
			using (StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap))
			{
				fmt.LineAlignment = fmt.Alignment = StringAlignment.Center;
				e.Graphics.DrawString(base.Value.ToString(), base.Font,
					this.Enabled ? Brushes.Black : Brushes.Gray, base.m_elementArray[2].Bounds, fmt);
			}
		}
	}
	public class VValueScrollBar : ValueScrollBar
	{
		protected override void UpdateElementsLayout()
		{
			m_elementArray[0].Bounds = new Rectangle(0, 0, this.Width, SystemInformation.VerticalScrollBarWidth);
			m_elementArray[1].Bounds = Rectangle.FromLTRB(0,
				this.Height - m_elementArray[0].Bounds.Height, this.Width, this.Height);
			m_elementArray[2].Bounds.Width = this.Width;
			using (Graphics gr = this.CreateGraphics())
			{
				float height = Math.Max(
					gr.MeasureString(Maximum.ToString(), base.Font).Width,
					gr.MeasureString(Minimum.ToString(), base.Font).Width) + 4f;
				m_elementArray[2].Bounds.Height = (int)height;
			}
			UpdateTrackerPosition();
		}
		protected override void UpdateTrackerPosition()
		{
			m_elementArray[2].Bounds.Location = new Point(
				0,
				GetPercentage(m_elementArray[1].Bounds.Top - m_elementArray[0].Bounds.Bottom - m_elementArray[2].Bounds.Height) + m_elementArray[0].Bounds.Bottom);
		}
		protected override void SetPosition(int x, int y)
		{
			if (SetPercentage(y - m_elementArray[0].Bounds.Bottom,
				m_elementArray[1].Bounds.Top - m_elementArray[0].Bounds.Bottom - m_elementArray[2].Bounds.Height))
				RaiseValueChanged();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			IntPtr data = Win32.OpenThemeData2(this.Handle, "Scrollbar");
			if (data != IntPtr.Zero)//mit xp themes
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Win32.RECT rct = new Win32.RECT(0, 0, this.Width, this.Height);
				Win32.DrawThemeBackground2(data, hdc, 6, 2, ref rct);
				rct = base.m_elementArray[0].Bounds; rct.Left--;
				Win32.DrawThemeBackground2(data, hdc, 1, (int)base.m_elementArray[0].State, ref rct);
				rct = base.m_elementArray[1].Bounds; rct.Left--;
				Win32.DrawThemeBackground2(data, hdc, 1, (int)base.m_elementArray[1].State + 4, ref rct);
				rct = base.m_elementArray[2].Bounds; rct.Left--;
				Win32.DrawThemeBackground2(data, hdc, 3, (int)base.m_elementArray[2].State, ref rct);
				e.Graphics.ReleaseHdc(hdc);
				Win32.CloseThemeData2(data);
			}
			else//ohne xpthemes, einfache rechtecke
			{
				e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, this.ClientRectangle);
				ControlPaint.DrawScrollButton(e.Graphics, base.m_elementArray[0].Bounds, ScrollButton.Up, base.m_elementArray[0].ToButtonState());
				ControlPaint.DrawScrollButton(e.Graphics, base.m_elementArray[1].Bounds, ScrollButton.Down, base.m_elementArray[1].ToButtonState());
				ControlPaint.DrawButton(e.Graphics, base.m_elementArray[2].Bounds, ButtonState.Normal);
			}
			base.m_elementArray[2].Bounds.X -= 3;
			using (StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap |
					  StringFormatFlags.DirectionVertical))
			{
				fmt.LineAlignment = fmt.Alignment = StringAlignment.Center;

				e.Graphics.DrawString(base.Value.ToString(), base.Font,
					this.Enabled ? Brushes.Black : Brushes.Gray, base.m_elementArray[2].Bounds, fmt);
			}
			base.m_elementArray[2].Bounds.X += 3;
		}
	}
}

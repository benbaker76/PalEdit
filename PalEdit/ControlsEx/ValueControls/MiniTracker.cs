using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ControlsEx.ValueControls
{
	/// <summary>
	/// encapsulates a trackbar
	/// </summary>
	public abstract class MiniTracker : ValueControl
	{
		#region variables
		protected int m_offsetX, m_offsetY;
		protected ElementInfo m_tracker;
		#endregion
		/// <summary>
		/// ctor
		/// </summary>
		public MiniTracker()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw, true);
			m_tracker = new ElementInfo();
			m_tracker.State = ElementState.hot;
			m_tracker.Bounds = new Rectangle(0, this.Height / 2 - 7, 11, 20);
			UpdateTrackerPosition();
		}
		#region helper
		/// <summary>
		/// sets the value of the tracker according to the position
		/// </summary>
		protected abstract bool SetValue(int x, int y);
		protected abstract void UpdateTrackerPosition();
		protected override void OnAfterSetValue()
		{
			UpdateTrackerPosition();
		}
		/// <summary>
		/// gets the position of the center of the tracker
		/// </summary>
		public Point GetTrackerPos()
		{
			Point pt = m_tracker.Bounds.Location;
			pt.Offset(m_tracker.Bounds.Width / 2,
				m_tracker.Bounds.Height / 2);
			return pt;
		}
		#endregion
		#region controller
		// make sure the tracker is aligned correct
		protected override void OnSizeChanged(EventArgs e)
		{
			this.UpdateTrackerPosition();
			base.OnSizeChanged(e);
		}
		// makes sure the tracker is enabled correct
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			m_tracker.State = this.Enabled ?
				ElementState.hot :
				ElementState.disabled;
			this.Refresh();
		}
		#region mouse actions
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!this.Enabled) return;
			if (m_tracker.Bounds.Contains(e.X, e.Y))
			{
				m_offsetX = m_tracker.Bounds.X - e.X;
				m_offsetY = m_tracker.Bounds.Y - e.Y;
			}
			else
			{
				m_offsetX = -m_tracker.Bounds.Width / 2;
				m_offsetY = -m_tracker.Bounds.Height / 2;
				this.SetValue(e.X + m_offsetX, e.Y + m_offsetX);
			}
			m_tracker.State = ElementState.pushed;
			this.Refresh();
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!this.Enabled || e.Button != MouseButtons.Left || m_tracker.State != ElementState.pushed) return;
			if (this.SetValue(e.X + m_offsetX, e.Y + m_offsetY))
				this.Refresh();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!this.Enabled) return;
			m_tracker.State = ElementState.hot;
			m_offsetX = m_offsetY = 0;
			this.Refresh();
		}
		#endregion
		#endregion
		/// <summary>
		/// sets the specified values of the tracker
		/// </summary>
		public void Assign(int value, int maximum, int minimum)
		{
			maximum = Math.Max(maximum, minimum + 1);
			value = Math.Max(minimum, Math.Min(maximum, value));
			base.SetMinimumCore(minimum);
			base.SetMaximumCore(maximum);
			base.SetValueCore(value);
		}
	}
	/// <summary>
	/// horizontal implementation of the minitracker control
	/// </summary>
	public class HMiniTracker : MiniTracker
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			IntPtr data = Win32.OpenThemeData2(this.Handle, "Trackbar");
			if (data != IntPtr.Zero)
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Win32.RECT rct = new Win32.RECT(
					m_tracker.Bounds.Width / 2, this.Height / 2 - 2,
					this.Width - m_tracker.Bounds.Width, 4);
				Win32.DrawThemeBackground2(data, hdc, 1, 1, ref rct);
				rct = m_tracker.Bounds;
				Win32.DrawThemeBackground2(data, hdc, 4, 1 + (int)m_tracker.State, ref rct);
				Win32.CloseThemeData2(hdc);
				e.Graphics.ReleaseHdc(hdc);
			}
			else
			{
				ControlPaint.DrawBorder3D(e.Graphics,
					m_tracker.Bounds.Width / 2, this.Height / 2 - 2,
					this.Width - m_tracker.Bounds.Width, 4,
					Border3DStyle.SunkenOuter, Border3DSide.All);
				ControlPaint.DrawButton(e.Graphics, m_tracker.Bounds, ButtonState.Normal);
			}
		}
		protected override void UpdateTrackerPosition()
		{
			m_tracker.Bounds.X =
				m_tracker.Bounds.Width / 2 + GetPercentage(
				this.Width - m_tracker.Bounds.Width * 2);
		}
		protected override bool SetValue(int x, int y)
		{
			if (this.Width <= this.m_tracker.Bounds.Width * 2) return false;
			int oldvalue = Value;
			if (SetPercentage(x - this.m_tracker.Bounds.Width / 2,
				this.Width - this.m_tracker.Bounds.Width * 2))
			{
				base.RaiseValueChanged();
			}
			return true;
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			m_tracker.Bounds.Y = (this.Height - this.m_tracker.Bounds.Height) / 2;
			base.OnSizeChanged(e);
		}
	}
	/// <summary>
	/// horizontal implementation of the minitracker control
	/// </summary>
	public class VMiniTracker : MiniTracker
	{
		public VMiniTracker()
		{
			m_tracker.Bounds.Size = new Size(20, 11);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			IntPtr data = Win32.OpenThemeData2(this.Handle, "Trackbar");
			if (data != IntPtr.Zero)
			{
				IntPtr hdc = e.Graphics.GetHdc();
				Win32.RECT rct = new Win32.RECT(
					this.Width / 2 - 2, m_tracker.Bounds.Height / 2,
					 4, this.Height - m_tracker.Bounds.Height);
				Win32.DrawThemeBackground2(data, hdc, 1, 1, ref rct);
				rct = m_tracker.Bounds;
				Win32.DrawThemeBackground2(data, hdc, 8, 1 + (int)m_tracker.State, ref rct);
				Win32.CloseThemeData2(hdc);
				e.Graphics.ReleaseHdc(hdc);
			}
			else
			{
				ControlPaint.DrawBorder3D(e.Graphics,
					m_tracker.Bounds.Width / 2, this.Height / 2 - 2,
					this.Width - m_tracker.Bounds.Width, 4,
					Border3DStyle.SunkenOuter, Border3DSide.All);
				ControlPaint.DrawButton(e.Graphics, m_tracker.Bounds, ButtonState.Normal);
			}
		}
		protected override void UpdateTrackerPosition()
		{
			m_tracker.Bounds.Y =
				m_tracker.Bounds.Height / 2 + 1 + GetPercentage(
				this.Height - m_tracker.Bounds.Height * 2);
		}
		protected override bool SetValue(int x, int y)
		{
			if (this.Height <= this.m_tracker.Bounds.Height * 2) return false;
			int oldvalue = Value;
			if (SetPercentage(y - m_tracker.Bounds.Height / 2 + 1,
				this.Height - m_tracker.Bounds.Height * 2))
			{
				base.RaiseValueChanged();
			}
			return true;
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			m_tracker.Bounds.X = (this.Width - this.m_tracker.Bounds.Width) / 2;
			base.OnSizeChanged(e);
		}
	}

}

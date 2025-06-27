using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Resources;

namespace ControlsEx.ColorManagement
{
	public partial class EyeDropper : Control
	{
		private Bitmap m_screenCaptureBitmap = null;

		private Bitmap m_eyeDropperBitmap = null;
		private Cursor m_eyeDropperCursor = null;

		private Color m_selectedColor = Color.Black;

		private float m_pixelPreviewZoom = 4.0f;

		private bool m_isCapturing = false;

		private bool m_isLocked = false;
		private int m_index = 0;

		public event EventHandler<ColorEventArgs> SelectedColorChanged;
		public event EventHandler<ColorEventArgs> SelectedColorComplete;

		public EyeDropper()
		{
			this.Size = new Size(32, 32);

			ResourceManager resourceManager = new ResourceManager("ControlsEx.ColorManagement.EyeDropper", GetType().Assembly);

			using (MemoryStream memoryStream = new MemoryStream((byte[])resourceManager.GetObject("EyeDropperCursor")))
				m_eyeDropperCursor = new Cursor(memoryStream);

			m_eyeDropperBitmap = new Bitmap((Bitmap)resourceManager.GetObject("EyeDropperBitmap"));

			InitializeComponent();

			CalcSnapshotSize();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			Rectangle controlRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

			PaintScreenCapture(pe.Graphics);

			pe.Graphics.DrawRectangle(SystemPens.ControlDark, controlRectangle);
			pe.Graphics.DrawLine(SystemPens.ControlLightLight, 0, controlRectangle.Bottom, controlRectangle.Right, controlRectangle.Bottom);
			pe.Graphics.DrawLine(SystemPens.ControlLightLight, controlRectangle.Right, 0, controlRectangle.Right, controlRectangle.Bottom);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				Cursor = m_eyeDropperCursor;
				Cursor.Position = this.Parent.PointToScreen(new Point(this.Left + 2, this.Bottom - 4));
				m_isCapturing = true;

				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (m_isCapturing)
			{
				CaptureScreen();

				this.Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			ColorEventArgs colorEventArgs = new ColorEventArgs(m_selectedColor, m_index);

			if (SelectedColorComplete != null)
			{
				SelectedColorComplete(this, colorEventArgs);

				m_isLocked = colorEventArgs.IsLocked;
				m_index = colorEventArgs.Index;
			}

			Cursor = Cursors.Arrow;
			m_isCapturing = false;

			this.Invalidate();
		}

		private void CaptureScreen()
		{
			Point mousePoint = Control.MousePosition;
			mousePoint.X -= m_screenCaptureBitmap.Width / 2;
			mousePoint.Y -= m_screenCaptureBitmap.Height / 2;

			using (System.Drawing.Graphics dc = System.Drawing.Graphics.FromImage(m_screenCaptureBitmap))
			{
				dc.CopyFromScreen(mousePoint, new Point(0, 0), m_screenCaptureBitmap.Size);

				Color selectedColor = m_screenCaptureBitmap.GetPixel((int)(m_screenCaptureBitmap.Size.Width / 2.0f), (int)(m_screenCaptureBitmap.Size.Height / 2.0f));

				if (selectedColor != m_selectedColor)
				{
					m_selectedColor = selectedColor;

					SelectedColorChanged?.Invoke(this, new ColorEventArgs(m_selectedColor, m_index));
				}
			}
		}

		void CalcSnapshotSize()
		{
			if (m_screenCaptureBitmap != null)
			{
				m_screenCaptureBitmap.Dispose();
				m_screenCaptureBitmap = null;
			}

			int screenCaptureWidth = (int)(Math.Floor(this.Width / m_pixelPreviewZoom));
			int screenCaptureHeight = (int)(Math.Floor(this.Height / m_pixelPreviewZoom));

			m_screenCaptureBitmap = new Bitmap(screenCaptureWidth, screenCaptureHeight);
		}

		public void PaintScreenCapture(Graphics graphics)
		{
			if (m_isCapturing)
			{
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				Rectangle screenRectangle = new Rectangle(0, 0, this.Width, this.Height);
				Point screenPoint = new Point(this.Width / 2, this.Height / 2);
				graphics.DrawImage(m_screenCaptureBitmap, screenRectangle);
				bool useBlack = (m_selectedColor.R + m_selectedColor.G + m_selectedColor.B > 128 * 3 ? true : false);
				graphics.DrawLine(useBlack ? Pens.Black : Pens.White, screenPoint.X - 4, screenPoint.Y, screenPoint.X + 4, screenPoint.Y);
				graphics.DrawLine(useBlack ? Pens.Black : Pens.White, screenPoint.X, screenPoint.Y - 4, screenPoint.X, screenPoint.Y + 4);
			}
			else
			{
				//using (SolidBrush solidBrush = new SolidBrush(m_selectedColor))
				//	graphics.FillRectangle(solidBrush, this.ClientRectangle);

				graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);
				graphics.DrawImage(m_eyeDropperBitmap, (this.Width - m_eyeDropperBitmap.Width) / 2, (this.Height - m_eyeDropperBitmap.Height) / 2);
			}
		}

		public bool IsLocked
		{
			get { return m_isLocked; }
		}

		public Color SelectedColor
		{
			get { return m_selectedColor; }
			set { m_selectedColor = value; }
		}
	}

	public class ColorEventArgs : EventArgs
	{
		public ColorEventArgs(Color color, int index)
		{
			Color = color;
			Index = index;
		}

		public bool IsLocked { get; set; }

		public int Index { get; set; }

		public Color Color { get; set; }
	}
}

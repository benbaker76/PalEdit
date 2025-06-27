using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PalEdit
{
    public partial class MyPictureBox : PictureBox
    {
        protected Rectangle m_selectRectangle = Rectangle.Empty;
        protected int m_zoomLevel = 1;
        protected bool m_showGrid = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            base.OnPaint(e);

            if (!m_selectRectangle.IsEmpty)
            {
                using (Pen pen = new Pen(Color.Red, 1))
                {
                    pen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, m_selectRectangle);
                }
            }

            if (m_showGrid)
            {
                if (m_zoomLevel >= 20)
                {
                    for (int y = 0; y < this.Height; y += m_zoomLevel)
                    {
                        for (int x = 0; x < this.Width; x += m_zoomLevel)
                        {
                            e.Graphics.DrawLine(Pens.Gray, new Point(x, 0), new Point(x, this.Height));
                            e.Graphics.DrawLine(Pens.Gray, new Point(0, y), new Point(this.Width, y));
                        }
                    }
                }
            }
        }

		public Rectangle SelectRectangle
		{
			get { return m_selectRectangle; }
			set { m_selectRectangle = value; this.Invalidate(); }
		}

		public bool ShowGrid
		{
			get { return m_showGrid; }
			set { m_showGrid = value; this.Invalidate(); }
		}

		public int ZoomLevel
		{
			get { return m_zoomLevel; }
			set
			{
				m_zoomLevel = value;
				if (this.Image != null)
				{
					this.Size = new Size(this.Image.Width * m_zoomLevel, this.Image.Height * m_zoomLevel);
				}
			}
		}

		public new Image Image
		{
			get { return base.Image; }
			set
			{
				base.Image = value;

				ZoomLevel = m_zoomLevel;
			}
		}
	}
}

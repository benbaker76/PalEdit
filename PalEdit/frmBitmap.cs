using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace PalEdit
{
    public partial class frmBitmap : Form
    {
        private int m_zoom = 1;

        private Point m_mouseStart = Point.Empty;
        private Point m_mouseEnd = Point.Empty;

        private bool m_mouseDown = false;

        private PaletteControl m_palControl = null;
        private PictureBox m_picMagnify = null;

        public delegate void PixelSelectDelegate(int index);
        public delegate void RectangleSelectDelegate(Rectangle rectangle);

        public event PixelSelectDelegate OnPixelSelect = null;
        public event RectangleSelectDelegate OnRectangleSelect = null;

        public frmBitmap(PaletteControl palControl, PictureBox picMagnify, Color transparentColor, int zoom)
        {
            InitializeComponent();

            m_palControl = palControl;

            picBitmap.Image = m_palControl.Bitmap;
            picBitmap.BackColor = transparentColor;
            picBitmap.UpdateBitmap();

            m_picMagnify = picMagnify;

            m_zoom = zoom;
        }

        private void DrawMagnifier(Point location)
        {
            Point mouseLocation = GetScaledMouseLocation(location);
            Bitmap bmpMagnify = (Bitmap)m_picMagnify.Image;
            Bitmap bmp = (Bitmap)picBitmap.Image;

            using (Graphics g = Graphics.FromImage(bmpMagnify))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                g.Clear(Color.Black);

                int left = 0, right = 0;
                int top = 0, bottom = 0;
                float zoomHalf = m_zoom / 2.0f;
                PointF offset = new PointF(mouseLocation.X - (m_picMagnify.Width / m_zoom), mouseLocation.Y - (m_picMagnify.Height / m_zoom));

                if (mouseLocation.X < m_picMagnify.Width / zoomHalf)
                    left = (m_picMagnify.Width / m_zoom) - mouseLocation.X;
                if (mouseLocation.Y < m_picMagnify.Height / zoomHalf)
                    top = (m_picMagnify.Height / m_zoom) - mouseLocation.Y;
                if (mouseLocation.X > bmp.Width - m_picMagnify.Width / zoomHalf)
                    right = (m_picMagnify.Width / m_zoom) - (bmp.Width - mouseLocation.X);
                if (mouseLocation.Y > bmp.Height - m_picMagnify.Height / zoomHalf)
                    bottom = (m_picMagnify.Height / m_zoom) - (bmp.Height - mouseLocation.Y);

                for (int y = 0 + top; y < (m_picMagnify.Height / zoomHalf) - bottom; y++)
                {
                    for (int x = 0 + left; x < (m_picMagnify.Width / zoomHalf) - right; x++)
                    {
                        Point pixelPoint = new Point((int)Math.Round(offset.X + x), (int)Math.Round(offset.Y + y));

                        if (pixelPoint.X >= bmp.Width || pixelPoint.Y >= bmp.Height)
                            continue;

                        Color pixelColor = bmp.GetPixel(pixelPoint.X, pixelPoint.Y);

                        using (SolidBrush b = new SolidBrush(pixelColor))
                            g.FillRectangle(b, x * zoomHalf, y * zoomHalf, zoomHalf, zoomHalf);
                    }
                }

                if (!picBitmap.SelectRectangle.IsEmpty)
                {
                    using (Pen pen = new Pen(Color.Red, 1))
                    {
                        Rectangle scaledRectangle = GetScaledBitmapRectangle();
                        RectangleF rect = RectangleF.FromLTRB((scaledRectangle.X - offset.X) * zoomHalf, (scaledRectangle.Y - offset.Y) * zoomHalf, (scaledRectangle.Right - offset.X) * zoomHalf, (scaledRectangle.Bottom - offset.Y) * zoomHalf);
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    }
                }

                m_picMagnify.Invalidate();

                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawLine(pen, m_picMagnify.Width / 2 - 8, m_picMagnify.Height / 2, m_picMagnify.Width / 2 + 8, m_picMagnify.Height / 2);
                    g.DrawLine(pen, m_picMagnify.Width / 2, m_picMagnify.Height / 2 - 8, m_picMagnify.Width / 2, m_picMagnify.Height / 2 + 8);
                }
            }
        }

        public void UpdateBitmap()
        {
            picBitmap.Update();
        }

        public void SetBitmap(Bitmap bitmap)
        {
            picBitmap.Image = bitmap;
            picBitmap.UpdateBitmap();
        }

        public void SetBackColor(Color color)
        {
            picBitmap.BackColor = Color.FromArgb(255, color);
            picBitmap.UpdateBitmap();
        }

        public void CloseBitmap()
        {
            picBitmap.Image = null;
            picBitmap.UpdateBitmap();
        }

        private void frmBitmap_FormClosing(object sender, FormClosingEventArgs e)
        {
            picBitmap.Image = null;
        }

        private void picBitmap_MouseMove(object sender, MouseEventArgs e)
        {
            m_mouseEnd = picBitmap.SnapToGrid(e.Location);

            if (picBitmap.Image == null)
                return;

            if (tsbPaint.CheckState == CheckState.Checked)
            {
                if (m_mouseDown)
                {
                    if (m_palControl.SelectedIndex != -1)
                    {
                        Point mouseLocation = GetScaledMouseLocation(e.Location);
                        Colors.SetPixelIndex((Bitmap)picBitmap.Image, mouseLocation.X, mouseLocation.Y, m_palControl.SelectedIndex);

                        picBitmap.UpdateBitmap();
                    }
                }
            }
            else if (tsbColorPicker.CheckState == CheckState.Checked)
            {
                if (m_mouseDown)
                {
                    if (!m_mouseStart.Equals(m_mouseEnd))
                    {
                        picBitmap.SelectRectangle = new Rectangle(Math.Min(m_mouseStart.X, m_mouseEnd.X), Math.Min(m_mouseStart.Y, m_mouseEnd.Y), Math.Abs(m_mouseStart.X - m_mouseEnd.X), Math.Abs(m_mouseStart.Y - m_mouseEnd.Y));
                    }
                }
                else
                {
                    picBitmap.SelectRectangle = Rectangle.Empty;
                }
            }
            else if (tsbSwapColor.CheckState == CheckState.Checked)
            {
            }

            DrawMagnifier(e.Location);
        }

        private void picBitmap_MouseDown(object sender, MouseEventArgs e)
        {
            m_mouseDown = true;
            m_mouseStart = picBitmap.SnapToGrid(e.Location);
            m_mouseEnd = m_mouseStart;
        }

        private void picBitmap_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouseDown = false;

            if (picBitmap.Image == null)
                return;

            if (tsbPaint.CheckState == CheckState.Checked)
            {
                if (m_palControl.SelectedIndex != -1)
                {
                    Point mouseLocation = GetScaledMouseLocation(e.Location);
                    Colors.SetPixelIndex((Bitmap)picBitmap.Image, mouseLocation.X, mouseLocation.Y, m_palControl.SelectedIndex);

                    picBitmap.UpdateBitmap();
                }
            }
            else if (tsbColorPicker.CheckState == CheckState.Checked)
            {
                if (m_mouseStart.Equals(m_mouseEnd))
                {
                    Point mouseLocation = GetScaledMouseLocation(e.Location);
                    int index = Colors.GetPixelIndex((Bitmap)picBitmap.Image, mouseLocation.X, mouseLocation.Y);

                    if (OnPixelSelect != null)
                        OnPixelSelect(index);
                }
                else
                {
                    if (!picBitmap.SelectRectangle.IsEmpty)
                    {
                        if (OnRectangleSelect != null)
                        {
                            OnRectangleSelect(GetScaledBitmapRectangle());
                        }

                        picBitmap.SelectRectangle = Rectangle.Empty;
                    }
                }
            }
            else if (tsbSwapColor.CheckState == CheckState.Checked)
            {
                if (m_palControl.SelectedIndex != -1)
                {
                    Point mouseLocation = GetScaledMouseLocation(e.Location);
                    int index = Colors.SwapPixelIndex((Bitmap)picBitmap.Image, mouseLocation.X, mouseLocation.Y, m_palControl.SelectedIndex);

                    if (OnPixelSelect != null)
                        OnPixelSelect(index);
                }
            }
        }

        private void picBitmap_MouseLeave(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(m_picMagnify.Image))
                g.Clear(Color.Black);

            m_picMagnify.Invalidate();
        }

        private Rectangle GetScaledBitmapRectangle()
        {
            return Rectangle.FromLTRB((int)Math.Floor((double)picBitmap.SelectRectangle.X / picBitmap.ZoomLevel), (int)Math.Floor((double)picBitmap.SelectRectangle.Y / picBitmap.ZoomLevel), (int)Math.Floor((double)picBitmap.SelectRectangle.Right / picBitmap.ZoomLevel), (int)Math.Floor((double)picBitmap.SelectRectangle.Bottom / picBitmap.ZoomLevel));
        }

        private Point GetScaledMouseLocation(Point location)
        {
            return new Point((int)Math.Floor((double)location.X / picBitmap.ZoomLevel), (int)Math.Floor((double)location.Y / picBitmap.ZoomLevel));
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton tsbButton = (ToolStripButton)sender;

            switch(tsbButton.Name)
            {
                case "tsbPaint":
                    tsbPaint.CheckState = CheckState.Checked;
                    tsbColorPicker.CheckState = CheckState.Unchecked;
                    tsbSwapColor.CheckState = CheckState.Unchecked;
                    break;
                case "tsbColorPicker":
                    tsbPaint.CheckState = CheckState.Unchecked;
                    tsbColorPicker.CheckState = CheckState.Checked;
                    tsbSwapColor.CheckState = CheckState.Unchecked;
                    break;
                case "tsbSwapColor":
                    tsbPaint.CheckState = CheckState.Unchecked;
                    tsbColorPicker.CheckState = CheckState.Unchecked;
                    tsbSwapColor.CheckState = CheckState.Checked;
                    break;
                case "tsbZoomIn":
                    picBitmap.ZoomIn();
                    break;
                case "tsbZoomOut":
                    picBitmap.ZoomOut();
                    break;
                case "tsbGrid":
                    picBitmap.ShowGrid = tsbGrid.Checked;
                    break;
            }
        }

        public int Zoom
        {
            get { return m_zoom; }
            set { m_zoom = value; }
        }

        private void picBitmap_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
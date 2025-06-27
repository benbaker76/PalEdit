using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace PalEdit
{
    public partial class ZoomPanPictureBox : UserControl
    {
        private int m_zoomMin = 1;
        private int m_zoomMax = 40;
        private int m_zoomLevel = 1;

        public ZoomPanPictureBox()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            SetZoom();
            this.KeyDown += OnKeyDown;
            this.MouseWheel += OnMouseWheel;
            this.picBox.MouseMove += picBox_MouseMove;
            this.picBox.MouseDown += picBox_MouseDown;
            this.picBox.MouseUp += picBox_MouseUp;
            this.picBox.MouseLeave += picBox_MouseLeave;
            this.picBox.MouseWheel += OnMouseWheel;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Oemplus | Keys.Control))
            {
                ZoomIn();
            }
            else if (e.KeyData == (Keys.OemMinus | Keys.Control))
            {
                ZoomOut();
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn();
            }
            else if (e.Delta < 0)
            {
                ZoomOut();
            }
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!picBox.ClientRectangle.Contains(e.Location))
                return;

            base.OnMouseMove(e);
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!picBox.ClientRectangle.Contains(e.Location))
                return;

            base.OnMouseDown(e);
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (!picBox.ClientRectangle.Contains(e.Location))
                return;

            base.OnMouseUp(e);
        }

        private void picBox_MouseLeave(object sender, EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        public void UpdateBitmap()
        {
            picBox.Invalidate();
        }

        public void ZoomIn()
        {
            if (m_zoomLevel <= m_zoomMax)
            {
                m_zoomLevel++;
                SetZoom();
            }
        }

        public void ZoomOut()
        {
            if (m_zoomLevel > m_zoomMin)
            {
                m_zoomLevel--;
                SetZoom();
            }
        }

        private void SetZoom()
        {
            picBox.ZoomLevel = m_zoomLevel;
        }

        public Image Image
        {
            get
            {
                return picBox.Image;
            }
            set
            {
                picBox.Image = value;

                if (value == null)
                {
                    picBox.Size = panel1.Size;
                }
            }
        }
        public Point SnapToGrid(Point p)
        {
            double x = Math.Round((double)p.X / m_zoomLevel) * m_zoomLevel;
            double y = Math.Round((double)p.Y / m_zoomLevel) * m_zoomLevel;
            return new Point((int)x, (int)y);
        }

        public int ZoomLevel
        {
            get { return m_zoomLevel; }
            set { m_zoomLevel = value; SetZoom(); }
        }

        public Rectangle SelectRectangle
        {
            get { return picBox.SelectRectangle; }
            set { picBox.SelectRectangle = value; }
        }

        public bool ShowGrid
        {
            set { picBox.ShowGrid = value; }
            get { return picBox.ShowGrid; }
        }

        public BorderStyle Border
        {
            set { picBox.BorderStyle = value; }
        }

        public Color PanelColor
        {
            get { return panel1.BackColor; }
            set { panel1.BackColor = value; }
        }

        public override Color BackColor
        {
            get { return picBox.BackColor; }
            set { picBox.BackColor = value; }
        }
    }
}

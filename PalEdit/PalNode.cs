using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PalEdit
{
    public class PalNode
    {
        private Color m_color = Color.Empty;
        private bool m_selected = false;
        private Rectangle m_rectangle = Rectangle.Empty;
        private float m_hue = 1;
        private float m_saturation = 1;
        private float m_brightness = 1;
        private float m_tint = 0;

        public PalNode(Color color)
        {
            m_color = color;
        }

        public PalNode(Color color, Rectangle rect)
        {
            m_color = color;
            Rectangle = rect;
        }

        public PalNode(Color color, bool isSelected, Rectangle rect, float hue, float saturation, float brightness, float tint)
        {
            m_color = color;
            IsSelected = isSelected;
            Rectangle = rect;
            m_hue = hue;
            m_saturation = saturation;
            m_brightness = brightness;
            m_tint = tint;
        }

        private void ResetHSB()
        {
            m_hue = 1;
            m_saturation = 1;
            m_brightness = 1;
            m_tint = 0;
        }

        private HSB GetProcessedHSB()
        {
            Color color = Color.FromArgb(m_color.A, m_color.R, m_color.G, (int)Math.Min(m_color.B + Tint * 255.0, 255));

            return new HSB(color.GetHue() * Hue, color.GetSaturation() * Saturation, color.GetBrightness() * Brightness);
        }

        private Color GetProcessedColor()
        {
            HSB hsb = GetProcessedHSB();

            return Color.FromArgb(m_color.A, hsb.RGB);
        }

        public HSB HSB
        {
            get { return GetProcessedHSB(); }
            set { m_color = value.RGB; ResetHSB();  }
        }

        public Color Color
        {
            get { return GetProcessedColor(); }
            set { m_color = value; ResetHSB();  }
        }

        public int Alpha
        {
            set { m_color = Color.FromArgb(value, m_color.R, m_color.G, m_color.B); }
            get { return m_color.A; }
        }

        public Rectangle Rectangle
        {
            set { m_rectangle = value; }
            get { return m_rectangle; }
        }

        public bool IsSelected
        {
            set { m_selected = value; }
            get { return m_selected; }
        }

        public float Hue
        {
            set { m_hue = value; }
            get { return m_hue; }
        }

        public float Saturation
        {
            set { m_saturation = value; }
            get { return m_saturation; }
        }

        public float Brightness
        {
            set { m_brightness = value; }
            get { return m_brightness; }
        }

        public float Tint
        {
            set { m_tint = value; }
            get { return m_tint; }
        }

		public object Tag { get; set; }

        #region ICloneable Members

        public PalNode Clone()
        {
			PalNode palNode = new PalNode(m_color, m_selected, m_rectangle, m_hue, m_saturation, m_brightness, m_tint);

			palNode.Tag = Tag;

			return palNode;
        }

        #endregion
    }
}

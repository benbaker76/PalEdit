using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PalEdit
{
    public class HSB
    {
        private float h;
        private float s;
        private float b;

        public float Hue
        {
            get
            {
                return h;
            }
            set
            {
                h = (float)(Math.Abs(value) % 360);
            }
        }

        public float Saturation
        {
            get
            {
                return s;
            }
            set
            {
                s = (float)Math.Max(Math.Min(1.0, value), 0.0);
            }
        }

        public float Brightness
        {
            get
            {
                return b;
            }
            set
            {
                b = (float)Math.Max(Math.Min(1.0, value), 0.0);
            }
        }

        private HSB()
        {
        }
        public HSB(float hue, float saturation, float brightness)
        {
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
        }

		public Color RGB
		{
			get
			{
				float fMax, fMid, fMin;
				int iSextant, iMax, iMid, iMin;
				if (0.5 < b)
				{
					fMax = b - (b * s) + s;
					fMin = b + (b * s) - s;
				}
				else
				{
					fMax = b + (b * s);
					fMin = b - (b * s);
				}
				iSextant = (int)Math.Floor(h / 60.0f);
				if (300.0f <= h) {
					h -= 360.0f;
				}
				h /= 60.0f;
				h -= 2.0f * (float)Math.Floor(((iSextant + 1.0f) % 6.0f) / 2.0f);
				if (0 == iSextant % 2)
				{
					fMid = h * (fMax - fMin) + fMin;
				}
				else
				{
					fMid = fMin - h * (fMax - fMin);
				}
				iMax = System.Convert.ToInt32(fMax * 255);
				iMid = System.Convert.ToInt32(fMid * 255);
				iMin = System.Convert.ToInt32(fMin * 255);
				switch (iSextant)
				{
					case 1:
						return Color.FromArgb(iMid, iMax, iMin);
					case 2:
						return Color.FromArgb(iMin, iMax, iMid);
					case 3:
						return Color.FromArgb(iMin, iMid, iMax);
					case 4:
						return Color.FromArgb(iMid, iMin, iMax);
					case 5:
						return Color.FromArgb(iMax, iMin, iMid);
					default:
						return Color.FromArgb(iMax, iMid, iMin);
				}
			}
		}

        public static HSB FromRGB(byte red, byte green, byte blue)
        {
            return FromRGB(Color.FromArgb(red, green, blue));
        }

        public static HSB FromRGB(Color c)
        {
            return new HSB(c.GetHue(), c.GetSaturation(), c.GetBrightness());
        }
    }
}

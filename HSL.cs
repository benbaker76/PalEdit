using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PalEdit
{
    public class HSL
    {
        private float h;
        private float s;
        private float l;

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

        public float Luminance
        {
            get
            {
                return l;
            }
            set
            {
                l = (float)Math.Max(Math.Min(1.0, value), 0.0);
            }
        }

        private HSL()
        {
        }
        public HSL(float hue, float saturation, float luminance)
        {
            Hue = hue;
            Saturation = saturation;
            Luminance = luminance;
        }


        public Color RGB
        {
            get
            {
                double r = 0, g = 0, b = 0;

                double temp1, temp2;

                double normalisedH = h / 360.0;

                if (l == 0)
                {
                    r = g = b = 0;
                }
                else
                {
                    if (s == 0)
                    {
                        r = g = b = l;
                    }
                    else
                    {
                        temp2 = ((l <= 0.5) ? l * (1.0 + s) : l + s - (l * s));

                        temp1 = 2.0 * l - temp2;

                        double[] t3 = new double[] { normalisedH + 1.0 / 3.0, normalisedH, normalisedH - 1.0 / 3.0 };

                        double[] clr = new double[] { 0, 0, 0 };

                        for (int i = 0; i < 3; ++i)
                        {
                            if (t3[i] < 0)
                                t3[i] += 1.0;

                            if (t3[i] > 1)
                                t3[i] -= 1.0;

                            if (6.0 * t3[i] < 1.0)
                                clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                            else if (2.0 * t3[i] < 1.0)
                                clr[i] = temp2;
                            else if (3.0 * t3[i] < 2.0)
                                clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                            else
                                clr[i] = temp1;

                        }

                        r = clr[0];
                        g = clr[1];
                        b = clr[2];
                    }

                }
                return Color.FromArgb((int)Math.Round(255.0 * r), (int)Math.Round(255.0 * g), (int)Math.Round(255.0 * b));
            }
        }

        private static byte toRGB(float rm1, float rm2, float rh)
        {
            if (rh > 360) rh -= 360;
            else if (rh < 0) rh += 360;

            if (rh < 60) rm1 = rm1 + (rm2 - rm1) * rh / 60;
            else if (rh < 180) rm1 = rm2;
            else if (rh < 240) rm1 = rm1 + (rm2 - rm1) * (240 - rh) / 60;

            return (byte)(rm1 * 255);
        }


        public static HSL FromRGB(byte red, byte green, byte blue)
        {
            return FromRGB(Color.FromArgb(red, green, blue));
        }

        public static HSL FromRGB(Color c)
        {
            return new HSL(c.GetHue(), c.GetSaturation(), c.GetBrightness());
        }
    }
}

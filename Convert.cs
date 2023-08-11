using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PalEdit
{
    public class Convert
    {
        public static bool IsWindows()
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows);
        }

        public static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public static int ColorToX1B5G5R5(Color color)
        {
            return (int)((((int)Math.Round(((double)color.B / 255.0) * 0x1F) & 0x1F) << 10) | (((int)Math.Round(((double)color.G / 255.0) * 0x1F) & 0x1F) << 5) | ((int)Math.Round(((double)color.R / 255.0) * 0x1F) & 0x1F));
        }

        public static double RGBColorDistance(Color Color1, Color Color2)
        {
            return Math.Pow(Color1.R - Color2.R, 2) + Math.Pow(Color1.G - Color2.G, 2) + Math.Pow(Color1.B - Color2.B, 2);
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
        }
    }
}

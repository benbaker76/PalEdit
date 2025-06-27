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

		public static int ColorToRGB332(Color color)
		{
			int r = (color.R >> 5) & 0x7;
			int g = (color.G >> 5) & 0x7;
			int b = (color.B >> 6) & 0x3;
			return (r << 5) | (g << 2) | b;
		}

		public static int ColorToRGB333(Color color)
		{
			int r = (color.R >> 5) & 0x7;
			int g = (color.G >> 5) & 0x7;
			int b = (color.B >> 5) & 0x7;
			return (r << 6) | (g << 3) | b;
		}

		public static int ColorToRGB444(Color color)
		{
			int r = (color.R >> 4) & 0xF;
			int g = (color.G >> 4) & 0xF;
			int b = (color.B >> 4) & 0xF;
			return (r << 8) | (g << 4) | b;
		}

		public static int ColorToRGB555(Color color)
		{
			int r = (color.R >> 3) & 0x1F;
			int g = (color.G >> 3) & 0x1F;
			int b = (color.B >> 3) & 0x1F;
			return (r << 10) | (g << 5) | b;
		}

		public static int ColorToRGB565(Color color)
		{
			int r = (color.R >> 3) & 0x1F;
			int g = (color.G >> 2) & 0x3F;
			int b = (color.B >> 3) & 0x1F;
			return (r << 11) | (g << 5) | b;
		}

		public static Color ColorFromRGB332(int color)
		{
			int r = ((color >> 5) & 0x7) << 5;
			int g = ((color >> 2) & 0x7) << 5;
			int b = (color & 0x3) << 6;
			return Color.FromArgb(r, g, b);
		}

		public static Color ColorFromRGB333(int color)
		{
			int r = ((color >> 6) & 0x7) << 5;
			int g = ((color >> 3) & 0x7) << 5;
			int b = (color & 0x7) << 5;
			return Color.FromArgb(r, g, b);
		}

		public static Color ColorFromRGB444(int color)
		{
			int r = ((color >> 8) & 0xF) << 4;
			int g = ((color >> 4) & 0xF) << 4;
			int b = (color & 0xF) << 4;
			return Color.FromArgb(r, g, b);
		}

		public static Color ColorFromRGB555(int color)
		{
			int r = ((color >> 10) & 0x1F) << 3;
			int g = ((color >> 5) & 0x1F) << 3;
			int b = (color & 0x1F) << 3;
			return Color.FromArgb(r, g, b);
		}

		public static Color ColorFromRGB565(int color)
		{
			int r = ((color >> 11) & 0x1F) << 3;
			int g = ((color >> 5) & 0x3F) << 2;
			int b = (color & 0x1F) << 3;
			return Color.FromArgb(r, g, b);
		}

		public static double RGBColorDistance(Color color1, Color color2)
        {
            return Math.Pow(color1.R - color2.R, 2) + Math.Pow(color1.G - color2.G, 2) + Math.Pow(color1.B - color2.B, 2);
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

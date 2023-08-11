using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

namespace PalEdit
{
    class Palette
    {
        public const uint BI_RGB = 0;
        public const uint DIB_RGB_COLORS = 0;
        public const int SRCCOPY = 0x00CC0020;

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public uint[] cols;
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);

        static uint MAKERGB(int r, int g, int b)
        {
            return ((uint)(b & 255)) | ((uint)((r & 255) << 8)) | ((uint)((g & 255) << 16));
        }

        public static Bitmap SetBitmapPalette(Bitmap bitmap, int paletteCount)
        {
            if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                return bitmap;

            if (bitmap.Palette == null)
                return bitmap;

            int width = bitmap.Width, height = bitmap.Height;
            IntPtr hBitmap = bitmap.GetHbitmap();

            BITMAPINFO bmi = new BITMAPINFO();
            bmi.biSize = 40;
            bmi.biWidth = width;
            bmi.biHeight = height;
            bmi.biPlanes = 1;
            bmi.biBitCount = (short)8;
            bmi.biCompression = BI_RGB;
            bmi.biSizeImage = (uint)(((width + 7) & 0xFFFFFFF8) * height / 8);
            bmi.biXPelsPerMeter = 1000000;
            bmi.biYPelsPerMeter = 1000000;

            uint ncols = (uint)paletteCount; 
            bmi.biClrUsed = ncols;
            bmi.biClrImportant = ncols;
            bmi.cols = new uint[128];

            for (int i=0; i<Math.Min(ncols, bitmap.Palette.Entries.Length); i++)
                bmi.cols[i] = (uint) bitmap.Palette.Entries[i].ToArgb();
            
            IntPtr bits0;
            IntPtr hBitmap0 = CreateDIBSection(IntPtr.Zero, ref bmi, DIB_RGB_COLORS, out bits0, IntPtr.Zero, 0);

            IntPtr sdc = GetDC(IntPtr.Zero);
            IntPtr hdc = CreateCompatibleDC(sdc); SelectObject(hdc, hBitmap);
            IntPtr hdc0 = CreateCompatibleDC(sdc); SelectObject(hdc0, hBitmap0);

            BitBlt(hdc0, 0, 0, width, height, hdc, 0, 0, SRCCOPY);

            Bitmap bmp = Bitmap.FromHbitmap(hBitmap0);

            bmp.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            //FIBITMAP dib = FreeImage.CreateFromHbitmap(hBitmap0, hdc0);

            DeleteDC(hdc);
            DeleteDC(hdc0);
            ReleaseDC(IntPtr.Zero, sdc);
            DeleteObject(hBitmap);
            DeleteObject(hBitmap0);

            return bmp;
        }
    }
}

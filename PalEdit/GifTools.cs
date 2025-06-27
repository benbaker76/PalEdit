using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace PalEdit
{
    class GifTools
    {
        public static ColorPalette GetColorPalette(int nColors)
        {
            PixelFormat bitscolordepth = PixelFormat.Format1bppIndexed;
            ColorPalette palette;
            Bitmap bitmap;

            if (nColors > 2)
                bitscolordepth = PixelFormat.Format4bppIndexed;
            if (nColors > 16)
                bitscolordepth = PixelFormat.Format8bppIndexed;

            bitmap = new Bitmap(1, 1, bitscolordepth);

            palette = bitmap.Palette;

            bitmap.Dispose();

            return palette;
        }

        public static void SaveGIFWithNewColorTable(Image image, string filename)
        {
            if (image.Palette == null)
                return;

            int nColors = image.Palette.Entries.Length;

            if (nColors > 256)
                nColors = 256;
            if (nColors < 2)
                nColors = 2;

            int Width = image.Width;
            int Height = image.Height;

            using (Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed))
            {
                ColorPalette pal = GetColorPalette(nColors);

                for (uint i = 0; i < nColors; i++)
                    pal.Entries[i] = image.Palette.Entries[i];

                bitmap.Palette = pal;

                using (Bitmap BmpCopy = new Bitmap(Width, Height, PixelFormat.Format32bppArgb))
                {
                    using (Graphics g = Graphics.FromImage(BmpCopy))
                    {
                        g.PageUnit = GraphicsUnit.Pixel;

                        g.DrawImage(image, 0, 0, Width, Height);
                    }

                    BitmapData bitmapData;
                    Rectangle rect = new Rectangle(0, 0, Width, Height);

                    bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                    IntPtr pixels = bitmapData.Scan0;

                    unsafe
                    {
                        byte* pBits;

                        if (bitmapData.Stride > 0)
                            pBits = (byte*)pixels.ToPointer();
                        else
                            pBits = (byte*)pixels.ToPointer() + bitmapData.Stride * (Height - 1);

                        uint stride = (uint)Math.Abs(bitmapData.Stride);

                        for (uint row = 0; row < Height; ++row)
                        {
                            for (uint col = 0; col < Width; ++col)
                            {
                                Color pixel;

                                byte* p8bppPixel = pBits + row * stride + col;

                                pixel = BmpCopy.GetPixel((int)col, (int)row);

                                double luminance = (pixel.R * 0.299) +
                                    (pixel.G * 0.587) +
                                    (pixel.B * 0.114);

                                *p8bppPixel = (byte)(luminance * (nColors - 1) / 255 + 0.5);
                            }
                        }
                    }

                    bitmap.UnlockBits(bitmapData);

                    bitmap.Save(filename, ImageFormat.Gif);
                }
            }
        }

        public static void SaveGIFWithNewColorTable(Bitmap bitmap, string filename)
        {
            if (bitmap.Palette == null)
                return;

            using (Bitmap clone = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat))
            {
                ColorPalette palette = clone.Palette;

                for (int i = 0; (i < palette.Entries.Length) && (i < bitmap.Palette.Entries.Length); i++)
                    palette.Entries[i] = bitmap.Palette.Entries[i];

                clone.Palette = palette;

                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                BitmapData bitmapDataClone = clone.LockBits(new Rectangle(0, 0, clone.Width, clone.Height), ImageLockMode.WriteOnly | ImageLockMode.UserInputBuffer, clone.PixelFormat, bitmapData);

                clone.UnlockBits(bitmapDataClone);
                bitmap.UnlockBits(bitmapData);

                clone.Save(filename, ImageFormat.Gif);
            }
        }
    }
}

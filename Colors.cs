using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers.XiaolinWu;

namespace PalEdit
{
    public class Colors
    {
        public enum PaletteType
        {
            Invalid,
            BitmapFile,
            PaletteFile
        };

        public enum ColorMode
        {
            Distance,
            Round,
            Floor,
            Ceil
        };

        public enum NearestColorMode
        {
            Sqrt,
            HSB,
            Lab
        };

		public enum SortColorMode
		{
			Sqrt,
			HSB,
			Lab
		}

		public static Color[] NextPalette332
        {
            get { return GetNextPalette(false); }
        }

        public static Color[] NextPalette333
        {
            get { return GetNextPalette(true); }
        }

        public static Color[] SwatchArray = new Color[]
        {
            Color.Black,
            Color.White,
            Color.Red,
            Color.Yellow,
            Color.Orange,
            Color.Lime,
            Color.Green,
            Color.Cyan,
            Color.Blue,
            Color.Purple,
            Color.Violet,
            Color.Magenta,
            Color.Brown
        };

        public static Color[] DescriptionColorArray = new Color[] { };
        public static string[] DescriptionTextArray = new string[] { };

        public static string[] PaletteNames;
        public static Color[][] PaletteArray;

        public static UInt32 RGB888(Byte r8, Byte g8, Byte b8) { return (UInt32)((r8 << 16) | (g8 << 8) | b8); }
        public static UInt16 RGB332(Byte r3, Byte g3, Byte b2) { return (UInt16)((r3 << 5) | (g3 << 2) | b2); }
        public static UInt16 RGB333(Byte r3, Byte g3, Byte b3) { return (UInt16)((r3 << 6) | (g3 << 3) | b3); }

		public static void LoadPalettes()
		{
			LoadPalettes(null, null, null, null);
		}

		public static void LoadPalettes(ToolStripMenuItem paletteToolStripMenuItem, ToolStripDropDownButton paletteToolStripDropDownButton, EventHandler paletteToolStripMenuEvent, EventHandler paletteToolStripDropDownButtonEvent)
        {
            List<FileSystemInfo> fileSystemInfoList = new List<FileSystemInfo>();
            List<string> fileNameList = new List<string>();

            fileSystemInfoList = FileIO.GetFileList(Globals.Folders.Palettes, "*.*", true);

            fileSystemInfoList.Sort(new CompareFileInfo());

            List<string> paletteNameList = new List<string>();
            List<Color[]> paletteList = new List<Color[]>();
            Dictionary<string, ToolStripMenuItem> dirToolStripMenuItemDictionary1 = new Dictionary<string, ToolStripMenuItem>();
			Dictionary<string, ToolStripMenuItem> dirToolStripMenuItemDictionary2 = new Dictionary<string, ToolStripMenuItem>();

			if (paletteToolStripDropDownButton != null)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Custom");
				toolStripMenuItem.Click += paletteToolStripDropDownButtonEvent;
				toolStripMenuItem.Tag = paletteList.Count.ToString();

				paletteToolStripDropDownButton.DropDownItems.Add(toolStripMenuItem);
			}

			paletteNameList.Add("Custom");
            paletteList.Add(new Color[] { });

			for (int i = 0; i < fileSystemInfoList.Count; i++)
            {
                FileSystemInfo fileSystemInfo = fileSystemInfoList[i];

                if ((fileSystemInfo.Attributes & FileAttributes.Directory) != 0)
                {
					string dirFileName = fileSystemInfo.FullName;
					string dirParent = Path.GetDirectoryName(fileSystemInfo.FullName);

                    if (paletteToolStripMenuItem != null)
                    {
						ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(fileSystemInfo.Name);
						ToolStripMenuItem dirToolStripMenuItem = null;

						if (dirToolStripMenuItemDictionary1.TryGetValue(dirParent, out dirToolStripMenuItem))
							dirToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
						else
							paletteToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);

						dirToolStripMenuItemDictionary1.Add(dirFileName, toolStripMenuItem);
					}

					if (paletteToolStripDropDownButton != null)
					{
						ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(fileSystemInfo.Name);
						ToolStripMenuItem dirToolStripMenuItem = null;

						if (dirToolStripMenuItemDictionary2.TryGetValue(dirParent, out dirToolStripMenuItem))
							dirToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
						else
							paletteToolStripDropDownButton.DropDownItems.Add(toolStripMenuItem);

						dirToolStripMenuItemDictionary2.Add(dirFileName, toolStripMenuItem);
					}
				}
                else
                {
                    string fileFileName = fileSystemInfo.FullName;
                    string fileParent = Path.GetDirectoryName(fileSystemInfo.FullName);
                    string fileName = Path.GetFileNameWithoutExtension(fileSystemInfo.Name);
                    Color[] colorPalette = null;

					if (PalFile.TryReadPalette(fileFileName, out colorPalette))
					{
						//PalFile.WritePalette(Path.ChangeExtension(fileFileName, ".txt"), colorPalette, -1, PaletteFormat.PaintNET);

						if (paletteToolStripMenuItem != null)
						{
							ToolStripMenuItem fileToolStripMenuItem = new ToolStripMenuItem(fileName);
							fileToolStripMenuItem.Click += paletteToolStripMenuEvent;
							fileToolStripMenuItem.Tag = paletteList.Count.ToString();

							ToolStripMenuItem dirToolStripMenuItem = null;

							if (dirToolStripMenuItemDictionary1.TryGetValue(fileParent, out dirToolStripMenuItem))
								dirToolStripMenuItem.DropDownItems.Add(fileToolStripMenuItem);
							else
								paletteToolStripMenuItem.DropDownItems.Add(fileToolStripMenuItem);
						}

						if (paletteToolStripDropDownButton != null)
						{
							ToolStripMenuItem fileToolStripMenuItem = new ToolStripMenuItem(fileName);
							fileToolStripMenuItem.Click += paletteToolStripDropDownButtonEvent;
							fileToolStripMenuItem.Tag = paletteList.Count.ToString();

							ToolStripMenuItem dirToolStripMenuItem = null;

							if (dirToolStripMenuItemDictionary2.TryGetValue(fileParent, out dirToolStripMenuItem))
								dirToolStripMenuItem.DropDownItems.Add(fileToolStripMenuItem);
							else
								paletteToolStripDropDownButton.DropDownItems.Add(fileToolStripMenuItem);
						}

						paletteNameList.Add(fileName);
						paletteList.Add(colorPalette);
					}
                }
            }

            PaletteNames = paletteNameList.ToArray();
            PaletteArray = paletteList.ToArray();
        }

        public static Color[] GetPalette(string name)
        {
            for (int i = 0; i < PaletteNames.Length; i++)
            {
                if (PaletteNames[i].Equals(name))
                    return PaletteArray[i];
            }

            return null;
        }

        public static Byte C8ToC3(Byte c8, ColorMode colorMode)
        {
            double c3 = (c8 * 7.0) / 255.0;

            switch (colorMode)
            {
                case ColorMode.Floor:
                    return (Byte)Math.Floor(c3);
                case ColorMode.Ceil:
                    return (Byte)Math.Ceiling(c3);
                case ColorMode.Round:
                case ColorMode.Distance:
                // Fall through
                default:
                    return (Byte)Math.Round(c3);
            }
        }

        public static Byte C8ToC2(Byte c8, ColorMode colorMode)
        {
            double c2 = (c8 * 3.0) / 255.0;

            switch (colorMode)
            {
                case ColorMode.Floor:
                    return (Byte)Math.Floor(c2);
                case ColorMode.Ceil:
                    return (Byte)Math.Ceiling(c2);
                case ColorMode.Round:
                case ColorMode.Distance:
                // Fall through
                default:
                    return (Byte)Math.Round(c2);
            }
        }

        public static Byte C2ToC3(Byte c2)
        {
            return (Byte)((c2 << 1) | (((c2 >> 1) | c2) & 1));
        }

        public static Byte C3ToC8(Byte c3)
        {
            return (Byte)Math.Round((c3 * 255.0) / 7.0);
        }

        public static UInt32 RGB332ToRGB888(UInt16 rgb333)
        {
            Byte r3 = (Byte)((rgb333 >> 5) & 7);
            Byte g3 = (Byte)((rgb333 >> 2) & 7);
            Byte b2 = (Byte)(rgb333 & 3);
            Byte b3 = C2ToC3(b2);
            Byte r = C3ToC8(r3);
            Byte g = C3ToC8(g3);
            Byte b = C3ToC8(b3);
            return RGB888(r, g, b);
        }

        public static UInt32 RGB333ToRGB888(UInt16 rgb333)
        {
            Byte r3 = (Byte)((rgb333 >> 6) & 7);
            Byte g3 = (Byte)((rgb333 >> 3) & 7);
            Byte b3 = (Byte)(rgb333 & 7);
            Byte r = C3ToC8(r3);
            Byte g = C3ToC8(g3);
            Byte b = C3ToC8(b3);
            return RGB888(r, g, b);
        }

        public static UInt16 RGB888ToRGB332(UInt32 rgb888, ColorMode colorMode)
        {
            Byte r8 = (Byte)(rgb888 >> 16);
            Byte g8 = (Byte)(rgb888 >> 8);
            Byte b8 = (Byte)rgb888;
            Byte r3 = C8ToC3(r8, colorMode);
            Byte g3 = C8ToC3(g8, colorMode);
            Byte b2 = C8ToC2(b8, colorMode);
            return RGB332(r3, g3, b2);
        }

        public static UInt16 RGB888ToRGB333(UInt32 rgb888, ColorMode colorMode)
        {
            Byte r8 = (Byte)(rgb888 >> 16);
            Byte g8 = (Byte)(rgb888 >> 8);
            Byte b8 = (Byte)rgb888;
            Byte r3 = C8ToC3(r8, colorMode);
            Byte g3 = C8ToC3(g8, colorMode);
            Byte b3 = C8ToC3(b8, colorMode);
            return RGB333(r3, g3, b3);
        }

        public static double GetColorDistance(Color e1, Color e2)
        {
            long rmean = ((long)e1.R + (long)e2.R) / 2;
            long r = (long)e1.R - (long)e2.R;
            long g = (long)e1.G - (long)e2.G;
            long b = (long)e1.B - (long)e2.B;
            return Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
        }

        public static bool DoesPaletteContain(Color[] palette, Color color, out int colorIndex)
        {
            colorIndex = 0;

            for (int i = 0; i < palette.Length; i++)
            {
                if (palette[i].R == color.R &&
                     palette[i].G == color.G &&
                     palette[i].B == color.B)
                {
                    colorIndex = i;
                    return true;
                }
            }

            return false;
        }


        private static Color[] GetNextPalette(bool use333)
        {
            List<Color> colorList = new List<Color>();
            int colorCount = use333 ? 512 : 256;

            for (int i = 0; i < colorCount; i++)
                colorList.Add(Color.FromArgb(use333 ? (int)RGB333ToRGB888((UInt16)i) : (int)RGB332ToRGB888((UInt16)i)));

            return colorList.ToArray();
        }

        public static int GetPixelIndex(Bitmap bitmap, int x, int y)
        {
            int index = 0;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            index = (int)Marshal.ReadByte(bmpData.Scan0, x + y * bmpData.Stride);

            bitmap.UnlockBits(bmpData);

            return index;
        }

        public static void SetPixelIndex(Bitmap bitmap, int x, int y, int selectedIndex)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            indices[x + y * bmpData.Stride] = (byte)selectedIndex;

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            bitmap.UnlockBits(bmpData);
        }

        public static void ClearBitmap(Bitmap bitmap, int colorIndex)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = (byte)colorIndex;
            }

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            bitmap.UnlockBits(bmpData);
        }

        public static void ClearPalette(Bitmap bitmap)
        {
            ColorPalette colorPalette = bitmap.Palette;

            for (int i = 0; i < colorPalette.Entries.Length; i++)
                colorPalette.Entries[i] = Color.FromArgb(255, Color.Black);

            bitmap.Palette = colorPalette;
        }

        public static void FloodFill(Bitmap bitmap, Point pt, int replacementIndex)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            int targetIndex = indices[pt.X + pt.Y * bmpData.Stride];

            if (!targetIndex.Equals(replacementIndex))
            {
                Stack<Point> pixels = new Stack<Point>();

                pixels.Push(pt);

                while (pixels.Count != 0)
                {
                    Point temp = pixels.Pop();
                    int y1 = temp.Y;
                    while (y1 >= 0 && indices[temp.X + y1 * bmpData.Stride] == targetIndex)
                    {
                        y1--;
                    }
                    y1++;

                    bool spanLeft = false, spanRight = false;

                    while (y1 < bitmap.Height && indices[temp.X + y1 * bmpData.Stride] == targetIndex)
                    {
                        indices[temp.X + y1 * bmpData.Stride] = (byte)replacementIndex;

                        if (!spanLeft && temp.X > 0 && indices[(temp.X - 1) + y1 * bmpData.Stride] == targetIndex)
                        {
                            pixels.Push(new Point(temp.X - 1, y1));
                            spanLeft = true;
                        }
                        else if (spanLeft && temp.X - 1 >= 0 && indices[(temp.X - 1) + y1 * bmpData.Stride] != targetIndex)
                        {
                            spanLeft = false;
                        }

                        if (!spanRight && temp.X < bitmap.Width - 1 && indices[(temp.X + 1) + y1 * bmpData.Stride] == targetIndex)
                        {
                            pixels.Push(new Point(temp.X + 1, y1));
                            spanRight = true;
                        }
                        else if (spanRight && temp.X < bitmap.Width - 1 && indices[(temp.X + 1) + y1 * bmpData.Stride] != targetIndex)
                        {
                            spanRight = false;
                        }
                        y1++;
                    }
                }

                Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);
            }

            bitmap.UnlockBits(bmpData);
        }

        public static int SwapPixelIndex(Bitmap bitmap, int x, int y, int selectedIndex)
        {
            int index = 0;
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            index = (int)indices[x + y * bmpData.Stride];

            for (int i = 0; i < indices.Length; i++)
                if (indices[i] == index)
                    indices[i] = (byte)selectedIndex;

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            bitmap.UnlockBits(bmpData);

            return index;
        }

        public static void CopyBitmap(Bitmap srcBitmap, Bitmap dstBitmap, Rectangle srcRectangle, Rectangle dstRectangle)
        {
            BitmapData srcBitmapData = srcBitmap.LockBits(new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            BitmapData dstBitmapData = dstBitmap.LockBits(new Rectangle(0, 0, dstBitmap.Width, dstBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int srcSize = srcBitmapData.Height * srcBitmapData.Stride;
            int dstSize = dstBitmapData.Height * dstBitmapData.Stride;
            byte[] srcPixels = new byte[srcSize];
            byte[] dstPixels = new byte[dstSize];

            Marshal.Copy(srcBitmapData.Scan0, srcPixels, 0, srcSize);
            Marshal.Copy(dstBitmapData.Scan0, dstPixels, 0, dstSize);

            for (int y = 0; y < srcRectangle.Height; y++)
            {
                for (int x = 0; x < srcRectangle.Width; x++)
                {
                    int srcIndex = (srcRectangle.Y + y) * srcBitmapData.Stride + (srcRectangle.X + x);
                    int dstIndex = (dstRectangle.Y + y) * dstBitmapData.Stride + (dstRectangle.X + x);

                    //if (srcIndex >= srcPixels.Length || dstIndex >= dstPixels.Length)
                    //    continue;

                    if (srcRectangle.X + x >= srcBitmap.Width || srcRectangle.Y + y >= srcBitmap.Height)
                        continue;

                    if (dstRectangle.X + x >= dstBitmap.Width || dstRectangle.Y + y >= dstBitmap.Height)
                        continue;

                    dstPixels[dstIndex] = srcPixels[srcIndex];
                }
            }

            Marshal.Copy(srcPixels, 0, srcBitmapData.Scan0, srcSize);
            Marshal.Copy(dstPixels, 0, dstBitmapData.Scan0, dstSize);

            srcBitmap.UnlockBits(srcBitmapData);
            dstBitmap.UnlockBits(dstBitmapData);
        }


        public static void SetColorPalette32bpp(Bitmap bitmap, Color[] colorPalette, NearestColorMode nearestColorMode)
        {
            List<Color> colorList = new List<Color>();
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* pPixel = (int*)bitmapData.Scan0;
                var pLastPixel = pPixel + bitmap.Width * bitmap.Height;

                while (pPixel < pLastPixel)
                {
                    *pPixel = GetNearestColor(Color.FromArgb(*pPixel), colorPalette, nearestColorMode).ToArgb();

                    pPixel++;
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        public static void SetColorPalette(Bitmap bitmap, Color[] colorArray)
        {
            ColorPalette colorPalette = bitmap.Palette;

            for (int i = 0; i < colorPalette.Entries.Length && i < colorArray.Length; i++)
                colorPalette.Entries[i] = Color.FromArgb(255, colorArray[i]);

            bitmap.Palette = colorPalette;
        }

        public static void SetColorPaletteAlpha(Color[] colorArray, int alpha)
        {
            for (int i = 0; i < colorArray.Length; i++)
                colorArray[i] = Color.FromArgb(alpha, colorArray[i]);
        }

        public static void RemoveUnusedColors(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];
            List<int> usedIndices = new List<int>();

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                if (!usedIndices.Contains(indices[i]))
                    usedIndices.Add(indices[i]);
            }

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            ColorPalette colorPalette = bitmap.Palette;

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                if (!usedIndices.Contains(i))
                    colorPalette.Entries[i] = Color.Empty;
            }

            bitmap.Palette = colorPalette;

            bitmap.UnlockBits(bmpData);
        }

        public static void AddColorIndexOffset(Bitmap bitmap, int offset)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
                indices[i] += (byte)offset;

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            bitmap.UnlockBits(bmpData);
        }

		public static void SortColorList(List<ColorNode> colorList, SortColorMode sortColorMode, int transparentIndex)
		{
			switch (sortColorMode)
			{
				case SortColorMode.Sqrt:
					colorList.Sort(new SqrtSorter(transparentIndex));
					break;
				case SortColorMode.HSB:
					colorList.Sort(new HSLSorter(transparentIndex));
					break;
				case SortColorMode.Lab:
					colorList.Sort(new LabSorter(transparentIndex));
					break;
			}
		}

		public static void SortColorList(List<ColorNode> colorList, SortColorMode sortColorMode)
		{
			SortColorList(colorList, sortColorMode, -1);
		}

		public static void SortPalette(Bitmap bitmap, SortColorMode sortColorMode)
        {
            ColorPalette colorPalette = bitmap.Palette;
            List<ColorNode> colorList = new List<ColorNode>();

            for (int i = 0; i < colorPalette.Entries.Length; i++)
                colorList.Add(new ColorNode(i, colorPalette.Entries[i]));

			SortColorList(colorList, sortColorMode);

			int count = 0;
            int[] colorIndices = new int[colorPalette.Entries.Length];

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                colorPalette.Entries[i] = colorList[count].Color;
                colorIndices[colorList[count].Index] = i;
                count++;
            }

            Colors.SetColorIndices(bitmap, colorIndices);
        }

        public static void SetColorIndices(Bitmap bitmap, int[] colorIndices)
        {
			if (bitmap == null)
				return;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] >= colorIndices.Length)
                    continue;

                indices[i] = (byte)colorIndices[indices[i]];
            }

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            ColorPalette colorPalette = bitmap.Palette;
            Color[] tempEntries = new Color[colorPalette.Entries.Length];

            Array.Copy(colorPalette.Entries, tempEntries, colorPalette.Entries.Length);

            for (int i = 0; i < colorIndices.Length; i++)
            {
				int colorIndex = colorIndices[i];

				if (colorIndex >= colorPalette.Entries.Length)
					continue;

				colorPalette.Entries[colorIndices[i]] = tempEntries[i];
            }

            bitmap.Palette = colorPalette;

            bitmap.UnlockBits(bmpData);
        }

        public static void GetColorIndices(Bitmap bitmap, Rectangle rect, out int[] colorIndices)
        {
            ColorPalette colorPalette = bitmap.Palette;
            List<int> colorIndicesList = new List<int>();
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    if (!rect.IsEmpty && !rect.Contains(x, y))
                        continue;

                    int index = y * bmpData.Stride + x;

                    byte colorIndex = indices[index];

                    if (colorIndex > colorPalette.Entries.Length)
                        continue;

                    if (!colorIndicesList.Contains(colorIndex))
                        colorIndicesList.Add(colorIndex);
                }
            }

            bitmap.UnlockBits(bmpData);

            colorIndices = colorIndicesList.ToArray();
        }

        public static void GetColorIndices(Bitmap bitmap, out int[] colorIndices)
        {
            GetColorIndices(bitmap, new Rectangle(Point.Empty, bitmap.Size), out colorIndices);
        }

        public static float Sqrt(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            u.f = z;
            u.tmp -= 1 << 23; // Subtract 2^m.
            u.tmp >>= 1; // Divide by 2.
            u.tmp += 1 << 29; // Add ((b + 1) / 2) * 2^m.
            return u.f;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int tmp;
        }

        public static float Pow2(float v)
        {
            return v * v;
        }


        public static Color GetNearestColorSqrt(Color color, Color[] colorPalette, out int colorIndex, out float leastDistance)
        {
            colorIndex = 0;
            leastDistance = float.MaxValue;

            if (colorPalette == null)
                return color;

            Color matchColor = Color.Empty;

            for (int i = 0; i < colorPalette.Length; i++)
            {
                Color paletteColor = colorPalette[i];

                float distance = Sqrt(Pow2(paletteColor.R - color.R) + Pow2(paletteColor.G - color.G) + Pow2(paletteColor.B - color.B));

                if (distance < leastDistance)
                {
                    matchColor = paletteColor;
                    colorIndex = i;
                    leastDistance = distance;

                    if (distance == 0)
                        return matchColor;
                }
            }

            return matchColor;
        }

        public static Color GetNearestColorHSB(Color color, Color[] colorPalette, out int colorIndex, out float leastDistance)
        {
            colorIndex = 0;
            leastDistance = float.MaxValue;

            if (colorPalette == null)
                return color;

            const float weightHue = 0.8f;
            const float weightSaturation = 0.1f;
            const float weightBrightness = 0.1f;

            Color matchColor = Color.Empty;

            for (int i = 0; i < colorPalette.Length; i++)
            {
                Color paletteColor = colorPalette[i];

                float distance = Sqrt(weightHue * Pow2(paletteColor.GetHue() - color.GetHue()) + weightSaturation * Pow2(paletteColor.GetSaturation() - color.GetSaturation()) + weightBrightness * Pow2(paletteColor.GetBrightness() - color.GetBrightness()));

                if (distance < leastDistance)
                {
                    matchColor = paletteColor;
                    colorIndex = i;
                    leastDistance = distance;

                    if (distance == 0)
                        return matchColor;
                }
            }

            return matchColor;
        }

        public static Color GetNearestColorLab(Color color, Color[] colorPalette, out int colorIndex, out float leastDistance)
        {
            colorIndex = 0;
            leastDistance = float.MaxValue;

            if (colorPalette == null)
                return color;

            Color matchColor = Color.Empty;
            CIELab labColor = Lab.RGBtoLab(color);

            for (int i = 0; i < colorPalette.Length; i++)
            {
                Color paletteColor = colorPalette[i];
                CIELab paletteLabColor = Lab.RGBtoLab(paletteColor);

                float distance1 = (float)Lab.GetDeltaE_CIEDE2000(labColor, paletteLabColor);
                float distance2 = (float)Lab.GetDeltaE_CMC(labColor, paletteLabColor);
                float distance = (distance1 + distance2) / 2.0f;

                if (distance < leastDistance)
                {
                    matchColor = paletteColor;
                    colorIndex = i;
                    leastDistance = distance;

                    if (distance == 0)
                        return matchColor;
                }
            }

            return matchColor;
        }

        public static int GetPaletteMatchIndex(Color[] colorPalette1, Color[] colorPalette2, NearestColorMode nearestColorMode)
        {
            int palettedIndex = 0;
            float leastDistanceTotal = float.MaxValue;

            for (int i = 0; i < colorPalette2.Length / 16; i++)
            {
                int paletteOffset = i * 16;
                Color[] colorPalette = new Color[16];
                Array.Copy(colorPalette2, paletteOffset, colorPalette, 0, 16);
                float distanceTotal = GetPaletteDistanceTotal(colorPalette1, colorPalette, nearestColorMode);

                if (distanceTotal < leastDistanceTotal)
                {
                    palettedIndex = i;
                    leastDistanceTotal = distanceTotal;
                }
            }

            return palettedIndex;
        }

        public static float GetPaletteDistanceTotal(Color[] colorPalette1, Color[] colorPalette2, NearestColorMode nearestColorMode)
        {
            int colorIndex = 0;
            float distanceTotal = 0.0f;

            foreach (Color color in colorPalette1)
            {
                float leastDistance = 0.0f;

                switch (nearestColorMode)
                {
                    default:
                    case NearestColorMode.Sqrt:
                        GetNearestColorSqrt(color, colorPalette2, out colorIndex, out leastDistance);
                        break;
                    case NearestColorMode.HSB:
                        GetNearestColorHSB(color, colorPalette2, out colorIndex, out leastDistance);
                        break;
                    case NearestColorMode.Lab:
                        GetNearestColorLab(color, colorPalette2, out colorIndex, out leastDistance);
                        break;
                }

                distanceTotal += leastDistance;
            }

            return distanceTotal;
        }

        public static int GetNearestColorIndex(Color color, Color[] colorPalette, NearestColorMode nearestColorMode)
        {
            int colorIndex = 0;
            float leastDistance = 0.0f;

            switch (nearestColorMode)
            {
                default:
                case NearestColorMode.Sqrt:
                    GetNearestColorSqrt(color, colorPalette, out colorIndex, out leastDistance);
                    break;
                case NearestColorMode.HSB:
                    GetNearestColorHSB(color, colorPalette, out colorIndex, out leastDistance);
                    break;
                case NearestColorMode.Lab:
                    GetNearestColorLab(color, colorPalette, out colorIndex, out leastDistance);
                    break;
            }

            return colorIndex;
        }

        public static Color GetNearestColor(Color color, Color[] colorPalette, NearestColorMode nearestColorMode)
        {
            int colorIndex = 0;
            float leastDistance = 0.0f;

            switch (nearestColorMode)
            {
                default:
                case NearestColorMode.Sqrt:
                    return Color.FromArgb(color.A, GetNearestColorSqrt(color, colorPalette, out colorIndex, out leastDistance));
                case NearestColorMode.HSB:
                    return Color.FromArgb(color.A, GetNearestColorHSB(color, colorPalette, out colorIndex, out leastDistance));
                case NearestColorMode.Lab:
                    return Color.FromArgb(color.A, GetNearestColorLab(color, colorPalette, out colorIndex, out leastDistance));
            }
        }

        public static Color[] CreateColorPalette(Bitmap bitmap)
        {
            List<Color> colorList = new List<Color>();
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* pPixel = (int*)bitmapData.Scan0;
                var pLastPixel = pPixel + bitmap.Width * bitmap.Height;

                while (pPixel < pLastPixel)
                {
                    int pixelValue = *pPixel;

                    Color color = Color.FromArgb(pixelValue);

                    if (!colorList.Contains(color))
                        colorList.Add(color);

                    pPixel++;
                }
            }

            bitmap.UnlockBits(bitmapData);

            return colorList.ToArray();
        }

        public static void SetNearestColorPalette(Color[] colorPalette, Color[] nearestColorPalette, NearestColorMode nearestColorMode)
        {
            for (int i = 0; i < colorPalette.Length; i++)
            {
                colorPalette[i] = GetNearestColor(colorPalette[i], nearestColorPalette, nearestColorMode);
            }
        }

        public static void GetColorPalette(Color[] colorPalette, Color[] nearestColorPalette, NearestColorMode nearestColorMode)
        {
            for (int i = 0; i < colorPalette.Length; i++)
            {
                colorPalette[i] = GetNearestColor(colorPalette[i], nearestColorPalette, nearestColorMode);
            }
        }

        public static Color[] GetColorPalette(Bitmap bitmap)
        {
            ColorPalette colorPalette = bitmap.Palette;
            Color[] colorArray = new Color[colorPalette.Entries.Length];

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                colorArray[i] = colorPalette.Entries[i];
            }

            return colorArray;
        }

        public static void SetBitmapNearestColorPalette(Bitmap bitmap, int[] selectedIndices, Color[] nearestColorPalette, NearestColorMode nearestColorMode)
        {
            if (nearestColorPalette == null)
                return;

            ColorPalette colorPalette = bitmap.Palette;

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                bool found = false;

                if (selectedIndices == null || Array.IndexOf(selectedIndices, i) >= 0)
                {
                    found = true;
                    break;
                }

                if (!found)
                    continue;

                colorPalette.Entries[i] = GetNearestColor(colorPalette.Entries[i], nearestColorPalette, nearestColorMode);
            }

            bitmap.Palette = colorPalette;
        }

		public static void SetBitmapNearestColorPaletteIndices(Bitmap bitmap, Color[] nearestColorPalette, NearestColorMode nearestColorMode)
        {
            if (nearestColorPalette == null)
                return;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            ColorPalette colorPalette = bitmap.Palette;
            int[] newIndices = new int[colorPalette.Entries.Length];

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
				newIndices[i] = GetNearestColorIndex(colorPalette.Entries[i], nearestColorPalette, nearestColorMode);
				colorPalette.Entries[i] = nearestColorPalette[newIndices[i]];
			}

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] > colorPalette.Entries.Length)
                    continue;

                indices[i] = (byte)newIndices[indices[i]];
            }

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            bitmap.Palette = colorPalette;

            bitmap.UnlockBits(bmpData);
        }

		public static Color[] GetBitmapUsedColorPalette(Bitmap bitmap)
        {
            ColorPalette colorPalette = bitmap.Palette;
            List<Color> colorList = new List<Color>();

            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] > colorPalette.Entries.Length)
                    continue;

                Color color = colorPalette.Entries[indices[i]];

                if (!colorList.Contains(color))
                    colorList.Add(color);
            }

            bitmap.UnlockBits(bmpData);

            return colorList.ToArray();
        }

        public static void SortBitmapPalette(Bitmap bitmap, SortColorMode sortColorMode, int transparentIndex)
        {
            ColorPalette colorPalette = bitmap.Palette;
            int[] indices = new int[colorPalette.Entries.Length];
            List<ColorNode> colorList = new List<ColorNode>();

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                colorList.Add(new ColorNode(i, colorPalette.Entries[i]));
            }

			SortColorList(colorList, sortColorMode, transparentIndex);

            for (int i = 0; i < colorList.Count; i++)
            {
                indices[colorList[i].Index] = i;
            }

            SetColorIndices(bitmap, indices);
        }

        /* public static void SetBitmapNearestColorPalette(Bitmap bitmap, int[] selectedIndices, Color[] nearestColorPalette, NearestColorMode nearestColorMode, bool use333)
        {
            ColorPalette colorPalette = bitmap.Palette;
            int[] colorIndices = new int[colorPalette.Entries.Length];

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                bool found = false;

                if (selectedIndices == null)
                {
                    found = true;
                }
                else
                {
                    for (int j = 0; j < selectedIndices.Length; j++)
                    {
                        if (selectedIndices[j] == i)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    colorIndices[i] = i;
                    continue;
                }

                Color nearestColor = GetNearestColor(colorPalette.Entries[i], nearestColorPalette, nearestColorMode, use333);
                int colorIndex = 0;

                if (DoesPaletteContain(colorPalette.Entries, nearestColor, out colorIndex))
                {
                    colorIndices[i] = colorIndex;
                }
                else
                {
                    colorIndices[i] = i;
                    colorPalette.Entries[i] = nearestColor;
                }
            }

            bitmap.Palette = colorPalette;

            SetColorIndices(bitmap, colorIndices);
        } */

        public static void CreateNextPalette(Color[] colorPalette, ColorMode colorMode, out Byte[] nextPalette)
        {
            nextPalette = new Byte[colorPalette.Length * 2];

            for (int i = 0; i < colorPalette.Length * 2; i++)
            {
                UInt32 rgb888 = (UInt32)(colorPalette[i >> 1].ToArgb() & 0xFFFFFF);
                UInt16 rgb333 = RGB888ToRGB333(rgb888, ColorMode.Distance);

                nextPalette[i] = (i % 2 == 0 ? (Byte)(rgb333 >> 1) : (Byte)(rgb333 & 1));
            }
        }

        public static void SwapColors(Bitmap bitmap, int indexA, int indexB)
        {
            if (indexA == indexB)
                return;

            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int indicesCount = bmpData.Height * bmpData.Stride;
            byte[] indices = new byte[indicesCount];

            Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] == indexA)
                {
                    indices[i] = (byte)indexB;
                }
                else if (indices[i] == indexB)
                {
                    indices[i] = (byte)indexA;
                }
            }

            Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

            ColorPalette colorPalette = bitmap.Palette;
            Color tempColor = colorPalette.Entries[indexA];

            colorPalette.Entries[indexA] = colorPalette.Entries[indexB];
            colorPalette.Entries[indexB] = tempColor;

            bitmap.Palette = colorPalette;

            bitmap.UnlockBits(bmpData);
        }

        public static PaletteType GetPaletteType(string fileName)
        {
            if (fileName == null)
                return PaletteType.Invalid;

            string fileExtension = Path.GetExtension(fileName);

            switch (fileExtension)
            {
                case ".act":
                case ".pal":
                case ".gpl":
                case ".txt":
                    return PaletteType.PaletteFile;
                case ".bmp":
                case ".gif":
                case ".png":
                case ".jpg":
                case ".pcx":
                case ".tif":
                    return PaletteType.BitmapFile;
            }

            return PaletteType.Invalid;
        }

        public static PaletteType TryOpenPalette(Form owner, ref string fileName)
        {
            if (FileIO.TryOpenFile(owner, null, fileName, "Palette Files", new string[] { ".act", ".pal", ".gpl", ".txt", ".bmp", ".gif", ".png", ".jpg", ".pcx", ".tif" }, out fileName))
            {
                return GetPaletteType(fileName);
            }

            return PaletteType.Invalid;
        }

        public static void BatchProcessIniFile(string fileName)
        {
            string workingPath = Path.GetDirectoryName(fileName);
            List<ImageNode> imageList = new List<ImageNode>();
            List<SliceNode> sliceList = new List<SliceNode>();
            IniFile iniFile = new IniFile(fileName);
            BatchSettings settings = new BatchSettings();

            string sourceDirectory = iniFile.Read("General", "SourceDirectory");
            string destinationDirectory = iniFile.Read("General", "DestinationDirectory");
            string outputFileName = iniFile.Read("General", "OutputFileName");
            Size outputSize = iniFile.Read<Size>("General", "OutputSize");
            string colorPalette = iniFile.Read("General", "ColorPalette");
            int colorPaletteIndex = Array.IndexOf(PaletteNames, colorPalette);
            Color[] customPalette = (colorPaletteIndex == 0 ? iniFile.ReadArray<Color>("General", "CustomPalette") : PaletteArray[colorPaletteIndex]);
            settings.SwapMagentaWithTransparentIndex = iniFile.Read<bool>("General", "SwapMagentaWithTransparentIndex", true);
            settings.SortSizes = iniFile.Read<bool>("General", "SortSizes", true);
            settings.SortColors = iniFile.Read<bool>("General", "SortColors", true);
            settings.Quantize = iniFile.Read<bool>("General", "Quantize", true);
            settings.AddPaletteOffset = iniFile.Read<bool>("General", "AddPaletteOffset", false);
            settings.CreateCombinedImage = iniFile.Read<bool>("General", "CreateCombinedImage", true);
            settings.MaxPaletteCount = iniFile.Read<int>("General", "MaxPaletteCount", 16);
            settings.TransparentIndex = iniFile.Read<int>("General", "TransparentIndex", 0);
			settings.AutoPosition = iniFile.Read<bool>("General", "AutoPosition", true);

			if (!String.IsNullOrEmpty(sourceDirectory))
            {
                if (!Path.IsPathRooted(sourceDirectory))
                {
                    sourceDirectory = Path.GetFullPath(Path.Combine(workingPath, sourceDirectory));
                }
            }

            if (!String.IsNullOrEmpty(destinationDirectory))
            {
                if (!Path.IsPathRooted(destinationDirectory))
                {
                    destinationDirectory = Path.GetFullPath(Path.Combine(workingPath, destinationDirectory));
                }
            }

            if (!String.IsNullOrEmpty(outputFileName))
            {
                if (!Path.IsPathRooted(outputFileName))
                {
                    outputFileName = Path.GetFullPath(Path.Combine(workingPath, outputFileName));
                }
            }

            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine("Error: Source Directory '{0}' Doesn't Exist", sourceDirectory);

                return;
            }

            for (int i = 0; i < 256; i++)
            {
                string sectionName = String.Format("Image{0}", i);
                string name = iniFile.Read(sectionName, "Name");

                if (name == null)
                    break;

                Point position = iniFile.Read<Point>(sectionName, "Position", Point.Empty);
                int paletteIndex = iniFile.Read<int>(sectionName, "PaletteIndex", -1);
                string path = Path.Combine(sourceDirectory, name);

                imageList.Add(new ImageNode(i, path, position, paletteIndex));
            }

            for (int i = 0; i < 256; i++)
            {
                string sectionName = String.Format("Slice{0}", i);
                string name = iniFile.Read(sectionName, "Name");

                if (name == null)
                    break;

                Point position = iniFile.Read<Point>(sectionName, "Position", Point.Empty);
                Size size = iniFile.Read<Size>(sectionName, "Size", Size.Empty);
                string path = Path.Combine(destinationDirectory, name);

                sliceList.Add(new SliceNode(path, position, size));
            }

            settings.SourceDirectory = sourceDirectory;
            settings.DestinationDirectory = destinationDirectory;
            settings.OutputFileName = outputFileName;
            settings.OutputSize = outputSize;
            settings.ColorPalette = customPalette;

            BatchProcessFolder(imageList, sliceList, settings);
        }

        public static void BatchProcessFolder(Form parent, BatchSettings settings)
        {
            if (!Directory.Exists(settings.SourceDirectory))
            {
                Console.WriteLine("Error: Source Directory '{0}' Doesn't Exist", settings.SourceDirectory);

                return;
            }

            string[] fileArray = Directory.GetFiles(settings.SourceDirectory, "*.*");
            List<ImageNode> imageList = new List<ImageNode>();
            RectanglePacker rectanglePacker = new RectanglePacker(settings.OutputSize.Width, settings.OutputSize.Height);

            if (fileArray.Length == 0)
            {
                Console.WriteLine("Error: No Files Found in '{0}'", settings.SourceDirectory);

                return;
            }

            for (int i = 0; i < fileArray.Length; i++)
                imageList.Add(new ImageNode(i, fileArray[i]));

            imageList.Sort(new ImageNode());

            if (settings.SortSizes)
            {
                imageList.Sort(new BitmapSizeComparer());
            }

            for (int i = 0; i < imageList.Count; i++)
            {
                ImageNode image = imageList[i];
                Rectangle srcRectangle = new Rectangle(0, 0, image.Size.Width, image.Size.Height);
                Point position = Point.Empty;

                if (rectanglePacker.FindPoint(srcRectangle.Size, ref position))
                    image.Position = position;

                image.PaletteIndex = i;
            }

            string outputFileName = null;

            if (!FileIO.TrySaveFile(parent, null, null, "Bitmap Files", new string[] { ".bmp", ".png" }, out outputFileName))
            {
                Console.WriteLine("Error: Saving File '{0}'", outputFileName);
            }

            settings.OutputFileName = outputFileName;

            BatchProcessFolder(imageList, new List<SliceNode>(), settings);
        }

        public static void GetBitmaps(List<ImageNode> imageList, BatchSettings settings, out Color[] colorPalette)
        {
            colorPalette = new Color[256];
            List<Color> colorList = new List<Color>();

            for (int i = 0; i < imageList.Count; i++)
            {
                ImageNode image = imageList[i];

                if (!File.Exists(image.FileName))
                {
                    Console.WriteLine("Error: File '{0}' Not Found", image.FileName);

                    continue;
                }

                Bitmap originalBitmap = (Bitmap)Bitmap.FromFile(image.FileName);

                if (!settings.Quantize && (originalBitmap.PixelFormat != PixelFormat.Format4bppIndexed && originalBitmap.PixelFormat != PixelFormat.Format8bppIndexed))
                    continue;

                string name = Path.GetFileName(image.FileName);
                image.Bitmap = (settings.Quantize ? (Bitmap)ImageBuffer.QuantizeImage(originalBitmap, new WuColorQuantizer(), null, 16, 1) : originalBitmap);
                SetBitmapNearestColorPalette(image.Bitmap, null, settings.ColorPalette, NearestColorMode.Sqrt);
                //RemoveUnusedColors(image.Bitmap);
                ColorPalette palette = image.Bitmap.Palette;
                int totalColors = palette.Entries.Length;
                image.Palette = GetBitmapUsedColorPalette(image.Bitmap);

                colorList.AddRange(image.Palette);

                for (int j = 0; j < 16 - image.Palette.Length; j++)
                {
                    colorList.Add(Color.Empty);
                }
            }

            for (int i = 0; i < imageList.Count; i++)
            {
                ImageNode image = imageList[i];
                List<Color> colorPaletteTemp = new List<Color>(colorList);

                for (int j = 0; j < 16; j++)
                    colorPaletteTemp[i * 16 + j] = Color.Empty;

                image.ClosestPaletteIndex = GetPaletteMatchIndex(image.Palette, colorPaletteTemp.ToArray(), NearestColorMode.Sqrt);

                imageList[image.ClosestPaletteIndex].PaletteIndexCount++;

                //Color[] nearestColorPalette = new Color[16];
                //Array.Copy(colorPaletteTemp.ToArray(), image.ClosestPaletteIndex * 16, nearestColorPalette, 0, 16);

                //SetBitmapNearestColorPaletteIndices(image.Bitmap, nearestColorPalette, NearestColorMode.Sqrt);
            }

            //imageList.Sort(new ClosestPaletteIndexComparer());
            //imageList.Reverse()

            imageList.Sort(new PaletteIndexCountComparer());
            imageList.Reverse();

            /* for (int i = 0; i < imageList.Count; i++)
            {
                ImageNode image = imageList[i];

                Console.WriteLine("{0}: {1} -> {2}", i, image.ClosestPaletteIndex, image.PaletteIndexCount);
            } */

            imageList.Sort(new PaletteLengthComparer());
            imageList.Reverse();
        }

        public static void BatchProcessFolder(List<ImageNode> imageList, List<SliceNode> sliceList, BatchSettings settings)
        {
            if (!Directory.Exists(settings.SourceDirectory))
            {
                Console.WriteLine("Error: Source Directory '{0}' Doesn't Exist", settings.SourceDirectory);

                return;
            }

            Color[] colorPalette = new Color[256];
            bool[] paletteUsed = new bool[16];
            int paletteCount = 0;
            ColorPalette globalPalette;

            GetBitmaps(imageList, settings, out colorPalette);

            for (int i = 0; i < imageList.Count; i++)
            {
                ImageNode image = imageList[i];

                if (image.Bitmap == null)
                    continue;

                string name = Path.GetFileName(image.FileName);
                int paletteIndex = (image.PaletteIndex == -2 ? paletteCount++ : image.PaletteIndex);
                int paletteOffset = paletteIndex * 16;
                ColorPalette palette = image.Bitmap.Palette;
                int totalColors = palette.Entries.Length;

                if (paletteCount > settings.MaxPaletteCount || paletteIndex > 15)
                {
                    paletteIndex = GetPaletteMatchIndex(image.Bitmap.Palette.Entries, colorPalette, NearestColorMode.Sqrt);
                    paletteOffset = paletteIndex * 16;
                }

                Console.WriteLine("Processing '{0}' palette index {1} ({2} used of {3} colors)", name, paletteIndex, image.Palette.Length, totalColors);

                if (paletteIndex != -1 && paletteUsed[paletteIndex])
                {
                    Color[] nearestColorPalette = new Color[16];
                    Array.Copy(colorPalette, paletteOffset, nearestColorPalette, 0, 16);

                    Console.WriteLine("'{0}' Reusing PaletteIndex {1}", image.FileName, paletteIndex);

                    SetBitmapNearestColorPaletteIndices(image.Bitmap, nearestColorPalette, NearestColorMode.Sqrt);
                    palette = image.Bitmap.Palette;
                }
                else
                {
                    int transparentIndex = -1;

                    if (settings.SwapMagentaWithTransparentIndex)
                    {
                        for (int j = 0; j < palette.Entries.Length; j++)
                        {
                            if (palette.Entries[j].R == Color.Magenta.R &&
                                palette.Entries[j].G == Color.Magenta.G &&
                                palette.Entries[j].B == Color.Magenta.B)
                            {
                                transparentIndex = j;
                                break;
                            }
                        }

                        if (transparentIndex != -1)
                        {
                            SwapColors(image.Bitmap, settings.TransparentIndex, transparentIndex);
                            palette = image.Bitmap.Palette;
                        }
                    }

                    if (settings.SortColors)
                    {
                        SortBitmapPalette(image.Bitmap, SortColorMode.Sqrt, settings.TransparentIndex);
                        palette = image.Bitmap.Palette;
                    }
                }

                Bitmap newBitmap = new Bitmap(image.Bitmap.Width, image.Bitmap.Height, PixelFormat.Format8bppIndexed);
                CopyBitmap(image.Bitmap, newBitmap, new Rectangle(Point.Empty, image.Bitmap.Size), new Rectangle(Point.Empty, image.Bitmap.Size));

                if (paletteIndex != -1)
                {
                    if (settings.AddPaletteOffset)
                    {
                        AddColorIndexOffset(newBitmap, paletteOffset);
                    }

                    if (!paletteUsed[paletteIndex])
                    {
                        for (int j = 0; j < palette.Entries.Length && j < 16; j++)
                        {
                            colorPalette[paletteOffset + j] = palette.Entries[j];
                        }

                        paletteUsed[paletteIndex] = true;
                    }
                }

                image.Bitmap.Dispose();
                image.Bitmap = newBitmap;
            }

            using (Bitmap originalBitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                globalPalette = originalBitmap.Palette;
                Color[] globalEntries = globalPalette.Entries;

                for (int i = 0; i < 256; i++)
                {
                    if (colorPalette[i].A == 0)
                        globalEntries[i] = colorPalette[i];
                    else
                        globalEntries[i] = GetNearestColor(colorPalette[i], settings.ColorPalette, NearestColorMode.Sqrt);
                }
            }

            if (!settings.CreateCombinedImage)
            {
                if (Directory.Exists(settings.DestinationDirectory))
                {
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        ImageNode image = imageList[i];
                        Bitmap bitmap = image.Bitmap;
                        string fileName = Path.GetFileName(image.FileName);
                        string destinationFileName = Path.Combine(settings.DestinationDirectory, fileName);

                        bitmap.Palette = globalPalette;
                        bitmap.Save(destinationFileName);
                    }
                }
            }

            Rectangle outputRectangle = new Rectangle(Point.Empty, settings.OutputSize);

            using (Bitmap outputBitmap = new Bitmap(settings.OutputSize.Width, settings.OutputSize.Height, PixelFormat.Format8bppIndexed))
            {
                ClearBitmap(outputBitmap, settings.TransparentIndex);

                for (int i = 0; i < imageList.Count; i++)
                {
                    ImageNode image = imageList[i];
                    Bitmap bitmap = image.Bitmap;
					Rectangle srcRectangle = new Rectangle(Point.Empty, bitmap.Size);
					Rectangle dstRectangle = new Rectangle(image.Position.X, image.Position.Y, bitmap.Width, bitmap.Height);

					if (settings.AutoPosition)
					{
						int dstWidth = bitmap.Width, imageCount = 1;

						dstRectangle.Intersect(outputRectangle);
						dstWidth -= dstRectangle.Width;

						CopyBitmap(bitmap, outputBitmap, srcRectangle, dstRectangle);

						while (dstWidth > 0)
						{
							srcRectangle = new Rectangle(bitmap.Width - dstWidth, 0, dstWidth, bitmap.Height);
							dstRectangle = new Rectangle(0, image.Position.Y + bitmap.Height * imageCount, dstWidth, bitmap.Height);

							dstRectangle.Intersect(outputRectangle);
							dstWidth -= dstRectangle.Width;

							CopyBitmap(bitmap, outputBitmap, srcRectangle, dstRectangle);

							imageCount++;
						}
					}
					else
					{
						CopyBitmap(bitmap, outputBitmap, srcRectangle, dstRectangle);
					}

                    image.Bitmap.Dispose();
                    image.Bitmap = null;
                }

                if (settings.CreateCombinedImage)
                {
                    if (File.Exists(settings.OutputFileName))
                        File.Delete(settings.OutputFileName);

                    outputBitmap.Palette = globalPalette;
                    outputBitmap.Save(settings.OutputFileName, Path.GetExtension(settings.OutputFileName) == ".png" ? ImageFormat.Png : ImageFormat.Bmp);
                }

                foreach (SliceNode slice in sliceList)
                {
                    using (Bitmap sliceBitmap = new Bitmap(slice.Size.Width, slice.Size.Height, PixelFormat.Format8bppIndexed))
                    {
                        sliceBitmap.Palette = globalPalette;

                        CopyBitmap(outputBitmap, sliceBitmap, new Rectangle(slice.Position, slice.Size), new Rectangle(Point.Empty, slice.Size));

                        sliceBitmap.Save(slice.FileName, Path.GetExtension(slice.FileName) == ".png" ? ImageFormat.Png : ImageFormat.Bmp);
                    }
                }
            }
        }

        public static void WriteNextTiles(string tilesFileName, string paletteFileName, string outputFileName, int tileWidth, int tileHeight, bool colors4Bit)
        {
            byte[] tileData = File.ReadAllBytes(tilesFileName);
            int indexCount = (colors4Bit ? 2 : 1);
            int dataSize = tileData.Length * indexCount;
            int tileSize = tileWidth * tileHeight;
            int tileCount = dataSize / tileSize;
            int bitmapWidth = Math.Min(256, tileCount * tileWidth);
            Size bitmapSize = new Size(bitmapWidth, (int)Math.Ceiling((double)(dataSize / bitmapWidth) / tileHeight) * tileHeight);
            int tileCols = bitmapSize.Width / tileWidth;
            int tileRows = bitmapSize.Height / tileHeight;

            using (Bitmap bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height, PixelFormat.Format8bppIndexed))
            {
                ColorPalette colorPalette = bitmap.Palette;

                for (int i = 0; i < colorPalette.Entries.Length; i++)
                    colorPalette.Entries[i] = Color.FromArgb(255, 0, 0, 0);

                using (BinaryReader binaryReader = new BinaryReader(File.Open(paletteFileName, FileMode.Open)))
                {
                    int count = (int)binaryReader.BaseStream.Length / sizeof(UInt16);

                    for (int i = 0; i < count; i++)
                    {
                        UInt16 value = binaryReader.ReadUInt16();
                        Color color = Color.FromArgb((int)Colors.RGB332ToRGB888(value));
                        colorPalette.Entries[i] = Color.FromArgb(255, color);
                    }
                }

                BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                int indicesCount = bmpData.Height * bmpData.Stride;
                byte[] indices = new byte[indicesCount];

                Marshal.Copy(bmpData.Scan0, indices, 0, indicesCount);

                for (int t = 0; t < tileCount; t++)
                {
                    int tileX = t % tileCols;
                    int tileY = t / tileCols;
                    int srcOffset = t * tileSize;
                    int dstOffset = tileY * bitmapSize.Width * tileHeight + tileX * tileWidth;

                    for (int y = 0; y < tileHeight; y++)
                    {
                        for (int x = 0; x < tileWidth; x++)
                        {
                            int srcIndex = srcOffset + y * tileWidth + x;
                            int dstIndex = dstOffset + y * bitmapSize.Width + x;

                            if (colors4Bit)
                                srcIndex >>= 1;

                            if (colors4Bit)
                            {
                                if ((x & 1) == 0)
                                    indices[dstIndex] = (byte)(tileData[srcIndex] >> 4);
                                else
                                    indices[dstIndex] = (byte)(tileData[srcIndex] & 0xf);
                            }
                            else
                            {
                                indices[dstIndex] = (byte)tileData[srcIndex];
                            }
                        }
                    }
                }

                Marshal.Copy(indices, 0, bmpData.Scan0, indicesCount);

                bitmap.Palette = colorPalette;

                bitmap.UnlockBits(bmpData);

                bitmap.Save(outputFileName, Path.GetExtension(outputFileName) == ".png" ? ImageFormat.Png : ImageFormat.Bmp);
            }
        }

        public static Color GetComplement(Color color)
        {
            return WithHueOffset(color, 180);
        }

        public static Color GetSplitComplement0(Color color)
        {
            return WithHueOffset(color, 150);
        }

        public static Color GetSplitComplement1(Color color)
        {
            return WithHueOffset(color, 210);
        }

        public static Color GetTriadic0(Color color)
        {
            return WithHueOffset(color, 120);
        }

        public static Color GetTriadic1(Color color)
        {
            return WithHueOffset(color, 240);
        }

        public static Color GetTetradic0(Color color)
        {
            return WithHueOffset(color, 90);
        }

        public static Color GetTetradic1(Color color)
        {
            return GetComplement(color);
        }

        public static Color GetTetradic2(Color color)
        {
            return WithHueOffset(color, 270);
        }

        public static Color GetAnalagous0(Color color)
        {
            return WithHueOffset(color, -30);
        }

        public static Color GetAnalagous1(Color color)
        {
            return WithHueOffset(color, 30);
        }

        public static Color WithHueOffset(Color color, float offset)
        {
            HSL hsl = HSL.FromRGB(color);
            hsl.Hue = (hsl.Hue + offset) % 360;
            return hsl.RGB;
        }
    }

    class SqrtSorter : IComparer<ColorNode>
    {
        private int m_transparentIndex = -1;

        public SqrtSorter()
        {
        }

        public SqrtSorter(int transparentIndex)
        {
            m_transparentIndex = transparentIndex;
        }

        #region IComparer Members

        public int Compare(ColorNode cnx, ColorNode cny)
        {
            if (m_transparentIndex != -1)
            {
                if (cnx.Index == m_transparentIndex || cny.Index == m_transparentIndex)
                {
                    if (cnx.Index > cny.Index)
                        return cny.Index.CompareTo(cnx.Index);
                    else
                        return cnx.Index.CompareTo(cny.Index);
                }
            }

            double vx = cnx.Value;
            double vy = cny.Value;

            if (vx.Equals(vy))
            {
                if (cnx.Index > cny.Index)
                    return cny.Index.CompareTo(cnx.Index);
                else
                    return cnx.Index.CompareTo(cny.Index);
            }

            return vx.CompareTo(vy);
        }

        #endregion
    }

	class HSLSorter : IComparer<ColorNode>
	{
		private int m_transparentIndex = -1;

		public HSLSorter()
		{
		}

		public HSLSorter(int transparentIndex)
		{
			m_transparentIndex = transparentIndex;
		}

		public int Compare(ColorNode cnx, ColorNode cny)
		{
			if (m_transparentIndex != -1)
			{
				if (cnx.Index == m_transparentIndex || cny.Index == m_transparentIndex)
				{
					if (cnx.Index > cny.Index)
						return cny.Index.CompareTo(cnx.Index);
					else
						return cnx.Index.CompareTo(cny.Index);
				}
			}

			HSL hslX = HSL.FromRGB(cnx.Color.R, cnx.Color.G, cnx.Color.B);
			HSL hslY = HSL.FromRGB(cny.Color.R, cny.Color.G, cny.Color.B);

			if (hslX.Hue < hslY.Hue || (hslX.Hue == hslY.Hue && hslX.Saturation < hslY.Saturation) || (hslX.Hue == hslY.Hue && hslX.Saturation == hslY.Saturation && hslX.Luminance < hslY.Luminance))
			{
				return -1;
			}
			else if (hslX.Hue == hslY.Hue && hslX.Saturation == hslY.Saturation && hslX.Luminance == hslY.Luminance)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}
	}

	class LabSorter : IComparer<ColorNode>
	{
		private int m_transparentIndex = -1;

		public LabSorter()
		{
		}

		public LabSorter(int transparentIndex)
		{
			m_transparentIndex = transparentIndex;
		}

		#region IComparer Members

		public int Compare(ColorNode cnx, ColorNode cny)
		{
			if (m_transparentIndex != -1)
			{
				if (cnx.Index == m_transparentIndex || cny.Index == m_transparentIndex)
				{
					if (cnx.Index > cny.Index)
						return cny.Index.CompareTo(cnx.Index);
					else
						return cnx.Index.CompareTo(cny.Index);
				}
			}

			CIELab vx = Lab.RGBtoLab(cnx.Color);
			CIELab vy = Lab.RGBtoLab(cny.Color);
			CIELab vz = Lab.RGBtoLab(Color.Empty);

			double vxd1 = Lab.GetDeltaE_CIEDE2000(vx, vz);
			double vxd2 = Lab.GetDeltaE_CMC(vx, vz);
			double vxd = (vxd1 + vxd2) / 2.0;

			double vyd1 = Lab.GetDeltaE_CIEDE2000(vy, vz);
			double vyd2 = Lab.GetDeltaE_CMC(vy, vz);
			double vyd = (vyd1 + vyd2) / 2.0;

			if (vxd.Equals(vyd))
			{
				if (cnx.Index > cny.Index)
					return cny.Index.CompareTo(cnx.Index);
				else
					return cnx.Index.CompareTo(cny.Index);
			}

			return vxd.CompareTo(vyd);
		}

		#endregion
	}

	public class ColorNode
    {
        public int Index;
        public Color Color;

        public ColorNode()
        {
            Index = -1;
            Color = Color.Empty;
        }

        public ColorNode(int index, Color color)
        {
            Index = index;
            Color = color;
        }

        public double Value
        {
            get { return Math.Sqrt(Math.Pow(Color.A, 2) + Math.Pow(Color.R, 2) + Math.Pow(Color.G, 2) + Math.Pow(Color.B, 2)); }
        }
    }

    public class ColorSorter : IComparer<Color>
    {
        #region IComparer Members

        public int Compare(Color cx, Color cy)
        {
            double dx = Math.Pow(cx.A, 2) + Math.Pow(cx.R, 2) + Math.Pow(cx.G, 2) + Math.Pow(cx.B, 2);
            double dy = Math.Pow(cy.A, 2) + Math.Pow(cy.R, 2) + Math.Pow(cy.G, 2) + Math.Pow(cy.B, 2);

            return dx.CompareTo(dy);
        }

        #endregion
    }

    public class BatchSettings
    {
        public string SourceDirectory;
        public string DestinationDirectory;
        public string OutputFileName;
        public Size OutputSize;
        public Color[] ColorPalette;
        public int TransparentIndex;
        public bool SwapMagentaWithTransparentIndex;
        public bool SortSizes;
        public bool SortColors;
        public bool Quantize;
        public bool AddPaletteOffset;
        public bool CreateCombinedImage;
		public bool AutoPosition;
		public int MaxPaletteCount;
    }

    public class ImageNode : IComparer<ImageNode>
    {
        public int Index = -1;
        public string FileName = null;
        public Point Position = Point.Empty;
        public Size Size = Size.Empty;
        public Bitmap Bitmap = null;
        public Color[] Palette = null;
        public int PaletteIndex = -1;
        public int ClosestPaletteIndex = -1;
        public int PaletteIndexCount = 0;

        public ImageNode()
        {
        }

        public ImageNode(int index, string fileName)
        {
            Index = index;
            FileName = fileName;

            ImageHeader.TryGetDimensions(fileName, out Size);
        }

        public ImageNode(int index, string fileName, Point position, int paletteIndex)
        {
            Index = index;
            FileName = fileName;
            Position = position;
            PaletteIndex = paletteIndex;

            ImageHeader.TryGetDimensions(fileName, out Size);
        }

        #region IComparer<ImageAtlasNode> Members

        public int Compare(ImageNode x, ImageNode y)
        {
            return x.FileName.CompareTo(y.FileName);
        }

        #endregion
    }

    public class SliceNode
    {
        public string FileName = null;
        public Point Position = Point.Empty;
        public Size Size = Size.Empty;

        public SliceNode(string fileName, Point position, Size size)
        {
            FileName = fileName;
            Position = position;
            Size = size;
        }
    }

    public class BitmapSizeComparer : IComparer<ImageNode>
    {
        public int Compare(ImageNode x, ImageNode y)
        {
            int width = -(x.Size.Width).CompareTo(y.Size.Width);
            int height = -(x.Size.Height).CompareTo(y.Size.Height);

            if (width == 0 && height == 0)
                return x.Index.CompareTo(y.Index);

            return (width != 0 ? width : height);
        }
    }

    public class PaletteLengthComparer : IComparer<ImageNode>
    {
        public int Compare(ImageNode x, ImageNode y)
        {
            return x.Palette.Length.CompareTo(y.Palette.Length);
        }
    }

    public class ClosestPaletteIndexComparer : IComparer<ImageNode>
    {
        public int Compare(ImageNode x, ImageNode y)
        {
            return x.ClosestPaletteIndex.CompareTo(y.ClosestPaletteIndex);
        }
    }

    public class PaletteIndexCountComparer : IComparer<ImageNode>
    {
        public int Compare(ImageNode x, ImageNode y)
        {
            return x.PaletteIndexCount.CompareTo(y.PaletteIndexCount);
        }
    }
}

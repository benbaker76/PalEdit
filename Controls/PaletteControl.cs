using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

using FreeImageAPI;

using ControlsEx.ColorManagement;
using ControlsEx.ColorManagement.Gradients;

using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers.XiaolinWu;

namespace PalEdit
{
    public partial class PaletteControl : UserControl
    {
        private Bitmap m_bitmap = null;
        private PalNode[] m_palClipboard = null;

        private Keys m_modifierKeys;

        private bool m_cutClipboard = false;

        private Point m_startPoint = Point.Empty;
        private Point m_endPoint = Point.Empty;
        private bool m_mouseDown = false;

        private bool m_allowMultipleSelection = true;

        private MouseButtons m_lastButtonUp = MouseButtons.None;

        private bool m_eyeDropper = false;

        private GradientCollection m_gradientCollection = null;

        public delegate void ResetControlDelegate();
        public delegate void UpdateBitmapDelegate(Color transparentColor);
        public delegate void CloseBitmapDelegate();
        public delegate void SetBitmapDelegate(Bitmap bitmap);

        public EventHandler<ColorEventArgs> OnPaletteSelect = null;
        public event ResetControlDelegate OnResetControl = null;
        public event UpdateBitmapDelegate OnUpdateBitmap = null;
        public event CloseBitmapDelegate OnCloseBitmap = null;
        public event SetBitmapDelegate OnSetBitmap = null;

        public PaletteControl()
        {
            string gradientsPath = Path.Combine(Application.StartupPath, "Gradients");
            string gradientsFile = Path.Combine(gradientsPath, "default.grdx");

            if (File.Exists(gradientsFile))
            {
                m_gradientCollection = new GradientCollection();
                m_gradientCollection.Load(gradientsFile);
            }

            InitializeComponent();

            NewPalette(256);
        }

        private void DrawTransparency(Graphics g, int index)
        {
            Size gridSize = new Size(SwatchSize.Width / 4, SwatchSize.Height / 4);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Color color = ((x + y) % 2 == 0 ? Color.White : Color.LightGray);

                    using (SolidBrush b = new SolidBrush(color))
                        g.FillRectangle(b, new Rectangle(Palette[index].Rectangle.X + x * gridSize.Width, Palette[index].Rectangle.Y + y * gridSize.Height, gridSize.Width, gridSize.Height));
                }
            }
        }

        private void DrawCross(Graphics g, int index)
        {
            using (Pen p = new Pen(Color.Red, 2))
            {
                g.DrawLine(p, Palette[index].Rectangle.X, Palette[index].Rectangle.Y, Palette[index].Rectangle.X + Palette[index].Rectangle.Width, Palette[index].Rectangle.Y + Palette[index].Rectangle.Height);
                g.DrawLine(p, Palette[index].Rectangle.X + Palette[index].Rectangle.Width, Palette[index].Rectangle.Y, Palette[index].Rectangle.X, Palette[index].Rectangle.Y + Palette[index].Rectangle.Height);
            }
        }

        private void DrawColors(Graphics g)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (i < PalUsed)
                {
                    if (Palette[i].Alpha == 0)
                        DrawTransparency(g, i);
                    else
                        using (SolidBrush b = new SolidBrush(Palette[i].Color))
                            g.FillRectangle(b, Palette[i].Rectangle);
                }
                else
                {
                    DrawTransparency(g, i);
                    DrawCross(g, i);
                }
            }
        }

        private void DrawSelection(Graphics g)
        {
            using (Pen p = new Pen(Color.Red, 2))
            {
                for (int i = 0; i < Palette.Length; i++)
                {
                    if (!Palette[i].IsSelected)
                        continue;

                    g.DrawRectangle(p, Palette[i].Rectangle);
                }
            }
        }


        private void DrawGrid(Graphics g)
        {
            using (Pen p = new Pen(Color.FromArgb(237, 237, 237), 2))
            {
                for (int i = 0; i < Palette.Length; i++)
                    g.DrawRectangle(p, Palette[i].Rectangle);
            }
        }

        private void ClearTransparency()
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (Selected == Palette[i])
                    continue;

                Palette[i].Alpha = 255;
            }
        }

        public Color GetTransparentColor()
        {
            Color transparentColor = Color.Black;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (Palette[i].Alpha != 0)
                    continue;

                transparentColor = Palette[i].Color;
            }

            return transparentColor;
        }

        private void UpdateBitmapPalette()
        {
            if (m_bitmap == null)
                return;

            ColorPalette colorPalette = m_bitmap.Palette;
            Color transparentColor = Color.Black;

            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                colorPalette.Entries[i] = (i < Palette.Length ? Palette[i].Color : Color.Empty);

                if (colorPalette.Entries[i].A == 0)
                    transparentColor = colorPalette.Entries[i];
            }

            m_bitmap.Palette = colorPalette;

            OnUpdateBitmap?.Invoke(transparentColor);
        }

        public void NewPalette(int palCount)
        {
            DestroyBitmap();

            ResetPalette(palCount);
            ResizePalette();
            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        private void ResetPalette(int palCount)
        {
            Palette = new PalNode[palCount];
            m_palClipboard = new PalNode[palCount];

            for (int i = 0; i < Palette.Length; i++)
                 Palette[i] = new PalNode(Color.Black);

            RefreshPalette();
        }

        private void ResizePalette()
        {
            this.ClientSize = new Size(SwatchSize.Width * Columns + (Offset.X * 2), SwatchSize.Height * Rows + (Offset.Y * 2));
        }

        private void RefreshPalette()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    int index = x + y * Columns;

                    if (index >= Palette.Length)
                        break;

                    Palette[index].Rectangle = new Rectangle(Offset.X + x * SwatchSize.Width, Offset.Y + y * SwatchSize.Height, SwatchSize.Width, SwatchSize.Height);
                }
            }
        }

        private Size SnapToGrid(SizeF size, float snapValue)
        {
            double width = Math.Floor((double)size.Width / snapValue) * snapValue;
            double height = Math.Floor((double)size.Height / snapValue) * snapValue;
            return new Size((int)width, (int)height);
        }

        public void DrawPalette()
        {
            this.Invalidate();
            UpdateBitmapPalette();
        }

        public Color SetSelectedIndex(int index)
        {
            if (index < 0 || index >= Palette.Length)
                return SelectedColor;

            SetIsSelected(false);
            Selected = Palette[index];
            Selected.IsSelected = true;

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());

            return SelectedColor;
        }

        private void UpdatePaletteSelection()
        {
            bool ctrlDown = (m_modifierKeys & Keys.Control) != 0;
            bool shiftDown = (m_modifierKeys & Keys.Shift) != 0;
            Point startPoint = new Point(m_startPoint.X < m_endPoint.X ? m_startPoint.X : m_endPoint.X, m_startPoint.Y < m_endPoint.Y ? m_startPoint.Y : m_endPoint.Y);
            Point endPoint = new Point(m_endPoint.X > m_startPoint.X ? m_endPoint.X : m_startPoint.X, m_endPoint.Y > m_startPoint.Y ? m_endPoint.Y : m_startPoint.Y);
            Rectangle selectRect = new Rectangle(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

            if (m_allowMultipleSelection || shiftDown)
            {
                Selected = null;

                for (int i = 0; i < Palette.Length; i++)
                {
                    if (selectRect.IntersectsWith(Palette[i].Rectangle))
                    {
                        if (!Palette[i].IsSelected)
                            Palette[i].IsSelected = true;

                        if (Selected == null)
                        {
                            Selected = Palette[i];

                            OnPaletteSelect?.Invoke(this, new ColorEventArgs(i, Selected.Color));
                        }
                    }
                    else
                    {
                        if (Palette[i].IsSelected && !ctrlDown)
                            Palette[i].IsSelected = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Palette.Length; i++)
                {
                    if (Palette[i].Rectangle.Contains(m_endPoint))
                    {
                        //if (!Palette[i].IsSelected)
                        {
                            SetIsSelected(false);
                            Selected = Palette[i];
                            Palette[i].IsSelected = true;

                            OnPaletteSelect?.Invoke(this, new ColorEventArgs(i, Selected.Color));

                            break;
                        }
                    }
                }
            }

            DrawPalette();
        }

        public void SelectAll()
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                Palette[i].IsSelected = true;

                if (!m_allowMultipleSelection)
                    break;
            }

            DrawPalette();
        }

        public void SelectNone()
        {
            m_startPoint = Point.Empty;
            m_endPoint = Point.Empty;
            m_mouseDown = false;
            Selected = null;
            SetIsSelected(false);
            DrawPalette();
        }

        public void SetIsSelected(bool value)
        {
            for (int i = 0; i < Palette.Length; i++)
                Palette[i].IsSelected = value;
        }

        public void SelectInverse()
        {
            for (int i = 0; i < Palette.Length; i++)
                Palette[i].IsSelected = !Palette[i].IsSelected;

            DrawPalette();
        }

        public void SelectUsedColors(bool clearSelected)
        {
            SelectUsedColors(Rectangle.Empty, clearSelected);
        }

        public void SelectUsedColors(Rectangle selectRectangle, bool clearSelected)
        {
            if (m_bitmap == null)
                return;

            if (clearSelected)
            {
                for (int i = 0; i < Palette.Length; i++)
                    Palette[i].IsSelected = false;
            }

            int[] colorIndices = null;
            bool hasSetSelected = false;

            Colors.GetColorIndices(m_bitmap, selectRectangle, out colorIndices);

            for (int i = 0; i < colorIndices.Length; i++)
            {
                int colorIndex = colorIndices[i];
                Palette[colorIndex].IsSelected = true;

                if (!hasSetSelected)
                {
                    hasSetSelected = true;
                    Selected = Palette[colorIndex];
                }
            }

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SelectMatchingColors()
        {
            if (m_bitmap == null)
                return;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (Palette[i].IsSelected)
                {
                    for (int j = 0; j < Palette.Length; j++)
                    {
                        if (Palette[i].Color.Equals(Palette[j].Color))
                            Palette[j].IsSelected = true;
                    }
                }
            }

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SelectMatchingColors(Color[] colorPalette)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                bool foundColor = false;
                
                for (int j = 0; j < colorPalette.Length; j++)
                {
                    if (Color.FromArgb(0xFF, colorPalette[j]).Equals(Color.FromArgb(0xFF, Palette[i].Color)))
                        foundColor = true;
                }

                Palette[i].IsSelected = foundColor;
            }

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SelectNonMatchingColors(Color[] colorPalette)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                bool foundColor = false;

                for (int j = 0; j < colorPalette.Length; j++)
                {
                    if (Color.FromArgb(0xFF, colorPalette[j]).Equals(Color.FromArgb(0xFF, Palette[i].Color)))
                        foundColor = true;
                }

                Palette[i].IsSelected = !foundColor;
            }

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetFirstSelectedColor(Color color)
        {
            Selected.Color = color;

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetNearestSelectedColor(Color color)
        {
            SetSelectedIndex(Colors.GetNearestColorIndex(color, ColorArray, Colors.NearestColorMode.Sqrt));

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetAllSelectedColor(Color color)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Color = color;
            }

            DrawPalette();
            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetPalette(Color[] colorPalette)
        {
            for (int i = 0; i < Palette.Length; i++)
                Palette[i].Color = colorPalette[i];
        }

        public void SetPaletteWithAlpha(Color[] colorPalette, int alpha)
        {
            for (int i = 0; i < Palette.Length; i++)
                Palette[i].Color = Color.FromArgb(alpha, colorPalette[i]);
        }

        public void SetColorPalette(Color[] colorPalette, bool destroyBitmap)
        {
            if (m_bitmap == null || destroyBitmap)
            {
                DestroyBitmap();

                PalUsed = colorPalette.Length;
                ResetPalette(colorPalette.Length);
                SetPaletteWithAlpha(colorPalette, 0xFF);
                ResizePalette();
                DrawPalette();
                OnPaletteSelect?.Invoke(this, new ColorEventArgs());
            }
            else
            {
                PalUsed = colorPalette.Length;
                ResetPalette(colorPalette.Length);
                Colors.SetBitmapNearestColorPaletteIndices(m_bitmap, colorPalette, Colors.NearestColorMode.Sqrt);
                SetPaletteWithAlpha(colorPalette, 0xFF);
                ResizePalette();
                DrawPalette();
                OnPaletteSelect?.Invoke(this, new ColorEventArgs());
            }
        }

        public void SetBitmapColorPalette(Color[] colorPalette, Colors.NearestColorMode nearestColorMode, bool selectedOnly)
        {
            if (m_bitmap != null)
            {
                Colors.SetBitmapNearestColorPalette(m_bitmap, selectedOnly ? SelectedIndices : null, colorPalette, nearestColorMode);

                SetBitmapPalette(m_bitmap);
            }

            Color[] palette = ColorArray;
 
            Colors.SetNearestColorPalette(palette, colorPalette, nearestColorMode);

            if (selectedOnly)
                SetPaletteSelected(palette);
            else
                SetPalette(palette);

            UpdateBitmapPalette();
            DrawPalette();
        }

        public void QuantizeBitmap(int colorOffset, int colorCount)
        {
            if (m_bitmap == null)
                return;

            Bitmap newBitmap = new Bitmap(m_bitmap.Width, m_bitmap.Height, PixelFormat.Format8bppIndexed);
            newBitmap.SetResolution(m_bitmap.HorizontalResolution, m_bitmap.VerticalResolution);
            ColorPalette newPalette = newBitmap.Palette;

            for (int i = 0; i < Palette.Length; i++)
            {
                newPalette.Entries[i] = Color.Black;
            }

            using (Bitmap bitmap = (Bitmap)ImageBuffer.QuantizeImage(m_bitmap, new WuColorQuantizer(), null, colorCount, 1))
            {
                ColorPalette palette = bitmap.Palette;

                Colors.CopyBitmap(bitmap, newBitmap, new Rectangle(Point.Empty, newBitmap.Size), new Rectangle(Point.Empty, newBitmap.Size));
                Colors.AddColorIndexOffset(newBitmap, colorOffset);

                for (int i = 0; i < colorCount; i++)
                    newPalette.Entries[colorOffset + i] = palette.Entries[i];
            }

            newBitmap.Palette = newPalette;

            SetBitmap(newBitmap);
        }

        public void SetBitmapPalette(Bitmap bitmap)
        {
            ColorPalette colorPalette = bitmap.Palette;

            for (int i = 0; i < Palette.Length; i++)
                Palette[i].Color = (i < colorPalette.Entries.Length ? colorPalette.Entries[i] : Color.Black);
        }

        public void SetBitmap(Bitmap bitmap)
        {
            DestroyBitmap();

            m_bitmap = bitmap;
            PalUsed = m_bitmap.Palette.Entries.Length;

            OnSetBitmap?.Invoke(m_bitmap);

            ResetPalette(256);
            SetBitmapPalette(m_bitmap);
            ResizePalette();
            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());

            OnResetControl?.Invoke();
        }

        public void DestroyBitmap()
        {
            if (m_bitmap != null)
            {
                m_bitmap.Dispose();
                m_bitmap = null;
            }

            OnCloseBitmap?.Invoke();
        }

        public void OpenBitmapFile(string fileName)
        {
            Bitmap bitmap = null;

            //FileIO.TryLoadImage(fileName, out bitmap);
            //m_bitmap = (Bitmap)Bitmap.FromFile(fileName);

            try
            {
                FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_UNKNOWN;

                bitmap = FreeImage.LoadBitmap(fileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref imageFormat);
            }
            catch
            {
                FileIO.TryLoadImage(fileName, out bitmap);
            }

            if (bitmap.PixelFormat != PixelFormat.Format4bppIndexed && bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                using (Bitmap tempBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb))
                {
                    if (bitmap.HorizontalResolution > 0 && bitmap.VerticalResolution > 0)
                    {
                        tempBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                    }

                    using (Graphics g = Graphics.FromImage(tempBitmap))
                    {
                        g.DrawImage(bitmap, Point.Empty);
                        bitmap = (Bitmap)ImageBuffer.QuantizeImage(tempBitmap, new WuColorQuantizer(), null, 256, 1);
                    }
                }
            }

            SetBitmap(bitmap);
        }

        public void OpenPaletteFile(string fileName)
        {
            Color[] colorPalette = null;

            if (!PalFile.TryReadPalette(fileName, out colorPalette))
                return;

            DestroyBitmap();

            PalUsed = colorPalette.Length;

            ResetPalette(colorPalette.Length);

            ColorArray = colorPalette;

            ResizePalette();
            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());

            OnResetControl?.Invoke();
        }

        public void SetPaletteSelected(Color[] palette)
        {
            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Color = palette[i];
            }
        }

        public void SetClipboardPalette(PalNode[] palette, bool cutClipboard)
        {
            for (int i = 0; i < palette.Length; i++)
                m_palClipboard[i] = palette[i].Clone();

            m_cutClipboard = cutClipboard;
        }

        private void PastePalette(bool swap)
        {
            if (m_palClipboard[0] == null)
                return;

            int count = 0;

            for (int i = 0; i < Palette.Length; i++)
            {
                for (; count < m_palClipboard.Length; count++)
                    if (m_palClipboard[count].IsSelected)
                        break;

                if (count == Palette.Length)
                    break;

                if (Palette[i].IsSelected && m_palClipboard[count].IsSelected)
                {
                    Palette[i].Color = m_palClipboard[count].Color;

                    if (swap)
                        Palette[count].Color = m_palClipboard[i].Color;

                    count++;
                }
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        private void PasteBitmap()
        {
            if (m_palClipboard[0] == null)
                return;

            BitmapData bmpData = m_bitmap.LockBits(new Rectangle(0, 0, m_bitmap.Width, m_bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            int bmpSize = bmpData.Height * bmpData.Stride;
            byte[] pixels = new byte[bmpSize];
            Marshal.Copy(bmpData.Scan0, pixels, 0, bmpSize);

            int count = 0;

            for (int i = 0; i < Palette.Length; i++)
            {
                for (; count < Palette.Length; count++)
                    if (m_palClipboard[count].IsSelected)
                        break;

                if (count == Palette.Length)
                    break;

                if (Palette[i].IsSelected && m_palClipboard[count].IsSelected)
                {
                    for (int j = 0; j < pixels.Length; j++)
                    {
                        if (pixels[j] == i)
                            pixels[j] = (byte)count;
                        else if (pixels[j] == count)
                            pixels[j] = (byte)i;
                    }

                    count++;
                }
            }

            Marshal.Copy(pixels, 0, bmpData.Scan0, bmpSize);

            m_bitmap.UnlockBits(bmpData);

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SavePaletteFile(string fileName, PaletteFormat paletteFormat)
        {
            int transparentIndex = -1;
            Color[] colorPalette = new Color[Palette.Length];

            for (int i = 0; i < Palette.Length; i++)
            {
                colorPalette[i] = Palette[i].Color;

                if (Palette[i].Alpha == 0)
                    transparentIndex = i;
            }

            PalFile.WritePalette(fileName, colorPalette, transparentIndex, paletteFormat);

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SaveBitmapFile(string fileName, FREE_IMAGE_FORMAT imageFormat)
        {
            if (m_bitmap == null)
                return;

            FIBITMAP fiBitmap = FreeImage.CreateFromBitmap(m_bitmap);

            if (!fiBitmap.IsNull)
            {
                FreeImage.Save(imageFormat, fiBitmap, fileName, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
                FreeImage.Unload(fiBitmap);
            }
        }

        public void SaveBitmapFile(string fileName, ImageFormat imageFormat)
        {
            if (m_bitmap == null)
                return;

            m_bitmap.Save(fileName, imageFormat);
        }

        public void SetHue(int value, int maxValue)
        {
            if (Selected == null)
                return;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Hue = ((float)value / (float)maxValue) * 2f;
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetSaturation(int value, int maxValue)
        {
            if (Selected == null)
                return;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Saturation = ((float)value / (float)maxValue) * 2f;
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetBrightness(int value, int maxValue)
        {
            if (Selected == null)
                return;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Brightness = ((float)value / (float)maxValue) * 2f;
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetTint(int value, int maxValue)
        {
            if (Selected == null)
                return;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                Palette[i].Tint = ((float)value / (float)maxValue);
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void CutPalette()
        {
            SetClipboardPalette(Palette, true);
        }

        public void CopyPalette()
        {
            SetClipboardPalette(Palette, false);
        }

        public void PastePalette()
        {
            if (m_palClipboard[0] == null)
                return;

            if (m_cutClipboard)
            {
                if (m_bitmap != null)
                {
                    PasteBitmap();
                    PastePalette(false);
                }
                else
                {
                    PastePalette(false);
                }
            }
            else
            {
                int count = 0;

                for (int i = 0; i < Palette.Length; i++)
                {
                    for (; count < Palette.Length; count++)
                        if (m_palClipboard[count].IsSelected)
                            break;

                    if (count == Palette.Length)
                        break;

                    if (Palette[i].IsSelected && m_palClipboard[count].IsSelected)
                    {
                        Palette[i].Color = Color.FromArgb(255, m_palClipboard[count].Color);

                        count++;
                    }
                }
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void FillPalette()
        {
            if (m_palClipboard[0] == null)
                return;

            if (m_cutClipboard)
            {
                if (m_bitmap != null)
                {
                    PasteBitmap();
                    PastePalette(false);
                }
                else
                {
                    PastePalette(false);
                }
            }
            else
            {
                int count = 0;

                for (int i = 0; i < Palette.Length; i++)
                {
                    for (; count < Palette.Length; count++)
                        if (m_palClipboard[count].IsSelected)
                            break;

                    if (count == Palette.Length)
                    {
                        for (count = 0; count < Palette.Length; count++)
                            if (m_palClipboard[count].IsSelected)
                                break;
                    }

                    if (Palette[i].IsSelected && m_palClipboard[count].IsSelected)
                    {
                        Palette[i].Color = Color.FromArgb(255, m_palClipboard[count].Color);

                        count++;
                    }
                }
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SetEyeDropper(bool eyeDropper)
        {
            m_eyeDropper = eyeDropper;

            if (m_eyeDropper)
            {
                using (MemoryStream stream = new MemoryStream(PalEdit.Properties.Resources.Eyedropper_cur))
                    this.Cursor = new Cursor(stream);
            }
            else
                this.Cursor = Cursors.Default;
        }

        public void SwapPalette()
        {
            if (m_bitmap != null)
            {
                PasteBitmap();
                PastePalette(true);
            }
            else
            {
                PastePalette(true);
            }
        }

        public void MergePalette()
        {
            List<ColorNode> colorList = new List<ColorNode>();

            for (int i = 0; i < Palette.Length; i++)
            {
                if (Palette[i].IsSelected)
                    colorList.Add(new ColorNode(i, Palette[i].Color));
            }

            int count = 0;
            int[] colorIndices = new int[Palette.Length];

            for (int i = 0; i < Palette.Length; i++)
            {
                if (Palette[i].IsSelected)
                {
                    Palette[i].Color = colorList[count].Color;
                    colorIndices[colorList[count].Index] = SelectedIndex;
                    count++;
                }
                else
                    colorIndices[i] = i;
            }

			Colors.SetColorIndices(m_bitmap, colorIndices);

			DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        private Color InterpolateColors(Color startColor, Color endColor, double mix)
        {
            HSL startHSL = new HSL(startColor.GetHue(), startColor.GetSaturation(), startColor.GetBrightness());
            HSL endHSL = new HSL(endColor.GetHue(), endColor.GetSaturation(), endColor.GetBrightness());
            double alt = 1.0 - mix;
            double h = mix * startHSL.Hue + alt * endHSL.Hue;
            double s = mix * startHSL.Saturation + alt * endHSL.Saturation;
            double l = mix * startHSL.Luminance + alt * endHSL.Luminance;
            HSL hsl = new HSL((float)h, (float)s, (float)l);
            return hsl.RGB;
        }

        public int TryGetGradient(bool startAndEndOnly, out Gradient gradient)
        {
            gradient = new Gradient();
            gradient.Title = "Selected";
            gradient.Alphas.Add(new AlphaPoint(255, 0));
            gradient.Alphas.Add(new AlphaPoint(255, 1));

            List<Color> colorArray = new List<Color>();
            Color lastColor = Color.Empty;

            for (int i = 0; i < Palette.Length; i++)
            {
                if (!Palette[i].IsSelected)
                    continue;

                colorArray.Add(Palette[i].Color);
            }

            if (colorArray.Count == 0)
                return 0;

            if (startAndEndOnly)
            {
                gradient.Colors.Add(new ColorPoint(colorArray[0], 0));
                gradient.Colors.Add(new ColorPoint(colorArray[colorArray.Count - 1], 1));
            }
            else
            {
                for (int i = 0; i < colorArray.Count; i++)
                    gradient.Colors.Add(new ColorPoint(colorArray[i], (float)i / Math.Max(colorArray.Count - 1, 1)));
            }

            return colorArray.Count;
        }

        public void ShowGradientPicker(bool startAndEndOnly)
        {
            Gradient gradient = null;
            int colorCount = TryGetGradient(startAndEndOnly, out gradient);

            using (GradientCollectionEditor gradientCollectionEditor = new GradientCollectionEditor())
            {
                foreach (Gradient g in m_gradientCollection)
                    gradientCollectionEditor.Gradients.Add(g);

                if (gradient != null)
                {
                    gradientCollectionEditor.Gradients.Insert(0, gradient);
                    gradientCollectionEditor.SelectedGradient = gradient;
                }

                if (gradientCollectionEditor.ShowDialog() == DialogResult.OK)
                {
                    m_gradientCollection.Clear();

                    foreach (Gradient g in gradientCollectionEditor.Gradients)
                    {
                        if (g.Title == "Selected")
                            continue;

                        m_gradientCollection.Add(g);
                    }

                    Gradient selectedGradient = gradientCollectionEditor.SelectedGradient;
                    int index = 0;

                    for (int i = 0; i < Palette.Length; i++)
                    {
                        if (Palette[i].IsSelected)
                        {
                            Palette[i].Color = selectedGradient.GetColorAt((float)index / Math.Max(colorCount - 1, 1));
                            index++;
                        }
                    }
                }
            }

            DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

		private List<ColorNode> CreateColorList()
		{
			List<ColorNode> colorList = new List<ColorNode>();

			for (int i = 0; i < Palette.Length; i++)
			{
				if (!Palette[i].IsSelected)
					continue;

				colorList.Add(new ColorNode(i, Palette[i].Color));
			}

			return colorList;
		}

		private List<ColorNode> CreateSelectedColorList()
		{
			List<ColorNode> colorList = new List<ColorNode>();

			for (int i = 0; i < Palette.Length; i++)
			{
				if (!Palette[i].IsSelected)
					continue;

				colorList.Add(new ColorNode(i, Palette[i].Color));
			}

			return colorList;
		}

		private void UpdateIndices(List<ColorNode> colorList)
		{
			int count = 0;
			int[] colorIndices = new int[Palette.Length];

			for (int i = 0; i < Palette.Length; i++)
			{
				Palette[i].Color = colorList[count].Color;
				colorIndices[colorList[count].Index] = i;
				count++;
			}

			Colors.SetColorIndices(m_bitmap, colorIndices);
		}

		private void UpdateSelectedIndices(List<ColorNode> colorList)
		{
			int count = 0;
			int[] colorIndices = new int[Palette.Length];

			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = colorList[count].Color;
					colorIndices[colorList[count].Index] = i;
					count++;
				}
				else
					colorIndices[i] = i;
			}

			Colors.SetColorIndices(m_bitmap, colorIndices);
		}

		private int[] GetColorIndices()
		{
			int[] colorIndices;

			if (m_bitmap != null)
				Colors.GetColorIndices(m_bitmap, out colorIndices);
			else
				colorIndices = new int[Palette.Length];

			return colorIndices;
		}

		private List<int> GetSelectedIndices()
		{
			List<int> selectedIndices = new List<int>();

			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					selectedIndices.Add(i);
				}
			}

			return selectedIndices;
		}

		public void SortPalette(Colors.SortColorMode sortColorMode)
        {
			List<ColorNode> colorList = CreateColorList();

			Colors.SortColorList(colorList, sortColorMode);

			UpdateIndices(colorList);

			DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

        public void SortSelectedPalette(Colors.SortColorMode sortColorMode)
		{
			List<ColorNode> colorList = CreateSelectedColorList();

			Colors.SortColorList(colorList, sortColorMode);

			UpdateSelectedIndices(colorList);

			DrawPalette();

            OnPaletteSelect?.Invoke(this, new ColorEventArgs());
        }

		public void RotateLeftSelectedPalette()
		{
			List<ColorNode> colorList = CreateSelectedColorList();
			int[] colorIndices = GetColorIndices();
			List<int> selectedIndices = GetSelectedIndices();

			for (int i = 0; i < selectedIndices.Count; i++)
			{
				int index = selectedIndices[i];
				int newIndex = (i == selectedIndices.Count - 1 ? 0 : i + 1);
				Palette[index].Color = colorList[newIndex].Color;
				colorIndices[colorList[newIndex].Index] = index;
			}

			Colors.SetColorIndices(m_bitmap, colorIndices);

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RotateRightSelectedPalette()
		{
			List<ColorNode> colorList = CreateSelectedColorList();
			int[] colorIndices = GetColorIndices();
			List<int> selectedIndices = GetSelectedIndices();

			for (int i = 0; i < selectedIndices.Count; i++)
			{
				int index = selectedIndices[i];
				int newIndex = (i == 0 ? selectedIndices.Count - 1 : i - 1);
				Palette[index].Color = colorList[newIndex].Color;
				colorIndices[colorList[newIndex].Index] = index;
			}

			Colors.SetColorIndices(m_bitmap, colorIndices);

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}


		public void ReverseSelectedPalette()
		{
			List<ColorNode> colorList = CreateSelectedColorList();
			int[] colorIndices = GetColorIndices();
			List<int> selectedIndices = GetSelectedIndices();

			for (int i = 0; i < selectedIndices.Count; i++)
			{
				int index = selectedIndices[i];
				int newIndex = selectedIndices.Count - 1 - i;
				Palette[index].Color = colorList[newIndex].Color;
				colorIndices[colorList[newIndex].Index] = index;
			}

			Colors.SetColorIndices(m_bitmap, colorIndices);

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RestrictSelectedPaletteToRGB332()
		{
			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = Convert.ColorFromRGB332(Convert.ColorToRGB332(Palette[i].Color));
				}
			}

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RestrictSelectedPaletteToRGB333()
		{
			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = Convert.ColorFromRGB333(Convert.ColorToRGB333(Palette[i].Color));
				}
			}

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RestrictSelectedPaletteToRGB444()
		{
			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = Convert.ColorFromRGB444(Convert.ColorToRGB444(Palette[i].Color));
				}
			}

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RestrictSelectedPaletteToRGB555()
		{
			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = Convert.ColorFromRGB555(Convert.ColorToRGB555(Palette[i].Color));
				}
			}

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void RestrictSelectedPaletteToRGB565()
		{
			for (int i = 0; i < Palette.Length; i++)
			{
				if (Palette[i].IsSelected)
				{
					Palette[i].Color = Convert.ColorFromRGB565(Convert.ColorToRGB565(Palette[i].Color));
				}
			}

			DrawPalette();

			OnPaletteSelect?.Invoke(this, new ColorEventArgs());
		}

		public void ShowColorPicker()
        {
            using (ColorDialogEx colDiag = new ColorDialogEx())
            {
                colDiag.Color = (Selected != null ? Selected.Color : Color.Black);

                if (colDiag.ShowDialog(this) == DialogResult.OK)
                    SetAllSelectedColor(colDiag.Color);
            }
        }

        private void PaletteControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Low;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;

            e.Graphics.Clear(this.BackColor);

            DrawColors(e.Graphics);
            DrawGrid(e.Graphics);
            DrawSelection(e.Graphics);
        }

        private void PaletteControl_Resize(object sender, EventArgs e)
        {
            ResizePalette();
            RefreshPalette();
            DrawPalette();
        }

        private void PaletteControl_KeyDown(object sender, KeyEventArgs e)
        {
            m_modifierKeys = e.Modifiers;
        }

        private void PaletteControl_KeyUp(object sender, KeyEventArgs e)
        {
            m_modifierKeys = e.Modifiers;
        }

        private void PaletteControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            this.Focus();

            bool shiftDown = (m_modifierKeys & Keys.Shift) != 0;

            if (!shiftDown)
                m_startPoint = new Point(e.X, e.Y);

            m_endPoint = new Point(e.X, e.Y);
            m_mouseDown = true;

            UpdatePaletteSelection();
        }

        private void PaletteControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            m_endPoint = new Point(e.X, e.Y);

            if (m_mouseDown)
                UpdatePaletteSelection();
        }

        private void PaletteControl_MouseUp(object sender, MouseEventArgs e)
        {
            m_lastButtonUp = e.Button;

            if (e.Button != MouseButtons.Left)
                return;

            m_endPoint = new Point(e.X, e.Y);
            m_mouseDown = false;

            OnResetControl?.Invoke();

            if (m_eyeDropper)
            {
                if (Selected != null)
                {
                    ClearTransparency();

                    Selected.Alpha = (Selected.Alpha == 255 ? 0 : 255);

                    DrawPalette();

                    OnPaletteSelect?.Invoke(this, new ColorEventArgs());
                }
            }
            else
                UpdatePaletteSelection();
        }

        private void PaletteControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_lastButtonUp != MouseButtons.Left)
                return;

            if (Selected == null)
                return;

            using (ColorDialogEx colDiag = new ColorDialogEx())
            {
                colDiag.Color = Selected.Color;
                //colDiag.FullOpen = true;

                if (colDiag.ShowDialog(this) == DialogResult.OK)
                {
                    Selected.Color = colDiag.Color;

                    DrawPalette();

                    OnPaletteSelect?.Invoke(this, new ColorEventArgs());
                }
            }
        }

        public bool AllowMultipleSelection
        {
            get { return m_allowMultipleSelection; }
            set { m_allowMultipleSelection = value; }
        }

        public Bitmap Bitmap
        {
            get { return m_bitmap; }
            set { m_bitmap = value; }
        }

        public Point Offset { get; set; } = new Point(4, 4);

        public PalNode[] Palette { get; private set; } = null;

        public Size SwatchSize { get { int swatchWidth = (this.ClientSize.Width - (Offset.X * 2)) / Columns; return SnapToGrid(new SizeF(swatchWidth, swatchWidth), 2f); } }

        public int PalUsed { get; set; } = 256;

        public int Columns { get; set; } = 16;

        public int Rows { get { return (Palette == null ? 0 : (int)Math.Ceiling((float)Palette.Length / Columns)); } }

        public int SelectedIndex
        {
            get { return (Selected == null ? -1 : Array.IndexOf(Palette, Selected)); }
            set { SetSelectedIndex(value); }
        }

        public PalNode Selected { get; private set; } = null;

        public Color SelectedColor
        {
            get { return (Selected == null ? Color.Empty : Selected.Color); }
            set { if (Selected != null) { Selected.Color = value; DrawPalette(); } }
        }

        public int[] SelectedIndices
        {
            get
            {
                List<int> selectedIndices = new List<int>();

                for (int i = 0; i < Palette.Length; i++)
                {
                    if (!Palette[i].IsSelected)
                        continue;

                    selectedIndices.Add(i);
                }

                return selectedIndices.ToArray();
            }
        }

        public bool EyeDropper
        {
            set { SetEyeDropper(value); }
        }

        public Color[] ColorArray
        {
            get
            {
                Color[] palette = new Color[Palette.Length];

                for (int i = 0; i < palette.Length; i++)
                    palette[i] = Palette[i].Color;

                return palette;
            }
            set
            {
                for (int i = 0; i < Palette.Length && i < value.Length; i++)
                    Palette[i].Color = value[i];
            }
        }
    }

    public class ColorEventArgs : EventArgs
    {
        public int Index;
        public Color Color;

        public ColorEventArgs() :
            base()
        {
            Index = -1;
            Color = Color.Empty;
        }

        public ColorEventArgs(int index, Color color) :
            base()
        {
            Index = index;
            Color = color;
        }
    }
}

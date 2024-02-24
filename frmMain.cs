using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Globalization;

using FreeImageAPI;

using ControlsEx.ColorManagement.Gradients;
using System.Drawing.Drawing2D;

namespace PalEdit
{
    public partial class frmMain : Form
    {
        private string m_fileName = null;
		private int m_swatchesPaletteIndex = 0;
        private string m_swatchesPaletteFileName = null;
        private frmBitmap m_frmBitmap = null;

        private char[] m_decimalChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace("[VERSION]", Globals.Version);

            /* if (!FreeImage.IsAvailable())
            {
                MessageBox.Show("FreeImage.dll seems to be missing. Aborting.");
                this.Close();
            } */

            /* string paletteFileName = Path.Combine(Application.StartupPath, "Level000.nxp");
            string tilesFileName = Path.Combine(Application.StartupPath, "Level000.nxt");
            string outputFileName = Path.Combine(Application.StartupPath, "Level000.png");

            Colors.WriteNextTiles(tilesFileName, paletteFileName, outputFileName, 8, 8, false);

            return; */

            Colors.LoadPalettes(mnuPalette, tsddbSwatchesPalette, OnPalette_Click, OnSwatchesPalette_Click);
            LoadDescriptionColors();
            //string workingPath = Path.Combine(Application.StartupPath, "Batch3");
            //Colors.BatchProcessIniFile(Path.Combine(workingPath, "SpriteSheet.ini"));

            ReadConfig(Globals.FileNames.Ini);

            palControl.OnPaletteSelect += OnPaletteSelect;
            palControl.OnResetControl += OnResetControl;
            palControl.OnUpdateBitmap += OnUpdateBitmap;
            palControl.OnCloseBitmap += OnCloseBitmap;
            palControl.OnSetBitmap += OnSetBitmap;

            palSwatches.AllowMultipleSelection = false;
            palSwatches.OnPaletteSelect += OnSwatchPaletteSelect;

            picMagnify.Image = new Bitmap(picMagnify.Width, picMagnify.Height);

            ResetSlidersNoEvent();
		}

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteConfig(Globals.FileNames.Ini);
        }

        private void ReadConfig(string fileName)
        {
            FileIO.ReadFormState(fileName, this);

            using (IniFile iniFile = new IniFile(fileName))
            {
                // General
                //m_fileName = iniFile.Read("General", "FileName", null);

                // Gradient
                chkStartAndEndOnly.Checked = iniFile.Read<bool>("Gradient", "StartAndEndOnly", true);

                // Swatches
                tsbSwatchesLock.Checked = iniFile.Read<bool>("Swatches", "Lock", false);
                tsbSortColors.Checked = iniFile.Read<bool>("Swatches", "SortColors", false);
                m_swatchesPaletteIndex = iniFile.Read<int>("Swatches", "Palette", 0);
                m_swatchesPaletteFileName = iniFile.Read("Swatches", "PaletteFileName");
            }

            //LoadPalette(m_fileName);

            if (IsCustomSwatchesPalette())
            {
                LoadSwatchesPalette(m_swatchesPaletteFileName);
            }
            else
            {
                palSwatches.SetColorPalette(GetSwatchesPalette(), true);
            }
        }

        private void WriteConfig(string fileName)
        {
            FileIO.WriteFormState(fileName, this);

            using (IniFile iniFile = new IniFile(fileName))
            {
                // General
                //iniFile.Write("General", "FileName", m_fileName);

                // Gradient
                iniFile.Write<bool>("Gradient", "StartAndEndOnly", chkStartAndEndOnly.Checked);

                // Swatches
                iniFile.Write<bool>("Swatches", "Lock", tsbSwatchesLock.Checked);
                iniFile.Write<bool>("Swatches", "SortColors", tsbSortColors.Checked);
                iniFile.Write<int>("Swatches", "Palette", m_swatchesPaletteIndex);
                iniFile.Write("Swatches", "PaletteFileName", m_swatchesPaletteFileName);
            }
        }

        private void LoadDescriptionColors()
        {
            string fileName = Path.Combine(Application.StartupPath, "Colors.ini");
            string[] lineArray = File.ReadAllLines(fileName);
            List<Color> colorList = new List<Color>();
            List<string> nameList = new List<string>();

            if (!File.Exists(fileName))
                return;

            foreach(string line in lineArray)
            {
                if (line.StartsWith("["))
                    continue;

                string[] lineSplit = line.Split(new char[] { '=' }, StringSplitOptions.None);

                if (lineSplit.Length != 2)
                    continue;

                string colorString = lineSplit[0];
                string nameString = lineSplit[1];

                Int32 colorInteger = 0;

                if (!Int32.TryParse(colorString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out colorInteger))
                    continue;

                colorList.Add(Color.FromArgb(colorInteger));
                nameList.Add(nameString);

                //Console.WriteLine("{0}={1}", nameString, color.ToString());
            }

            Colors.DescriptionColorArray = colorList.ToArray();
            Colors.DescriptionTextArray = nameList.ToArray();
        }

        private void OnSwatchPaletteSelect(object sender, ColorEventArgs e)
        {
            //palSwatches.SelectNone();
            //palSwatches.DrawPalette();

            if (e.Index != -1)
                palControl.SetAllSelectedColor(e.Color);
        }

        /* private void LoadPalettes()
        {
            string palettesPath = Path.Combine(Application.StartupPath, "Palettes");
            string[] fileArray = Directory.GetFiles(palettesPath, "*.png");

            foreach (string fileName in fileArray)
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                Color[] colorPalette = null;

                using (Bitmap palBitmap = (Bitmap)Bitmap.FromFile(fileName))
                {
                    colorPalette = Colors.CreateColorPalette(palBitmap);
                    // PalFile palFile = new PalFile();
                    // palFile.OpenPaletteFile(fileName);
                    // colorPalette = palFile.Palette;

                    Console.WriteLine("\n\npublic static Color[] {0}Palette =", name);
                    Console.Write("{\n\t");

                    for (int i = 0; i < colorPalette.Length; i++)
                    {
                        Console.Write("Color.FromArgb(0x{0:X6})", colorPalette[i].ToArgb() & 0xFFFFFF);

                        if (i < colorPalette.Length - 1)
                        {
                            if ((i + 1) % 16 == 0)
                                Console.Write("\n\t");
                            else
                                Console.Write(", ");
                        }
                    }

                    Console.Write("\n};");
                }
            }
        } */

        private void LoadPalette(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            Colors.PaletteType paletteType = Colors.GetPaletteType(fileName);

            if (paletteType == Colors.PaletteType.PaletteFile)
            {
                m_fileName = fileName;
                toolStripStatusLabel1.Text = Path.GetFileName(m_fileName);
                palControl.OpenPaletteFile(m_fileName);
                ApplySwatchesPalette();
            }
            else if (paletteType == Colors.PaletteType.BitmapFile)
            {
                m_fileName = fileName;
                toolStripStatusLabel1.Text = Path.GetFileName(m_fileName);
                palControl.OpenBitmapFile(m_fileName);
                ApplySwatchesPalette();
            }
        }

		private bool IsCustomSwatchesPalette()
		{
			return (m_swatchesPaletteIndex == 0);
		}

        private void LoadSwatchesPalette(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            Colors.PaletteType paletteType = Colors.GetPaletteType(fileName);

            if (paletteType == Colors.PaletteType.PaletteFile)
            {
                m_swatchesPaletteFileName = fileName;
                palSwatches.OpenPaletteFile(m_swatchesPaletteFileName);
                Colors.PaletteArray[0] = palSwatches.ColorArray;
                m_swatchesPaletteIndex = 0;
            }
            else if (paletteType == Colors.PaletteType.BitmapFile)
            {
                m_swatchesPaletteFileName = fileName;
                palSwatches.OpenBitmapFile(m_swatchesPaletteFileName);
                Colors.PaletteArray[0] = palSwatches.ColorArray;
				m_swatchesPaletteIndex = 0;
            }
        }

        private void OnPaletteSelect(object sender, ColorEventArgs e)
        {
            UpdatePalette();
        }

        private void UpdatePalette()
        {
            if (palControl.SelectedIndex == -1)
                return;

            ApplySwatchesPalette();

            Color selectedColor = palControl.SelectedColor;
            int selectedIndex = palControl.SelectedIndex;
            int colorIndex = 0;
            float leastDistance = 0.0f;
            Color nearestColor = Colors.GetNearestColorSqrt(selectedColor, Colors.DescriptionColorArray, out colorIndex, out leastDistance);
            string colorName = Colors.DescriptionTextArray[colorIndex];

            txtR.Text = String.Format("{0}", selectedColor.R);
            txtG.Text = String.Format("{0}", selectedColor.G);
            txtB.Text = String.Format("{0}", selectedColor.B);
            ControlsEx.ColorManagement.ColorModels.HSV hsv = ControlsEx.ColorManagement.ColorModels.HSV.FromRGB(selectedColor);
            txtH.Text = String.Format("{0}", (int)Math.Round(hsv.H * 360));
            txtS.Text = String.Format("{0}", (int)Math.Round(hsv.S * 100));
            txtV.Text = String.Format("{0}", (int)Math.Round(hsv.V * 100));
            txtDescription.Text = colorName;
            txtWeb.Text = String.Format("#{0:X6}", selectedColor.ToArgb() & 0xFFFFFF);
            txtRGB.Text = String.Format("{0}, {1}, {2}", selectedColor.R, selectedColor.G, selectedColor.B);
            txtRGBNormalized.Text = String.Format("{0:0.###}, {1:0.###}, {2:0.###}", selectedColor.R / 255f, selectedColor.G / 255f, selectedColor.B / 255f);
            toolStripStatusLabel2.Text = String.Format("Index {0}, {1} Selected.", selectedIndex, palControl.SelectedIndices.Length);

            picComplement.BackColor = Colors.GetComplement(selectedColor);
            picSplitComplement0.BackColor = Colors.GetSplitComplement0(selectedColor);
            picSplitComplement1.BackColor = Colors.GetSplitComplement1(selectedColor);
            picTriadic0.BackColor = Colors.GetTriadic0(selectedColor);
            picTriadic1.BackColor = Colors.GetTriadic1(selectedColor);
            picTetradic0.BackColor = Colors.GetTetradic0(selectedColor);
            picTetradic1.BackColor = Colors.GetTetradic1(selectedColor);
            picTetradic2.BackColor = Colors.GetTetradic2(selectedColor);
            picAnalagous0.BackColor = Colors.GetAnalagous0(selectedColor);
            picAnalagous1.BackColor = Colors.GetAnalagous1(selectedColor);

            SetMagnifyColor(selectedColor);
            pbGradient.Invalidate();
        }

        private void OnResetControl()
        {
            ResetSlidersNoEvent();
        }

        private void OnUpdateBitmap(Color transparentColor)
        {
            if (m_frmBitmap != null)
            {
                m_frmBitmap.UpdateBitmap();
                m_frmBitmap.SetBackColor(transparentColor);
            }
        }

        private void OnCloseBitmap()
        {
            if (m_frmBitmap != null)
                m_frmBitmap.CloseBitmap();
        }

        private void OnSetBitmap(Bitmap bitmap)
        {
            if (m_frmBitmap != null)
                m_frmBitmap.SetBitmap(bitmap);
        }

        private void SetMagnifyColor(Color color)
        {
            using (Graphics g = Graphics.FromImage(picMagnify.Image))
                g.Clear(color);

            picMagnify.Invalidate();
        }

        private void WriteX1B5G5R5c(string fileName)
        {
            List<string> fileOutput = new List<string>();

            string labelName = Path.GetFileNameWithoutExtension(fileName);

            fileOutput.Add(String.Format("// Generated by PalEdit {0} By Ben Baker", Globals.Version));
            fileOutput.Add("");
            //fileOutput.Add(String.Format("#define {0}Len \t{1}", labelName, palControl.PalCount * 2));
            //fileOutput.Add("");
            fileOutput.Add(String.Format("ushort[] {0} = ", labelName) + "{");

            for (int y = 0; y < 32; y++)
            {
                string lineOutput = "\t";

                for (int x = 0; x < 8; x++)
                {
                    lineOutput += String.Format("0x{0:x4}", Convert.ColorToX1B5G5R5(palControl.Palette[x + y * 8].Color));

                    if (x + y * 8 != 255)
                        lineOutput += ",";

                    if (y % 8 == 0)
                        lineOutput += "";
                }

                fileOutput.Add(lineOutput);
            }

            fileOutput.Add("};");

            File.WriteAllLines(fileName, fileOutput.ToArray());
        }

        private void WriteX1B5G5R5s(string fileName)
        {
            List<string> fileOutput = new List<string>();

            string labelName = Path.GetFileNameWithoutExtension(fileName);

            fileOutput.Add(String.Format("@ Generated by PalEdit {0} By Ben Baker", Globals.Version));
            fileOutput.Add("");
            //fileOutput.Add(String.Format("#define {0}Len \t{1}", labelName, palControl.PalCount * 2));
            //fileOutput.Add("");
            fileOutput.Add(String.Format("\t.global {0}", labelName));
            fileOutput.Add(String.Format("{0}:", labelName));

            for (int y = 0; y < 32; y++)
            {
                string lineOutput = "\t.hword ";

                for (int x = 0; x < 8; x++)
                {
                    lineOutput += String.Format("0x{0:x4}", Convert.ColorToX1B5G5R5(palControl.Palette[x + y * 8].Color));

                    if (x < 7)
                        lineOutput += ",";

                    if (y % 8 == 0)
                        lineOutput += "";
                }

                fileOutput.Add(lineOutput);
            }

            File.WriteAllLines(fileName, fileOutput.ToArray());
        }

        private void WriteRGB333nxp(string fileName)
        {
            Byte[] colorPalette;

            Colors.CreateNextPalette(palControl.ColorArray, Colors.ColorMode.Distance, out colorPalette);

            File.WriteAllBytes(fileName, colorPalette);
        }

        private void WriteRGB333asmZ80ASM(string fileName)
        {
            Byte[] colorPalette;

            Colors.CreateNextPalette(palControl.ColorArray, Colors.ColorMode.Distance, out colorPalette);

            List<string> fileOutput = new List<string>();

            string labelName = Path.GetFileNameWithoutExtension(fileName);

            fileOutput.Add(String.Format("; Generated by PalEdit {0} By Ben Baker", Globals.Version));
            fileOutput.Add("");
            fileOutput.Add(String.Format("{0}:", labelName));

            string lineOutput = String.Empty;

            for (int i = 0; i < colorPalette.Length; i++)
            {
                int x = i % 16;
                int y = i / 16;

                if (x == 0)
                {
                    if (y > 0)
                        fileOutput.Add(lineOutput);

                    lineOutput = "\tDEFB ";
                }

                lineOutput += String.Format("${0:x2}", colorPalette[i]);

                if (x < 15)
                    lineOutput += ",";
            }

            File.WriteAllLines(fileName, fileOutput.ToArray());
        }

        private void WriteRGB333asmSjASM(string fileName)
        {
            Byte[] colorPalette;

            Colors.CreateNextPalette(palControl.ColorArray, Colors.ColorMode.Distance, out colorPalette);

            List<string> fileOutput = new List<string>();

            string labelName = Path.GetFileNameWithoutExtension(fileName);

            fileOutput.Add(String.Format("; Generated by PalEdit {0} By Ben Baker", Globals.Version));
            fileOutput.Add("");
            fileOutput.Add(String.Format(".{0}", labelName));

            string lineOutput = String.Empty;

            for (int i = 0; i < colorPalette.Length; i++)
            {
                int x = i % 16;
                int y = i / 16;

                if (x == 0)
                {
                    if (y > 0)
                        fileOutput.Add(lineOutput);

                    lineOutput = "\tdb ";
                }
                
                lineOutput += String.Format("${0:x2}", colorPalette[i]);

                if (x < 15)
                    lineOutput += ",";          
            }

            File.WriteAllLines(fileName, fileOutput.ToArray());
        }

        private void ResetSlidersNoEvent()
        {
            trkHue.Scroll -= trkHue_Scroll;
            trkSaturation.Scroll -= trkSaturation_Scroll;
            trkBrightness.Scroll -= trkBrightness_Scroll;
            trkTint.Scroll -= trkTint_Scroll;

            txtHue.TextChanged -= txtHue_TextChanged;
            txtSaturation.TextChanged -= txtSaturation_TextChanged;
            txtBrightness.TextChanged -= txtBrightness_TextChanged;
            txtTint.TextChanged -= txtTint_TextChanged;

            trkHue.Value = 50;
            trkSaturation.Value = 50;
            trkBrightness.Value = 50;
            trkTint.Value = 0;
            txtHue.Text = trkHue.Value.ToString();
            txtSaturation.Text = trkSaturation.Value.ToString();
            txtBrightness.Text = trkBrightness.Value.ToString();
            txtTint.Text = trkTint.Value.ToString();

            trkHue.Scroll += trkHue_Scroll;
            trkSaturation.Scroll += trkSaturation_Scroll;
            trkBrightness.Scroll += trkBrightness_Scroll;
            trkTint.Scroll += trkTint_Scroll;

            txtHue.TextChanged += txtHue_TextChanged;
            txtSaturation.TextChanged += txtSaturation_TextChanged;
            txtBrightness.TextChanged += txtBrightness_TextChanged;
            txtTint.TextChanged += txtTint_TextChanged;
        }

        private void ResetSliders()
        {
            trkHue.Value = 50;
            trkSaturation.Value = 50;
            trkBrightness.Value = 50;
            trkTint.Value = 0;
            txtHue.Text = trkHue.Value.ToString();
            txtSaturation.Text = trkSaturation.Value.ToString();
            txtBrightness.Text = trkBrightness.Value.ToString();
            txtTint.Text = trkTint.Value.ToString();

            palControl.SetHue(trkHue.Value, trkHue.Maximum);
            palControl.SetSaturation(trkSaturation.Value, trkSaturation.Maximum);
            palControl.SetBrightness(trkBrightness.Value, trkBrightness.Maximum);
            palControl.SetTint(trkTint.Value, trkTint.Maximum);
        }

        private void Quantize()
        {
            if (palControl.Bitmap == null)
                return;

            int[] colorIndices;

            Colors.GetColorIndices(palControl.Bitmap, out colorIndices);

            using (frmQuantize frmQuantize = new frmQuantize(colorIndices.Length))
            {
                if (frmQuantize.ShowDialog(this) == DialogResult.OK)
                {
                    palControl.QuantizeBitmap(frmQuantize.ColorOffset, frmQuantize.ColorCount);
                }
            }
        }

        private bool IsShiftDown()
        {
            return (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
        }

        private void trkHue_Scroll(object sender, EventArgs e)
        {
            palControl.SetHue(trkHue.Value, trkHue.Maximum);
            txtHue.Text = trkHue.Value.ToString();
        }

        private void trkSaturation_Scroll(object sender, EventArgs e)
        {
            palControl.SetSaturation(trkSaturation.Value, trkSaturation.Maximum);
            txtSaturation.Text = trkSaturation.Value.ToString();
        }

        private void trkBrightness_Scroll(object sender, EventArgs e)
        {
            palControl.SetBrightness(trkBrightness.Value, trkBrightness.Maximum);
            txtBrightness.Text = trkBrightness.Value.ToString();
        }

        private void trkTint_Scroll(object sender, EventArgs e)
        {
            palControl.SetTint(trkTint.Value, trkTint.Maximum);
            txtTint.Text = trkTint.Value.ToString();
        }

        private void txtHue_TextChanged(object sender, EventArgs e)
        {
            int value = 0;

            if (Int32.TryParse(txtHue.Text, out value))
            {
                trkHue.Value = Math.Min(100, value);
                palControl.SetHue(trkHue.Value, trkHue.Maximum);
            }
        }

        private void txtSaturation_TextChanged(object sender, EventArgs e)
        {
            int value = 0;

            if (Int32.TryParse(txtSaturation.Text, out value))
            {
                trkSaturation.Value = Math.Min(100, value);
                palControl.SetSaturation(trkSaturation.Value, trkSaturation.Maximum);
            }
        }

        private void txtBrightness_TextChanged(object sender, EventArgs e)
        {
            int value = 0;

            if (Int32.TryParse(txtBrightness.Text, out value))
            {
                trkBrightness.Value = Math.Min(100, value);
                palControl.SetBrightness(trkBrightness.Value, trkBrightness.Maximum);
            }
        }

        private void txtTint_TextChanged(object sender, EventArgs e)
        {
            int value = 0;

            if (Int32.TryParse(txtTint.Text, out value))
            {
                trkTint.Value = Math.Min(100, value);
                palControl.SetTint(trkTint.Value, trkTint.Maximum);
            }
        }

        private void chkEyeDropper_CheckedChanged(object sender, EventArgs e)
        {
            palControl.EyeDropper = chkEyeDropper.Checked;
        }

        private void butReset_Click(object sender, EventArgs e)
        {
            ResetSliders();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            using (frmNew frmNew = new frmNew())
            {
                if (frmNew.ShowDialog(this) == DialogResult.OK)
                {
                    if (frmNew.CreateBitmap)
                    {
                        Bitmap bitmap = new Bitmap(frmNew.BitmapWidth, frmNew.BitmapHeight, frmNew.Format4Bpp ? PixelFormat.Format4bppIndexed : PixelFormat.Format8bppIndexed);
                        ColorPalette colorPalette = bitmap.Palette;

                        for (int i = 0; i < colorPalette.Entries.Length; i++)
                            colorPalette.Entries[i] = Color.Black;

                        bitmap.Palette = colorPalette;

                        palControl.SetBitmap(bitmap);
                    }
                    else
                    {
                        palControl.NewPalette(256);
                    }
                }
            }
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string fileName = null;

            Colors.PaletteType paletteType = Colors.TryOpenPalette(this, ref fileName);

            LoadPalette(fileName);
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (FileIO.TrySaveFile(this, null, m_fileName, "Palette Files", new string[] { ".act", ".pal", ".gpl", ".txt", ".bmp", ".gif", ".png", ".jpg", ".pcx", ".tif" }, out m_fileName))
            {
                string fileExtension = Path.GetExtension(m_fileName);
                toolStripStatusLabel1.Text = Path.GetFileName(m_fileName);

                switch (fileExtension)
                {
                    case ".act":
                        palControl.SavePaletteFile(m_fileName, PaletteFormat.Act);
                        break;
                    case ".pal":
                        //palControl.SavePaletteFile(m_fileName, PaletteFormat.MSPal);
                        palControl.SavePaletteFile(m_fileName, PaletteFormat.JASC);
                        break;
                    case ".gpl":
                        palControl.SavePaletteFile(m_fileName, PaletteFormat.GIMP);
                        break;
                    case ".txt":
                        palControl.SavePaletteFile(m_fileName, PaletteFormat.PaintNET);
                        break;
                    case ".bmp":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_BMP);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Bmp);
                        break;
                    case ".gif":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_GIF);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Gif);
                        break;
                    case ".png":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_PNG);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Png);
                        break;
                    case ".jpg":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_JPEG);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Jpeg);
                        break;
                    case ".pcx":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_PCX);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Pcx);
                        break;
                    case ".tif":
                        palControl.SaveBitmapFile(m_fileName, FREE_IMAGE_FORMAT.FIF_TIFF);
                        //palControl.SaveBitmapFile(m_fileName, ImageFormat.Tiff);
                        break;
                }
            }
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TryOpenFile(this, null, null, "Palette Files", new string[] { ".act", ".pal", ".gpl", ".txt", ".bmp", ".gif", ".png", ".jpg", ".pcx", ".tif"  }, out fileName))
            {
                string fileExtension = Path.GetExtension(fileName);

                switch (fileExtension)
                {
                    case ".act":
                    case ".pal":
                    case ".gpl":
                    case ".txt":
                        using (frmImport frmImport = new frmImport())
                        {
                            frmImport.OpenPaletteFile(fileName);

                            if (frmImport.ShowDialog(this) == DialogResult.OK)
                            {
                                palControl.SetClipboardPalette(frmImport.Palette, false);
                                palControl.PastePalette();
                            }
                        }
                        break;
                    case ".bmp":
                    case ".gif":
                    case ".png":
                    case ".jpg":
                    case ".pcx":
                    case ".tif":
                        using (frmImport frmImport = new frmImport())
                        {
                            frmImport.OpenBitmapFile(fileName);

                            if (frmImport.ShowDialog(this) == DialogResult.OK)
                            {
                                palControl.SetClipboardPalette(frmImport.Palette, false);
                                palControl.PastePalette();
                            }
                        }
                        break;
                }
            }
        }

        private void mnuBatch_Click(object sender, EventArgs e)
        {
            using (frmBatch frmBatch = new frmBatch())
            {
                if (frmBatch.ShowDialog(this) == DialogResult.OK)
                {

                }
            }
        }

        private void mnuX1B5G5R5c_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TrySaveFile(this, null, null, "C Files", new string[] { ".c" }, out fileName))
                WriteX1B5G5R5c(fileName);
        }

        private void mnuX1B5G5R5s_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TrySaveFile(this, null, null, "S Files", new string[] { ".s" }, out fileName))
                WriteX1B5G5R5s(fileName);
        }

        private void mnuRGB333nxp_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TrySaveFile(this, null, null, "Nxp Files", new string[] { ".nxp" }, out fileName))
                WriteRGB333nxp(fileName);
        }

        private void mnuRGB333asmZ80ASM_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TrySaveFile(this, null, null, "Asm Files", new string[] { ".asm" }, out fileName))
                WriteRGB333asmZ80ASM(fileName);
        }

        private void mnuRGB333asmSjASM_Click(object sender, EventArgs e)
        {
            string fileName = null;

            if (FileIO.TrySaveFile(this, null, null, "Asm Files", new string[] { ".asm" }, out fileName))
                WriteRGB333asmSjASM(fileName);
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            palControl.CutPalette();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            palControl.CopyPalette();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            palControl.PastePalette();
        }

        private void mnuFill_Click(object sender, EventArgs e)
        {
            palControl.FillPalette();
        }

        private void mnuSwap_Click(object sender, EventArgs e)
        {
            palControl.SwapPalette();
        }

        private void mnuMerge_Click(object sender, EventArgs e)
        {
            palControl.MergePalette();
        }

        private void mnuGradient_Click(object sender, EventArgs e)
        {
            palControl.ShowGradientPicker(chkStartAndEndOnly.Checked);
        }

        private void chkStartAndEndOnly_CheckedChanged(object sender, EventArgs e)
        {
            pbGradient.Invalidate();
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            palControl.SelectAll();
        }

        private void mnuSelectNone_Click(object sender, EventArgs e)
        {
            palControl.SelectNone();
        }

        private void mnuSelectInverse_Click(object sender, EventArgs e)
        {
            palControl.SelectInverse();
        }

        private void mnuSelectUsedColors_Click(object sender, EventArgs e)
        {
            palControl.SelectUsedColors(!IsShiftDown());

            OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
        }

        private void mnuSelectMatchingColors_Click(object sender, EventArgs e)
        {
            palControl.SelectMatchingColors();

            OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
        }

        private void mnuColor_Click(object sender, EventArgs e)
        {
            palControl.ShowColorPicker();
        }
		
		private void mnuSortSqrt_Click(object sender, EventArgs e)
		{
			palControl.SortSelectedPalette(Colors.SortColorMode.Sqrt);
		}

		private void mnuSortHSL_Click(object sender, EventArgs e)
		{
			palControl.SortSelectedPalette(Colors.SortColorMode.HSB);
		}
		private void mnuSortLAB_Click(object sender, EventArgs e)
		{
			palControl.SortSelectedPalette(Colors.SortColorMode.Lab);
		}

		private void mnuRotateLeft_Click(object sender, EventArgs e)
		{
			palControl.RotateLeftSelectedPalette();
		}

		private void mnuRotateRight_Click(object sender, EventArgs e)
		{
			palControl.RotateRightSelectedPalette();
		}

		private void mnuReverse_Click(object sender, EventArgs e)
		{
			palControl.ReverseSelectedPalette();
		}

		private void mnuRGB332_Click(object sender, EventArgs e)
		{
			palControl.RestrictSelectedPaletteToRGB332();
		}

		private void mnuRGB333_Click(object sender, EventArgs e)
		{
			palControl.RestrictSelectedPaletteToRGB333();
		}

		private void mnuRGB444_Click(object sender, EventArgs e)
		{
			palControl.RestrictSelectedPaletteToRGB444();
		}

		private void mnuRGB555_Click(object sender, EventArgs e)
		{
			palControl.RestrictSelectedPaletteToRGB555();
		}

		private void mnuRGB565_Click(object sender, EventArgs e)
		{
			palControl.RestrictSelectedPaletteToRGB565();
		}

		private void mnuBitmap_Click(object sender, EventArgs e)
        {
            if (palControl.Bitmap != null && m_frmBitmap == null)
            {
                m_frmBitmap = new frmBitmap(palControl, picMagnify, palControl.GetTransparentColor(), trkZoom.Value);
                m_frmBitmap.OnPixelSelect += m_frmBitmap_OnPixelSelect;
                m_frmBitmap.OnRectangleSelect += m_frmBitmap_OnRectangleSelect;
                m_frmBitmap.Disposed += m_frmBitmap_Disposed;
                m_frmBitmap.Show();

                palControl.DrawPalette();
            }
        }

        private void m_frmBitmap_OnPixelSelect(int index)
        {
            Color color = palControl.SetSelectedIndex(index);

            OnPaletteSelect(this, new ColorEventArgs(index, color));
        }

        private void m_frmBitmap_OnRectangleSelect(Rectangle rectangleSelect)
        {
            palControl.SelectUsedColors(rectangleSelect, !IsShiftDown());

            OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
        }

        private void m_frmBitmap_Disposed(object sender, EventArgs e)
        {
            m_frmBitmap.OnPixelSelect -= new frmBitmap.PixelSelectDelegate(m_frmBitmap_OnPixelSelect);
            m_frmBitmap.OnRectangleSelect -= m_frmBitmap_OnRectangleSelect;
            m_frmBitmap.Disposed -= m_frmBitmap_Disposed;
            m_frmBitmap = null;
        }

		private void mnuDonate_Click(object sender, EventArgs e)
		{
			using (frmDonate frmDonate = new frmDonate())
				frmDonate.ShowDialog(this);
		}

		private void mnuAbout_Click(object sender, EventArgs e)
        {
            using (frmAbout frmAbout = new frmAbout())
                frmAbout.ShowDialog(this);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.X) // Cut
                palControl.CutPalette();
            if (e.Control && e.KeyCode == Keys.C) // Copy
                palControl.CopyPalette();
            if (e.Control && e.KeyCode == Keys.V) // Paste
                palControl.PastePalette();
            if (e.Control && e.KeyCode == Keys.F) // Fill
                palControl.FillPalette();
            if (e.Control && e.KeyCode == Keys.S) // Swap
                palControl.SwapPalette();
            if (e.Control && e.KeyCode == Keys.G) // Gradient
                palControl.ShowGradientPicker(chkStartAndEndOnly.Checked);
            if (e.Control && e.KeyCode == Keys.A) // Select All
                palControl.SelectAll();
            if (e.Control && e.KeyCode == Keys.N) // Select None
                palControl.SelectNone();
            if (e.Control && e.KeyCode == Keys.I) // Select Inverse
                palControl.SelectInverse();
            if (e.Control && e.KeyCode == Keys.U) // Select Used Colors
            {
                palControl.SelectUsedColors(!IsShiftDown());

                OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
            }
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            palControl.CutPalette();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            palControl.CopyPalette();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            palControl.PastePalette();
        }

        private void tsmiFill_Click(object sender, EventArgs e)
        {
            palControl.FillPalette();
        }

        private void tsmiSwap_Click(object sender, EventArgs e)
        {
            palControl.SwapPalette();
        }

        private void tsmiMerge_Click(object sender, EventArgs e)
        {
            palControl.MergePalette();
        }

        private void tsmiGradient_Click(object sender, EventArgs e)
        {
            palControl.ShowGradientPicker(chkStartAndEndOnly.Checked);
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            palControl.SelectAll();
        }

        private void tsmiSelectNone_Click(object sender, EventArgs e)
        {
            palControl.SelectNone();
        }

        private void tsmiSelectInverse_Click(object sender, EventArgs e)
        {
            palControl.SelectInverse();
        }

        private void tsmiSelectUsedColors_Click(object sender, EventArgs e)
        {
            palControl.SelectUsedColors(!IsShiftDown());

            OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
        }

        private void tsmiSelectMatchingColors_Click(object sender, EventArgs e)
        {
            palControl.SelectMatchingColors();

            OnPaletteSelect(this, new ColorEventArgs(palControl.SelectedIndex, palControl.SelectedColor));
        }

        private void mnuQuantize_Click(object sender, EventArgs e)
        {
            Quantize();
        }

        private void tsmiColor_Click(object sender, EventArgs e)
        {
            palControl.ShowColorPicker();
        }

        private void tsmiSort_Click(object sender, EventArgs e)
        {
            palControl.SortSelectedPalette(Colors.SortColorMode.Sqrt);
        }

        private void tsmiQuantize_Click(object sender, EventArgs e)
        {
            Quantize();
        }

        private void tsbLoadSwatches_Click(object sender, EventArgs e)
        {
            string fileName = null;

            Colors.TryOpenPalette(this, ref fileName);

            LoadSwatchesPalette(fileName);
        }

        private void OnPalette_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            string tag = (string)toolStripMenuItem.Tag;
            int result = 0;

            if (Int32.TryParse(tag, out result))
            {
                palControl.SetBitmapColorPalette(Colors.PaletteArray[result], Colors.NearestColorMode.Sqrt, true);
            }
        }

		private void OnSwatchesPalette_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			string tag = (string)toolStripMenuItem.Tag;

			if (Int32.TryParse(tag, out m_swatchesPaletteIndex))
			{
				palSwatches.SetColorPalette(GetSwatchesPalette(), true);
			}
		}

		private void trkZoom_Scroll(object sender, EventArgs e)
        {
            if (m_frmBitmap == null)
                return;

            m_frmBitmap.Zoom = trkZoom.Value;
        }

        private void OnRGBLostFocus(object sender, EventArgs e)
        {
            if (palControl.SelectedColor == null)
                return;

            TextBox textBox = (TextBox)sender;
            int value = 0;

            if (Int32.TryParse(textBox.Text, NumberStyles.Integer, null, out value))
            {
                switch(textBox.Name)
                {
                    case "txtR":
                        palControl.SetAllSelectedColor(Color.FromArgb(palControl.SelectedColor.A, Convert.Clamp(value, 0, 255), palControl.SelectedColor.G, palControl.SelectedColor.B));
                        break;
                    case "txtG":
                        palControl.SetAllSelectedColor(Color.FromArgb(palControl.SelectedColor.A, palControl.SelectedColor.R, Convert.Clamp(value, 0, 255), palControl.SelectedColor.B));
                        break;
                    case "txtB":
                        palControl.SetAllSelectedColor(Color.FromArgb(palControl.SelectedColor.A, palControl.SelectedColor.R, palControl.SelectedColor.G, Convert.Clamp(value, 0, 255)));
                        break;
                }
            }
        }

        private void OnHSVLostFocus(object sender, EventArgs e)
        {
            if (palControl.SelectedColor == null)
                return;

            TextBox textBox = (TextBox)sender;
            int value = 0;
            ControlsEx.ColorManagement.ColorModels.HSV hsv = ControlsEx.ColorManagement.ColorModels.HSV.FromRGB(palControl.SelectedColor);

            if (Int32.TryParse(textBox.Text, NumberStyles.Integer, null, out value))
            {
                switch (textBox.Name)
                {
                    case "txtH":
                        hsv.H = value / 360f;
                        palControl.SetAllSelectedColor(hsv.ToRGB());
                        break;
                    case "txtS":
                        hsv.S = value / 100f;
                        palControl.SetAllSelectedColor(hsv.ToRGB());
                        break;
                    case "txtV":
                        hsv.V = value / 100f;
                        palControl.SetAllSelectedColor(hsv.ToRGB());
                        break;
                }
            }
        }

        private void OnDecimalKeypress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (Char.IsLetterOrDigit(e.KeyChar) && Array.IndexOf(m_decimalChars, e.KeyChar) == -1)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                switch (textBox.Name)
                {
                    case "txtR":
                    case "txtG":
                    case "txtB":
                        OnRGBLostFocus(sender, EventArgs.Empty);
                        break;
                    case "txtH":
                    case "txtS":
                    case "txtV":
                        OnHSVLostFocus(sender, EventArgs.Empty);
                        break;
                }
            }
        }

        private void ApplySwatchesPalette()
        {
            if (tsbSwatchesLock.Checked)
            {
                palControl.SetBitmapColorPalette(GetSwatchesPalette(), Colors.NearestColorMode.Sqrt, false);
            }
        }

        private Color GetNearestSwatchColor(Color color)
        {
            return Colors.GetNearestColor(color, GetSwatchesPalette(), Colors.NearestColorMode.Sqrt);
        }

        private Color[] GetSwatchesPalette()
        {
            Color[] srcColorPalette = Colors.PaletteArray[m_swatchesPaletteIndex];

            if (!tsbSortColors.Checked)
                return srcColorPalette;

            Color[] dstColorPalette = new Color[srcColorPalette.Length];

            srcColorPalette.CopyTo(dstColorPalette, 0);

            Array.Sort(dstColorPalette, new ColorSorter());

            return dstColorPalette;
        }

		private void tsbSwatchesReload_Click(object sender, EventArgs e)
		{
			palSwatches.SetColorPalette(GetSwatchesPalette(), true);
		}

		private void tsbSwatchesLock_Click(object sender, EventArgs e)
        {
            ApplySwatchesPalette();
        }

        private void tsbSelectMatchingColors_Click(object sender, EventArgs e)
        {
            palControl.SelectMatchingColors(GetSwatchesPalette());
        }

        private void tsbSelectNonMatchingColors_Click(object sender, EventArgs e)
        {
            palControl.SelectNonMatchingColors(GetSwatchesPalette());
        }

        private void tsbSortColors_Click(object sender, EventArgs e)
        {
            palSwatches.SetColorPalette(GetSwatchesPalette(), true);
        }

        private void pbGradient_Click(object sender, EventArgs e)
        {
            palControl.ShowGradientPicker(chkStartAndEndOnly.Checked);
            pbGradient.Invalidate();
        }

        private void pbGradient_Paint(object sender, PaintEventArgs e)
        {
            Gradient gradient = null;

            int colorCount = palControl.TryGetGradient(chkStartAndEndOnly.Checked, out gradient);

            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(Math.Max(1, pbGradient.Width), 0), Color.Transparent, Color.Black))
            {
                linearGradientBrush.InterpolationColors = gradient.GetColorBlend();
                e.Graphics.FillRectangle(linearGradientBrush, pbGradient.ClientRectangle);
            }
        }

        private void picMagnify_Click(object sender, EventArgs e)
        {
            palControl.ShowColorPicker();
        }

        private void picScheme_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;

            palControl.SetAllSelectedColor(pictureBox.BackColor);
        }

        private bool TryGetFileName(out string fileName, DragEventArgs e)
        {
            fileName = null;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;

                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        fileName = ((string[])data)[0];

                        return true;
                    }
                }
            }

            return false;
        }

        private void palControl_DragEnter(object sender, DragEventArgs e)
        {
            string fileName = null;

            if (TryGetFileName(out fileName, e))
            {
                LoadPalette(fileName);
            }
        }
	}
}
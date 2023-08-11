using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FreeImageAPI;

using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers.XiaolinWu;

namespace PalEdit
{
    public partial class frmBatch : Form
    {
        private PalFile m_palFile = null;
        private Color[] m_customPalette = null;
        private string m_customPaletteFileName = null;

        public frmBatch()
        {
            InitializeComponent();
        }

        private void frmBatch_Load(object sender, EventArgs e)
        {
            m_palFile = new PalFile();

            cboPalette.Items.AddRange(Colors.PaletteNames);
            cboPalette.SelectedIndex = 0;

            ReadConfig(Globals.FileNames.Ini);
        }

        private void frmBatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteConfig(Globals.FileNames.Ini);
        }

        private void butGO_Click(object sender, EventArgs e)
        {
            BatchSettings settings = new BatchSettings();
            settings.SourceDirectory = txtSource.Text;
            settings.DestinationDirectory = txtDestination.Text;
            settings.OutputSize = new Size((int)nudWidth.Value, (int)nudHeight.Value);
            settings.ColorPalette = GetColorPalette();
            settings.TransparentIndex = (int)nudTransparentIndex.Value;
            settings.SwapMagentaWithTransparentIndex = chkSwapMagentaWithTransparentIndex.Checked;
            settings.SortSizes = chkSortSizes.Checked;
            settings.SortColors = chkSortColors.Checked;
            settings.Quantize = chkQuantize.Checked;
            settings.AddPaletteOffset = chkAddPaletteOffset.Checked;
            settings.CreateCombinedImage = chkCreateCombinedImage.Checked;

            Colors.BatchProcessFolder(this, settings);

            this.DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void butBrowseSource_Click(object sender, EventArgs e)
        {
            string folder = null;

            if (!FileIO.TryOpenFolder(this, txtSource.Text, out folder))
                return;

            txtSource.Text = folder;
        }

        private void butBrowseDestination_Click(object sender, EventArgs e)
        {
            string folder = null;

            if (!FileIO.TryOpenFolder(this, txtSource.Text, out folder))
                return;

            txtDestination.Text = folder;
        }

        private void butBrowsePalette_Click(object sender, EventArgs e)
        {
            string fileName = null;

            Colors.PaletteType paletteType = Colors.TryOpenPalette(this, ref fileName);

            LoadCustomPalette(fileName);
        }

        private void LoadCustomPalette(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            Colors.PaletteType paletteType = Colors.GetPaletteType(fileName);

            if (paletteType == Colors.PaletteType.PaletteFile)
            {
                m_customPaletteFileName = fileName;

                Color[] colorPalette = null;

                if (PalFile.TryReadPalette(m_customPaletteFileName, out colorPalette))
                {
                    m_customPalette = colorPalette;
                    cboPalette.SelectedIndex = 0;
                }
            }
            else if (paletteType == Colors.PaletteType.BitmapFile)
            {
                m_customPaletteFileName = fileName;

                Bitmap bitmap = null;

                if (TryReadBitmapFile(m_customPaletteFileName, out bitmap))
                {
                    m_customPalette = new Color[bitmap.Palette.Entries.Length];
                    for (int i = 0; i < bitmap.Palette.Entries.Length; i++)
                        m_customPalette[i] = bitmap.Palette.Entries[i];
                    cboPalette.SelectedIndex = 0;
                }
            }
        }

        private void ReadConfig(string fileName)
        {
            using (IniFile iniFile = new IniFile(fileName))
            {
                txtSource.Text = iniFile.Read("Batch", "SourceDirectory", String.Empty);
                txtDestination.Text = iniFile.Read("Batch", "DestinationDirectory", String.Empty);
                Size outputSize = iniFile.Read<Size>("Batch", "OutputSize", new Size(640, 480));
                nudWidth.Value = outputSize.Width;
                nudHeight.Value = outputSize.Height;
                m_customPaletteFileName = iniFile.Read("Batch", "CustomPaletteFileName");
                cboPalette.SelectedItem = iniFile.Read("Batch", "ColorPalette");
                chkSwapMagentaWithTransparentIndex.Checked = iniFile.Read<bool>("Batch", "MakeMagentaIndex0", true);
                chkSortSizes.Checked = iniFile.Read<bool>("Batch", "SortSizes", true);
                chkSortColors.Checked = iniFile.Read<bool>("Batch", "SortColors", true);
                chkQuantize.Checked = iniFile.Read<bool>("Batch", "Quantize", true);
                chkAddPaletteOffset.Checked = iniFile.Read<bool>("Batch", "AddPaletteOffset", false);
                chkCreateCombinedImage.Checked = iniFile.Read<bool>("Batch", "CreateCombinedImage", true);
            }

            LoadCustomPalette(m_customPaletteFileName);
        }

        private void WriteConfig(string fileName)
        {
            using (IniFile iniFile = new IniFile(fileName))
            {
                iniFile.Write("Batch", "SourceDirectory", txtSource.Text);
                iniFile.Write("Batch", "DestinationDirectory", txtDestination.Text);
                iniFile.Write<Size>("Batch", "OutputSize", new Size((int)nudWidth.Value, (int)nudHeight.Value));
                m_customPaletteFileName = iniFile.Read("Batch", "CustomPaletteFileName");
                iniFile.Write("Batch", "ColorPalette", cboPalette.SelectedItem);
                iniFile.Write<bool>("Batch", "MakeMagentaIndex0", chkSwapMagentaWithTransparentIndex.Checked);
                iniFile.Write<bool>("Batch", "SortSizes", chkSortSizes.Checked);
                iniFile.Write<bool>("Batch", "SortColors", chkSortColors.Checked);
                iniFile.Write<bool>("Batch", "Quantize", chkQuantize.Checked);
                iniFile.Write<bool>("Batch", "AddPaletteOffset", chkAddPaletteOffset.Checked);
                iniFile.Write<bool>("Batch", "CreateCombinedImage", chkCreateCombinedImage.Checked);
            }
        }

        private bool TryReadBitmapFile(string fileName, out Bitmap bitmap)
        {
            bitmap = null;

            try
            {
                FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_UNKNOWN;

                bitmap = FreeImage.LoadBitmap(fileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref imageFormat);

                return true;
            }
            catch
            {
            }

            if (bitmap == null)
            {
                try
                {
                    if (FileIO.TryLoadImage(fileName, out bitmap))
                        return true;
                }
                catch
                {
                }
            }

            if (bitmap.PixelFormat != PixelFormat.Format4bppIndexed && bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                try
                {
                    bitmap = (Bitmap)ImageBuffer.QuantizeImage(bitmap, new WuColorQuantizer(), null, 256, 1);

                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private bool IsCustomPalette()
        {
            return (cboPalette.SelectedIndex == 0);
        }

        private Color[] GetColorPalette()
        {
            return (IsCustomPalette() ? m_customPalette : Colors.PaletteArray[cboPalette.SelectedIndex]);
        }
    }
}

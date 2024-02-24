using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;

namespace PalEdit
{
    public enum PaletteFormat
    {
        Act,
        MSPal,
        JASC,
        GIMP,
        PaintNET
    }

    public class PalFile
    {
        // http://www.softhelp.ru/fileformat/pal1/Pal.htm

        private delegate TResult Func<T, TResult>(T arg);
        private const string errorMessage = "Could not recognise image format.";

        private static Dictionary<byte[], Func<BinaryReader, Color[]>> paletteFormatDecoders = new Dictionary<byte[], Func<BinaryReader, Color[]>>()
        {
            { new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' }, DecodeMSPal },      // MS Palette
            { new byte[] { (byte)'J', (byte)'A', (byte)'S', (byte)'C', (byte)'-', (byte)'P', (byte)'A', (byte)'L' }, DecodeJASCPal },               // JASC Palette
            { new byte[] { (byte)'G', (byte)'I', (byte)'M', (byte)'P', (byte)' ', (byte)'P', (byte)'a', (byte)'l', (byte)'e', (byte)'t', (byte)'t', (byte)'e' }, DecodeGIMPPal },               // GIMP Palette
            { new byte[] { (byte)';', (byte)' ', (byte)'P', (byte)'a', (byte)'i', (byte)'n', (byte)'t', (byte)'.', (byte)'N', (byte)'E', (byte)'T', (byte)' ', (byte)'P', (byte)'a', (byte)'l', (byte)'e', (byte)'t', (byte)'t', (byte)'e' }, DecodePaintNetPal },
            { new byte[] { }, DecodeActPal },
        };

        private static byte[] MsPalRiffSig = { (byte)'R', (byte)'I', (byte)'F', (byte)'F' };
        private static byte[] MsPalRiffType = { (byte)'P', (byte)'A', (byte)'L', (byte)' ' };
        private static byte[] MsPalRiffChunkSig = { (byte)'d', (byte)'a', (byte)'t', (byte)'a' };
        private static byte[] MsPalRiffChunkPalVer = { 0x00, 0x03 };

        private Color[] m_colorPalette = null;

        public PalFile()
        {
            m_colorPalette = new Color[256];
        }

        public static bool TryReadPalette(string fileName, out Color[] palette)
        {
            palette = null;

            try
            {
                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        return GetPalette(binaryReader, out palette);
                }
            }
            catch
            {
            }

            return false;
        }

        private static bool GetPalette(BinaryReader binaryReader, out Color[] palette)
        {
            palette = null;
            int maxMagicBytesLength = GetMaxMagicBytesLength();

            byte[] magicBytes = binaryReader.ReadBytes(maxMagicBytesLength);

            foreach (var kvPair in paletteFormatDecoders)
            {
                if (StartsWith(magicBytes, kvPair.Key))
                {
                    binaryReader.BaseStream.Position = kvPair.Key.Length;

                    palette = kvPair.Value(binaryReader);

                    return true;
                }
            }

            return false;
        }

        private static bool StartsWith(byte[] thisBytes, byte[] thatBytes)
        {
            if (thatBytes.Length > thisBytes.Length)
                return false;

            for (int i = 0; i < thatBytes.Length; i++)
            {
                if (thisBytes[i] != thatBytes[i])
                    return false;
            }

            return true;
        }

        private static int GetMaxMagicBytesLength()
        {
            int maxMagicBytesLength = 0;

            foreach (byte[] magicBytes in paletteFormatDecoders.Keys)
                maxMagicBytesLength = Math.Max(maxMagicBytesLength, magicBytes.Length);

            return maxMagicBytesLength;
        }

        private static Color[] DecodeMSPal(BinaryReader binaryReader)
        {
            Color[] colorPalette = null;
            
            try
            {
                int fileLength = binaryReader.ReadInt32() - 16;
                binaryReader.ReadBytes(4); // Skip RIFF type
                binaryReader.ReadBytes(4); // Skip RIFF chunk signature
                binaryReader.ReadBytes(4); // Skip Chunk size
                binaryReader.ReadBytes(2); // Skip palette version
                int palCount = binaryReader.ReadInt16();
                colorPalette = new Color[palCount];

                for (int i = 0; i < colorPalette.Length; i++)
                {
                    byte[] colorArray = binaryReader.ReadBytes(4);
                    colorPalette[i] = Color.FromArgb(colorArray[0], colorArray[1], colorArray[2]);
                }
            }
            catch
            {
            }

            return colorPalette;
        }

        private static Color[] DecodeActPal(BinaryReader binaryReader)
        {
			List<Color> colorPalette = new List<Color>();

            try
            {
                for (int i = 0; i < 256; i++)
                {
                    byte[] colorArray = binaryReader.ReadBytes(3);
					colorPalette.Add(Color.FromArgb(colorArray[0], colorArray[1], colorArray[2]));
                }

				if (binaryReader.BaseStream.Position == binaryReader.BaseStream.Length - 4)
                {
                    short palCount = ReadLittleEndianInt16(binaryReader);
                    short transparentIndex = ReadLittleEndianInt16(binaryReader);

					colorPalette.RemoveRange(palCount, 256 - palCount);

                    if (transparentIndex != -1)
                        colorPalette[transparentIndex] = Color.FromArgb(0, colorPalette[transparentIndex]);
                }
            }
            catch
            {
            }

            return colorPalette.ToArray();
        }

        private static Color[] DecodeJASCPal(BinaryReader binaryReader)
        {
            Color[] colorPalette = null;

            try
            {
                string tempString = ReadLine(binaryReader);
                string versionString = ReadLine(binaryReader);
                int palCount = Int32.Parse(ReadLine(binaryReader));
                colorPalette = new Color[palCount];

                for (int i = 0; i < colorPalette.Length; i++)
                {
                    string colorString = ReadLine(binaryReader);

                    if (colorString == null)
                        break;

                    string[] colorArray = colorString.Split(new char[] { ' ' }, StringSplitOptions.None);
                    colorPalette[i] = Color.FromArgb(Int32.Parse(colorArray[0]), Int32.Parse(colorArray[1]), Int32.Parse(colorArray[2]));
                }
            }
            catch
            {
            }

            return colorPalette;
        }

        private static Color[] DecodeGIMPPal(BinaryReader binaryReader)
        {
            List<Color> colorList = new List<Color>();

            try
            {
                while (true)
                {
                    string lineString = ReadLine(binaryReader);

                    if (lineString == null)
                        break;

                    if (lineString.Equals("") ||
                        lineString.StartsWith("Name:") ||
                        lineString.StartsWith("Columns:") ||
                        lineString.StartsWith("#"))
                        continue;

                    string[] colorArray = lineString.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (colorArray.Length < 3)
                        continue;

                    int red = 0, green = 0, blue = 0;

                    if (!Int32.TryParse(colorArray[0], out red) ||
                        !Int32.TryParse(colorArray[1], out green) ||
                        !Int32.TryParse(colorArray[2], out blue))
                        continue;

                    colorList.Add(Color.FromArgb(red, green, blue));
                }
            }
            catch
            {
            }

            return colorList.ToArray();
        }

        private static Color[] DecodePaintNetPal(BinaryReader binaryReader)
        {
            List<Color> colorList = new List<Color>();

            try
            {
                while (true)
                {
                    string lineString = ReadLine(binaryReader);

                    if (lineString == null)
                        break;

                    if (lineString.Equals("") ||
                        lineString.StartsWith(";"))
                        continue;

                    int result = 0;

                    if (Int32.TryParse(lineString, NumberStyles.HexNumber, null, out result))
                    {
                        colorList.Add(Color.FromArgb(result));
                    }
                }
            }
            catch
            {
            }

            return colorList.ToArray();
        }

        private static string ReadLine(BinaryReader binaryReader)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool foundEOL = false;
            char ch;

            while (!foundEOL)
            {
                if (binaryReader.BaseStream.Position == binaryReader.BaseStream.Length)
                {
                    if (stringBuilder.Length == 0)
                        return null;
                    else
                        break;
                }

                ch = binaryReader.ReadChar();

                switch (ch)
                {
                    case '\r':
                        if (binaryReader.PeekChar() == '\n')
                            binaryReader.ReadChar();
                        foundEOL = true;
                        break;
                    case '\n':
                        foundEOL = true;
                        break;
                    default:
                        stringBuilder.Append(ch);
                        break;
                }
            }

            return stringBuilder.ToString();
        }

        private static short ReadLittleEndianInt16(BinaryReader binaryReader)
        {
            byte[] byteArray = binaryReader.ReadBytes(sizeof(short));
            Array.Reverse(byteArray, 0, byteArray.Length);

            return BitConverter.ToInt16(byteArray, 0);
        }

        private static ushort ReadLittleEndianUInt16(BinaryReader binaryReader)
        {
            byte[] byteArray = binaryReader.ReadBytes(sizeof(ushort));
            Array.Reverse(byteArray, 0, byteArray.Length);

            return BitConverter.ToUInt16(byteArray, 0);
        }

        private static int ReadLittleEndianInt32(BinaryReader binaryReader)
        {
            byte[] byteArray = binaryReader.ReadBytes(sizeof(int));
            Array.Reverse(byteArray, 0, byteArray.Length);

            return BitConverter.ToInt32(byteArray, 0);
        }

        public static void WritePalette(string fileName, Color[] colorPalette, int transparentIndex, PaletteFormat paletteFormat)
        {
            switch (paletteFormat)
            {
                case PaletteFormat.Act:
                    WriteActFile(fileName, colorPalette, transparentIndex);
                    break;
                case PaletteFormat.MSPal:
                    WriteMSPalFile(fileName, colorPalette);
                    break;
                case PaletteFormat.JASC:
                    WriteJASCPalFile(fileName, colorPalette);
                    break;
                case PaletteFormat.GIMP:
                    WriteGIMPPalFile(fileName, colorPalette);
                    break;
                case PaletteFormat.PaintNET:
                    WritePaintNETPalFile(fileName, colorPalette);
                    break;
            }
        }

		private static void WriteActFile(string fileName, Color[] colorPalette, int transparentIndex)
		{
			List<byte> byteList = new List<byte>();

			for (int i = 0; i < 256; i++)
			{
				if (i < colorPalette.Length)
				{
					byteList.Add(colorPalette[i].R);
					byteList.Add(colorPalette[i].G);
					byteList.Add(colorPalette[i].B);
				}
				else
				{
					byteList.Add(0);
					byteList.Add(0);
					byteList.Add(0);
				}
			}

			if (transparentIndex != -1 || colorPalette.Length < 256)
			{
				byteList.Add((byte)(colorPalette.Length >> 8));
				byteList.Add((byte)colorPalette.Length);

				if (transparentIndex == -1)
				{
					byteList.Add((byte)0xFF);
					byteList.Add((byte)0xFF);
				}
				else
				{
					byteList.Add((byte)(transparentIndex >> 8));
					byteList.Add((byte)transparentIndex);
				}
			}

			File.WriteAllBytes(fileName, byteList.ToArray());
		}

		private static void WriteMSPalFile(string fileName, Color[] colorPalette)
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(MsPalRiffSig);
            byteList.AddRange(BitConverter.GetBytes(colorPalette.Length * 4 + 16));
            byteList.AddRange(MsPalRiffType);
            byteList.AddRange(MsPalRiffChunkSig);
            byteList.AddRange(BitConverter.GetBytes(colorPalette.Length * 4 + 4));
            byteList.AddRange(MsPalRiffChunkPalVer);
            byteList.AddRange(BitConverter.GetBytes((short)256));

            for (int i = 0; i < colorPalette.Length; i++)
            {
                byteList.Add(colorPalette[i].R);
                byteList.Add(colorPalette[i].G);
                byteList.Add(colorPalette[i].B);
                byteList.Add(0);
            }

            File.WriteAllBytes(fileName, byteList.ToArray());
        }

        private static void WriteJASCPalFile(string fileName, Color[] colorPalette)
        {
            List<string> lineList = new List<string>();

            lineList.Add("JASC-PAL");
            lineList.Add("0100");
            lineList.Add(colorPalette.Length.ToString());

            for (int i = 0; i < colorPalette.Length; i++)
            {
                lineList.Add(String.Format("{0} {1} {2}", colorPalette[i].R, colorPalette[i].G, colorPalette[i].B));
            }

            File.WriteAllLines(fileName, lineList.ToArray());
        }

        private static void WriteGIMPPalFile(string fileName, Color[] colorPalette)
        {
            List<string> lineList = new List<string>();

            lineList.Add("GIMP Palette");
            lineList.Add(String.Format("Name: {0}", Path.GetFileNameWithoutExtension(fileName)));
            lineList.Add("Columns: 0");
            lineList.Add("#");

            for (int i = 0; i < colorPalette.Length; i++)
            {
                lineList.Add(String.Format("{0,3} {1,3} {2,3}\tUntitled", colorPalette[i].R, colorPalette[i].G, colorPalette[i].B));
            }

            File.WriteAllLines(fileName, lineList.ToArray());
        }

        private static void WritePaintNETPalFile(string fileName, Color[] colorPalette)
        {
            List<string> lineList = new List<string>();

            lineList.Add("; Paint.NET Palette");
            lineList.Add(String.Format("; {0}", Path.GetFileNameWithoutExtension(fileName)));

            for (int i = 0; i < colorPalette.Length; i++)
            {
                lineList.Add(colorPalette[i].ToArgb().ToString("X8"));
            }

            File.WriteAllLines(fileName, lineList.ToArray());
        }
    }
}

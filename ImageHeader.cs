using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace PalEdit
{
	/// <summary>
	/// Taken from http://stackoverflow.com/questions/111345/getting-image-dimensions-without-reading-the-entire-file/111349
	/// Minor improvements including supporting unsigned 16-bit integers when decoding Jfif and added logic
	/// to load the image using new Bitmap if reading the headers fails
	/// </summary>
	public static class ImageHeader
	{
		private delegate TResult Func<T, TResult>(T arg);
		private const string errorMessage = "Could not recognise image format.";

		private static Dictionary<byte[], Func<BinaryReader, Size>> imageFormatDecoders = new Dictionary<byte[], Func<BinaryReader, Size>>()
        {
            { new byte[] { 0x42, 0x4D }, DecodeBitmap },            // Bitmap
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, DecodeGif },               // Gif
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, DecodeGif },               // Gif
            { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },   // Png
            { new byte[] { 0x00, 0x00, 0x01, 0x00 }, DecodeIco },   // Icon
            { new byte[] { 0x00, 0x00, 0x02, 0x00 }, DecodeIco },   // Cursor
            { new byte[] { 0xff, 0xd8 }, DecodeJfif },              // Jpeg
            { new byte[] { 0x00, 0x00, 0x00 }, DecodeTga },         // No Image Data Included
            { new byte[] { 0x00, 0x00, 0x01 }, DecodeTga },         // Uncompressed, Color-mapped Image
            { new byte[] { 0x00, 0x00, 0x02 }, DecodeTga },         // Uncompressed, True-color Image
            { new byte[] { 0x00, 0x00, 0x03 }, DecodeTga },         // Uncompressed, Black-and-white Image
            { new byte[] { 0x00, 0x00, 0x09 }, DecodeTga },         // Run-length encoded, Color-mapped Image
            { new byte[] { 0x00, 0x00, 0x0A }, DecodeTga },         // Run-length encoded, True-color Image
            { new byte[] { 0x00, 0x00, 0x0B }, DecodeTga },         // Run-length encoded, Black-and-white Image
            //{ new byte[] { 0x43, 0x57, 0x53 }, DecodeSwf },         // Swf Compressed
            //{ new byte[] { 0x46, 0x57, 0x53 }, DecodeSwf },         // Swf Uncompressed
        };

		/// <summary>        
		/// Gets the dimensions of an image.        
		/// </summary>        
		/// <param name="path">The path of the image to get the dimensions of.</param>        
		/// <returns>The dimensions of the specified image.</returns>        
		/// <exception cref="ArgumentException">The image was of an unrecognised format.</exception>        
		public static bool TryGetDimensions(string path, out Size size)
		{
			size = Size.Empty;

			try
			{
				using (FileStream fileStream = File.OpenRead(path))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
						return GetDimensions(binaryReader, out size);
				}
			}
			catch
			{
				//LogFile.WriteLine("TryGetDimensions", "ImageHeader", ex.Message, ex.StackTrace);
			}

			return false;
		}

		public static int GetMaxMagicBytesLength()
		{
			int maxMagicBytesLength = 0;

			foreach (byte[] magicBytes in imageFormatDecoders.Keys)
				maxMagicBytesLength = Math.Max(maxMagicBytesLength, magicBytes.Length);

			return maxMagicBytesLength;
		}

		/// <summary>        
		/// Gets the dimensions of an image.        
		/// </summary>        
		/// <param name="path">The path of the image to get the dimensions of.</param>        
		/// <returns>The dimensions of the specified image.</returns>        
		/// <exception cref="ArgumentException">The image was of an unrecognised format.</exception>            
		public static bool GetDimensions(BinaryReader binaryReader, out Size size)
		{
			size = Size.Empty;
			int maxMagicBytesLength = GetMaxMagicBytesLength();

			byte[] magicBytes = binaryReader.ReadBytes(maxMagicBytesLength);

			foreach (var kvPair in imageFormatDecoders)
			{
				if (StartsWith(magicBytes, kvPair.Key))
				{
					binaryReader.BaseStream.Position = kvPair.Key.Length;

					size = kvPair.Value(binaryReader);

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

		private static int BitStringToInteger(String bits)
		{
			int converted = 0;
			for (int i = 0; i < bits.Length; i++)
			{
				converted = (converted << 1) + (bits[i] == '1' ? 1 : 0);
			}
			return converted;
		}

		private static String ByteArrayToBitString(byte[] byteArray)
		{
			byte[] newByteArray = new byte[byteArray.Length];
			Array.Copy(byteArray, newByteArray, byteArray.Length);
			String converted = "";
			for (int i = 0; i < newByteArray.Length; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					converted += (newByteArray[i] & 0x80) > 0 ? "1" : "0";
					newByteArray[i] <<= 1;
				}
			}
			return converted;
		}

		private static Size DecodeBitmap(BinaryReader binaryReader)
		{
			binaryReader.ReadBytes(16);
			int width = binaryReader.ReadInt32();
			int height = binaryReader.ReadInt32();
			return new Size(width, height);
		}

		private static Size DecodeGif(BinaryReader binaryReader)
		{
			int width = binaryReader.ReadInt16();
			int height = binaryReader.ReadInt16();
			return new Size(width, height);
		}

		private static Size DecodePng(BinaryReader binaryReader)
		{
			binaryReader.ReadBytes(8);
			int width = ReadLittleEndianInt32(binaryReader);
			int height = ReadLittleEndianInt32(binaryReader);
			return new Size(width, height);
		}

		private static Size DecodeJfif(BinaryReader binaryReader)
		{
			while (binaryReader.ReadByte() == 0xff)
			{
				byte marker = binaryReader.ReadByte();
				short chunkLength = ReadLittleEndianInt16(binaryReader);

				if (marker >= 0xc0 && marker <= 0xc3)
				{
					binaryReader.ReadByte();

					int height = ReadLittleEndianInt16(binaryReader);
					int width = ReadLittleEndianInt16(binaryReader);

					return new Size(width, height);
				}

				if (chunkLength < 0)
				{
					ushort uchunkLength = (ushort)chunkLength;
					binaryReader.ReadBytes(uchunkLength - 2);
				}
				else
				{
					binaryReader.ReadBytes(chunkLength - 2);
				}
			}

			throw new ArgumentException(errorMessage);
		}

		private static Size DecodeTga(BinaryReader binaryReader)
		{
			binaryReader.ReadBytes(11);

			int width = binaryReader.ReadInt16();
			int height = binaryReader.ReadInt16();

			return new Size(width, height);
		}

		/* private static Size DecodeSwf(BinaryReader binaryReader)
		{
			Stream inputStream = null;
			byte[] signature = new byte[8];
			byte[] rect = new byte[8];
			binaryReader.BaseStream.Position = 0;
			binaryReader.Read(signature, 0, 8);
			inputStream = (Encoding.ASCII.GetString(signature, 0, 3) == "CWS" ? new InflaterInputStream(binaryReader.BaseStream) : binaryReader.BaseStream);
			inputStream.Read(rect, 0, 8);
			int nbits = rect[0] >> 3;
			rect[0] = (byte)(rect[0] & 0x07);
			String bits = ByteArrayToBitString(rect);
			bits = bits.Remove(0, 5);
			int[] dims = new int[4];
			for (int i = 0; i < 4; i++)
			{
				char[] dest = new char[nbits];
				bits.CopyTo(0, dest, 0, bits.Length > nbits ? nbits : bits.Length);
				bits = bits.Remove(0, bits.Length > nbits ? nbits : bits.Length);
				dims[i] = BitStringToInteger(new String(dest)) / 20;
			}

			return new Size(dims[1] - dims[0], dims[3] - dims[2]);
		} */

		private static Size DecodeIco(BinaryReader binaryReader)
		{
			int count = binaryReader.ReadInt16();
			Size maxSize = new Size();

			for (int i = 0; i < count; i++)
			{
				int width = binaryReader.ReadByte();
				int height = binaryReader.ReadByte();

				if (width > maxSize.Width && height > maxSize.Height)
					maxSize = new Size(width, height);

				binaryReader.ReadBytes(14);
			}

			return maxSize;
		}
	}
}

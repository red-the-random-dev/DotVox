/*
 * Class that carries the WAV file metadata.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace DotVox.Wave
{
	public static partial class WaveFormatter
	{
		public static void PackUInt32ToByteArray(UInt32 x, Byte[] DataBuffer, Int32 o = 0, Boolean Invert = false)
		{
			if (Invert)
			{
				DataBuffer[o+3] = ((Byte) (x >> 24));
				DataBuffer[o+2] = ((Byte) ((x >> 16) % 256));
				DataBuffer[o+1] = ((Byte) ((x >> 8) % 256));
				DataBuffer[o+0] = ((Byte) (x % 256));
			}
			else
			{
				DataBuffer[o+0] = ((Byte) (x >> 24));
				DataBuffer[o+1] = ((Byte) ((x >> 16) % 256));
				DataBuffer[o+2] = ((Byte) ((x >> 8) % 256));
				DataBuffer[o+3] = ((Byte) (x % 256));
			}
		}
		public static void PackUInt32ToByteArray(UInt16 x, Byte[] DataBuffer, Int32 o = 0, Boolean Invert = false)
		{
			if (Invert)
			{
				DataBuffer[o+1] = ((Byte) (x >> 8));
				DataBuffer[o+0] = ((Byte) (x % 256));
			}
			else
			{
				DataBuffer[o+0] = ((Byte) (x >> 8));
				DataBuffer[o+1] = ((Byte) (x % 256));
			}
		}
		
		public static Byte[] FormBinaryHeader(WaveHeader w, Boolean ii = false)
		{
			Int32 HeaderSize = Marshal.SizeOf(w);
			Byte[] DataBuffer = new Byte[HeaderSize];
			PackUInt32ToByteArray(w.ChunkId, DataBuffer, 0, ii);
			PackUInt32ToByteArray(w.ChunkSize, DataBuffer, 4, true);
			PackUInt32ToByteArray(w.Format, DataBuffer, 8, ii);
			PackUInt32ToByteArray(w.Subchunk1Id, DataBuffer, 12, ii);
			PackUInt32ToByteArray(w.Subchunk1Size, DataBuffer, 16, true);
			PackUInt32ToByteArray(w.AudioFormat, DataBuffer, 20, true);
			PackUInt32ToByteArray(w.NumChannels, DataBuffer, 22, true);
			PackUInt32ToByteArray(w.SampleRate, DataBuffer, 24, true);
			PackUInt32ToByteArray(w.ByteRate, DataBuffer, 28, true);
			PackUInt32ToByteArray(w.BlockAlign, DataBuffer, 32, true);
			PackUInt32ToByteArray(w.BitsPerSample, DataBuffer, 34, true);
			PackUInt32ToByteArray(w.Subchunk2Id, DataBuffer, 36, ii);
			PackUInt32ToByteArray(w.Subchunk2Size, DataBuffer, 40, true);
			
			return DataBuffer;
		}
	}
	
	
	/// <summary>
	/// Struct for description WAV file header.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public class WaveHeader
	{
		// RIFF header
		public UInt32 ChunkId = 0x52494646;

		// 36 + subchunk2Size, or:
		// 4 + (8 + subchunk1Size) + (8 + subchunk2Size)
		// Remaining chain length counted from this position.
		// In fact it is file_size - 8,
		// (with RIFF header excluded)
		public UInt32 ChunkSize;

		// "WAVE" in ASCII encoding
		// (0x57415645 in big-endian interpretation)
		public UInt32 Format = 0x57415645;
	
		// WAVE format consists of two chains: "fmt " and "data":
		// Chain "fmt " describes format of encoded audio:
	
		// Contains "fmt " symbols
		// (0x666d7420 in big-endian interpretation)
		public UInt32 Subchunk1Id = 0x666d7420;
		
		// Remaining size of chain counted down from this position.
		public UInt32 Subchunk1Size = 16;

		// Audio format
		// For PCM = 1
		// Values different from 1 signify the compression.
		public UInt16 AudioFormat = 1;
	
		// Amount of channels.
		public UInt16 NumChannels = 1;

		// Sampling rate.
		public UInt32 SampleRate = 44100;

		// sampleRate * numChannels * bitsPerSample/8
		public UInt32 ByteRate = 44100;

		// numChannels * bitsPerSample/8
		// Amount of bytes for single sample.
		public UInt16 BlockAlign = 1;

		// Depth of sound (8 bits, 16 bits, ...)
		public UInt16 BitsPerSample = 8;

		// Chain "data" contains audio data and info about its length.

		// "data" in ASCII
		// (0x64617461 in big-endian interpretation)
		public UInt32 Subchunk2Id = 0x64617461;

		// numSamples * numChannels * bitsPerSample/8
		// Amount of bytes in data scope.
		public UInt32 Subchunk2Size;

		// Further file contains audio wave peaks.
	}
}
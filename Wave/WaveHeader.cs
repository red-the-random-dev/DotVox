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
	
	[StructLayout(LayoutKind.Sequential)]
	// Структура, описывающая заголовок WAV файла.
	public class WaveHeader
	{
		// RIFF header
		public UInt32 ChunkId = 0x52494646;

		// 36 + subchunk2Size, или более точно:
		// 4 + (8 + subchunk1Size) + (8 + subchunk2Size)
		// Это оставшийся размер цепочки, начиная с этой позиции.
		// Иначе говоря, это размер файла - 8, то есть,
		// исключены поля chunkId и chunkSize.
		public UInt32 ChunkSize;

		// Содержит символы "WAVE"
		// (0x57415645 в big-endian представлении)
		public UInt32 Format = 0x57415645;
	
		// Формат "WAVE" состоит из двух подцепочек: "fmt " и "data":
		// Подцепочка "fmt " описывает формат звуковых данных:
	
		// Содержит символы "fmt "
		// (0x666d7420 в big-endian представлении)
		public UInt32 Subchunk1Id = 0x666d7420;
		
		// 16 для формата PCM.
		// Это оставшийся размер подцепочки, начиная с этой позиции.
		public UInt32 Subchunk1Size = 16;

		// Аудио формат, полный список можно получить здесь http://audiocoding.ru/wav_formats.txt
		// Для PCM = 1 (то есть, Линейное квантование).
		// Значения, отличающиеся от 1, обозначают некоторый формат сжатия.
		public UInt16 AudioFormat = 1;
	
		// Количество каналов. Моно = 1, Стерео = 2 и т.д.
		public UInt16 NumChannels = 1;

		// Частота дискретизации. 8000 Гц, 44100 Гц и т.д.
		public UInt32 SampleRate = 44100;

		// sampleRate * numChannels * bitsPerSample/8
		public UInt32 ByteRate = 44100;

		// numChannels * bitsPerSample/8
		// Количество байт для одного сэмпла, включая все каналы.
		public UInt16 BlockAlign = 1;

		// Так называемая "глубиная" или точность звучания. 8 бит, 16 бит и т.д.
		public UInt16 BitsPerSample = 8;

		// Подцепочка "data" содержит аудио-данные и их размер.

		// Содержит символы "data"
		// (0x64617461 в big-endian представлении)
		public UInt32 Subchunk2Id = 0x64617461;

		// numSamples * numChannels * bitsPerSample/8
		// Количество байт в области данных.
		public UInt32 Subchunk2Size;

		// Далее следуют непосредственно Wav данные.
	}
}
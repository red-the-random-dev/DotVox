/*
 * Not mounted at the moment 
 */
using System;
using System.IO;

using DotVox.Wave;
using DotVox.Vocalization;

namespace Speaker
{
	static class SoundTest
	{
		public static void LegacyProcess(string[] args)
		{
			WaveHeader header = new WaveHeader();
			// Размер заголовка
			Int32 headerSize = Marshal.SizeOf(header);

			FileStream fileStream = new FileStream("aa-a-a-a.wav", FileMode.Open, FileAccess.Read);
			Byte[] buffer = new byte[headerSize];
			fileStream.Read(buffer, 0, headerSize);
			
			// Чтобы не считывать каждое значение заголовка по отдельности,
			// воспользуемся выделением unmanaged блока памяти
			var headerPtr = Marshal.AllocHGlobal(headerSize);
			// Копируем считанные байты из файла в выделенный блок памяти
			Marshal.Copy(buffer, 0, headerPtr, headerSize);
			// Преобразовываем указатель на блок памяти к нашей структуре
			Marshal.PtrToStructure(headerPtr, header);

			// Выводим полученные данные
			Console.WriteLine("Sample rate: {0}", header.SampleRate);
			Console.WriteLine("Channels: {0}", header.NumChannels);
			Console.WriteLine("Bits per sample: {0}", header.BitsPerSample);

			// Посчитаем длительность воспроизведения в секундах
			Double durationSeconds = 1.0 * header.Subchunk2Size / (header.BitsPerSample / 8.0) / header.NumChannels / header.SampleRate;
			Double durationMinutes = (int)Math.Floor(durationSeconds / 60);
			durationSeconds = durationSeconds - (durationMinutes * 60);
			Console.WriteLine("Duration: {0:00}:{1:00}", durationMinutes, durationSeconds);

			Console.ReadKey();
			
			// Освобождаем выделенный блок памяти
			Marshal.FreeHGlobal(headerPtr);
			fileStream.Close();
			fileStream = new FileStream("a.wav", FileMode.Open, FileAccess.Read);
			SoundPlayer sp = new SoundPlayer();
			sp.Stream = fileStream;
			sp.PlaySync();
			Console.WriteLine("hey!");
			fileStream.Close();
			
			// fileStream = new FileStream("subject.wav", FileMode.Open, FileAccess.Read);
			
			header.BlockAlign = 2;
			header.BitsPerSample = 8;
			header.NumChannels = 2;
			
			Console.WriteLine("Wave data ready!");
			
			// Console.ReadKey(true);
			
			header.ChunkSize = (UInt32) (36 + ((UInt32) header.SampleRate * 12));
			header.Subchunk2Size = ((UInt32) ((UInt32) header.SampleRate * 12));
			
			Byte[] a = WaveFormatter.FormBinaryHeader(header, true);
			FileStream x = new FileStream("all_notes.wav", FileMode.Create, FileAccess.Write);
			BinaryWriter bw = new BinaryWriter(x);
			bw.Write(a);
			foreach (String j in new String[] {"AH", "OH", "UH", "IY", "IH", "IX"})
			{
				ConjoinedWave cw = Phonemes.Get(j, Note.Eb, 6, 64, (header.SampleRate));
				Polyphony o = new Polyphony(cw, 16);
				o += cw;
				for (Double i = 0; i < 1; i += o.DeltaTime)
				{
					bw.Write(o[i]);
				}
			}
			x.Close();
			x = new FileStream("all_notes.wav", FileMode.Open, FileAccess.Read);
			sp.Stream = x;
			sp.Load();
			sp.PlaySync();
			Console.ReadKey(true);
		}
		public static void Process(String[] args)
		{
			Sequencer sq = new Sequencer(1, 22050);
			for (int i = 0; i < 5; i++)
			{
				sq.Push(Phonemes.Get("/N", Note.D, 6, 32, 22050), 0.07);
				sq.Push(Phonemes.Get("IY", Note.B, 5, 64, 22050), 0.2);
				sq.Push(Phonemes.Get("/G", Note.G, 6, 64, 22050), 0.07);
				sq.Push(Phonemes.Get("AH", Note.B, 5, 64, 22050), 0.2);
				sq.Skip(0.15);
			}
			
			
			FileStream fs = new FileStream("nword2.wav", FileMode.Create, FileAccess.Write);
			WaveFormatter.WriteSequenceToFile(sq, fs);
			fs.Close();
			Console.WriteLine("Done");
			Console.ReadKey(true);
			Sequencer.SequencerStream ms = sq.SnapshotStream;
			SoundPlayer sp = new SoundPlayer();
			sp.Stream = ms;
			sp.Load();
			sp.PlaySync();
			Console.ReadKey(true);
		}
	}
}

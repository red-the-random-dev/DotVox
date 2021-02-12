/*
 * Created by SharpDevelop.
 * User: Lenovo Yoga
 * Date: 10.02.2021
 * Time: 16:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using DotVox.Wave;
using DotVox.Vocalization;

namespace DotVox.Speaker
{
	class SoundTest
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
	class Program
	{
		static void QuickPlay(TactUnit toPlay)
		{
			Sequencer sq = new Sequencer(1, 44100);
			sq.Push(toPlay);
			SoundPlayer sp = new SoundPlayer();
			sp.Stream = sq.SnapshotStream;
			sp.Load();
			sp.PlaySync();
		}
		
		static Int32 GetNoteFromChar(Char key)
		{
			switch (key)
			{
				case '1':
					return (Int32) Note.C;
				case '2':
					return (Int32) Note.Db;
				case '3':
					return (Int32) Note.D;
				case '4':
					return (Int32) Note.Eb;
				case '5':
					return (Int32) Note.E;
				case '6':
					return (Int32) Note.F;
				case '7':
					return (Int32) Note.Gb;
				case '8':
					return (Int32) Note.G;
				case '9':
					return (Int32) Note.Ab;
				case '0':
					return (Int32) Note.A;
				case '-':
					return (Int32) Note.Bb;
				case '=':
					return (Int32) Note.B;
				default:
					return -1;
			}
		}
		
		static void UpdateConsoleTitle(Byte octave = 5, Int32 position = 0)
		{
			Console.Title = String.Format("DotVox Speaker == Position: {0}, Octave: {1}", position, octave);
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			String DefaultConsoleTitle = Console.Title;
			Int32 Position = 0;
			Byte CurrentOctave = 4;
			Boolean Uninterrupted = true;
			List<TactUnit> Tacts = new List<TactUnit>();
			
			ConsoleColor DefaultTextColor = Console.ForegroundColor;
			ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;
			
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			
			for (int i = 0; i < 12; i++)
			{
				Console.Write(Notes.StringForm((Note) i, 4));
				Console.Write("  ");
			}
			Console.Write("\n");
			
			Console.BackgroundColor = DefaultBackgroundColor;
			Console.ForegroundColor = DefaultTextColor;
			
			while (Uninterrupted)
			{
				UpdateConsoleTitle(CurrentOctave, Position);
				
				ConsoleKeyInfo cki = Console.ReadKey(true);
				switch (cki.KeyChar)
				{
					case ' ':
						Console.Write("___");
						Console.Write(" ");
						Tacts.Add(new TactUnit(0.125));
						break;
					case 'z':
						Uninterrupted = false;
						Console.Write("\n");
						break;
					case '	':
					{
						Sequencer sq = new Sequencer(1, 44100);
						foreach (TactUnit i in Tacts)
						{
							sq.Push(i);
						}
						
						if (sq.Length != 0)
						{
							SoundPlayer sp = new SoundPlayer();
							sp.Stream = sq.SnapshotStream;
							sp.Load();
							sp.PlaySync();
						}
						break;
					}
					case ',':
					{
						if (CurrentOctave > 4)
						{
							CurrentOctave--;
						}
						break;
					}
					case '.':
					{
						if (CurrentOctave < 9)
						{
							CurrentOctave++;
						}
						break;
					}
					case '\r':
					case '\n':
					{
						SaveFileDialog sfd = new SaveFileDialog();
						sfd.Title = "Save wave file to needed location...";
						sfd.Filter = "WAV file (*.wav)|*.wav";
						DialogResult dr = sfd.ShowDialog();
						if (dr == DialogResult.OK)
						{
							FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
							Sequencer sq = new Sequencer(1, 44100);
							foreach (TactUnit i in Tacts)
							{
								sq.Push(i);
							}
							WaveFormatter.WriteSequenceToFile(sq, fs);
						}
						break;
					}
					default:
					{
						Int32 x = GetNoteFromChar(cki.KeyChar);
						if (x != -1)
						{
							Note a = (Note) x;
							Console.Write(Notes.StringForm(a, CurrentOctave));
							TactUnit tact = new TactUnit(Phonemes.Get("AH", a, CurrentOctave, 64, 44100), 0.125);
							Tacts.Add(tact);
							// QuickPlay(tact);
							Console.Write(" ");
						}
						break;
					}
				}
				Position = Tacts.Count;
				if ((Console.CursorLeft != 0) && (Position % 16 == 0))
				{
					Console.Write("\n");
				}
			}
			Console.Title = DefaultConsoleTitle;
		}
	}
}
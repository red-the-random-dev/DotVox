/*
 * Basic 8 bit music synthesis program.
 * 
 * Dependencies:
 * -- DotVox.Wave
 * -- DotVox.Vocalization
 * 
 */
using System;
using System.Collections.Generic;
using System.Media;
using System.IO;
using System.Windows.Forms;


using DotVox.Wave;
using DotVox.Vocalization;

namespace DotVox.Speaker
{
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
		
		static void UpdateConsoleTitle(Byte octave = 5, Int32 position = 0, Boolean harmonic = false, Boolean square = false)
		{
			Console.Title = String.Format("DotVox Speaker == Position: {0}, Octave: {1}, Mode: {2}", position, octave, (harmonic ? (square ? "Square" : "Harmonic") : "Phonetic"));
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			String DefaultConsoleTitle = Console.Title;
			Int32 Position = 0;
			Byte CurrentOctave = 4;
			Boolean Uninterrupted = true;
			Boolean Harmonic = false;
			Boolean Square = false;
			List<TactUnit> Tacts = new List<TactUnit>();
			Queue<Char> InitialEntries = new Queue<Char>();
			
			ConsoleColor DefaultTextColor = Console.ForegroundColor;
			ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;
			
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			
			if (args.Length >= 1)
			{
				foreach (Char i in args[0])
				{
					InitialEntries.Enqueue(i);
				}
			}
			
			for (int i = 0; i < 12; i++)
			{
				if (i == 1 || i == 3 || i == 6 || i == 8 || i == 10)
				{
					Console.BackgroundColor = DefaultBackgroundColor;
					Console.ForegroundColor = DefaultTextColor;
				}
				else
				{
					Console.BackgroundColor = ConsoleColor.White;
					Console.ForegroundColor = ConsoleColor.Black;
				}
				
				Console.Write(Notes.StringForm((Note) i));
				Console.BackgroundColor = ConsoleColor.White;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.Write(" ");
			}
			Console.Write("\n");
			List<Char> InputStack = new List<Char>();
			TactUnit Last = new TactUnit(0.0);
			
			Console.BackgroundColor = DefaultBackgroundColor;
			Console.ForegroundColor = DefaultTextColor;
			
			while (Uninterrupted)
			{
				UpdateConsoleTitle(CurrentOctave, Position, Harmonic, Square);
				
				Char cki = (InitialEntries.Count > 0 ? InitialEntries.Dequeue() : Console.ReadKey(true).KeyChar);
				if (cki == ' ' || cki == '1' || cki == '2' || cki == '3' || cki == '4' || cki == '5' || cki == '6' || cki == '7' || cki == '8' || cki == '9' || cki == '0' || cki == '-' || cki == '=' || cki == ',' || cki == '.' || cki == '`' || cki == 'v' || cki == 'q')
				{
					InputStack.Add(cki);
				}
				switch (cki)
				{
					case ' ':
						Console.Write("____");
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
							fs.Close();
						}
						break;
					}
					case 'r':
					{
						if (Position > 0)
						{
							List<TactUnit> tu = new List<TactUnit>();
							
							for (int i = 0; i < Position-1; i++)
							{
								tu.Add(Tacts[i]);
							}
							Tacts.Clear();
							
							foreach (TactUnit x in tu)
							{
								Tacts.Add(x);
							}
							
							Int32 CharsEnlisted = InputStack.Count;
							Int32 CutHere = CharsEnlisted;
							while (CutHere > 0)
							{
								Char last = InputStack[CutHere-1];
								Boolean pop = false;
								switch (last)
								{
									case ',':
										if (CurrentOctave < 9)
										{
											CurrentOctave++;
										}
										CutHere--;
										break;
									case '.':
										if (CurrentOctave > 4)
										{
											CurrentOctave--;
										}
										CutHere--;
										break;
									case '~':
										Harmonic = false;
										CutHere--;
										break;
									case 'v':
										Harmonic = true;
										CutHere--;
										break;
									case 'q':
										Square = !Square;
										CutHere--;
										break;
									default:
										pop = true;
										CutHere--;
										break;
								}
								if (pop)
								{
									break;
								}
							}
							if (CharsEnlisted > 0)
							{
								List<Char> cl = new List<char>();
								for (int i = 0; i < CutHere; i++)
								{
									cl.Add(InputStack[i]);
								}
								
								InputStack.Clear();
								
								foreach (Char i in cl)
								{
									InputStack.Add(i);
								}
							}
							
							if (Console.CursorLeft == 0)
							{
								Console.CursorTop -= 1;
								Console.CursorLeft = 75;
							}
							else
							{
								Console.CursorLeft -= 5;
							}
							Console.Write("     ");
							Console.CursorLeft -= 5;
						}
						break;
					}
					case '`':
					{
						Harmonic = true;
						break;
					}
					case 'v':
					{
						Harmonic = false;
						break;
					}
					case 'q':
					{
						Square = !Square;
						break;	
					}
					default:
					{
						Int32 x = GetNoteFromChar(cki);
						if (x != -1)
						{
							Note a = (Note) x;
							Console.Write("{0}{1}", (Harmonic ? (Square ? "+" : "~") : "-") , Notes.StringForm(a, CurrentOctave));
							TactUnit tact = new TactUnit(new AmplifiedWave(Phonemes.Get((Harmonic ? (Square ? "__" : "~~") : "AH"), a, CurrentOctave, 64, 44100), 0.25), 0.125);
							if (Position > 1)
							{
								if (Tacts[Position-1].Frequency == tact.Frequency)
								{
									tact.BeginningOffset = Tacts[Position-1].BeginningOffset + 0.125;
								}
							}
							
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
			foreach (Char i in InputStack)
			{
				Console.Write(i);
			}
			Console.WriteLine();
			Console.ReadLine();
		}
	}
}
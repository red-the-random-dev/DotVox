/*
 * Application for converting phoneme sequences into speech. 
 */
using System;
using System.Media;
using System.Collections.Generic;
using System.IO;
using DotVox.Vocalization;
using DotVox.Wave;

namespace DotVox.Voxel
{
	class Program
	{
		static Double DefaultVowelLength = 0.125;
		
		static T[] CutArray<T>(T[] target, Int32 Offset)
		{
			T[] x = new T[target.Length-Offset];
			for (int i = Offset; i < target.Length; i++)
			{
				x[i-Offset] = target[i];
			}
			return x;
		}
		
		public static void Process(string[] args, Byte speed, Note key, Byte keyOctave, Boolean ExporttoWAV = false, String ExportPath="")
		{
			DefaultVowelLength = 0.125 / (1.0 * speed / 64);
			
			if (args.Length == 0)
			{
				Console.WriteLine("VOXEL v.0.3.1\n\nVOWELS:\n__ AH __ p(a)rk\n__ EH __ n(e)ck\n__ IY __ f(ee)l\n__ ER __ b(i)rd\n__ AE __ d(a)y\n__ AO __ t(a)lk\n__ AX __ b(u)dget\n__ IH __ p(i)ck\n__ UH __ p(u)t");
			}
			else if (args.Length >= 3 && (args.Length-1) % 2 == 0)
			{
				switch (args[0])
				{
					case "-wav":
					{
						String ExportFile = args[1];
						Process(CutArray<String>(args, 2), speed, key, keyOctave, true, ExportFile);
						break;
					}
					case "-speed":
					{
						Byte newSpeed = Byte.Parse(args[1]);
						Process(CutArray<String>(args, 2), newSpeed, key, keyOctave, ExporttoWAV, ExportPath);
						break;
					}
				}
			}
			else if (args.Length == 1)
			{
				Double plusTime = 0.0;
				List<TactUnit> tacts = new List<TactUnit>();
				for (int i = 0; i < args[0].Length; i += 2)
				{
					Char first = args[0][i];
					if (first == ' ')
					{
						TactUnit skip = new TactUnit(DefaultVowelLength/2);
						tacts.Add(skip);
						i--;
						continue;
					}
					if (first == '.')
					{
						TactUnit skip = new TactUnit(DefaultVowelLength);
						tacts.Add(skip);
						i--;
						continue;
					}
					if (i == args[0].Length-1)
					{
						break;
					}
					Char second = args[0][i+1];
					String phoneme = "" + first + second;
					TactUnit t = new TactUnit(Phonemes.Get(phoneme, key, keyOctave, 96, 44100), (Phonemes.IsVowel(phoneme) ? DefaultVowelLength : DefaultVowelLength/4));
					t.BeginningOffset = plusTime;
					plusTime += t.PlayTime;
					tacts.Add(t);
				}
				Sequencer sq = new Sequencer(1, 44100);
				
				for (int i = 0; i < tacts.Count; i++)
				{
					sq.Push(tacts[i]);
					if (i < tacts.Count - 1)
					{
						if (!tacts[i+1].Blank && !tacts[i].Blank)
						{
							if (tacts[i+1].Phoneme != tacts[1].Phoneme)
							{
								Transition conjoin = new Transition(tacts[i].Wave, tacts[i+1].Wave, DefaultVowelLength/8, tacts[i].BeginningOffset+tacts[i].PlayTime, tacts[i+1].BeginningOffset-(DefaultVowelLength/8));
								sq.Push(conjoin);
							}
						}
					}
				}
				
				if (ExporttoWAV)
				{
					FileStream fs = new FileStream(ExportPath, FileMode.Create, FileAccess.Write);
					fs.Position = 0;
					WaveFormatter.WriteSequenceToFile(sq, fs);
					fs.Close();
				}
				else
				{
					SoundPlayer sp = new SoundPlayer();
					sp.Stream = sq.SnapshotStream;
					sp.Load();
					sp.PlaySync();
				}
			}
		}
		
		public static void Main(string[] args)
		{
			Process(args, 64, Note.C, 5);
		}
	}
}
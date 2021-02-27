/* 
 * Created by SharpDevelop.
 * Date: 27.02.2021
 * Time: 15:19
 */
using System;
using System.Collections.Generic;
using System.IO;

using DotVox.Composition16;
using DotVox.Riffs16;
using DotVox.Wave16;

namespace DotVox.SheetProcessor
{
	class Program
	{
		static Dictionary<String, UInt32> ComposerSettings = new Dictionary<String, UInt32>();
		static Dictionary<String, InstrumentConfig16> InstrumentSet = new Dictionary<String, InstrumentConfig16>();
		static KeyboardConfig Keys = KeyboardConfig.Default;
		static SequenceTracer16 sqt;
		static Sequencer16 sq16;
		
		static Boolean OnAir = false;
		
		static String SavePath = "comp_"+DateTime.Now.Ticks+".wav";
		
		public static void InitializeSettings()
		{
			ComposerSettings.Add("feedback", 0);
			ComposerSettings.Add("saveatend", 0);
			ComposerSettings.Add("tactrate", 8);
			ComposerSettings.Add("samplerate", 44100);
			ComposerSettings.Add("visualize", 0);
			ComposerSettings.Add("pauseatend", 0);
		}
		
		public static void Main(string[] args)
		{
			InitializeSettings();
			if (args[0].Length == 0)
			{
				Environment.Exit(-1);
			}
			FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(fs);
			
			List<String[]> Commands = new List<string[]>();
			
			while (!sr.EndOfStream)
			{
				Commands.Add(sr.ReadLine().Split());
			}
			sr.Close();
			fs.Close();
			foreach (String[] x in Commands)
			{
				Boolean DoFeedback = (ComposerSettings["feedback"] == 1);
				Boolean DoVisualize = (ComposerSettings["visualize"] == 1);
				if (x.Length == 0)
				{
					if (OnAir)
					{
						sqt.SkipAll();
					}
					continue;
				}
				
				switch(x[0])
				{
					case "#cfg":
					{
						List<String[]> cfgs = new List<string[]>();
						
						for (int c = 1; c < x.Length; c++)
						{
							cfgs.Add(x[c].Split(':'));
						}
						foreach (String[] p in cfgs)
						{
							ComposerSettings[p[0]] = UInt32.Parse(p[1]);
						}
						break;
					}
					case "#instruments":
					{
						List<String[]> cfgs = new List<string[]>();
						
						for (int c = 1; c < x.Length; c++)
						{
							cfgs.Add(x[c].Split(':'));
						}
						foreach (String[] p in cfgs)
						{
							String[] defSettings = {null, null, "24576", "1", "1", "1", "1"};
							
							for (int i = 0; i < Math.Min(p.Length, defSettings.Length); i++)
							{
								defSettings[i] = p[i];
							}
							
							InstrumentConfig16 ic16 = new InstrumentConfig16
							(
								Instruments16.GetTimedWave,
								Int32.Parse(defSettings[1]),
								Int16.Parse(defSettings[2]),
								ComposerSettings["samplerate"],
								(defSettings[3] == "1"),
								Double.Parse(defSettings[4]),
								Double.Parse(defSettings[5]),
								(defSettings[6] == "1")
							);
							if (DoFeedback)
							{
								Console.WriteLine("Instrument definition: Tag: {0}, IID: {1}, Volume: {2}, Fadeout: {3}, Fadeout strength: {4}, Start position: {5}, Keep start volume: {6}", defSettings[0], ic16.Number, ic16.Amplitude, ic16.DoFadeout, ic16.FadeoutStrength, ic16.FadeoutPosition, ic16.KeepVolumeBeforeFadeout);
							}
							InstrumentSet.Add(defSettings[0], ic16);
						}
						break;
					}
					case "#saveat":
					{
						SavePath = x[1];
						break;
					}
					case "#begin":
					{
						if (DoFeedback)
						{
							Console.WriteLine("Starting file parsing...");
						}	
						OnAir = true;
						sqt = new SequenceTracer16(Keys, InstrumentSet, (1.0 / ComposerSettings["tactrate"]));
						break;
					}
					case "#end":
					{
						if (DoFeedback)
						{
							Console.WriteLine("Finished parsing, saving...");
						}
						OnAir = false;
						sq16 = new Sequencer16(1, ComposerSettings["samplerate"]);
						sqt.Export(sq16);
						break;
					}
					case "#play":
					{
						if (DoFeedback)
						{
							Console.WriteLine("Playing . . .");
						}
						if (sq16 == null)
						{
							break;
						}
						System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
						sp.Stream = sq16.SnapshotStream;
						
						sp.Load();
						sp.PlaySync();
						break;
					}
					case "#save":
					{
						FileStream fout = new FileStream(SavePath, FileMode.Create, FileAccess.Write);
						WaveFormatter16.WriteSequenceToFile(sq16, fout);
						fout.Close();
						break;
					}
					default:
					{
						if (!OnAir)
						{
							break;
						}
						if (x.Length == 0)
						{
							sqt.SkipAll();
							break;
						}
						foreach(String b in x)
						{
							Boolean StartAnew = false;
							String[] p = b.Split(':');
							if (p.Length < 2)
							{
								sqt.SkipAll();
								break;
							}
							if (p.Length == 3)
							{
								if (p[2] == "x")
								{
									StartAnew = true;
								}
							}
							Int32 key = Int32.Parse(p[1]);
							if (StartAnew)
							{
								sqt.PushAnew(key, p[0]);
							}
							else
							{
								sqt.Push(key, p[0]);
							}
							
							if (DoVisualize)
							{
								Console.CursorLeft = key;
								Console.Write(p[0][0]);
							}
						}
						sqt.Snap();
						if (DoVisualize)
						{
							Console.WriteLine();
						}
						break;
					}
				}
			}
			
			if (ComposerSettings["pauseatend"] == 1)
			{
				Console.WriteLine("Press any key to continue . . .");
				Console.ReadKey(true);
			}
			
		}
	}
}
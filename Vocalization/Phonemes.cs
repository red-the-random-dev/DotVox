/*
 * Phonemes class definition.
 */
using System;
using DotVox.Wave;
using System.Collections.Generic;

namespace DotVox.Vocalization
{
	/// <summary>
	/// Static class for getting phoneme wave chains.
	/// </summary>
	public static class Phonemes
	{
		public static ConjoinedWave Get(String phoneme, UInt32 frequency = 523, SByte volume = 96, UInt32 SampleRate = 44100)
		{
			switch (phoneme)
			{
				#region Vovels: AH, OH, UH, IY, IH, IX
				case "AH":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, ((SByte) (volume / 3 * 2)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, 0, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a1, a3}, new UInt32[] {a1.Lambda, (a2.Lambda / 2), (a3.Lambda / 2), a1.Lambda, (a3.Lambda / 2)});
						return cw;
					}
					
				case "OH":
					{
						Oscillation a1 = new Oscillation((frequency * 19) / 50, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation((frequency * 19) / 25, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation((frequency * 19) / 25, 0, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a2}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a2.Lambda});
						return cw;
					}
				case "UH":
					{
						UInt32 newFreq = (frequency * 19) / 30;
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq, 0, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(newFreq / 2, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a1, a2, a3, a4}, new UInt32[] {(a1.Lambda / 2), (a1.Lambda / 2), a2.Lambda, (a3.Lambda / 2), (a4.Lambda / 2)});
						return cw;
					}
				case "IY":
					{
						UInt32 newFreq = (frequency * 3) / 2;
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq * 2, 0, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(newFreq * 2, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(newFreq, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(newFreq * 2, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a7 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a2, a3, a4, a5, a3, a6, a7, a3, a4, a3}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a3.Lambda / 2, a6.Lambda, a7.Lambda / 2, a3.Lambda / 2, a4.Lambda, a3.Lambda});
						return cw;
					}
				case "IH":
					{
						UInt32 newFreq = frequency;
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq * 2, 0, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(newFreq * 2, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(newFreq, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(newFreq * 2, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a7 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a2, a3, a4, a5, a3, a6, a7, a3, a4, a3}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a3.Lambda / 2, a6.Lambda, a7.Lambda / 2, a3.Lambda / 2, a4.Lambda, a3.Lambda});
						return cw;
					}
				case "IX":
					{
						UInt32 newFreq = (frequency * 2) / 3;
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq * 2, 0, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(newFreq * 2, ((SByte) (volume / 2)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(newFreq, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(newFreq * 2, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a7 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a2, a3, a4, a5, a3, a6, a7, a3, a4, a3}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a3.Lambda / 2, a6.Lambda, a7.Lambda / 2, a3.Lambda / 2, a4.Lambda, a3.Lambda});
						return cw;
					}
				#endregion
				#region Consonants: /N, ...
				case "/N":
					{
						UInt32 newFreq = (frequency * 3) / 4;
						Oscillation a1 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, ((SByte) ((0 - volume) / 2)), WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a1, a1, a2, a2, a1, a1, a1}, new UInt32[] {(a1.Lambda / 2), (a1.Lambda / 2), (a1.Lambda / 2), ((a2.Lambda * 5) / 2), ((a2.Lambda * 5) / 2), (a1.Lambda / 2), (a1.Lambda / 2), (a1.Lambda / 2)});
						return cw;
					}
				case "/G":
					{
						UInt32 newFreq = (frequency * 2 / 3);
						SByte newVolume = ((SByte) (volume * 11 / 12));
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (0 - newVolume)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, 0, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq, newVolume, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a3, a3, a2, a3, a3, a3, a3, a2, a1, a1, a2, a3, a3, a3, a2}, new UInt32[] {(a1.Lambda / 2), (a2.Lambda * 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), a2.Lambda, (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a2.Lambda * 2), (a1.Lambda / 2), (a1.Lambda / 2), (a2.Lambda * 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a2.Lambda * 48)});
						return cw;
					}
				#endregion
				default:
					return null;
			}
		}
		
		public static ConjoinedWave Get(String phoneme, Note note = Note.C, Byte octave = 5, SByte volume = 96, UInt32 SampleRate = 44100)
		{
			return Get(phoneme, Notes.Get(note, octave), volume, SampleRate);
		}
	}
}
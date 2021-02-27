/*
 * Phonemes class definition.
 */
using System;
using DotVox.Wave;

namespace DotVox.Vocalization
{
	/// <summary>
	/// Static class for getting phoneme wave chains.
	/// </summary>
	public static class Phonemes
	{
		/// <summary>
		/// Receive a wave chain which plays sound that corresponds to selected phoneme.
		/// </summary>
		/// <param name="phoneme">Index of phoneme.</param>
		/// <param name="frequency">Main frequency of sound.</param>
		/// <param name="volume">Wave's amplitude.</param>
		/// <param name="SampleRate">Sample rate of wave chain.</param>
		/// <returns>Timed wave chain.</returns>
		public static ConjoinedWave Get(String phoneme, UInt32 frequency = 523, SByte volume = 96, UInt32 SampleRate = 44100)
		{
			if (phoneme == "EH" || phoneme == "AE" || phoneme == "AA" || phoneme == "AO" || phoneme == "UX" || phoneme == "ER" || phoneme == "AH" || phoneme == "OH" || phoneme == "IY" || phoneme == "IH" || phoneme == "IX" || phoneme == "AX" || phoneme == "/N")
			{
				frequency *= 2;
			}
			else if (phoneme == "/R")
			{
				frequency = frequency * 3 / 2;
			}
			
			switch (phoneme)
			{
				#region Vowels: AH, EH, AE, AA, AO, OH, ER, UH, UX, IY, IH, IX, AX
				case "AH":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, ((SByte) (volume / 3 * 2)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, 0, WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a1, a3}, new UInt32[] {a1.Lambda, (a2.Lambda / 2), (a3.Lambda / 2), a1.Lambda, (a3.Lambda / 2)});
						return cw;
					}
				case "EH":
					{
						SByte halfAmplitude = ((SByte) (volume / 2));
						UInt32 thirdFrequency = frequency / 3;
						UInt32 doubleFrequency = frequency * 2;
						
						Oscillation a1 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(thirdFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a4, a3, a2, a1, a5, a5, a1, a1}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a3.Lambda / 2, a2.Lambda / 2, a1.Lambda / 2, a5.Lambda / 2, a5.Lambda / 2, a1.Lambda / 2, a1.Lambda / 2});
						return cw;
					}
				case "AE":
					{
						SByte halfAmplitude = ((SByte) (volume / 2));
						UInt32 doubleFrequency = ((UInt32) (frequency * 2));
						
						Oscillation a1 = new Oscillation(frequency, 0, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(doubleFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(frequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a4, a2, a2, a4, a4, a2, a5, a5, a6}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda, a4.Lambda / 2, a2.Lambda / 2, a2.Lambda / 2, a4.Lambda / 2, a4.Lambda / 2, a2.Lambda / 2, a5.Lambda / 2, a5.Lambda / 2, a6.Lambda / 2});
						return cw;
					}
				case "AA":
					{
						UInt32 halfFrequency = ((UInt32) (frequency / 2));
						SByte halfAmplitude = ((SByte) (volume / 2));
						SByte quarterAmplitude = ((SByte) (volume / 4));
						
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, ((SByte) (0 - quarterAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(halfFrequency, ((SByte) (0 - quarterAmplitude)), WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a1, a4, a1}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a1.Lambda * 3 / 2, a4.Lambda / 2, a1.Lambda / 2});
						return cw;
					}
				case "AO":
					{
						UInt32 doubleFrequency = frequency * 2;
						SByte halfAmplitude = ((SByte) (volume / 2));
						
						Oscillation a1 = new Oscillation(doubleFrequency, 0, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(doubleFrequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(frequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a4, a5, a4, a6}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a4.Lambda / 2, a6.Lambda / 2});
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
				case "ER":
					{
						UInt32 doubleFrequency = frequency * 2;
						UInt32 halfFrequency = frequency / 2;
						SByte halfAmplitude = ((SByte) (volume / 2));
						
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(halfFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(doubleFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(halfFrequency, volume, WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a1, a2, a3, a4, a5, a6, a5}, new UInt32[] {a1.Lambda / 2, a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda, a6.Lambda / 2, a5.Lambda});
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
				case "UX":
					{
						UInt32 doubleFrequency = frequency * 2;
						SByte halfAmplitude = ((SByte) (volume / 2));
						
						Oscillation a1 = new Oscillation(frequency, 0, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(doubleFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a1, a3, a2, a4}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a1.Lambda / 2, a3.Lambda, a2.Lambda / 2, a4.Lambda});
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
				case "AX":
					{
						UInt32 halfFrequency = frequency / 2;
						UInt32 doubleFrequency = frequency * 2;
						SByte halfAmplitude = ((SByte) (volume / 2));
						
						Oscillation a1 = new Oscillation(halfFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(doubleFrequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(doubleFrequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a4, a5, a4, a3, a6}, new UInt32[] {a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a4.Lambda / 2, a3.Lambda / 2, a6.Lambda / 2});
						return cw;
					}
				#endregion
				#region Voiced consonants: /R, /L, /W, /N, /G
				case "/R":
					{
						UInt32 doubleFrequency = frequency * 2;
						UInt32 halfFrequency = frequency / 2;
						SByte halfAmplitude = ((SByte) (volume / 2));
						SByte quarterAmplitude = ((SByte) (halfAmplitude / 2));
						
						Oscillation a1 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(doubleFrequency, ((SByte) (0 - halfAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(doubleFrequency, 0, WaveType.Sine, SampleRate);
						Oscillation a4 = new Oscillation(doubleFrequency, ((SByte) (0 - quarterAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a5 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a6 = new Oscillation(frequency, ((SByte) (0 - quarterAmplitude)), WaveType.Sine, SampleRate);
						Oscillation a7 = new Oscillation(frequency, quarterAmplitude, WaveType.Sine, SampleRate);
						Oscillation a8 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						Oscillation a9 = new Oscillation(halfFrequency, ((SByte) (0 - quarterAmplitude)), WaveType.Sine, SampleRate);
						
						VerticalCompressor x1 = new VerticalCompressor(a1, 32.0);
						VerticalCompressor x2 = new VerticalCompressor(a2, 32.0);
						VerticalCompressor x3 = new VerticalCompressor(a3, 32.0);
						VerticalCompressor x4 = new VerticalCompressor(a4, 32.0);
						
						ConjoinedWave cw = new ConjoinedWave(new ITimedWave[] {x1, x1, x2, x3, x4, a5, x1, a6, a7, a5, a7, a8, x1, x1, a9, a5, a5}, new UInt32[] {a1.Lambda / 2, a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2, a4.Lambda / 2, a5.Lambda / 2, a1.Lambda / 2, a6.Lambda / 2, a7.Lambda / 2, a5.Lambda / 2, a7.Lambda / 2, a8.Lambda / 2, a1.Lambda / 2, a1.Lambda / 2, a9.Lambda / 2, a5.Lambda / 2, a5.Lambda});
						return cw;
					}
				case "/L":
					{
						SByte halfAmplitude = ((SByte) (volume / 2));
						
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(frequency, halfAmplitude, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(frequency, ((SByte) (0 - volume)), WaveType.Sine, SampleRate);
						
						ConjoinedWave cw = new ConjoinedWave(new Oscillation[] {a1, a2, a3, a2, a3}, new UInt32[] {a1.Lambda, a2.Lambda / 2, a3.Lambda / 2, a2.Lambda / 2, a3.Lambda});
						return cw;
					}
				case "/N":
					{
						UInt32 newFreq = (frequency * 3) / 4;
						Oscillation a1 = new Oscillation(newFreq, volume, WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, ((SByte) ((0 - volume) / 2)), WaveType.Sine, SampleRate);
						VerticalCompressor x1 = new VerticalCompressor(a1, 32.0);
						VerticalCompressor x2 = new VerticalCompressor(a2, 32.0);
						ConjoinedWave cw = new ConjoinedWave(new ITimedWave[] {x1, x1, x1, x2, x2, x1, x1, x1}, new UInt32[] {(a1.Lambda / 2), (a1.Lambda / 2), (a1.Lambda / 2), ((a2.Lambda * 5) / 2), ((a2.Lambda * 5) / 2), (a1.Lambda / 2), (a1.Lambda / 2), (a1.Lambda / 2)});
						return cw;
					}
				case "/G":
					{
						UInt32 newFreq = (frequency * 2 / 3);
						SByte newVolume = ((SByte) (volume * 11 / 12));
						Oscillation a1 = new Oscillation(newFreq, ((SByte) (0 - newVolume)), WaveType.Sine, SampleRate);
						Oscillation a2 = new Oscillation(newFreq, 0, WaveType.Sine, SampleRate);
						Oscillation a3 = new Oscillation(newFreq, newVolume, WaveType.Sine, SampleRate);
						VerticalCompressor x1 = new VerticalCompressor(a1, 32.0);
						VerticalCompressor x2 = new VerticalCompressor(a2, 32.0);
						VerticalCompressor x3 = new VerticalCompressor(a3, 32.0);
						ConjoinedWave cw = new ConjoinedWave(new ITimedWave[] {x1, x2, x3, x3, x3, x2, x3, x3, x3, x3, x2, x1, x1, x2, x3, x3, x3, x2}, new UInt32[] {(a1.Lambda / 2), (a2.Lambda * 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), a2.Lambda, (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a2.Lambda * 2), (a1.Lambda / 2), (a1.Lambda / 2), (a2.Lambda * 2), (a3.Lambda / 2), (a3.Lambda / 2), (a3.Lambda / 2), (a2.Lambda * 24)});
						return cw;
					}
				
				#endregion
				#region Simple waves
				// Sine
				case "~~":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Sine, SampleRate);
						return new ConjoinedWave(new Oscillation[] {a1}, new UInt32[] {a1.Lambda});
					}
				// Square
				case "__":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Square, SampleRate);
						return new ConjoinedWave(new Oscillation[] {a1}, new UInt32[] {a1.Lambda});
					}
				// Triangle
				case "//":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Triangle, SampleRate);
						return new ConjoinedWave(new Oscillation[] {a1}, new UInt32[] {a1.Lambda});
					}
				// Pulse
				case "..":
					{
						Oscillation a1 = new Oscillation(frequency, volume, WaveType.Pulse, SampleRate);
						return new ConjoinedWave(new Oscillation[] {a1}, new UInt32[] {a1.Lambda});
					}
				#endregion
				default:
					return null;
			}
		}
		
		public static Boolean IsVowel(String phoneme)
		{
			switch (phoneme)
			{
				case "AH":
				case "AX":
				case "EH":
				case "IH":
				case "IY":
				case "AE":
				case "AO":
				case "AA":
				case "OH":
				case "UH":
				case "ER":
				case "UX":
					return true;
				default:
					return false;
			}
		}
		
		/// <summary>
		/// Receive a wave chain which plays sound that corresponds to selected phoneme.
		/// </summary>
		/// <param name="phoneme">Index of phoneme.</param>
		/// <param name="note">Determines tone of sound.</param>
		/// <param name="octave">Determines octave of requested note.</param>
		/// <param name="volume">Wave's amplitude.</param>
		/// <param name="SampleRate">Sample rate of wave chain.</param>
		/// <returns>Timed wave chain.</returns>
		public static ConjoinedWave Get(String phoneme, Note note = Note.C, Byte octave = 5, SByte volume = 96, UInt32 SampleRate = 44100)
		{
			return Get(phoneme, Notes.Get(note, octave), volume, SampleRate);
		}
		
		public static ConjoinedWave Get(String phoneme, Tone tone, SByte volume = 96, UInt32 SampleRate = 44100)
		{
			return Get(phoneme, tone.Frequency, volume, 44100);
		}
	}
}
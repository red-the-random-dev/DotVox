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
			if (phoneme == "EH" || phoneme == "AE" || phoneme == "AA" || phoneme == "AO" || phoneme == "AH" || phoneme == "OH")
			{
				frequency *= 2;
			}
			
			switch (phoneme)
			{
				#region Vowels: AH, EH, AE, AA, OH, UH, IY, IH, IX
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
				#region Consonants: /N, /G
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
	}
}
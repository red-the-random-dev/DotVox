/* 
 * Created by SharpDevelop.
 * Date: 24.02.2021
 * Time: 22:12
 */
using System;
using System.Collections.Generic;
using DotVox.Wave16;

namespace DotVox.Riffs16
{
	public enum KeyboardChord
	{
		Cm = 0,
		C = 1,
		Csus2 = 2, // When the chord is C sustained! :O
		Csus4 = 3,
		C5 = 4,
		Cm7 = 6,
		C7 = 7,
		Cmaj7 = 8,
		Cdim7 = 9,
		Cadd9 = 10,
		Cadd13 = 11,
		C6 = 12,
		C6_9 = 13, // Nice
		C9 = 14,
		C11 = 15,
		C13 = 16,
	}
	
	/// <summary>
	/// Allows to get polyphonic instrument sounds.
	/// </summary>
	public static class Chords16
	{
		public static ITimedWave16 Get
		(
			UInt32[] tones,
			Int32 instrumentNumber,
			Int16 volume = 24576,
			UInt32 SampleRate = 44100,
			Boolean doFadeout = false,
			Double fadeoutStrength = 1.0,
			Double fadeoutPosition = 1.0,
			Boolean keepVolumeBeforeFadeout = true
		)
		{
			List<ITimedWave16> l = new List<ITimedWave16>();
			
			foreach (UInt32 x in tones)
			{
				l.Add(Instruments16.GetTimedWave(x, instrumentNumber, volume, SampleRate, doFadeout, fadeoutStrength, fadeoutPosition, keepVolumeBeforeFadeout));
			}
			
			Polyphony16 p = new Polyphony16(l[0], ((UInt16) (l.Count*8)));
			
			for (int i = 1; i < l.Count; i++)
			{
				p += l[i];
			}
			
			return new Mixer16(p);
		}
		
		/// <summary>
		/// Receive polyphonic sound wave that corresponds to given keyboard chord.
		/// </summary>
		/// <param name="chord">Selected keyboard chord.</param>
		/// <param name="octave">Selected octave.</param>
		/// <param name="instrumentNumber">Number of instrument.</param>
		/// <param name="volume"></param>
		/// <param name="SampleRate"></param>
		/// <param name="doFadeout"></param>
		/// <param name="fadeoutStrength"></param>
		/// <param name="fadeoutPosition"></param>
		/// <param name="keepVolumeBeforeFadeout"></param>
		/// <returns></returns>
		public static ITimedWave16 GetKeyboardChord
		(
			KeyboardChord chord,
			Byte octave = 4,
			Int32 instrumentNumber = 1,
			Int16 volume = 24576,
			UInt32 SampleRate = 44100,
			Boolean doFadeout = false,
			Double fadeoutStrength = 1.0,
			Double fadeoutPosition = 1.0,
			Boolean keepVolumeBeforeFadeout = true
		)
		{
			UInt32[] newTones;
			
			switch (chord)
			{
				case KeyboardChord.C:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(4, octave), Tones16.Receive(7, octave)};
					break;
				case KeyboardChord.Cm:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(3, octave), Tones16.Receive(7, octave)};
					break;
				case KeyboardChord.Csus2:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(2, octave), Tones16.Receive(7, octave)};
					break;
				case KeyboardChord.Csus4:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(5, octave), Tones16.Receive(7, octave)};
					break;
				case KeyboardChord.C5:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(7, octave)};
					break;
				case KeyboardChord.Cmaj7:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(4, octave), Tones16.Receive(7, octave), Tones16.Receive(11, octave)};
					break;
				case KeyboardChord.Cm7:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(3, octave), Tones16.Receive(7, octave), Tones16.Receive(10, octave)};
					break;
				case KeyboardChord.C7:
					newTones = new uint[]{Tones16.Receive(0, octave), Tones16.Receive(4, octave), Tones16.Receive(7, octave), Tones16.Receive(10, octave)};
					break;
				default:
					throw new NotImplementedException("No implementation for this chord.");                                
			}
			
			return Get(newTones, instrumentNumber, volume, SampleRate, doFadeout, fadeoutStrength, fadeoutPosition, keepVolumeBeforeFadeout);
		}
	}
}

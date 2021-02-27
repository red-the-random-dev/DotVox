/* 
 * Created by SharpDevelop.
 * Date: 24.02.2021
 * Time: 22:12
 */
using System;
using System.Collections.Generic;
using DotVox.Wave;

namespace DotVox.Riffs
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
	public static class Chords
	{
		public static ITimedWave Get
		(
			UInt32[] tones,
			Int32 instrumentNumber,
			SByte volume = 96,
			UInt32 SampleRate = 44100,
			Boolean doFadeout = false,
			Double fadeoutStrength = 1.0,
			Double fadeoutPosition = 1.0,
			Boolean keepVolumeBeforeFadeout = true
		)
		{
			List<ITimedWave> l = new List<ITimedWave>();
			
			foreach (UInt32 x in tones)
			{
				l.Add(Instruments.GetTimedWave(x, instrumentNumber, volume, SampleRate, doFadeout, fadeoutStrength, fadeoutPosition, keepVolumeBeforeFadeout));
			}
			
			Polyphony p = new Polyphony(l[0], ((UInt16) (l.Count*8)));
			
			for (int i = 1; i < l.Count; i++)
			{
				p += l[i];
			}
			
			return new Mixer(p, 128);
		}
		
		public static ITimedWave GetKeyboardChord
		(
			KeyboardChord chord,
			Byte octave = 4,
			Int32 instrumentNumber = 1,
			SByte volume = 96,
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
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(4, octave), Tones.Receive(7, octave)};
					break;
				case KeyboardChord.Cm:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(3, octave), Tones.Receive(7, octave)};
					break;
				case KeyboardChord.Csus2:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(2, octave), Tones.Receive(7, octave)};
					break;
				case KeyboardChord.Csus4:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(5, octave), Tones.Receive(7, octave)};
					break;
				case KeyboardChord.C5:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(7, octave)};
					break;
				case KeyboardChord.Cmaj7:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(4, octave), Tones.Receive(7, octave), Tones.Receive(11, octave)};
					break;
				case KeyboardChord.Cm7:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(3, octave), Tones.Receive(7, octave), Tones.Receive(10, octave)};
					break;
				case KeyboardChord.C7:
					newTones = new uint[]{Tones.Receive(0, octave), Tones.Receive(4, octave), Tones.Receive(7, octave), Tones.Receive(10, octave)};
					break;
				default:
					throw new NotImplementedException("No implementation for this chord.");                                
			}
			
			return Get(newTones, instrumentNumber, volume, SampleRate, doFadeout, fadeoutStrength, fadeoutPosition, keepVolumeBeforeFadeout);
		}
	}
}

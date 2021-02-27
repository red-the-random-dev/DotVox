/* 
 * Created by SharpDevelop.
 * Date: 23.02.2021
 * Time: 16:16
 */
using System;
using System.Collections.Generic;

using DotVox.Wave16;

namespace DotVox.Riffs16
{
	// TODO: Implement more instruments and enlist them in this enumeration.
	
	/// <summary>
	/// List that contains every supported instrument type.
	/// </summary>
	public enum Instrument
	{
		HarmonicWave = 0,
		GrandPiano = 1,
		SoftGuitar = 21
	}
	
	public static class Instruments16
	{
		/// <summary>
		/// Get timed wave of needed instrument.
		/// </summary>
		/// <param name="frequency">Determines tone of needed instrument.</param>
		/// <param name="InstrumentNumber">Determines type of needed instrument.</param>
		/// <param name="volume">Sets up peak value for instrument's waveform.</param>
		/// <param name="SampleRate">Wave's sample rate.</param>
		/// <param name="DoFadeout">Perform fadeout at end of tile.</param>
		/// <param name="FadeoutStrength">Multiplier for parabolic amplitude reduction. Volume(t) = Wave(t)/t*a, where a is fadeout strength.</param>
		/// <param name="FadeoutPosition">Sets up position offset for parabolic reduction graph. Volume(t) = Wave(t)/(t*a+b), where b is fadeout position.</param>
		/// <param name="KeepVolumeBeforeFadeout">If true, volume will stay at original level rather than being increased if it happens to be so.</param>
		/// <returns>ITimedWave instance that can be used with sequencers.</returns>
		/// <example>
		/// // Receiving harmonic oscillation that corresponds to note C of octave 5 and fades out after 1 second mark:
		/// 
		/// Instruments.GetTimedWave(523, 0, 96, DoFadeout: true);
		/// </example>
		/// <exception cref="System.NotImplementedException">Occurs if attempted to instantiate sound of instrument that is not enlisted in library.</exception>
		public static ITimedWave16 GetTimedWave
		(
			UInt32 frequency,
			Int32 InstrumentNumber = 0,
			Int16 volume = 24576,
			UInt32 SampleRate = 44100u,
			Boolean DoFadeout = false,
			Double FadeoutStrength = 1.0,
			Double FadeoutPosition = 1.0,
			Boolean KeepVolumeBeforeFadeout = true
		)
		{
			ITimedWave16 myInstrument;
			
			switch (InstrumentNumber)
			{
				case 0:
				{
					myInstrument = new Oscillation16(frequency, volume, WaveType.Sine, SampleRate);
					break;
				}
				case 1:
				{
					// Some fine tuning
					frequency = Tones16.Shift(frequency, 7);
					Int16 eighthAmplitude = (Int16) (volume / 8);
					Oscillation16 a1 = new Oscillation16(frequency, volume, WaveType.Sine, SampleRate);
					Oscillation16 a2 = new Oscillation16(frequency, eighthAmplitude, WaveType.Sine, SampleRate);
					Oscillation16 a3 = new Oscillation16(frequency, ((Int16) (0 - volume)), WaveType.Sine, SampleRate);
					
					myInstrument = new ConjoinedWave16(new ITimedWave16[]{a1, a2, a3}, new uint[]{a1.Lambda / 2, a2.Lambda / 2, a3.Lambda / 2});
					break;
				}
				case 21:
				{
					// increasing base frequency in 2 times
					frequency = Tones16.Shift(frequency, 12);
					volume = ((Int16) (Math.Min(Math.Max(volume * 3 / 2, Int16.MinValue), Int16.MaxValue)));
					Int16 halfAmplitude = ((Int16) (volume / 2));
					Int16 twoThirdsAmplitude = ((Int16) (volume * 2 / 3));
					Oscillation16 a1 = new Oscillation16(frequency, volume, WaveType.Sine, SampleRate);
					Oscillation16 a2 = new Oscillation16(frequency, halfAmplitude, WaveType.Sine, SampleRate);
					Oscillation16 a3 = new Oscillation16(frequency, ((Int16) (0 - twoThirdsAmplitude)), WaveType.Sine, SampleRate);
					
					myInstrument = new ConjoinedWave16(new ITimedWave16[]{a1, a2, a3}, new uint[]{a1.Lambda, a2.Lambda / 2, a3.Lambda / 2});
					break;
				}
				default:
				{
					throw new NotImplementedException(String.Format("Unable to instantiate instrument with following number: {0}.", InstrumentNumber));
				}
			}
			
			if (DoFadeout)
			{
				myInstrument = new Fadeout16(myInstrument, FadeoutStrength, FadeoutPosition, !KeepVolumeBeforeFadeout);
			}
			
			return myInstrument;
		}
	}
}
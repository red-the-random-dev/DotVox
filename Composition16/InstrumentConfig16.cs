/* 
 * Created by SharpDevelop.
 * Date: 27.02.2021
 * Time: 14:02
 */
using System;
using DotVox.Wave16;
using DotVox.Riffs16;

namespace DotVox.Composition16
{
	public delegate ITimedWave16 D_ToneRetrievalMethod16
	(
		UInt32 frequency,
		Int32 InstrumentNumber,
		Int16 volume,
		UInt32 SampleRate,
		Boolean DoFadeout,
		Double FadeoutStrength,
		Double FadeoutPosition,
		Boolean KeepVolumeBeforeFadeout
	);
	
	/// <summary>
	/// Instrument preset.
	/// </summary>
	public class InstrumentConfig16
	{
		public D_ToneRetrievalMethod16 RetrieveTone { get; private set; }
		
		public Int16 Amplitude { get; private set; }
		public UInt32 SampleRate { get; private set; }
		public Boolean DoFadeout { get; private set; }
		public Boolean KeepVolumeBeforeFadeout { get; private set; }
		
		public Int32 Number { get; private set; }
		public Double FadeoutPosition { get; private set; }
		public Double FadeoutStrength { get; private set; }
		
		public InstrumentConfig16(D_ToneRetrievalMethod16 toneRetrievalMethod, Int32 instrumentNumber, Int16 amplitude, UInt32 sampleRate = 44100, Boolean doFadeout = false, Double fadeoutPosition = 1.0, Double fadeoutStrength = 1.0, Boolean keepVolumeBeforeFadeout = true)
		{
			RetrieveTone = toneRetrievalMethod;
			SampleRate = sampleRate;
			Number = instrumentNumber;
			Amplitude = amplitude;
			DoFadeout = doFadeout;
			FadeoutPosition = fadeoutPosition;
			FadeoutStrength = fadeoutStrength;
			KeepVolumeBeforeFadeout = keepVolumeBeforeFadeout;
		}
		
		public ITimedWave16 GetTimedWave(UInt32 frequency)
		{
			return RetrieveTone(frequency, Number, Amplitude, SampleRate, DoFadeout, FadeoutStrength, FadeoutPosition, KeepVolumeBeforeFadeout);
		}
		
		public ITimedWave16 GetTimedWave(Int32 key, KeyboardConfig kcfg)
		{
			return RetrieveTone(kcfg[key], Number, Amplitude, SampleRate, DoFadeout, FadeoutStrength, FadeoutPosition, KeepVolumeBeforeFadeout);
		}
		
		public static InstrumentConfig16 GrandPiano
		{
			get
			{
				return new InstrumentConfig16(Instruments16.GetTimedWave, 1, 24576, 44100, true);
			}
		}
		
		public static InstrumentConfig16 SoftGuitar
		{
			get
			{
				return new InstrumentConfig16(Instruments16.GetTimedWave, 1, 24576, 44100, true);
			}
		}
	}
}

/*
 * Created by SharpDevelop.
 * User: Lenovo Yoga
 * Date: 10.02.2021
 * Time: 18:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DotVox.Wave
{
	public enum WaveType { Sine, Square, Triangle, Pulse };
	
	/// <summary>
	/// Harmonic audio oscillation.
	/// </summary>
	public class Oscillation : ITimedWave
	{
		public UInt32 Frequency;
		SByte amp;
		public SByte Amplitude
		{
			get
			{
				return this.amp;
			}
			set
			{
				this.amp = value;
			}
		}
		public WaveType WaveMode;
		public readonly UInt32 SampleRate;
		
		/// <summary>
		/// Creates a preset for generating mathematic audio wave.
		/// </summary>
		/// <param name="frequency">Determines the tone of the wave.</param>
		/// <param name="amplitude">The volume of the wave at its peak.</param>
		/// <param name="mode"></param>
		/// <param name="samplerate"></param>
		public Oscillation(UInt32 frequency, SByte amplitude = 127, WaveType mode = WaveType.Sine, UInt32 samplerate = 44100)
		{
			this.Frequency = frequency;
			this.Amplitude = amplitude;
			this.WaveMode = mode;
			this.SampleRate = samplerate;
		}
		
		/// <summary>
		/// Wave length in sample units.
		/// </summary>
		public UInt32 Lambda
		{
			get
			{
				return ((SampleRate) / Frequency);
			}
		}
		
		/// <summary>
		/// Period of time between each pulsation.
		/// </summary>
		public Double Period
		{
			get
			{
				return (1.0 * this.Lambda / this.SampleRate);
			}
		}
		
		/// <summary>
		/// Period of time passing between samples.
		/// </summary>
		public Double DeltaTime
		{
			get
			{
				return (1.0 / this.SampleRate);
			}
		}
		
		/// <summary>
		/// Receive 8-bit sample on given timestamp.
		/// </summary>
		public Byte this[Double TimeStamp]
		{
			get
			{
				switch (this.WaveMode)
				{
					case WaveType.Sine:
						return ((Byte) Math.Round(Math.Sin(TimeStamp * Math.PI * this.Frequency * 2) * this.Amplitude + 128));
					case WaveType.Square:
						return ((Byte) ((((TimeStamp * SampleRate % Lambda) <= (Lambda / 2)) ? ((Int16) this.Amplitude) : ((Int16) (0 - this.Amplitude))) + 128));
					default:
						return 0;
				}
			}
		}
	}
}

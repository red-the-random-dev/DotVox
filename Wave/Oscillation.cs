/*
 * Class for generation of mathematic timed waves.
 */
using System;

namespace DotVox.Wave
{
	public enum WaveType { Sine, Square, Triangle, Pulse };
	
	/// <summary>
	/// Harmonic audio oscillation.
	/// </summary>
	[Serializable]
	public class Oscillation : ITimedWave
	{
		UInt32 freq;
		/// <summary>
		/// Returns or declares frequency of the wave.
		/// </summary>
		/// <exception cref="System.ArgumentOutOfRangeException">Received value is larger than half of sample rate.</exception>
		/// <exception cref="System.ArithmeticException">Attempted to set frequency to 0. Hint: to create flat line, set amplitude to 0 instead.</exception>
		public UInt32 Frequency
		{
			get
			{
				return freq;
			}
			set
			{
				if (SampleRate != 0)
				{
					if (value > (SampleRate / 2))
					{
						throw new ArgumentOutOfRangeException("value", "Received frequency value that does not fit into estabilished sample rate.");
					}
					else if (value == 0)
					{
						throw new ArithmeticException("Attempted to create a wave with zero frequency.");
					}
				}
				freq = value;
			}
		}
		public SByte Amplitude;
		public WaveType WaveMode;
		public readonly UInt32 SampleRate;
		
		/// <summary>
		/// Creates a preset for generating mathematic audio wave.
		/// </summary>
		/// <param name="frequency">Determines the tone of the wave.</param>
		/// <param name="amplitude">The volume of the wave at its peak.</param>
		/// <param name="mode">One of available wave generation modes: sine, square, triangle or simple pulses.</param>
		/// <param name="samplerate">Amount of bytes in one second.</param>
		public Oscillation(UInt32 frequency, SByte amplitude = 127, WaveType mode = WaveType.Sine, UInt32 samplerate = 44100)
		{
			if (samplerate == 0)
			{
				throw new ArithmeticException("Received zero value for sample rate.");
			}
			
			this.SampleRate = samplerate;
			this.Frequency = frequency;
			this.Amplitude = amplitude;
			this.WaveMode = mode;
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
		/// Period of time passing between samples. Derived from sample rate.
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
					case WaveType.Triangle:
					{
						Int64 point = ((Int64) Math.Round(TimeStamp / DeltaTime)) % Lambda;
						Int64 quarterPeriod = ((Int64) (Lambda / 4));
						Int64 halfPeriod = ((Int64) (Lambda / 2));
						
						Int64 distanceFromHalf = ((Int64) Math.Abs(point - halfPeriod));
						Int64 distanceFromQuarter = ((Int64) (((Int64) (quarterPeriod)) - distanceFromHalf));
						Double relativeDistance = (1.0 * distanceFromQuarter) / quarterPeriod;
						return ((Byte) (Math.Round(relativeDistance * Amplitude) + 128));
					}
					case WaveType.Pulse:
					{
						Int64 point = ((Int64) Math.Round(TimeStamp / DeltaTime)) % Lambda;
						return ((Byte) (point == 0 ? (Amplitude+128) : 128));
					}
					default:
						return 0;
				}
			}
		}
	}
}

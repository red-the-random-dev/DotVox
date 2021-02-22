/*
 * Object that allows to create transition between two waves.
 */
using System;

namespace DotVox.Wave
{
	/// <summary>
	/// Smooth transition between two waves.
	/// </summary>
	public class Transition : ITimedWave, IWaveExtract
	{
		readonly Double FadeTime;
		readonly Double FirstStart;
		readonly Double SecondStart;
		readonly ITimedWave FirstWave;
		readonly ITimedWave SecondWave;
		
		public Transition(ITimedWave first, ITimedWave second, Double fadeTime, Double firstStart = 0.0, Double secondStart = 0.0)
		{
			FadeTime = fadeTime;
			FirstWave = first;
			SecondWave = second;
			FirstStart = firstStart;
			SecondStart = secondStart;
		}
		
		public Double DeltaTime
		{
			get
			{
				return Math.Min(FirstWave.DeltaTime, SecondWave.DeltaTime);
			}
		}
		
		public Double PlayTime
		{
			get
			{
				return FadeTime;
			}
		}
		
		public ITimedWave Wave
		{
			get
			{
				return this;
			}
		}
		
		public Boolean Blank
		{
			get
			{
				return false;
			}
		}
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				if (TimeStamp > PlayTime)
				{
					return this[TimeStamp - PlayTime];
				}
				else if (TimeStamp < 0)
				{
					return this[TimeStamp + PlayTime];
				}
				
				Double FirstWaveVolume = (PlayTime - TimeStamp) / PlayTime;
				Double SecondWaveVolume = (TimeStamp) / PlayTime;
				
				SByte firstElement = ((SByte) ((FirstWave[FirstStart + TimeStamp] - 128) * FirstWaveVolume));
				SByte secondElement = ((SByte) ((SecondWave[SecondStart + TimeStamp] - 128) * SecondWaveVolume));
				
				Byte final = ((Byte) (firstElement + secondElement + 128));
				return final;
			}
		}
	}
}

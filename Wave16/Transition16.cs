/*
 * Object that allows to create transition between two waves.
 */
using System;

namespace DotVox.Wave16
{
	/// <summary>
	/// Smooth transition between two waves.
	/// </summary>
	public class Transition16 : ITimedWave16, IWaveExtract16
	{
		readonly Double FadeTime;
		readonly Double FirstStart;
		readonly Double SecondStart;
		readonly ITimedWave16 FirstWave;
		readonly ITimedWave16 SecondWave;
		
		public Transition16(ITimedWave16 first, ITimedWave16 second, Double fadeTime, Double firstStart = 0.0, Double secondStart = 0.0)
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
		
		public ITimedWave16 Wave
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
		
		public Int16 this[Double TimeStamp]
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
				
				Int16 firstElement = ((SByte) ((FirstWave[FirstStart + TimeStamp]) * FirstWaveVolume));
				Int16 secondElement = ((SByte) ((SecondWave[SecondStart + TimeStamp]) * SecondWaveVolume));
				
				Int16 final = ((Int16) (firstElement + secondElement));
				return final;
			}
		}
	}
}

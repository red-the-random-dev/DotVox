/* 
 * Created by SharpDevelop.
 * Date: 23.02.2021
 * Time: 18:03
 */
using System;

using DotVox.Wave16;

namespace DotVox.Riffs16
{
	/// <summary>
	/// Creates a parabolic fadeout for wave object.
	/// </summary>
	public class Fadeout16 : ITimedWave16
	{
		readonly ITimedWave16 WrappedWave;
		Boolean _skipIntro;
		Double _reduction;
		Double _beginningOffset;
		
		public Boolean SkipIntro
		{
			get
			{
				return _skipIntro;
			}
			set
			{
				_skipIntro = value;
			}
		}
		
		public Double Reduction
		{
			get
			{
				return _reduction;
			}
			set
			{
				try
				{
					Double attempt = 1.0 / value;
				}
				catch (Exception e)
				{
					throw new ArgumentException(String.Format("Cannot set reduction multiplier to given value: {0}.", value), e);
				}
				_reduction = value;
			}
		}
		
		public Double BeginningOffset
		{
			get
			{
				return _beginningOffset;
			}
			set
			{
				_beginningOffset = value;
			}
		}
		
		public Fadeout16(ITimedWave16 target, Double reductionMultiplier = 1.0, Double beginningOffset = 0.0, Boolean increaseAtStart = false)
		{
			WrappedWave = target;
			Reduction = reductionMultiplier;
			BeginningOffset = beginningOffset;
			SkipIntro = !increaseAtStart;
		}
		
		public Double DeltaTime
		{
			get
			{
				return WrappedWave.DeltaTime;
			}
		}
		
		public Int16 this[Double TimeStamp]
		{
			get
			{
				Double multiplier;
				Double volumeDivider = (Reduction * TimeStamp + BeginningOffset);
				
				if (volumeDivider == 0.0)
				{
					multiplier = 1.0;
				}
				else if (volumeDivider < 1.0 && SkipIntro)
				{
					return WrappedWave[TimeStamp];
				}
				else
				{
					multiplier = 1.0 / volumeDivider;
				}
				
				Int16 Sound = WrappedWave[TimeStamp];
				Sound = ((Int16) Math.Round(Sound * multiplier));
				
				return (Int16) Sound;
			}
		}
	}
}

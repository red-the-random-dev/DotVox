/* 
 * Created by SharpDevelop.
 * Date: 23.02.2021
 * Time: 18:03
 */
using System;

using DotVox.Wave;

namespace DotVox.Riffs
{
	/// <summary>
	/// Creates a parabolic fadeout for wave object.
	/// </summary>
	public class Fadeout : ITimedWave
	{
		readonly ITimedWave WrappedWave;
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
		
		public Fadeout(ITimedWave target, Double reductionMultiplier = 1.0, Double beginningOffset = 0.0, Boolean increaseAtStart = false)
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
		
		public Byte this[Double TimeStamp]
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
				
				Int32 Sound = WrappedWave[TimeStamp] - 128;
				Sound = ((Int32) Math.Round(Sound * multiplier));
				Sound += 128;
				Sound = Math.Min(Sound, 255);
				Sound = Math.Max(Sound, 0);
				
				return (Byte) Sound;
			}
		}
	}
}

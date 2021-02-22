/*
 * Definition of conjoined wave.
 */
using System;
using System.Collections.Generic;

namespace DotVox.Wave16
{
	/// <summary>
	/// Consecutive array of waves.
	/// </summary>
	[Serializable]
	public class ConjoinedWave16 : ITimedWave16
	{
		Double dTime = 1.0;
		UInt32[] AbsoluteLambdaReferences;
		UInt32 PeriodLength = 0;
		List<ITimedWave16> WaveArray = new List<ITimedWave16>();
		
		/// <summary>
		/// Construct the consecutive array of waves.
		/// </summary>
		/// <param name="waves">Wave array.</param>
		/// <param name="segmentationLimits">Length of each segments taken out of wave in bytes.</param>
		public ConjoinedWave16
		(
			ITimedWave16[] waves,
			UInt32[] segmentationLimits
		)
		{
			if (waves.Length != segmentationLimits.Length)
			{
				throw new ArgumentException("Amount of waves and segmentation limits are unequal.");
			}
			
			foreach (ITimedWave16 i in waves)
			{
				WaveArray.Add(i);
				dTime = Math.Min(this.DeltaTime, i.DeltaTime);
			}
			List<UInt32> s_lims = new List<uint>();
			s_lims.Add(0);
			foreach (UInt32 x in segmentationLimits)
			{
				UInt32 Last = s_lims[s_lims.Count-1];
				PeriodLength += x;
				s_lims.Add(Last + x);
			}
			AbsoluteLambdaReferences = s_lims.ToArray();
		}
		
		/// <summary>
		/// Minimal fraction of time available for the indexer.
		/// </summary>
		public Double DeltaTime
		{
			get
			{
				return dTime;
			}
		}
		
		ITimedWave16 PickElement(UInt32 Tick, ref UInt32 RelativeTick)
		{
			for (Int32 i = 0; i < AbsoluteLambdaReferences.Length-1; i++)
			{
				if (Tick >= AbsoluteLambdaReferences[i] && Tick < AbsoluteLambdaReferences[i+1])
				{
					RelativeTick = Tick - AbsoluteLambdaReferences[i];
					return this.WaveArray[i];
				}
			}
			throw new ArgumentOutOfRangeException("Could not find fitting wave on given lambda segment.");
		}
		
		Int16 ReceiveVoltage(UInt32 Tick)
		{
			UInt32 RelativeTick = 0;
			ITimedWave16 x = PickElement(Tick, ref RelativeTick);
			return x[RelativeTick * DeltaTime];
		}
		
		/// <summary>
		/// 8-bit voltage value for given timestamp.
		/// </summary>
		public Int16 this[Double TimeStamp]
		{
			get
			{
				UInt32 Tick = ((UInt32) (Math.Round(TimeStamp / DeltaTime) % PeriodLength));
				return ReceiveVoltage(Tick);
			}
		}
	}
}

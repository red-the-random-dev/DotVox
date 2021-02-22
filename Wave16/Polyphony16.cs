/*
 * Created by SharpDevelop.
 * Date: 10.02.2021
 * Time: 20:55
 */
using System;
using System.Collections.Generic;

namespace DotVox.Wave16
{
	/// <summary>
	/// Class that conjoins several waves together.
	/// </summary>
	[Serializable]
	public class Polyphony16 : ITimedWaveArray16
	{
		public List<ITimedWave16> WaveSet = new List<ITimedWave16>();
		Double dtime = 1.0;
		public readonly UInt16 BitsPerSample;
		
		public static Polyphony16 operator + (Polyphony16 x, ITimedWave16 y)
		{
			if (x.WaveSet.ToArray().Length * 8 == x.BitsPerSample)
			{
				throw new ArgumentOutOfRangeException("Limit of joinable oscillations exceeded.");
			}
			x.WaveSet.Add(y);
			x.dtime = Math.Min(x.dtime, y.DeltaTime);
			return x;
		}
		
		public Polyphony16(ITimedWave16 BaseWave, UInt16 MaxSampleSize = 64)
		{
			WaveSet.Add(BaseWave);
			BitsPerSample = (UInt16) (MaxSampleSize - (MaxSampleSize % 8));
			dtime = BaseWave.DeltaTime;
		}
		
		public Double DeltaTime
		{
			get
			{
				return dtime;
			}
		}
		
		public Int16[] this[Double TimeStamp]
		{
			get
			{
				ITimedWave16[] Waves = WaveSet.ToArray();
				Int16[] Output = new Int16[BitsPerSample / 8];
				for (int i = 0; i < Waves.Length; i++)
				{
					Output[i] = Waves[i][TimeStamp];
				}
				return Output;
			}
		}
	}
}

/*
 * Created by SharpDevelop.
 * User: Lenovo Yoga
 * Date: 10.02.2021
 * Time: 20:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace DotVox.Wave
{
	/// <summary>
	/// Class that conjoins several waves together.
	/// </summary>
	[Serializable]
	public class Polyphony : ITimedWaveArray
	{
		public List<ITimedWave> WaveSet = new List<ITimedWave>();
		Double dtime = 1.0;
		public readonly UInt16 BitsPerSample;
		
		public static Polyphony operator + (Polyphony x, ITimedWave y)
		{
			if (x.WaveSet.ToArray().Length * 8 == x.BitsPerSample)
			{
				throw new ArgumentOutOfRangeException("Limit of joinable oscillations exceeded.");
			}
			x.WaveSet.Add(y);
			x.dtime = Math.Min(x.dtime, y.DeltaTime);
			return x;
		}
		
		public Polyphony(ITimedWave BaseWave, UInt16 MaxSampleSize = 64)
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
		
		public Byte[] this[Double TimeStamp]
		{
			get
			{
				ITimedWave[] Waves = WaveSet.ToArray();
				Byte[] Output = new Byte[BitsPerSample / 8];
				for (int i = 0; i < Waves.Length; i++)
				{
					Output[i] = Waves[i][TimeStamp];
				}
				return Output;
			}
		}
	}
}

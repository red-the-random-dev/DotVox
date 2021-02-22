/*
 * Class for converting wave arrays into single wave.
 */
using System;

namespace DotVox.Wave16
{
	/// <summary>
	/// Used to summarize wave arrays into singular output.
	/// </summary>
	[Serializable]
	public class Mixer16 : ITimedWave16
	{
		ITimedWaveArray16 MixTarget;
		
		public Mixer16(ITimedWaveArray16 inputWave)
		{
			MixTarget = inputWave;
		}
		
		public Double DeltaTime
		{
			get
			{
				return MixTarget.DeltaTime;
			}
		}
		
		public Int16 this[Double TimeStamp]
		{
			get
			{
				Int16[] SourceArray = MixTarget[TimeStamp];
				Int32[] ConvertableArray = new Int32[SourceArray.Length];
				for (int i = 0; i < SourceArray.Length; i++)
				{
					ConvertableArray[i] = SourceArray[i];
				}
				
				Int32 Sum = 0;
				foreach (Int32 i in ConvertableArray)
				{
					Sum += i;
				}
				
				Sum /= ConvertableArray.Length;
				Sum %= 256;
				return ((Int16) Sum);
			}
		}
	}
}

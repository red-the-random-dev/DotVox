/*
 * Class for converting wave arrays into single wave.
 */
using System;

namespace DotVox.Wave
{
	/// <summary>
	/// Used to summarize wave arrays into singular output.
	/// </summary>
	[Serializable]
	public class Mixer : ITimedWave
	{
		ITimedWaveArray MixTarget;
		public readonly Byte Center;
		
		public Mixer(ITimedWaveArray inputWave, Byte center = 128)
		{
			MixTarget = inputWave;
			Center = center;
		}
		
		public Double DeltaTime
		{
			get
			{
				return MixTarget.DeltaTime;
			}
		}
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				Byte[] SourceArray = MixTarget[TimeStamp];
				Int32[] ConvertableArray = new Int32[SourceArray.Length];
				for (int i = 0; i < SourceArray.Length; i++)
				{
					ConvertableArray[i] = SourceArray[i] - Center;
				}
				
				Int32 Sum = 0;
				foreach (Int16 i in ConvertableArray)
				{
					Sum += i;
				}
				
				Sum /= ConvertableArray.Length;
				Sum += Center;
				Sum %= 256;
				return ((Byte) Sum);
			}
		}
	}
}

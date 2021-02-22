/* 
 * Created by SharpDevelop.
 * Date: 22.02.2021
 * Time: 14:13
 */
using System;
using DotVox.Wave;
using DotVox.Wave16;

namespace DotVox.Resampling.Depth
{
	/// <summary>
	/// Compresses 16-bit waves into 8-bit waves.
	/// </summary>
	[Serializable]
	public class WaveDowncast : ITimedWave
	{
		readonly ITimedWave16 EmbeddedWave;
		
		public WaveDowncast(ITimedWave16 source)
		{
			EmbeddedWave = source;
		}
		
		public Double DeltaTime
		{
			get
			{
				return EmbeddedWave.DeltaTime;
			}
		}
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				Int32 og = (EmbeddedWave[TimeStamp]) / 256;
				og += 128;
				og = Math.Max(og, 0);
				og = Math.Min(og, 255);
				return ((Byte) og);
			}
		}
	}
}

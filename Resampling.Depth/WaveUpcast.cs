/* 
 * Created by SharpDevelop.
 * Date: 22.02.2021
 * Time: 13:58
 */
using System;

using DotVox.Wave;
using DotVox.Wave16;

namespace DotVox.Resampling.Depth
{
	/// <summary>
	/// Converts 8-bit waves to 16-bit waves.
	/// </summary>
	[Serializable]
	public class WaveUpcast : ITimedWave16
	{
		readonly ITimedWave EmbeddedWave;
		
		public WaveUpcast(ITimedWave source)
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
		
		public Int16 this[Double TimeStamp]
		{
			get
			{
				Int32 og = (EmbeddedWave[TimeStamp] - 128) * 256;
				return ((Int16) og);
			}
		}
	}
}
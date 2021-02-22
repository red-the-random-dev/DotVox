/*
 * Definition of wave compressor class.
 */
using System;

namespace DotVox.Wave16
{
	/// <summary>
	/// Wave wrapper that narrows variety of wave's vertical values.
	/// </summary>
	public class VerticalCompressor16 : ITimedWave16
	{
		Double comprate;
		ITimedWave16 source;
		
		public Double CompressionRate
		{
			get
			{
				return comprate;
			}
			set
			{
				comprate = Math.Abs(value);
			}
		}
		
		public ITimedWave16 SourceWave
		{
			get
			{
				return source;
			}
			private set
			{
				source = value;
			}
		}
		
		public VerticalCompressor16(ITimedWave16 subWave, Double compressionRate)
		{
			SourceWave = subWave;
			CompressionRate = compressionRate;
		}
		
		public Double DeltaTime
		{
			get
			{
				return SourceWave.DeltaTime;
			}
		}
		
		public Int16 this[Double TimeStamp]
		{
			get
			{
				Int16 vertice = ((Int16) (SourceWave[TimeStamp]));
				vertice = ((Int16) (Math.Ceiling(vertice/CompressionRate) * CompressionRate));
				return vertice;
			}
		}
	}
}

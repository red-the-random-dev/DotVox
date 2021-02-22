/*
 * Definition of wave compressor class.
 */
using System;

namespace DotVox.Wave
{
	/// <summary>
	/// Wave wrapper that narrows variety of wave's vertical values.
	/// </summary>
	public class VerticalCompressor : ITimedWave
	{
		Double comprate;
		ITimedWave source;
		
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
		
		public ITimedWave SourceWave
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
		
		public VerticalCompressor(ITimedWave subWave, Double compressionRate)
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
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				SByte vertice = ((SByte) (SourceWave[TimeStamp] - 128));
				vertice = ((SByte) (Math.Ceiling(vertice/CompressionRate) * CompressionRate));
				return ((Byte) (vertice+128));
			}
		}
	}
}

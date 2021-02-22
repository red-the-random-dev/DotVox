/*
 * Created by SharpDevelop.
 * Date: 10.02.2021
 * Time: 21:15
 */
using System;

namespace DotVox.Wave16
{
	/// <summary>
	/// Base type for all time-dependent 16-bit waves.
	/// </summary>
	public interface ITimedWave16
	{
		Int16 this[Double TimeStamp]
		{
			get;
		}
		
		/// <summary>
		/// Minimal fraction of time available for indexer.
		/// </summary>
		Double DeltaTime
		{
			get;
		}
	}
	
	/// <summary>
	/// Base time for polyphonic objects.
	/// </summary>
	public interface ITimedWaveArray16
	{
		Int16[] this[Double TimeStamp]
		{
			get;
		}
		
		/// <summary>
		/// Minimal fraction of time available for indexer.
		/// </summary>
		Double DeltaTime
		{
			get;
		}
	}
	
	public interface IWaveExtract16
	{
		Double PlayTime
		{
			get;
		}
		
		ITimedWave16 Wave
		{
			get;
		}
		
		Boolean Blank
		{
			get;
		}
	}
	
	public interface IWaveArrayExtract16
	{
		Double PlayTime
		{
			get;
		}
		
		ITimedWaveArray16 WaveArray
		{
			get;
		}
	}
}

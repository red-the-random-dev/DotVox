/*
 * Created by SharpDevelop.
 * User: Lenovo Yoga
 * Date: 10.02.2021
 * Time: 21:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DotVox.Wave
{
	/// <summary>
	/// Base type for all time-dependent waves.
	/// </summary>
	public interface ITimedWave
	{
		Byte this[Double TimeStamp]
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
	public interface ITimedWaveArray
	{
		Byte[] this[Double TimeStamp]
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
	
	public interface IWaveExtract
	{
		Double PlayTime
		{
			get;
		}
		
		ITimedWave Wave
		{
			get;
		}
		
		Boolean Blank
		{
			get;
		}
	}
	
	public interface IWaveArrayExtract
	{
		Double PlayTime
		{
			get;
		}
		
		ITimedWaveArray WaveArray
		{
			get;
		}
	}
}

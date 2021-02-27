/* 
 * Created by SharpDevelop.
 * Date: 27.02.2021
 * Time: 12:49
 */
using System;
using DotVox.Wave16;

namespace DotVox.Riffs16
{
	/// <summary>
	/// Implementation of wave segment object class that allows to refer to wave with certain delay.
	/// </summary>
	public class Extract16 : ITimedWave16, IWaveExtract16
	{
		/// <summary>
		/// Receives unhandled wave that is not affected by segmentation or beginning delay.
		/// </summary>
		public ITimedWave16 RawWave { get; private set; }
		/// <summary>
		/// Implicit upcast to basic timed wave type.
		/// </summary>
		public ITimedWave16 Wave {get { return this; }}
		/// <summary>
		/// Determines how long this wave should play.
		/// </summary>
		public Double PlayTime { get; private set; }
		/// <summary>
		/// Determines if extract is blank or not. Used by Sequencer.
		/// </summary>
		public Boolean Blank { get; private set; }
		/// <summary>
		/// Determines beginning offset.
		/// </summary>
		public Double BeginningOffset { get; private set; }
		/// <summary>
		/// Extract length in ticks.
		/// </summary>
		public UInt32 Length
		{
			get
			{
				return (UInt32) Math.Round(PlayTime / DeltaTime);
			}
		}
		/// <summary>
		/// Duration of single tick. Determined by RawWave's deltatime.
		/// </summary>
		public Double DeltaTime
		{
			get
			{
				return RawWave.DeltaTime;
			}
		}
		/// <summary>
		/// Receive waveform voltage that corresponds to given time stamp.
		/// </summary>
		public Int16 this[Double TimeStamp]
		{
			get
			{
				if (Blank)
				{
					return 0;
				}
				return RawWave[TimeStamp+BeginningOffset];
			}
		}
		/// <summary>
		/// Instantiates Extract object.
		/// </summary>
		/// <param name="targetWave">Raw wave.</param>
		/// <param name="playTime">Determines how long wave will play.</param>
		/// <param name="beginningOffset">Determines playback offset.</param>
		public Extract16(ITimedWave16 targetWave, Double playTime, Double beginningOffset = 0.0)
		{
			RawWave = targetWave;
			PlayTime = playTime;
			Blank = false;
			BeginningOffset = beginningOffset;
		}
		/// <summary>
		/// Instantiates blank wave extract.
		/// </summary>
		/// <param name="playTime">Determines how long this skip will last.</param>
		public Extract16(Double playTime)
		{
			RawWave = null;
			PlayTime = playTime;
			Blank = true;
			BeginningOffset = 0.0;
		}
	}
}

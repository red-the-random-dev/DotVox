/*
 * Wave object for amplifying timed waves.
 */
using System;

namespace DotVox.Wave16
{
	/// <summary>
	/// Wave wrapper that increases source wave amplitude by given value.
	/// </summary>
	[Serializable]
	public class AmplifiedWave16 : ITimedWave16
	{
		readonly ITimedWave16 DefaultWave;
		public readonly Double AmplificationLevel;
		
		public AmplifiedWave16(ITimedWave16 defaultWave, Double amplification)
		{
			DefaultWave = defaultWave;
			AmplificationLevel = amplification;
		}
		
		public Double DeltaTime
		{
			get
			{
				return DefaultWave.DeltaTime;
			}
		}
		
		public Int16 this[Double TimeStamp]
		{
			get
			{
				Int32 Intake = DefaultWave[TimeStamp];
				Intake = ((Int32) Math.Round(Intake * AmplificationLevel));
				Intake = Math.Max(Intake, Int16.MinValue);
				Intake = Math.Min(Intake, Int16.MaxValue);
				return ((Int16) Intake);
			}
		}
	}
}

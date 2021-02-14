/*
 * Wave object for amplifying timed waves.
 */
using System;

namespace DotVox.Wave
{
	/// <summary>
	/// Wave wrapper that increases source wave amplitude by given value.
	/// </summary>
	[Serializable]
	public class AmplifiedWave : ITimedWave
	{
		readonly ITimedWave DefaultWave;
		public readonly Byte Center;
		public readonly Double AmplificationLevel;
		
		public AmplifiedWave(ITimedWave defaultWave, Double amplification, Byte center = 128)
		{
			DefaultWave = defaultWave;
			Center = center;
			AmplificationLevel = amplification;
		}
		
		public Double DeltaTime
		{
			get
			{
				return DefaultWave.DeltaTime;
			}
		}
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				Int32 Intake = DefaultWave[TimeStamp] - Center;
				Intake = ((Int32) Math.Round(Intake * AmplificationLevel));
				Intake += Center;
				Intake = Math.Max(Intake, 0);
				Intake = Math.Min(Intake, 255);
				return ((Byte) Intake);
			}
		}
	}
}

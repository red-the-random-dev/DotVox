/* 
 * Created by SharpDevelop.
 * Date: 24.02.2021
 * Time: 21:49
 */
using System;

namespace DotVox.Riffs
{
	/// <summary>
	/// Static class for receiving standard instrumental tones.
	/// </summary>
	public static class Tones
	{
		/// <summary>
		/// Tone of note C of zeroth octave.
		/// </summary>
		public static Double C_0 { get { return 16.35; }}
		
		/// <summary>
		/// Receive the key of required tone. Formula: f(i) = f_0 * (2 ^ ((oct * 12) + i / 12)), where i - needed key, oct - octave, f_0 = 16.35
		/// </summary>
		/// <param name="key">Needed key. 0 is C, 1 is C#/Db, ...</param>
		/// <param name="octave">Needed octave.</param>
		/// <returns>Frequency that corresponds to needed tone.</returns>
		public static UInt32 Receive(Int32 key, Byte octave)
		{
			Double note_New = C_0 * Math.Pow(2, ((1.0 * octave * 12) + key)/12.0);
			return (UInt32) Math.Round(note_New);
		}
		
		/// <summary>
		/// Shifts frequency by needed amount of semitones.
		/// f(x,i) = x*(2^(i/12))
		/// </summary>
		/// <param name="target">Base frequency.</param>
		/// <param name="semitones">Amount of semitones for shifting.</param>
		/// <returns>A shifted frequency.</returns>
		public static UInt32 Shift(UInt32 target, Int32 semitones)
		{
			return (UInt32) Math.Round(1.0 * target * Math.Pow(2, (1.0 * semitones / 12.0)));
		}
	}
}

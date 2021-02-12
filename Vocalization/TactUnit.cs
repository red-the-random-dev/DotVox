/*
 * Struct that represents wave extract.
 */
using System;
using DotVox.Wave;

namespace DotVox.Vocalization
{
	/// <summary>
	/// Description of TactUnit.
	/// </summary>
	public struct TactUnit : IEquatable<TactUnit>, IWaveExtract
	{
		ITimedWave in_wave;
		Double playtime;
		Boolean blank;
		
		public TactUnit(ITimedWave sound, Double timing)
		{
			in_wave = sound;
			playtime = timing;
			blank = false;
		}
		
		public TactUnit(Double timing)
		{
			in_wave = new Oscillation(1, 0, WaveType.Sine, 800);
			playtime = timing;
			blank = true;
		}
		
		public TactUnit(String phoneme, Note note, Byte octave, Double timing, UInt32 sampleRate = 44100, SByte amplitude = 64)
		{
			in_wave = Phonemes.Get(phoneme, note, octave, amplitude, sampleRate);
			playtime = timing;
			blank = false;
		}
		
		public Boolean Blank
		{
			get
			{
				return blank;
			}
		}
		
		public ITimedWave Wave
		{
			get
			{
				return in_wave;
			}
		}
		public Double PlayTime
		{
			get
			{
				return playtime;
			}
		}
		
		public void ForceInto(Sequencer sq)
		{
			if (!this.blank)
			{
				sq.Push(this.Wave, this.PlayTime);
			}
			else
			{
				sq.Skip(this.PlayTime);
			}
		}
		
		#region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<TactUnit>" declaration.
		
		public override bool Equals(object obj)
		{
			if (obj is TactUnit)
				return Equals((TactUnit)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(TactUnit other)
		{
			// add comparisions for all members here
			return this.playtime == other.playtime;
		}
		
		public override int GetHashCode()
		{
			// combine the hash codes of all members here (e.g. with XOR operator ^)
			return playtime.GetHashCode();
		}
		
		public static bool operator ==(TactUnit left, TactUnit right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(TactUnit left, TactUnit right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}

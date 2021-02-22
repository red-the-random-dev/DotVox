/*
 * Struct that represents wave extract.
 */
using System;
using DotVox.Wave;

namespace DotVox.Vocalization
{
	/// <summary>
	/// Wave object with specified start offset.
	/// </summary>
	[Serializable]
	public struct OffsetGenerator : ITimedWave
	{
		Double offset;
		ITimedWave internalWave;
		
		public Double DeltaTime
		{
			get
			{
				return internalWave.DeltaTime;
			}
		}
		
		public Byte this[Double TimeStamp]
		{
			get
			{
				return internalWave[TimeStamp+offset];
			}
		}
		
		public Double Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}
		
		public OffsetGenerator(ITimedWave wave, Double startOffset)
		{
			internalWave = wave;
			offset = startOffset;
		}
	}
	
	/// <summary>
	/// Wave object carrier with specified play time limit. Designed for use with DotVox.Wave.Sequencer.
	/// </summary>
	[Serializable]
	public struct TactUnit : IEquatable<TactUnit>, IWaveExtract
	{
		ITimedWave in_wave;
		Double beginningOffset;
		Double playtime;
		Boolean blank;
		UInt32 frequency;
		String phoneme;
		
		public UInt32 Frequency
		{
			get
			{
				return Frequency;
			}
		}
		
		public String Phoneme
		{
			get
			{
				return phoneme;
			}
		}
		
		public TactUnit(ITimedWave sound, Double timing)
		{
			in_wave = sound;
			phoneme = null;
			playtime = timing;
			blank = false;
			frequency = 0;
			beginningOffset = 0.0;
		}
		
		public TactUnit(Double timing)
		{
			in_wave = new Oscillation(1, 0, WaveType.Sine, 800);
			playtime = timing;
			phoneme = null;
			blank = true;
			frequency = 1;
			beginningOffset = 0.0;
		}
		
		public TactUnit(String phonemeString, Note note, Byte octave, Double timing, UInt32 sampleRate = 44100, SByte amplitude = 64)
		{
			in_wave = Phonemes.Get(phonemeString, note, octave, amplitude, sampleRate);
			phoneme = phonemeString;
			playtime = timing;
			blank = false;
			frequency = Notes.Get(note, octave);
			beginningOffset = 0.0;
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
				return new OffsetGenerator(in_wave, beginningOffset);
			}
		}
		public Double PlayTime
		{
			get
			{
				return playtime;
			}
		}
		public Double BeginningOffset
		{
			get
			{
				return beginningOffset;
			}
			set
			{
				beginningOffset = value;
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
			return (this.playtime == other.playtime && (this.frequency == other.frequency && this.frequency != 0) && this.blank == other.blank && this.Phoneme == other.Phoneme);
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

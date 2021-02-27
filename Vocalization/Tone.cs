/* 
 * Created by SharpDevelop.
 * Date: 22.02.2021
 * Time: 21:22
 */
using System;

namespace DotVox.Vocalization
{
	/// <summary>
	/// Struct that contains information about key and octave.
	/// </summary>
	public struct Tone : IEquatable<Tone>
	{
		public Note Key;
		public Byte Octave;
		
		/// <summary>
		/// Private function declared for tone shifting purposes.
		/// </summary>
		/// <returns>Number of key.</returns>
		/// <exception cref="System.InvalidCastException">Unable to find correct declaration for key.</exception>
		Byte NumericInterpretation()
		{
			switch (Key)
			{
				case Note.C:
					return 0;
				case Note.Db:
					return 1;
				case Note.D:
					return 2;
				case Note.Eb:
					return 3;
				case Note.E:
					return 4;
				case Note.F:
					return 6;
				case Note.Gb:
					return 7;
				case Note.G:
					return 8;
				case Note.Ab:
					return 9;
				case Note.A:
					return 10;
				case Note.Bb:
					return 11;
				case Note.B:
					return 12;
				default:
					throw new InvalidCastException("Unable to find valid key number value.");
			}
		}
		
		/// <summary>
		/// Instantiate new struct of Tone type.
		/// </summary>
		/// <param name="key">One of standard keys.</param>
		/// <param name="octave">Octave selected key is located in.</param>
		public Tone(Note key, Byte octave)
		{
			Key = key;
			Octave = octave;
		}
		
		/// <summary>
		/// Shifts tone by one key.
		/// </summary>
		/// <param name="target">Target tone object.</param>
		/// <returns></returns>
		public static Tone operator ++(Tone target)
		{
			SByte keyNumber = ((SByte) target.NumericInterpretation());
			keyNumber += 2;
			
			if (keyNumber >= 13)
			{
				target.Octave++;
			}
			keyNumber %= 13;
			if (keyNumber == 5)
			{
				keyNumber++;
			}
			
			Note newNote = (keyNumber < 5 ? ((Note) keyNumber) : ((Note) (keyNumber-1)));
			target.Key = newNote;
			return target;
		}
		
		#region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Tone>" declaration.
		
		public override bool Equals(object obj)
		{
			if (obj is Tone)
				return Equals((Tone)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(Tone other)
		{
			// add comparisions for all members here
			return (this.Key == other.Key) && (this.Octave == other.Octave);
		}
		
		public void Export(out Note key, out Byte octave)
		{
			key = Key;
			octave = Octave;
		}
		
		public void GetFrequency(out UInt32 frequency)
		{
			frequency = this.Frequency;
		}
		
		public UInt32 Frequency
		{
			get
			{
				return Notes.Get(this.Key, this.Octave);
			}
		}
		
		public override int GetHashCode()
		{
			// combine the hash codes of all members here (e.g. with XOR operator ^)
			return (Key.GetHashCode() ^ Octave.GetHashCode());
		}
		
		public static bool operator ==(Tone left, Tone right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(Tone left, Tone right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}

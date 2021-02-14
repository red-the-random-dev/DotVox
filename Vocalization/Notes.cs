/*
 * Created by SharpDevelop.
 * User: Lenovo Yoga
 * Date: 11.02.2021
 * Time: 15:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DotVox.Vocalization
{
	// Enumeration of notes.
	public enum Note {C = 0, Db = 1, D = 2, Eb = 3, E = 4, F = 5, Gb = 6, G = 7, Ab = 8, A = 9, Bb = 10, B = 11};
	/// <summary>
	/// Description of Notes.
	/// </summary>
	public static class Notes
	{
		static Double[] NoteBases = new Double[] {16.35, 17.32, 18.35, 19.45, 20.60, 21.83, 23.12, 24.50, 25.96, 27.50, 29.14, 30.87};
		static String[] NoteStrings = new String[] {"C_", "Db", "D_", "Eb", "E_", "F_", "Gb", "G_", "Ab", "A_", "Bb", "B_"};
		
		/// <summary>
		/// Find a frequency of needed note.
		/// </summary>
		/// <param name="note">One of notes.</param>
		/// <param name="octave">Number of octave.</param>
		/// <returns>Note's frequency.</returns>
		public static UInt32 Get(Note note, Byte octave)
		{
			return ((UInt32) Math.Round(NoteBases[(Int32) note] * Math.Pow(2, ((double) (octave)))));
		}
		
		public static String StringForm(Note note, Byte octave)
		{
			return String.Format("{0}{1}", NoteStrings[(Int32) note], octave);
		}
		
		public static String StringForm(Note note)
		{
			return NoteStrings[(Int32) note];
		}
	}
}

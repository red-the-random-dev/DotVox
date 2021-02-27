/* 
 * Created by SharpDevelop.
 * Date: 27.02.2021
 * Time: 13:48
 */
using System;
using System.Collections.Generic;

using DotVox.Riffs16;

namespace DotVox.Composition16
{
	/// <summary>
	/// Used for configuring the composer's keyboard layout.
	/// </summary>
	[Serializable]
	public class KeyboardConfig
	{
		readonly UInt32[] FrequencyMap;
		
		public KeyboardConfig(UInt32[] freqs)
		{
			if (freqs.Length == 0)
			{
				throw new ArgumentException("Received empty frequency map.");
			}
			
			foreach (UInt32 i in freqs)
			{
				if (i == 0)
				{
					throw new ArgumentException("Received frequency map with one of frequencies being equal to 0.");
				}
			}
			FrequencyMap = freqs;
		}
		
		public UInt32 this[Int32 Index]
		{
			get
			{
				return FrequencyMap[Index];
			}
		}
		
		public Int32 Length
		{
			get
			{
				return FrequencyMap.Length;
			}
		}
		
		public static KeyboardConfig Default
		{
			get
			{
				List<UInt32> keys = new List<uint>();
				
				for (Byte x = 3; x <= 9; x++)
				{
					for (int i = 0; i < 12; i++)
					{
						keys.Add(Tones16.Receive(i, x));
					}
				}
				
				return new KeyboardConfig(keys.ToArray());
			}
		}
	}
}
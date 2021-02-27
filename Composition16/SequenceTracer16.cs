/* 
 * Created by SharpDevelop.
 * Date: 27.02.2021
 * Time: 14:49
 */
using System;
using System.Collections.Generic;
using DotVox.Wave16;
using DotVox.Riffs16;

namespace DotVox.Composition16
{
	/// <summary>
	/// Description of SequenceTracer16.
	/// </summary>
	public class SequenceTracer16
	{
		readonly KeyboardConfig KeyMap;
		List<List<Extract16>> Channels = new List<List<Extract16>>();
		public readonly Double TactTime;
		readonly Dictionary<String, InstrumentConfig16> InstrumentMap;
		
		public SequenceTracer16(KeyboardConfig kcfg, Dictionary<String, InstrumentConfig16> instruments, Double tactTime)
		{
			for (int i = 0; i < kcfg.Length; i++)
			{
				Channels.Add(new List<Extract16>());
			}
			KeyMap = kcfg;
			InstrumentMap = instruments;
			TactTime = tactTime;
		}
		
		public Int32 Length
		{
			get
			{
				Int32 l = 0;
				
				foreach (List<Extract16> i in Channels)
				{
					l = Math.Max(l, i.Count);
				}
				return l;
			}
		}
		
		public void Push(Int32 key, String instrument)
		{
			Double initDelay;
			if (Channels[key].Count == 0)
			{
				initDelay = 0.0;
			}
			else if (Channels[key][Channels[key].Count-1].Blank)
			{
				initDelay = 0.0;
			}
			else
			{
				initDelay = Channels[key][Channels[key].Count-1].BeginningOffset + TactTime;
			}
			
			ITimedWave16 toAdd = InstrumentMap[instrument].GetTimedWave(key, KeyMap);
			Extract16 xtr = new Extract16(toAdd, TactTime, initDelay);
			Channels[key].Add(xtr);
		}
		
		public void PushAnew(Int32 key, String instrument)
		{
			ITimedWave16 toAdd = InstrumentMap[instrument].GetTimedWave(key, KeyMap);
			Extract16 xtr = new Extract16(toAdd, TactTime);
			Channels[key].Add(xtr);
		}
		
		public void Skip(Int32 key)
		{
			Channels[key].Add(new Extract16(TactTime));
		}
		
		public void SkipAll()
		{
			for (int i = 0; i < KeyMap.Length; i++)
			{
				Skip(i);
			}
		}
		
		public void Snap()
		{
			Int32 l = Length;
			for (int i = 0; i < KeyMap.Length; i++)
			{
				List<Extract16> x = Channels[i];
				for (int a = x.Count; a < l; a++)
				{
					Skip(i);
				}
			}
		}
		
		public void Export(Sequencer16 sq16)
		{
			Snap();
			sq16.Snap();
			Int32 l = Length;
			
			for (int i = 0; i < l; i++)
			{
				List<Extract16> lst = new List<Extract16>();
				
				for (int x = 0; x < KeyMap.Length; x++)
				{
					Extract16 ex = Channels[x][i];
					if (!ex.Blank)
					{
						lst.Add(ex);
					}
				}
				
				if (lst.Count == 0)
				{
					sq16.Skip(TactTime);
					continue;
				}
				
				Polyphony16 ph = new Polyphony16(lst[0], (UInt16) (lst.Count * 8));
				
				for (int h = 1; h < lst.Count; h++)
				{
					ph += lst[h];
				}
				
				Mixer16 mx = new Mixer16(ph);
				
				sq16.Push(mx, TactTime);
			}
		}
	}
}

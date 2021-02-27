/*
 * Initialization of sequencer.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace DotVox.Wave16
{
	public static partial class WaveFormatter16
	{
		public static WaveHeader GetSequencerHeader(Sequencer16 x)
		{
			WaveHeader fileData = new WaveHeader();
			fileData.BitsPerSample = 16;
			fileData.BlockAlign = ((UInt16) (x.ChannelAmount * 2));
			fileData.NumChannels = x.ChannelAmount;
			fileData.SampleRate = x.SampleRate;
			fileData.ByteRate = ((UInt32) (x.SampleRate * x.ChannelAmount * 2));
			fileData.Subchunk2Size = ((UInt32) (x.Length * x.ChannelAmount * 2));
			fileData.ChunkSize = 36 + fileData.Subchunk2Size;
			return fileData;
		}
		
		/// <summary>
		/// Loads sequence into binary stream.
		/// </summary>
		/// <param name="x">Sequencer to pick data from.</param>
		/// <param name="target">Target stream.</param>
		public static void WriteSequenceToFile(Sequencer16 x, Stream target)
		{
			WaveHeader fileData = GetSequencerHeader(x);
			BinaryWriter bw = new BinaryWriter(target);
			bw.Write(FormBinaryHeader(fileData));
			foreach (Int16[] i in x.Unload())
			{
				foreach (Int16 i_ in i)
				{
					bw.Write(BitConverter.GetBytes(i_));
				}
			}
		}
	}
	
	/// <summary>
	/// Linear sample array that can be exported into binary stream.
	/// </summary>
	[Serializable]
	public class Sequencer16 : ITimedWaveArray16, IWaveArrayExtract16
	{
		public Boolean Edited { get; private set; }
		public Boolean Sealed { get; private set; }
		
		/// <summary>
		/// Resembles read-only stream with sequencer's data.
		/// </summary>
		public class SequencerStream : Stream
		{
			Int64 Pointer = 0;
			List<Byte> InternalBuffer = new List<Byte>();
			
			public override Boolean CanWrite
			{
				get
				{
					return false;
				}
			}
			
			public override Boolean CanRead
			{
				get
				{
					return true;
				}
			}
			
			public override Int64 Length
			{
				get
				{
					return ((Int64) (InternalBuffer.Count));
				}
			}
			
			public override void Write(Byte[] buffer, Int32 offset, Int32 count)
			{
				throw new NotImplementedException();
			}
			
			public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
			{
				for (int i = offset; i < offset+Math.Min(count, (InternalBuffer.Count - Pointer)); i++)
				{
					buffer[i] = InternalBuffer[((Int32) ((i-offset)+Pointer % 0xf00000000))];
				}
				Pointer += count;
				if (Pointer == InternalBuffer.Count)
				{
					return 0;
				}
				else
				{
					return Math.Min(count, ((Int32) ((InternalBuffer.Count - Pointer) % 0xf00000000)));
				}
			}
			
			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}
			
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}
			
			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotImplementedException();
			}
			
			public override void Flush()
			{
				throw new NotImplementedException();
			}
			
			public override void Close()
			{
				InternalBuffer.Clear();
				base.Close();
			}
			
			public override long Position
			{
				get
				{
					return Pointer;
				}
				set
				{
					Pointer = value;
				}
			}
			
			public SequencerStream(Sequencer16 target)
			{
				foreach (Byte x in WaveFormatter16.FormBinaryHeader(WaveFormatter16.GetSequencerHeader(target)))
				{
					InternalBuffer.Add(x);
				}
				
				foreach (Int16[] a in target.Unload())
				{
					foreach (Int16 b in a)
					{
						Byte[] tr = BitConverter.GetBytes(b);
						InternalBuffer.Add(tr[0]);
						InternalBuffer.Add(tr[1]);
					}
				}
			}
		}
		
		List<List<Int16>> Track = new List<List<Int16>>();
		List<Int16[]> SecondaryTrackBuffer = new List<short[]>();
		public readonly UInt32 SampleRate;
		public readonly UInt16 ChannelAmount;
		
		public ITimedWaveArray16 WaveArray
		{
			get
			{
				return this;
			}
		}
		
		public Double PlayTime
		{
			get
			{
				return DeltaTime * Length;
			}
		}
					
		public Int32 Length
		{
			get
			{
				if (Sealed)
				{
					return SecondaryTrackBuffer.Count;
				}
				Int32 y = 0;
				foreach (List<Int16> x in Track)
				{
					y = Math.Max(y, x.Count);
				}
				return y;
			}
		}
		
		public Double DeltaTime
		{
			get
			{
				return (1.0 / SampleRate);
			}
		}
		
		/// <summary>
		/// Receive a stream that contains current sequencer wave data.
		/// </summary>
		public SequencerStream SnapshotStream
		{
			get
			{
				return new SequencerStream(this);
			}
		}
		
		public Int16[] this[Double TimeStamp]
		{
			get
			{
				if (Length == 0)
				{
					throw new FieldAccessException("Referred sequencer is empty.");
				}
				if (Edited)
				{
					RebuildWaveform();
				}
				
				return SecondaryTrackBuffer[((Int32) Math.Round(TimeStamp / DeltaTime)) % Track.Count];
			}
		}
		
		public Sequencer16(UInt16 channelAmount, UInt32 sampleRate = 44100)
		{
			for (Int32 i = 0; i < channelAmount; i++)
			{
				Track.Add(new List<Int16>());
			}
			
			Sealed = false;
			SampleRate = sampleRate;
			ChannelAmount = channelAmount;
		}
		
		public void Seal()
		{
			RebuildWaveform();
			
			foreach (List<Int16> a in Track)
			{
				a.Clear();
			}
			Track.Clear();
			Sealed = true;
		}
		
		public void Push(ITimedWave16 i, Double time)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			if (ChannelAmount != 1)
			{
				throw new ArgumentException("Used sequencer instance does not support single-channel inputs.");
			}
			
			for (Double x = 0; x < time; x += DeltaTime)
			{
				Track[0].Add(i[x]);
			}
			Edited = true;
		}
		
		public void Push(ITimedWave16 i, Double time, UInt16 channelNumber)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			for (Double x = 0; x < time; x += DeltaTime)
			{
				Track[channelNumber].Add(i[x]);
			}
			Edited = true;
		}
		
		public void Snap()
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			foreach (List<Int16> n in Track)
			{
				for (int i = n.Count; i < Length; i++)
				{
					n.Add(128);
				}
			}
			RebuildWaveform();
		}
		
		public void Push(ITimedWave16[] i, Double Time)
		{
			Snap();
			if (i.Length != ChannelAmount)
			{
				throw new ArgumentException("Mismatch of input array length and channel amount.");
			}
			
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Int16[] newBuffer = new Int16[i.Length];
				for (int j = 0; j < i.Length; j++)
				{
					newBuffer[j] = i[j][x];
				}
				for (int j = 0; j < newBuffer.Length; j++)
				{
					Track[j].Add(newBuffer[j]);
				}
			}
			Edited = true;
		}
		
		public void Push(IWaveExtract16 i, UInt16 channel)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			if (!i.Blank)
			{
				Push(i.Wave, i.PlayTime, channel);
			}
			else
			{
				Skip(i.PlayTime, channel);
			}
			Edited = true;
		}
		
		public void Push(IWaveExtract16 i)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			if (!i.Blank)
			{
				Push(i.Wave, i.PlayTime);
			}
			else
			{
				Skip(i.PlayTime);
			}
			Edited = true;
		}
		
		public void Push(IWaveArrayExtract16 i)
		{
			Push(i.WaveArray, i.PlayTime);
			Edited = true;
		}
		
		public void Push(ITimedWaveArray16 i, Double Time)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Int16[] newBuffer = i[x];
				if (newBuffer.Length != ChannelAmount)
				{
					throw new ArgumentException("Attempted to push data from source with different channel amount.");
				}
				for (int a = 0; a < ChannelAmount; a++)
				{
					Track[a].Add(newBuffer[a]);
				}
			}
			Edited = true;
		}
		
		public void Push(Int16 i)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			if (ChannelAmount != 1)
			{
				throw new ArgumentException("Used sequencer instance does not support single-channel inputs.");
			}
			
			Track[0].Add(i);
			Edited = true;
		}
		
		public void Push(Int16 i, UInt16 channel)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			
			Track[channel].Add(i);
			Edited = true;
		}
		
		public void Push(Int16[] i)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			if (i.Length != ChannelAmount)
			{
				throw new ArgumentException("Mismatch of input array length and channel amount.");
			}
			for (int x = 0; x < ChannelAmount; x++)
			{
				Track[x].Add(i[x]);
			}
			Edited = true;
		}
		
		public void Skip(Double Time)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				for (int i = 0; i < ChannelAmount; i++)
				{
					Track[i].Add(0);
				}
			}
			Edited = true;
		}
		
		public void Skip(Double Time, UInt16 channel)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Track[channel].Add(0);
			}
			Edited = true;
		}
		
		/// <summary>
		/// Rebuild inner secondary buffer.
		/// </summary>
		public void RebuildWaveform()
		{
			if (Sealed)
			{
				return;
			}
			SecondaryTrackBuffer.Clear();
			for (Int32 x = 0; x < Length; x++)
			{
				Int16[] AdderBuffer = new Int16[ChannelAmount];
				for (Int32 i = 0; i < ChannelAmount; i++)
				{
					if (Track[i].Count < Length && x >= Track[i].Count)
					{
						AdderBuffer[i] = 0;
					}
					else
					{
						AdderBuffer[i] = Track[i][x];
					}
				}
				SecondaryTrackBuffer.Add(AdderBuffer);
			}
			Edited = false;
		}
		
		/// <summary>
		/// Forces object to rebuild secondary waveform and unload it as array of bytes.
		/// </summary>
		/// <returns></returns>
		public Int16[][] Unload()
		{
			if (Edited)
			{
				RebuildWaveform();
			}
			
			return SecondaryTrackBuffer.ToArray();
		}
		
		/// <summary>
		/// Unloads secondary buffer's version since last call of Snap() or Unload() method.
		/// </summary>
		/// <returns></returns>
		public Int16[][] SoftUnload()
		{
			return SecondaryTrackBuffer.ToArray();
		}
	}
}

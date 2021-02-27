/*
 * Initialization of sequencer.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace DotVox.Wave
{
	public static partial class WaveFormatter
	{
		public static WaveHeader GetSequencerHeader(Sequencer x)
		{
			WaveHeader fileData = new WaveHeader();
			fileData.BlockAlign = x.ChannelAmount;
			fileData.NumChannels = x.ChannelAmount;
			fileData.SampleRate = x.SampleRate;
			fileData.ByteRate = ((UInt32) (x.SampleRate * x.ChannelAmount));
			fileData.Subchunk2Size = ((UInt32) (x.Length * x.ChannelAmount));
			fileData.ChunkSize = 36 + fileData.Subchunk2Size;
			return fileData;
		}
		
		/// <summary>
		/// Loads sequence into binary stream.
		/// </summary>
		/// <param name="x">Sequencer to pick data from.</param>
		/// <param name="target">Target stream.</param>
		public static void WriteSequenceToFile(Sequencer x, Stream target)
		{
			WaveHeader fileData = GetSequencerHeader(x);
			BinaryWriter bw = new BinaryWriter(target);
			bw.Write(FormBinaryHeader(fileData));
			foreach (Byte[] i in x.Unload())
			{
				bw.Write(i);
			}
		}
	}
	
	/// <summary>
	/// Linear sample array that can be exported into binary stream.
	/// </summary>
	[Serializable]
	public class Sequencer : ITimedWaveArray, IWaveArrayExtract
	{
		public Boolean Edited { get; private set; }
		public Boolean Sealed { get; private set; }
		
		/// <summary>
		/// Resembles read-only stream with sequencer's data.
		/// </summary>
		public class SequencerStream : Stream
		{
			Int64 Pointer = 0;
			List<Byte> InternalBuffer = new List<byte>();
			
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
			
			public SequencerStream(Sequencer target)
			{
				foreach (Byte x in WaveFormatter.FormBinaryHeader(WaveFormatter.GetSequencerHeader(target)))
				{
					InternalBuffer.Add(x);
				}
				
				foreach (Byte[] a in target.Unload())
				{
					foreach (Byte b in a)
					{
						InternalBuffer.Add(b);
					}
				}
			}
		}
		
		List<List<Byte>> Track = new List<List<Byte>>();
		List<Byte[]> SecondaryTrackBuffer = new List<Byte[]>();
		public readonly UInt32 SampleRate;
		public readonly UInt16 ChannelAmount;
		
		public ITimedWaveArray WaveArray
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
				foreach (List<Byte> x in Track)
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
		
		public Byte[] this[Double TimeStamp]
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
		
		public Sequencer(UInt16 channelAmount, UInt32 sampleRate = 44100)
		{
			for (Int32 i = 0; i < channelAmount; i++)
			{
				Track.Add(new List<byte>());
			}
			
			Sealed = false;
			Edited = false;
			SampleRate = sampleRate;
			ChannelAmount = channelAmount;
		}
		
		/// <summary>
		/// Locks sequencer from further editing and frees up primary track buffer, making it easier for further reading.
		/// </summary>
		public void Seal()
		{
			RebuildWaveform();
			foreach (List<Byte> a in Track)
			{
				a.Clear();
			}
			Track.Clear();
			Sealed = true;
		}
		
		public void Push(ITimedWave i, Double time)
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
		
		public void Push(ITimedWave i, Double time, UInt16 channelNumber)
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
			foreach (List<Byte> n in Track)
			{
				for (int i = n.Count; i < Length; i++)
				{
					n.Add(128);
				}
			}
			RebuildWaveform();
		}
		
		public void Push(ITimedWave[] i, Double Time)
		{
			Snap();
			if (i.Length != ChannelAmount)
			{
				throw new ArgumentException("Mismatch of input array length and channel amount.");
			}
			
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Byte[] newBuffer = new Byte[i.Length];
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
		
		public void Push(IWaveExtract i, UInt16 channel)
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
		
		public void Push(IWaveExtract i)
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
		
		public void Push(IWaveArrayExtract i)
		{
			Push(i.WaveArray, i.PlayTime);
			Edited = true;
		}
		
		public void Push(ITimedWaveArray i, Double Time)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Byte[] newBuffer = i[x];
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
		
		public void Push(Byte i)
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
		
		public void Push(Byte i, UInt16 channel)
		{
			if (Sealed)
			{
				throw new FieldAccessException("Attempt to push sequence into sealed sequencer.");
			}
			
			Track[channel].Add(i);
			Edited = true;
		}
		
		public void Push(Byte[] i)
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
					Track[i].Add(128);
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
				Track[channel].Add(128);
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
				Byte[] AdderBuffer = new Byte[ChannelAmount];
				for (Int32 i = 0; i < ChannelAmount; i++)
				{
					if (Track[i].Count < Length && x >= Track[i].Count)
					{
						AdderBuffer[i] = 128;
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
		public Byte[][] Unload()
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
		public Byte[][] SoftUnload()
		{
			return SecondaryTrackBuffer.ToArray();
		}
	}
}

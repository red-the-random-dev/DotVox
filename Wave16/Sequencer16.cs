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
		
		List<Int16[]> Track = new List<Int16[]>();
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
				return Track.Count;
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
				return Track[((Int32) Math.Round(TimeStamp / DeltaTime)) % Track.Count];
			}
		}
		
		public Sequencer16(UInt16 channelAmount, UInt32 sampleRate = 44100)
		{
			SampleRate = sampleRate;
			ChannelAmount = channelAmount;
		}
		
		public void Push(ITimedWave16 i, Double time)
		{
			if (ChannelAmount != 1)
			{
				throw new ArgumentException("Used sequencer instance does not support single-channel inputs.");
			}
			
			for (Double x = 0; x < time; x += DeltaTime)
			{
				Track.Add(new Int16[] {i[x]});
			}
		}
		
		public void Push(ITimedWave16[] i, Double Time)
		{
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
				Track.Add(newBuffer);
			}
		}
		
		public void Push(IWaveExtract16 i)
		{
			if (!i.Blank)
			{
				Push(i.Wave, i.PlayTime);
			}
			else
			{
				Skip(i.PlayTime);
			}
		}
		
		public void Push(IWaveArrayExtract16 i)
		{
			Push(i.WaveArray, i.PlayTime);
		}
		
		public void Push(ITimedWaveArray16 i, Double Time)
		{
			
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Int16[] newBuffer = i[x];
				if (newBuffer.Length != ChannelAmount)
				{
					throw new ArgumentException("Attempted to push data from source with different channel amount.");
				}
				
				Track.Add(newBuffer);
			}
		}
		
		public void Push(Int16 i)
		{
			if (ChannelAmount != 1)
			{
				throw new ArgumentException("Used sequencer instance does not support single-channel inputs.");
			}
			
			Track.Add(new Int16[] {i});
		}
		
		public void Push(Int16[] i)
		{
			if (i.Length != ChannelAmount)
			{
				throw new ArgumentException("Mismatch of input array length and channel amount.");
			}
			
			Track.Add(i);
		}
		
		public void Skip(Double Time)
		{
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Int16[] newBuffer = new Int16[ChannelAmount];
				for (int i = 0; i < newBuffer.Length; i++)
				{
					newBuffer[i] = 0;
				}
				Track.Add(newBuffer);
			}
		}
		
		public Int16[][] Unload()
		{
			return Track.ToArray();
		}
	}
}

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
	/// Linear sample array.
	/// </summary>
	public class Sequencer : ITimedWaveArray, IWaveArrayExtract
	{
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
			
			public override bool CanTimeout
			{
				get
				{
					return base.CanTimeout;
				}
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
		
		List<Byte[]> Track = new List<Byte[]>();
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
		
		public Byte[] this[Double TimeStamp]
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
		
		public Sequencer(UInt16 channelAmount, UInt32 sampleRate = 44100)
		{
			SampleRate = sampleRate;
			ChannelAmount = channelAmount;
		}
		
		public void Push(ITimedWave i, Double time)
		{
			if (ChannelAmount != 1)
			{
				throw new ArgumentException("Used sequencer instance does not support single-channel inputs.");
			}
			
			for (Double x = 0; x < time; x += DeltaTime)
			{
				Track.Add(new Byte[] {i[x]});
			}
		}
		
		public void Push(ITimedWave[] i, Double Time)
		{
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
				Track.Add(newBuffer);
			}
		}
		
		public void Push(IWaveExtract i)
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
		
		public void Push(IWaveArrayExtract i)
		{
			Push(i.WaveArray, i.PlayTime);
		}
		
		public void Push(ITimedWaveArray i, Double Time)
		{
			
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Byte[] newBuffer = i[x];
				if (newBuffer.Length != ChannelAmount)
				{
					throw new ArgumentException("Attempted to push data from source with different channel amount.");
				}
				
				Track.Add(newBuffer);
			}
		}
		
		public void Skip(Double Time)
		{
			for (Double x = 0; x < Time; x += DeltaTime)
			{
				Byte[] newBuffer = new Byte[ChannelAmount];
				for (int i = 0; i < newBuffer.Length; i++)
				{
					newBuffer[i] = 128;
				}
				Track.Add(newBuffer);
			}
		}
		
		public Byte[][] Unload()
		{
			return Track.ToArray();
		}
	}
}

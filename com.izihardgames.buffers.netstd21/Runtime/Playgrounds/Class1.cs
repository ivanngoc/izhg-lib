using System;
using System.Buffers;

namespace IziHardGames.Buffers.Playgrounds
{
    /// <summary>
    /// <see cref="System.Buffers.BuffersExtensions.Write{T}(IBufferWriter{T}, ReadOnlySpan{T})"/>
    /// <see cref="ArrayBufferWriter{T}"/>
    /// </summary>
    public class BufferWriter : IBufferWriter<byte>
    {
        public void Advance(int count)
        {
            throw new NotImplementedException();
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            throw new NotImplementedException();
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            throw new NotImplementedException();
        }
    }

    public class Memowner : IMemoryOwner<byte>
    {
        public Memory<byte> Memory { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.IO;

namespace IziHardGames.IO
{
	public static class ExtensionStream
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="from"></param>
		/// <param name="count"></param>
		/// <param name="buffer"></param>
		/// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
		/// <returns> current carriage position</returns>
		public static long Shift(this Stream stream, long from, long count, byte[] buffer, int offset)
		{
			if (!stream.CanWrite) throw new AccessViolationException("No rights for writing into stream");

			//stream.Read();

			stream.SetLength(stream.Length + count);

			stream.Position = from;

			return stream.Position;
		}
		public static long Insert(this Stream stream, long indexOfInsert, byte[] value, byte[] buffer)
		{
			if (!stream.CanWrite) throw new AccessViolationException("No rights for writing into stream");
			// размер вставляемых данных
			int valSize = value.Length;
			// сколько байт нужно переместить вправо от места вставки
			long moveSize = stream.Length - indexOfInsert;

			stream.SetLength(stream.Length + valSize);

			if (moveSize == default)
			{
				stream.Seek(0, SeekOrigin.Begin);

				stream.Write(value, 0, value.Length);
			}
			else
			{
				long seekOffsetRead = -valSize;
				// shift
				while (moveSize > 0)
				{   // определить сколько байтов будет переноситься за иттерацию
					int countPortition = (int)HelperMath.Clamp(moveSize, 1, buffer.Length);
					// добавить смещение отуда читать
					seekOffsetRead -= countPortition;
					// поставить каретку в позицию чтения
					stream.Seek(seekOffsetRead, SeekOrigin.End);
					// считать порцию байтов
					int readed = stream.Read(buffer, 0, countPortition);

					stream.Seek(seekOffsetRead + valSize, SeekOrigin.End);

					stream.Write(buffer, 0, countPortition);

					moveSize -= countPortition;
				}
				stream.Seek(indexOfInsert, SeekOrigin.Begin);

				stream.Write(value, 0, value.Length);
			}

			return stream.Position;
		}
	}
}
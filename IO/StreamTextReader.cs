using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace IziHardGames.IO
{
	public static class StreamTextReader
	{
		private static string newLineDelimeter;
		private static byte[] delimeterBytes;
		private static byte[] delimeterBytesCompare;

		public const int MB32 = 33554432;

		private static Encoding encoding;

		static StreamTextReader()
		{
			newLineDelimeter = Environment.NewLine;
			encoding = Encoding.UTF8;
			delimeterBytes = encoding.GetBytes(newLineDelimeter);
			delimeterBytesCompare = new byte[delimeterBytes.Length];
		}
		public static void SetLinesDelimeter(string delimterArg, Encoding encodingArg)
		{
			encoding = encodingArg;
			newLineDelimeter = delimterArg;
			delimeterBytes = encoding.GetBytes(delimterArg);
		}

		public static bool TryGetIndexOfNewLineChar(FileStream fileStream, byte[] bufferArg, long seekFrom, out long position)
		{
			fileStream.Position = seekFrom;

			long searchPos = fileStream.Position;

			while (fileStream.Position < fileStream.Length)
			{
				int read = fileStream.Read(bufferArg, 0, bufferArg.Length);

				for (int i = 0; i < read; i++)
				{
					delimeterBytesCompare.PushAppend(bufferArg[i]);

					for (int j = 0; j < delimeterBytesCompare.Length; j++)
					{
						if (delimeterBytesCompare[j] != delimeterBytes[j]) break;

						position = searchPos + i + 1;

						return true;
					}
				}
				searchPos += read;
			}

			position = searchPos;

			return default;
		}
		/// <summary>
		/// Переводит каретку за символ новой линии. Поиск идет от текущей позиции стрима
		/// </summary>
		/// <param name="fileStream"></param>
		/// <param name="bufferArg"></param>
		public static void SeekNewLine(FileStream fileStream, byte[] bufferArg)
		{
			if (StreamTextReader.TryGetIndexOfNewLineChar(fileStream, bufferArg, fileStream.Position, out long pos))
			{
				fileStream.Seek(pos, SeekOrigin.Begin);
			}
			else
			{
				fileStream.Seek(pos, SeekOrigin.Begin);
			}
		}



		public static bool TrySeekToLine(FileStream fileStream, int indexLine)
		{
			var buffer = ArrayPool<byte>.Shared.Rent(33554432);

			bool isSeek = default;

			for (int i = 0; i < indexLine; i++)
			{
				if (TryGetIndexOfNewLineChar(fileStream, buffer, fileStream.Position, out long pos))
				{
					isSeek = true;

					fileStream.Position = pos;
				}
			}

			ArrayPool<byte>.Shared.Return(buffer);

			return isSeek;
		}
		public static bool LineFindThatContain(string dirArg, string fileNameArg, string value, out int index)
		{
			StreamReader streamReader = new StreamReader(Path.Combine(dirArg, fileNameArg));

			index = default;

			while (!streamReader.EndOfStream)
			{
				string val = streamReader.ReadLine();

				if (val.Contains(value))
				{
					return true;
				}
				index++;
			}

			index = -1;

			return false;
		}
	}
}
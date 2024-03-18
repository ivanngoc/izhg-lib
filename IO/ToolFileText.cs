using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziHardGames.IO
{
	/// <summary>
	/// Работа с файлом в текстовом режиме
	/// </summary>
	public static class ToolFileText
	{
		public static StringBuilder fileName = new StringBuilder(256);
		public static StringBuilder directory = new StringBuilder(256);
		public static StringBuilder absolutePath = new StringBuilder(256);

		private const int KB4 = 4096;

		private const int MB1 = 1048576;
		private const int MB32 = 33554432;

		private static MemoryStream memoryStreamCaptured;

		private static byte[] buffer;

		private static string newLineDelimeter;
		private static Encoding encoding;

		static ToolFileText()
		{
			newLineDelimeter = Environment.NewLine;
			encoding = Encoding.UTF8;
		}
		public static void SetLinesDelimeter(string delimterArg, Encoding encodingArg)
		{
			encoding = encodingArg;
			newLineDelimeter = delimterArg;
		}

		/// <summary>
		/// Загружает файл в память для работы. Начинает сессию редактирования файла в памяти
		/// Реализовать режим монопольной работы или позволить создавать экземпляры класса при работе с захваченными файлами
		/// </summary>
		/// <returns></returns>
		public static Task Capture()
		{
			string result = default;

			var absoluteName = absolutePath.ToString();

			if (File.Exists(absoluteName))
			{
				using (var fs = new FileStream(absoluteName, FileMode.Open, FileAccess.Read, FileShare.Read, KB4, true))
				{
					var lenght = (int)fs.Length;

					buffer = ArrayPool<byte>.Shared.Rent(lenght * 2);

					memoryStreamCaptured = new MemoryStream(buffer);

					fs.CopyTo(memoryStreamCaptured);

					fs.Close();
				}
			}
			else
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"Save with path [{absoluteName}] doesn't exist");
#endif
			}

			return Task.CompletedTask;
		}
		/// <summary>
		/// Заканчивает сессию редактирования файла в памяти
		/// </summary>
		public static Task Release()
		{
			throw new NotImplementedException($"Не реализована обратная запис в файл");

			memoryStreamCaptured.Dispose();

			ArrayPool<byte>.Shared.Return(buffer);
		}

		public static void PathSet(string fullPath)
		{
			fileName.Clear();
			fileName.Append(Path.GetFileName(fullPath));

			directory.Clear();
			directory.Append(fullPath.Replace(fileName.ToString(), string.Empty));

			absolutePath.Clear();
			absolutePath.Append(Path.Combine(directory.ToString(), fileName.ToString()));
		}

		public static string PathSet(string directoryArg, string filenameArg)
		{
			fileName.Clear();
			fileName.Append(fileName);

			directory.Clear();
			directory.Append(directoryArg);

			string fullPath = Path.Combine(directoryArg, filenameArg);

			absolutePath.Clear();

			absolutePath.Append(fullPath);

			return fullPath;
		}

		public static void PathClean()
		{
			fileName.Clear();
			directory.Clear();
			absolutePath.Clear();
		}
		/// <summary>
		/// Вставка сктроки в конец файлы
		/// </summary>
		/// <param name="values"></param>
		public static Task LineAppend(string value)
		{
			byte[] bytes = default;

			FileInfo fileInfo = new FileInfo(absolutePath.ToString());

			if (fileInfo.Length == 0)
			{
				bytes = Encoding.UTF8.GetBytes(value);
			}
			else
			{
				bytes = Encoding.UTF8.GetBytes(newLineDelimeter + value);
			}

			var dir = directory.ToString();

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			using (var fileStream = new FileStream(absolutePath.ToString(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, true))
			{
				fileStream.Seek(0, SeekOrigin.End);

				using (var ms = new MemoryStream())
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}

			return Task.CompletedTask;

		}
		public static Task LinePrepend(string value)
		{
			var dir = directory.ToString();

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			LineInsert(value + newLineDelimeter, 0, Encoding.UTF8);

			return Task.CompletedTask;
		}
		public static void LineReplace(string value, int index)
		{
			throw new NotImplementedException();
		}
		public static void LineReplace(string oldValue, string newValue)
		{
			var strings = File.ReadLines(absolutePath.ToString()).ToList();

			List<string> copy = new List<string>();

			int count = strings.Count();

			if (count > 0)
			{
				bool isReplaced = default;

				foreach (var item in strings)
				{
					if (item == oldValue)
					{
						isReplaced = true;

						copy.Add(newValue);
					}
					else
					{
						copy.Add(item);
					}
				}

				if (isReplaced)
				{
					ToolFile.Clean(directory.ToString(), fileName.ToString());

					foreach (var item in copy)
					{
						LineAppend(item);
					}
				}
			}
		}
		public static void LineInsert(string value, int index, Encoding encoding)
		{
			var absoluteName = absolutePath.ToString();

			byte[] bytes = encoding.GetBytes(value);

			if (File.Exists(absoluteName))
			{
				var buffer = ArrayPool<byte>.Shared.Rent(MB32);

				using (var stream = new FileStream(absoluteName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, KB4, true))
				{
					stream.Insert(index, bytes, buffer);
				}
				ArrayPool<byte>.Shared.Return(buffer);
			}
			else
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"Save with path [{absoluteName}] doesn't exist");
#endif
			}
		}
		public static void LineRemove(string value)
		{
			throw new NotImplementedException();
		}
		public static void LineRemoveAt(int index)
		{
			throw new NotImplementedException();
		}
		public static void LineSwap(int left, int right)
		{
			throw new NotImplementedException();
		}
		public static void LineIndexOf(string fragment)
		{
			throw new NotImplementedException();
		}
		public static int GetLinesCount()
		{
			throw new NotImplementedException();
		}
		public static string LineGet(string dirArg, string fileNameArg, int index)
		{
			StreamReader streamReader = new StreamReader(Path.Combine(dirArg, fileNameArg));

			string result = default;

			for (int i = 0; i < index; i++)
			{
				result = streamReader.ReadLine();
			}

			return result;
		}
		public static int LineGetIndex(string dirArg, string fileNameArg, string lineValue)
		{
			StreamReader streamReader = new StreamReader(Path.Combine(dirArg, fileNameArg));

			int index = default;

			while (!streamReader.EndOfStream)
			{
				string val = streamReader.ReadLine();

				if (lineValue == val) return index;

				index++;
			}

			return -1;
		}

		public static bool ContainLine(string valueLine)
		{
			//FileStream fileStream = File.Open(absolutePath.ToString(), FileMode.OpenOrCreate);

			//StreamReader streamReader = new StreamReader(fileStream);

			var lines = File.ReadLines(absolutePath.ToString());

			foreach (var item in lines)
			{
				if (item == valueLine)
				{
					return true;
				}
			}

			return false;
		}

		public static void ReplaceDelimeters(string delimter)
		{
			throw new NotImplementedException();
		}

		public static IEnumerable<string> GetLines()
		{
			var lines = File.ReadLines(absolutePath.ToString());

			return lines;
		}


		private static Task<string> LineRead()
		{
			string result = default;

			var absoluteName = absolutePath.ToString();

			if (File.Exists(absoluteName))
			{
				int numRead = default;

				int total = default;

				int countLines = default;

				using (var stream = new FileStream(absoluteName, FileMode.Open, FileAccess.Read, FileShare.Read, KB4, true))
				{
					var lenght = (int)stream.Length;

					var buffer = ArrayPool<byte>.Shared.Rent(HelperMath.Clamp(lenght, MB32, int.MaxValue));

					while ((numRead = stream.Read(buffer, 0, MB32)) != 0)
					{

					}

					result = Encoding.UTF8.GetString(buffer, 0, total);

					stream.Close();

					ArrayPool<byte>.Shared.Return(buffer);
				}
#if UNITY_EDITOR
				//UnityEngine.Debug.Log($"Readline :{absoluteName} {Environment.NewLine} {result}");
#endif
			}
			else
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"Save with path [{absoluteName}] doesn't exist");
#endif
			}

			return Task.FromResult(result);
		}

		public static Task<string> Get(int line)
		{
			throw new System.NotImplementedException();
		}

		private static Task Write()
		{
			throw new System.NotImplementedException();
		}

		private static Task<string> ReadWholeUTF8()
		{
			string result = default;

			var absoluteName = absolutePath.ToString();

			if (File.Exists(absoluteName))
			{
				int numRead = default;
				int total = default;

				using (var stream = new FileStream(absoluteName, FileMode.Open, FileAccess.Read, FileShare.Read, KB4, true))
				{
					var lenght = (int)stream.Length;

					var buffer = ArrayPool<byte>.Shared.Rent(HelperMath.Clamp(lenght, MB32, int.MaxValue));

					while ((numRead = stream.Read(buffer, 0, MB32)) != 0)
					{

					}

					result = Encoding.UTF8.GetString(buffer, 0, total);

					stream.Close();

					ArrayPool<byte>.Shared.Return(buffer);
				}
#if UNITY_EDITOR
				//UnityEngine.Debug.Log($"Read whole:{absoluteName} {Environment.NewLine} {result}");
#endif
			}
			else
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"Save with path [{absoluteName}] doesn't exist");
#endif
			}
			return Task.FromResult(result);
		}
	}


}
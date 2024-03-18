using System;
using System.Buffers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IziHardGames.IO
{
	public class ToolFileBinary
	{
		public const int KB4 = 4096;
		public const int KB8 = 8192;
		public const int KB16 = 16384;
		public const int KB32 = 32768;

		public const int MB8 = 8388608;
		public const int MB16 = 16777216;
		public const int MB32 = 33554432;
		public const int MB64 = 67108864;
		public const int MB128 = 134217728;
		public const int MB256 = 268435456;
		public const int MB512 = 536870912;

		public static StringBuilder stringBuilder = new StringBuilder(260);
		public static StringBuilder directory = new StringBuilder(260);
		public static StringBuilder fileName = new StringBuilder(260);

		public static float readProgress;

		private static object objToWrite;
		private static byte[] bytesToWrite;

		private static byte[] buffer = new byte[0x1000];
		public static void SetObjectToWrite(object obj)
		{
			objToWrite = obj;
		}
		public static string SetPath(string directoryIn, string fileNameIn = default)
		{
			stringBuilder.Clear();
			stringBuilder.Append(directoryIn);

			fileName.Clear();
			fileName.Append(fileNameIn);

			directory.Clear();
			directory.Append(directoryIn);

			if (!string.IsNullOrEmpty(fileNameIn))
			{
				/// <see cref="Path.DirectorySeparatorChar"/> = '\' или '/' (не проверено)
				/// <see cref="Path.VolumeSeparatorChar"/> = ':' 
				/// <see cref="Path.PathSeparator"/> = ';' 
				stringBuilder.Append(Path.DirectorySeparatorChar);
				stringBuilder.Append(fileNameIn);
			}

			return stringBuilder.ToString();
		}

		public static Task WriteFileBinAsync()
		{
			var absolutePath = stringBuilder.ToString();
#if UNITY_EDITOR
			if (objToWrite == null)
			{
				throw new NullReferenceException($"объект для записи в файл пустой");
			}
#endif
			if (!Directory.Exists(directory.ToString()))
			{
				Directory.CreateDirectory(directory.ToString());
			}

			Task task = default;

			using (var fileStream = new FileStream(absolutePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, true))
			{
				var bf = new BinaryFormatter();

				using (var ms = new MemoryStream())
				{
					bf.Serialize(ms, objToWrite);

					var toWrite = ms.ToArray();
					/// TODO: чтобы разграничивать файлы разных размеров перед собой нужно придумать формат делиметра.
					/// Самый простой вариант писать long размер объекта перед ним и потом считывать это значение
					task = fileStream.WriteAsync(toWrite, 0, toWrite.Length);

					task.Wait();
				}
			}

#if UNITY_EDITOR
			//UnityEngine.Debug.Log($"Saved {Environment.NewLine}{absolutePath}");
#endif

			return task;
		}

		public static async Task<object> ReadFileBinAsync()
		{
			object result = default;

			var absoluteName = stringBuilder.ToString();

			if (File.Exists(absoluteName))
			{
				var binaryFormatter = new BinaryFormatter();

				int numRead = default;
				int total = default;

				using (var stream = new FileStream(absoluteName,
														  FileMode.Open,
														  FileAccess.Read,
														  FileShare.Read,
														  KB4,
														  true))
				{
					var lenght = (int)stream.Length;

					var buffer = ArrayPool<byte>.Shared.Rent(HelperMath.Clamp(lenght, MB32, int.MaxValue));

					while ((numRead = await stream.ReadAsync(buffer, 0, MB32)) != 0)
					{
						total += numRead;

						readProgress = (float)total / lenght;
					}

					stream.Position = 0;

					result = binaryFormatter.Deserialize(stream);

					stream.Close();

					ArrayPool<byte>.Shared.Return(buffer);
				}

#if UNITY_EDITOR

				//UnityEngine.Debug.Log($"Save load {Environment.NewLine}{absoluteName}");

#endif
			}
			else
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"Save with path [{absoluteName}] doesn't exist");
#endif
				result = default;
			}

			return result;
		}

		public static void DelteFile(string path)
		{
			try
			{
				File.Delete(path);
			}
			catch (Exception ex)
			{
#if UNITY_EDITOR
				//UnityEngine.Debug.LogError($"{ex}");
#endif

				return;
			}
		}

		public static Task WriteFileXMLAsync<T>()
		{
			var absolutePath = stringBuilder.ToString();
#if UNITY_EDITOR
			if (objToWrite == null)
			{
				throw new NullReferenceException($"объект для записи в файл пустой");
			}
#endif
			if (!Directory.Exists(directory.ToString()))
			{
				Directory.CreateDirectory(directory.ToString());
			}

			Task task = default;

			using (var fileStream = new FileStream(absolutePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, true))
			{
				using (var ms = new MemoryStream())
				{
					var serializer = new XmlSerializer(typeof(T));

					serializer.Serialize(ms, objToWrite);

					var toWrite = ms.ToArray();
					/// TODO: чтобы разграничивать файлы разных размеров перед собой нужно придумать формат делиметра.
					/// Самый простой вариант писать long размер объекта перед ним и потом считывать это значение
					task = fileStream.WriteAsync(toWrite, 0, toWrite.Length);

					task.Wait();
				}
			}

#if UNITY_EDITOR
			//UnityEngine.Debug.Log($"Saved {Environment.NewLine}{absolutePath}");
#endif
			return task;
		}
	}

	public class ToolFileDelete
	{
		public static Task DeleteFile(string fullPath)
		{
			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}
			else
			{
				return Task.FromException(new FileNotFoundException(fullPath));
			}
			return Task.CompletedTask;
		}
	}
}
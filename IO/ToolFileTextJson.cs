using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IziHardGames.IO
{
	/// <summary>
	/// Инструмент для работы с текстовым файлом в формате json
	/// </summary>
	public static class ToolFileTextJson
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

		public static float readProgress;
		private static object objToWrite;

		public static StringBuilder absolutePath = new StringBuilder(260);
		public static StringBuilder directory = new StringBuilder(260);
		public static StringBuilder fileName = new StringBuilder(260);

		public static void Capture(string fullPath)
		{
			throw new NotImplementedException();
		}
		public static async Task Append(string text)
		{
			throw new NotImplementedException();
		}
		public static async Task Append(string fullPath, string text)
		{
			throw new NotImplementedException();
		}

		public static string SetObjectToWrite(object toWrite, string directory, string filename)
		{
			Set(toWrite);
			return Set(directory, filename);
		}
		public static string Set(string directoryS, string filenameS)
		{
			absolutePath.Clear();
			directory.Clear();
			fileName.Clear();

			directory.Append(directoryS);
			fileName.Append(filenameS);
			absolutePath.Append(Path.Combine(directoryS, filenameS));

			return absolutePath.ToString();
		}
		public static void Set(object toWrite)
		{
			objToWrite = toWrite;
		}
		public static async Task<object> WriteJsonReplace()
		{
			throw new System.NotImplementedException();
			string json =""; //JsonConvert.SerializeObject(objToWrite, objToWrite.GetType(), new JsonSerializerSettings() { });

			byte[] bytes = Encoding.UTF8.GetBytes(json);

			if (!Directory.Exists(directory.ToString()))
			{
				Directory.CreateDirectory(directory.ToString());
			}

			if (File.Exists(absolutePath.ToString()))
			{
				File.Delete(absolutePath.ToString());
			}

			using (var fileStream = new FileStream(absolutePath.ToString(), FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
			{
				using (var ms = new MemoryStream())
				{
					/// Самый простой вариант писать long размер объекта перед ним и потом считывать это значение
					await fileStream.WriteAsync(bytes, 0, bytes.Length);
				}
			}
			return objToWrite;
		}

		public static async Task<object> ReadJson<T>()
		{
			object result = default;

			var absoluteName = absolutePath.ToString();

			if (File.Exists(absoluteName))
			{

				int numRead = default;
				int total = default;

				using (var stream = new FileStream(absoluteName, FileMode.Open, FileAccess.Read, FileShare.Read, KB4, true))
				{
					var lenght = (int)stream.Length;

					var buffer = ArrayPool<byte>.Shared.Rent(HelperMath.Clamp(lenght, MB32, int.MaxValue));

					while ((numRead = await stream.ReadAsync(buffer, 0, MB32)) != 0)
					{
						total += numRead;

						readProgress = (float)total / lenght;
					}

					string value = Encoding.UTF8.GetString(buffer, 0, total);
					throw new System.NotImplementedException();
					//result = JsonConvert.DeserializeObject<T>(value);

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
	}
}
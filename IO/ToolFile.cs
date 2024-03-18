using System;
using System.IO;

namespace IziHardGames.IO
{
	public static class ToolFile
	{
		public static void Create(string directoryArg, string fileNameArg)
		{
			if (!Directory.Exists(directoryArg))
			{
				Directory.CreateDirectory(directoryArg);
			}
			string absolutePath = Path.Combine(directoryArg, fileNameArg);

			if (!File.Exists(absolutePath))
			{
				FileStream fileStream = File.Create(absolutePath);

				fileStream.Dispose();
			}
		}
		public static FileStream Create(string directoryArg, string fileNameArg, int bufferSize, FileOptions fileOptions)
		{
			if (!Directory.Exists(directoryArg))
			{
				Directory.CreateDirectory(directoryArg);

			}
			string absolutePath = Path.Combine(directoryArg, fileNameArg);

			if (!File.Exists(absolutePath))
			{
				FileStream fileStream = File.Create(absolutePath, bufferSize, fileOptions);

				return fileStream;
			}

			throw new NotSupportedException("File existed");
		}
		public static void Delete(string directoryArg, string fileNameArg)
		{
			if (!Directory.Exists(directoryArg)) return;

			string absolutePath = Path.Combine(directoryArg, fileNameArg);

			if (File.Exists(absolutePath))
			{
				File.Delete(absolutePath);
			}
		}
		/// <summary>
		/// Set File to Empty State (size 0)
		/// </summary>
		/// <param name="directoryArg"></param>
		/// <param name="fileNameArg"></param>
		public static void Clean(string directoryArg, string fileNameArg)
		{
			if (!Directory.Exists(directoryArg)) return;

			string absolutePath = Path.Combine(directoryArg, fileNameArg);

			if (File.Exists(absolutePath))
			{
				using (FileStream fileStream = File.OpenWrite(absolutePath))
				{
					fileStream.SetLength(0);
				}
			}
		}
	}
}
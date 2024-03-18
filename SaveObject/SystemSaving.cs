using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IziHardGames.Libs.NonEngine.SavingSystems.InFile
{
	public class SystemSaving : IDisposable
	{
		protected int countOfSlots;
		protected string path;
		protected string fileNameTemplate;

		protected object toSaveObject;
		protected string toSaveFilename;
		protected string toSaveDirectory;

		private FileStream fileStream;
		private MemoryStream memoryStream;

		public SystemSaving(int countOfSlots, string path, string fileNameTemplate)
		{
			this.countOfSlots = countOfSlots;
			this.path = path;
			this.fileNameTemplate = fileNameTemplate;
		}

		public void Save(int indexSlot, string data)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// If save file is corrupted 
		/// </summary>
		/// <param name="indexSlot"></param>
		public void Repair(int indexSlot)
		{
			throw new NotImplementedException();
		}

		public void Backup()
		{
			throw new NotImplementedException();
		}

		public T Load<T>(int indexOfSlot)
		{
			throw new NotImplementedException();
		}

		protected Task WriteObjectToFileNonBlocking()
		{
			if (fileStream == null)
			{
				if (toSaveObject is string text)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(text);

					if (!Directory.Exists(toSaveDirectory))
					{
						Directory.CreateDirectory(toSaveDirectory.ToString());
					}
					string absPath = Path.Combine(toSaveDirectory, toSaveFilename);

					if (File.Exists(absPath))
					{
						File.Delete(absPath);
					}
					fileStream = new FileStream(absPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

					Task task = fileStream.WriteAsync(bytes, 0, bytes.Length);
					task.ContinueWith((x) => Dispose(x));
					return task;
				}
			}
			throw new NotSupportedException();
		}
		private void Dispose(Task task)
		{
			Dispose();
		}
		public void Dispose()
		{
			if (fileStream != null)
			{
				fileStream.Dispose();
				fileStream = default;
			}
		}
	}
}

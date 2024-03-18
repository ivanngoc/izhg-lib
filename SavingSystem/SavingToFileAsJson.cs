using IziHardGames.Libs.NonEngine.SavingSystems.InFile;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IziHardGames.Libs.Engine.SavingSystem.ToFile
{
	public class SavingToFileAsJson : SystemSaving
	{
		public SavingToFileAsJson(int countOfSlots, string path, string fileNameTemplate) : base(countOfSlots, path, fileNameTemplate)
		{
		}

		public void Save<T>(int slot, T item)
		{
			string json = JsonUtility.ToJson(item);
			base.Save(slot, json);
		}

		public void SetToSave<T>(int indexOfSlot, T item) where T : class
		{
			string json = JsonUtility.ToJson(item);
			SetToSave(indexOfSlot, json);
		}
		public void SetToSave(int slot, string json)
		{
			this.toSaveObject = json;
			this.toSaveDirectory = this.path;
			this.toSaveFilename = this.fileNameTemplate + slot.ToString();
		}

		public Task SaveWithTask()
		{
			return base.WriteObjectToFileNonBlocking();
		}
	}
}
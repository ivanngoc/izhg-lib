using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using IziHardGames.IO;
using System.IO;
using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Libs.Engine.ResourcesManagment;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace IziHardGames.ProjectResourceManagment
{
	/// <summary>
	/// Соотносит guid ассета с guid проекта. ДЛя каждого проекта свой экземпляр
	/// </summary>
	[CreateAssetMenu(fileName = "Registry", menuName = "IziHardGames/Asset's Registry List", order = 0)]
	public class RegistryAsset : ScriptableObject
	{
		///// <summary>
		///// Merge taken guid List
		///// </summary>
		//[SerializeField] public TextAsset textAsset;

		[SerializeField] public TextAsset staticReferences;
		/// <summary>
		/// <see cref="GUID"/>
		/// </summary>
		[SerializeField] public List<int> ids = new List<int>();
		/// <summary>
		/// guid as unity asset (string)
		/// </summary>
		[SerializeField] public List<string> guids = new List<string>();

		[SerializeField] public List<string> descriptions = new List<string>();
		[Header("Sprites")]
		[SerializeField] public List<ScriptableSpritesGroupe> scriptableSpritesGroupes = new List<ScriptableSpritesGroupe>();

		public int freeGuid = int.MinValue;

		public Sprite GetRandonSpriteFromGroupe(int idGroupe)
		{
			int count = scriptableSpritesGroupes.Count;

			for (int i = 0; i < count; i++)
			{
				if (scriptableSpritesGroupes[i].idGroupe == idGroupe)
				{
					return scriptableSpritesGroupes[i].sprites.ElementAt(UnityEngine.Random.Range(0, scriptableSpritesGroupes[i].sprites.Length));
				}
			}
			return default;
		}

		#region Editor only
#if UNITY_EDITOR
		[Space]
		[Space]
		[Space]
		[Header("Editor only")]
		public List<long> localIdentifiers;
		public List<Object> objects;
		[Header("Групировка по моменту загрузки")]
		/// <summary>
		/// Присутствуют при старте
		/// </summary>
		public List<Object> loadedOnStart;
		/// <summary>
		/// ЗАпрашиваются в зависимости от ситуации
		/// </summary>
		public List<Object> loadedOnRequest;
		/// <summary>
		/// Запращиваются после запуске в фоне
		/// </summary>
		public List<Object> loadedOnBackground;

		[Header("Конкретные")]
		public List<Sprite> sprites;

		#region Unity Message
		#endregion

		[ContextMenu("Создать айди")]
		public void CreateIds()
		{
			ids.Clear();
			localIdentifiers.Clear();
			guids.Clear();

			for (int i = 0; i < objects.Count; i++)
			{
				ids.Add(i);
				//#if UNITY_EDITOR
				if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objects[i], out string guid, out long local))
				{
					guids.Add(guid);

					localIdentifiers.Add(local);
				}
				//#endif
			}
		}

		public void RegenerateIds()
		{
			foreach (var item in objects)
			{

			}
		}

		public void Add(Object obj)
		{
#if UNITY_EDITOR
			if (objects.Contains(obj)) return;

			objects.Add(obj);
#endif


			ids.Add(ids.Count);
		}

		public bool Insert(Object toInsert, string description, out int id)
		{
			id = default;

			if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(toInsert, out string guid, out long local))
			{
				int indexOf = objects.IndexOf(toInsert);

				if (indexOf > -1)
				{
					id = ids[indexOf];
				}

				if (!CheckContains(toInsert, id, guid))
				{
					guids.Add(guid);

					localIdentifiers.Add(local);

					objects.Add(toInsert);

					id = GetFreeId();

					ids.Add(id);

					if (string.IsNullOrEmpty(description))
					{
						description = "Description";
					}

					descriptions.Add(description);

					DevideByTypes(toInsert, guid);
				}
			}

			WriteStaticLinkToFile(toInsert, id);

			UnityEditor.EditorUtility.SetDirty(this);

			return id != 0;
		}

		private bool CheckContains(Object toInsert, int guidInProject, string guidAsset)
		{
			bool isContainObject = objects.Contains(toInsert);
			bool isGuidInProject = ids.Contains(guidInProject);
			bool isGuidAsset = guids.Contains(guidAsset);

			if (!isContainObject && (isGuidInProject || isGuidAsset))
			{
				throw new Exception($"Miss link in [{nameof(objects)}] field");
			}
			if (!isGuidInProject && (isContainObject || isGuidAsset))
			{
				throw new Exception($"Miss guid int in project [{nameof(ids)}] fields");
			}
			if (!isGuidAsset && (isContainObject || isGuidInProject))
			{
				throw new Exception($"Miss guid Asset [{nameof(guids)}] fields");
			}
			return isContainObject && isGuidInProject && isGuidAsset;
		}
		/// <summary>
		/// 
		/// </summary>
		public void WriteStaticLinkToFile(Object toInsert, int guidInProject)
		{
			if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(toInsert, out string guid, out long local))
			{
				if (CheckContains(toInsert, guidInProject, guid))
				{
					Debug.LogError($"Уже записан");
				}
				else
				{

				}
				string path = AssetDatabase.GetAssetPath(staticReferences);

				path = path.Replace('/', Path.DirectorySeparatorChar);

				string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);

				Debug.Log(fullPath);

				string filename = Path.GetFileName(fullPath);

				string dir = fullPath.Replace(filename, string.Empty);

				string content = GetRecordString(guidInProject, guid);

				ToolFileText.PathSet(dir, filename);

				if (!ToolFileText.ContainLine(content))
				{
					ToolFileText.LineAppend(content);
				}
			}
		}

		public int GetIdFromPath(Object asset)
		{
			if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string guid, out long localId))
			{
				int indexOf = guids.IndexOf(guid);

				return ids[indexOf];
			}
			return default;
		}
		public int GetId(Object @object)
		{
			int index = objects.IndexOf(@object);

			return ids[index];
		}

		public (int, string, string, string) GetIds(Object obj)
		{
			int indexOf = objects.IndexOf(obj);

			if (indexOf > -1)
			{
				return (ids[indexOf], guids[indexOf], ids[indexOf].UIntToStringHex(), descriptions[indexOf]);
			}
			else
			{
				return default;
			}
		}

		public Object GetObjectByHex(string hexGuid)
		{
			if (int.TryParse(hexGuid, out int id))
			{
				return GetObject(id);
			}
			return default;
		}

		internal void UpdateDescription(int guidInProjectToSearch, string description)
		{
			int index = ids.IndexOf(guidInProjectToSearch);

			string oldLine = GetRecordString(index);

			descriptions[index] = description;

			string path = AssetDatabase.GetAssetPath(staticReferences);

			path = path.Replace('/', Path.DirectorySeparatorChar);

			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);

			ToolFileText.PathSet(fullPath);

			string newLine = GetRecordString(index);

			ToolFileText.LineReplace(oldLine, newLine);
		}

		public Object GetObject(int guid)
		{
			int index = ids.IndexOf(guid);

			if (index > -1)
			{
				return objects[index];
			}

			return default;
		}
		public bool TryGetId(Object obj, out int id)
		{
			if (objects.Contains(obj))
			{
				int index = objects.IndexOf(obj);

				id = ids[index];

				return true;
			}

			id = default;

			return false;
		}
		public string GetGUID(int id)
		{
			int index = ids.IndexOf(id);

			if (index > 0)
			{
				return guids[index];
			}

			throw new ArgumentException($"Нет зарегистрированного объекта с id {id}");
		}
		public string GetGUID(Object @object)
		{
			int index = objects.IndexOf(@object);

			return guids[index];
		}
		public int GetIndex(Object @object)
		{
			return objects.IndexOf(@object);
		}
		[ContextMenu("ОбновитЬ")]
		public void Refresh()
		{
			DevideByTypes();
		}

		public void DevideByTypes()
		{
			//int spriteCount = objects.Count(X => X is Sprite);

			//Texture

			//sprites = new Sprite[spriteCount];
			sprites.Clear();

			for (int i = 0; i < objects.Count; i++)
			{
				Type type = objects[i].GetType();

				DevideByTypes(objects[i], guids[i]);
			}
		}

		private void DevideByTypes(Object value, string guid)
		{
			Type type = value.GetType();

			if (value is Texture2D)
			{
				Sprite sprite = default;
				//#if UNITY_EDITOR
				sprite = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guid), typeof(Sprite)) as Sprite;
				//#endif
				sprites.Add(sprite);
			}
		}

		public (bool, bool, bool) IsContains(Object obj, out int idInProject)
		{
			(bool, bool, bool) v = default;

			idInProject = default;

			v.Item1 = TryGetId(obj, out idInProject);

			v.Item2 = UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long local);

			v.Item3 = guids.Contains(guid);

			return v;
		}

		public Sprite GetSprite(int id)
		{
			int index = ids.IndexOf(id);

			string path = AssetDatabase.GUIDToAssetPath(guids[index]);

			return AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		}

		public int GetFreeId()
		{
			int newId = freeGuid;

			while (ids.Contains(newId) || newId == default)
			{
				freeGuid++;

				newId = freeGuid;
			}

			freeGuid++;

			return newId;
		}

		public void Destinct()
		{
			objects = objects.Distinct().ToList();
			ids = ids.Distinct().ToList();
			guids = guids.Distinct().ToList();
		}

		public void RegainIdsFromFile()
		{
			string path = AssetDatabase.GetAssetPath(staticReferences);

			path = path.Replace('/', Path.DirectorySeparatorChar);

			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);

			ToolFileText.PathSet(fullPath);

			var lines = ToolFileText.GetLines();

			ids.Clear();
			objects.Clear();
			guids.Clear();
			descriptions.Clear();
			localIdentifiers.Clear();

			foreach (var line in lines)
			{
				string[] split = line.Split('\t');

				string idHex = split[0];
				string guid = split[1];
				string description = split[2];

				int id = Convert.ToInt32(idHex, 16);

				//if (int.TryParse(idHex, out int id))
				//{
				ids.Add(id);
				guids.Add(guid);
				descriptions.Add(description);

				string unityPath = AssetDatabase.GUIDToAssetPath(guid);

				Object obj = AssetDatabase.LoadAssetAtPath(unityPath, typeof(Object));

				if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out guid, out long local))
				{
					localIdentifiers.Add(local);
				}
				objects.Add(obj);
				//}
				//else
				//{
				//    Debug.LogError($"Cant parse hex line to int");
				//}
			}

			DevideByTypes();
		}

		public void OverrideFile()
		{
			string path = AssetDatabase.GetAssetPath(staticReferences);

			path = path.Replace('/', Path.DirectorySeparatorChar);

			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);

			ToolFileText.PathSet(fullPath);

			ToolFile.Clean(ToolFileText.directory.ToString(), ToolFileText.fileName.ToString());

			for (int i = 0; i < ids.Count; i++)
			{
				ToolFileText.LineAppend(GetRecordString(i));
			}
		}

		[ContextMenu("Вызвать дебаг")]
		public void CustomCall()
		{
			for (int i = 0; i < 32; i++)
			{
				WriteStaticLinkToFile(objects[i], ids[i]);
			}
		}
		public string GetRecordString(int index)
		{
			return GetRecordString(ids[index], guids[index], descriptions[index]);
		}
		public string GetRecordString(int id, string guid)
		{
			return GetRecordString(id, guid, "Description");
		}
		public string GetRecordString(int id, string guid, string description)
		{
			return $"{id.UIntToStringHex()}\t{guid}\t{description}";
		}
#endif
		#endregion

	}
}
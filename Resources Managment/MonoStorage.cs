using IziHardGames.Core;
using IziHardGames.Ticking.Lib.ApplicationLevel;
using IziHardGames.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.ResourcesManagment
{
	/// <summary>
	/// Простое хранилище объектов. Хранит ссылки по ключу
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MonoStorage<T> : MonoBehaviour, IInitializable
		where T : IUnique
	{
		[SerializeField] protected List<T> storage;

		public UnityDictionary<int, T> keyValuePairs = new UnityDictionary<int, T>();

		#region Unity Message
		public virtual void OnDestroy()
		{
			Reg.SingletonRemove(this);
		}
		#endregion

		public virtual void Initilize()
		{
			Reg.SingletonAdd(this);
		}

		public virtual void Add(T value)
		{
			storage.Add(value);

			keyValuePairs.Add(value.Id, value);
		}
		public virtual void AddRange(ICollection<T> value)
		{
			foreach (var item in value)
			{
				Add(item);
			}
		}
		public virtual void AddRange(IEnumerable<T> value)
		{
			foreach (var item in value)
			{
				Add(item);
			}
		}
		public virtual void Remove(int id)
		{
			storage.TryRemoveById(id);

			keyValuePairs.Remove(id);
		}
		public virtual void Remove(T value)
		{
			storage.Remove(value);

			keyValuePairs.Remove(value.Id);
		}

		public virtual T GetById(int id)
		{
			return keyValuePairs[id];
		}

		public IEnumerable<T> GetAll()
		{
			return storage;
		}

		public virtual void Clear()
		{
			storage.Clear();
			keyValuePairs.Clear();
		}

		public int GetFreeId()
		{
			var newId = int.MinValue;

			while (keyValuePairs.ContainsKey(newId))
			{
				newId++;
			}

			return newId;
		}

		public List<T> GetStorage() => storage;

		public Dictionary<int, T> GetDic() => keyValuePairs;
	}
}
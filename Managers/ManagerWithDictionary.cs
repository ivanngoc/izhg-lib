using IziHardGames.Core;
using System.Collections.Generic;

namespace IziHardGames.Libs.Engine.Managers
{
	public class ManagerWithDictionary<T>
		where T : IUnique
	{
		public readonly Dictionary<int, T> keyValuePairs;
		public ManagerWithDictionary(int capacity = 256)
		{
			keyValuePairs = new Dictionary<int, T>(capacity);
		}

		public void Add(T value)
		{
			keyValuePairs.Add(value.Id, value);
		}

		public T Remove(T value)
		{
			return Remove(value.Id);
		}
		public T Remove(int key)
		{
			var val = keyValuePairs[key];
			keyValuePairs.Remove(key);
			return val;
		}

		public void Replace(T value)
		{
			keyValuePairs[value.Id] = value;
		}

		public void AddOrReplace(T value)
		{
			var key = value.Id;

			if (keyValuePairs.ContainsKey(key))
			{
				Replace(value);
			}
			else
			{
				Add(value);
			}
		}

		public T GetItem(int key)
		{
			return keyValuePairs[key];
		}

		public bool TryGetItem(int key, out T value)
		{
			return keyValuePairs.TryGetValue(key, out value);
		}

		

		public void Clear()
		{

		}
	}

}
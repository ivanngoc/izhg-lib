using IziHardGames.Core;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Storages.Multifunc
{
}

namespace IziHardGames.Libs.NonEngine.Storages
{
	/// <summary>
	/// Ститическое хранилизще объектов. One-time init. Не оптимизирован на частые вставки и изъятия 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class Storage<T> where T : IUnique
	{
		public T[] items;
		protected Dictionary<int, T> itemsByKey;

		#region Unity Message


		#endregion

		public T GetItemByIndex(int index) => items[index];
		public virtual T GetItemByKey(int key) => itemsByKey[key];

		public void Init(T[] array)
		{
			items = array;
		}
		/// <summary>
		/// Инициализация функций поиска таких как доступ по ключу и т.д.
		/// </summary>
		public void InitSearchFeatures()
		{
			itemsByKey = itemsByKey ?? new Dictionary<int, T>(items.Length);

			for (int i = 0; i < items.Length; i++)
			{
				itemsByKey.Add(items[i].Id, items[i]);
			}
		}
		public void InitSearchFeaturesReverse()
		{
			itemsByKey.Clear();
		}
	}


	/// <summary>
	/// Глобальное Хранилище объектов
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	public static class StorageShared<TItem> where TItem : IUnique
	{
		public static List<TItem> items;
		public static Dictionary<int, TItem> itemsById;

		static StorageShared()
		{
			items = new List<TItem>(0);
			itemsById = new Dictionary<int, TItem>(0);
		}

		public static void Initilize(int capacity)
		{
			items = new List<TItem>(capacity);
			itemsById = new Dictionary<int, TItem>(capacity);
		}

		public static TItem GetByIndex(int index)
		{
			return items[index];
		}
		public static TItem GetById(int id)
		{
			return itemsById[id];
		}

		public static void Add(TItem item)
		{
			Add(item.Id, item);
		}
		public static void Add(int id, TItem item)
		{
			items.Add(item);
			itemsById.Add(id, item);
		}
		public static void Remove(int id, TItem item)
		{
			items.Remove(item);
			itemsById.Remove(id);
		}
		public static void Replace(int id, TItem item)
		{
			int index = items.IndexOf(item);
			items[index] = item;
			itemsById[id] = item;
		}

		public static List<TItem> SelectAll()
		{
			return items;
		}
		public static TItem First(Func<TItem, bool> selector)
		{
			foreach (var item in items)
			{
				if (selector(item)) return item;
			}
			throw new ArgumentOutOfRangeException($"No match for query");
		}
		public static TItem FirstOrDefault(Func<TItem, bool> selector)
		{
			foreach (var item in items)
			{
				if (selector(item)) return item;
			}
			return default;
		}
		public static int GetFreeIdExceptZeroFrom(int from)
		{
			return itemsById.GetFreeExceptZeroIdAscending(from);
		}
	}

	/// <summary>
	/// Бывыет что объекта ссылочного типа еще нет, но уже нужна законтрактовать его.
	/// Это своего рода объект одещания или гарантии при обращении к которому к моменту этого обращения объект будет создан и к нему можно будет получить доступ черех этот объект
	/// </summary>
	public readonly struct TokenForGetFromStorageShared<T> where T : IUnique
	{
		public readonly int id;
		public T GetById()
		{
			return StorageShared<T>.GetById(id);
		}
	}
}
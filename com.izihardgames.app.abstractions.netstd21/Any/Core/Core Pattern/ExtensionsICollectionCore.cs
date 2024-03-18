using IziHardGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames
{
	public static partial class ExtensionsICollection
	{
		public static bool ContainsId<T>(this ICollection<T> collection, int id) where T : IziHardGames.Core.IUnique
		{
			foreach (var item in collection)
			{
				if (item.Id == id) return true;
			}
			return false;
		}

		public static T FirstUnique<T>(this ICollection<T> collection, int id) where T : IUnique
		{
			foreach (var item in collection)
			{
				if (item.Id == id) return item;
			}
			return default;
		}
	}

	public static class ExtensionsQueue
	{
		/// <summary>
		/// Вставить объект не увеличивая в размерах список. Вставка с удержание размера. 
		/// Циклическая запись с перезаписью
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queue"></param>
		/// <param name="item"></param>
		public static void PushWithOverride<T>(this Queue<T> queue, T item, int fixedSize)
		{
			if (queue.Count >= fixedSize)
			{
				queue.Dequeue();
			}
			queue.Enqueue(item);
		}
	}
	public static class ExtensionsIList
	{
		/// <summary>
		/// Each element in Left is equal to element in Right and has same order 
		/// </summary>
		/// <returns></returns>
		public static bool IsEqualContentCorrespondingly<T>(this IList<T> left, IList<T> right)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Left is got same data that Right, but order can be mismatched. Each element is Unique. With duplicates correctness is not guaranted.
		/// </summary>
		/// <returns></returns>
		public static bool IsEqualContentForSingles<T>(this IList<T> left, IList<T> right, Func<T, T, bool> isEqual)
		{
			if (left.Count != right.Count) return false;

			int count = left.Count;
			int matchCount = default;

			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (isEqual(left[i], right[j]))
					{
						matchCount++;
						break;
					}
				}
			}
			return count == matchCount;
		}
		/// <summary>
		/// Left is got same data that Right, but order can be mismatched. Duplicates is considered.
		/// </summary>
		/// <returns></returns>
		public static bool IsEqualContentForMultiplesWithSorting<TItem, TKey>(this IList<TItem> left, IList<TItem> right, Func<TItem, TItem, bool> isEqual, Func<TItem, TKey> selector, IComparer<TKey> comparer)
		{
			if (left.Count != right.Count) return false;
			int count = left.Count;

			var sortedLeft = left.OrderBy(selector, comparer).ToArray();
			var sortedRigh = right.OrderBy(selector, comparer).ToArray();

			for (int i = 0; i < count; i++)
			{
				if (isEqual(sortedLeft[i], sortedRigh[i])) return false;
			}
			return true;
		}
	}

	public static class ExtensionsIListForId
	{
		public static bool Contains<T>(this IList<T> list, T element, int indexFrom, int indexTo) where T : IEquatable<T>
		{
			for (int i = indexFrom; i < indexTo; i++)
			{
				if (list[i].Equals(element)) return true;
			}
			return false;
		}
		public static bool ContainsId<T>(this IList<T> list, int id) where T : IziHardGames.Core.IUnique
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Id == id)
				{
					return true;
				}
			}
			return false;
		}
		public static bool TryRemoveById<T>(this IList<T> source, int id, out T item) where T : IUnique
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].Id == id)
				{
					item = source[i];

					source.RemoveAt(i);

					return true;
				}
			}

			item = default;

			return false;
		}
		public static bool TryRemoveById<T>(this IList<T> source, int id) where T : IUnique
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].Id == id)
				{
					source.RemoveAt(i);

					return true;
				}
			}

			return false;
		}
	}

	public static partial class ExtensionIEnumerable
	{
		public static bool IsMatchId<T>(this IEnumerable<T> source, int idCompare) where T : IUnique
		{
			foreach (var item in source)
			{
				if (item.Id == idCompare) return true;
			}

			return false;
		}
	}

	public static partial class ExtensionsIDictionary
	{
		public static int GetFreeExceptZeroIdAscending<TValue>(this IDictionary<int, TValue> keyValuePairs, int from)
		{
			int result = default;
			LABEL:
			{
				for (int i = from; i < int.MaxValue; i++)
				{
					if (!keyValuePairs.ContainsKey(i))
					{
						result = i;
						break;
					}
				}
				if (result == 0)
				{
					from = 1;
					goto LABEL;
				}
			}
			return result;
		}
	}
	public static partial class ExtensionsICollectionCore
	{
		public static int GetFreeIdMinToMaxExceptZero<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = int.MinValue;

			while (collection.ContainsId(newID))
			{
				newID++;

				if (newID == 0) continue;

				if (newID == int.MaxValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdZeroToMaxExceptZero<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = 1;

			while (collection.ContainsId(newID))
			{
				newID++;

				if (newID == int.MaxValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdZeroToMinExceptZero<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = -1;

			while (collection.ContainsId(newID))
			{
				newID--;

				if (newID == int.MinValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdAscendingExceptZero<T>(this ICollection<T> collection, int from) where T : IUnique
		{
			int newID = default;
			LABEL:
			{
				for (int i = from; i < int.MaxValue; i++)
				{
					if (!collection.ContainsId(i))
					{
						newID = i;
						break;
					}
				}
			}
			if (newID == 0)
			{
				from = 1;
				goto LABEL;
			}
			return newID;
		}

		public static int GetFreeIdDescendingExceptZero<T>(this ICollection<T> collection, int from) where T : IUnique
		{
			int newID = from;

			while (collection.ContainsId(newID))
			{
				newID--;

				if (newID == 0) continue;

				if (newID == int.MinValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdMinToMax<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = int.MinValue;

			while (collection.ContainsId(newID))
			{
				newID++;

				if (newID == int.MaxValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdZeroToMax<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = 0;

			while (collection.ContainsId(newID))
			{
				newID++;

				if (newID == int.MaxValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdZeroToMin<T>(this ICollection<T> collection) where T : IUnique
		{
			int newID = 0;

			while (collection.ContainsId(newID))
			{
				newID--;

				if (newID == int.MinValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}
		public static int GetFreeIdAscending<T>(this ICollection<T> collection, int from) where T : IUnique
		{
			int newID = from;

			while (collection.ContainsId(newID))
			{
				newID++;

				if (newID == int.MaxValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}

		public static int GetFreeIdDescending<T>(this ICollection<T> collection, int from) where T : IUnique
		{
			int newID = from;

			while (collection.ContainsId(newID))
			{
				newID--;

				if (newID == int.MinValue) throw new OverflowException($"Коничились свободные id");
			}
			return newID;
		}
	}
}
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
	public static partial class ExtensionsICollection
	{

	}

	public static partial class ExtensionsIList
	{
		public static bool IndexAtRange<T>(this IList<T> list, int index)
		{
			return 0 < index && index < list.Count;
		}
		public static void InsertOrderBy<T>(this IList<T> list, T value, Func<T, T, bool> func)
		{
			for (int i = 0; i < list.Count; i++)
			{
				//if (value <= list[i])
				if (func(value, list[i]))
				{
					list.Insert(i, value);

					return;
				}
			}

			list.Add(value);
		}
		public static IList<T> Inverse<T>(this IList<T> list)
		{
			var count = list.Count / 2;

			var listcount = list.Count;

			for (var i = 0; i < count; i++)
			{
				var temp = list[i];
				var inverseIndex = listcount - 1 - i;
				list[i] = list[inverseIndex];
				list[inverseIndex] = temp;
			}

			return list;
		}
		public static IList<T> SwapRightToLeftAndSetDefaultRight<T>(this IList<T> list, int left, int right)
		{
			list[left] = list[right];
			list[right] = default;

			return list;
		}
		public static void Swap<TList, TValue>(this ref TList list, int left, int right) where TList : unmanaged, IList<TValue>
		{
			var temp = list[left];
			list[left] = list[right];
			list[right] = temp;
		}
		public static TList Swap<TList, TValue>(this TList list, int left, int right) where TList : class, IList<TValue>
		{
			var temp = list[left];
			list[left] = list[right];
			list[right] = temp;

			return list;
		}
		/// <summary>
		/// Вставка числа в регион который долже быть упорядочен по возрастанию<br/>
		/// После вставки сдвигает элементы
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static IList<int> InsertAscending(this IList<int> list, int toMove, int delimeter) { throw new NotImplementedException("TBD"); }


		/// <summary>
		/// Зеркальная перестановка индексов.
		/// Условно список делится пополам. В параметре указывается индекс элемента от 0 в первой половине.
		/// Перестановка происходит с элементом равноудаленным от центра на противоположной стороне. 
		/// Пример Если параметр = 0 то произойдет перестановка с последним элементом
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool TrySimmetrySwap<T>(this IList<T> list, int index, out T newLeft, out T newRight)
		{
			if (list.Count > 1)
			{
				int indexEnd = list.Count - index - 1;
				T temp = list[index];
				list[index] = list[indexEnd];
				list[indexEnd] = temp;

				newLeft = list[indexEnd];
				newRight = list[index];

				return true;
			}
			newLeft = default;
			newRight = default;
			return false;
		}
	}

	public static partial class ExtensionsList
	{
		public static void CopyTo<T>(this List<T> list, List<T> target)
		{
			foreach (var item in list)
			{
				target.Add(item);
			}
		}
		public static List<T> Fill<T>(this List<T> list, T value, int count)
		{
			while (list.Count < count)
			{
				list.Add(default);
			}

			for (int i = 0; i < list.Count; i++)
			{
				list[i] = value;
			}

			return list;
		}
		/// <summary>
		/// Изъять из списка последний элемент
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T PullLast<T>(this List<T> list)
		{
			T item = list.Last();
			list.Remove(item);
			return item;
		}
		/// <summary>
		/// Изъять из списка первый элемент
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T PullFirst<T>(this List<T> list)
		{
			T item = list.First();
			list.RemoveAt(0);
			return item;
		}
		public static T PullAt<T>(this List<T> list, int index)
		{
			T item = list.ElementAt(index);
			list.RemoveAt(index);
			return item;
		}
		/// <summary>
		/// Сдвинуть элементы по часовой стрелке || вперед || вправо . То есть последний элемент станет первым и так заданной количество
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="count"></param>
		public static void CircleShiftRight<T>(this List<T> list, int count)
		{
			for (int i = 0; i < count; i++)
			{
				T temp = list.PullLast();
				list.Insert(0, temp);
			}
		}
		public static void CircleShiftLeft<T>(this List<T> list, int count)
		{
			for (int i = 0; i < count; i++)
			{
				T temp = list.PullFirst();
				list.Add(temp);
			}
		}
	}

	public static partial class ExtensionDictionary
	{
		/// <summary>
		/// Modes:<br/>
		/// int.Min	- from int.Min to int Max<br/>
		/// -1		- from 0 to int.Min<br/>
		/// 1		- from 0 to Int.Max<br/>
		/// int.Max - from int.max to Int min<br/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static int GetFreeKey<T>(this Dictionary<int, T> self, int mode, bool isExceptZero)
		{
			int key = default;

			switch (mode)
			{
				case int.MinValue:
					{
						key = int.MinValue;

						while (self.ContainsKey(key))
						{
							if (isExceptZero && key == 0)
							{
								key++;
							}
							key++;
						}
						return key;
					}
				case -1:
					{
						key = 0;

						while (self.ContainsKey(key))
						{
							if (isExceptZero && key == 0)
							{
								key--;
							}
							key--;
						}
						return key;
					}
				case 1:
					{
						key = 0;

						while (self.ContainsKey(key))
						{
							if (isExceptZero && key == 0)
							{
								key++;
							}
							key++;
						}
						return key;
					}
				case int.MaxValue:
					{
						key = int.MaxValue;

						while (self.ContainsKey(key))
						{
							if (isExceptZero && key == 0)
							{
								key--;
							}
							key--;
						}

						return key;
					}
				default:
					break;
			}
			throw new System.NotSupportedException("Mode value not supproted");
		}
	}
}

namespace System.Linq
{
	public static partial class ExtensionIEnumerable
	{
		/// <summary>
		/// Получить первый свободный int последовательности. Например если минимальное значение 5 то если послеждовательность не содежрит 6 будет возвращено 6
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static bool GetFirstGapInt(this IEnumerable<int> self, out int value)
		{
			if (self.Count() > 0)
			{
				int min = self.Min() + 1;

				foreach (var item in self)
				{
					if (self.Contains(min))
					{
						min++;
					}
					else
					{
						value = min;

						return true;
					}
				}
			}
			value = default;

			return false;
		}

		public static IEnumerable<T> LibTakeLast<T>(this IEnumerable<T> source, int N)
		{
			return source.Skip(Math.Max(0, source.Count() - N));
		}
		/// <summary>
		/// Any c указанием индекса совпадения
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="validator"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> validator, out int index)
		{
			index = 0;

			foreach (var item in source)
			{
				if (validator(item))
				{
					return true;
				}
				index++;
			}
			return false;
		}

		public static bool Any<T>(this IEnumerable<T> source, T compareTo) where T : IComparable<T>
		{
			foreach (var item in source)
			{
				if (item.CompareTo(compareTo) == 0) return true;
			}

			return false;
		}

	}
}
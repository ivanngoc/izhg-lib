using System;
using System.Buffers;
using System.Collections.Generic;
using static IziHardGames.Libs.Sorting.SortingArray;

namespace IziHardGames.Libs.Sorting
{
	public static class SortingIList
	{
		/// <summary>
		/// Сортировка вставками. Сортировка по выборке минимального элемента без аллокации, внутри самого списка
		/// </summary>
		/// <typeparam name="TList"></typeparam>
		/// <typeparam name="TElem"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparator"></param>
		/// <returns></returns>
		public static TList SortSelectionAscending<TList, TElem, TSelect>(TList list, Func<TElem, TSelect> selector)
			where TList : class, IList<TElem>
			where TSelect : IComparable<TSelect>
		{
			int count = list.Count;

			for (int i = 0; i < count; i++)
			{
				int index = i;
				TSelect minCurrent = selector(list[i]);

				for (int j = i + 1; j < count; j++)
				{
					TSelect val = selector(list[j]);

					if (minCurrent.CompareTo(val) > 0)
					{
						index = j;
						minCurrent = val;
					}
				}
				list.Swap<TList, TElem>(i, index);
			}
			return list;
		}

	}
	public static class Radix
	{
		private static T GetMax<T>(IList<T> arr, int n)
		where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
		{
			T max = arr[0];

			for (int i = 1; i < n; i++)
			{
				if (arr[i].CompareTo(max) > 0)
					max = arr[i];
			}
			return max;
		}
		private static void SortByNumberRunk(IList<int> list, int n, int rank)
		{
			int i;

			Span<int> count = stackalloc int[10];

			/// для каждой цифры в порядке exp подсчитать количество совпадений
			for (i = 0; i < n; i++)
			{
				count[(list[i] / rank) % 10]++;

			}

			/// резервирует сегменты в массиве для каждого exp-n разряда
			/// пример: для exp=10. для элементов массива у которых в разряде десятков стоит цифра 0 внутри массива будет выделенно count[0] место от 0
			/// для цифры 1 во втором разряде будет count[1] мест, но так как перед ним уже были элементы с цифрой 0 во втором разряде
			/// нужно прибавить count[0]. 
			/// Таким образом count[x] будет показывать не количество элементов с цифро в разряде а индекс в массиве с которого диапазон для этих чисел  начинается
			for (i = 1; i < 10; i++)
			{
				count[i] += count[i - 1];

			}

			int[] output = ArrayPool<int>.Shared.Rent(list.Count); // output array 

			for (i = n - 1; i >= 0; i--)
			{
				int index = (list[i] / rank) % 10;

				output[count[index] - 1] = list[i];

				count[index]--;

				Console.WriteLine("Count1: " + SortingArray.ArrayToString(count.ToArray()));
			}

			for (i = 0; i < n; i++)
			{
				list[i] = output[i];
			}

			ArrayPool<int>.Shared.Return(output);
		}
		public static void SortAscending<T>(IList<int> list, int n)
		{
			int m = GetMax(list, n);

			for (int exp = 1; m / exp > 0; exp *= 10)
			{
				SortByNumberRunk(list, n, exp);
			}
		}
		public static void SortDescending<T>(IList<int> list, int n)
		{
			int m = GetMax(list, n);

			for (int exp = 1; m / exp > 0; exp *= 10)
			{
				SortByNumberRunk(list, n, exp);
			}

			//list.Reverse();
			throw new System.NotImplementedException();
		}

		#region Not Implemented 
#if Experimental
            private static void CountSortTestNonAlloc(int[] arr, int n, int exp)
            {
#pragma warning disable
                throw new NotImplementedException("20201217");
                int[] output = new int[n]; // output array                 
                int i;
                Span<int> count = stackalloc int[10];

                Span<int> span = stackalloc int[10];
                Span<int> spanCount = stackalloc int[10];
                Span<int> spanIndexEnd = stackalloc int[10];

                /// для каждой цифры в порядке exp подсчитать количество совпадений
                for (i = 0; i < n; i++)
                {
                    count[(arr[i] / exp) % 10]++;
                    span[(arr[i] / exp) % 10]++;
                    spanCount[(arr[i] / exp) % 10]++;
                }

                Console.WriteLine($"[Span1 exp {exp}] {SortingArray.ArrayToString(count.ToArray())}");
                Console.WriteLine($"[Span2 exp {exp}] {SortingArray.ArrayToString(span.ToArray())}");

                /// резервирует сегменты в массиве для каждого exp-n разряда
                /// пример: для exp=10. для элементов массива у которых в разряде десятков стоит цифра 0 внутри массива будет выделенно count[0] место от 0
                /// для цифры 1 во втором разряде будет count[1] мест, но так как перед ним уже были элементы с цифрой 0 во втором разряде
                /// нужно прибавить count[0]. 
                /// Таким образом count[x] будет показывать не количество элементов с цифро в разряде а индекс в массиве с которого диапазон для этих чисел  начинается
                for (i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                    span[i] = count[i];
                    spanIndexEnd[i] = span[i];
                }

                Console.WriteLine($"[Span1 exp {exp}] {SortingArray.ArrayToString(count.ToArray())}");
                Console.WriteLine($"[Span2 exp {exp}] {SortingArray.ArrayToString(span.ToArray())}");

                Console.WriteLine();

                Console.WriteLine($"Cycle: ");
                Console.WriteLine($"Cycle: {SortingArray.ArrayToString(arr, "000")}");
                //// Build the output array 
                for (i = n - 1; i >= 0; i--)
                {
                    //Console.Write($"(arr[i] / exp) % 10 = {(arr[i] / exp) % 10}| ");
                    //Console.Write($"count[x] = {count[(arr[i] / exp) % 10]}| ");

                    int index = (arr[i] / exp) % 10;

                    Console.WriteLine($"{i}|{arr[i]}|{index}| [count[index]-1] = {count[index] - 1}> ");

                    //Console.Write($"{arr[i]} => {count[index] - 1}| ");

                    output[count[index] - 1] = arr[i];

                    count[index]--;

                    //Console.Write($"count[x]-- = {count[(arr[i] / exp) % 10]}| ");
                    //Console.WriteLine();
                    Console.WriteLine("Count1: " + SortingArray.ArrayToString(count.ToArray()));
                }


                Console.WriteLine("ARR1: " + SortingArray.ArrayToString(output, "000"));
                Console.WriteLine();
                Console.WriteLine("Sep");
                var arr2 = arr.ToArray();
                var arrayCount = spanCount.ToArray();

                if (true)
                {
                    Console.WriteLine(SortingArray.ArrayToString(span.ToArray()) + " span");
                    Console.WriteLine(SortingArray.ArrayToString(spanCount.ToArray()) + " spanCount");
                    Console.WriteLine(SortingArray.ArrayToString(spanIndexEnd.ToArray()) + " spanIndexEnd");
                    Console.WriteLine($"V2 POW {exp}");

                    int tempIndex = n - 1;
                    bool isNextTemp = false;

                    int indexFromStart = default;

                    string append = default;

                    for (int j = tempIndex; j >= 0; j--)
                    {
                        Console.WriteLine($"{Environment.NewLine}");

                        int temp = arr2[tempIndex];

                        int indexSpan = (arr2[j] / exp) % 10;
                        int indexSwap = span[indexSpan] - 1;

                        bool isSkip = indexSwap < 0;
                        bool isDontNeedSwap = (
                            //(span[indexSpan] > 0) ||
                            ((spanIndexEnd[indexSpan] - 1 - spanCount[indexSpan]) <= j) &&
                            (j < spanIndexEnd[indexSpan])
                            );

                        Console.Write(SortingArray.ArrayToString(arr2, "000") + $"| j {j.ToString("00")} | tempI {tempIndex.ToString("00")}| tempVal {arr2[tempIndex].ToString("000")}| cureVal {arr2[j].ToString("000")}|? {isDontNeedSwap}|");
                        append = default;
                        if (isDontNeedSwap)
                        {
                            indexSpan = (temp / exp) % 10;
                            indexSwap = span[indexSpan] - 1;

                            if (!isSkip && indexSwap == tempIndex)
                            {
                                tempIndex = indexFromStart;
                                indexFromStart++;
                            }
                            else
                            {
                                if (indexSwap < 0) continue;
                                int temp2 = arr2[tempIndex];
                                arr2[tempIndex] = arr2[indexSwap];
                                arr2[indexSwap] = temp2;
                                append = $"| SWAP {temp2.ToString("000")} => {arr2[tempIndex].ToString("000")}";
                            }
                        }
                        else
                        {
                            int temp2 = arr2[j];
                            arr2[j] = arr2[indexSwap];
                            arr2[indexSwap] = temp2;
                            append = $"| SWAP {temp2.ToString("000")} => {arr2[j].ToString("000")}";
                            tempIndex = j;
                        }
                        Console.Write(append);
                        span[indexSpan]--;
                        arrayCount[indexSpan]--;
                        Console.Write($"|{indexSpan}: {arrayCount[indexSpan]}");
                    }
                    //Console.WriteLine($"arr2[j]={tempIternal}|arr2[swapIndex]={arr2[j] }");
                }

                Console.WriteLine();

                // Copy the output array to arr[], so that arr[] now 
                // contains sorted numbers according to current 
                // digit 
                for (i = 0; i < n; i++)
                {
                    arr[i] = output[i];
                }

                Console.WriteLine("ARR1 " + SortingArray.ArrayToString(arr, "0000"));
                Console.WriteLine("ARR2 " + SortingArray.ArrayToString(arr2, "0000"));
#pragma warning restore
            }

#endif
		#endregion

	}

	public static class SortByInserts
	{
		public static void SortAscending<T>(IList<T> list, Func<T, int> selector)
		{
			if (list.Count > 1)
			{
				int indexMin = 0;

				int count = list.Count - 1;

				for (int i = 0; i < count; i++)
				{
					int min = selector(list[i]);

					for (int j = i + 1; j < list.Count; j++)
					{
						int minTemp = selector(list[j]);

						if (min > minTemp)
						{
							indexMin = j;
							min = minTemp;
						}
					}

					T temp = list[i];
					list[i] = list[indexMin];
					list[indexMin] = temp;
				}
			}
		}
		/// <summary>
		/// Сортировка вставками через поиск в каждей иттерации минимального значения
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="selector"></param>
		public static void SortAscending<T>(IList<T> list, Func<T, float> selector)
		{
			if (list.Count > 1)
			{
				int indexMin = 0;

				int count = list.Count - 1;

				for (int i = 0; i < count; i++)
				{
					float min = selector(list[i]);

					for (int j = i + 1; j < list.Count; j++)
					{
						float minTemp = selector(list[j]);

						if (min > minTemp)
						{
							indexMin = j;
							min = minTemp;
						}
					}

					T temp = list[i];
					list[i] = list[indexMin];
					list[indexMin] = temp;
				}
			}
		}
		public static CompareInfo SortingInsert(int[] array)
		{
			CompareInfo compareInfo = new CompareInfo();

			for (int i = 0; i < array.Length; i++)
			{
				int index = Min(i, array.Length);

				Swap(i, index);
			}
			//Console.WriteLine(ArrayToString(array));

			return compareInfo;

			void Swap(int x, int y)
			{
				compareInfo.countSwap++;
				int temp = array[x];
				array[x] = array[y];
				array[y] = temp;
			}

			int Min(int from, int to)
			{
				int minIndex = from;

				for (int i = from; i < to; i++)
				{
					compareInfo.countItteration++;
					compareInfo.countComparison++;

					if (array[minIndex] > array[i])
					{
						minIndex = i;
					}
				}
				return minIndex;
			}
		}
		public static void SortingInsertClean(int[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				int index = i;

				for (int j = i; j < array.Length; j++)
				{
					if (array[index] > array[j])
					{
						index = j;
					}
				}

				int temp = array[i];
				array[i] = array[index];
				array[index] = temp;
			}
		}
	}

	public static class SortWithBubble
	{
		public static void SortFloatAscending<T>(IList<T> list, Func<T, float> selector)
		{
			if (list.Count > 1)
			{
				bool isSwapped = true;

				while (isSwapped)
				{
					isSwapped = false;

					for (int i = 1; i < list.Count; i++)
					{
						int index = i - 1;
						float left = selector(list[index]);
						float right = selector(list[i]);

						if (left > right)
						{
							T temp = list[index];
							list[index] = list[i];
							list[i] = temp;

							isSwapped = true;
						}
					}
				}
			}
		}
		public static void SortIntAscending(int[] array)
		{
			bool isSWaped = true;

			while (isSWaped)
			{
				isSWaped = false;

				for (int i = 1; i < array.Length; i++)
				{
					int index = i - 1;
					int left = array[index];
					int right = array[i];

					if (left > right)
					{
						int temp = array[index];
						array[index] = array[i];
						array[i] = temp;

						isSWaped = true;
					}
				}
			}
		}
		public static CompareInfo SortingBubble(int[] array)
		{
			CompareInfo compareInfo = new CompareInfo();

			// cache line is 64 then sort it buy blocks = 16 int per once
			// sort ascending
			int count = array.Length;

			bool isSWaped = true;

			while (isSWaped)
			{
				isSWaped = false;

				for (int i = 1; i < count; i++)
				{
					compareInfo.countItteration++;
					int left = array[i - 1];
					int right = array[i];

					compareInfo.countComparison++;
					if (left > right)
					{
						Swap(i - 1, i);
						isSWaped = true;
					}
				}
			}
			//Console.WriteLine(ArrayToString(array));
			return compareInfo;

			void Swap(int x, int y)
			{
				compareInfo.countSwap++;
				int temp = array[x];
				array[x] = array[y];
				array[y] = temp;
			}
		}

	}
}
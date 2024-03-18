using System;
using System.Buffers;
using System.Collections.Generic;

namespace IziHardGames.Libs.Sorting
{

	public partial class SortingArray
	{
		public class Radix
		{
			private static int GetMax(int[] arr, int n)
			{
				int mx = arr[0];
				for (int i = 1; i < n; i++)
					if (arr[i] > mx)
						mx = arr[i];
				return mx;
			}

			private static void SortByRank(int[] arr, int size, int rank)
			{
				int i;
				Span<int> count = stackalloc int[10];

				/// для каждой цифры в порядке exp подсчитать количество совпадений
				for (i = 0; i < size; i++)
				{
					count[(arr[i] / rank) % 10]++;
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

				int[] output = ArrayPool<int>.Shared.Rent(arr.Length); // output array 

				for (i = size - 1; i >= 0; i--)
				{
					int index = (arr[i] / rank) % 10;

					output[count[index] - 1] = arr[i];

					count[index]--;
				}

				for (i = 0; i < size; i++)
				{
					arr[i] = output[i];
				}
				ArrayPool<int>.Shared.Return(output);
			}

			public static void SortAscending(int[] arr, int size)
			{
				int m = GetMax(arr, size);

				for (int exp = 1; m / exp > 0; exp *= 10)
				{
					SortByRank(arr, size, exp);
				}
			}

			public static void SortDescending(int[] arr, int size)
			{
				int m = GetMax(arr, size);

				for (int exp = 1; m / exp > 0; exp *= 10)
				{
					SortByRank(arr, size, exp);
				}

				arr.Inverse();
			}

#if Experimental
            private static void countSortTestNonAlloc(int[] arr, int n, int exp)
            {
                int[] output = new int[n];
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

                    int index = (arr[i] / exp) % 10;

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
            }
#endif
		}

	}



}

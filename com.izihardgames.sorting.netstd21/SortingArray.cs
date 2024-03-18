using System;
using System.Linq;

namespace IziHardGames.Libs.Sorting
{
	public partial class SortingArray
	{
		public static string ArrayToString(int[] array)
		{
			return array.Select(x => x.ToString()).Aggregate((z, y) => z + "|" + y);
		}
		public static string ArrayToString(int[] array, string format)
		{
			return array.Select(x => x.ToString(format)).Aggregate((z, y) => z + "|" + y);
		}


#if UNITY_EDITOR
		private static class RandomArray
		{
			private static Random random = new Random();

			public static int[] Create(int size)
			{
				int[] array = new int[size];

				for (int i = 0; i < size; i++)
				{
					array[i] = i;
				}

				Shuffle(array, array.Length);

				return array;
			}

			public static void Shuffle(int[] array, int count)
			{
				for (int i = 0; i < count; i++)
				{
					int pusValue = random.Next(0, count);

					int temp = array[pusValue];
					array[pusValue] = array[i];
					array[i] = temp;
				}
			}
		}
		public static void TestSort()
		{
			if (false)
			{
				var array = RandomArray.Create(16);

				var array1 = array.ToArray();
				var array2 = array.ToArray();
				var array3 = array.ToArray();
				var array4 = array.ToArray();
				var array5 = array.ToArray();

				//Stopwatch stopwatch = Stopwatch.StartNew();
				//if (false) SortingArray.SortingBubbleClean(array1);
				//stopwatch.Stop();
				//Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
				//stopwatch.Reset();


				//stopwatch.Start();
				//if (false) SortingArray.SortingInsertClean(array2);
				//stopwatch.Stop();
				//Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
				//stopwatch.Reset();

				//stopwatch.Start();
				//array3 = array3.OrderBy(x => x).ToArray();
				//stopwatch.Stop();
				//Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
				//stopwatch.Reset();

				//stopwatch.Start();
				//SortingArray.Merge.MergeSort(array4);
				//stopwatch.Stop();
				//Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
				//stopwatch.Reset();

				//stopwatch.Start();
				//Radix.SortAscending(array5, array4.Length);
				//stopwatch.Stop();
				//Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
				//stopwatch.Reset();


				//Console.WriteLine($"array4 {SortingArray.ArrayToString(array4)}");
				//Console.WriteLine($"array5 {SortingArray.ArrayToString(array5)}");
				Console.ReadLine();
				//Console.WriteLine(SortingArray.ArrayToString(array));
				//Console.WriteLine();
			}
			{
				//int[] array = new int[] { 1, 8, 5, 4, 27, 36, 55, 83, 101, 479, 872, 520 };

				int[] array = new int[] { 0520, 0005, 0055, 0008, 0036, 0027, 0004, 0083, 0001, 0479, 0101, 0872 };

				//RandomArray.Shuffle(array, array.Length);
				Console.WriteLine(SortingArray.ArrayToString(array, "0000"));
				Radix.SortAscending(array, array.Length);
				Console.WriteLine(SortingArray.ArrayToString(array));

			}
			{
				Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}New Block");

				//var array = RandomArray.Create(16);
				//var array1 = array.ToArray();
				//var array2 = array.ToArray();
				//SortingArray.SortingBubbleClean(array1);
				//SortingArray.SortingInsertClean(array2);

				//Console.WriteLine($"{SortingArray.ArrayToString(array1)} | ");
				//Console.WriteLine($"{SortingArray.ArrayToString(array2)} | ");
				Console.ReadLine();
			}
		}
#endif



		public struct CompareInfo
		{
			public int countComparison;
			public int countItteration;
			public int countSwap;

			public override string ToString()
			{
				return $"Comparison {countComparison}| Itteration {countItteration}| Swap {countSwap}";
			}
		}

	}



}

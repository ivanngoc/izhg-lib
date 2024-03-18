using System;
using System.Collections.Generic;
using System.Linq;


namespace IziHardGames.Libs.NonEngine.Sorting
{
	public static class Insertations
	{
	}
	public static class InsertationsList
	{
		public static int InsertWithBinaryTreeDescending<T>(this List<T> list, T item) where T : IComparable<T>
		{
			int count = list.Count;
			if (count > 0)
			{
				if (count > 1)
				{
					int shift = count >> 1;

					int indexRangeFirst = default;
					int indexRangeMidAtRight = shift;
					int indexRangeLast = count - 1;
					int countRange = count;


					while (true)
					{
						T midAtRight = list[indexRangeMidAtRight];
						int compareMidToItem = midAtRight.CompareTo(item);

						if (compareMidToItem < 0)
						{/// choose left branch
							indexRangeLast = indexRangeMidAtRight - 1;
							countRange = indexRangeMidAtRight - indexRangeFirst;

							if (countRange > 1)
							{
								indexRangeMidAtRight = (countRange >> 1) + indexRangeFirst;
								continue;
							}
							else
							{
								if (countRange < 1)
								{
									list.Insert(indexRangeMidAtRight, item);
									return indexRangeMidAtRight;
								}
								else
								{
									indexRangeMidAtRight = indexRangeLast;
									continue;
								}
							}
						}
						else
						{
							if (compareMidToItem > 0) //item is more than midItem case true
							{/// choose Right branch
								countRange = (countRange & 1) + (countRange >> 1);

								if (countRange > 1)
								{
									indexRangeFirst = indexRangeMidAtRight;
									indexRangeMidAtRight = indexRangeFirst + (countRange >> 1);
									continue;
								}
								else
								{
									int insertIndex = indexRangeMidAtRight + 1;
									list.Insert(insertIndex, item);
									return insertIndex;
								}

							}
							else
							{/// Lucky found item==midItem
								list.Insert(indexRangeMidAtRight, item);
								return indexRangeMidAtRight;
							}
						}
					}
				}
				else
				{
					T other = list[0];

					if (item.CompareTo(other) > 0)
					{
						list.Insert(0, item);
						return 0;
					}
					else
					{
						list.Add(item);
						return 1;
					}
				}
			}
			else
			{
				list.Add(item);
				return 0;
			}
		}
		/// <summary>
		/// Items that equal to each other don't have specific order and just inserted near each other depended at itteration they compared.
		/// If there is 1 item and inserted equal item then item will be inserted before existed item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">
		/// Must be sorted ascending. Exm: 0,1,2,3,4
		/// </param>
		/// <param name="item"></param>
		public static int InsertWithBinaryTreeAscending<T>(this List<T> list, T item) where T : IComparable<T>
		{
			int count = list.Count;
			if (count > 0)
			{
				if (count > 1)
				{
					int shift = count >> 1;

					int indexRangeFirst = default;
					int indexRangeMidAtRight = shift;
					int indexRangeLast = count - 1;
					int countRange = count;


					while (true)
					{
						T midAtRight = list[indexRangeMidAtRight];
						int compareMidToItem = midAtRight.CompareTo(item);

						if (compareMidToItem > 0)
						{/// choose left branch
							indexRangeLast = indexRangeMidAtRight - 1;
							countRange = indexRangeMidAtRight - indexRangeFirst;

							if (countRange > 1)
							{
								indexRangeMidAtRight = (countRange >> 1) + indexRangeFirst;
								continue;
							}
							else
							{
								if (countRange < 1)
								{
									list.Insert(indexRangeMidAtRight, item);
									return indexRangeMidAtRight;
								}
								else
								{
									indexRangeMidAtRight = indexRangeLast;
									continue;
								}
							}
						}
						else
						{
							if (compareMidToItem < 0) //item is more than midItem case true
							{/// choose Right branch
								countRange = (countRange & 1) + (countRange >> 1);

								if (countRange > 1)
								{
									indexRangeFirst = indexRangeMidAtRight;
									indexRangeMidAtRight = indexRangeFirst + (countRange >> 1);
									continue;
								}
								else
								{
									int insertIndex = indexRangeMidAtRight + 1;
									list.Insert(insertIndex, item);
									return insertIndex;
								}

							}
							else
							{/// Lucky found item==midItem
								list.Insert(indexRangeMidAtRight, item);
								return indexRangeMidAtRight;
							}
						}
					}
				}
				else
				{
					T other = list[0];

					if (item.CompareTo(other) < 0)
					{
						list.Insert(0, item);
						return 0;
					}
					else
					{
						list.Add(item);
						return 1;
					}
				}
			}
			else
			{
				list.Add(item);
				return 0;
			}
		}

		#region Test
#if DEBUG
		public static void SortingTest1Descending()
		{
			List<int> list = new List<int>() {
				7,
				8,
				1,
				1,
				-1,
				500,
				4,
				2,
				0,
				2};
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			int[] etalon = list.OrderByDescending(x => x).ToArray();
			int[] etalonTemp = default;
			int step = default;
			foreach (var item in list)
			{
				//list2.InsertWithBinaryTreeDescending(item);
				list3.InsertWithBinaryTreeDescending(item);
				etalonTemp = list3.OrderByDescending(x => x).ToArray();

				var ff = list3.ToArray();

				for (int i = 0; i < etalonTemp.Length; i++)
				{
					if (etalonTemp[i] != ff[i]) throw new Exception();
				}
				step++;
			}

		}
		public static void SortingTest2Descending()
		{
			for (int j = 0; j < 1000; j++)
			{
				int size = 10000;
				int[] testSource = new int[size];
				List<int> testList = new List<int>(size);

				Random random = new Random();

				for (int i = 0; i < size; i++)
				{
					testSource[i] = random.Next(int.MinValue, int.MaxValue);
				}

				int[] testFinal = testSource.OrderByDescending(x => x).ToArray();

				foreach (var item in testSource)
				{
					testList.InsertWithBinaryTreeDescending(item);
				}
				int[] testFinal2 = testList.ToArray();

				for (int i = 0; i < size; i++)
				{
					if (testFinal[i] != testFinal2[i])
					{
						throw new Exception();
					}
				}
			}
		}
		public static void SortingTest3Descending()
		{
			int size = 10;
			int[] source = new int[size];

			source[size - 1] = int.MinValue;

			List<int> testList = new List<int>(size);

			for (int i = 0; i < size; i++)
			{
				testList.InsertWithBinaryTreeDescending(source[i]);
			}
			if (testList[size - 1] != int.MinValue) throw new Exception();
		}
		public static void SortingTest4Descending()
		{
			int size = 10;
			int[] source = new int[size];

			source[size - 1] = int.MaxValue;

			List<int> testList = new List<int>(size);

			for (int i = 0; i < size; i++)
			{
				testList.InsertWithBinaryTreeDescending(source[i]);
			}
			if (testList[0] != int.MaxValue) throw new Exception();
		}


		public static void SortingTest1()
		{
			List<int> list = new List<int>() {
				2075920366,
				1096412460,
				1710394830,
				1379429816,
				786993914,
				-265133590,
				-1232953637,
				-1822176965,
				1531620344,
				533253269 };
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			int[] etalon = list.OrderBy(x => x).ToArray();
			int[] etalonTemp = default;
			int step = default;
			foreach (var item in list)
			{
				//list2.InsertWithBinaryTreeDescending(item);
				list3.InsertWithBinaryTreeAscending(item);
				etalonTemp = list3.OrderBy(x => x).ToArray();

				var ff = list3.ToArray();

				for (int i = 0; i < etalonTemp.Length; i++)
				{
					if (etalonTemp[i] != ff[i]) throw new Exception();
				}
				step++;
			}

		}
		public static void SortingTest2()
		{
			for (int j = 0; j < 1000; j++)
			{
				int size = 10000;
				int[] testSource = new int[size];
				List<int> testList = new List<int>(size);

				Random random = new Random();

				for (int i = 0; i < size; i++)
				{
					testSource[i] = random.Next(int.MinValue, int.MaxValue);
				}

				int[] testFinal = testSource.OrderBy(x => x).ToArray();

				foreach (var item in testSource)
				{
					testList.InsertWithBinaryTreeAscending(item);
				}
				int[] testFinal2 = testList.ToArray();

				for (int i = 0; i < size; i++)
				{
					if (testFinal[i] != testFinal2[i])
					{
						throw new Exception();
					}
				}
			}
		}
		public static void SortingTest3()
		{
			int size = 100000;
			int[] source = new int[size];

			source[size - 1] = int.MaxValue;

			List<int> testList = new List<int>(size);

			for (int i = 0; i < size; i++)
			{
				testList.InsertWithBinaryTreeAscending(source[i]);
			}
			if (testList[size - 1] != int.MaxValue) throw new Exception();
		}

#endif
		#endregion
	}// class
}// namespace
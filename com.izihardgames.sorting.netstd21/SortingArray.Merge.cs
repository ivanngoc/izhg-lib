namespace IziHardGames.Libs.Sorting

{
	public partial class SortingArray
	{
		public class Merge
		{
			/// <summary>
			/// SortingMergeClean
			/// </summary>
			/// <param name="array"></param>
			/// <param name="lowIndex"></param>
			/// <param name="middleIndex"></param>
			/// <param name="highIndex"></param>
			private static void MergeResult(int[] array, int lowIndex, int middleIndex, int highIndex)
			{
				var left = lowIndex;
				var right = middleIndex + 1;
				var tempArray = new int[highIndex - lowIndex + 1];
				var index = 0;

				while ((left <= middleIndex) && (right <= highIndex))
				{
					if (array[left] < array[right])
					{
						tempArray[index] = array[left];
						left++;
					}
					else
					{
						tempArray[index] = array[right];
						right++;
					}

					index++;
				}

				for (var i = left; i <= middleIndex; i++)
				{
					tempArray[index] = array[i];
					index++;
				}

				for (var i = right; i <= highIndex; i++)
				{
					tempArray[index] = array[i];
					index++;
				}

				for (var i = 0; i < tempArray.Length; i++)
				{
					array[lowIndex + i] = tempArray[i];
				}
			}
			private static int[] MergeSortSplit(int[] array, int lowIndex, int highIndex)
			{
				if (lowIndex < highIndex)
				{
					int middleIndex = (lowIndex + highIndex) / 2;
					MergeSortSplit(array, lowIndex, middleIndex);
					MergeSortSplit(array, middleIndex + 1, highIndex);
					MergeResult(array, lowIndex, middleIndex, highIndex);
				}

				return array;
			}
			public static int[] MergeSort(int[] array)
			{
				return MergeSortSplit(array, 0, array.Length - 1);
			}

		}

	}



}

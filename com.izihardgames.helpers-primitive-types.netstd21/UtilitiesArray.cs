using Random = System.Random;

namespace IziHardGames.Libs.Arrays
{
	public static class UtilitiesArray
	{

	}
	public static class RandomArray
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
}
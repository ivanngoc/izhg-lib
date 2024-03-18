using Random = System.Random;

namespace IziHardGames.Libs.RandomAlgs
{
	public partial class Randomizer
	{
		public static Random random = new Random();
		public static void ArrayShuffle<T>(T[] array, int indexFrom, int indexTo)
		{
			for (var i = indexFrom; i < indexTo; i++)
			{
				var pusValue = random.Next(indexFrom, indexTo);

				var temp = array[pusValue];
				array[pusValue] = array[indexTo];
				array[indexTo] = temp;
			}
		}

	}
}
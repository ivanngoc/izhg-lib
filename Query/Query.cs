using System;
using System.Collections.Generic;


namespace IziHardGames.Libs.NonEngine.Query
{
	public class Query
	{
		/// Один из способов применения для разработки:
		/// DataSet - результат Query.
		/// Доступ к DataSet глобальный.
	}

	public static class QueryIListExtensions
	{
		public static bool QueryWhereIs<T>(this IList<T> self, Func<T, bool> func, ref int indexFounded, ref int indexCurrent, ref T result)
		{
			bool isBreak = default;

			for (int i = indexFounded; i < self.Count; i++)
			{
				indexCurrent = i;

				if (isBreak)
				{
					break;
				}
				if (func(self[i]))
				{
					indexFounded = i;

					result = self[i];

					isBreak = true;
				}
			}
			return isBreak;
		}
	}
}
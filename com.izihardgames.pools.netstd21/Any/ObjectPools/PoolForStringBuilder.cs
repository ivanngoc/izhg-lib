using System.Collections.Generic;
using System.Text;

namespace IziHardGames.Libs.Engine.Memory
{
	public static class PoolForStringBuilder
	{
		private static List<StringBuilder> pool = new List<StringBuilder>();
		private static List<StringBuilder> active = new List<StringBuilder>();

		/// <summary>
		/// renting wont moving item from <see cref="pool"/> list to <see cref="active"/>
		/// </summary>
		/// <param name="capacity"></param>
		/// <returns></returns>
		public static StringBuilder RentForUsingOnce(int capacity)
		{
			foreach (var item in pool)
			{
				if (item.Capacity >= capacity)
				{
					return item;
				}
			}
			StringBuilder newObj = new StringBuilder(capacity);
			pool.Add(newObj);
			return newObj;
		}

		public static void Dispose()
		{
			pool.Clear();
			active.Clear();
		}
	}
}
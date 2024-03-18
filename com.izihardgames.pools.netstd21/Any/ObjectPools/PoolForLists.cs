using System.Collections.Generic;

namespace IziHardGames.Libs.Engine.Memory
{
	public class PoolForLists<TItem>
	{
		private static readonly List<TItem> empty = new List<TItem>(0);
		public static List<TItem> Empty()
		{
			return empty;
		}
	}
}
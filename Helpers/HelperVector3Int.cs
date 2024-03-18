using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{
	public static class HelperVector3Int
	{
		public static Vector3Int IndexToCoord2D(int index, Vector3Int sizeMap, Vector3Int origin)
		{
			return IndexToCoord2D(index, (Vector2Int)sizeMap, (Vector2Int)origin);
		}
		public static Vector3Int IndexToCoord2D(int index, Vector2Int sizeMap, Vector2Int origin)
		{
			int y = index / sizeMap.x;
			int x = index - y * sizeMap.x;

			return new Vector3Int(x + origin.x, y + origin.y, 0);
		}
		/// <summary>
		/// Один бануд назодится внутри другого баунда или идентичен ему - <see langword="true"/>
		/// </summary>
		/// <param name="minOne"></param>
		/// <param name="maxOne"></param>
		/// <param name="minTwo"></param>
		/// <returns></returns>
		public static bool IsRectOneInsideRectTwo(ref Vector3Int minOne, ref Vector3Int maxOne, ref Vector3Int minTwo, ref Vector3Int maxTwo)
		{
			bool sideLeft = minOne.x >= minTwo.x;

			if (sideLeft)
			{
				bool sideBot = minOne.y >= minTwo.y;

				if (sideBot)
				{
					bool sideRight = maxOne.x <= maxTwo.x;

					if (sideRight)
					{
						bool sideTop = maxOne.y <= maxTwo.y;

						if (sideTop)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
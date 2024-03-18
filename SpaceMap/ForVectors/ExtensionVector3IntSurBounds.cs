using Vector3Int = IziHardGames.Libs.NonEngine.Vectors.Point3Int;

namespace IziHardGames.Libs.NonEngine.SpaceMap
{
	public static class ExtensionVector3IntSurBounds
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="min">from <see cref="Tilemap.cellBounds"/></param>
		/// <param name="max">from <see cref="Tilemap.cellBounds"/></param>
		/// <returns></returns>
		public static bool IsOutOfBoundsXY(this Vector3Int vector3Int, Vector3Int min, Vector3Int max)
		{
			if (min.x > vector3Int.x) return true;
			if (min.y > vector3Int.y) return true;

			if (max.x - 1 < vector3Int.x) return true;
			if (max.y - 1 < vector3Int.y) return true;

			return false;
		}
	}
}
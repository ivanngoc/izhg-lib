using IziHardGames.Libs.NonEngine.Vectors;

namespace UnityEngine
{
	public static class ExtensionsForVector3IntForBounds
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="min">from <see cref="Tilemap.cellBounds"/></param>
		/// <param name="max">from <see cref="Tilemap.cellBounds"/></param>
		/// <returns></returns>
		public static bool IsOutOfBoundsXY(this Vector3Int vector3Int, Point3Int min, Point3Int max)
		{
			if (min.x > vector3Int.x) return true;
			if (min.y > vector3Int.y) return true;

			if (max.x - 1 < vector3Int.x) return true;
			if (max.y - 1 < vector3Int.y) return true;

			return false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="min">from <see cref="Tilemap.cellBounds"/></param>
		/// <param name="max">from <see cref="Tilemap.cellBounds"/></param>
		/// <returns></returns>
		public static bool IsOutOfBoundsXY(ref this Vector3Int vector3Int, ref Point3Int min, ref Point3Int max)
		{
			if (min.x > vector3Int.x) return true;
			if (min.y > vector3Int.y) return true;

			if (max.x - 1 < vector3Int.x) return true;
			if (max.y - 1 < vector3Int.y) return true;

			return false;
		}
	}
}
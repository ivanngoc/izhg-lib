using System;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	/// <summary>
	/// Id преобразования для поля с четырезугольной ячейкой
	/// </summary>

	public static class ExtensionVector3IntToQuadIndexes
	{
		public static int ToCellIndex(this Vector3Int self, ref Vector3Int size, int precisionGrade)
		{
			Vector3Int shifted = new Vector3Int(self.x >> precisionGrade, self.y >> precisionGrade, self.z >> precisionGrade);

			return shifted.ToCellIndex(ref size);
		}
		public static int ToCellIndex(this Vector3Int self, ref Vector3Int size, ref Vector3Int origin, int precisionGrade)
		{
			Vector3Int shifted = new Vector3Int(self.x >> precisionGrade, self.y >> precisionGrade, self.z >> precisionGrade);

			return shifted.ToCellIndex(ref size, ref origin);
		}
		public static int ToCellIndex(this Vector3Int self, ref Vector3Int size, ref Vector3Int origin)
		{
#if UNITY_EDITOR
			if (self.IsOutOfBoundsXY(origin, origin + size)) throw new ArgumentOutOfRangeException();
#endif
			return (self - origin).ToCellIndex(ref size);
		}

		public static int ToCellIndex(this Vector3Int v, ref Vector2Int boundsDelta)
		{
#if UNITY_EDITOR
			if (v.IsOutOfBoundsXY(Vector3Int.zero, new Vector3Int(boundsDelta.x, boundsDelta.y, 0))) throw new ArgumentOutOfRangeException();
#endif
			return v.y * boundsDelta.x + v.x;
		}
		public static int ToCellIndex(this Vector3Int v, ref Vector3Int boundsDelta)
		{
#if UNITY_EDITOR
			if (v.IsOutOfBoundsXY(Vector3Int.zero, boundsDelta)) throw new ArgumentOutOfRangeException();
#endif
			return v.y * boundsDelta.x + v.x;
		}


		// non ref
		public static int ToCellIndex(this Vector3Int self, Vector2Int size, Vector2Int origin)
		{
			return ((Vector3Int)((Vector2Int)self - origin)).ToCellIndex(size);
		}
		public static int ToCellIndex(this Vector3Int self, Vector3Int size, Vector3Int origin)
		{
			return (self - origin).ToCellIndex(size);
		}
		public static int ToCellIndex(this Vector3Int self, Vector2Int size, Vector3Int origin)
		{
			return (self - origin).ToCellIndex(size);
		}
		public static int ToCellIndex(this Vector3Int v, Vector2Int boundsDelta)
		{
			return v.y * boundsDelta.x + v.x;
		}
		/// <summary>
		/// Index from min. Min is origin and == 0,0,0
		/// </summary>
		/// <param name="v"></param>
		/// <param name="boundsDelta"></param>
		/// <returns></returns>
		public static int ToCellIndex(this Vector3Int v, Vector3Int boundsDelta)
		{
			return v.y * boundsDelta.x + v.x;
		}
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
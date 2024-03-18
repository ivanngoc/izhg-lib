using System;

namespace UnityEngine
{
	public static partial class ExtensionVector2IntToCellIndex
	{
		public static int ToCellIndex(this ref Vector2Int self, ref Vector2Int size, int precisionGrade)
		{
			Vector2Int shifted = new Vector2Int(self.x >> precisionGrade, self.y >> precisionGrade);

			return shifted.ToCellIndex(ref size);
		}
		public static int ToCellIndex(this ref Vector2Int v, ref Vector2Int sizeMapIn)
		{
#if UNITY_EDITOR
			if (((Vector3Int)v).IsOutOfBoundsXY(Vector3Int.zero, new Vector3Int(sizeMapIn.x, sizeMapIn.y, 0))) throw new ArgumentOutOfRangeException();
#endif
			return v.y * sizeMapIn.x + v.x;
		}
		public static int ToCellIndex(this ref Vector2Int self, ref Vector2Int size, ref Vector2Int origin)
		{
			var temp = (self - origin);

			return temp.ToCellIndex(ref size);
		}
	}
}
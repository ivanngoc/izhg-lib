using System;

namespace UnityEngine
{

	public static partial class ExtensionsVector3
	{
		public static string ToStringAsFloat(this Vector3 vector3, int precision)
		{
			return $"({vector3.x.ToString($"F{precision}")}, {vector3.y.ToString($"F{precision}")}, {vector3.z.ToString($"F{precision}")}";
		}
		public static Vector3 GetCopyWithZ(this Vector3 vector3, float z)
		{
			return new Vector3(vector3.x, vector3.y, z);
		}

		public static int ToCellIndexXY(this ref Vector3 pos, ref Vector2Int size)
		{
			int x = Mathf.CeilToInt(pos.x);
			int y = Mathf.CeilToInt(pos.y);

			return y * size.x + x;
		}
		public static int ToCellIndexXYZ(this ref Vector3 pos)
		{
			throw new NotImplementedException();
		}
	}
}
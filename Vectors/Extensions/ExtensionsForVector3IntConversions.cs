using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	public static class ExtensionsForVector3IntConversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point3Int ToPoint3Int(this Vector3Int input)
		{
			return new Point3Int(input.x, input.y, input.z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2Int ToPoint2Int(this Vector3Int input)
		{
			return new Point2Int(input.x, input.y);
		}
	}
}
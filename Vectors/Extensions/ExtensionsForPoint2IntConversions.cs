using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	public static class ExtensionsForPoint2IntConversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int ToVector2Int(this Point2Int input)
		{
			return new Vector2Int(input.x, input.y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int ToVector3Int(this Point2Int input)
		{
			return new Vector3Int(input.x, input.y);
		}
	}
}
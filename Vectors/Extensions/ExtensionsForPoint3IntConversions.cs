using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	public static class ExtensionsForPoint3IntConversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int ToVector3Int(this Point3Int input)
		{
			return new Vector3Int(input.x, input.y, input.z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int ToVector3IntByRef(ref this Point3Int input)
		{
			return new Vector3Int(input.x, input.y, input.z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3ByRef(ref this Point3Int input)
		{
			return new Vector3(input.x, input.y, input.z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int ToVector2Int(this Point3Int input)
		{
			return new Vector2Int(input.x, input.y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int ToPoint3Int(this Point3Int input)
		{
			return new Vector3Int(input.x, input.y, input.z);
		}
	}
}
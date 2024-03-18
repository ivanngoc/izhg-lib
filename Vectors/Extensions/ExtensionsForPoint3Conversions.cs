using IziHardGames.Libs.NonEngine.Vectors;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Vectors
{
	public static class ExtensionsForPoint3Conversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this Point3 input)
		{
			return new Vector3(input.x, input.y, input.z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3ByRef(ref this Point3 input)
		{
			return new Vector3(input.x, input.y, input.z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ToVector2(ref this Point3 input)
		{
			return new Vector2(input.x, input.y);
		}
	}

}
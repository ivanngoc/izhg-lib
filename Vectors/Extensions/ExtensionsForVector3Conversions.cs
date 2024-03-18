using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	public static class ExtensionsForVector3Conversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point3 ToPoint3(this Vector3 input)
		{
			return new Point3(input.x, input.y, input.z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 ToPoint2(ref this Vector3 input)
		{
			return new Point2(input.x, input.y);
		}
	}
}
using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	public static class ExtensionsForPoint2Conversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this Point2 input)
		{
			return new Vector3(input.x, input.y);
		}
	}
}
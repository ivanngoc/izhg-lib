using IziHardGames.Libs.NonEngine.Vectors;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Vectors
{
	public static class ExtensionsForVector2Conversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 ToPoint2(this Vector2 input)
		{
			return new Point2(input.x, input.y);
		}
	}
}

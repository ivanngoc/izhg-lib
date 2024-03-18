using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Libs.NonEngine.Math.Trigonometry
{
	public static class MathForTrigonometry 
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcHypotenuseWithAdjacent(float angle, float adjacentLength)
		{
			float cos = MathF.Cos(angle * MathF.PI / 180);
			return adjacentLength / cos;
		}
	}
}
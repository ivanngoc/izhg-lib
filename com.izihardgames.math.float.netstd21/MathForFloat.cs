using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.Math.Float
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/api/system.single.epsilon?view=netstandard-2.1&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Single.Epsilon)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.7.1)%3Bk(DevLang-csharp)%26rd%3Dtrue#platform-notes
	/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(volatile_CSharpKeyword)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.7.1)%3Bk(DevLang-csharp)%26rd%3Dtrue
	/// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs
	/// https://developer.arm.com/documentation/ddi0406/c/Application-Level-Architecture/Application-Level-Programmers--Model/Floating-point-data-types-and-arithmetic/Flush-to-zero
	/// </summary>
	public class MathfInternal
	{
		public const float FloatMinNormal = 1.17549435E-38f;
		public const float FloatMinDenormal = float.Epsilon;
		public static bool IsFlushToZeroEnabled = FloatMinDenormal == 0f;
	}
	public static class MathForFloat
	{
		public static readonly float Epsilon = MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal : MathfInternal.FloatMinDenormal;

		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}

			return value;
		}
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}

			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp01(float value)
		{
			if (value < 0) return 0;
			if (value > 1) return 1;
			return value;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CeilToInt(float f)
		{
			return (int)System.Math.Ceiling(f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FloorToInt(float f)
		{
			return (int)System.MathF.Floor(f);
		}
		/// <summary>
		/// Returns the sign of f.
		/// </summary>
		/// <param name="f"></param>
		/// <returns>
		/// Return value is 1 when f is positive or zero, -1 when f is negative.
		/// </returns>
		public static float Sign(float f)
		{
			return f >= 0f ? 1f : -1f;
		}
	}
}
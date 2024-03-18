using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Extensions.PremetiveTypes
{
	public static partial class ExtensionFloat
	{
		//public static StringBuilder stringBuilder32 = new StringBuilder(32);
		//public static StringBuilder stringBuilder64 = new StringBuilder(64);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundToOne(this float f)
		{
			if (f.IsEqualEpsiolon(0)) return 0;
			if (f > 0) return 1f;
			if (f < 1) return -1f;

			throw new ArgumentException($"[{f}] is not a number");
		}

		public static unsafe string FloatToStringBin(this float f)
		{
			byte* b = (byte*)&f;

			Span<char> span = stackalloc char[32];

			Span<char> span0 = span.Slice(0, 8);
			Span<char> span1 = span.Slice(8, 8);
			Span<char> span2 = span.Slice(16, 8);
			Span<char> span3 = span.Slice(24, 8);

			b[0].ByteToBinString().AsSpan().CopyTo(span3);
			b[1].ByteToBinString().AsSpan().CopyTo(span2);
			b[2].ByteToBinString().AsSpan().CopyTo(span1);
			b[3].ByteToBinString().AsSpan().CopyTo(span0);

			return span.ToString();
		}

		/// <summary>
		/// Non allocation set precision
		/// </summary>
		/// <param name="self"></param>
		/// <param name="digitAfterPoint"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SetPrecision(this float self, int digitAfterPoint)
		{
			return (float)Math.Round(self, digitAfterPoint);
		}

		/// <summary>
		/// epsilon = 0.1f
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualDeci(this float self, float compare)
		{
			return Math.Abs(self - compare) < 0.1f;
		}
		/// <summary>
		/// epsilon = 0.01f
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualSanti(this float self, float compare)
		{
			return Math.Abs(self - compare) < 0.01f;
		}
		/// <summary>
		///   epsilon = 0.001f
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualMili(this float self, float compare)
		{
			return Math.Abs(self - compare) < 0.001f;
		}
		/// <summary>
		/// epsilon = 0.000001f
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualMicro(this float self, float compare)
		{
			return Math.Abs(self - compare) < 0.000001f;
		}
		/// <summary>
		/// epsilon = 0.000001f
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualNano(this float self, float compare)
		{
			return Math.Abs(self - compare) < 0.000000001f;
		}
		//public static bool IsMoreThan(this float self, float than)
		//      {
		//	return (self - than) > 0;
		//}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqualEpsiolon(this float self, float compare)
		{
			return Math.Abs(self - compare) < float.Epsilon;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetDecimical(this float f)
		{
			return f - (int)f;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWholeNumber(this float f)
		{
			return (f - (int)f) < float.Epsilon;
		}
		/// <summary>
		/// 657ns Mathf 
		/// 699ns unsafe in scope
		/// 666ns custom method same with mathf 
		/// 1739ns this as method
		/// 1741ns this method as extension 
		/// ~ same without inline option
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float Abs(this float f)
		{
			/// -2147483648 = 0b_1000_0000_0000_0000_0000_0000_0000_0000;
			/// 2147483647 =  0b_0111_1111_1111_1111_1111_1111_1111_1111;
			int* pointer = (int*)&f;
			int val = (*pointer) & int.MaxValue;
			return *((float*)&val);
		}
	}// class

} // namespace
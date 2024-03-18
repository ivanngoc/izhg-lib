using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Extensions.PremetiveTypes
{
	public static class ExtensionInt
	{
		/// <summary>
		/// https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
		/// !для 0 - 0<br/>
		/// <br/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextPowOfTwoForPositive(this int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInRangeInclusive(this int self, int left, int right)
		{
			return left <= self && self <= right;
		}
		/// <summary>
		/// Проверка на четность
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEven(this int self)
		{
			return (self & 1) == 0;
		}
		/// <summary>
		/// Проверка на нечетность
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsOdd(this int self)
		{
			return (self & 1) == 1;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IncLoop(this int self, int max)
		{
			self++;

			return (max == self) ? default : self;
		}
		/// <summary>
		/// Двигается по числовой последовательности как на часах от 0 до заданного значения. Учитывает обороты но не отдает их результатом
		/// </summary>
		/// <param name="self"></param>
		/// <param name="shift"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ShiftLoopForward(this int self, int shift, int max)
		{
			//int leftAfterRound = shift % max;
			/// self=4;shift=5;max=6;             
			return (self + shift) % max;
		}
		/// <summary>
		/// Смещение как по часовой так и против часовой стрелки
		/// </summary>
		/// <param name="self"></param>
		/// <param name="shift"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		public static int CircleShift(this int self, int shift, int max)
		{
			if (shift > 0)
			{
				return self.ShiftLoopForward(shift, max);
			}
			else
			{
				int rem = shift % max;

				int result = self + rem;

				if (result < 0)
					return result + max;
				else
					return result;
			}
		}

		/// <summary>
		/// result = (n)*i+(n-1)*i...(n-m)*i, n>m
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecrementSumOfMul(this int self, int mul)
		{
			int count = self;

			int result = default;

			for (int i = 0; i < count; i++)
			{
				result += (self - i) * mul;
			}

			return result;
		}

		/// <summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/standard-numeric-format-strings"/> 
		/// <see cref="https://docs.microsoft.com/ru-ru/dotnet/api/system.int32.tostring?view=netstandard-2.0"/> 
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public unsafe static string IntToStringBin(this int self)
		{
			byte* b = (byte*)&self;

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

		public unsafe static string UIntToStringHex(this int self)
		{
			byte* b = (byte*)&self;

			Span<char> span = stackalloc char[10];

			Span<char> span1 = span.Slice(2, 2);
			Span<char> span2 = span.Slice(4, 2);
			Span<char> span3 = span.Slice(6, 2);
			Span<char> span4 = span.Slice(8, 2);

			span[0] = '0';
			span[1] = 'x';

			b[0].ByteToHexString().AsSpan().CopyTo(span4);
			b[1].ByteToHexString().AsSpan().CopyTo(span3);
			b[2].ByteToHexString().AsSpan().CopyTo(span2);
			b[3].ByteToHexString().AsSpan().CopyTo(span1);

			return span.ToString();
		}
		public unsafe static string IntToStringHex(this int self)
		{
			if (self < 0)
			{
				if (self == int.MinValue)
				{
					return "-0x80000000";
				}
				else
				{
					self = -self;
				}

				byte* b = (byte*)&self;

				Span<char> span = stackalloc char[11];

				Span<char> span1 = span.Slice(3, 2);
				Span<char> span2 = span.Slice(5, 2);
				Span<char> span3 = span.Slice(7, 2);
				Span<char> span4 = span.Slice(9, 2);

				span[0] = '-';
				span[1] = '0';
				span[2] = 'x';

				b[0].ByteToHexString().AsSpan().CopyTo(span4);
				b[1].ByteToHexString().AsSpan().CopyTo(span3);
				b[2].ByteToHexString().AsSpan().CopyTo(span2);
				b[3].ByteToHexString().AsSpan().CopyTo(span1);

				return span.ToString();
			}
			else
			{
				byte* b = (byte*)&self;

				Span<char> span = stackalloc char[10];

				Span<char> span1 = span.Slice(2, 2);
				Span<char> span2 = span.Slice(4, 2);
				Span<char> span3 = span.Slice(6, 2);
				Span<char> span4 = span.Slice(8, 2);

				span[0] = '0';
				span[1] = 'x';

				b[0].ByteToHexString().AsSpan().CopyTo(span4);
				b[1].ByteToHexString().AsSpan().CopyTo(span3);
				b[2].ByteToHexString().AsSpan().CopyTo(span2);
				b[3].ByteToHexString().AsSpan().CopyTo(span1);

				return span.ToString();
			}
		}
	}

} // namespace
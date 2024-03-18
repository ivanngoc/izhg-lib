using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Extensions.PremetiveTypes
{
	public static class ExtensionIntFlags
	{
		public static int BitSet(this int flag, int position)
		{
			return flag & (1 << position);
		}
		public static int BitReset(this int flag, int position)
		{
			return flag & (~(1 << position));
		}
		/// <summary>
		/// Подсчитать количество битов =1 
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CountBitSet(this int flag)
		{
			int result = default;

			for (int i = 0; i < 32; i++)
			{
				int iterFlag = flag >> i;

				if (iterFlag.IsOdd())
				{
					result++;
				}
			}
			return result;
		}
		public static bool HasFlag(this int value, int flag)
		{
#if UNITY_EDITOR
			if (flag.CountBitSet() > 1) throw new FormatException($" More than one flag set.{flag.IntToStringBin()}");
#endif
			return (value & flag) != 0;
		}
		/// <summary>
		/// 0 - 31. Бит в указаной похиции = 1 
		/// </summary>
		/// <param name="self"></param>
		/// <param name="digit32"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBitIsOne(this int self, int digit32)
		{
			return (self & (1 << digit32)) != 0;
		}
		/// <summary>
		/// Бит в указаной позиции = 0 (0-31)
		/// </summary>
		/// <param name="self"></param>
		/// <param name="digit32"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBitIsZero(this int self, int digit32)
		{
			return !self.IsBitIsOne(digit32);
		}

		/// <summary>
		/// Значение от 0 до 31 переводится во флаг, где значение это количество сплошных единиц  справа налево. Оставшаяся часть - нули
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IntToFlag32(this int value)
		{
#if UNITY_EDITOR
			if (!value.IsInRangeInclusive(0, 31)) throw new ArgumentOutOfRangeException();
#endif
			return ~(-1 << value);
		}
	}

} // namespace
using System;
using System.Runtime.CompilerServices;
using IziHardGames.Extensions.PremetiveTypes;

namespace IziHardGames.Libs.NonEngine.Helpers
{
	public static class HelperFlagsInt
	{
		/// <summary>
		/// Затолкнуть флаги в <see cref="int"/> крайний левый аргумент будет крайним правым битом, крайний правый аргумент будет крайним левым битом
		/// </summary>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Combine(bool value0, bool value1)
		{
			return (value0 ? 1 : 0) | (value1 ? 2 : 0);
		}
		/// <summary>
		/// <inheritdoc cref="Combine(bool, bool)"/>
		/// </summary>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Combine(bool value0, bool value1, bool value2)
		{
			return Combine(value0, value1) | (value2 ? 4 : 0);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Combine(bool value0, bool value1, bool value2, bool value3)
		{
			return Combine(value0, value1, value2) | (value3 ? 16 : 0);
		}
		/// <summary>
		/// <inheritdoc cref="Combine(bool, bool)"/>
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Combine(params bool[] args) => throw new NotImplementedException();

		/// <summary>
		/// Установить 1 из 32 флагов инта в <see langword="true"/>
		/// </summary>
		/// <param name="bit"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int BitFlagWithBitNumberSet(int digit32, int value)
		{
#if UNITY_EDITOR
			if (!digit32.IsInRangeInclusive(0, 32))
			{
				throw new ArgumentOutOfRangeException();
			}
#endif
			return value | (1 << digit32);
		}
		/// <summary>
		/// Установить 1 из 32 флагов инта в <see langword="false"/>
		/// </summary>
		/// <param name="bit"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int BitFlagWithBitNumberReset(int digit32, int value)
		{
#if UNITY_EDITOR
			if (!digit32.IsInRangeInclusive(0, 32))
			{
				throw new ArgumentOutOfRangeException();
			}
#endif
			return value & (~(1 << digit32));
		}

		public static int BitFlagWithMaskSet(int mask, int value)
		{
#if UNITY_EDITOR
			if (!mask.IsInRangeInclusive(0, 32))
			{
				throw new ArgumentOutOfRangeException();
			}
#endif
			return value & mask;
		}
		public static int BitFlagWithMaskReset(int mask, int value)
		{
#if UNITY_EDITOR
			if (!mask.IsInRangeInclusive(0, 32))
			{
				throw new ArgumentOutOfRangeException();
			}
#endif
			return value & (~mask);
		}

		/// <summary>
		/// is value contains any bit flag<br/>
		/// Flag with value=0 must be free, not asigned to logic, use for not init error detection
		/// </summary>
		/// <param name="value"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool FlagAnyIntersect(int value, int flag)
		{
			return (value & flag) != 0;
		}
		/// <summary>
		/// Левый аргумент имеет все битовые флаги = 1, какие имеет правый аргумент
		/// </summary>
		/// <param name="value"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool FlagLeftIncludeAllSecond(int left, int right)
		{
			return (left & right) == right;
		}
	}
}
namespace IziHardGames.Libs
{
	/// <summary>
	/// <see cref="ExtensionIntFlags"/>
	/// </summary>
	public static class HelperEnumFlag
	{
#if UNITY_EDITOR
		private enum Flags32Enum
		{
			value0 = 1,

		}
#endif

		public static bool FlagAny<TEnum>(TEnum value, TEnum flag1, TEnum flag2, TEnum flag3) where TEnum : Enum
		{
			return value.HasFlag((Enum)flag1) || value.HasFlag((Enum)flag2) || value.HasFlag((Enum)flag3);
		}
		public static bool FlagAny<TEnum>(TEnum value, TEnum flag1, TEnum flag2) where TEnum : Enum
		{
			return value.HasFlag((Enum)flag1) || value.HasFlag((Enum)flag2);
		}
	}


	public static class HelperInt
	{
#if UNITY_EDITOR
		/// <summary>
		///  медленней чем через временную переменную
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SwapIntSlow(ref int x, ref int y)
		{
			x = x ^ y;
			y = y ^ x;
			x = x ^ y;
		}
#endif
		public static void SwapInt(ref int x, ref int y)
		{
			int temp = x;
			x = y;
			y = temp;
		}
	}
}
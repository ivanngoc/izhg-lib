using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Extensions.PremetiveTypes
{
	public static class ExtensionUIntAsFlag
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsBitIsZero(this uint self, int digit32)
		{
			return (~self & (1 << digit32)) > 0;
		}
		/// <summary>
		/// https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
		/// !для 0 - 0<br/>
		/// <br/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint NextPowOf2(this uint v)
		{
#if UNITY_EDITOR
			if (v == 0) throw new NotSupportedException();
#endif
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}
	}

} // namespace
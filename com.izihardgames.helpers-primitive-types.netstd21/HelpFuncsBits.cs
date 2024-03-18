using IziHardGames.Extensions.PremetiveTypes;

namespace IziHardGames.Libs.NonEngine.Helpers
{
	/// <summary>
	/// Binary representations
	/// </summary>
	public static class HelpFuncsBits
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static unsafe int BitsFloatToInt(float value)
		{
			///<see cref="BitConverter"/> method SingleToInt32Bits is available only in NetStandart2.1. Current version is 2.0
			int* pointerAsInt = (int*)&value;
			int val = *pointerAsInt;
			return val;
		}

		public static string GetBitString(float value)
		{
			return value.FloatToStringBin();
		}
		public static string GetBitString(int value)
		{
			return value.IntToStringBin();
		}
	}
}
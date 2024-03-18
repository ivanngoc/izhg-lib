namespace System
{
	public static class HelperMath
	{
		#region Unity Message		

		#endregion

		public static T ClampMax<T>(T val, T max) where T : IComparable<T>
		{
			if (val.CompareTo(max) > 0)
			{
				return max;
			}
			else
			{
				return val;
			}
		}
		public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			else
			{
				if (val.CompareTo(max) > 0)
				{
					return max;
				}
				else
				{
					return val;
				}
			}
		}

		/// <summary>
		/// power of 0 = 1
		/// power of 1 = x
		/// power of 2 = x * x
		/// </summary>
		/// <param name="basis"></param>
		/// <param name="power"></param>
		/// <returns></returns>
		public static int Pow(int basis, int power)
		{
			if (power == 0) return 1;

			int mul = basis;

			for (int i = 1; i < power; i++)
			{
				basis *= mul;
			}
			return basis;
		}

		public static float Clamp01(float v)
		{
			if (v < 0) return 0;
			if (v > 1) return 1;

			return v;
		}
		/// <summary>
		/// https://www.youtube.com/watch?v=p8u_k2LIZyo&list=LL&index=4
		/// </summary>
		/// <returns></returns>
		public unsafe static float FastInverseSquareRoot(float number)
		{
			int i;
			float x2, y;
			const float threehalfs = 1.5f;

			x2 = number * 0.5f;
			y = number;
			i = *(int*)&y;
			i = 0x5f3759df - (i >> 1);
			y = *(float*)&i;
			y = y * (threehalfs - (x2 * y * y));
			//y = y * (threehalfs - (x2 * y * y));

			return y;
		}
	}
}
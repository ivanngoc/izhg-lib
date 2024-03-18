using Vector3 = IziHardGames.Libs.NonEngine.Vectors.Point3;

namespace IziHardGames.Libs.NonEngine.Helpers
{
	public static class HelpFuncsVector3NonEngine
	{
		/// <summary>
		/// <see cref="https://docs.unity3d.com/ScriptReference/Vector3-operator_ne.html"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool PreciseEqualsNot(Vector3 a, Vector3 b)
		{
			if ((a.x - b.x) > 0 || (a.x - b.x) < 0) return true;
			if ((a.y - b.y) > 0 || (a.y - b.y) < 0) return true;
			if ((a.z - b.z) > 0 || (a.z - b.z) < 0) return true;

			return false;
		}
		/// <summary>
		/// <see cref="https://docs.unity3d.com/ScriptReference/Vector3-operator_eq.html"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool PreciseEquals(Vector3 a, Vector3 b)
		{
			if ((a.x - b.x) > 0 && (a.x - b.x) < 0) return false;
			if ((a.y - b.y) > 0 && (a.y - b.y) < 0) return false;
			if ((a.z - b.z) > 0 && (a.z - b.z) < 0) return false;

			return true;
		}
		public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
		{
			float x = Clamp(value.x, min.x, max.x);
			float y = Clamp(value.y, min.y, max.y);
			float z = Clamp(value.z, min.z, max.z);

			return new Vector3(x, y, z);
		}
#pragma warning disable HAA0601
		public static string ToString(Vector3 vector3, int decimical = 4)
		{
			return $"{vector3.x.ToString($"F{decimical}")},{vector3.y.ToString($"F{decimical}")}, {vector3.z.ToString($"F{decimical}")}";
		}

		private static float Clamp(float value, float min, float max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}
	}
}
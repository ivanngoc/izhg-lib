using UnityEngine;


namespace IziHardGames.Libs.Engine.Math.Interpolations
{
	public class Interpolations
	{
		public static float Coserp(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
		}
		public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
		{
			return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
		}
		public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
		{
			return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
		}
	}
}
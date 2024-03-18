using IziHardGames.Libs.Engine.Helpers;
using UnityEngine;


namespace IziHardGames.Libs.Engine.Tools
{
	public static class RandomizerVector3
	{
		public static Vector3 GetRandomDirection2d()
		{
			return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
		}
		/// <summary>
		/// Get Random position form center with ensured certain length that wont exceed given bounds. 
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static Vector2 GetRandomPosition2d(ref Bounds bounds, Vector2 center, float radius)
		{
			Vector2 dir = GetRandomDirection2d().normalized;

			for (int i = 0; i < 36; i++)
			{
				Vector2 targetPos = center + (dir * radius);
				if (HelpFuncsBoundsEngine.IsPointInsideBound(ref bounds, targetPos))
				{
					return targetPos;
				}
				// counter clockwise rotation
				dir = Quaternion.AngleAxis(i * 10f, Vector3.forward) * dir;
			}
			return dir * radius;
		}
	}
}
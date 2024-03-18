using UnityEngine;
using Random = UnityEngine.Random;

namespace IziHardGames.Libs.RandomAlgs
{
	public static class RandomizerEngine
	{
		#region Unity Message

		#endregion

		public static Vector2 GetRandomScreenPos()
		{
			int x = Random.Range(0, Screen.width);
			int y = Random.Range(0, Screen.height);

			return new Vector2(x, y);
		}

		/// <summary>
		/// Ќаходит произвльную точку в области в произвольном направлении с ограничением дистанции и границ пр¤моуголной области
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		public static Vector3 GetRandomInBoundPosAtDir2D(Bounds bounds, Vector3 pos, float maxDistance)
		{
			Vector3 dir = Random.insideUnitCircle.normalized;

			pos = pos + dir * maxDistance;

			float x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
			float y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);

			return new Vector3(x, y, default);
		}
		/// <summary>
		/// ѕроизвольна¤ точка в области
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		public static Vector3 GetRandomInBoundPos(Bounds bounds)
		{
			float x = Random.Range(bounds.min.x, bounds.max.x);
			float y = Random.Range(bounds.min.y, bounds.max.y);
			float z = Random.Range(bounds.min.y, bounds.max.z);

			return new Vector3(x, y, z);
		}
		public static Vector3 GetRandomPosVisibleFixZ(Camera camera, float fixZ)
		{
			var screenPos = GetRandomScreenPos();

			return camera.ScreenToWorldPoint(screenPos).GetCopyWithZ(fixZ);
		}

		public static Vector3 GetRandomPosVisibleRangeZ(Camera camera, float maxZ)
		{
			var screenPos = GetRandomScreenPos();

			float z = Random.Range(0f, maxZ);

			return camera.ScreenToWorldPoint(screenPos).GetCopyWithZ(z);
		}
	}
}
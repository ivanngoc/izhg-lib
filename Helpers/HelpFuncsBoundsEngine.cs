using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{
	/// <summary>
	/// 
	/// </summary>
	/// <see cref="IziHardGames.Libs.Engine.SpaceMap.Helpers.HelpFuncsBoundsEngineForSurrogates"/>
	public static class HelpFuncsBoundsEngine
	{
		public static Bounds CalculateBounds(Transform[] transforms)
		{
			var minX = transforms[0].position.x;
			var minY = transforms[0].position.y;
			var minZ = transforms[0].position.z;

			var maxX = transforms[0].position.x;
			var maxY = transforms[0].position.y;
			var maxZ = transforms[0].position.z;

			for (var i = 0; i < transforms.Length; i++)
			{
				var transform = transforms[i];

				if (minX > transform.position.x) minX = transform.position.x;
				if (minY > transform.position.y) minY = transform.position.y;
				if (minZ > transform.position.z) minZ = transform.position.z;

				if (maxX < transform.position.x) maxX = transform.position.x;
				if (maxY < transform.position.y) maxY = transform.position.y;
				if (maxZ < transform.position.z) maxZ = transform.position.z;
			}

			var lenghtX = maxX - minX;
			var lenghtY = maxY - minY;
			var lenghtZ = maxZ - minZ;

			var midX = minX + lenghtX / 2f;
			var midY = minY + lenghtY / 2f;
			var midZ = minZ + lenghtZ / 2f;

			return new Bounds(new Vector3(midX, midY, midZ), new Vector3(lenghtX, lenghtY, lenghtZ));
		}

		public static Bounds CalculateBounds(Renderer[] renderers)
		{
			Vector3 min = renderers[0].bounds.min;
			Vector3 max = renderers[0].bounds.max;

			for (int i = 0; i < renderers.Length; i++)
			{
				RenewBound(renderers[i].bounds.min, ref min, ref max);
				RenewBound(renderers[i].bounds.max, ref min, ref max);
			}
			return new Bounds(Vector3.Lerp(min, max, 0.5f), max - min);
		}

		public static Bounds CalculateBounds(Bounds[] bounds)
		{
			Vector3 min = bounds[0].min;
			Vector3 max = bounds[0].max;

			for (int i = 0; i < bounds.Length; i++)
			{
				RenewBound(bounds[i].min, ref min, ref max);
				RenewBound(bounds[i].max, ref min, ref max);
			}
			return new Bounds(Vector3.Lerp(min, max, 0.5f), max - min);
		}

		public static Bounds CalculateBounds(Vector3[] vector3s)
		{
			var minX = vector3s[0].x;
			var minY = vector3s[0].y;
			var minZ = vector3s[0].z;

			var maxX = vector3s[0].x;
			var maxY = vector3s[0].y;
			var maxZ = vector3s[0].z;

			for (var i = 0; i < vector3s.Length; i++)
			{
				if (minX > vector3s[i].x) minX = vector3s[i].x;
				if (minY > vector3s[i].y) minY = vector3s[i].y;
				if (minZ > vector3s[i].z) minZ = vector3s[i].z;

				if (maxX < vector3s[i].x) maxX = vector3s[i].x;
				if (maxY < vector3s[i].y) maxY = vector3s[i].y;
				if (maxZ < vector3s[i].z) maxZ = vector3s[i].z;
			}

			var lenghtX = maxX - minX;
			var lenghtY = maxY - minY;
			var lenghtZ = maxZ - minZ;

			var midX = minX + lenghtX / 2f;
			var midY = minY + lenghtY / 2f;
			var midZ = minZ + lenghtZ / 2f;

			return new Bounds(new Vector3(midX, midY, midZ), new Vector3(lenghtX, lenghtY, lenghtZ));
		}

		public static bool RenewBound(Vector3 vector3, ref Vector3 currentMin, ref Vector3 currentMax)
		{
			bool isModified = default;

			if (currentMin.x > vector3.x)
			{
				currentMin.x = vector3.x;

				isModified = true;
			}
			if (currentMin.y > vector3.y)
			{
				currentMin.y = vector3.y;

				isModified = true;
			}
			if (currentMin.z > vector3.z)
			{
				currentMin.z = vector3.z;

				isModified = true;
			}

			if (currentMax.x < vector3.x)
			{
				currentMax.x = vector3.x;

				isModified = true;
			}
			if (currentMax.y < vector3.y)
			{
				currentMax.y = vector3.y;

				isModified = true;
			}
			if (currentMax.z < vector3.z)
			{
				currentMax.z = vector3.z;

				isModified = true;
			}
			return isModified;
		}
		/// <summary>
		/// Первый четырехугольник (первые два аргумента) либо совпадает по всем границам либо больше на любой стороне и/или равен любой стороне.
		/// Иначе говоря второй четырехугольник либо совпадает по границами или находится внутри первого. Или второй четырехугольник гарантировано меньше первого и не выходит за его границы<br/>
		/// <see langword="true"/> - да <br/>
		/// Полезен при поиске из центра второго четырехугольника с постепенным расширением границы первого
		/// </summary>
		/// <param name="minFirst"></param>
		/// <param name="maxFirst"></param>
		/// <param name="minSecond"></param>
		/// <param name="maxSecond"></param>
		/// <returns></returns>
		public static bool IsRectFirstEqualOrOverSecond2d(Vector2Int minFirst, Vector2Int maxFirst, Vector2Int minSecond, Vector2Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
		public static bool IsRectFirstEqualOrOverSecond3d(Vector3Int minFirst, Vector3Int maxFirst, Vector3Int minSecond, Vector3Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
		public static bool IsPointInsideRect2d(Vector3Int point, Vector2Int rectMin, Vector2Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y);
		}
		public static bool IsPointInsideRect3d(Vector3Int point, Vector3Int rectMin, Vector3Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y) && (rectMin.z <= point.z) && (point.z <= rectMax.z);
		}
		public static bool IsPointInsideBound(ref Bounds bounds, Vector3 pos)
		{
			if (pos.x < bounds.min.x) return false;
			if (pos.y < bounds.min.y) return false;
			if (pos.z < bounds.min.z) return false;
			if (pos.x > bounds.max.x) return false;
			if (pos.y > bounds.max.y) return false;
			if (pos.z > bounds.max.z) return false;
			return true;
		}
		public static bool IsPointInsideBound(ref Bounds bounds, Vector2 pos)
		{
			if (pos.x < bounds.min.x) return false;
			if (pos.y < bounds.min.y) return false;
			if (pos.x > bounds.max.x) return false;
			if (pos.y > bounds.max.y) return false;
			return true;
		}
	}
}
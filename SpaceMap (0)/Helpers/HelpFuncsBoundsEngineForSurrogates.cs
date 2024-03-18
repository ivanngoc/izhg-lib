using IziHardGames.Libs.NonEngine.Vectors;
using UnityEngine;

namespace IziHardGames.Libs.Engine.SpaceMap.Helpers
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <see cref="IziHardGames.Libs.Engine.Helpers.HelpFuncsBoundsEngine"/>
	/// </remarks>
	public static class HelpFuncsBoundsEngineForSurrogates
	{
		/// <summary>
		/// Является ли левый прямоугольник частично или полностью вписанным в правый. (Есть ли пересечения у крайнего кольца из координат левого с любой областью правого).
		/// Используется при расширяющемся поиске от центра для того чтобы определить вышел ли за границы поиск.
		/// </summary>
		/// <param name="minLeftRect"></param>
		/// <param name="maxLeftRect"></param>
		/// <param name="minRightRect"></param>
		/// <param name="maxRightRect"></param>
		/// <returns></returns>
		public static bool IsEdgeOfLeftRectIntersectWithRightRect(Vector2Int minLeftRect, Vector2Int maxLeftRect, Point2Int minRightRect, Point2Int maxRightRect)
		{
			return (minLeftRect.x >= minRightRect.x) || (minLeftRect.y >= minRightRect.y) || (maxLeftRect.x <= maxRightRect.x) || (maxLeftRect.y <= maxRightRect.y);
		}
		/// <summary>
		/// Является ли левый прямоугольник полностью вписанным в правый
		/// </summary>
		/// <param name="minLeftRect"></param>
		/// <param name="maxLeftRect"></param>
		/// <param name="minRightRect"></param>
		/// <param name="maxRightRect"></param>
		/// <returns></returns>
		public static bool IsRectFirstEqualOrOverSecond2d(Vector2Int minLeftRect, Vector2Int maxLeftRect, Point2Int minRightRect, Point2Int maxRightRect)
		{
			return (minLeftRect.x <= minRightRect.x) && (minLeftRect.y <= minRightRect.y) && (maxLeftRect.x >= maxRightRect.x) && (maxLeftRect.y >= maxRightRect.y);
		}
		public static bool IsRectFirstEqualOrOverSecond3d(Vector3Int minFirst, Vector3Int maxFirst, Point3Int minSecond, Point3Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
		public static bool IsPointInsideRect2d(Vector3Int point, Point2Int rectMin, Point2Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y);
		}
		public static bool IsPointInsideRect3d(Vector3Int point, Point3Int rectMin, Point3Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y) && (rectMin.z <= point.z) && (point.z <= rectMax.z);
		}
	}
}

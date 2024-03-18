using IziHardGames.Libs.NonEngine.Vectors;

namespace IziHardGames.Libs.NonEngine.Helpers
{
	public static class HelpFuncsBoundsNonEngine
	{
		public static bool RenewBound(Point3 vector3, ref Point3 currentMin, ref Point3 currentMax)
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
		public static bool IsRectFirstEqualOrOverSecond(Point2Int minFirst, Point2Int maxFirst, Point2Int minSecond, Point2Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
		public static bool IsPointInsideRect(Point3Int point, Point2Int rectMin, Point2Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y);
		}

		public static bool IsRectFirstEqualOrOverSecond2d(Point2Int minFirst, Point2Int maxFirst, Point2Int minSecond, Point2Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
		public static bool IsRectFirstEqualOrOverSecond3d(Point3Int minFirst, Point3Int maxFirst, Point3Int minSecond, Point3Int maxSecond)
		{
			return (minFirst.x <= minSecond.x) && (minFirst.y <= minSecond.y) && (maxFirst.x >= maxSecond.x) && (maxFirst.y >= maxSecond.y);
		}
	}
}
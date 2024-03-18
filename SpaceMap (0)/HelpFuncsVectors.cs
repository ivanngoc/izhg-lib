
using IziHardGames.Libs.NonEngine.Vectors;

namespace IziHardGames.Libs.SerilizationTypes.Vectors
{
	public static class HelpFuncsVectors
	{
		public static bool IsPointInsideRect(Point3Int point, Point2Int rectMin, Point2Int rectMax)
		{
			return (rectMin.x <= point.x) && (point.x <= rectMax.x) && (rectMin.y <= point.y) && (point.y <= rectMax.y);
		}
	}
}
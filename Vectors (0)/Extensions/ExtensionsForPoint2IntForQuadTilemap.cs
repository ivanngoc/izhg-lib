using System;


namespace IziHardGames.Libs.NonEngine.Vectors.Extensions.ForTilemap
{
	public static class ExtensionsForPoint2IntForQuadTilemap 
	{
		public static Point2Int GetAtRadiusQuadsLayerPosition(this Point2Int vector, int layerRadius, int index)
		{
			if (layerRadius == 0) return vector;

			int oneSideCount = layerRadius * 2 + 1;
			int circleCount = layerRadius * 8;
			int half = circleCount >> 1;
#if UNITY_EDITOR
			if (!(index < circleCount)) { throw new ArgumentOutOfRangeException(); }
#endif
			if (index < half)
			{
				Point2Int min = vector.GetAtRadiusMinQuad(layerRadius);

				if (index < oneSideCount)
				{
					return new Point2Int(min.x, min.y + index);
				}
				else
				{
					Point2Int max = vector.GetAtRadiusMaxQuad(layerRadius);

					return new Point2Int(min.x + index - (oneSideCount - 1), max.y);
				}
			}
			else
			{
				if (index > half + (oneSideCount - 1))
				{
					Point2Int min = vector.GetAtRadiusMinQuad(layerRadius);

					return new Point2Int(min.x + (circleCount - index), min.y);
				}
				else
				{
					Point2Int max = vector.GetAtRadiusMaxQuad(layerRadius);

					return new Point2Int(max.x, max.y - (index - half));
				}
			}
		}

		public static Point2Int GetAtRadiusMinQuad(this Point2Int vector, int radius)
		{
			return new Point2Int(vector.x - radius, vector.y - radius);
		}

		public static Point2Int GetAtRadiusMaxQuad(this Point2Int vector, int radius)
		{
			return new Point2Int(vector.x + radius, vector.y + radius);
		}
	}
}
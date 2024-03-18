using IziHardGames.Libs.NonEngine.Vectors;
using System;
using Vector2Int = IziHardGames.Libs.NonEngine.Vectors.Point2Int;

namespace UnityEngine
{
	public static class ExtensionVector3IntSurQuadNonEngine
	{
		/// <summary>
		/// Min is origin(start). Next element go clockwise. First Increase Y. then...Increase X...then decrease Y...then decrease X
		/// Слой (layer/circle) это контур образованный на расстоянии R (условно радиус) от центра. 
		/// Например на радиусе 1 у ячейки будет слой или кольцо из 8 прилегающих к нему ячеек
		/// На радиусе 2 - 16. Это все ячейки контактирующие с предыдущим кольцом радиуса 1.
		/// На радиусе 3 - 24 и т.д.
		/// Визуально можно представить как волну расходящуюся из центра указанной координаты
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="layerRadius"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <remarks>
		/// Такой же принцип как и в <see cref="GraphTileJobByExpansion.GetMapIndex"/>
		/// </remarks>
		public static Point3Int GetAtRadiusQuadsLayerPosition(this Point3Int vector, int layerRadius, int index)
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
				Point3Int min = vector.GetAtRadiusMinQuad(layerRadius);

				if (index < oneSideCount)
				{
					return new Point3Int(min.x, min.y + index, vector.z);
				}
				else
				{
					Point3Int max = vector.GetAtRadiusMaxQuad(layerRadius);

					return new Point3Int(min.x + index - (oneSideCount - 1), max.y, vector.z);
				}
			}
			else
			{
				if (index > half + (oneSideCount - 1))
				{
					Point3Int min = vector.GetAtRadiusMinQuad(layerRadius);

					return new Point3Int(min.x + (circleCount - index), min.y, vector.z);
				}
				else
				{
					Point3Int max = vector.GetAtRadiusMaxQuad(layerRadius);

					return new Point3Int(max.x, max.y - (index - half), vector.z);
				}
			}
		}
		/// <summary>
		/// Соответствует максимальной точке прямоугольного баунда. Является серединой кругового отсчета. 
		/// Если по часовой то сначала растет ось Y затем X а потом спад в том же порядке
		/// Если против часовой стрелки сначала растет X затем Y  а потом спад в том же порядке
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static Point3Int GetAtRadiusMaxQuad(this Point3Int vector3Int, int radius)
		{
			return new Point3Int(vector3Int.x + radius, vector3Int.y + radius, vector3Int.z);
		}
		/// <summary>
		/// Если представить ось координат то минимальная точка - это начало кругового отсчета и будет соотвествовать минимальной координате баунда
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static Point3Int GetAtRadiusMinQuad(this Point3Int vector3Int, int radius)
		{
			return new Point3Int(vector3Int.x - radius, vector3Int.y - radius, vector3Int.z);
		}
		/// <summary>
		/// Расширение крестом от центра (без переходов по диагонали)
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <param name="radius"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static Point3Int GetRectangularExpansion(this Point3Int vector3Int, int radius, int index)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// 0 - left <br/>
		/// 1 - bot <br/>
		/// 2 - top <br/>
		/// 3 - right <br/>
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <returns></returns>
		public static Point3Int QuadGetRectangularNeighbour(this Point3Int vector3Int, int index)
		{
			switch (index)
			{
				case 0: return new Point3Int(vector3Int.x - 1, vector3Int.y, vector3Int.z);
				case 1: return new Point3Int(vector3Int.x, vector3Int.y - 1, vector3Int.z);
				case 2: return new Point3Int(vector3Int.x, vector3Int.y + 1, vector3Int.z);
				case 3: return new Point3Int(vector3Int.x + 1, vector3Int.y, vector3Int.z);

				default: throw new NotSupportedException();
			}
		}
		public static Point3Int QuadGetTop(this Point3Int vector3Int) => new Point3Int(vector3Int.x, vector3Int.y + 1, vector3Int.z);
		public static Point3Int QuadGetBot(this Point3Int vector3Int) => new Point3Int(vector3Int.x, vector3Int.y - 1, vector3Int.z);
		public static Point3Int QuadGetRight(this Point3Int vector3Int) => new Point3Int(vector3Int.x + 1, vector3Int.y, vector3Int.z);
		public static Point3Int QuadGetLeft(this Point3Int vector3Int) => new Point3Int(vector3Int.x - 1, vector3Int.y, vector3Int.z);
		/// <summary>
		/// 0 = top<br/>
		/// 1 = right<br/>
		/// 2 = bot<br/>
		/// 3 = left<br/>
		/// </summary>
		/// <param name="vector3Int"></param>
		/// <returns></returns>
		public static Point3Int QuadGetSide4(this Point3Int vector3Int, int index)
		{
			switch (index)
			{
				case 0: return vector3Int.QuadGetTop();
				case 1: return vector3Int.QuadGetRight();
				case 2: return vector3Int.QuadGetBot();
				case 3: return vector3Int.QuadGetLeft();
				default: throw new ArgumentOutOfRangeException($"Range [0,3]");
			}
		}
		public static Point3Int QuadGetSide8(this Point3Int vector3Int) => new Point3Int(vector3Int.x - 1, vector3Int.y, vector3Int.z);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns>
		/// item0 - radius/minimal transitions count<br/>
		/// item1 - index on perimeter (aka radius) <br/>
		/// </returns>
		public static (int, int) GetPerimetralInfo(this Point3Int from, Point3Int to)
		{
			Point3Int delta = (to - from);

			int x = Math.Abs(delta.x);
			int y = Math.Abs(delta.y);

			bool isX = x > y;

			int radius = isX ? x : y;

			if (radius == 0) return (0, 0);

			Point3Int min = new Point3Int(from.x - radius, from.y - radius, default);
			Point3Int minToTarget = to - min;

			if (minToTarget.x > minToTarget.y)
			{
				int countCircle = 8 * radius;
				int half = countCircle >> 1;
				return (radius, countCircle - minToTarget.x - minToTarget.y);
			}
			else
			{
				return (radius, minToTarget.x + minToTarget.y);
			}

			#region Expiremental
			//Point3Int max = new Point3Int(from.x + radius, from.y + radius, default);
			//Point3Int maxToIndex = to - max;

			//int quad = half >> 1;
			//int index = countCircle - (half - minToTarget.x);
			//int index0 = (countCircle - minToTarget.x) - minToTarget.y - (quad - x) - (quad - y);

			//return (radius, minToTarget.x + minToTarget.y);

			//Half + delta(x, y)
			/*
			int sizeQuad = radius * 2;
			int sizeSide = sizeQuad + 1;
			int index = countCircle +;

			16 = 4*4
			16 = 2*8

			15 = (16-1)-0-(4-1)-(4-0);


			0 =		0;0		0		0
			1 =		0;1		1		-1
			2 =		0;2		2		-2
			3 =		0;3		3		-3
			4 =		0;4		4		-4
			5 =		1;4		5		-3
			6 =		2;4		6		-2
			7 =		3;4		7		-1

			8 =		4;4		8		0
			9 =		4;3		7		1
			10 =	4;2		6		2
			11 =	4;1		5		3
			12 =	4;0		4		4
			13 =	3;0		3		3
			14 =	2;0		2		2
			15 =	1;0		1		0

			 */
			#endregion
		}
		public static int GetPerimetralIndex(this Point3Int from, Point3Int to, int radius)
		{
			throw new NotImplementedException();
		}
	}
}
using IziHardGames.Libs.NonEngine.Vectors;
using IziHardGames.Libs.NonEngine.Vectors.Extensions.ForTilemap;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vector2Int = IziHardGames.Libs.NonEngine.Vectors.Point2Int;

namespace IziHardGames.Libs.NonEngine.SpaceMap.Searching.ForQuadTilemap
{
	[Serializable]
	public class JobSearchRadialWithOffsetPreset
	{
		private readonly static Dictionary<int, Vector2Int[]> offsetsPerRadius = new Dictionary<int, Vector2Int[]>(8);

		private DataForJobFindCellAround data;

		private Vector2Int min;
		private Vector2Int max;
		private int radiusMax;

		private Vector2Int center;
		private Func<Vector2Int, bool> funcValidation;

		public bool isFinded;
		public Vector2Int result;

		public void SetPersitant(Vector2Int min, Vector2Int max, int radiusMax, Func<Vector2Int, bool> funcValidation)
		{
			this.min = min;
			this.max = max;
			this.funcValidation = funcValidation;
			this.radiusMax = radiusMax;
			data.indexAtRadius = default;
		}

		public void Execute(int startRadius, Point2Int center)
		{
			this.center = center;
			data.radius = startRadius;
			data.SetCenter(center);
			Execute();
		}
		public void Execute()
		{
			data.isSearchProcceed = true;
			while (data.isSearchProcceed)
			{
				if (TryFindPositionRadial())
				{
					isFinded = true;
					return;
				}
				else
				{
					if (radiusMax > data.radius)
					{
						data.ExpandAreaByOne();
						bool isInRect = (data.minX >= min.x) || (data.minY >= min.y) || (data.maxX <= max.x) || (data.maxY <= max.y);
						data.isSearchProcceed = isInRect;

						if (!isInRect)
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
			isFinded = false;
		}

		public void Reset()
		{

		}

		private bool TryFindPositionRadial()
		{
			EnsureCacheOffsetPositions(data.radius);
			var offsets = offsetsPerRadius[data.radius];

			data.countOfElementsAtRadius = offsets.Length;
			data.indexAtRadius = 0;

			for (; data.indexAtRadius < data.countOfElementsAtRadius;)
			{
				Vector2Int posTemp = center + offsets[data.indexAtRadius];
				if (funcValidation(posTemp))
				{
					result = posTemp;
					return true;
				}
				data.indexAtRadius++;
			}
			return false;
		}

		private static void EnsureCacheOffsetPositions(int radius)
		{
			if (!offsetsPerRadius.TryGetValue(radius, out Vector2Int[] offsets))
			{
				if (radius == 0)
				{
					offsetsPerRadius.Add(radius, new Vector2Int[] { Vector2Int.zero });
				}
				else
				{
					offsetsPerRadius.Add(radius, GenerateOffsetsClockwise(radius));
				}
			}
		}
		private static Vector2Int[] GenerateOffsetsClockwise(int radius)
		{
			if (radius == 0) return new Vector2Int[] { Vector2Int.zero };

			int count = radius * 8;

			Vector2Int[] offsets = new Vector2Int[count];

			for (int i = 0; i < count; i++)
			{
				offsets[i] = Vector2Int.zero.GetAtRadiusQuadsLayerPosition(radius, i);
			}

			return offsets;
		}
	}
}

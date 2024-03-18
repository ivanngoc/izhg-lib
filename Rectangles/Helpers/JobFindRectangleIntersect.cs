using IziHardGames.Libs.NonEngine.Math.Trigonometry;
using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Runtime.CompilerServices;

namespace IziHardGames.Libs.NonEngine.Rectangles.Helpers
{
	/// <summary>
	/// Find intersection of rectangle with ray from <see cref="point"/> in <see cref="direction"/>.
	/// If <see cref="direction"/> is intersect with one of corner than there is same result for <see cref="intersect0"/> and <see cref="intersect1"/>.
	/// For avoiding that use <see cref="JoinResultWithApproximation"/>
	/// </summary>
	[Serializable]
	public struct JobFindRectangleIntersect
	{
		// input
		public Rectangle2D rectangle2D;
		public Point2 point;
		public Point2 direction;
		/// <summary>
		/// <see langword="true"/>: if point inside rect intersection with opposite to <see cref="direction"/> direction will be counted<br/>
		/// <see langword="false"/>: Only intersections at forward will be countet
		/// </summary>
		public bool isIgnoreBackSideIntersections;

		// derivatives
		public Point2 lb;
		public Point2 lt;
		public Point2 rt;
		public Point2 rb;

		// result
		public Point2 intersect0;
		public Point2 intersect1;
		public int countOfIntersects;

		public int Count => countOfIntersects;

		public Point2 this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return intersect0;
					case 1: return intersect1;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}

		public JobFindRectangleIntersect(in Rectangle2D rectangle2D, Point2 point, Point2 direction01, bool ignoreBackSideIntersactions) : this()
		{
#if UNITY_EDITOR
			//if ((direction01.magnitude - 1) > float.Epsilon) throw new ArgumentOutOfRangeException($"Direction iz not normilized: {direction01.magnitude}");
			if (direction01 == Point2.zero) throw new ArgumentOutOfRangeException($"Direction iz zero vector");
#endif
			this.rectangle2D = rectangle2D;
			this.point = point;
			this.direction = direction01;
			this.isIgnoreBackSideIntersections = ignoreBackSideIntersactions;

			lb = rectangle2D.LeftBot;
			lt = rectangle2D.LeftTop;
			rt = rectangle2D.RightTop;
			rb = rectangle2D.RightBot;
		}

		public void FindIntersectsWithDuplicatingCheck()
		{
			FindIntersects();
			JoinResultWithApproximation(Point2.kEpsilon);
		}
		public void FindIntersects()
		{
			float dot = Point2.Dot(direction, Point2.up);

			switch (dot)
			{
				case -1:// look down
					{
						if (point.x == rectangle2D.xMin)
						{
							if (point.y >= rectangle2D.yMax)
							{
								countOfIntersects = 1;
								intersect0 = lt;
								return;
							}
							if (point.y >= rectangle2D.yMin)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
							// no intersects
							return;
						}

						if (point.x == rectangle2D.xMax)
						{
							if (point.y >= rectangle2D.yMax)
							{
								countOfIntersects = 1;
								intersect0 = rt;
								return;
							}
							if (point.y >= rectangle2D.yMin)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
							// no intersects
							return;
						}

						if (rectangle2D.xMin < point.x && point.x < rectangle2D.xMax)
						{
							if (point.y >= rectangle2D.yMax)
							{
								DoubleIntersectAlongHeightToDown();
								return;
							}
							if (point.y > rectangle2D.yMin)
							{
								if (isIgnoreBackSideIntersections)
								{
									IntersectAlongHeightToDown();
								}
								else
								{
									DoubleIntersectAlongHeight();
								}
								return;
							}
							if (point.y == rectangle2D.yMin)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						// no intersect
						return;
					}
				case 0: // perpendicular
					{
						HandleHorizontal();
						return;
					}
				case 1: // look up
					{
						if (point.x == rectangle2D.xMin)
						{
							if (point.y <= rectangle2D.yMin)
							{
								countOfIntersects = 1;
								intersect0 = lb;
								return;
							}
							if (point.y <= rectangle2D.yMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
							// no intersect
							return;
						}

						if (point.x == rectangle2D.xMax)
						{
							if (point.y <= rectangle2D.yMin)
							{
								countOfIntersects = 1;
								intersect0 = rb;
								return;
							}
							if (point.y <= rectangle2D.yMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
							// no intersect
							return;
						}

						if (rectangle2D.xMin < point.x && point.x < rectangle2D.xMax)
						{
							if (point.y <= rectangle2D.yMin)
							{
								DoubleIntersectAlongHeightToUp();
								return;
							}
							if (point.y < rectangle2D.yMax)
							{
								if (isIgnoreBackSideIntersections)
								{
									IntersectAlongHeightToUp();
								}
								else
								{
									DoubleIntersectAlongHeight();
								}
								return;
							}
							if (point.y == rectangle2D.yMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						// no intersects
						return;
					}
				default: break;
			}

			// луч под углом (не паралельно оси X или Y)
			if (TryIntersectSide(lb, lt, out Point2 intrSideLeft))
			{
				countOfIntersects = 1;
				intersect0 = intrSideLeft;
			}
			if (TryIntersectSide(lt, rt, out Point2 intrSideTop))
			{
				if (countOfIntersects > 0)
				{
					intersect1 = intrSideTop;
					countOfIntersects = 2;
					return;
				}
				else
				{
					intersect0 = intrSideTop;
					countOfIntersects = 1;
				}
			}
			if (TryIntersectSide(rt, rb, out Point2 intrSideRight))
			{
				if (countOfIntersects > 0)
				{
					intersect1 = intrSideRight;
					countOfIntersects = 2;
					return;
				}
				else
				{
					intersect0 = intrSideRight;
					countOfIntersects = 1;
				}
			}
			if (TryIntersectSide(rb, lb, out Point2 intrSideBot))
			{
				if (countOfIntersects > 0)
				{
					intersect1 = intrSideBot;
					countOfIntersects = 2;
					return;
				}
				else
				{
					intersect0 = intrSideBot;
					countOfIntersects = 1;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Point2 IntersectSide(Point2 a, Point2 b)
		{
			var aToPoint = point - a;
			var aToB = b - a;

			var projectionToPoint = Point2.Project(aToPoint, aToB.normalized);
			var othoFromPointToSide = (a + projectionToPoint) - point;
			float angle = Point2.Angle(othoFromPointToSide, direction);

			return MathForTrigonometry.CalcHypotenuseWithAdjacent(angle, othoFromPointToSide.Magnitude) * direction + point;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryIntersectSide(Point2 a, Point2 b, out Point2 pointOfIntersect)
		{
			var aToPoint = point - a;
			var aToB = b - a;

			var projectionToPoint = Point2.Project(aToPoint, aToB.normalized);
			var othoFromPointToSide = (a + projectionToPoint) - point;
			float angle = Point2.Angle(othoFromPointToSide, direction);

			pointOfIntersect = MathForTrigonometry.CalcHypotenuseWithAdjacent(angle, othoFromPointToSide.Magnitude) * direction + point;

			var aToPointIntersect = pointOfIntersect - a;

			if (aToPointIntersect.sqrMagnitude <= aToB.sqrMagnitude)
			{
				if (Point2.Dot(aToPointIntersect, aToB) > 0)
				{
					if (isIgnoreBackSideIntersections)
					{
						var pointToIntersect = pointOfIntersect - point;

						if (Point2.Dot(direction, pointToIntersect) > 0)
						{
							return true;
						}
					}
					else
					{
						return true;
					}
				}
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void HandleHorizontal()
		{
			float dot = Point2.Dot(direction, Point2.right);

			switch (dot)
			{
				case -1:// right to left
					{
						if (point.y == rectangle2D.yMin)
						{
							if (rectangle2D.xMax <= point.x)
							{
								countOfIntersects = 1;
								intersect0 = rb;
								return;
							}
							if (rectangle2D.xMin <= point.x)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}

						if (point.y == rectangle2D.yMax)
						{
							if (rectangle2D.xMax <= point.x)
							{
								countOfIntersects = 1;
								intersect0 = rt;
								return;
							}
							if (rectangle2D.xMin <= point.x)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}

						if (rectangle2D.yMin < point.y && point.y < rectangle2D.yMax)
						{
							if (rectangle2D.xMax <= point.x)
							{
								DoubleIntersectAlongWidthToLeft();
								return;
							}
							if (rectangle2D.xMin < point.x)
							{
								if (isIgnoreBackSideIntersections)
								{
									IntersectAlongWidthToLeft();
								}
								else
								{
									DoubleIntersectAlongWidth();
								}
								return;
							}
							if (rectangle2D.xMin == point.x)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						// no intersections
						return;
					}
				case 0: throw new ArgumentOutOfRangeException($"Direction might be Vector.Zero. Check direction input");
				case 1: // left to right
					{
						if (point.y == rectangle2D.yMin)
						{
							if (point.x <= rectangle2D.xMin)
							{
								countOfIntersects = 1;
								intersect0 = lb;
								return;
							}
							if (point.x <= rectangle2D.xMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						if (point.y == rectangle2D.yMax)
						{
							if (point.x <= rectangle2D.xMin)
							{
								countOfIntersects = 1;
								intersect0 = lt;
								return;
							}
							if (point.x <= rectangle2D.xMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						if (rectangle2D.yMin < point.y && point.y < rectangle2D.yMax)
						{
							if (point.x <= rectangle2D.xMin)
							{
								DoubleIntersectAlongWidthToRight();
								return;
							}
							if (point.x < rectangle2D.xMax)
							{
								if (isIgnoreBackSideIntersections)
								{
									IntersectAlongWidthToRight();
								}
								else
								{
									DoubleIntersectAlongWidth();
								}
								return;
							}
							if (point.x == rectangle2D.xMax)
							{
								countOfIntersects = 1;
								intersect0 = point;
								return;
							}
						}
						// no intersect
						return;
					}
				default: throw new ArgumentOutOfRangeException($"Code Authoring Error. Dot = [{dot}]. But must be == -1/0/1. Check debugger");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongWidthToRight()
		{
			countOfIntersects = 2;

			var poiintToLeftCorner = lt - point;
			var poiintToRightCorner = rt - point;

			intersect0 = Point2.Project(poiintToLeftCorner, Point2.right) + point;
			intersect1 = Point2.Project(poiintToRightCorner, Point2.right) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void IntersectAlongWidthToRight()
		{
			countOfIntersects = 1;
			var poiintToRightCorner = rt - point;
			intersect0 = Point2.Project(poiintToRightCorner, Point2.right) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongWidth()
		{
			countOfIntersects = 2;

			var poiintToLeftCorner = lt - point;
			intersect0 = Point2.Project(poiintToLeftCorner, Point2.right) + point;

			var poiintToRightCorner = rt - point;
			intersect1 = Point2.Project(poiintToRightCorner, Point2.right) + point;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongWidthToLeft()
		{
			countOfIntersects = 2;

			var poiintToRightCorner = rt - point;
			var poiintToLeftCorner = lt - point;

			intersect0 = Point2.Project(poiintToRightCorner, Point2.right) + point;
			intersect1 = Point2.Project(poiintToLeftCorner, Point2.right) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void IntersectAlongWidthToLeft()
		{
			countOfIntersects = 1;
			var poiintToLeftCorner = lt - point;
			intersect0 = Point2.Project(poiintToLeftCorner, Point2.right) + point;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongHeightToDown()
		{
			countOfIntersects = 2;

			var pointToTopCorner = lt - point;
			var pointToBotCorner = lb - point;

			intersect0 = Point2.Project(pointToTopCorner, Point2.down) + point;
			intersect1 = Point2.Project(pointToBotCorner, Point2.down) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongHeight()
		{
			var pointToBotCorner = lb - point;
			intersect0 = Point2.Project(pointToBotCorner, Point2.down) + point;

			var pointToTopCorner = lt - point;
			intersect1 = Point2.Project(pointToTopCorner, Point2.up) + point;

			countOfIntersects = 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void IntersectAlongHeightToDown()
		{
			countOfIntersects = 1;
			var pointToBotCorner = lb - point;
			intersect0 = Point2.Project(pointToBotCorner, Point2.down) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoubleIntersectAlongHeightToUp()
		{
			countOfIntersects = 2;
			var pointToBotCorner = lb - point;
			var pointToTopCorner = lt - point;

			intersect0 = Point2.Project(pointToBotCorner, Point2.up) + point;
			intersect1 = Point2.Project(pointToTopCorner, Point2.up) + point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void IntersectAlongHeightToUp()
		{
			countOfIntersects = 1;
			var pointToTopCorner = lt - point;
			intersect0 = Point2.Project(pointToTopCorner, Point2.up) + point;
		}

		/// <summary>
		/// Because of algorithm in <see cref="FindIntersects"/> is processing each side individualy there is double result of intersect when ray go through one of corner.
		/// To gain single value with tolerance as distance between intersects that method can be used
		/// </summary>
		/// <param name="distanceBetweenIntersect"></param>
		public void JoinResultWithApproximation(float distanceBetweenIntersect)
		{
			if (countOfIntersects == 2)
			{
				if ((intersect0 - intersect1).sqrMagnitude < (distanceBetweenIntersect * distanceBetweenIntersect))
				{
					countOfIntersects = 1;
				}
			}
		}
		public Point2 GetResultAlongDirection(Point2 direction)
		{
			var pointToInters0 = intersect0 - point;
			var dot0 = Point2.Dot(pointToInters0.normalized, direction);
			if (dot0 >= 0) return intersect0;
			var pointToInters1 = intersect1 - point;
			var dot1 = Point2.Dot(pointToInters1.normalized, direction);
			if (dot1 >= 0) return intersect1;
			throw new ArgumentOutOfRangeException();
		}
#if UNITY_EDITOR
		public static void Test()
		{
			Rectangle2D rect = new Rectangle2D();
			throw new NotImplementedException();
		}
#endif
	}
}
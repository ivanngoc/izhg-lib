using IziHardGames.Libs.NonEngine.Vectors;
using IziHardGames.Libs.NonEngine.Vectors.ExtensionsForDirections;
using System;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.Rectangles
{

	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	public struct Rectangle2D : IEquatable<Rectangle2D>
	{
		[FieldOffset(0)] public float xMin;
		[FieldOffset(4)] public float yMin;

		[FieldOffset(8)] public float xMax;
		[FieldOffset(12)] public float yMax;

		[FieldOffset(16)] public float height;
		[FieldOffset(20)] public float width;

		[FieldOffset(24)] public float widthHalf;
		[FieldOffset(28)] public float heightHalf;

		public Point2 Min => new Point2(xMin, yMin);
		public Point2 Max => new Point2(xMax, yMax);
		public Point2 LeftBot => new Point2(xMin, yMin);
		public Point2 LeftTop => new Point2(xMin, yMax);
		public Point2 RightBot => new Point2(xMax, yMin);
		public Point2 RightTop => new Point2(xMax, yMax);

		public Point2 VectorLeftBotToLeftTop => LeftTop - LeftBot;
		public Point2 VectorLeftTopToLeftBot => LeftBot - LeftTop;
		public Point2 VectorRightBotToRightTop => RightTop - RightBot;
		public Point2 VectorRightTopToRightBot => RightBot - RightTop;

		public Rectangle2D(Rect2D rect2D)
		{
			this.xMin = rect2D.x;
			this.yMin = rect2D.y;

			this.width = rect2D.width;
			this.height = rect2D.height;

			this.xMax = rect2D.x + width;
			this.yMax = rect2D.y + height;

			this.widthHalf = width / 2;
			this.heightHalf = height / 2;
		}
		public Rectangle2D(float xMin, float yMin, float width, float height)
		{
			this.xMin = xMin;
			this.yMin = yMin;

			this.width = width;
			this.height = height;

			this.xMax = xMin + width;
			this.yMax = yMin + height;

			this.widthHalf = width / 2;
			this.heightHalf = height / 2;
		}

		public bool Equals(Rectangle2D other)
		{
			return this == other;
		}
		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(xMin, yMin, height, width);
		}
		public void MoveByLeftTopCornerTo(ref Point3 position)
		{
			xMin = position.x;
			xMax = position.x + width;

			yMin = position.y - height;
			yMax = position.y;
		}
		public void MoveByRightBotCornerTo(ref Point3 position)
		{
			throw new NotImplementedException();
		}
		public void MoveByLeftBotCornerTo(ref Point3 position)
		{
			xMin = position.x;
			yMin = position.y;

			xMax = xMin + width;
			yMax = yMin + height;
		}
		public void Move(ref Point3 scrollOffset)
		{
			xMin += scrollOffset.x;
			yMin += scrollOffset.y;

			xMax += scrollOffset.x;
			yMax += scrollOffset.y;
		}
		public void Shift(Point3 scrollDelta)
		{
			xMin += scrollDelta.x;
			yMin += scrollDelta.y;
			xMax += scrollDelta.x;
			yMax += scrollDelta.y;
		}
		public float GetLengthAlongDirectionAdaptiveVertical(Point2 direction01)
		{
			int direction = direction01.GetDirectionQuarter();

			switch (direction)
			{
				case 0:
					{
						return width;
					}
				case 1:
					{
						var vector = VectorLeftBotToLeftTop;
						return Point2.Project(vector, direction01).Magnitude;
					}
				case 2:
					{
						return height;
					}
				case 3:
					{
						var vector = VectorRightBotToRightTop;
						return Point2.Project(vector, direction01).Magnitude;
					}
				case 4:
					{
						return width;
					}
				case 5:
					{
						var vector = VectorRightTopToRightBot;
						return Point2.Project(vector, direction01).Magnitude;
					}
				case 6:
					{
						return height;
					}
				case 7:
					{
						var vector = VectorLeftTopToLeftBot;
						return Point2.Project(vector, direction01).Magnitude;
					}
				case 8: goto default;
				default: throw new ArgumentOutOfRangeException(direction.ToString());
			}
		}
		public static bool operator ==(Rectangle2D a, Rectangle2D b)
		{
			return a.xMin == b.xMin && a.yMin == b.yMin && a.width == b.width && a.height == b.height;
		}
		public static bool operator !=(Rectangle2D a, Rectangle2D b)
		{
			return a.xMin != b.xMin || a.yMin != b.yMin || a.width != b.width || a.height != b.height;
		}
	}
}